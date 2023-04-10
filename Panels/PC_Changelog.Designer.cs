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
			this.P_Content = new System.Windows.Forms.Panel();
			this.P_VersionInfo = new System.Windows.Forms.Panel();
			this.base_P_Side = new System.Windows.Forms.Panel();
			this.base_TLP_Side = new SlickControls.RoundedPanel();
			this.P_All = new System.Windows.Forms.Panel();
			this.verticalScroll1 = new SlickControls.SlickScroll();
			this.P_Content.SuspendLayout();
			this.base_P_Side.SuspendLayout();
			this.P_All.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(92, 26);
			this.base_Text.Text = "Change Log";
			// 
			// P_Content
			// 
			this.P_Content.Controls.Add(this.P_VersionInfo);
			this.P_Content.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Content.Location = new System.Drawing.Point(0, 0);
			this.P_Content.Margin = new System.Windows.Forms.Padding(0);
			this.P_Content.Name = "P_Content";
			this.P_Content.Size = new System.Drawing.Size(618, 438);
			this.P_Content.TabIndex = 41;
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
			// base_P_Side
			// 
			this.base_P_Side.Controls.Add(this.base_TLP_Side);
			this.base_P_Side.Dock = System.Windows.Forms.DockStyle.Left;
			this.base_P_Side.Location = new System.Drawing.Point(0, 0);
			this.base_P_Side.Name = "base_P_Side";
			this.base_P_Side.Size = new System.Drawing.Size(165, 438);
			this.base_P_Side.TabIndex = 15;
			// 
			// base_TLP_Side
			// 
			this.base_TLP_Side.BotLeft = true;
			this.base_TLP_Side.Dock = System.Windows.Forms.DockStyle.Fill;
			this.base_TLP_Side.Location = new System.Drawing.Point(0, 0);
			this.base_TLP_Side.Name = "base_TLP_Side";
			this.base_TLP_Side.Size = new System.Drawing.Size(165, 438);
			this.base_TLP_Side.TabIndex = 43;
			this.base_TLP_Side.TopLeft = true;
			// 
			// P_All
			// 
			this.P_All.Controls.Add(this.verticalScroll1);
			this.P_All.Controls.Add(this.P_Content);
			this.P_All.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_All.Location = new System.Drawing.Point(165, 0);
			this.P_All.Name = "P_All";
			this.P_All.Size = new System.Drawing.Size(618, 438);
			this.P_All.TabIndex = 42;
			// 
			// verticalScroll1
			// 
			this.verticalScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.verticalScroll1.LinkedControl = this.P_VersionInfo;
			this.verticalScroll1.Location = new System.Drawing.Point(610, 0);
			this.verticalScroll1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll1.Name = "verticalScroll1";
			this.verticalScroll1.Size = new System.Drawing.Size(8, 438);
			this.verticalScroll1.Style = SlickControls.StyleType.Vertical;
			this.verticalScroll1.TabIndex = 15;
			this.verticalScroll1.TabStop = false;
			// 
			// PC_Changelog
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.P_All);
			this.Controls.Add(this.base_P_Side);
			this.Name = "PC_Changelog";
			this.Padding = new System.Windows.Forms.Padding(0);
			this.Text = "Change Log";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.base_P_Side, 0);
			this.Controls.SetChildIndex(this.P_All, 0);
			this.P_Content.ResumeLayout(false);
			this.P_Content.PerformLayout();
			this.base_P_Side.ResumeLayout(false);
			this.P_All.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Panel P_Content;
		private System.Windows.Forms.Panel P_VersionInfo;
		private System.Windows.Forms.Panel base_P_Side;
		internal RoundedPanel base_TLP_Side;
		private System.Windows.Forms.Panel P_All;
		private SlickScroll verticalScroll1;
	}
}
