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
			this.P_Bar.Location = new System.Drawing.Point(0, 34);
			this.P_Bar.Size = new System.Drawing.Size(200, 1);
			// 
			// label
			// 
			this.label.Size = new System.Drawing.Size(36, 14);
			this.label.Text = "Folder";
			// 
			// TB
			// 
			this.TB.Size = new System.Drawing.Size(180, 15);
			// 
			// L_Placerholder
			// 
			this.L_Placerholder.Size = new System.Drawing.Size(70, 15);
			this.L_Placerholder.Text = "Folder Path";
			// 
			// PB
			// 
			this.PB.Image = global::SlickControls.Properties.Resources.Tiny_Search;
			this.PB.Location = new System.Drawing.Point(103, 2);
			// 
			// ioSelectionDialog
			// 
			this.ioSelectionDialog.LastFolder = null;
			this.ioSelectionDialog.ValidExtensions = null;
			// 
			// SlickPathTextBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Image = global::SlickControls.Properties.Resources.Tiny_Search;
			this.LabelText = "Folder";
			this.MinimumSize = new System.Drawing.Size(50, 35);
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
