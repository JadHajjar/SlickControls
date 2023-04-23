
namespace SlickControls
{
	partial class WorkingPrompt
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
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.P_Spacer_1 = new System.Windows.Forms.Panel();
			this.FLP_Buttons = new System.Windows.Forms.FlowLayoutPanel();
			this.B_Cancel = new SlickControls.SlickButton();
			this.TLP_ImgText = new System.Windows.Forms.TableLayoutPanel();
			this.L_Text = new System.Windows.Forms.Label();
			this.PB_Icon = new SlickControls.SlickPictureBox();
			this.TLP_Main.SuspendLayout();
			this.FLP_Buttons.SuspendLayout();
			this.TLP_ImgText.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_Icon)).BeginInit();
			this.SuspendLayout();
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 1;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Controls.Add(this.P_Spacer_1, 0, 1);
			this.TLP_Main.Controls.Add(this.FLP_Buttons, 0, 2);
			this.TLP_Main.Controls.Add(this.TLP_ImgText, 0, 0);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(1, 1);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 3;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.TLP_Main.Size = new System.Drawing.Size(328, 179);
			this.TLP_Main.TabIndex = 1;
			// 
			// P_Spacer_1
			// 
			this.P_Spacer_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(49)))), ((int)(((byte)(64)))));
			this.P_Spacer_1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Spacer_1.Location = new System.Drawing.Point(50, 136);
			this.P_Spacer_1.Margin = new System.Windows.Forms.Padding(50, 0, 50, 0);
			this.P_Spacer_1.Name = "P_Spacer_1";
			this.P_Spacer_1.Size = new System.Drawing.Size(228, 1);
			this.P_Spacer_1.TabIndex = 12;
			// 
			// FLP_Buttons
			// 
			this.FLP_Buttons.Controls.Add(this.B_Cancel);
			this.FLP_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FLP_Buttons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.FLP_Buttons.Location = new System.Drawing.Point(0, 137);
			this.FLP_Buttons.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Buttons.Name = "FLP_Buttons";
			this.FLP_Buttons.Size = new System.Drawing.Size(328, 42);
			this.FLP_Buttons.TabIndex = 2;
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
			this.B_Cancel.Image = Properties.Resources.I_Cancel_16;
			this.B_Cancel.Location = new System.Drawing.Point(233, 7);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Cancel.Size = new System.Drawing.Size(90, 28);
			this.B_Cancel.SpaceTriggersClick = true;
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
			this.TLP_ImgText.Size = new System.Drawing.Size(322, 130);
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
			this.L_Text.Size = new System.Drawing.Size(212, 100);
			this.L_Text.TabIndex = 0;
			this.L_Text.Text = "Text";
			this.L_Text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PB_Icon
			// 
			this.PB_Icon.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.PB_Icon.Image = null;
			this.PB_Icon.Loading = true;
			this.PB_Icon.Location = new System.Drawing.Point(19, 44);
			this.PB_Icon.Name = "PB_Icon";
			this.PB_Icon.Size = new System.Drawing.Size(42, 42);
			this.PB_Icon.TabIndex = 1;
			this.PB_Icon.TabStop = false;
			// 
			// WorkingPrompt
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Main);
			this.Name = "WorkingPrompt";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(330, 181);
			this.TLP_Main.ResumeLayout(false);
			this.FLP_Buttons.ResumeLayout(false);
			this.TLP_ImgText.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PB_Icon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.TableLayoutPanel TLP_Main;
		private System.Windows.Forms.Panel P_Spacer_1;
		private System.Windows.Forms.FlowLayoutPanel FLP_Buttons;
		private SlickControls.SlickButton B_Cancel;
		internal System.Windows.Forms.TableLayoutPanel TLP_ImgText;
		internal System.Windows.Forms.Label L_Text;
		private SlickControls.SlickPictureBox PB_Icon;
	}
}
