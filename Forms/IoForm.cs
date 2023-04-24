using Extensions;

using SlickControls.Controls.Other;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Controls;
using System.Windows.Forms;

namespace SlickControls
{
	internal partial class IoForm : BasePanelForm
	{
		protected readonly bool folderSelection;
		protected readonly IOSelectionDialog _dialog;
		protected string selectedPath;
		protected static string lastPath;

		public string CurrentPath => libraryViewer.CurrentPath;
		public string SelectedPath => selectedPath;

		internal IoForm(bool folder, IOSelectionDialog dialog)
		{
			InitializeComponent();

			TLP_Main.Parent = base_P_PanelContent;
			base_P_Icon.Visible = false;

			folderSelection = folder;
			_dialog = dialog;
			L_Title.Text = dialog.Title ?? LocaleHelper.GetGlobalText(folder ? "Select a folder" : "Select a file");

			SlickTip.SetTo(B_GridView, "Switch to Grid-View");
			SlickTip.SetTo(B_ListView, "Switch to List-View");

			L_Title.MouseDown += Form_MouseDown;
			TLP_Main.MouseDown += Form_MouseDown;

			libraryViewer.ValidFile += dialog.Filter;

			if (dialog.PreserveLastPath)
			{
				libraryViewer.StartingFolder = lastPath ?? dialog.StartingFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
			else
			{
				libraryViewer.StartingFolder = dialog.StartingFolder ?? lastPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}

			libraryViewer.FoldersOnly = folder;
			libraryViewer.Extensions = dialog.ValidExtensions;
			libraryViewer.CustomFiles = dialog.CustomFiles;
			libraryViewer.TopFolders = DriveInfo.GetDrives().Select(x => x.RootDirectory.FullName).ToArray();
			libraryViewer.SearchCleared += (s, e) => TB_Search.Text = string.Empty;
			libraryViewer.FileOpened += (s, e) =>
			{
				selectedPath = e.File.FullName;
				DialogResult = DialogResult.OK;
				Close();
			};

			add(string.Empty, "This PC", "I_PC");

			base_P_Tabs.Add(PanelTab.Separator());

			if (dialog.PinnedFolders?.Any() ?? false)
			{
				base_P_Tabs.Add(PanelTab.GroupName("Pinned Folders"));

				foreach (var item in dialog.PinnedFolders)
				{
					add(item.Value, item.Key, "I_Folder");
				}

				base_P_Tabs.Add(PanelTab.Separator());
			}

			base_P_Tabs.Add(PanelTab.GroupName("Common Folders"));

			foreach (var item in new[]
			{
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
				Environment.GetFolderPath(Environment.SpecialFolder.Recent),
			})
			{
				add(item, Path.GetFileName(item).IfEmpty(item), "I_UserFolder");
			}

			base_P_Tabs.Add(PanelTab.Separator());

			base_P_Tabs.Add(PanelTab.GroupName("Drives"));

			foreach (var item in libraryViewer.TopFolders)
			{
				add(item, item, "I_Drive");
			}

			void add(string path, string name, string icon)
			{
				var item = new PanelItem
				{
					IconName = new DynamicIcon(icon),
					Text = name,
					Data = path
				};

				item.OnClick += Item_OnClick;

				base_P_Tabs.Add(new PanelTab(item));
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			TB_Search.Focus();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.F))
				TB_Search.Focus();

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			roundedPanel1.BackColor = design.AccentBackColor;
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			tableLayoutPanel1.Height = 0;
			roundedPanel1.Margin = UI.Scale(new Padding(10, 10, 10, 5), UI.FontScale);
			B_GridView.Margin = UI.Scale(new Padding(0, 5, 10, 10), UI.FontScale);
			B_Cancel.Margin = B_OK.Margin = B_ListView.Margin = UI.Scale(new Padding(10, 5, 10, 10), UI.FontScale);
			roundedPanel1.Padding = UI.Scale(new Padding(10, 5, 5, 5), UI.FontScale);
			tableLayoutPanel1.Margin = TB_Search.Margin = UI.Scale(new Padding(10, 12, 5, 0), UI.FontScale);
			L_Title.Margin = UI.Scale(new Padding(10, 10, 5, 0), UI.FontScale);
			L_Title.Font = UI.Font(8.25F, FontStyle.Bold);
			ioSortDropdown.Width = (int)(125 * UI.FontScale);
			TB_Search.Width = (int)(200 * UI.FontScale);
			I_SortDirection.Height = ioSortDropdown.Height = 0;
		}

		private void Item_OnClick(object sender, MouseEventArgs e)
		{
			var pi = sender as PanelItem;

			if (!pi.Selected)
			{
				libraryViewer.setFolder(!string.IsNullOrWhiteSpace(pi.Data as string) ? pi.Data as string : null);
			}
		}

