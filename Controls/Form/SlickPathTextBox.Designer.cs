namespace SlickControls
{
	partial class SlickPathTextBox
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
			this.ioSelectionDialog = new SlickControls.IOSelectionDialog();
			this.SuspendLayout();
			// 
			// P_Bar
			// 
			// ioSelectionDialog
			// 
			// 
			// SlickPathTextBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.LabelText = "Folder";
			this.Name = "SlickPathTextBox";
			this.Placeholder = "Folder Path";
			this.Size = new System.Drawing.Size(200, 35);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private IOSelectionDialog ioSelectionDialog;
	}
}
