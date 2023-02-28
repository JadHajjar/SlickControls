using System.Windows.Forms;

namespace SlickControls
{
	partial class SlickTileButton
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
			// SlickTileButton
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SlickTileButton";
			this.Size = new System.Drawing.Size(75, 75);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SlickButton_Paint);
			this.Enter += new System.EventHandler(this.SlickButton_FocusChange);
			this.Leave += new System.EventHandler(this.SlickButton_FocusChange);
			this.Resize += new System.EventHandler(this.MyButton_Resize);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
