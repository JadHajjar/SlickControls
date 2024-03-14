using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SlickControls;

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

	[Category("Behavior")]
	public event EventHandler CurrentPathChanged;

	private string startingFolder;
	private string hoveredAction;
	private string currentSearch;
	private string _currentPath;
	private readonly WaitIdentifier searchWait = new();
	private readonly TicketBooth ticketBooth = new();
	internal List<IOSelectionDialog.CustomFile> CustomFiles;

	public event Func<FileInfo, bool> ValidFile;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<ExtensionClass.action> Factory { get; } = [];

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string CurrentPath
	{
		get => _currentPath; private set
		{
			_currentPath = value;
			CurrentPathChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Func<IOControl, SlickStripItem[]> RightClickContext { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedPath => ioList.SelectedItem?.Path;

	[Category("Behavior")]
	public bool FoldersOnly { get; set; }

	[Category("Behavior")]
	public string[] Extensions { get; set; }

	[Category("Behavior")]
	public string[] TopFolders { get; set; } = new string[0];

	[Category("Behavior")]
	public string StartingFolder { get; set; }

	internal IoSortingOption Sorting { get => ioList.IoSortingOption; set => ioList.IoSortingOption = value; }
	internal bool SortDesc { get => ioList.SortDesc; set => ioList.SortDesc = value; }

	public SlickLibraryViewer()
	{
		InitializeComponent();

		ioList.CanDrawItem += IoList_CanDrawItem;
		ioList.Controller = this;

		PB_Bar.MouseMove += (s, e) => PB_Bar.Invalidate();
	}

	protected override void UIChanged()
	{
		I_Back.Padding = PB_Bar.Padding = UI.Scale(new Padding(5), UI.FontScale);
		PB_Bar.Height = (int)FontMeasuring.Measure(" ", UI.Font(9F)).Height + PB_Bar.Padding.Vertical;
		slickSpacer1.Height = (int)(1.5 * UI.FontScale);
		slickSpacer1.Padding = UI.Scale(new Padding(15, 0, 15, 0), UI.FontScale);
		TB_Path.Width = PB_Bar.Width = (int)(450 * UI.UIScale);
		tableLayoutPanel2.ColumnStyles[1].Width = PB_Bar.Height;
		tableLayoutPanel2.ColumnStyles[3].Width = PB_Bar.Height;
		PB_Loader.Size = new Size(PB_Bar.Height, PB_Bar.Height);
		I_Back.Size = UI.Scale(new Size(24, 24), UI.FontScale);
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();
		new BackgroundAction(() => setFolder(StartingFolder ?? (TopFolders.Count(Directory.Exists) == 1 ? TopFolders.First(Directory.Exists) : string.Empty))).RunIn(50);
	}

	public bool GoBack()
	{
		if (!string.IsNullOrWhiteSpace(CurrentPath) && !TB_Path.Visible)
		{
			if (CurrentPath.Equals(startingFolder, StringComparison.CurrentCultureIgnoreCase))
			{
				setFolder(string.Empty);
			}
			else if (Directory.GetParent(CurrentPath) == null)
			{
				return false;
			}
			else
			{
				setFolder(Directory.GetParent(CurrentPath).FullName);
			}

			return true;
		}

		return false;
	}

	public void Search(string query)
	{
		if (currentSearch == query)
		{
			return;
		}

		currentSearch = query;

		ioList.FilterChanged();
	}

	private void IoList_CanDrawItem(object sender, CanDrawItemEventArgs<IOControl> e)
	{
		if (string.IsNullOrWhiteSpace(currentSearch))
		{
			return;
		}

		if (currentSearch.Contains("*"))
		{
			e.DoNotDraw = !Regex.IsMatch(e.Item.Name, Regex.Escape(currentSearch).Replace("\\*", "."), RegexOptions.IgnoreCase);
		}
		else
		{
			e.DoNotDraw = !currentSearch.SearchCheck(e.Item.Name);
		}
	}

	internal void setFolder(string folder, bool forced = false, bool fromTextBox = false)
	{
		new BackgroundAction("Loading library folder", () =>
		{
			if ((CurrentPath != folder && (folder == IOSelectionDialog.CustomDirectory || Directory.Exists(folder) || string.IsNullOrWhiteSpace(folder))) || forced)
			{
				currentSearch = null;
				Factory.Clear();
				ticketBooth.GetTicket();

				if (string.IsNullOrWhiteSpace(CurrentPath) || string.IsNullOrWhiteSpace(folder) || !folder.Contains(startingFolder))
				{
					startingFolder = folder;
				}

				CurrentPath = folder;

				if (TB_Path.Visible && !fromTextBox)
				{
					this.TryInvoke(() =>
					{
						TB_Path.Text = CurrentPath;
						TB_Path.SelectionStart = TB_Path.Text.Length;
					});
				}

				StartLoad();
				var ticket = ticketBooth.GetTicket();

				try
				{
					IEnumerable<object> content;

					if (folder == IOSelectionDialog.CustomDirectory)
					{
						handleControls(CustomFiles, ticket);

						return;
					}
					else if (!Directory.Exists(folder))
					{
						content = TopFolders.Where(x => Directory.Exists(x)).Select(x => (object)new DirectoryInfo(x));
					}
					else
					{
						content = new DirectoryInfo(folder).EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
							.Select(f =>
							{
								if (FoldersOnly)
								{
									return f;
								}

								try
								{
									if (f.EnumerateDirectories("*", SearchOption.TopDirectoryOnly).Any())
									{
										return f;
									}

									var files = f.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Where(isValid).ToList();

									return files.Count == 1 ? files[0] : f as object;
								}
								catch
								{
									return null;
								}
							})
						.Concat(new DirectoryInfo(folder).EnumerateFiles(FoldersOnly ? "*.lnk" : "*.*", SearchOption.TopDirectoryOnly).Where(isValid))
						.Where(x => x != null);
					}

					handleControls(content, ticket);
				}
				catch { }
			}

			this.TryInvoke(() => SearchCleared?.Invoke(this, EventArgs.Empty));
		}).Run();
	}

	private void handleControls(IEnumerable<object> content, TicketBooth.Ticket ticket)
	{
		var toBeAdded = new List<IOControl>();

		try
		{
			foreach (var item in content)
			{
				if (!ticketBooth.IsLast(ticket))
				{
					return;
				}

				var c = new IOControl(item, this, out var valid);

				if (!valid)
				{
					continue;
				}

				toBeAdded.Add(c);

				if (toBeAdded.Count == 200)
				{
					ioList.AddRange(toBeAdded);
					toBeAdded.Clear();
				}
			}

			ioList.AddRange(toBeAdded);
		}
		catch { }

		EndLoad();
	}

	private void handleControls(List<IOSelectionDialog.CustomFile> content, TicketBooth.Ticket ticket)
	{
		var toBeAdded = new List<IOControl>();

		try
		{
			foreach (var item in content)
			{
				if (!ticketBooth.IsLast(ticket))
				{
					return;
				}

				var c = new IOControl(new FileInfo(item.Path), this, out var valid, item.Icon)
				{
					Text = item.Name
				};

				if (!valid)
				{
					continue;
				}

				toBeAdded.Add(c);

				if (toBeAdded.Count == 200)
				{
					ioList.AddRange(toBeAdded);
					toBeAdded.Clear();
				}
			}

			ioList.AddRange(toBeAdded);
		}
		catch { }

		EndLoad();
	}

	private void EndLoad()
	{
		this.TryInvoke(() =>
		{
			PB_Loader.Visible = false;

			LoadEnded?.Invoke(this, EventArgs.Empty);
		});

		Parallelism.ForEach(Factory);

		ioList.Invalidate();
	}

	private void StartLoad()
	{
		PB_Bar.Invalidate();

		ioList.Clear();

		this.TryInvoke(() =>
		{
			PB_Loader.Visible = true;

			LoadStarted?.Invoke(this, EventArgs.Empty);
		});
	}

	public void folderOpened(DirectoryInfo directory)
	{
		setFolder(directory?.FullName);
	}

	public void fileOpened(FileInfo file)
	{
		FileOpened?.Invoke(this, new FileOpenedEventArgs(file));
	}

	private bool isValid(FileInfo file)
	{
		if (FoldersOnly)
		{
			return file.FullName.IsFolder();
		}

		if ((Extensions?.Length ?? 0) != 0 && !Extensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
		{
			return false;
		}

		return ValidFile?.Invoke(file) ?? true;
	}

	protected override void DesignChanged(FormDesign design)
	{
		//PB_Bar.BackColor = design.AccentBackColor;
	}

	private void P_Bar_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);
		hoveredAction = null;

		var mousePos = PB_Bar.PointToClient(Cursor.Position);
		var w = PB_Bar.Padding.Left;
		var validRect = PB_Bar.ClientRectangle.Pad(PB_Bar.Padding);

		using (var arrow = IconManager.GetIcon("ArrowRight").Color(FormDesign.Design.ForeColor))
		using (var font = UI.Font(9F))
		using (var brush = new SolidBrush(FormDesign.Design.ForeColor))
		{
			var bnds = SizeF.Empty;

			drawItem(null, string.Empty);

			if (!string.IsNullOrWhiteSpace(startingFolder) && startingFolder != IOSelectionDialog.CustomDirectory)
			{
				drawArrow();

				drawItem(Path.GetFileName(startingFolder).FormatWords().IfEmpty(startingFolder), startingFolder);

				var currentFolder = startingFolder;
				var items = CurrentPath.Remove(startingFolder).Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

				if (items.Length > 4)
				{
					items = items.Take(1).Concat(new[] { string.Empty }).Concat(items.TakeLast(3)).ToArray();
				}

				foreach (var folder in items)
				{
					currentFolder = Path.Combine(currentFolder, folder);
					drawArrow();
					if (!string.IsNullOrWhiteSpace(folder))
					{
						drawItem(folder.FormatWords(), currentFolder);
					}
				}
			}

			void drawItem(string text, string action)
			{
				if (text != null)
				{
					bnds = e.Graphics.Measure(text, font);
				}
				else
				{
					bnds = new SizeF(validRect.Height, validRect.Height);
				}

				var rect = new Rectangle(w, validRect.Y, 0, validRect.Height).Align(Size.Ceiling(bnds), ContentAlignment.MiddleLeft);

				if (rect.Contains(mousePos))
				{
					var i = (int)(3 * UI.FontScale);
					using (var backBrush = Gradient(PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : Color.FromArgb(120, FormDesign.Design.AccentColor)))
					{
						e.Graphics.FillRoundedRectangle(backBrush, rect.Pad(-i), i);
					}

					hoveredAction = action;
					Cursor = Cursors.Hand;
				}

				if (text != null)
				{
					e.Graphics.DrawString(text, font, rect.Contains(mousePos) && PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? Gradient(FormDesign.Design.ActiveForeColor) : brush, rect);
				}
				else
				{
					using var homeIcon = IconManager.GetIcon("Home").Color(rect.Contains(mousePos) && PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor);
					e.Graphics.DrawImage(homeIcon, rect.CenterR(homeIcon.Size));
				}

				w += rect.Width + PB_Bar.Padding.Horizontal;
			}

			void drawArrow()
			{
				var rect = new Rectangle(w, validRect.Y, 0, validRect.Height).Align(arrow.Size, ContentAlignment.MiddleLeft);
				e.Graphics.DrawImage(arrow, rect);
				w += arrow.Width + PB_Bar.Padding.Horizontal;
			}
		}

		if (hoveredAction == null)
		{
			Cursor = PB_Bar.HoverState.HasFlag(HoverState.Hovered) ? Cursors.IBeam : Cursors.Default;
		}
	}

	private void P_Bar_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			if (hoveredAction != null)
			{
				setFolder(hoveredAction);
			}
			else
			{
				PB_Bar.Visible = false;
				TB_Path.Text = CurrentPath;
				TB_Path.Visible = true;
				BeginInvoke(new Action(() =>
				{
					TB_Path.Focus();
					TB_Path.SelectionStart = TB_Path.Text.Length;
					TB_Path.Top = PB_Bar.Height - TB_Path.Height + 1;
				}));
			}
		}
	}

	private void TB_Path_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar is (char)13 or (char)27)
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
			var io = ioList.SelectedItem;

			if (io != null)
			{
				if (io.FileObject != null)
				{
					fileOpened(io.FileObject);
				}
				else if (io.FolderObject != null)
				{
					folderOpened(io.FolderObject);
				}

				return true;
			}
		}

		if (keyData is Keys.Right or Keys.Left)
		{
			var inc = keyData == Keys.Right ? 1 : -1;
			var io = ioList.SelectedItem;
			var list = ioList.SafeGetItems();
			var ind = (io == null ? -1 : list.FindIndex(x => x.Item == io)) + inc;

			if (ind < 0 || list.Count == 1)
			{
				ind = list.Count - 1;
			}
			else if (ind >= list.Count)
			{
				ind = 0;
			}

			if (ind < list.Count)
			{
				io = list[ind].Item;

				ioList.SelectedItem = io;
				ioList.Invalidate();
				ioList.ScrollTo(io);
			}
		}

		if (keyData is Keys.Up or Keys.Down)
		{
			var inc = keyData == Keys.Down ? 1 : -1;
			var io = ioList.SelectedItem;
			var list = ioList.SafeGetItems();
			var ind = (io == null ? -1 : list.FindIndex(x => x.Item == io)) + inc;

			if (io != null)
			{
				while (true)
				{
					ind += inc;

					if (!ind.IsWithin(-1, list.Count))
					{
						if (ind == list.Count && keyData == Keys.Down)
						{
							ind = 0;
						}
						else if (ind == -1 && keyData == Keys.Up)
						{
							ind = list.Count - 1;
						}
						else
						{
							break;
						}
					}

					//if (list[ind].Left == io.Left)
					//	break;
				}
			}

			if (ind < 0 || list.Count == 1)
			{
				ind = list.Count - 1;
			}

			if (ind < list.Count)
			{
				io = list[ind].Item;

				ioList.SelectedItem = io;
				ioList.Invalidate();
				ioList.ScrollTo(io);
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
		{
			setFolder(TB_Path.Text, false, true);
		}
	}

	private void Generic_Click(object sender, EventArgs e)
	{
		Focus();
	}

	internal void SortingChanged()
	{
		ioList.SortingChanged();
	}

	private void I_Back_Click(object sender, EventArgs e)
	{
		GoBack();
	}
}