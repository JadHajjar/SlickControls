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
			this.FLP_CommonFolders = new System.Windows.Forms.FlowLayoutPanel();
			this.base_P_Container.SuspendLayout();




			this.TLP_Main.SuspendLayout();

			this.TLP_Side.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(217)))), ((int)(((byte)(179)))));
			this.base_P_Container.Controls.Add(this.TLP_Main);
			this.base_P_Container.Size = new System.Drawing.Size(789, 439);
			this.base_P_Container.Paint += new System.Windows.Forms.PaintEventHandler(this.base_P_Container_Paint);
			// 
			// base_PB_Icon
			// 
			this.base_PB_Icon.Size = new System.Drawing.Size(20, 20);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 4;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.Controls.Add(this.L_Title, 1, 0);
			this.TLP_Main.Controls.Add(this.B_Cancel, 3, 4);
			this.TLP_Main.Controls.Add(this.B_OK, 1, 4);
			this.TLP_Main.Controls.Add(this.libraryViewer, 0, 2);
			this.TLP_Main.Controls.Add(this.TB_Search, 2, 1);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 2, 3);
			this.TLP_Main.Controls.Add(this.TI_Close, 3, 0);
			this.TLP_Main.Controls.Add(this.TLP_Side, 0, 0);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(1, 1);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 5;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.Size = new System.Drawing.Size(787, 437);
			this.TLP_Main.TabIndex = 0;
			// 
			// L_Title
			// 
			this.L_Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.L_Title.AutoSize = true;
			this.L_Title.Location = new System.Drawing.Point(182, 7);
			this.L_Title.Margin = new System.Windows.Forms.Padding(7);
			this.L_Title.Name = "L_Title";
			this.TLP_Main.SetRowSpan(this.L_Title, 2);
			this.L_Title.Size = new System.Drawing.Size(84, 13);
			this.L_Title.TabIndex = 0;
			this.L_Title.Text = "Pinned Folders";
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.IconSize = 16;
			this.B_Cancel.Image = global::SlickControls.Properties.Resources.Tiny_Cancel;
			this.B_Cancel.Location = new System.Drawing.Point(677, 397);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(10);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Cancel.Size = new System.Drawing.Size(100, 30);
			this.B_Cancel.TabIndex = 2;
			this.B_Cancel.Text = "CANCEL";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// B_OK
			// 
			this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_OK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_OK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_OK.ColorShade = null;
			this.TLP_Main.SetColumnSpan(this.B_OK, 2);
			this.B_OK.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OK.IconSize = 16;
			this.B_OK.Image = global::SlickControls.Properties.Resources.Tiny_Ok;
			this.B_OK.Location = new System.Drawing.Point(557, 397);
			this.B_OK.Margin = new System.Windows.Forms.Padding(10);
			this.B_OK.Name = "B_OK";
			this.B_OK.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_OK.Size = new System.Drawing.Size(100, 30);
			this.B_OK.TabIndex = 2;
			this.B_OK.Text = "SELECT";
			this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
			// 
			// libraryViewer
			// 
			this.libraryViewer.AutoSize = true;
			this.libraryViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.SetColumnSpan(this.libraryViewer, 3);
			this.libraryViewer.Cursor = System.Windows.Forms.Cursors.Default;
			this.libraryViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.libraryViewer.Extensions = null;
			this.libraryViewer.FoldersOnly = false;
			this.libraryViewer.Location = new System.Drawing.Point(175, 65);
			this.libraryViewer.Margin = new System.Windows.Forms.Padding(0);
			this.libraryViewer.MinimumSize = new System.Drawing.Size(200, 50);
			this.libraryViewer.Name = "libraryViewer";
			this.libraryViewer.Size = new System.Drawing.Size(612, 315);
			this.libraryViewer.StartingFolder = null;
			this.libraryViewer.TabIndex = 1;
			this.libraryViewer.TopFolders = new string[0];
			this.libraryViewer.LoadEnded += new System.EventHandler(this.libraryViewer_LoadEnded);
			// 
			// TB_Search
			// 
			this.TB_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.TLP_Main.SetColumnSpan(this.TB_Search, 2);
			this.TB_Search.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Search.Image = null;
			this.TB_Search.LabelText = "Search";
			this.TB_Search.Location = new System.Drawing.Point(302, 22);
			this.TB_Search.Margin = new System.Windows.Forms.Padding(3, 3, 110, 3);
			this.TB_Search.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Search.MaxLength = 32767;
			this.TB_Search.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Password = false;
			this.TB_Search.Placeholder = "Type to start searching";
			this.TB_Search.ReadOnly = false;
			this.TB_Search.Required = false;
			this.TB_Search.SelectAllOnFocus = false;
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.Size = new System.Drawing.Size(375, 40);
			this.TB_Search.TabIndex = 3;
			this.TB_Search.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Search.Validation = SlickControls.ValidationType.None;
			this.TB_Search.ValidationRegex = "";
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 2);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(276, 383);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(508, 1);
			this.slickSpacer1.TabIndex = 4;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TI_Close
			// 
			this.TI_Close.Color = SlickControls.TopIcon.IconStyle.Close;
			this.TI_Close.Cursor = System.Windows.Forms.Cursors.Hand;
			this.TI_Close.Dock = System.Windows.Forms.DockStyle.Right;
			this.TI_Close.Location = new System.Drawing.Point(768, 3);
			this.TI_Close.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
			this.TI_Close.Name = "TI_Close";
			this.TI_Close.Size = new System.Drawing.Size(16, 16);
			this.TI_Close.TabIndex = 6;
			this.TI_Close.TabStop = false;
			this.TI_Close.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// TLP_Side
			// 
			this.TLP_Side.ColumnCount = 1;
			this.TLP_Side.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Side.Controls.Add(this.L_Side, 0, 0);
			this.TLP_Side.Controls.Add(this.FLP_CommonFolders, 0, 1);
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
			// FLP_CommonFolders
			// 
			this.FLP_CommonFolders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FLP_CommonFolders.Location = new System.Drawing.Point(0, 27);
			this.FLP_CommonFolders.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_CommonFolders.Name = "FLP_CommonFolders";
			this.FLP_CommonFolders.Size = new System.Drawing.Size(175, 410);
			this.FLP_CommonFolders.TabIndex = 5;
			// 
			// IOSelectionForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.IconBounds = new System.Drawing.Rectangle(3, 4, 20, 20);
			this.Name = "IOSelectionForm";
			this.Opacity = 1D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "IOSelectionDialog";
			this.base_P_Container.ResumeLayout(false);




			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();

			this.TLP_Side.ResumeLayout(false);
			this.TLP_Side.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TLP_Main;
		private SlickLibraryViewer libraryViewer;
		private SlickButton B_Cancel;
		private SlickButton B_OK;
		private SlickTextBox TB_Search;
		private SlickSpacer slickSpacer1;
		private System.Windows.Forms.FlowLayoutPanel FLP_CommonFolders;
		private TopIcon TI_Close;
		private System.Windows.Forms.TableLayoutPanel TLP_Side;
		private System.Windows.Forms.Label L_Side;
		private System.Windows.Forms.Label L_Title;
	}
}