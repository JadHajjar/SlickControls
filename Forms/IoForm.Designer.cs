namespace SlickControls
{
	partial class IoForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon6 = new SlickControls.DynamicIcon();
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ioSortDropdown = new SlickControls.IoSortDropdown();
			this.I_SortDirection = new SlickControls.SlickIcon();
			this.B_ListView = new SlickControls.SlickIcon();
			this.B_Cancel = new SlickControls.SlickButton();
			this.B_OK = new SlickControls.SlickButton();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.L_Title = new System.Windows.Forms.Label();
			this.roundedPanel1 = new SlickControls.RoundedPanel();
			this.libraryViewer = new SlickControls.SlickLibraryViewer();
			this.B_GridView = new SlickControls.SlickIcon();
			this.base_P_Content.SuspendLayout();
			this.base_P_Container.SuspendLayout();
			this.TLP_Main.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.roundedPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
			this.base_P_Content.Controls.Add(this.TLP_Main);
			this.base_P_Content.Size = new System.Drawing.Size(761, 457);
			this.base_P_Content.Controls.SetChildIndex(this.TLP_Main, 0);
			// 
			// base_P_SideControls
			// 
			this.base_P_SideControls.Font = new System.Drawing.Font("Nirmala UI", 6.75F);
			this.base_P_SideControls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(129)))), ((int)(((byte)(150)))));
			this.base_P_SideControls.Location = new System.Drawing.Point(9, 420);
			this.base_P_SideControls.Size = new System.Drawing.Size(282, 10);
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(203)))), ((int)(((byte)(145)))));
			this.base_P_Container.Size = new System.Drawing.Size(763, 459);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 5;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Controls.Add(this.tableLayoutPanel1, 3, 1);
			this.TLP_Main.Controls.Add(this.B_ListView, 0, 3);
			this.TLP_Main.Controls.Add(this.B_Cancel, 4, 3);
			this.TLP_Main.Controls.Add(this.B_OK, 3, 3);
			this.TLP_Main.Controls.Add(this.TB_Search, 0, 1);
			this.TLP_Main.Controls.Add(this.L_Title, 0, 0);
			this.TLP_Main.Controls.Add(this.roundedPanel1, 0, 2);
			this.TLP_Main.Controls.Add(this.B_GridView, 1, 3);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(309, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 4;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.Size = new System.Drawing.Size(452, 457);
			this.TLP_Main.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.ioSortDropdown, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.I_SortDirection, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(332, 19);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(120, 31);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// ioSortDropdown
			// 
			this.ioSortDropdown.AccentBackColor = true;
			this.ioSortDropdown.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ioSortDropdown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ioSortDropdown.Location = new System.Drawing.Point(21, 0);
			this.ioSortDropdown.Margin = new System.Windows.Forms.Padding(0);
			this.ioSortDropdown.Name = "ioSortDropdown";
			this.ioSortDropdown.Size = new System.Drawing.Size(99, 31);
			this.ioSortDropdown.TabIndex = 1;
			this.ioSortDropdown.SelectedItemChanged += new System.EventHandler(this.ioSortDropdown_SelectedItemChanged);
			// 
			// I_SortDirection
			// 
			this.I_SortDirection.ActiveColor = null;
			this.I_SortDirection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.I_SortDirection.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "I_SortAsc";
			this.I_SortDirection.ImageName = dynamicIcon1;
			this.I_SortDirection.Location = new System.Drawing.Point(0, 0);
			this.I_SortDirection.Margin = new System.Windows.Forms.Padding(0, 0, 7, 0);
			this.I_SortDirection.Name = "I_SortDirection";
			this.I_SortDirection.Size = new System.Drawing.Size(14, 31);
			this.I_SortDirection.SpaceTriggersClick = true;
			this.I_SortDirection.TabIndex = 0;
			this.I_SortDirection.SizeChanged += new System.EventHandler(this.I_SortDirection_SizeChanged);
			this.I_SortDirection.Click += new System.EventHandler(this.I_SortDirection_Click);
			// 
			// B_ListView
			// 
			this.B_ListView.ActiveColor = null;
			this.B_ListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.B_ListView.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_List";
			this.B_ListView.ImageName = dynamicIcon2;
			this.B_ListView.Location = new System.Drawing.Point(3, 424);
			this.B_ListView.Name = "B_ListView";
			this.B_ListView.Size = new System.Drawing.Size(34, 30);
			this.B_ListView.SpaceTriggersClick = true;
			this.B_ListView.TabIndex = 5;
			this.B_ListView.SizeChanged += new System.EventHandler(this.B_ListView_SizeChanged);
			this.B_ListView.Click += new System.EventHandler(this.B_ListView_Click);
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cancel.AutoSize = true;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon3.Name = "I_Cancel";
			this.B_Cancel.ImageName = dynamicIcon3;
			this.B_Cancel.Location = new System.Drawing.Point(349, 424);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Size = new System.Drawing.Size(100, 30);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 2;
			this.B_Cancel.Text = "CANCEL";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// B_OK
			// 
			this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_OK.AutoSize = true;
			this.B_OK.ColorShade = null;
			this.B_OK.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "I_Ok";
			this.B_OK.ImageName = dynamicIcon4;
			this.B_OK.Location = new System.Drawing.Point(243, 424);
			this.B_OK.Name = "B_OK";
			this.B_OK.Size = new System.Drawing.Size(100, 30);
			this.B_OK.SpaceTriggersClick = true;
			this.B_OK.TabIndex = 3;
			this.B_OK.Text = "SELECT";
			this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
			// 
			// TB_Search
			// 
			this.TB_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.TLP_Main.SetColumnSpan(this.TB_Search, 3);
			this.TB_Search.EnterTriggersClick = false;
			this.TB_Search.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dynamicIcon5.Name = "I_Search";
			this.TB_Search.ImageName = dynamicIcon5;
			this.TB_Search.LabelText = "";
			this.TB_Search.Location = new System.Drawing.Point(3, 22);
			this.TB_Search.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Search.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Placeholder = "Search..";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(179, 25);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			// 
			// L_Title
			// 
			this.L_Title.AutoSize = true;
			this.TLP_Main.SetColumnSpan(this.L_Title, 3);
			this.L_Title.Location = new System.Drawing.Point(3, 0);
			this.L_Title.Name = "L_Title";
			this.L_Title.Size = new System.Drawing.Size(99, 19);
			this.L_Title.TabIndex = 1;
			this.L_Title.Text = "Pinned Folders";
			// 
			// roundedPanel1
			// 
			this.roundedPanel1.AddOutline = true;
			this.TLP_Main.SetColumnSpan(this.roundedPanel1, 5);
			this.roundedPanel1.Controls.Add(this.libraryViewer);
			this.roundedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.roundedPanel1.Location = new System.Drawing.Point(3, 53);
			this.roundedPanel1.Name = "roundedPanel1";
			this.roundedPanel1.Size = new System.Drawing.Size(446, 365);
			this.roundedPanel1.TabIndex = 9;
			// 
			// libraryViewer
			// 
			this.libraryViewer.AutoSize = true;
			this.libraryViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.libraryViewer.Cursor = System.Windows.Forms.Cursors.Default;
			this.libraryViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.libraryViewer.Extensions = null;
			this.libraryViewer.FoldersOnly = false;
			this.libraryViewer.Location = new System.Drawing.Point(0, 0);
			this.libraryViewer.Margin = new System.Windows.Forms.Padding(0);
			this.libraryViewer.MinimumSize = new System.Drawing.Size(200, 50);
			this.libraryViewer.Name = "libraryViewer";
			this.libraryViewer.Size = new System.Drawing.Size(446, 365);
			this.libraryViewer.StartingFolder = null;
			this.libraryViewer.TabIndex = 0;
			this.libraryViewer.TopFolders = new string[0];
			this.libraryViewer.CurrentPathChanged += new System.EventHandler(this.libraryViewer_CurrentPathChanged);
			// 
			// B_GridView
			// 
			this.B_GridView.ActiveColor = null;
			this.B_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.B_GridView.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon6.Name = "I_Grid";
			this.B_GridView.ImageName = dynamicIcon6;
			this.B_GridView.Location = new System.Drawing.Point(43, 424);
			this.B_GridView.Name = "B_GridView";
			this.B_GridView.Selected = true;
			this.B_GridView.Size = new System.Drawing.Size(33, 30);
			this.B_GridView.SpaceTriggersClick = true;
			this.B_GridView.TabIndex = 4;
			this.B_GridView.Click += new System.EventHandler(this.B_GridView_Click);
			// 
			// IoForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(774, 470);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.IconBounds = new System.Drawing.Rectangle(125, 46, 32, 32);
			this.MaximizeBox = true;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 2560, 1380);
			this.MinimizeBox = true;
			this.Name = "IoForm";
			this.SidebarItems = new SlickControls.PanelItem[0];
			this.Text = "IoForm";
			this.base_P_Content.ResumeLayout(false);
			this.base_P_Content.PerformLayout();
			this.base_P_Container.ResumeLayout(false);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.roundedPanel1.ResumeLayout(false);
			this.roundedPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel TLP_Main;
		protected System.Windows.Forms.Label L_Title;
		protected SlickTextBox TB_Search;
		protected SlickButton B_Cancel;
		protected SlickButton B_OK;
		private RoundedPanel roundedPanel1;
		protected SlickLibraryViewer libraryViewer;
		protected SlickIcon B_ListView;
		protected SlickIcon B_GridView;
		protected SlickIcon I_SortDirection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private IoSortDropdown ioSortDropdown;
	}
}