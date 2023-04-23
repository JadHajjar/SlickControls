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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.B_Cancel = new SlickControls.SlickButton();
			this.B_Confirm = new SlickControls.SlickButton();
			this.FLP_LastColors = new System.Windows.Forms.FlowLayoutPanel();
			this.base_P_Container.SuspendLayout();




			this.TLP_Main.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Container
			// 
			this.base_P_Container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(157)))), ((int)(((byte)(22)))));
			this.base_P_Container.Controls.Add(this.TLP_Main);
			this.base_P_Container.Size = new System.Drawing.Size(689, 303);
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 5;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.90009F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.5758F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.62543F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.89868F));
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
			this.TLP_Main.Controls.Add(this.tableLayoutPanel2, 0, 6);
			this.TLP_Main.Controls.Add(this.FLP_LastColors, 0, 0);
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
			this.TLP_Main.Size = new System.Drawing.Size(687, 301);
			this.TLP_Main.TabIndex = 3;
			// 
			// colorBox2D
			// 
			this.colorBox2D.ColorMode = MechanikaDesign.WinForms.UI.ColorPicker.ColorModes.Hue;
			this.colorBox2D.ColorRGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.colorBox2D.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorBox2D.Location = new System.Drawing.Point(122, 7);
			this.colorBox2D.Margin = new System.Windows.Forms.Padding(7);
			this.colorBox2D.Name = "colorBox2D";
			this.TLP_Main.SetRowSpan(this.colorBox2D, 6);
			this.colorBox2D.Size = new System.Drawing.Size(242, 242);
			this.colorBox2D.TabIndex = 0;
			this.colorBox2D.ColorChanged += new MechanikaDesign.WinForms.UI.ColorPicker.ColorBox2D.ColorChangedEventHandler(this.colorBox2D_ColorChanged);
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
			this.TLP_Main.SetRowSpan(this.colorSlider, 6);
			this.colorSlider.Size = new System.Drawing.Size(40, 250);
			this.colorSlider.TabIndex = 1;
			this.colorSlider.ColorChanged += new MechanikaDesign.WinForms.UI.ColorPicker.ColorSliderVertical.ColorChangedEventHandler(this.colorBox2D_ColorChanged);
			// 
			// TB_Lum
			// 
			this.TB_Lum.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Lum.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Lum.Image = null;
			this.TB_Lum.LabelText = "Luminance";
			this.TB_Lum.Location = new System.Drawing.Point(596, 198);
			this.TB_Lum.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Lum.MaximumSize = new System.Drawing.Size(900, 34);
			this.TB_Lum.MaxLength = 32767;
			this.TB_Lum.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Lum.Name = "TB_Lum";
			this.TB_Lum.Password = false;
			this.TB_Lum.Placeholder = "0";
			this.TB_Lum.ReadOnly = false;
			this.TB_Lum.Required = true;
			this.TB_Lum.SelectAllOnFocus = false;
			this.TB_Lum.SelectedText = "";
			this.TB_Lum.SelectionLength = 0;
			this.TB_Lum.SelectionStart = 0;
			this.TB_Lum.Size = new System.Drawing.Size(60, 34);
			this.TB_Lum.TabIndex = 6;
			this.TB_Lum.Text = "0";
			this.TB_Lum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Lum.Validation = SlickControls.ValidationType.Number;
			this.TB_Lum.ValidationRegex = "";
			this.TB_Lum.TextChanged += new System.EventHandler(this.HSL_TextChanged);
			// 
			// TB_Red
			// 
			this.TB_Red.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Red.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Red.Image = null;
			this.TB_Red.LabelText = "Red";
			this.TB_Red.Location = new System.Drawing.Point(471, 82);
			this.TB_Red.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Red.MaximumSize = new System.Drawing.Size(900, 34);
			this.TB_Red.MaxLength = 32767;
			this.TB_Red.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Red.Name = "TB_Red";
			this.TB_Red.Password = false;
			this.TB_Red.Placeholder = "0";
			this.TB_Red.ReadOnly = false;
			this.TB_Red.Required = true;
			this.TB_Red.SelectAllOnFocus = false;
			this.TB_Red.SelectedText = "";
			this.TB_Red.SelectionLength = 0;
			this.TB_Red.SelectionStart = 0;
			this.TB_Red.Size = new System.Drawing.Size(60, 34);
			this.TB_Red.TabIndex = 1;
			this.TB_Red.Text = "0";
			this.TB_Red.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Red.Validation = SlickControls.ValidationType.Number;
			this.TB_Red.ValidationRegex = "";
			this.TB_Red.TextChanged += new System.EventHandler(this.RGB_TextChanged);
			// 
			// TB_Sat
			// 
			this.TB_Sat.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Sat.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Sat.Image = null;
			this.TB_Sat.LabelText = "Saturation";
			this.TB_Sat.Location = new System.Drawing.Point(596, 140);
			this.TB_Sat.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Sat.MaximumSize = new System.Drawing.Size(900, 34);
			this.TB_Sat.MaxLength = 32767;
			this.TB_Sat.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Sat.Name = "TB_Sat";
			this.TB_Sat.Password = false;
			this.TB_Sat.Placeholder = "0";
			this.TB_Sat.ReadOnly = false;
			this.TB_Sat.Required = true;
			this.TB_Sat.SelectAllOnFocus = false;
			this.TB_Sat.SelectedText = "";
			this.TB_Sat.SelectionLength = 0;
			this.TB_Sat.SelectionStart = 0;
			this.TB_Sat.Size = new System.Drawing.Size(60, 34);
			this.TB_Sat.TabIndex = 5;
			this.TB_Sat.Text = "0";
			this.TB_Sat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Sat.Validation = SlickControls.ValidationType.Number;
			this.TB_Sat.ValidationRegex = "";
			this.TB_Sat.TextChanged += new System.EventHandler(this.HSL_TextChanged);
			// 
			// TB_Green
			// 
			this.TB_Green.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Green.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Green.Image = null;
			this.TB_Green.LabelText = "Green";
			this.TB_Green.Location = new System.Drawing.Point(471, 140);
			this.TB_Green.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Green.MaximumSize = new System.Drawing.Size(900, 34);
			this.TB_Green.MaxLength = 32767;
			this.TB_Green.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Green.Name = "TB_Green";
			this.TB_Green.Password = false;
			this.TB_Green.Placeholder = "0";
			this.TB_Green.ReadOnly = false;
			this.TB_Green.Required = true;
			this.TB_Green.SelectAllOnFocus = false;
			this.TB_Green.SelectedText = "";
			this.TB_Green.SelectionLength = 0;
			this.TB_Green.SelectionStart = 0;
			this.TB_Green.Size = new System.Drawing.Size(60, 34);
			this.TB_Green.TabIndex = 2;
			this.TB_Green.Text = "0";
			this.TB_Green.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Green.Validation = SlickControls.ValidationType.Number;
			this.TB_Green.ValidationRegex = "";
			this.TB_Green.TextChanged += new System.EventHandler(this.RGB_TextChanged);
			// 
			// TB_Hue
			// 
			this.TB_Hue.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Hue.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Hue.Image = null;
			this.TB_Hue.LabelText = "Hue";
			this.TB_Hue.Location = new System.Drawing.Point(596, 82);
			this.TB_Hue.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Hue.MaximumSize = new System.Drawing.Size(900, 34);
			this.TB_Hue.MaxLength = 32767;
			this.TB_Hue.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Hue.Name = "TB_Hue";
			this.TB_Hue.Password = false;
			this.TB_Hue.Placeholder = "0";
			this.TB_Hue.ReadOnly = false;
			this.TB_Hue.Required = true;
			this.TB_Hue.SelectAllOnFocus = false;
			this.TB_Hue.SelectedText = "";
			this.TB_Hue.SelectionLength = 0;
			this.TB_Hue.SelectionStart = 0;
			this.TB_Hue.Size = new System.Drawing.Size(60, 34);
			this.TB_Hue.TabIndex = 4;
			this.TB_Hue.Text = "0";
			this.TB_Hue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Hue.Validation = SlickControls.ValidationType.Number;
			this.TB_Hue.ValidationRegex = "";
			this.TB_Hue.TextChanged += new System.EventHandler(this.HSL_TextChanged);
			// 
			// TB_Blue
			// 
			this.TB_Blue.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Blue.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Blue.Image = null;
			this.TB_Blue.LabelText = "Blue";
			this.TB_Blue.Location = new System.Drawing.Point(471, 198);
			this.TB_Blue.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Blue.MaximumSize = new System.Drawing.Size(900, 34);
			this.TB_Blue.MaxLength = 32767;
			this.TB_Blue.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Blue.Name = "TB_Blue";
			this.TB_Blue.Password = false;
			this.TB_Blue.Placeholder = "0";
			this.TB_Blue.ReadOnly = false;
			this.TB_Blue.Required = true;
			this.TB_Blue.SelectAllOnFocus = false;
			this.TB_Blue.SelectedText = "";
			this.TB_Blue.SelectionLength = 0;
			this.TB_Blue.SelectionStart = 0;
			this.TB_Blue.Size = new System.Drawing.Size(60, 34);
			this.TB_Blue.TabIndex = 3;
			this.TB_Blue.Text = "0";
			this.TB_Blue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TB_Blue.Validation = SlickControls.ValidationType.Number;
			this.TB_Blue.ValidationRegex = "";
			this.TB_Blue.TextChanged += new System.EventHandler(this.RGB_TextChanged);
			// 
			// colorPreview
			// 
			this.colorPreview.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.colorPreview.Location = new System.Drawing.Point(477, 5);
			this.colorPreview.Name = "colorPreview";
			this.colorPreview.Size = new System.Drawing.Size(48, 48);
			this.colorPreview.TabIndex = 3;
			this.colorPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.colorPreview_Paint);
			// 
			// TB_Hex
			// 
			this.TB_Hex.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TB_Hex.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Hex.Image = null;
			this.TB_Hex.LabelText = "Hex";
			this.TB_Hex.Location = new System.Drawing.Point(596, 12);
			this.TB_Hex.Margin = new System.Windows.Forms.Padding(12);
			this.TB_Hex.MaximumSize = new System.Drawing.Size(900, 34);
			this.TB_Hex.MaxLength = 32767;
			this.TB_Hex.MinimumSize = new System.Drawing.Size(50, 34);
			this.TB_Hex.Name = "TB_Hex";
			this.TB_Hex.Password = false;
			this.TB_Hex.Placeholder = null;
			this.TB_Hex.ReadOnly = false;
			this.TB_Hex.Required = true;
			this.TB_Hex.SelectAllOnFocus = false;
			this.TB_Hex.SelectedText = "";
			this.TB_Hex.SelectionLength = 0;
			this.TB_Hex.SelectionStart = 0;
			this.TB_Hex.Size = new System.Drawing.Size(60, 34);
			this.TB_Hex.TabIndex = 0;
			this.TB_Hex.Text = "#00000";
			this.TB_Hex.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Hex.Validation = SlickControls.ValidationType.Custom;
			this.TB_Hex.ValidationRegex = "";
			this.TB_Hex.TextChanged += new System.EventHandler(this.TB_Hex_TextChanged);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel2, 5);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.B_Cancel, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.B_Confirm, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 257);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(687, 44);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// B_Cancel
			// 
			this.B_Cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.B_Cancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Cancel.ColorShade = null;
			this.B_Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Cancel.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
			this.B_Cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.B_Cancel.Image = global::SlickControls.Properties.Resources.I_Cancel_16;
			this.B_Cancel.Location = new System.Drawing.Point(121, 8);
			this.B_Cancel.Margin = new System.Windows.Forms.Padding(8);
			this.B_Cancel.Name = "B_Cancel";
			this.B_Cancel.Padding = new System.Windows.Forms.Padding(10, 3, 5, 3);
			this.B_Cancel.Size = new System.Drawing.Size(100, 28);
			this.B_Cancel.TabIndex = 13;
			this.B_Cancel.Text = "CANCEL";
			this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
			// 
			// B_Confirm
			// 
			this.B_Confirm.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.B_Confirm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.B_Confirm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.B_Confirm.ColorShade = null;
			this.B_Confirm.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Confirm.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
			this.B_Confirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.B_Confirm.Image = global::SlickControls.Properties.Resources.I_Ok_16;
			this.B_Confirm.Location = new System.Drawing.Point(461, 8);
			this.B_Confirm.Margin = new System.Windows.Forms.Padding(8);
			this.B_Confirm.Name = "B_Confirm";
			this.B_Confirm.Padding = new System.Windows.Forms.Padding(10, 3, 5, 3);
			this.B_Confirm.Size = new System.Drawing.Size(107, 28);
			this.B_Confirm.TabIndex = 11;
			this.B_Confirm.Text = "CONFIRM";
			this.B_Confirm.Click += new System.EventHandler(this.B_Confirm_Click);
			// 
			// FLP_LastColors
			// 
			this.FLP_LastColors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FLP_LastColors.Location = new System.Drawing.Point(0, 0);
			this.FLP_LastColors.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_LastColors.Name = "FLP_LastColors";
			this.FLP_LastColors.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.TLP_Main.SetRowSpan(this.FLP_LastColors, 6);
			this.FLP_LastColors.Size = new System.Drawing.Size(115, 256);
			this.FLP_LastColors.TabIndex = 7;
			// 
			// SlickColorPicker
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ClientSize = new System.Drawing.Size(700, 314);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SlickColorPicker";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Color Picker";
			this.Load += new System.EventHandler(this.SlickColorPicker_Load);
			this.base_P_Container.ResumeLayout(false);




			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
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
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickControls.SlickButton B_Cancel;
		private SlickControls.SlickButton B_Confirm;
		private System.Windows.Forms.FlowLayoutPanel FLP_LastColors;
	}
}