namespace SlickControls
{
	partial class SlickTab
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
			// SlickTab
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SlickTab";
			this.Size = new System.Drawing.Size(150, 26);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SlickTab_Paint);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SlickTab_MouseClick);
			this.MouseEnter += new System.EventHandler(this.SlickTab_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.SlickTab_MouseLeave);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
