namespace SlickControls
{
	partial class SlickProgressBar
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
			// SlickProgressBar
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Name = "SlickProgressBar";
			this.Size = new System.Drawing.Size(500, 28);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SlickProgressBar_Paint);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
