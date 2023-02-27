
namespace SlickControls
{
	partial class MessagePrompt
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
			this.B_Ignore = new SlickControls.SlickButton();
			this.B_Retry = new SlickControls.SlickButton();
			this.B_Abort = new SlickControls.SlickButton();
			this.B_OK = new SlickControls.SlickButton();
			this.B_No = new SlickControls.SlickButton();
			this.B_Yes = new SlickControls.SlickButton();
			this.TLP_ImgText = new System.Windows.Forms.TableLayoutPanel();
			this.TB_Input = new SlickControls.SlickTextBox();
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
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(157)))), ((int)(((byte)(22)))));
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
			this.FLP_Buttons.Controls.Add(this.B_Ignore);
			this.FLP_Buttons.Controls.Add(this.B_Retry);
			this.FLP_Buttons.Controls.Add(this.B_Abort);
			this.FLP_Buttons.Controls.Add(this.B_OK);
			this.FLP_Buttons.Controls.Add(this.B_No);
			this.FLP_Buttons.Controls.Add(this.B_Yes);
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
			// B_Ignore
			// 
			this.B_Ignore.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Ignore.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Ignore.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Ignore.ColorShade = null;
			this.B_Ignore.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Ignore.Image = global::SlickControls.Properties.Resources.Tiny_Skip;
			this.B_Ignore.Location = new System.Drawing.Point(171, 7);
			this.B_Ignore.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_Ignore.Name = "B_Ignore";
			this.B_Ignore.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Ignore.Size = new System.Drawing.Size(90, 28);
			this.B_Ignore.TabIndex = 1;
			this.B_Ignore.Text = "Ignore";
			this.B_Ignore.Visible = false;
			this.B_Ignore.Click += new System.EventHandler(this.B_Ignore_Click);
			// 
			// B_Retry
			// 
			this.B_Retry.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Retry.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Retry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Retry.ColorShade = null;
			this.B_Retry.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Retry.Image = global::SlickControls.Properties.Resources.Tiny_Retry;
			this.B_Retry.Location = new System.Drawing.Point(71, 7);
			this.B_Retry.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_Retry.Name = "B_Retry";
			this.B_Retry.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Retry.Size = new System.Drawing.Size(90, 28);
			this.B_Retry.TabIndex = 2;
			this.B_Retry.Text = "Retry";
			this.B_Retry.Visible = false;
			this.B_Retry.Click += new System.EventHandler(this.B_Retry_Click);
			// 
			// B_Abort
			// 
			this.B_Abort.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Abort.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Abort.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Abort.ColorShade = null;
			this.B_Abort.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Abort.Image = global::SlickControls.Properties.Resources.Tiny_Abort;
			this.B_Abort.Location = new System.Drawing.Point(271, 49);
			this.B_Abort.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_Abort.Name = "B_Abort";
			this.B_Abort.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Abort.Size = new System.Drawing.Size(90, 28);
			this.B_Abort.TabIndex = 3;
			this.B_Abort.Text = "Abort";
			this.B_Abort.Visible = false;
			this.B_Abort.Click += new System.EventHandler(this.B_Abort_Click);
			// 
			// B_OK
			// 
			this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_OK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_OK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_OK.ColorShade = null;
			this.B_OK.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_OK.Image = global::SlickControls.Properties.Resources.Tiny_Ok;
			this.B_OK.Location = new System.Drawing.Point(171, 49);
			this.B_OK.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_OK.Name = "B_OK";
			this.B_OK.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_OK.Size = new System.Drawing.Size(90, 28);
			this.B_OK.TabIndex = 4;
			this.B_OK.Text = "OK";
			this.B_OK.Visible = false;
			this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
			// 
			// B_No
			// 
			this.B_No.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_No.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_No.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_No.ColorShade = null;
			this.B_No.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_No.Image = global::SlickControls.Properties.Resources.Tiny_Cancel;
			this.B_No.Location = new System.Drawing.Point(71, 49);
			this.B_No.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_No.Name = "B_No";
			this.B_No.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_No.Size = new System.Drawing.Size(90, 28);
			this.B_No.TabIndex = 5;
			this.B_No.Text = "No";
			this.B_No.Visible = false;
			this.B_No.Click += new System.EventHandler(this.B_No_Click);
			// 
			// B_Yes
			// 
			this.B_Yes.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Yes.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Yes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Yes.ColorShade = null;
			this.B_Yes.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Yes.Image = global::SlickControls.Properties.Resources.Tiny_Ok;
			this.B_Yes.Location = new System.Drawing.Point(271, 91);
			this.B_Yes.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
			this.B_Yes.Name = "B_Yes";
			this.B_Yes.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Yes.Size = new System.Drawing.Size(90, 28);
			this.B_Yes.TabIndex = 6;
			this.B_Yes.Text = "Yes";
			this.B_Yes.Visible = false;
			this.B_Yes.Click += new System.EventHandler(this.B_Yes_Click);
			// 
			// TLP_ImgText
			// 
			this.TLP_ImgText.ColumnCount = 2;
			this.TLP_ImgText.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.TLP_ImgText.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_ImgText.Controls.Add(this.TB_Input, 0, 1);
			this.TLP_ImgText.Controls.Add(this.L_Text, 1, 0);
			this.TLP_ImgText.Controls.Add(this.PB_Icon, 0, 0);
			this.TLP_ImgText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_ImgText.Location = new System.Drawing.Point(3, 3);
			this.TLP_ImgText.MinimumSize = new System.Drawing.Size(0, 100);
			this.TLP_ImgText.Name = "TLP_ImgText";
			this.TLP_ImgText.RowCount = 2;
			this.TLP_ImgText.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_ImgText.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.TLP_ImgText.Size = new System.Drawing.Size(360, 148);
			this.TLP_ImgText.TabIndex = 0;
			// 
			// TB_Input
			// 
			this.TB_Input.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Input.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Input.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(26)))), ((int)(((byte)(32)))));
			this.TB_Input.Image = null;
			this.TB_Input.LabelText = "Input";
			this.TB_Input.Location = new System.Drawing.Point(75, 106);
			this.TB_Input.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
			this.TB_Input.MaximumSize = new System.Drawing.Size(9999, 34);
			this.TB_Input.MaxLength = 32767;
			this.TB_Input.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Input.Name = "TB_Input";
			this.TB_Input.Password = false;
			this.TB_Input.Placeholder = "";
			this.TB_Input.ReadOnly = false;
			this.TB_Input.Required = false;
			this.TB_Input.SelectAllOnFocus = false;
			this.TB_Input.SelectedText = "";
			this.TB_Input.SelectionLength = 0;
			this.TB_Input.SelectionStart = 0;
			this.TB_Input.Size = new System.Drawing.Size(269, 34);
			this.TB_Input.TabIndex = 0;
			this.TB_Input.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Input.Validation = SlickControls.ValidationType.None;
			this.TB_Input.ValidationRegex = "";
			this.TB_Input.Visible = false;
			// 
			// L_Text
			// 
			this.L_Text.AutoEllipsis = true;
			this.L_Text.Dock = System.Windows.Forms.DockStyle.Fill;
			this.L_Text.Location = new System.Drawing.Point(75, 15);
			this.L_Text.Margin = new System.Windows.Forms.Padding(15);
			this.L_Text.Name = "L_Text";
			this.L_Text.Size = new System.Drawing.Size(270, 68);
			this.L_Text.TabIndex = 0;
			this.L_Text.Text = "Text";
			this.L_Text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PB_Icon
			// 
			this.PB_Icon.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.PB_Icon.Image = null;
			this.PB_Icon.Location = new System.Drawing.Point(14, 58);
			this.PB_Icon.Name = "PB_Icon";
			this.TLP_ImgText.SetRowSpan(this.PB_Icon, 2);
			this.PB_Icon.Size = new System.Drawing.Size(32, 32);
			this.PB_Icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
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
			// MessagePrompt
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(385, 216);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(325, 180);
			this.Name = "MessagePrompt";
			this.Padding = new System.Windows.Forms.Padding(7, 7, 10, 10);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "MessagePrompt";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MessagePrompt_Load);
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
		private SlickPictureBox PB_Icon;
		private SlickControls.SlickButton B_Ignore;
		private SlickControls.SlickButton B_Retry;
		private SlickControls.SlickButton B_Abort;
		private SlickControls.SlickButton B_OK;
		private SlickControls.SlickButton B_No;
		private SlickControls.SlickButton B_Yes;
		private SlickControls.SlickTextBox TB_Input;
		private System.Windows.Forms.Panel P_Spacer_1;
		internal System.Windows.Forms.TableLayoutPanel TLP_Main;
		internal System.Windows.Forms.TableLayoutPanel TLP_ImgText;
		internal System.Windows.Forms.Label L_Text;
		protected System.Windows.Forms.Panel base_P_Content;
	}
}