namespace SlickControls
{
	partial class SlickDropdown
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
			// SlickDropdown
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.LabelText = "Dropdown";
			this.Name = "SlickDropdown";
			this.IconClicked += new System.EventHandler(this.PB_Arrow_Click);
			this.Load += new System.EventHandler(this.ActiveDropDown_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}
