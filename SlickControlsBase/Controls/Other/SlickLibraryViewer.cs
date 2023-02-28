using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class SlickLibraryViewer : SlickControl, IO.IController
	{
		[Category("Action")]
		public event FileOpenedEventHandler FileOpened;

		[Category("Action")]
		public event EventHandler SearchCleared;

		[Category("Behavior")]
		public event EventHandler LoadStarted;

		[Category("Behavior")]
		public event EventHandler LoadEnded;

		private string startingFolder;
		private bool firstLoadFinished;
		private string hoveredAction;
		private string currentSearch;
		private readonly WaitIdentifier searchWait = new WaitIdentifier();
		private readonly TicketBooth ticketBooth = new TicketBooth();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Factory Factory { get; } = new Factory { ProcessingPower = 4 };

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CurrentPath { get; private set; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Func<IOControl, SlickStripItem[]> RightClickContext { get; set; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedPath => FLP_Content.Controls.OfType<IOControl>().FirstOrDefault(x => x.Selected)?.Path;

		[Category("Behavior")]
		public bool FoldersOnly { get; set; }

		[Category("Behavior")]
		public string[] Extensions { get; set; }

		[Category("Behavior")]
		public string[] TopFolders { get; set; } = new string[0];

		[Category("Behavior")]
		public string StartingFolder { get; set; }

		public SlickLibraryViewer()
		{
			InitializeComponent();

			PB_Bar.MouseMove += (s, e) => PB_Bar.Invalidate();
		}

		protected override void UIChanged()
		{
			L_NoResults.Font = UI.Font(9.75F);
			P_Bar.Height = UI.Font(9F).Height + 16;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			new Action(() => setFolder(StartingFolder ?? (TopFolders.Count(Directory.Exists) == 1 ? TopFolders.First(Directory.Exists) : string.Empty))).RunInBackground(50);
		}

		public bool GoBack()
		{
			if (!string.IsNullOrWhiteSpace(CurrentPath) && !TB_Path.Visible)
			{
				if (CurrentPath.Equals(startingFolder, StringComparison.CurrentCultureIgnoreCase))
					setFolder(string.Empty);
				else if (Directory.GetParent(CurrentPath) == null)
					return false;
				else
					setFolder(Directory.GetParent(CurrentPath).FullName);

				return true;
			}

			return false;
		}

		public void Search(string querry)
		{
			if (currentSearch == querry) return;

			currentSearch = querry;
			ticketBooth.GetTicket();

			if (string.IsNullOrWhiteSpace(querry))
			{
				searchWait.Cancel();
				setFolder(CurrentPath, true);
				return;
			}

			StartLoad();

			Factory.Clear();

			if (querry.Length > 1)
			{
				searchWait.Wait(() =>
				{
					handleControls(Enumerable.Where(string.IsNullOrEmpty(CurrentPath) ? TopFolders : new[] { CurrentPath }, folder => Directory.Exists(folder))
						.SelectMany(searchFolder), ticketBooth.GetTicket());

					EndLoad();
				}, 350);
			}
		}

		private IEnumerable<object> searchFolder(string folder)
		{
			var f = new DirectoryInfo(folder);
			var items = new List<object>();
			var error = false;

			try
			{
				foreach (var file in f.EnumerateFiles(FoldersOnly ? "*.lnk" : "*", SearchOption.TopDirectoryOnly))
				{
					if (isValid(file) && file.Name.SearchCheck(currentSearch))
						items.Add(file);
				}

				foreach (var item in f.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
				{
					if (item.Name.SearchCheck(currentSearch))
						items.Add(item);
				}
			}
			catch { error = true; }

			foreach (var item in items)
				yield return item;

			if (!error)
				foreach (var file in f.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
					foreach (var item in searchFolder(file.FullName))
						yield return item;
		}

		private void setFolder(string folder, bool forced = false, bool fromTextBox = false) => new Action(() =>
		{
			if ((CurrentPath != folder && (Directory.Exists(folder) || string.IsNullOrWhiteSpace(folder))) || forced)
			{
				currentSearch = null;
				Factory.Clear();
				ticketBooth.GetTicket();

				if (string.IsNullOrWhiteSpace(CurrentPath) || string.IsNullOrWhiteSpace(folder) || !folder.Contains(startingFolder))
					startingFolder = folder;
				CurrentPath = folder;

				if (TB_Path.Visible && !fromTextBox)
					this.TryInvoke(() => { TB_Path.Text = CurrentPath; TB_Path.SelectionStart = TB_Path.Text.Length; });

				StartLoad();
				var ticket = ticketBooth.GetTicket();

				try
				{
					var content = string.IsNullOrWhiteSpace(folder)
						? TopFolders.Where(x => Directory.Exists(x)).Select(x => (object)new DirectoryInfo(x))
						: new DirectoryInfo(folder).EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
							.Select(f =>
							{
								if (FoldersOnly)
									return f;

								try
								{
									if (f.EnumerateDirectories("*", SearchOption.TopDirectoryOnly).Any()) return f;

									var files = f.GetFiles("*.*", SearchOption.TopDirectoryOnly).Where(isValid).ToArray();

									return files.Length == 1 ? files[0] : f as object;
								}
								catch { return null; }
							})
						.Concat(new DirectoryInfo(folder).EnumerateFiles(FoldersOnly ? "*.lnk" : "*.*", SearchOption.TopDirectoryOnly).Where(isValid))
						.Where(x => x != null);

					handleControls(content, ticket);
				}
				catch { }

				firstLoadFinished = true;
			}

			this.TryInvoke(() => SearchCleared?.Invoke(this, EventArgs.Empty));
		}).RunInBackground();

		private void handleControls(IEnumerable<object> content, TicketBooth.Ticket ticket)
		{
			var toBeAdded = new List<object>();
			var waitId = new WaitIdentifier();

			try
			{
				foreach (var item in content)
				{
					if (!ticketBooth.IsLast(ticket)) return;

					if (item != null)
						toBeAdded.Add(item);

					waitId.Wait(handle, 100);
				}

				waitId.Cancel();
				handle();
			}
			catch { }

			EndLoad();

			void handle()
			{
				if (!ticketBooth.IsLast(ticket)) return;
				var objs = toBeAdded.ToArray();
				toBeAdded.Clear();

				this.TryInvoke(() =>
				{
					if (firstLoadFinished)
						FLP_Content.SuspendDrawing();

					var cntrls = new IOControl[objs.Length];

					for (int i = 0; i < objs.Length; i++)
					{
						try
						{
							if (!ticketBooth.IsLast(ticket)) return;
							var c = new IOControl(objs[i], this);

							if ((c.FileObject != null || c.FolderObject != null) && !c.IsDisposed && ticketBooth.IsLast(ticket))
								FLP_Content.Controls.Add(c);

							if (i % 10 == 0)
								Application.DoEvents();
						}
						catch { }
					}

					FLP_Content.OrderBy(x => (x as IOControl).FolderObject == null ? "1" : "0" + x.Text, false);

					if (firstLoadFinished)
						FLP_Content.ResumeDrawing();
				});
			}
		}

		private void EndLoad() => this.TryInvoke(() =>
		{
			PB_Loader.Visible = false;
			L_NoResults.Visible = FLP_Content.Controls.Count == 0;

			LoadEnded?.Invoke(this, EventArgs.Empty);
		});

		private void StartLoad() => this.TryInvoke(() =>
		{
			PB_Bar.Invalidate();

			if (firstLoadFinished)
				FLP_Content.SuspendDrawing();

			FLP_Content.Controls.Clear(true);

			if (firstLoadFinished)
				FLP_Content.ResumeDrawing();

			PB_Loader.Visible = true;
			L_NoResults.Visible = false;

			LoadStarted?.Invoke(this, EventArgs.Empty);
		});

		public void folderOpened(DirectoryInfo directory) => setFolder(directory?.FullName);

		public void fileOpened(FileInfo file) => FileOpened?.Invoke(this, new FileOpenedEventArgs(file));

		private bool isValid(FileInfo file) => FoldersOnly ? file.FullName.IsFolder() : (Extensions?.Length ?? 0) == 0 || Extensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase));

		protected override void DesignChanged(FormDesign design)
		{
			P_Spacer.BackColor = design.AccentColor;
			PB_Bar.BackColor = design.AccentBackColor;
			L_NoResults.ForeColor = design.LabelColor;
		}

		private void P_Bar_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(FormDesign.Design.BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			hoveredAction = null;

			var mousePos = PB_Bar.PointToClient(Cursor.Position);
			var w = 3;
			var arrow = Properties.Resources.Tiny_MoreThan.Color(FormDesign.Design.ForeColor);

			using (var font = UI.Font(9F))
			using (var brush = Gradient(FormDesign.Design.ForeColor))
			{
				var bnds = SizeF.Empty;

				drawItem(null, string.Empty);

				if (!string.IsNullOrWhiteSpace(startingFolder))
				{
					drawArrow();
					drawItem(Path.GetFileName(startingFolder).FormatWords().IfEmpty(startingFolder), startingFolder);

					var currentFolder = startingFolder;
					var items = CurrentPath.Remove(startingFolder).Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

					if (items.Length > 4)
						items = items.Take(1).Concat(new[] { string.Empty }).Concat(items.TakeLast(3)).ToArray();

					foreach (var folder in items)
					{
						currentFolder = Path.Combine(currentFolder, folder);
						drawArrow();
						if (!string.IsNullOrWhiteSpace(folder))
							drawItem(folder.FormatWords(), currentFolder);
					}
				}

				void drawItem(string text, string action)
				{
					if (text != null)
						bnds = e.Graphics.MeasureString(text, font);
					else
						bnds = new SizeF(font.GetHeight(), font.GetHeight());

					var rect = new Rectangle(w - 3, (int)(PB_Bar.Height - bnds.Height - 6) / 2, (int)bnds.Width + 6, (int)bnds.Height + 6);

					if (rect.Contains(mousePos))
					{
						e.Graphics.FillRoundedRectangle(Gradient(PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentBackColor), rect, 5);
						hoveredAction = action;
						Cursor = Cursors.Hand;
					}

					if (text != null)
						e.Graphics.DrawString(text, font, rect.Contains(mousePos) && PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? Gradient(FormDesign.Design.ActiveForeColor) : brush, w, (int)(PB_Bar.Height - bnds.Height) / 2);
					else
						e.Graphics.DrawImage(Properties.Resources.Tiny_Home.Color(rect.Contains(mousePos) && PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor), new Rectangle(rect.Center(new Size(16, 16)), new Size(16, 16)));

					w += (int)bnds.Width + 5;
				}

				void drawArrow()
				{
					e.Graphics.DrawImage(arrow, w, (PB_Bar.Height - arrow.Height) / 2);
					w += 21;
				}
			}

			if (hoveredAction == null)
				Cursor = PB_Bar.HoverState.HasFlag(HoverState.Hovered) ? Cursors.IBeam : Cursors.Default;
		}

		private void P_Bar_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (hoveredAction != null)
					setFolder(hoveredAction);
				else
				{
					PB_Bar.Visible = false;
					TB_Path.Text = CurrentPath;
					TB_Path.Visible = true;
					BeginInvoke(new Action(() => { TB_Path.Focus(); TB_Path.SelectionStart = TB_Path.Text.Length; TB_Path.Top = P_Bar.Height - TB_Path.Height + 1; }));
				}
			}
		}

		private void FLP_Content_Resize(object sender, EventArgs e) => FLP_Content.MaximumSize = new Size(Width, int.MaxValue);

		private void TB_Path_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13 || e.KeyChar == 27)
			{
				PB_Bar.Visible = true;
				TB_Path.Visible = false;
				e.Handled = true;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				var io = FLP_Content.Controls.OfType<IOControl>().FirstOrDefault(x => x.Selected);

				if (io != null)
				{
					if (io.FileObject != null)
						fileOpened(io.FileObject);
					else if (io.FolderObject != null)
						folderOpened(io.FolderObject);

					return true;
				}
			}

			if (keyData == Keys.Right || keyData == Keys.Left)
			{
				var inc = keyData == Keys.Right ? 1 : -1;
				var io = FLP_Content.Controls.OfType<IOControl>().FirstOrDefault(x => x.Selected);
				var ind = (io == null ? -1 : FLP_Content.Controls.IndexOf(io)) + inc;

				if (ind < 0 || FLP_Content.Controls.Count == 1)
					ind = FLP_Content.Controls.Count - 1;
				else if (ind >= FLP_Content.Controls.Count)
					ind = 0;

				if (io != null)
				{
					io.Selected = false;
					io.Invalidate();
				}

				if (ind < FLP_Content.Controls.Count)
				{
					io = FLP_Content.Controls[ind] as IOControl;

					io.Selected = true;
					io.Invalidate();
					if (slickScroll1.Active)
						slickScroll1.ScrollTo(io, 3);
				}
			}

			if (keyData == Keys.Up || keyData == Keys.Down)
			{
				var inc = keyData == Keys.Down ? 1 : -1;
				var io = FLP_Content.Controls.OfType<IOControl>().FirstOrDefault(x => x.Selected);
				var ind = io == null ? (keyData == Keys.Up ? FLP_Content.Controls.Count - 1 : 0) : FLP_Content.Controls.IndexOf(io);

				if (io != null)
				{
					while (true)
					{
						ind += inc;

						if (!ind.IsWithin(-1, FLP_Content.Controls.Count))
						{
							if (ind == FLP_Content.Controls.Count && keyData == Keys.Down)
								ind = 0;
							else if (ind == -1 && keyData == Keys.Up)
								ind = FLP_Content.Controls.Count - 1;
							else break;
						}

						if (FLP_Content.Controls[ind].Left == io.Left)
							break;
					}
				}

				if (ind < 0 || FLP_Content.Controls.Count == 1)
					ind = FLP_Content.Controls.Count - 1;

				if (io != null)
				{
					io.Selected = false;
					io.Invalidate();
				}

				if (ind < FLP_Content.Controls.Count)
				{
					io = FLP_Content.Controls[ind] as IOControl;

					io.Selected = true;
					io.Invalidate();
					if (slickScroll1.Active)
						slickScroll1.ScrollTo(io, 3);
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void TB_Path_Leave(object sender, EventArgs e)
		{
			PB_Bar.Visible = true;
			TB_Path.Visible = false;
		}

		private void TB_Path_TextChanged(object sender, EventArgs e)
		{
			if (TB_Path.Visible)
				setFolder(TB_Path.Text, false, true);
		}

		private void Generic_Click(object sender, EventArgs e)
		{
			Focus();
		}
	}
}