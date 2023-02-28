namespace SlickControls
{
	partial class SlickScroll
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
			if (disposing)
			{
				ScrollTimer?.Dispose();
				animationHandler?.Dispose();
				components?.Dispose();
				MouseDetector?.Dispose();

				lock (activeScrolls)
					activeScrolls.Remove(this);
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
			// SlickScroll
			// 
			this.Size = new System.Drawing.Size(12, 137);
			this.Dock = System.Windows.Forms.DockStyle.Right;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SlickScroll_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SlickScroll_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SlickScroll_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion

	}
}
