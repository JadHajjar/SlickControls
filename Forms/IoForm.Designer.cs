namespace SlickControls.Forms
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
			this.L_Side = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.L_Title = new System.Windows.Forms.Label();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.B_Cancel = new SlickControls.SlickButton();
			this.B_OK = new SlickControls.SlickButton();
			this.roundedPanel1 = new SlickControls.RoundedPanel();
			this.libraryViewer = new SlickControls.SlickLibraryViewer();
			this.base_P_Content.SuspendLayout();
			this.base_P_Container.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.roundedPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
			this.base_P_Content.Controls.Add(this.tableLayoutPanel1);
			this.base_P_Content.Size = new System.Drawing.Size(789, 437);
			this.base_P_Content.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			// 
			// base_P_SideControls
			// 
			this.base_P_SideControls.Font = new System.Drawing.Font("Nirmala UI", 6.75F);
			this.base_P_SideControls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(129)))), ((int)(((byte)(150)))));
			this.base_P_SideControls.Location = new System.Drawing.Point(5, 412);
			this.base_P_SideControls.Size = new System.Drawing.Size(150, 10);
			// 
			// base_P_Container
			// 
			this.base_P_Container.Size = new System.Drawing.Size(791, 439);
			// 
			// L_Side
			// 
			this.L_Side.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.L_Side.AutoSize = true;
			this.L_Side.Location = new System.Drawing.Point(1, 0);
			this.L_Side.Margin = new System.Windows.Forms.Padding(7);
			this.L_Side.Name = "L_Side";
			this.L_Side.Size = new System.Drawing.Size(84, 13);
			this.L_Side.TabIndex = 6;
			this.L_Side.Text = "Pinned Folders";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.B_Cancel, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.B_OK, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.TB_Search, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.L_Title, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.roundedPanel1, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(165, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(624, 437);
			this.tableLayoutPanel1.TabIndex = 11;
			// 
			// L_Title
			// 
			this.L_Title.AutoSize = true;
			this.L_Title.Location = new System.Drawing.Point(15, 7);
			this.L_Title.Margin = new System.Windows.Forms.Padding(15, 7, 7, 0);
			this.L_Title.Name = "L_Title";
			this.L_Title.Size = new System.Drawing.Size(84, 13);
			this.L_Title.TabIndex = 1;
			this.L_Title.Text = "Pinned Folders";
			// 
			// TB_Search
			// 
			this.TB_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.TB_Search.EnterTriggersClick = false;
			this.TB_Search.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Search.LabelText = "";
			this.TB_Search.Location = new System.Drawing.Point(15, 20);
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
			this.TB_Search.TabIndex = 2;
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.ColorStyle = Extensions.ColorStyle.Red;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Image = global::SlickControls.Properties.Resources.Tiny_Cancel;
			this.B_Cancel.Location = new System.Drawing.Point(517, 400);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(7);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Cancel.Size = new System.Drawing.Size(100, 30);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 8;
			this.B_Cancel.Text = "CANCEL";
			// 
			// B_OK
			// 
			this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_OK.ColorShade = null;
			this.B_OK.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OK.Image = global::SlickControls.Properties.Resources.Tiny_Ok;
			this.B_OK.Location = new System.Drawing.Point(403, 400);
			this.B_OK.Margin = new System.Windows.Forms.Padding(7);
			this.B_OK.Name = "B_OK";
			this.B_OK.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_OK.Size = new System.Drawing.Size(100, 30);
			this.B_OK.SpaceTriggersClick = true;
			this.B_OK.TabIndex = 7;
			this.B_OK.Text = "SELECT";
			// 
			// roundedPanel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.roundedPanel1, 3);
			this.roundedPanel1.Controls.Add(this.libraryViewer);
			this.roundedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.roundedPanel1.Location = new System.Drawing.Point(3, 51);
			this.roundedPanel1.Name = "roundedPanel1";
			this.roundedPanel1.Size = new System.Drawing.Size(618, 339);
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
			this.libraryViewer.Size = new System.Drawing.Size(618, 339);
			this.libraryViewer.StartingFolder = null;
			this.libraryViewer.TabIndex = 4;
			this.libraryViewer.TopFolders = new string[0];
			// 
			// IoForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(802, 450);
			this.Controls.Add(this.L_Side);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.IconBounds = new System.Drawing.Rectangle(59, 19, 32, 32);
			this.MaximizeBox = true;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 1920, 1032);
			this.MinimizeBox = true;
			this.Name = "IoForm";
			this.SidebarItems = new SlickControls.PanelItem[0];
			this.Text = "IoForm";
			this.Controls.SetChildIndex(this.base_P_Container, 0);
			this.Controls.SetChildIndex(this.L_Side, 0);
			this.base_P_Content.ResumeLayout(false);
			this.base_P_Content.PerformLayout();
			this.base_P_Container.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.roundedPanel1.ResumeLayout(false);
			this.roundedPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.Label L_Side;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		protected System.Windows.Forms.Label L_Title;
		protected SlickTextBox TB_Search;
		protected SlickButton B_Cancel;
		protected SlickButton B_OK;
		private RoundedPanel roundedPanel1;
		protected SlickLibraryViewer libraryViewer;
	}
}