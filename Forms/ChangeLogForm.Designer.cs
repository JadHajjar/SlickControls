namespace SlickControls
{
	partial class ChangeLogForm
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
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.TLP_Mainzs = new System.Windows.Forms.TableLayoutPanel();
			this.P_Spacer = new SlickControls.DBPanel();
			this.B_Done = new SlickControls.SlickButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.P_LeftTabs = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.P_VersionInfo = new System.Windows.Forms.Panel();
			this.verticalScroll1 = new SlickControls.SlickScroll();
			this.verticalScroll2 = new SlickControls.SlickScroll();
			this.base_P_Content.SuspendLayout();
			this.TLP_Mainzs.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(48)))), ((int)(((byte)(58)))));
			this.base_P_Content.Controls.Add(this.verticalScroll2);
			this.base_P_Content.Controls.Add(this.verticalScroll1);
			this.base_P_Content.Controls.Add(this.TLP_Mainzs);
			this.base_P_Content.Size = new System.Drawing.Size(781, 404);
			// 
			// base_P_Controls
			// 
			this.base_P_Controls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
			this.base_P_Controls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.base_P_Controls.Size = new System.Drawing.Size(781, 45);
			// 
			// base_P_Top_Spacer
			// 
			this.base_P_Top_Spacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(73)))), ((int)(((byte)(89)))));
			this.base_P_Top_Spacer.Size = new System.Drawing.Size(781, 2);
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 20000;
			this.toolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.toolTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(65)))), ((int)(((byte)(77)))));
			this.toolTip.InitialDelay = 600;
			this.toolTip.ReshowDelay = 100;
			// 
			// TLP_Mainzs
			// 
			this.TLP_Mainzs.ColumnCount = 4;
			this.TLP_Mainzs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.TLP_Mainzs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
			this.TLP_Mainzs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Mainzs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
			this.TLP_Mainzs.Controls.Add(this.P_Spacer, 1, 0);
			this.TLP_Mainzs.Controls.Add(this.B_Done, 1, 1);
			this.TLP_Mainzs.Controls.Add(this.panel1, 0, 0);
			this.TLP_Mainzs.Controls.Add(this.panel2, 2, 0);
			this.TLP_Mainzs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Mainzs.Location = new System.Drawing.Point(0, 0);
			this.TLP_Mainzs.Name = "TLP_Mainzs";
			this.TLP_Mainzs.RowCount = 2;
			this.TLP_Mainzs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Mainzs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.TLP_Mainzs.Size = new System.Drawing.Size(781, 404);
			this.TLP_Mainzs.TabIndex = 1;
			// 
			// P_Spacer
			// 
			this.P_Spacer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Spacer.Location = new System.Drawing.Point(150, 0);
			this.P_Spacer.Margin = new System.Windows.Forms.Padding(0);
			this.P_Spacer.Name = "P_Spacer";
			this.P_Spacer.Size = new System.Drawing.Size(1, 364);
			this.P_Spacer.TabIndex = 37;
			this.P_Spacer.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Spacer_Paint);
			// 
			// B_Done
			// 
			this.B_Done.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.B_Done.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Done.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Done.ColorShade = null;
			this.TLP_Mainzs.SetColumnSpan(this.B_Done, 2);
			this.B_Done.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Done.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.B_Done.Image = global::SlickControls.Properties.Resources.I_Ok_16;
			this.B_Done.Location = new System.Drawing.Point(325, 370);
			this.B_Done.Margin = new System.Windows.Forms.Padding(0);
			this.B_Done.Name = "B_Done";
			this.B_Done.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Done.Size = new System.Drawing.Size(105, 28);
			this.B_Done.TabIndex = 3;
			this.B_Done.Text = "Dismiss";
			this.B_Done.Click += new System.EventHandler(this.B_Done_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.P_LeftTabs);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.TLP_Mainzs.SetRowSpan(this.panel1, 2);
			this.panel1.Size = new System.Drawing.Size(150, 404);
			this.panel1.TabIndex = 40;
			this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
			// 
			// P_LeftTabs
			// 
			this.P_LeftTabs.AutoSize = true;
			this.P_LeftTabs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_LeftTabs.Location = new System.Drawing.Point(0, 0);
			this.P_LeftTabs.Margin = new System.Windows.Forms.Padding(0);
			this.P_LeftTabs.MaximumSize = new System.Drawing.Size(175, 9999);
			this.P_LeftTabs.MinimumSize = new System.Drawing.Size(175, 0);
			this.P_LeftTabs.Name = "P_LeftTabs";
			this.P_LeftTabs.Size = new System.Drawing.Size(175, 0);
			this.P_LeftTabs.TabIndex = 41;
			// 
			// panel2
			// 
			this.TLP_Mainzs.SetColumnSpan(this.panel2, 2);
			this.panel2.Controls.Add(this.P_VersionInfo);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(151, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(630, 364);
			this.panel2.TabIndex = 41;
			this.panel2.Resize += new System.EventHandler(this.panel2_Resize);
			// 
			// P_VersionInfo
			// 
			this.P_VersionInfo.AutoSize = true;
			this.P_VersionInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_VersionInfo.Location = new System.Drawing.Point(0, 0);
			this.P_VersionInfo.MaximumSize = new System.Drawing.Size(600, 9999);
			this.P_VersionInfo.MinimumSize = new System.Drawing.Size(600, 0);
			this.P_VersionInfo.Name = "P_VersionInfo";
			this.P_VersionInfo.Size = new System.Drawing.Size(600, 0);
			this.P_VersionInfo.TabIndex = 0;
			// 
			// verticalScroll1
			// 
			this.verticalScroll1.Dock = System.Windows.Forms.DockStyle.Left;
			this.verticalScroll1.LinkedControl = this.P_LeftTabs;
			this.verticalScroll1.Location = new System.Drawing.Point(0, 22);
			this.verticalScroll1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll1.Name = "verticalScroll1";
			this.verticalScroll1.Size = new System.Drawing.Size(4, 276);
			this.verticalScroll1.TabIndex = 39;
			// 
			// verticalScroll2
			// 
			this.verticalScroll2.Dock = System.Windows.Forms.DockStyle.Right;
			this.verticalScroll2.LinkedControl = this.P_VersionInfo;
			this.verticalScroll2.Location = new System.Drawing.Point(397, 1);
			this.verticalScroll2.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll2.Name = "verticalScroll2";
			this.verticalScroll2.Size = new System.Drawing.Size(12, 372);
			this.verticalScroll2.TabIndex = 40;
			// 
			// ChangeLogForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.FormIcon = global::SlickControls.Properties.Resources.Tiny_Log;
			this.IconBounds = new System.Drawing.Rectangle(5, 5, 16, 16);
			this.MinimumSize = new System.Drawing.Size(450, 300);
			this.Name = "ChangeLogForm";
			this.Text = "Change-Log";
			this.base_P_Content.ResumeLayout(false);
			this.TLP_Mainzs.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TableLayoutPanel TLP_Mainzs;
		private SlickControls.SlickButton B_Done;
		private SlickControls.DBPanel P_Spacer;
		private System.Windows.Forms.Panel P_VersionInfo;
		private System.Windows.Forms.Panel P_LeftTabs;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private SlickControls.SlickScroll verticalScroll1;
		private SlickControls.SlickScroll verticalScroll2;
	}
}