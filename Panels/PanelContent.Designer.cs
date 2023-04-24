namespace SlickControls
{
	partial class PanelContent
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
			this.base_Text = new SlickControls.SlickLabel();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.AutoSize = true;
			this.base_Text.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.base_Text.Cursor = System.Windows.Forms.Cursors.Hand;
			this.base_Text.Display = true;
			this.base_Text.Enabled = false;
			this.base_Text.Font = new System.Drawing.Font("Nirmala UI", 11.25F);
			this.base_Text.Location = new System.Drawing.Point(3, 3);
			this.base_Text.Name = "base_Text";
			this.base_Text.Padding = new System.Windows.Forms.Padding(3, 2, 0, 3);
			this.base_Text.Size = new System.Drawing.Size(150, 30);
			this.base_Text.TabIndex = 12;
			this.base_Text.TabStop = false;
			this.base_Text.Text = "Panel";
			this.base_Text.Click += new System.EventHandler(this.base_Text_Click);
			// 
			// PanelContent
			// 
			this.AutoInvalidate = false;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
			this.Controls.Add(this.base_Text);
			this.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(50)))), ((int)(((byte)(59)))));
			this.Name = "PanelContent";
			this.Padding = new System.Windows.Forms.Padding(5, 30, 5, 5);
			this.Size = new System.Drawing.Size(783, 438);
			this.Load += new System.EventHandler(this.PanelContent_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelContent_Paint);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected SlickLabel base_Text;
	}
}
