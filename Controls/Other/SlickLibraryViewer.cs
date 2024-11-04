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
	private string hoveredItem;
	private Action<string> hoveredAction;
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
		PB_Bar.Padding = UI.Scale(new Padding(5));
		PB_Bar.Height = UI.Scale(30);
		slickSpacer1.Height = (int)(1.5 * UI.FontScale);
		slickSpacer1.Padding = UI.Scale(new Padding(15, 0, 15, 0));
		TB_Path.Width = PB_Bar.Width = (int)(450 * UI.UIScale);
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		new BackgroundAction(() => SetFolder(StartingFolder ?? (TopFolders.Count(Directory.Exists) == 1 ? TopFolders.First(Directory.Exists) : string.Empty))).RunIn(50);
	}

	public bool GoBack()
	{
		if (!string.IsNullOrWhiteSpace(CurrentPath) && !TB_Path.Visible)
		{
			if (CurrentPath.Equals(startingFolder, StringComparison.CurrentCultureIgnoreCase))
			{
				SetFolder(string.Empty);
			}
			else if (Directory.GetParent(CurrentPath) == null)
			{
				return false;
			}
			else
			{
				SetFolder(Directory.GetParent(CurrentPath).FullName);
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

	private void GoBack(string _)
	{
		GoBack();
	}

	private void AddFolder(string _)
	{
		var result = MessagePrompt.ShowInput(LocaleSlickUI.TypeFolderName, inputValidation: x => x.EscapeFileName() == x);

		if (result.DialogResult == DialogResult.OK && !string.IsNullOrWhiteSpace(result.Input) && !string.IsNullOrWhiteSpace(CurrentPath))
		{
			Directory.CreateDirectory(CrossIO.Combine(CurrentPath, result.Input));

			RefreshList();
		}
	}

	private void ShowTextbox()
	{
		PB_Bar.Visible = false;
		TB_Path.Text = CurrentPath;
		TB_Path.Visible = true;
		TB_Path.ImageName = null;
		BeginInvoke(new Action(() =>
		{
			TB_Path.Focus();
			TB_Path.SelectionStart = TB_Path.Text.Length;
			TB_Path.Top = PB_Bar.Height - TB_Path.Height + 1;
		}));
	}

	private void SetFolder(string folder)
	{
		SetFolder(folder, false, false);
	}

	private void RefreshList(string _)
	{
		RefreshList();
	}

	private void RefreshList()
	{
		SetFolder(CurrentPath, true);
	}

	internal void SetFolder(string folder, bool forced = false, bool fromTextBox = false)
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
			PB_Bar.Loading = false;

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
			PB_Bar.Loading = true;

			LoadStarted?.Invoke(this, EventArgs.Empty);
		});
	}

	public void folderOpened(DirectoryInfo directory)
	{
		SetFolder(directory?.FullName);
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

	private void P_Bar_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.SetUp(PB_Bar.BackColor);
		hoveredAction = null;

		var mousePos = PB_Bar.PointToClient(Cursor.Position);
		var w = PB_Bar.Padding.Left;
		var validRect = PB_Bar.ClientRectangle.Pad(PB_Bar.Padding);
		var canGoBack = !string.IsNullOrWhiteSpace(CurrentPath) && Directory.GetParent(CurrentPath) != null;

		using var home = IconManager.GetIcon("Home", validRect.Height - UI.Scale(3));
		using var add = IconManager.GetIcon("Add", validRect.Height - UI.Scale(3));
		using var arrowLeft = IconManager.GetIcon("ArrowLeft", validRect.Height - UI.Scale(3));
		using var arrowRight = IconManager.GetIcon("ArrowRight", validRect.Height - UI.Scale(3)).Color(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor));
		using var font = UI.Font(7.5F);
		using var brush = new SolidBrush(FormDesign.Design.ForeColor);

		var bnds = SizeF.Empty;

		drawItem(null, GoBack, null, arrowLeft);
		drawItem(null, SetFolder, string.Empty, home);

		if (!string.IsNullOrWhiteSpace(startingFolder) && startingFolder != IOSelectionDialog.CustomDirectory)
		{
			drawArrow();

			drawItem(Path.GetFileName(startingFolder).FormatWords().IfEmpty(startingFolder), SetFolder, startingFolder, null);

			var currentFolder = startingFolder;
			var items = new List<string>() { startingFolder };

			foreach (var item in CurrentPath.Remove(startingFolder).Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
			{
				items.Add(CrossIO.Combine(items[items.Count - 1], item));
			}

			items.RemoveAt(0);

			var dotted = items.Count >= 4;

			if (dotted)
			{
				items = [items[0], .. items.TakeLast(3)];
			}

			for (var i = 0; i < items.Count; i++)
			{
				drawArrow();

				if (dotted && i == 1)
				{
					drawItem(". . .", SetFolder, items[i], null);
				}
				else
				{
					drawItem(Path.GetFileName(items[i]), SetFolder, items[i], null);
				}
			}
		}

		w = validRect.Right - validRect.Height;

		if (PB_Bar.Loading)
		{
			var rect = new Rectangle(w, validRect.Y, 0, validRect.Height).Align(Size.Ceiling(new SizeF(validRect.Height, validRect.Height)), ContentAlignment.MiddleLeft);

			PB_Bar.DrawLoader(e.Graphics, rect.CenterR(add.Size));
		}
		else
		{
			using var refresh = IconManager.GetIcon("Refresh", validRect.Height - UI.Scale(3));
			drawItem(null, RefreshList, null, refresh);
		}

		w = validRect.Right - validRect.Height * 2 - PB_Bar.Padding.Horizontal;

		drawItem(null, AddFolder, null, add);

		void drawItem(string text, Action<string> action, string actionItem, Bitmap image)
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
				var i = UI.Scale(3);
				using (var backBrush = Gradient(PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : Color.FromArgb(120, FormDesign.Design.AccentColor)))
				{
					e.Graphics.FillRoundedRectangle(backBrush, rect.Pad(-i), i);
				}

				hoveredAction = action;
				hoveredItem = actionItem;
				Cursor = Cursors.Hand;
			}

			if (text != null)
			{
				e.Graphics.DrawString(text, font, rect.Contains(mousePos) && PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? Gradient(FormDesign.Design.ActiveForeColor) : brush, rect);
			}
			else
			{
				image.Color(rect.Contains(mousePos) && PB_Bar.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor);

				e.Graphics.DrawImage(image, rect.CenterR(image.Size));
			}

			w += rect.Width + PB_Bar.Padding.Horizontal;
		}

		void drawArrow()
		{
			var rect = new Rectangle(w, validRect.Y, 0, validRect.Height).Align(arrowRight.Size, ContentAlignment.MiddleLeft);
			e.Graphics.DrawImage(arrowRight, rect);
			w += arrowRight.Width + PB_Bar.Padding.Horizontal;
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
			if (hoveredAction is null)
			{
				ShowTextbox();
			}
			else
			{
				hoveredAction?.Invoke(hoveredItem);
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
			SetFolder(TB_Path.Text, false, true);
		}
	}

	internal void SortingChanged()
	{
		ioList.SortingChanged();
	}
}