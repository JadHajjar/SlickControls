
namespace SlickControls
{
	partial class ProccessPrompt
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
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.P_Spacer_1 = new System.Windows.Forms.Panel();
			this.FLP_Buttons = new System.Windows.Forms.FlowLayoutPanel();
			this.B_Cancel = new SlickControls.SlickButton();
			this.TLP_ImgText = new System.Windows.Forms.TableLayoutPanel();
			this.L_Text = new System.Windows.Forms.Label();
			this.PB_Icon = new SlickControls.SlickPictureBox();
			this.base_P_Content = new System.Windows.Forms.Panel();
			this.base_P_Container.SuspendLayout();




			this.TLP_Main.SuspendLayout();
			this.FLP_Buttons.SuspendLayout();
			this.TLP_ImgText.SuspendLayout();

			this.base_P_Content.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(217)))), ((int)(((byte)(179)))));
			this.base_P_Container.Controls.Add(this.base_P_Content);
			this.base_P_Container.Location = new System.Drawing.Point(7, 7);
			this.base_P_Container.Size = new System.Drawing.Size(368, 199);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 1;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Controls.Add(this.P_Spacer_1, 0, 1);
			this.TLP_Main.Controls.Add(this.FLP_Buttons, 0, 2);
			this.TLP_Main.Controls.Add(this.TLP_ImgText, 0, 0);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(0, 0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 3;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.TLP_Main.Size = new System.Drawing.Size(366, 197);
			this.TLP_Main.TabIndex = 0;
			// 
			// P_Spacer_1
			// 
			this.P_Spacer_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(49)))), ((int)(((byte)(64)))));
			this.P_Spacer_1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Spacer_1.Location = new System.Drawing.Point(50, 154);
			this.P_Spacer_1.Margin = new System.Windows.Forms.Padding(50, 0, 50, 0);
			this.P_Spacer_1.Name = "P_Spacer_1";
			this.P_Spacer_1.Size = new System.Drawing.Size(266, 1);
			this.P_Spacer_1.TabIndex = 12;
			// 
			// FLP_Buttons
			// 
			this.FLP_Buttons.Controls.Add(this.B_Cancel);
			this.FLP_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FLP_Buttons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.FLP_Buttons.Location = new System.Drawing.Point(0, 155);
			this.FLP_Buttons.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Buttons.Name = "FLP_Buttons";
			this.FLP_Buttons.Size = new System.Drawing.Size(366, 42);
			this.FLP_Buttons.TabIndex = 2;
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
			this.B_Cancel.IconSize = 16;
			this.B_Cancel.Image = global::SlickControls.Properties.Resources.Tiny_Cancel;
			this.B_Cancel.Location = new System.Drawing.Point(271, 7);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Cancel.Size = new System.Drawing.Size(90, 28);
			this.B_Cancel.TabIndex = 0;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.Visible = false;
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// TLP_ImgText
			// 
			this.TLP_ImgText.ColumnCount = 2;
			this.TLP_ImgText.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.TLP_ImgText.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_ImgText.Controls.Add(this.L_Text, 1, 0);
			this.TLP_ImgText.Controls.Add(this.PB_Icon, 0, 0);
			this.TLP_ImgText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_ImgText.Location = new System.Drawing.Point(3, 3);
			this.TLP_ImgText.MinimumSize = new System.Drawing.Size(0, 100);
			this.TLP_ImgText.Name = "TLP_ImgText";
			this.TLP_ImgText.RowCount = 1;
			this.TLP_ImgText.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_ImgText.Size = new System.Drawing.Size(360, 148);
			this.TLP_ImgText.TabIndex = 0;
			// 
			// L_Text
			// 
			this.L_Text.AutoEllipsis = true;
			this.L_Text.Dock = System.Windows.Forms.DockStyle.Fill;
			this.L_Text.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
			this.L_Text.Location = new System.Drawing.Point(95, 15);
			this.L_Text.Margin = new System.Windows.Forms.Padding(15);
			this.L_Text.Name = "L_Text";
			this.L_Text.Size = new System.Drawing.Size(250, 118);
			this.L_Text.TabIndex = 0;
			this.L_Text.Text = "Text";
			this.L_Text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PB_Icon
			// 
			this.PB_Icon.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.PB_Icon.Image = null;
			this.PB_Icon.Loading = true;
			this.PB_Icon.Location = new System.Drawing.Point(19, 53);
			this.PB_Icon.Name = "PB_Icon";
			this.PB_Icon.Size = new System.Drawing.Size(42, 42);
			this.PB_Icon.TabIndex = 1;
			this.PB_Icon.TabStop = false;
			// 
			// base_P_Content
			// 
			this.base_P_Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
			this.base_P_Content.Controls.Add(this.TLP_Main);
			this.base_P_Content.Dock = System.Windows.Forms.DockStyle.Fill;
			this.base_P_Content.Location = new System.Drawing.Point(1, 1);
			this.base_P_Content.Margin = new System.Windows.Forms.Padding(0);
			this.base_P_Content.Name = "base_P_Content";
			this.base_P_Content.Size = new System.Drawing.Size(366, 197);
			this.base_P_Content.TabIndex = 2;
			// 
			// ProccessPrompt
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(385, 216);
			this.Font = new System.Drawing.Font("Century Gothic", 9.75F);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(325, 180);
			this.Name = "ProccessPrompt";
			this.Padding = new System.Windows.Forms.Padding(7, 7, 10, 10);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "MessagePrompt";
			this.base_P_Container.ResumeLayout(false);




			this.TLP_Main.ResumeLayout(false);
			this.FLP_Buttons.ResumeLayout(false);
			this.TLP_ImgText.ResumeLayout(false);

			this.base_P_Content.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.FlowLayoutPanel FLP_Buttons;
		private SlickControls.SlickButton B_Cancel;
		private SlickControls.SlickPictureBox PB_Icon;
		private System.Windows.Forms.Panel P_Spacer_1;
		internal System.Windows.Forms.TableLayoutPanel TLP_Main;
		internal System.Windows.Forms.TableLayoutPanel TLP_ImgText;
		internal System.Windows.Forms.Label L_Text;
		protected System.Windows.Forms.Panel base_P_Content;
	}
}