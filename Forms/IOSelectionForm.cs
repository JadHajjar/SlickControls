using Extensions;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class IOSelectionForm : SlickForm
	{
		private readonly PanelItemControl base_P_Tabs;
		protected readonly bool folderSelection;
		protected string selectedPath;
		protected static string lastPath;

		public string CurrentPath => libraryViewer.CurrentPath;
		public string SelectedPath => selectedPath;

		protected IOSelectionForm()
		{
			InitializeComponent();
		}

		public IOSelectionForm(bool folder, string[] extensions, string startingFolder = null)
		{
			InitializeComponent();

			folderSelection = folder;
			L_Title.Text = folder ? "Select a Folder" : "Select a File";
			base_P_Tabs = new PanelItemControl(null) { Dock = DockStyle.Fill };

			TLP_Main.MouseDown += Form_MouseDown;
			TLP_Side.MouseDown += Form_MouseDown;
			base_P_Tabs.OnFormMove += Form_MouseDown;
			L_Side.MouseDown += Form_MouseDown;
			L_Title.MouseDown += Form_MouseDown;

			libraryViewer.StartingFolder = startingFolder ?? lastPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			libraryViewer.FoldersOnly = folder;
			libraryViewer.Extensions = extensions;
			libraryViewer.TopFolders = DriveInfo.GetDrives().Select(x => x.RootDirectory.FullName).ToArray();
			libraryViewer.SearchCleared += (s, e) => TB_Search.Text = string.Empty;
			libraryViewer.FileOpened += (s, e) =>
			{
				selectedPath = e.File.FullName;
				DialogResult = DialogResult.OK;
				Close();
			};

			add(string.Empty, "This PC", Properties.Resources.Tiny_PC);

			base_P_Tabs.Add(PanelTab.GroupName("Drives"));

			foreach (var item in libraryViewer.TopFolders)
			{
				add(item, item, Properties.Resources.Tiny_Drive);
			}

			TLP_Side.Controls.Add(base_P_Tabs, 0, 1);

			base_P_Tabs.Add(PanelTab.GroupName("Common Folders"));

			foreach (var item in new[]
			{
				Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
				Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
				Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
				Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
				Environment.GetFolderPath(Environment.SpecialFolder.Recent),
			})
			{
				add(item, Path.GetFileName(item).FormatWords().IfEmpty(item), Properties.Resources.Tiny_Folder);
			}

			FormDesign.DesignChanged += DesignChanged;

			void add(string path, string name, Bitmap icon)
			{
				var item = new PanelItem
				{
					Icon = icon,
					Text = name,
					Data = path
				};

				item.OnClick += Item_OnClick;

				base_P_Tabs.Add(new PanelTab(item));
			}
		}

		private void Item_OnClick(object sender, MouseEventArgs e)
		{
			var pi = sender as PanelItem;

			if (!pi.Selected)
			{
				libraryViewer.folderOpened(!string.IsNullOrWhiteSpace(pi.Data as string) ? new DirectoryInfo(pi.Data as string) : null);
			}
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			TLP_Main.BackColor = design.BackColor;
			TLP_Side.BackColor = design.MenuColor;
			TLP_Side.ForeColor = design.MenuForeColor;
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			TI_Close.Size = UI.Scale(new Size(16, 16), UI.UIScale);
			L_Title.Font = UI.Font(9F, FontStyle.Bold);
			TLP_Main.ColumnStyles[0].Width = (int)(175 * UI.UIScale);
		}

		private void B_Cancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
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

		private void base_P_Container_Paint(object sender, PaintEventArgs e)
		{
			switch (CurrentFormState)
			{
				case FormState.NormalUnfocused:
					e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.MenuColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Dark ? 3 : -3)), new Rectangle(0, 0, TLP_Side.Width + 1, TLP_Side.Height + 2));
					break;

				case FormState.ForcedFocused:
				case FormState.NormalFocused:
					e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.MenuColor.MergeColor(FormDesign.Design.ActiveColor, 70)), new Rectangle(0, 0, TLP_Side.Width + 1, TLP_Side.Height + 2));
					break;
			}
		}
	}

	public class IOSelectionDialog : Component
	{
		[DefaultValue(true)]
		public bool PreserveLastPath { get; set; } = true;
		public string[] ValidExtensions { get; set; }
		public string SelectedPath { get; private set; }
		public string LastFolder { get; set; }

		public DialogResult PromptFolder(Form form = null, string startingFolder = null)
		{
			return prompt(true, form, startingFolder);
		}

		public DialogResult PromptFile(Form form = null, string startingFolder = null)
		{
			return prompt(false, form, startingFolder);
		}

		private DialogResult prompt(bool folder, Form form, string startingFolder)
		{
			var frm = new IOSelectionForm(folder, ValidExtensions, PreserveLastPath ? LastFolder ?? startingFolder : startingFolder);

			try
			{ return frm.ShowDialog(form); }
			finally
			{
				SelectedPath = frm.SelectedPath;
				LastFolder = frm.CurrentPath;
			}
		}
	}
}