		private void B_Cancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void B_OK_Click(object sender, EventArgs e)
		{
			if ((folderSelection && (Directory.Exists(libraryViewer.SelectedPath) || Directory.Exists(libraryViewer.CurrentPath))) || (!folderSelection && !string.IsNullOrWhiteSpace(libraryViewer.SelectedPath) && File.Exists(libraryViewer.SelectedPath)))
			{
				selectedPath = folderSelection && !Directory.Exists(libraryViewer.SelectedPath) ? libraryViewer.CurrentPath : libraryViewer.SelectedPath;
				lastPath = folderSelection ? selectedPath : Directory.GetParent(selectedPath).FullName;
				DialogResult = DialogResult.OK;
				Close();
			}
			else
			{
				SystemSounds.Asterisk.Play();
			}
		}

		private void TB_Search_TextChanged(object sender, EventArgs e)
		{
			libraryViewer.Search(TB_Search.Text);
		}

		protected override bool HandleWndProc(ref Message m)
		{
			if (m.Msg == 0x210 && m.WParam == (IntPtr)0x1020b)
			{
				return libraryViewer.GoBack();
			}

			return base.HandleWndProc(ref m);
		}

		private void libraryViewer_LoadEnded(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(TB_Search.Text))
			{
				foreach (var item in base_P_Tabs.Items.Where(x => x.PanelItem != null))
				{
					item.PanelItem.Selected = item.PanelItem.Data.ToString().Equals(libraryViewer.CurrentPath ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
				}

				base_P_Tabs.Invalidate();
			}
		}

		private void B_ListView_Click(object sender, EventArgs e)
		{
			B_GridView.Selected = false;
			B_ListView.Selected = true;
			libraryViewer.ioList.GridView = false;
			libraryViewer.ioList.Invalidate();
		}

		private void B_GridView_Click(object sender, EventArgs e)
		{
			B_GridView.Selected = true;
			B_ListView.Selected = false;
			libraryViewer.ioList.GridView = true;
			libraryViewer.ioList.Invalidate();
		}

		private void libraryViewer_CurrentPathChanged(object sender, EventArgs e)
		{
			foreach (var item in base_P_Tabs.Items)
			{
				if (item.PanelItem != null)
				{
					item.PanelItem.Selected = item.PanelItem.Data?.ToString() == libraryViewer.CurrentPath;
				}
			}

			base_P_Tabs.Invalidate();
		}

		private void I_SortDirection_SizeChanged(object sender, EventArgs e)
		{
			I_SortDirection.Width = I_SortDirection.Height;
			B_GridView.Size = B_ListView.Size = I_SortDirection.Size;
		}

		private void I_SortDirection_Click(object sender, EventArgs e)
		{
			libraryViewer.SortDesc = !libraryViewer.SortDesc;
			I_SortDirection.ImageName = libraryViewer.SortDesc ? "I_SortDesc" : "I_SortAsc";
			libraryViewer.SortingChanged();
		}

		private void ioSortDropdown_SelectedItemChanged(object sender, EventArgs e)
		{
			libraryViewer.Sorting = ioSortDropdown.SelectedItem;
			libraryViewer.SortingChanged();
		}
	}

	public class IOSelectionDialog : Component
	{
		[DefaultValue(null)] public string Title { get; set; }
		[DefaultValue(true)] public bool PreserveLastPath { get; set; } = true;
		[DefaultValue(null)] public string[] ValidExtensions { get; set; }
		[DefaultValue(null)] public string StartingFolder { get; set; }
		[DefaultValue(null)] public Func<FileInfo, bool> Filter { get; set; }
		[DefaultValue(null)] public Dictionary<string, string> PinnedFolders { get; set; }
		[DefaultValue(null)] public List<CustomFile> CustomFiles { get; set; }

		[Browsable(false)] public string LastFolder { get; private set; }
		[Browsable(false)] public string SelectedPath { get; private set; }

		public const string CustomDirectory = "%CUSTOM%";

		public DialogResult PromptFolder(Form form = null)
		{
			return prompt(true, form);
		}

		public DialogResult PromptFile(Form form = null)
		{
			return prompt(false, form);
		}

		private DialogResult prompt(bool folder, Form form)
		{
			var frm = new IoForm(folder, this)
			{
				Icon = form.Icon
			};

			try
			{ return frm.ShowDialog(form); }
			finally
			{
				SelectedPath = frm.SelectedPath;
				LastFolder = frm.CurrentPath;
			}
		}

		public struct CustomFile
		{
			public string Name { get; set; }
			public string Path { get; set; }
			public Bitmap Icon { get; set; }
		}
	}
}
