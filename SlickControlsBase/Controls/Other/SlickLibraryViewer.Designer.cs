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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlickLibraryViewer));
			this.PB_Bar = new SlickControls.SlickPictureBox();
			this.FLP_Content = new System.Windows.Forms.FlowLayoutPanel();
			this.P_Spacer = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.PB_Loader = new SlickPictureBox();
			this.P_Bar = new System.Windows.Forms.Panel();
			this.TB_Path = new SlickControls.SlickPathTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.L_NoResults = new System.Windows.Forms.Label();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.tableLayoutPanel1.SuspendLayout();
			this.P_Bar.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PB_Bar
			// 
			this.PB_Bar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PB_Bar.Location = new System.Drawing.Point(0, 0);
			this.PB_Bar.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Bar.Name = "PB_Bar";
			this.PB_Bar.Size = new System.Drawing.Size(1, 100);
			this.PB_Bar.TabIndex = 0;
			this.PB_Bar.TabStop = false;
			this.PB_Bar.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Bar_Paint);
			this.PB_Bar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.P_Bar_MouseClick);
			// 
			// FLP_Content
			// 
			this.FLP_Content.AutoSize = true;
			this.FLP_Content.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FLP_Content.Location = new System.Drawing.Point(0, 0);
			this.FLP_Content.Name = "FLP_Content";
			this.FLP_Content.Size = new System.Drawing.Size(0, 0);
			this.FLP_Content.TabIndex = 2;
			this.FLP_Content.Click += new System.EventHandler(this.Generic_Click);
			// 
			// P_Spacer
			// 
			this.P_Spacer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.P_Spacer, 2);
			this.P_Spacer.Location = new System.Drawing.Point(70, 105);
			this.P_Spacer.Margin = new System.Windows.Forms.Padding(70, 0, 100, 0);
			this.P_Spacer.Name = "P_Spacer";
			this.P_Spacer.Size = new System.Drawing.Size(18, 1);
			this.P_Spacer.TabIndex = 3;
			this.P_Spacer.Click += new System.EventHandler(this.Generic_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.PB_Loader, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.P_Bar, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.P_Spacer, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 106);
			this.tableLayoutPanel1.TabIndex = 4;
			this.tableLayoutPanel1.Click += new System.EventHandler(this.Generic_Click);
			// 
			// PB_Loader
			// 
			this.PB_Loader.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.PB_Loader.Location = new System.Drawing.Point(-12, 27);
			this.PB_Loader.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
			this.PB_Loader.Name = "PB_Loader";
			this.PB_Loader.Size = new System.Drawing.Size(32, 32);
			this.PB_Loader.TabIndex = 6;
			this.PB_Loader.TabStop = false;
			this.PB_Loader.Loading = true;
			this.PB_Loader.Visible = false;
			this.PB_Loader.Click += new System.EventHandler(this.Generic_Click);
			// 
			// P_Bar
			// 
			this.P_Bar.Controls.Add(this.TB_Path);
			this.P_Bar.Controls.Add(this.PB_Bar);
			this.P_Bar.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Bar.Location = new System.Drawing.Point(75, 5);
			this.P_Bar.Margin = new System.Windows.Forms.Padding(75, 5, 105, 0);
			this.P_Bar.Name = "P_Bar";
			this.P_Bar.Size = new System.Drawing.Size(1, 100);
			this.P_Bar.TabIndex = 9;
			this.P_Bar.Click += new System.EventHandler(this.Generic_Click);
			// 
			// TB_Path
			// 
			this.TB_Path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Path.FileExtensions = new string[0];
			this.TB_Path.Folder = true;
			this.TB_Path.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Path.Image = Properties.Resources.Tiny_Search;
			this.TB_Path.LabelText = "Folder";
			this.TB_Path.Location = new System.Drawing.Point(0, 0);
			this.TB_Path.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Path.MaxLength = 32767;
			this.TB_Path.MinimumSize = new System.Drawing.Size(50, 35);
			this.TB_Path.Name = "TB_Path";
			this.TB_Path.Password = false;
			this.TB_Path.Placeholder = "Folder Path";
			this.TB_Path.ReadOnly = false;
			this.TB_Path.Required = false;
			this.TB_Path.SelectAllOnFocus = false;
			this.TB_Path.SelectedText = "";
			this.TB_Path.SelectionLength = 0;
			this.TB_Path.SelectionStart = 0;
			this.TB_Path.Size = new System.Drawing.Size(200, 35);
			this.TB_Path.TabIndex = 1;
			this.TB_Path.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Path.Validation = SlickControls.ValidationType.None;
			this.TB_Path.ValidationRegex = "";
			this.TB_Path.Visible = false;
			this.TB_Path.TextChanged += new System.EventHandler(this.TB_Path_TextChanged);
			this.TB_Path.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_Path_KeyPress);
			this.TB_Path.Leave += new System.EventHandler(this.TB_Path_Leave);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.L_NoResults);
			this.panel1.Controls.Add(this.FLP_Content);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 106);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(200, 0);
			this.panel1.TabIndex = 9;
			this.panel1.Click += new System.EventHandler(this.Generic_Click);
			// 
			// L_NoResults
			// 
			this.L_NoResults.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.L_NoResults.AutoSize = true;
			this.L_NoResults.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
			this.L_NoResults.Location = new System.Drawing.Point(65, 50);
			this.L_NoResults.Name = "L_NoResults";
			this.L_NoResults.Size = new System.Drawing.Size(71, 17);
			this.L_NoResults.TabIndex = 9;
			this.L_NoResults.Text = "No Results";
			this.L_NoResults.Visible = false;
			this.L_NoResults.Click += new System.EventHandler(this.Generic_Click);
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.FLP_Content;
			this.slickScroll1.Location = new System.Drawing.Point(194, 106);
			this.slickScroll1.MouseDownLocation = new System.Drawing.Point(0, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(6, 0);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 10;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// SlickLibraryViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.slickScroll1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(200, 50);
			this.Name = "SlickLibraryViewer";
			this.Size = new System.Drawing.Size(200, 106);
			this.Resize += new System.EventHandler(this.FLP_Content_Resize);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.P_Bar.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SlickControls.SlickPictureBox PB_Bar;
		private System.Windows.Forms.FlowLayoutPanel FLP_Content;
		private System.Windows.Forms.Panel P_Spacer;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private SlickControls.SlickPictureBox PB_Loader;
		private System.Windows.Forms.Panel P_Bar;
		private SlickControls.SlickPathTextBox TB_Path;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label L_NoResults;
		private SlickControls.SlickScroll slickScroll1;
	}
}
