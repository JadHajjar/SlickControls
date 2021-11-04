namespace SlickControls
{
	partial class DropDownItems
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
			this.panel = new System.Windows.Forms.Panel();
			this.scroll = new SlickControls.SlickScroll();
			this.P_Items = new SlickControls.DBPanel();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.Controls.Add(this.scroll);
			this.panel.Controls.Add(this.P_Items);
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(1, 1);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(218, 248);
			this.panel.TabIndex = 0;
			// 
			// scroll
			// 
			this.scroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.scroll.LinkedControl = this.P_Items;
			this.scroll.Location = new System.Drawing.Point(213, 0);
			this.scroll.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.scroll.MouseDownLocation = new System.Drawing.Point(0, 0);
			this.scroll.Name = "scroll";
			this.scroll.Size = new System.Drawing.Size(5, 248);
			this.scroll.SmallHandle = true;
			this.scroll.Style = SlickControls.StyleType.Vertical;
			this.scroll.TabIndex = 1;
			// 
			// P_Items
			// 
			this.P_Items.Location = new System.Drawing.Point(0, 0);
			this.P_Items.MaximumSize = new System.Drawing.Size(218, 2147483647);
			this.P_Items.MinimumSize = new System.Drawing.Size(218, 0);
			this.P_Items.Name = "P_Items";
			this.P_Items.Size = new System.Drawing.Size(218, 0);
			this.P_Items.TabIndex = 0;
			this.P_Items.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Items_Paint);
			this.P_Items.MouseClick += new System.Windows.Forms.MouseEventHandler(this.P_Items_MouseClick);
			// 
			// DropDownItems
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(162)))), ((int)(((byte)(229)))));
			this.Controls.Add(this.panel);
			this.Name = "DropDownItems";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(220, 250);
			this.panel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel;
		private SlickControls.SlickScroll scroll;
		private SlickControls.DBPanel P_Items;
	}
}