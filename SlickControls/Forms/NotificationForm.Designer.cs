namespace SlickControls
{
	partial class NotificationForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PictureBox = new SlickControls.SlickPictureBox();

			this.SuspendLayout();
			// 
			// PictureBox
			// 
			this.PictureBox.Location = new System.Drawing.Point(0, 0);
			this.PictureBox.Name = "PictureBox";
			this.PictureBox.Size = new System.Drawing.Size(300, 70);
			this.PictureBox.TabIndex = 0;
			this.PictureBox.UserDraw = true;
			this.PictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.NotificationForm_Paint);
			this.PictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotificationForm_MouseClick);
			this.PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NotificationForm_MouseMove);
			// 
			// NotificationForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 70);
			this.Controls.Add(this.PictureBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "NotificationForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Notification";
			this.Activated += new System.EventHandler(this.NotificationForm_Activated);
			this.Load += new System.EventHandler(this.NotificationForm_Load);

			this.ResumeLayout(false);

		}

		#endregion

		public SlickPictureBox PictureBox;
	}
}