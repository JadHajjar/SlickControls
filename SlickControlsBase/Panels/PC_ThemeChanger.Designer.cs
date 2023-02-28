namespace SlickControls
{
	partial class PC_ThemeChanger
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PC_ThemeChanger));
			this.panel1 = new System.Windows.Forms.Panel();
			this.verticalScroll1 = new SlickControls.SlickScroll();
			this.FLP_Pickers = new System.Windows.Forms.FlowLayoutPanel();
			this.CP_Back = new SlickControls.ColorPicker();
			this.CP_Text = new SlickControls.ColorPicker();
			this.CP_Menu = new SlickControls.ColorPicker();
			this.CP_MenuText = new SlickControls.ColorPicker();
			this.CP_Button = new SlickControls.ColorPicker();
			this.CP_ButtonText = new SlickControls.ColorPicker();
			this.CP_Active = new SlickControls.ColorPicker();
			this.CP_ActiveText = new SlickControls.ColorPicker();
			this.CP_Label = new SlickControls.ColorPicker();
			this.CP_Info = new SlickControls.ColorPicker();
			this.CP_Accent = new SlickControls.ColorPicker();
			this.CP_Icon = new SlickControls.ColorPicker();
			this.CP_Red = new SlickControls.ColorPicker();
			this.CP_Green = new SlickControls.ColorPicker();
			this.CP_Yellow = new SlickControls.ColorPicker();
			this.UD_BaseTheme = new SlickControls.SlickDropdown();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSectionPanel3 = new SlickControls.SlickSectionPanel();
			this.B_Reset = new SlickControls.SlickButton();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.DD_Font = new SlickControls.SlickDropdown();
			this.slickSectionPanel1 = new SlickControls.SlickSectionPanel();
			this.B_Save = new SlickControls.SlickButton();
			this.SS_Scale = new SlickControls.SlickSlider();
			this.label1 = new System.Windows.Forms.Label();
			this.B_Random = new SlickControls.SlickButton();
			this.CB_NightMode = new SlickControls.SlickCheckbox();
			this.CB_DisableAnimations = new SlickControls.SlickCheckbox();
			this.panel1.SuspendLayout();
			this.FLP_Pickers.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(118, 26);
			this.base_Text.Text = "Theme Changer";
			// 
			// panel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 4);
			this.panel1.Controls.Add(this.verticalScroll1);
			this.panel1.Controls.Add(this.FLP_Pickers);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 276);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(999, 387);
			this.panel1.TabIndex = 59;
			// 
			// verticalScroll1
			// 
			this.verticalScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.verticalScroll1.LinkedControl = this.FLP_Pickers;
			this.verticalScroll1.Location = new System.Drawing.Point(993, 0);
			this.verticalScroll1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll1.Name = "verticalScroll1";
			this.verticalScroll1.Size = new System.Drawing.Size(6, 387);
			this.verticalScroll1.Style = SlickControls.StyleType.Vertical;
			this.verticalScroll1.TabIndex = 1;
			this.verticalScroll1.TabStop = false;
			// 
			// FLP_Pickers
			// 
			this.FLP_Pickers.AutoSize = true;
			this.FLP_Pickers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FLP_Pickers.Controls.Add(this.CP_Back);
			this.FLP_Pickers.Controls.Add(this.CP_Text);
			this.FLP_Pickers.Controls.Add(this.CP_Menu);
			this.FLP_Pickers.Controls.Add(this.CP_MenuText);
			this.FLP_Pickers.Controls.Add(this.CP_Button);
			this.FLP_Pickers.Controls.Add(this.CP_ButtonText);
			this.FLP_Pickers.Controls.Add(this.CP_Active);
			this.FLP_Pickers.Controls.Add(this.CP_ActiveText);
			this.FLP_Pickers.Controls.Add(this.CP_Label);
			this.FLP_Pickers.Controls.Add(this.CP_Info);
			this.FLP_Pickers.Controls.Add(this.CP_Accent);
			this.FLP_Pickers.Controls.Add(this.CP_Icon);
			this.FLP_Pickers.Controls.Add(this.CP_Red);
			this.FLP_Pickers.Controls.Add(this.CP_Green);
			this.FLP_Pickers.Controls.Add(this.CP_Yellow);
			this.FLP_Pickers.Location = new System.Drawing.Point(0, 0);
			this.FLP_Pickers.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Pickers.MaximumSize = new System.Drawing.Size(1037, 2147483647);
			this.FLP_Pickers.MinimumSize = new System.Drawing.Size(1037, 0);
			this.FLP_Pickers.Name = "FLP_Pickers";
			this.FLP_Pickers.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this.FLP_Pickers.Size = new System.Drawing.Size(1037, 268);
			this.FLP_Pickers.TabIndex = 0;
			// 
			// CP_Back
			// 
			this.CP_Back.ColorName = "BackColor";
			this.CP_Back.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Back.Location = new System.Drawing.Point(45, 15);
			this.CP_Back.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Back.Name = "CP_Back";
			this.CP_Back.Size = new System.Drawing.Size(188, 37);
			this.CP_Back.TabIndex = 0;
			this.CP_Back.Text = "Background";
			this.CP_Back.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Text
			// 
			this.CP_Text.ColorName = "ForeColor";
			this.CP_Text.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Text.Location = new System.Drawing.Point(263, 15);
			this.CP_Text.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Text.Name = "CP_Text";
			this.CP_Text.Size = new System.Drawing.Size(188, 37);
			this.CP_Text.TabIndex = 1;
			this.CP_Text.Text = "Text";
			this.CP_Text.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Menu
			// 
			this.CP_Menu.ColorName = "MenuColor";
			this.CP_Menu.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Menu.Location = new System.Drawing.Point(481, 15);
			this.CP_Menu.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Menu.Name = "CP_Menu";
			this.CP_Menu.Size = new System.Drawing.Size(188, 37);
			this.CP_Menu.TabIndex = 8;
			this.CP_Menu.Text = "Menu Background";
			this.CP_Menu.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_MenuText
			// 
			this.CP_MenuText.ColorName = "MenuForeColor";
			this.CP_MenuText.DefaultColor = System.Drawing.Color.Empty;
			this.CP_MenuText.Location = new System.Drawing.Point(699, 15);
			this.CP_MenuText.Margin = new System.Windows.Forms.Padding(15);
			this.CP_MenuText.Name = "CP_MenuText";
			this.CP_MenuText.Size = new System.Drawing.Size(188, 37);
			this.CP_MenuText.TabIndex = 3;
			this.CP_MenuText.Text = "Menu Text";
			this.CP_MenuText.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Button
			// 
			this.CP_Button.ColorName = "ButtonColor";
			this.CP_Button.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Button.Location = new System.Drawing.Point(45, 82);
			this.CP_Button.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Button.Name = "CP_Button";
			this.CP_Button.Size = new System.Drawing.Size(188, 37);
			this.CP_Button.TabIndex = 14;
			this.CP_Button.Text = "Button";
			this.CP_Button.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_ButtonText
			// 
			this.CP_ButtonText.ColorName = "ButtonForeColor";
			this.CP_ButtonText.DefaultColor = System.Drawing.Color.Empty;
			this.CP_ButtonText.Location = new System.Drawing.Point(263, 82);
			this.CP_ButtonText.Margin = new System.Windows.Forms.Padding(15);
			this.CP_ButtonText.Name = "CP_ButtonText";
			this.CP_ButtonText.Size = new System.Drawing.Size(188, 37);
			this.CP_ButtonText.TabIndex = 2;
			this.CP_ButtonText.Text = "Button Text";
			this.CP_ButtonText.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Active
			// 
			this.CP_Active.ColorName = "ActiveColor";
			this.CP_Active.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Active.Location = new System.Drawing.Point(481, 82);
			this.CP_Active.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Active.Name = "CP_Active";
			this.CP_Active.Size = new System.Drawing.Size(188, 37);
			this.CP_Active.TabIndex = 9;
			this.CP_Active.Text = "Active";
			this.CP_Active.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_ActiveText
			// 
			this.CP_ActiveText.ColorName = "ActiveForeColor";
			this.CP_ActiveText.DefaultColor = System.Drawing.Color.Empty;
			this.CP_ActiveText.Location = new System.Drawing.Point(699, 82);
			this.CP_ActiveText.Margin = new System.Windows.Forms.Padding(15);
			this.CP_ActiveText.Name = "CP_ActiveText";
			this.CP_ActiveText.Size = new System.Drawing.Size(188, 37);
			this.CP_ActiveText.TabIndex = 9;
			this.CP_ActiveText.Text = "Active Text";
			this.CP_ActiveText.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Label
			// 
			this.CP_Label.ColorName = "LabelColor";
			this.CP_Label.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Label.Location = new System.Drawing.Point(45, 149);
			this.CP_Label.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Label.Name = "CP_Label";
			this.CP_Label.Size = new System.Drawing.Size(188, 37);
			this.CP_Label.TabIndex = 4;
			this.CP_Label.Text = "Label";
			this.CP_Label.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Info
			// 
			this.CP_Info.ColorName = "InfoColor";
			this.CP_Info.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Info.Location = new System.Drawing.Point(263, 149);
			this.CP_Info.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Info.Name = "CP_Info";
			this.CP_Info.Size = new System.Drawing.Size(188, 37);
			this.CP_Info.TabIndex = 5;
			this.CP_Info.Text = "Info Text";
			this.CP_Info.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Accent
			// 
			this.CP_Accent.ColorName = "AccentColor";
			this.CP_Accent.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Accent.Location = new System.Drawing.Point(481, 149);
			this.CP_Accent.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Accent.Name = "CP_Accent";
			this.CP_Accent.Size = new System.Drawing.Size(188, 37);
			this.CP_Accent.TabIndex = 6;
			this.CP_Accent.Text = "Accent Background";
			this.CP_Accent.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Icon
			// 
			this.CP_Icon.ColorName = "IconColor";
			this.CP_Icon.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Icon.Location = new System.Drawing.Point(699, 149);
			this.CP_Icon.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Icon.Name = "CP_Icon";
			this.CP_Icon.Size = new System.Drawing.Size(188, 37);
			this.CP_Icon.TabIndex = 10;
			this.CP_Icon.Text = "Icon";
			this.CP_Icon.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Red
			// 
			this.CP_Red.ColorName = "RedColor";
			this.CP_Red.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Red.Location = new System.Drawing.Point(45, 216);
			this.CP_Red.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Red.Name = "CP_Red";
			this.CP_Red.Size = new System.Drawing.Size(188, 37);
			this.CP_Red.TabIndex = 11;
			this.CP_Red.Text = "Red";
			this.CP_Red.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Green
			// 
			this.CP_Green.ColorName = "GreenColor";
			this.CP_Green.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Green.Location = new System.Drawing.Point(263, 216);
			this.CP_Green.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Green.Name = "CP_Green";
			this.CP_Green.Size = new System.Drawing.Size(188, 37);
			this.CP_Green.TabIndex = 12;
			this.CP_Green.Text = "Green";
			this.CP_Green.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Yellow
			// 
			this.CP_Yellow.ColorName = "YellowColor";
			this.CP_Yellow.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Yellow.Location = new System.Drawing.Point(481, 216);
			this.CP_Yellow.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Yellow.Name = "CP_Yellow";
			this.CP_Yellow.Size = new System.Drawing.Size(188, 37);
			this.CP_Yellow.TabIndex = 13;
			this.CP_Yellow.Text = "Yellow";
			this.CP_Yellow.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// UD_BaseTheme
			// 
			this.UD_BaseTheme.Conversion = null;
			this.UD_BaseTheme.EnterTriggersClick = false;
			this.UD_BaseTheme.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UD_BaseTheme.Items = new string[] {
        "Dark",
        "Grey",
        "Light",
        "Chic"};
			this.UD_BaseTheme.LabelText = "Base Theme";
			this.UD_BaseTheme.Location = new System.Drawing.Point(30, 227);
			this.UD_BaseTheme.Margin = new System.Windows.Forms.Padding(30, 6, 6, 6);
			this.UD_BaseTheme.MaximumSize = new System.Drawing.Size(400, 34);
			this.UD_BaseTheme.MaxLength = 32767;
			this.UD_BaseTheme.MinimumSize = new System.Drawing.Size(50, 34);
			this.UD_BaseTheme.Name = "UD_BaseTheme";
			this.UD_BaseTheme.Password = false;
			this.UD_BaseTheme.Placeholder = "Select how the app interacts with your theme";
			this.UD_BaseTheme.ReadOnly = false;
			this.UD_BaseTheme.Required = false;
			this.UD_BaseTheme.SelectAllOnFocus = false;
			this.UD_BaseTheme.SelectedItem = null;
			this.UD_BaseTheme.SelectedText = "";
			this.UD_BaseTheme.SelectionLength = 0;
			this.UD_BaseTheme.SelectionStart = 0;
			this.UD_BaseTheme.Size = new System.Drawing.Size(400, 34);
			this.UD_BaseTheme.TabIndex = 60;
			this.UD_BaseTheme.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.UD_BaseTheme.Validation = SlickControls.ValidationType.None;
			this.UD_BaseTheme.ValidationCustom = null;
			this.UD_BaseTheme.ValidationRegex = "";
			this.UD_BaseTheme.TextChanged += new System.EventHandler(this.UD_BaseTheme_TextChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.slickSectionPanel3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.B_Reset, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.UD_BaseTheme, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.B_Random, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.CB_NightMode, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 30);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(999, 663);
			this.tableLayoutPanel1.TabIndex = 62;
			// 
			// slickSectionPanel3
			// 
			this.slickSectionPanel3.Active = false;
			this.slickSectionPanel3.AutoHide = false;
			this.slickSectionPanel3.AutoSize = true;
			this.slickSectionPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan(this.slickSectionPanel3, 4);
			this.slickSectionPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSectionPanel3.Flavor = null;
			this.slickSectionPanel3.Info = "";
			this.slickSectionPanel3.Location = new System.Drawing.Point(3, 163);
			this.slickSectionPanel3.MaximumSize = new System.Drawing.Size(999, 2147483647);
			this.slickSectionPanel3.MinimumSize = new System.Drawing.Size(271, 55);
			this.slickSectionPanel3.Name = "slickSectionPanel3";
			this.slickSectionPanel3.Padding = new System.Windows.Forms.Padding(43, 54, 0, 0);
			this.slickSectionPanel3.Size = new System.Drawing.Size(993, 55);
			this.slickSectionPanel3.TabIndex = 63;
			this.slickSectionPanel3.Text = "Color Customization";
			// 
			// B_Reset
			// 
			this.B_Reset.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Reset.ColorShade = null;
			this.B_Reset.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Reset.IconSize = 16;
			this.B_Reset.Image = global::SlickControls.Properties.Resources.Tiny_Retry;
			this.B_Reset.Location = new System.Drawing.Point(869, 231);
			this.B_Reset.Margin = new System.Windows.Forms.Padding(0, 10, 30, 10);
			this.B_Reset.Name = "B_Reset";
			this.B_Reset.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Reset.Size = new System.Drawing.Size(100, 30);
			this.B_Reset.SpaceTriggersClick = true;
			this.B_Reset.TabIndex = 61;
			this.B_Reset.Text = "RESET";
			this.B_Reset.Click += new System.EventHandler(this.B_Reset_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 5;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 4);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.DD_Font, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.slickSectionPanel1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.B_Save, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.SS_Scale, 3, 2);
			this.tableLayoutPanel2.Controls.Add(this.label1, 2, 2);
			this.tableLayoutPanel2.Controls.Add(this.CB_DisableAnimations, 1, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(999, 145);
			this.tableLayoutPanel2.TabIndex = 62;
			// 
			// DD_Font
			// 
			this.DD_Font.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.DD_Font.Conversion = null;
			this.DD_Font.EnterTriggersClick = false;
			this.DD_Font.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DD_Font.Items = null;
			this.DD_Font.LabelText = "Font Family";
			this.DD_Font.Location = new System.Drawing.Point(30, 101);
			this.DD_Font.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
			this.DD_Font.MaximumSize = new System.Drawing.Size(9999, 34);
			this.DD_Font.MaxLength = 32767;
			this.DD_Font.MinimumSize = new System.Drawing.Size(50, 34);
			this.DD_Font.Name = "DD_Font";
			this.DD_Font.Password = false;
			this.DD_Font.Placeholder = null;
			this.DD_Font.ReadOnly = false;
			this.DD_Font.Required = false;
			this.DD_Font.SelectAllOnFocus = false;
			this.DD_Font.SelectedItem = null;
			this.DD_Font.SelectedText = "";
			this.DD_Font.SelectionLength = 0;
			this.DD_Font.SelectionStart = 0;
			this.DD_Font.Size = new System.Drawing.Size(246, 34);
			this.DD_Font.TabIndex = 65;
			this.DD_Font.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.DD_Font.Validation = SlickControls.ValidationType.None;
			this.DD_Font.ValidationCustom = null;
			this.DD_Font.ValidationRegex = "";
			this.DD_Font.TextChanged += new System.EventHandler(this.DD_Font_TextChanged);
			// 
			// slickSectionPanel1
			// 
			this.slickSectionPanel1.Active = false;
			this.slickSectionPanel1.AutoHide = false;
			this.slickSectionPanel1.AutoSize = true;
			this.slickSectionPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.SetColumnSpan(this.slickSectionPanel1, 5);
			this.slickSectionPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSectionPanel1.Flavor = null;
			this.slickSectionPanel1.Info = "";
			this.slickSectionPanel1.Location = new System.Drawing.Point(3, 33);
			this.slickSectionPanel1.MaximumSize = new System.Drawing.Size(999, 2147483647);
			this.slickSectionPanel1.MinimumSize = new System.Drawing.Size(244, 55);
			this.slickSectionPanel1.Name = "slickSectionPanel1";
			this.slickSectionPanel1.Padding = new System.Windows.Forms.Padding(43, 54, 0, 0);
			this.slickSectionPanel1.Size = new System.Drawing.Size(993, 55);
			this.slickSectionPanel1.TabIndex = 64;
			this.slickSectionPanel1.Text = "UI Customization";
			// 
			// B_Save
			// 
			this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Save.ColorShade = null;
			this.tableLayoutPanel2.SetColumnSpan(this.B_Save, 3);
			this.B_Save.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Save.IconSize = 16;
			this.B_Save.Image = global::SlickControls.Properties.Resources.Tiny_Ok;
			this.B_Save.Location = new System.Drawing.Point(868, 0);
			this.B_Save.Margin = new System.Windows.Forms.Padding(0, 0, 30, 0);
			this.B_Save.Name = "B_Save";
			this.B_Save.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Save.Size = new System.Drawing.Size(100, 30);
			this.B_Save.SpaceTriggersClick = true;
			this.B_Save.TabIndex = 67;
			this.B_Save.Text = "APPLY";
			this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
			// 
			// SS_Scale
			// 
			this.SS_Scale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SS_Scale.AnimatedValue = 0;
			this.SS_Scale.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SS_Scale.FromValue = 80D;
			this.SS_Scale.Location = new System.Drawing.Point(690, 94);
			this.SS_Scale.Margin = new System.Windows.Forms.Padding(0, 3, 30, 3);
			this.SS_Scale.MaxValue = 200D;
			this.SS_Scale.MinValue = 80D;
			this.SS_Scale.Name = "SS_Scale";
			this.SS_Scale.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
			this.SS_Scale.Percentage = 0.16666666666666666D;
			this.SS_Scale.PercFrom = 0D;
			this.SS_Scale.PercTo = 0.16666666666666666D;
			this.SS_Scale.Size = new System.Drawing.Size(278, 48);
			this.SS_Scale.SliderStyle = SlickControls.SliderStyle.SingleHorizontal;
			this.SS_Scale.TabIndex = 66;
			this.SS_Scale.TargetAnimationValue = 0;
			this.SS_Scale.ToValue = 100D;
			this.SS_Scale.Value = 100D;
			this.SS_Scale.ValueOutput = null;
			this.SS_Scale.ValuesChanged += new System.EventHandler(this.SS_Scale_ValuesChanged);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(637, 111);
			this.label1.Margin = new System.Windows.Forms.Padding(50, 0, 3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 13);
			this.label1.TabIndex = 68;
			this.label1.Text = "UI Scale:";
			// 
			// B_Random
			// 
			this.B_Random.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Random.ColorShade = null;
			this.B_Random.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Random.IconSize = 16;
			this.B_Random.Location = new System.Drawing.Point(739, 231);
			this.B_Random.Margin = new System.Windows.Forms.Padding(0, 10, 30, 10);
			this.B_Random.Name = "B_Random";
			this.B_Random.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Random.Size = new System.Drawing.Size(100, 30);
			this.B_Random.SpaceTriggersClick = true;
			this.B_Random.TabIndex = 61;
			this.B_Random.Text = "RANDOMIZE";
			this.B_Random.Click += new System.EventHandler(this.B_Random_Click);
			// 
			// CB_NightMode
			// 
			this.CB_NightMode.ActiveColor = null;
			this.CB_NightMode.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.CB_NightMode.AutoSize = true;
			this.CB_NightMode.Center = false;
			this.CB_NightMode.Checked = false;
			this.CB_NightMode.CheckedText = null;
			this.CB_NightMode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_NightMode.DefaultValue = false;
			this.CB_NightMode.HideText = false;
			this.CB_NightMode.IconSize = 16;
			this.CB_NightMode.Location = new System.Drawing.Point(576, 233);
			this.CB_NightMode.Margin = new System.Windows.Forms.Padding(0, 10, 30, 10);
			this.CB_NightMode.Name = "CB_NightMode";
			this.CB_NightMode.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.CB_NightMode.Size = new System.Drawing.Size(133, 26);
			this.CB_NightMode.TabIndex = 64;
			this.CB_NightMode.TabStop = false;
			this.CB_NightMode.Text = "Auto Night-Mode";
			this.CB_NightMode.UncheckedText = null;
			this.CB_NightMode.CheckChanged += new System.EventHandler(this.CB_NightMode_CheckChanged);
			// 
			// CB_DisableAnimations
			// 
			this.CB_DisableAnimations.ActiveColor = null;
			this.CB_DisableAnimations.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.CB_DisableAnimations.AutoSize = true;
			this.CB_DisableAnimations.Center = false;
			this.CB_DisableAnimations.Checked = false;
			this.CB_DisableAnimations.CheckedText = null;
			this.CB_DisableAnimations.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_DisableAnimations.DefaultValue = false;
			this.CB_DisableAnimations.HideText = false;
			this.CB_DisableAnimations.IconSize = 16;
			this.CB_DisableAnimations.Location = new System.Drawing.Point(348, 105);
			this.CB_DisableAnimations.Margin = new System.Windows.Forms.Padding(0, 10, 30, 10);
			this.CB_DisableAnimations.Name = "CB_DisableAnimations";
			this.CB_DisableAnimations.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.CB_DisableAnimations.Size = new System.Drawing.Size(139, 26);
			this.CB_DisableAnimations.TabIndex = 64;
			this.CB_DisableAnimations.TabStop = false;
			this.CB_DisableAnimations.Text = "Disable Animations";
			this.CB_DisableAnimations.UncheckedText = null;
			this.CB_DisableAnimations.CheckChanged += new System.EventHandler(this.CB_DisableAnimations_CheckChanged);
			// 
			// PC_ThemeChanger
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "PC_ThemeChanger";
			this.Padding = new System.Windows.Forms.Padding(5, 30, 0, 0);
			this.Size = new System.Drawing.Size(1004, 693);
			this.Text = "Theme Changer";
			this.Load += new System.EventHandler(this.Theme_Changer_Load);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.FLP_Pickers.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.FlowLayoutPanel FLP_Pickers;
		private SlickControls.ColorPicker CP_Back;
		private SlickControls.ColorPicker CP_Text;
		private SlickControls.ColorPicker CP_Menu;
		private SlickControls.ColorPicker CP_MenuText;
		private SlickControls.ColorPicker CP_Button;
		private SlickControls.ColorPicker CP_ButtonText;
		private SlickControls.ColorPicker CP_Active;
		private SlickControls.ColorPicker CP_ActiveText;
		private SlickControls.ColorPicker CP_Label;
		private SlickControls.ColorPicker CP_Info;
		private SlickControls.ColorPicker CP_Accent;
		private SlickControls.ColorPicker CP_Icon;
		private SlickControls.ColorPicker CP_Red;
		private SlickControls.ColorPicker CP_Green;
		private SlickControls.ColorPicker CP_Yellow;
		internal SlickControls.SlickDropdown UD_BaseTheme;
		private SlickControls.SlickScroll verticalScroll1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private SlickControls.SlickButton B_Reset;
		private SlickControls.SlickSectionPanel slickSectionPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickControls.SlickSectionPanel slickSectionPanel1;
		private SlickControls.SlickDropdown DD_Font;
		private SlickControls.SlickSlider SS_Scale;
		private SlickControls.SlickButton B_Save;
		private System.Windows.Forms.Label label1;
		private SlickButton B_Random;
		private SlickCheckbox CB_NightMode;
		private SlickCheckbox CB_DisableAnimations;
	}
}
