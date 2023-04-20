using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls.Forms
{
	public partial class IoForm : BasePanelForm
	{
		protected readonly bool folderSelection;
		protected string selectedPath;
		protected static string lastPath;

		public string CurrentPath => libraryViewer.CurrentPath;
		public string SelectedPath => selectedPath;

		public IoForm(bool folder, string[] extensions, string startingFolder = null)
		{
			InitializeComponent();

			base_P_Icon.Controls.Remove(base_PB_Icon);
			base_PB_Icon.Controls.Add(base_PB_Icon);

			folderSelection = folder;
			L_Title.Text = folder ? "Select a Folder" : "Select a File";

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
	}
}
