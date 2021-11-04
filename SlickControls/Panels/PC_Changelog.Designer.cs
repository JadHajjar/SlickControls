namespace SlickControls
{
	partial class PC_Changelog
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
			this.TLP_Mainzs = new System.Windows.Forms.TableLayoutPanel();
			this.P_Spacer = new SlickControls.DBPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.P_LeftTabs = new System.Windows.Forms.Panel();
			this.verticalScroll1 = new SlickControls.SlickScroll();
			this.verticalScroll2 = new SlickControls.SlickScroll();
			this.P_VersionInfo = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.TLP_Mainzs.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// TLP_Mainzs
			// 
			this.TLP_Mainzs.ColumnCount = 3;
			this.TLP_Mainzs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
			this.TLP_Mainzs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
			this.TLP_Mainzs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Mainzs.Controls.Add(this.panel2, 2, 1);
			this.TLP_Mainzs.Controls.Add(this.P_Spacer, 1, 1);
			this.TLP_Mainzs.Controls.Add(this.panel1, 0, 1);
			this.TLP_Mainzs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Mainzs.Location = new System.Drawing.Point(0, 0);
			this.TLP_Mainzs.Name = "TLP_Mainzs";
			this.TLP_Mainzs.RowCount = 2;
			this.TLP_Mainzs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
			this.TLP_Mainzs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Mainzs.Size = new System.Drawing.Size(783, 438);
			this.TLP_Mainzs.TabIndex = 13;
			// 
			// P_Spacer
			// 
			this.P_Spacer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Spacer.Location = new System.Drawing.Point(175, 1);
			this.P_Spacer.Margin = new System.Windows.Forms.Padding(0);
			this.P_Spacer.Name = "P_Spacer";
			this.P_Spacer.Size = new System.Drawing.Size(1, 437);
			this.P_Spacer.TabIndex = 37;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.P_LeftTabs);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 31);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(175, 407);
			this.panel1.TabIndex = 40;
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
			// verticalScroll1
			// 
			this.verticalScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.verticalScroll1.LinkedControl = this.P_VersionInfo;
			this.verticalScroll1.Location = new System.Drawing.Point(779, 30);
			this.verticalScroll1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll1.Name = "verticalScroll1";
			this.verticalScroll1.Size = new System.Drawing.Size(5, 407);
			this.verticalScroll1.Style = SlickControls.StyleType.Vertical;
			this.verticalScroll1.TabIndex = 14;
			// 
			// verticalScroll2
			// 
			this.verticalScroll2.Dock = System.Windows.Forms.DockStyle.Left;
			this.verticalScroll2.LinkedControl = this.P_LeftTabs;
			this.verticalScroll2.Location = new System.Drawing.Point(389, 69);
			this.verticalScroll2.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll2.Name = "verticalScroll2";
			this.verticalScroll2.ShowHandle = false;
			this.verticalScroll2.Visible = false;
			this.verticalScroll2.Size = new System.Drawing.Size(5, 300);
			this.verticalScroll2.Style = SlickControls.StyleType.Vertical;
			this.verticalScroll2.TabIndex = 41;
			// 
			// P_VersionInfo
			// 
			this.P_VersionInfo.AutoSize = true;
			this.P_VersionInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_VersionInfo.Location = new System.Drawing.Point(0, -30);
			this.P_VersionInfo.MaximumSize = new System.Drawing.Size(600, 9999);
			this.P_VersionInfo.MinimumSize = new System.Drawing.Size(600, 0);
			this.P_VersionInfo.Name = "P_VersionInfo";
			this.P_VersionInfo.Size = new System.Drawing.Size(600, 0);
			this.P_VersionInfo.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.P_VersionInfo);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(176, 1);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(607, 437);
			this.panel2.TabIndex = 41;
			// 
			// PC_Changelog
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.verticalScroll2);
			this.Controls.Add(this.verticalScroll1);
			this.Controls.Add(this.TLP_Mainzs);
			this.Name = "PC_Changelog";
			this.Padding = new System.Windows.Forms.Padding(0);
			this.Text = "Change Log";
			this.Resize += new System.EventHandler(this.PC_Changelog_Resize);
			this.Controls.SetChildIndex(this.TLP_Mainzs, 0);
			this.Controls.SetChildIndex(this.verticalScroll1, 0);
			this.Controls.SetChildIndex(this.verticalScroll2, 0);
			this.TLP_Mainzs.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TLP_Mainzs;
		private SlickControls.DBPanel P_Spacer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel P_LeftTabs;
		private SlickControls.SlickScroll verticalScroll1;
		private SlickControls.SlickScroll verticalScroll2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel P_VersionInfo;
	}
}
