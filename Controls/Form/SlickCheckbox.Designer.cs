using System.Windows.Forms;

namespace SlickControls
{
	partial class SlickCheckbox
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
			if (disposing && ( components != null ))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.s
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// SlickCheckbox
			// 
			this.Image = global::SlickControls.Properties.Resources.Tiny_ToggleOff;
			this.Name = "SlickCheckbox";
			this.Size = new System.Drawing.Size(92, 22);
			this.Text = "Check Box";
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SpaceTriggersClick = true;
			this.EnterTriggersClick = false;
			this.Click += new System.EventHandler(this.SlickCheckbox_Click);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
