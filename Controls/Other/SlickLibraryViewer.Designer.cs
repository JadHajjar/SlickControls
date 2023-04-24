namespace SlickControls
{
	partial class SlickLibraryViewer
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PB_Bar = new SlickControls.SlickPictureBox();
			this.PB_Loader = new SlickControls.SlickPictureBox();
			this.TB_Path = new SlickControls.SlickPathTextBox();
			this.ioList = new SlickControls.Controls.Other.IoListControl();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			((System.ComponentModel.ISupportInitialize)(this.PB_Bar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// PB_Bar
			// 
			this.PB_Bar.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.PB_Bar.Location = new System.Drawing.Point(59, 41);
			this.PB_Bar.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Bar.Name = "PB_Bar";
			this.PB_Bar.Size = new System.Drawing.Size(796, 78);
			this.PB_Bar.TabIndex = 0;
			this.PB_Bar.TabStop = false;
			this.PB_Bar.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Bar_Paint);
			this.PB_Bar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.P_Bar_MouseClick);
			// 
			// PB_Loader
			// 
			this.PB_Loader.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.PB_Loader.Loading = true;
			this.PB_Loader.Location = new System.Drawing.Point(855, 43);
			this.PB_Loader.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Loader.Name = "PB_Loader";
			this.tableLayoutPanel2.SetRowSpan(this.PB_Loader, 2);
			this.PB_Loader.Size = new System.Drawing.Size(20, 32);
			this.PB_Loader.TabIndex = 6;
			this.PB_Loader.TabStop = false;
			this.PB_Loader.Visible = false;
			this.PB_Loader.Click += new System.EventHandler(this.Generic_Click);
			// 
			// TB_Path
			// 
			this.TB_Path.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Path.FileExtensions = new string[0];
			this.TB_Path.Folder = true;
			this.TB_Path.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Path.LabelText = "Folder";
			this.TB_Path.Location = new System.Drawing.Point(62, 3);
			this.TB_Path.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Path.MinimumSize = new System.Drawing.Size(50, 35);
			this.TB_Path.Name = "TB_Path";
			this.TB_Path.Placeholder = "Folder Path";
			this.TB_Path.SelectedText = "";
			this.TB_Path.SelectionLength = 0;
			this.TB_Path.SelectionStart = 0;
			this.TB_Path.ShowLabel = false;
			this.TB_Path.Size = new System.Drawing.Size(790, 35);
			this.TB_Path.TabIndex = 1;
			this.TB_Path.Visible = false;
			this.TB_Path.TextChanged += new System.EventHandler(this.TB_Path_TextChanged);
			this.TB_Path.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_Path_KeyPress);
			this.TB_Path.Leave += new System.EventHandler(this.TB_Path_Leave);
			// 
			// ioList
			// 
			this.ioList.AutoInvalidate = false;
			this.ioList.AutoScroll = true;
			this.ioList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ioList.GridItemSize = new System.Drawing.Size(85, 85);
			this.ioList.GridView = true;
			this.ioList.ItemHeight = 28;
			this.ioList.Location = new System.Drawing.Point(0, 148);
			this.ioList.Name = "ioList";
			this.ioList.Size = new System.Drawing.Size(915, 658);
			this.ioList.TabIndex = 10;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 5;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.PB_Bar, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.TB_Path, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.PB_Loader, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.slickSpacer1, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(915, 148);
			this.tableLayoutPanel2.TabIndex = 11;
			// 
			// slickSpacer1
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.slickSpacer1, 5);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(3, 122);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(909, 23);
			this.slickSpacer1.TabIndex = 0;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// SlickLibraryViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.ioList);
			this.Controls.Add(this.tableLayoutPanel2);
			this.MinimumSize = new System.Drawing.Size(200, 50);
			this.Name = "SlickLibraryViewer";
			this.Size = new System.Drawing.Size(915, 806);
			((System.ComponentModel.ISupportInitialize)(this.PB_Bar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SlickControls.SlickPictureBox PB_Bar;
		private SlickControls.SlickPictureBox PB_Loader;
		private SlickControls.SlickPathTextBox TB_Path;
		internal Controls.Other.IoListControl ioList;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickSpacer slickSpacer1;
	}
}
