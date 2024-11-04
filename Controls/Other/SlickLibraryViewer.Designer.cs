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
			this.TB_Path = new SlickControls.SlickPathTextBox();
			this.ioList = new SlickControls.Controls.Other.IoListControl();
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			((System.ComponentModel.ISupportInitialize)(this.PB_Bar)).BeginInit();
			this.TLP_Main.SuspendLayout();
			this.SuspendLayout();
			// 
			// PB_Bar
			// 
			this.PB_Bar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PB_Bar.LoaderSpeed = 1D;
			this.PB_Bar.Location = new System.Drawing.Point(0, 41);
			this.PB_Bar.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Bar.Name = "PB_Bar";
			this.PB_Bar.Size = new System.Drawing.Size(915, 78);
			this.PB_Bar.TabIndex = 0;
			this.PB_Bar.TabStop = false;
			this.PB_Bar.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Bar_Paint);
			this.PB_Bar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.P_Bar_MouseClick);
			// 
			// TB_Path
			// 
			this.TB_Path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Path.FileExtensions = new string[0];
			this.TB_Path.Folder = true;
			this.TB_Path.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Path.LabelText = "Folder";
			this.TB_Path.Location = new System.Drawing.Point(3, 3);
			this.TB_Path.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Path.MinimumSize = new System.Drawing.Size(50, 35);
			this.TB_Path.Name = "TB_Path";
			this.TB_Path.Padding = new System.Windows.Forms.Padding(4, 10, 4, 10);
			this.TB_Path.Placeholder = "Folder Path";
			this.TB_Path.SelectedText = "";
			this.TB_Path.SelectionLength = 0;
			this.TB_Path.SelectionStart = 0;
			this.TB_Path.ShowLabel = false;
			this.TB_Path.Size = new System.Drawing.Size(909, 35);
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
			// TLP_Main
			// 
			this.TLP_Main.AutoSize = true;
			this.TLP_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.ColumnCount = 1;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.TLP_Main.Controls.Add(this.PB_Bar, 0, 1);
			this.TLP_Main.Controls.Add(this.TB_Path, 0, 0);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 0, 2);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_Main.Location = new System.Drawing.Point(0, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 3;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.Size = new System.Drawing.Size(915, 148);
			this.TLP_Main.TabIndex = 11;
			// 
			// slickSpacer1
			// 
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
			this.Controls.Add(this.TLP_Main);
			this.MinimumSize = new System.Drawing.Size(200, 50);
			this.Name = "SlickLibraryViewer";
			this.Size = new System.Drawing.Size(915, 806);
			((System.ComponentModel.ISupportInitialize)(this.PB_Bar)).EndInit();
			this.TLP_Main.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SlickControls.SlickPictureBox PB_Bar;
		private SlickControls.SlickPathTextBox TB_Path;
		internal Controls.Other.IoListControl ioList;
		private System.Windows.Forms.TableLayoutPanel TLP_Main;
		private SlickSpacer slickSpacer1;
	}
}
