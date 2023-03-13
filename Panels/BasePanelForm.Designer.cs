namespace SlickControls
{
	partial class BasePanelForm
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

			mouseDetector.Dispose();

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.base_P_Content = new System.Windows.Forms.Panel();
			this.base_TLP_TopButtons = new System.Windows.Forms.TableLayoutPanel();
			this.base_P_PanelContent = new SlickControls.DBPanel();
			this.base_P_Side = new System.Windows.Forms.Panel();
			this.base_TLP_Side = new SlickControls.RoundedPanel();
			this.base_P_Icon = new System.Windows.Forms.TableLayoutPanel();
			this.base_P_SideControls = new System.Windows.Forms.Panel();
			this.base_P_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).BeginInit();
			this.base_P_Content.SuspendLayout();
			this.base_TLP_TopButtons.SuspendLayout();
			this.base_P_Side.SuspendLayout();
			this.base_TLP_Side.SuspendLayout();
			this.base_P_Icon.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(217)))), ((int)(((byte)(179)))));
			this.base_P_Container.Controls.Add(this.base_P_Content);
			this.base_P_Container.Size = new System.Drawing.Size(850, 479);
			this.base_P_Container.TabIndex = 5;
			// 
			// base_PB_Icon
			// 
			this.base_PB_Icon.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.base_PB_Icon.Location = new System.Drawing.Point(66, 19);
			// 
			// base_B_Close
			// 
			this.base_B_Close.Location = new System.Drawing.Point(32, 0);
			this.base_B_Close.Padding = new System.Windows.Forms.Padding(0, 6, 6, 0);
			// 
			// base_B_Max
			// 
			this.base_B_Max.Location = new System.Drawing.Point(16, 0);
			this.base_B_Max.Margin = new System.Windows.Forms.Padding(0);
			this.base_B_Max.Padding = new System.Windows.Forms.Padding(0, 6, 6, 0);
			// 
			// base_B_Min
			// 
			this.base_B_Min.Padding = new System.Windows.Forms.Padding(0, 6, 6, 0);
			// 
			// base_P_Content
			// 
			this.base_P_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
			this.base_P_Content.Controls.Add(this.base_TLP_TopButtons);
			this.base_P_Content.Controls.Add(this.base_P_PanelContent);
			this.base_P_Content.Controls.Add(this.base_P_Side);
			this.base_P_Content.Dock = System.Windows.Forms.DockStyle.Fill;
			this.base_P_Content.Location = new System.Drawing.Point(1, 1);
			this.base_P_Content.Margin = new System.Windows.Forms.Padding(0);
			this.base_P_Content.Name = "base_P_Content";
			this.base_P_Content.Size = new System.Drawing.Size(848, 477);
			this.base_P_Content.TabIndex = 2;
			// 
			// base_TLP_TopButtons
			// 
			this.base_TLP_TopButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.base_TLP_TopButtons.AutoSize = true;
			this.base_TLP_TopButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.base_TLP_TopButtons.ColumnCount = 3;
			this.base_TLP_TopButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.base_TLP_TopButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.base_TLP_TopButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.base_TLP_TopButtons.Controls.Add(this.base_B_Min, 0, 0);
			this.base_TLP_TopButtons.Controls.Add(this.base_B_Close, 2, 0);
			this.base_TLP_TopButtons.Controls.Add(this.base_B_Max, 1, 0);
			this.base_TLP_TopButtons.Location = new System.Drawing.Point(800, 0);
			this.base_TLP_TopButtons.Name = "base_TLP_TopButtons";
			this.base_TLP_TopButtons.RowCount = 1;
			this.base_TLP_TopButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.base_TLP_TopButtons.Size = new System.Drawing.Size(48, 16);
			this.base_TLP_TopButtons.TabIndex = 10;
			// 
			// base_P_PanelContent
			// 
			this.base_P_PanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.base_P_PanelContent.Location = new System.Drawing.Point(165, 0);
			this.base_P_PanelContent.Name = "base_P_PanelContent";
			this.base_P_PanelContent.Size = new System.Drawing.Size(683, 477);
			this.base_P_PanelContent.TabIndex = 1;
			this.base_P_PanelContent.Paint += new System.Windows.Forms.PaintEventHandler(this.base_P_PanelContent_Paint);
			// 
			// base_P_Side
			// 
			this.base_P_Side.Controls.Add(this.base_TLP_Side);
			this.base_P_Side.Dock = System.Windows.Forms.DockStyle.Left;
			this.base_P_Side.Location = new System.Drawing.Point(0, 0);
			this.base_P_Side.Name = "base_P_Side";
			this.base_P_Side.Size = new System.Drawing.Size(165, 477);
			this.base_P_Side.TabIndex = 0;
			// 
			// base_TLP_Side
			// 
			this.base_TLP_Side.Controls.Add(this.base_P_Icon);
			this.base_TLP_Side.Controls.Add(this.base_P_SideControls);
			this.base_TLP_Side.Dock = System.Windows.Forms.DockStyle.Fill;
			this.base_TLP_Side.Location = new System.Drawing.Point(0, 0);
			this.base_TLP_Side.Name = "base_TLP_Side";
			this.base_TLP_Side.Size = new System.Drawing.Size(165, 477);
			this.base_TLP_Side.TabIndex = 13;
			// 
			// base_P_Icon
			// 
			this.base_P_Icon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.base_P_Icon.Controls.Add(this.base_PB_Icon);
			this.base_P_Icon.Dock = System.Windows.Forms.DockStyle.Top;
			this.base_P_Icon.Location = new System.Drawing.Point(0, 0);
			this.base_P_Icon.Margin = new System.Windows.Forms.Padding(0);
			this.base_P_Icon.Name = "base_P_Icon";
			this.base_P_Icon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.base_P_Icon.Size = new System.Drawing.Size(165, 70);
			this.base_P_Icon.TabIndex = 9;
			// 
			// base_P_SideControls
			// 
			this.base_P_SideControls.AutoSize = true;
			this.base_P_SideControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.base_P_SideControls.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.base_P_SideControls.Location = new System.Drawing.Point(0, 467);
			this.base_P_SideControls.Margin = new System.Windows.Forms.Padding(0);
			this.base_P_SideControls.MinimumSize = new System.Drawing.Size(0, 10);
			this.base_P_SideControls.Name = "base_P_SideControls";
			this.base_P_SideControls.Size = new System.Drawing.Size(165, 10);
			this.base_P_SideControls.TabIndex = 12;
			// 
			// BasePanelForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(861, 490);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(50)))), ((int)(((byte)(59)))));
			this.KeyPreview = true;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 1920, 1080);
			this.Name = "BasePanelForm";
			this.Text = "Form";
			this.base_P_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).EndInit();
			this.base_P_Content.ResumeLayout(false);
			this.base_P_Content.PerformLayout();
			this.base_TLP_TopButtons.ResumeLayout(false);
			this.base_P_Side.ResumeLayout(false);
			this.base_TLP_Side.ResumeLayout(false);
			this.base_TLP_Side.PerformLayout();
			this.base_P_Icon.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		protected System.Windows.Forms.Panel base_P_Content;
		private System.Windows.Forms.Panel base_P_Side;
		private System.Windows.Forms.TableLayoutPanel base_P_Icon;
		private DBPanel base_P_PanelContent;
		private System.Windows.Forms.TableLayoutPanel base_TLP_TopButtons;
		protected System.Windows.Forms.Panel base_P_SideControls;
		private RoundedPanel base_TLP_Side;
	}
}