namespace SlickControls
{
	partial class SlickColorPicker
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
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.colorBox2D = new MechanikaDesign.WinForms.UI.ColorPicker.ColorBox2D();
			this.colorSlider = new MechanikaDesign.WinForms.UI.ColorPicker.ColorSliderVertical();
			this.TB_Lum = new SlickControls.SlickTextBox();
			this.TB_Red = new SlickControls.SlickTextBox();
			this.TB_Sat = new SlickControls.SlickTextBox();
			this.TB_Green = new SlickControls.SlickTextBox();
			this.TB_Hue = new SlickControls.SlickTextBox();
			this.TB_Blue = new SlickControls.SlickTextBox();
			this.colorPreview = new System.Windows.Forms.Panel();
			this.TB_Hex = new SlickControls.SlickTextBox();
			this.FLP_LastColors = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.B_Confirm = new SlickControls.SlickButton();
			this.B_Cancel = new SlickControls.SlickButton();
			this.base_P_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).BeginInit();
			this.TLP_Main.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(203)))), ((int)(((byte)(145)))));
			this.base_P_Container.Controls.Add(this.TLP_Main);
			this.base_P_Container.Size = new System.Drawing.Size(719, 279);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 5;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.98366F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.01633F));
			this.TLP_Main.Controls.Add(this.colorBox2D, 1, 0);
			this.TLP_Main.Controls.Add(this.colorSlider, 2, 0);
			this.TLP_Main.Controls.Add(this.TB_Lum, 4, 4);
			this.TLP_Main.Controls.Add(this.TB_Red, 3, 2);
			this.TLP_Main.Controls.Add(this.TB_Sat, 4, 3);
			this.TLP_Main.Controls.Add(this.TB_Green, 3, 3);
			this.TLP_Main.Controls.Add(this.TB_Hue, 4, 2);
			this.TLP_Main.Controls.Add(this.TB_Blue, 3, 4);
			this.TLP_Main.Controls.Add(this.colorPreview, 3, 0);
			this.TLP_Main.Controls.Add(this.TB_Hex, 4, 0);
			this.TLP_Main.Controls.Add(this.FLP_LastColors, 0, 0);
			this.TLP_Main.Controls.Add(this.flowLayoutPanel1, 3, 6);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(1, 1);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.RowCount = 7;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.Size = new System.Drawing.Size(717, 277);
			this.TLP_Main.TabIndex = 0;
			// 
			// colorBox2D
			// 
			this.colorBox2D.ColorMode = MechanikaDesign.WinForms.UI.ColorPicker.ColorModes.Hue;
			this.colorBox2D.ColorRGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.colorBox2D.Dock = System.Windows.Forms.DockStyle.Left;
			this.colorBox2D.Location = new System.Drawing.Point(122, 7);
			this.colorBox2D.Margin = new System.Windows.Forms.Padding(7);
			this.colorBox2D.Name = "colorBox2D";
			this.TLP_Main.SetRowSpan(this.colorBox2D, 7);
			this.colorBox2D.Size = new System.Drawing.Size(242, 263);
			this.colorBox2D.TabIndex = 0;
			this.colorBox2D.TabStop = false;
			this.colorBox2D.ColorChanged += new MechanikaDesign.WinForms.UI.ColorPicker.ColorBox2D.ColorChangedEventHandler(this.colorBox2D_ColorChanged);
			this.colorBox2D.SizeChanged += new System.EventHandler(this.colorBox2D_SizeChanged);
			// 
			// colorSlider
			// 
			this.colorSlider.ColorMode = MechanikaDesign.WinForms.UI.ColorPicker.ColorModes.Hue;
			this.colorSlider.ColorRGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.colorSlider.Dock = System.Windows.Forms.DockStyle.Left;
			this.colorSlider.Location = new System.Drawing.Point(374, 3);
			this.colorSlider.Name = "colorSlider";
			this.colorSlider.NubColor = System.Drawing.Color.Empty;
			this.colorSlider.Position = 0;
			this.TLP_Main.SetRowSpan(this.colorSlider, 7);
			this.colorSlider.Size = new System.Drawing.Size(40, 271);
			this.colorSlider.TabIndex = 1;
			this.colorSlider.TabStop = false;
			this.colorSlider.ColorChanged += new MechanikaDesign.WinForms.UI.ColorPicker.ColorSliderVertical.ColorChangedEventHandler(this.colorBox2D_ColorChanged);
			// 
			// TB_Lum
			// 
			this.TB_Lum.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Lum.LabelText = "Luminance";
			this.TB_Lum.Location = new System.Drawing.Point(614, 182);
			this.TB_Lum.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.TB_Lum.Name = "TB_Lum";
			this.TB_Lum.Placeholder = "0";
			this.TB_Lum.Required = true;
			this.TB_Lum.SelectedText = "";
			this.TB_Lum.SelectionLength = 0;
			this.TB_Lum.SelectionStart = 0;
			this.TB_Lum.Size = new System.Drawing.Size(60, 34);
			this.TB_Lum.TabIndex = 7;
			this.TB_Lum.Text = "0";
			this.TB_Lum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Lum.Validation = SlickControls.ValidationType.Number;
			this.TB_Lum.TextChanged += new System.EventHandler(this.HSL_TextChanged);
			// 
			// TB_Red
			// 
			this.TB_Red.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Red.LabelText = "Red";
			this.TB_Red.Location = new System.Drawing.Point(464, 84);
			this.TB_Red.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.TB_Red.Name = "TB_Red";
			this.TB_Red.Placeholder = "0";
			this.TB_Red.Required = true;
			this.TB_Red.SelectedText = "";
			this.TB_Red.SelectionLength = 0;
			this.TB_Red.SelectionStart = 0;
			this.TB_Red.Size = new System.Drawing.Size(60, 34);
			this.TB_Red.TabIndex = 1;
			this.TB_Red.Text = "0";
			this.TB_Red.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Red.Validation = SlickControls.ValidationType.Number;
			this.TB_Red.TextChanged += new System.EventHandler(this.RGB_TextChanged);
			// 
			// TB_Sat
			// 
			this.TB_Sat.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Sat.LabelText = "Saturation";
			this.TB_Sat.Location = new System.Drawing.Point(614, 133);
			this.TB_Sat.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.TB_Sat.Name = "TB_Sat";
			this.TB_Sat.Placeholder = "0";
			this.TB_Sat.Required = true;
			this.TB_Sat.SelectedText = "";
			this.TB_Sat.SelectionLength = 0;
			this.TB_Sat.SelectionStart = 0;
			this.TB_Sat.Size = new System.Drawing.Size(60, 34);
			this.TB_Sat.TabIndex = 6;
			this.TB_Sat.Text = "0";
			this.TB_Sat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Sat.Validation = SlickControls.ValidationType.Number;
			this.TB_Sat.TextChanged += new System.EventHandler(this.HSL_TextChanged);
			// 
			// TB_Green
			// 
			this.TB_Green.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Green.LabelText = "Green";
			this.TB_Green.Location = new System.Drawing.Point(464, 133);
			this.TB_Green.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.TB_Green.Name = "TB_Green";
			this.TB_Green.Placeholder = "0";
			this.TB_Green.Required = true;
			this.TB_Green.SelectedText = "";
			this.TB_Green.SelectionLength = 0;
			this.TB_Green.SelectionStart = 0;
			this.TB_Green.Size = new System.Drawing.Size(60, 34);
			this.TB_Green.TabIndex = 3;
			this.TB_Green.Text = "0";
			this.TB_Green.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Green.Validation = SlickControls.ValidationType.Number;
			this.TB_Green.TextChanged += new System.EventHandler(this.RGB_TextChanged);
			// 
			// TB_Hue
			// 
			this.TB_Hue.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Hue.LabelText = "Hue";
			this.TB_Hue.Location = new System.Drawing.Point(614, 84);
			this.TB_Hue.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.TB_Hue.Name = "TB_Hue";
			this.TB_Hue.Placeholder = "0";
			this.TB_Hue.Required = true;
			this.TB_Hue.SelectedText = "";
			this.TB_Hue.SelectionLength = 0;
			this.TB_Hue.SelectionStart = 0;
			this.TB_Hue.Size = new System.Drawing.Size(60, 34);
			this.TB_Hue.TabIndex = 5;
			this.TB_Hue.Text = "0";
			this.TB_Hue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Hue.Validation = SlickControls.ValidationType.Number;
			this.TB_Hue.TextChanged += new System.EventHandler(this.HSL_TextChanged);
			// 
			// TB_Blue
			// 
			this.TB_Blue.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Blue.LabelText = "Blue";
			this.TB_Blue.Location = new System.Drawing.Point(464, 182);
			this.TB_Blue.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.TB_Blue.Name = "TB_Blue";
			this.TB_Blue.Placeholder = "0";
			this.TB_Blue.Required = true;
			this.TB_Blue.SelectedText = "";
			this.TB_Blue.SelectionLength = 0;
			this.TB_Blue.SelectionStart = 0;
			this.TB_Blue.Size = new System.Drawing.Size(60, 34);
			this.TB_Blue.TabIndex = 4;
			this.TB_Blue.Text = "0";
			this.TB_Blue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Blue.Validation = SlickControls.ValidationType.Number;
			this.TB_Blue.TextChanged += new System.EventHandler(this.RGB_TextChanged);
			// 
			// colorPreview
			// 
			this.colorPreview.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.colorPreview.Location = new System.Drawing.Point(470, 5);
			this.colorPreview.Name = "colorPreview";
			this.colorPreview.Size = new System.Drawing.Size(48, 48);
			this.colorPreview.TabIndex = 3;
			this.colorPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.colorPreview_Paint);
			// 
			// TB_Hex
			// 
			this.TB_Hex.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.TB_Hex.LabelText = "Hex";
			this.TB_Hex.Location = new System.Drawing.Point(614, 12);
			this.TB_Hex.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Hex.Name = "TB_Hex";
			this.TB_Hex.Placeholder = null;
			this.TB_Hex.Required = true;
			this.TB_Hex.SelectedText = "";
			this.TB_Hex.SelectionLength = 0;
			this.TB_Hex.SelectionStart = 0;
			this.TB_Hex.Size = new System.Drawing.Size(60, 34);
			this.TB_Hex.TabIndex = 0;
			this.TB_Hex.Text = "#00000";
			this.TB_Hex.Validation = SlickControls.ValidationType.Custom;
			this.TB_Hex.TextChanged += new System.EventHandler(this.TB_Hex_TextChanged);
			// 
			// FLP_LastColors
			// 
			this.FLP_LastColors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FLP_LastColors.Location = new System.Drawing.Point(0, 0);
			this.FLP_LastColors.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_LastColors.Name = "FLP_LastColors";
			this.TLP_Main.SetRowSpan(this.FLP_LastColors, 7);
			this.FLP_LastColors.Size = new System.Drawing.Size(115, 277);
			this.FLP_LastColors.TabIndex = 7;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Main.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.B_Confirm);
			this.flowLayoutPanel1.Controls.Add(this.B_Cancel);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(519, 230);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(195, 44);
			this.flowLayoutPanel1.TabIndex = 10;
			// 
			// B_Confirm
			// 
			this.B_Confirm.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Confirm.AutoSize = true;
			this.B_Confirm.ButtonType = SlickControls.ButtonType.Active;
			this.B_Confirm.ColorShade = null;
			this.B_Confirm.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Confirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			dynamicIcon1.Name = "Ok";
			this.B_Confirm.ImageName = dynamicIcon1;
			this.B_Confirm.Location = new System.Drawing.Point(3, 3);
			this.B_Confirm.Name = "B_Confirm";
			this.B_Confirm.Size = new System.Drawing.Size(89, 38);
			this.B_Confirm.SpaceTriggersClick = true;
			this.B_Confirm.TabIndex = 8;
			this.B_Confirm.Text = "Apply";
			this.B_Confirm.Click += new System.EventHandler(this.B_Confirm_Click);
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Cancel.AutoSize = true;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			dynamicIcon2.Name = "Cancel";
			this.B_Cancel.ImageName = dynamicIcon2;
			this.B_Cancel.Location = new System.Drawing.Point(98, 3);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Size = new System.Drawing.Size(94, 38);
			this.B_Cancel.SpaceTriggersClick = true;
			this.B_Cancel.TabIndex = 9;
			this.B_Cancel.Text = "Cancel";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// SlickColorPicker
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(730, 290);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.MaximizeBox = false;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 2560, 1380);
			this.MinimizeBox = false;
			this.Name = "SlickColorPicker";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Color Picker";
			this.base_P_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).EndInit();
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private MechanikaDesign.WinForms.UI.ColorPicker.ColorBox2D colorBox2D;
		private MechanikaDesign.WinForms.UI.ColorPicker.ColorSliderVertical colorSlider;
		private System.Windows.Forms.TableLayoutPanel TLP_Main;
		private SlickControls.SlickTextBox TB_Lum;
		private SlickControls.SlickTextBox TB_Red;
		private SlickControls.SlickTextBox TB_Sat;
		private SlickControls.SlickTextBox TB_Green;
		private SlickControls.SlickTextBox TB_Hue;
		private SlickControls.SlickTextBox TB_Blue;
		private System.Windows.Forms.Panel colorPreview;
		private SlickControls.SlickTextBox TB_Hex;
		private SlickControls.SlickButton B_Cancel;
		private SlickControls.SlickButton B_Confirm;
		private System.Windows.Forms.FlowLayoutPanel FLP_LastColors;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
	}
}