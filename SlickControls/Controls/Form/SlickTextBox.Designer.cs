namespace SlickControls
{
	partial class SlickTextBox
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.PB = new SlickPictureBox();
			this.L_Placerholder = new System.Windows.Forms.Label();
			this.TB = new System.Windows.Forms.MaskedTextBox();
			this.label = new System.Windows.Forms.Label();
			this.P_Bar = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();

			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.PB);
			this.panel1.Controls.Add(this.L_Placerholder);
			this.panel1.Controls.Add(this.TB);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 16);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
			this.panel1.Size = new System.Drawing.Size(278, 23);
			this.panel1.TabIndex = 8;
			this.panel1.Click += new System.EventHandler(this.L_Placerholder_Click);
			// 
			// PB
			// 
			this.PB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.PB.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PB.Location = new System.Drawing.Point(259, 5);
			this.PB.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
			this.PB.Name = "PB";
			this.PB.Size = new System.Drawing.Size(16, 16);
			this.PB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PB.TabIndex = 10;
			this.PB.TabStop = false;
			this.PB.Visible = false;
			this.PB.Click += new System.EventHandler(this.PB_Click);
			// 
			// L_Placerholder
			// 
			this.L_Placerholder.AutoSize = true;
			this.L_Placerholder.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.L_Placerholder.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Italic);
			this.L_Placerholder.Location = new System.Drawing.Point(3, 5);
			this.L_Placerholder.Name = "L_Placerholder";
			this.L_Placerholder.Size = new System.Drawing.Size(73, 15);
			this.L_Placerholder.TabIndex = 6;
			this.L_Placerholder.Text = "Placeholder";
			this.L_Placerholder.Click += new System.EventHandler(this.L_Placerholder_Click);
			// 
			// TB
			// 
			this.TB.BackColor = System.Drawing.SystemColors.Control;
			this.TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TB.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.TB.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
			this.TB.Location = new System.Drawing.Point(0, 8);
			this.TB.Margin = new System.Windows.Forms.Padding(0);
			this.TB.Name = "TB";
			this.TB.Size = new System.Drawing.Size(258, 15);
			this.TB.TabIndex = 5;
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Dock = System.Windows.Forms.DockStyle.Top;
			this.label.Font = new System.Drawing.Font("Century Gothic", 7.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
			this.label.Location = new System.Drawing.Point(0, 0);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(45, 14);
			this.label.TabIndex = 4;
			this.label.Text = "Textbox";
			this.label.Click += new System.EventHandler(this.L_Placerholder_Click);
			// 
			// P_Bar
			// 
			this.P_Bar.BackColor = System.Drawing.SystemColors.ControlDark;
			this.P_Bar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.P_Bar.Location = new System.Drawing.Point(0, 39);
			this.P_Bar.Margin = new System.Windows.Forms.Padding(0);
			this.P_Bar.Name = "P_Bar";
			this.P_Bar.Size = new System.Drawing.Size(278, 1);
			this.P_Bar.TabIndex = 3;
			// 
			// SlickTextBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label);
			this.Controls.Add(this.P_Bar);
			this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximumSize = new System.Drawing.Size(9999, 0);
			this.MinimumSize = new System.Drawing.Size(50, 0);
			this.EnterTriggersClick = false;
			this.Name = "SlickTextBox";
			this.EnterTriggersClick = false;
			this.Size = new System.Drawing.Size(278, 40);
			this.Load += new System.EventHandler(this.SlickTextBox_Load);
			this.BackColorChanged += new System.EventHandler(this.SlickTextBox_BackColorChanged);
			this.Click += new System.EventHandler(this.L_Placerholder_Click);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();

			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		protected System.Windows.Forms.Panel P_Bar;
		protected System.Windows.Forms.Label label;
		protected System.Windows.Forms.MaskedTextBox TB;
		protected System.Windows.Forms.Label L_Placerholder;
		private System.Windows.Forms.Panel panel1;
		protected SlickPictureBox PB;
	}
}
