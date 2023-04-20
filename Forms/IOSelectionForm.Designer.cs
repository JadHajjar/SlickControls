namespace SlickControls
{
	partial class IOSelectionForm
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
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.L_Title = new System.Windows.Forms.Label();
			this.B_Cancel = new SlickControls.SlickButton();
			this.B_OK = new SlickControls.SlickButton();
			this.libraryViewer = new SlickControls.SlickLibraryViewer();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.TI_Close = new SlickControls.TopIcon();
			this.TLP_Side = new System.Windows.Forms.TableLayoutPanel();
			this.L_Side = new System.Windows.Forms.Label();
			this.P_CustomPanel = new System.Windows.Forms.Panel();
			this.base_P_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).BeginInit();
			this.TLP_Main.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TI_Close)).BeginInit();
			this.TLP_Side.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(217)))), ((int)(((byte)(179)))));
			this.base_P_Container.Controls.Add(this.TLP_Main);
			this.base_P_Container.Size = new System.Drawing.Size(789, 439);
			// 
			// base_PB_Icon
			// 
			this.base_PB_Icon.Size = new System.Drawing.Size(20, 20);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 5;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.Controls.Add(this.L_Title, 0, 0);
			this.TLP_Main.Controls.Add(this.B_Cancel, 4, 4);
			this.TLP_Main.Controls.Add(this.B_OK, 3, 4);
			this.TLP_Main.Controls.Add(this.libraryViewer, 0, 2);
			this.TLP_Main.Controls.Add(this.TB_Search, 0, 1);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 1, 3);
			this.TLP_Main.Controls.Add(this.TI_Close, 4, 0);
			this.TLP_Main.Controls.Add(this.TLP_Side, 0, 0);
			this.TLP_Main.Controls.Add(this.P_CustomPanel, 1, 4);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(1, 1);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 5;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Size = new System.Drawing.Size(787, 437);
			this.TLP_Main.TabIndex = 0;
			// 
			// L_Title
			// 
			this.L_Title.AutoSize = true;
			this.TLP_Main.SetColumnSpan(this.L_Title, 2);
			this.L_Title.Location = new System.Drawing.Point(190, 7);
			this.L_Title.Margin = new System.Windows.Forms.Padding(15, 7, 7, 0);
			this.L_Title.Name = "L_Title";
			this.L_Title.Size = new System.Drawing.Size(84, 13);
			this.L_Title.TabIndex = 0;
			this.L_Title.Text = "Pinned Folders";
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Image = global::SlickControls.Properties.Resources.Tiny_Cancel;
			this.B_Cancel.Location = new System.Drawing.Point(680, 400);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(7);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Cancel.Size = new System.Drawing.Size(100, 30);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 3;
			this.B_Cancel.Text = "CANCEL";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// B_OK
			// 
			this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_OK.ColorShade = null;
			this.B_OK.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OK.Image = global::SlickControls.Properties.Resources.Tiny_Ok;
			this.B_OK.Location = new System.Drawing.Point(566, 400);
			this.B_OK.Margin = new System.Windows.Forms.Padding(7);
			this.B_OK.Name = "B_OK";
			this.B_OK.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_OK.Size = new System.Drawing.Size(100, 30);
			this.B_OK.SpaceTriggersClick = true;
			this.B_OK.TabIndex = 2;
			this.B_OK.Text = "SELECT";
			this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
			// 
			// libraryViewer
			// 
			this.libraryViewer.AutoSize = true;
			this.libraryViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.SetColumnSpan(this.libraryViewer, 4);
			this.libraryViewer.Cursor = System.Windows.Forms.Cursors.Default;
			this.libraryViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.libraryViewer.Extensions = null;
			this.libraryViewer.FoldersOnly = false;
			this.libraryViewer.Location = new System.Drawing.Point(175, 55);
			this.libraryViewer.Margin = new System.Windows.Forms.Padding(0);
			this.libraryViewer.MinimumSize = new System.Drawing.Size(200, 50);
			this.libraryViewer.Name = "libraryViewer";
			this.libraryViewer.Size = new System.Drawing.Size(612, 331);
			this.libraryViewer.StartingFolder = null;
			this.libraryViewer.TabIndex = 1;
			this.libraryViewer.TopFolders = new string[0];
			this.libraryViewer.LoadEnded += new System.EventHandler(this.libraryViewer_LoadEnded);
			// 
			// TB_Search
			// 
			this.TB_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.TLP_Main.SetColumnSpan(this.TB_Search, 3);
			this.TB_Search.EnterTriggersClick = false;
			this.TB_Search.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Search.LabelText = "";
			this.TB_Search.Location = new System.Drawing.Point(190, 27);
			this.TB_Search.Margin = new System.Windows.Forms.Padding(15, 0, 0, 3);
			this.TB_Search.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Search.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Placeholder = "Search..";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.ShowLabel = false;
			this.TB_Search.Size = new System.Drawing.Size(350, 25);
			this.TB_Search.TabIndex = 0;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 4);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(178, 389);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(606, 1);
			this.slickSpacer1.TabIndex = 4;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TI_Close
			// 
			this.TI_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TI_Close.Color = SlickControls.TopIcon.IconStyle.Close;
			this.TI_Close.Cursor = System.Windows.Forms.Cursors.Hand;
			this.TI_Close.Location = new System.Drawing.Point(760, 3);
			this.TI_Close.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
			this.TI_Close.Name = "TI_Close";
			this.TI_Close.Size = new System.Drawing.Size(24, 24);
			this.TI_Close.TabIndex = 6;
			this.TI_Close.TabStop = false;
			this.TI_Close.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// TLP_Side
			// 
			this.TLP_Side.ColumnCount = 1;
			this.TLP_Side.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Side.Controls.Add(this.L_Side, 0, 0);
			this.TLP_Side.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Side.Location = new System.Drawing.Point(0, 0);
			this.TLP_Side.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Side.Name = "TLP_Side";
			this.TLP_Side.RowCount = 2;
			this.TLP_Main.SetRowSpan(this.TLP_Side, 5);
			this.TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Side.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Side.Size = new System.Drawing.Size(175, 437);
			this.TLP_Side.TabIndex = 7;
			// 
			// L_Side
			// 
			this.L_Side.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.L_Side.AutoSize = true;
			this.L_Side.Location = new System.Drawing.Point(45, 7);
			this.L_Side.Margin = new System.Windows.Forms.Padding(7);
			this.L_Side.Name = "L_Side";
			this.L_Side.Size = new System.Drawing.Size(84, 13);
			this.L_Side.TabIndex = 0;
			this.L_Side.Text = "Pinned Folders";
			// 
			// P_CustomPanel
			// 
			this.P_CustomPanel.AutoSize = true;
			this.P_CustomPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.SetColumnSpan(this.P_CustomPanel, 2);
			this.P_CustomPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.P_CustomPanel.Location = new System.Drawing.Point(178, 396);
			this.P_CustomPanel.Name = "P_CustomPanel";
			this.P_CustomPanel.Size = new System.Drawing.Size(0, 38);
			this.P_CustomPanel.TabIndex = 8;
			this.P_CustomPanel.Visible = false;
			// 
			// IOSelectionForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.IconBounds = new System.Drawing.Rectangle(3, 4, 20, 20);
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 1920, 1032);
			this.Name = "IOSelectionForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "IOSelectionDialog";
			this.base_P_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).EndInit();
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TI_Close)).EndInit();
			this.TLP_Side.ResumeLayout(false);
			this.TLP_Side.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.TableLayoutPanel TLP_Main;
		protected SlickLibraryViewer libraryViewer;
		protected SlickButton B_Cancel;
		protected SlickButton B_OK;
		protected SlickTextBox TB_Search;
		protected SlickSpacer slickSpacer1;
		protected TopIcon TI_Close;
		protected System.Windows.Forms.TableLayoutPanel TLP_Side;
		protected System.Windows.Forms.Label L_Side;
		protected System.Windows.Forms.Label L_Title;
		public System.Windows.Forms.Panel P_CustomPanel;
	}
}