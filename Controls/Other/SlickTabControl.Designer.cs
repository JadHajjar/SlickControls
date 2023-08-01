namespace SlickControls
{
	partial class SlickTabControl
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
			this.P_Tabs = new System.Windows.Forms.Panel();
			this.P_Content = new SlickControls.DBPanel();
			this.ScrollBar = new SlickControls.SlickScroll();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.P_Content.SuspendLayout();
			this.SuspendLayout();
			// 
			// P_Tabs
			// 
			this.P_Tabs.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Tabs.Location = new System.Drawing.Point(0, 0);
			this.P_Tabs.Name = "P_Tabs";
			this.P_Tabs.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.P_Tabs.Size = new System.Drawing.Size(612, 30);
			this.P_Tabs.TabIndex = 0;
			this.P_Tabs.PaddingChanged += new System.EventHandler(this.P_Tabs_PaddingChanged);
			// 
			// P_Content
			// 
			this.P_Content.Controls.Add(this.ScrollBar);
			this.P_Content.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Content.Location = new System.Drawing.Point(0, 31);
			this.P_Content.Name = "P_Content";
			this.P_Content.Size = new System.Drawing.Size(612, 318);
			this.P_Content.TabIndex = 1;
			this.P_Content.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Content_Paint);
			// 
			// ScrollBar
			// 
			this.ScrollBar.AutoSizeSource = true;
			this.ScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.ScrollBar.LinkedControl = null;
			this.ScrollBar.Location = new System.Drawing.Point(604, 0);
			this.ScrollBar.Name = "ScrollBar";
			this.ScrollBar.Size = new System.Drawing.Size(8, 318);
			this.ScrollBar.SmallHandle = true;
			this.ScrollBar.Style = SlickControls.StyleType.Vertical;
			this.ScrollBar.TabIndex = 0;
			this.ScrollBar.TabStop = false;
			this.ScrollBar.Text = "ScrollBar";
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(0, 30);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(612, 1);
			this.slickSpacer1.TabIndex = 2;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// SlickTabControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.P_Content);
			this.Controls.Add(this.slickSpacer1);
			this.Controls.Add(this.P_Tabs);
			this.Name = "SlickTabControl";
			this.Size = new System.Drawing.Size(612, 349);
			this.Resize += new System.EventHandler(this.SlickTabControl_Resize);
			this.P_Content.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel P_Tabs;
		private DBPanel P_Content;
		public SlickScroll ScrollBar;
		private SlickSpacer slickSpacer1;
	}
}
