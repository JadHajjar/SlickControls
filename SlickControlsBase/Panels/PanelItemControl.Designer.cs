namespace SlickControls
{
	partial class PanelItemControl
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
			this.SuspendLayout();
			// 
			// PanelItemControl
			// 
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "PanelItemControl";
			this.Size = new System.Drawing.Size(165, 26);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelItemControl_Paint);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PanelItemControl_MouseClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelItemControl_MouseDown);
			this.MouseEnter += new System.EventHandler(this.PanelItemControl_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.PanelItemControl_MouseLeave);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelItemControl_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
