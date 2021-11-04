namespace SlickControls
{
	partial class SideBarPanelContent
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
			this.base_P_Sidebar = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// base_P_Sidebar
			// 
			this.base_P_Sidebar.Dock = System.Windows.Forms.DockStyle.Left;
			this.base_P_Sidebar.Location = new System.Drawing.Point(0, 30);
			this.base_P_Sidebar.Name = "base_P_Sidebar";
			this.base_P_Sidebar.Size = new System.Drawing.Size(150, 408);
			this.base_P_Sidebar.TabIndex = 13;
			// 
			// SideBarPanelContent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.base_P_Sidebar);
			this.Name = "SideBarPanelContent";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SideBarPanelContent_Paint);
			this.Controls.SetChildIndex(this.base_P_Sidebar, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel base_P_Sidebar;
	}
}
