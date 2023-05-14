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
			SlickControls.DynamicIcon dynamicIcon4 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon5 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon3 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon1 = new SlickControls.DynamicIcon();
			SlickControls.DynamicIcon dynamicIcon2 = new SlickControls.DynamicIcon();
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
			this.CB_UseSystemTheme = new SlickControls.SlickCheckbox();
			this.B_Save = new SlickControls.SlickButton();
			this.SS_Scale = new SlickControls.SlickSlider();
			this.L_UiScale = new System.Windows.Forms.Label();
			this.CB_DisableAnimations = new SlickControls.SlickCheckbox();
			this.CB_NightMode = new SlickControls.SlickCheckbox();
			this.P_UI = new SlickControls.RoundedGroupTableLayoutPanel();
			this.DD_Font = new SlickControls.SlickDropdown();
			this.P_Theme = new SlickControls.RoundedGroupTableLayoutPanel();
			this.B_Reset = new SlickControls.SlickButton();
			this.B_Random = new SlickControls.SlickButton();
			this.UD_BaseTheme = new SlickControls.SlickDropdown();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1.SuspendLayout();
			this.FLP_Pickers.SuspendLayout();
			this.P_UI.SuspendLayout();
			this.P_Theme.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(149, 26);
			this.base_Text.Text = "Theme Changer";
			// 
			// panel1
			// 
			this.P_Theme.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.verticalScroll1);
			this.panel1.Controls.Add(this.FLP_Pickers);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(9, 99);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(980, 287);
			this.panel1.TabIndex = 59;
			// 
			// verticalScroll1
			// 
			this.verticalScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.verticalScroll1.LinkedControl = this.FLP_Pickers;
			this.verticalScroll1.Location = new System.Drawing.Point(970, 0);
			this.verticalScroll1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll1.Name = "verticalScroll1";
			this.verticalScroll1.Size = new System.Drawing.Size(10, 287);
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
			this.FLP_Pickers.MaximumSize = new System.Drawing.Size(800, 0);
			this.FLP_Pickers.Name = "FLP_Pickers";
			this.FLP_Pickers.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this.FLP_Pickers.Size = new System.Drawing.Size(684, 335);
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
			this.CP_Menu.TabIndex = 2;
			this.CP_Menu.Text = "Menu Background";
			this.CP_Menu.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_MenuText
			// 
			this.CP_MenuText.ColorName = "MenuForeColor";
			this.CP_MenuText.DefaultColor = System.Drawing.Color.Empty;
			this.CP_MenuText.Location = new System.Drawing.Point(45, 82);
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
			this.CP_Button.Location = new System.Drawing.Point(263, 82);
			this.CP_Button.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Button.Name = "CP_Button";
			this.CP_Button.Size = new System.Drawing.Size(188, 37);
			this.CP_Button.TabIndex = 4;
			this.CP_Button.Text = "Button";
			this.CP_Button.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_ButtonText
			// 
			this.CP_ButtonText.ColorName = "ButtonForeColor";
			this.CP_ButtonText.DefaultColor = System.Drawing.Color.Empty;
			this.CP_ButtonText.Location = new System.Drawing.Point(481, 82);
			this.CP_ButtonText.Margin = new System.Windows.Forms.Padding(15);
			this.CP_ButtonText.Name = "CP_ButtonText";
			this.CP_ButtonText.Size = new System.Drawing.Size(188, 37);
			this.CP_ButtonText.TabIndex = 5;
			this.CP_ButtonText.Text = "Button Text";
			this.CP_ButtonText.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Active
			// 
			this.CP_Active.ColorName = "ActiveColor";
			this.CP_Active.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Active.Location = new System.Drawing.Point(45, 149);
			this.CP_Active.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Active.Name = "CP_Active";
			this.CP_Active.Size = new System.Drawing.Size(188, 37);
			this.CP_Active.TabIndex = 6;
			this.CP_Active.Text = "Active";
			this.CP_Active.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_ActiveText
			// 
			this.CP_ActiveText.ColorName = "ActiveForeColor";
			this.CP_ActiveText.DefaultColor = System.Drawing.Color.Empty;
			this.CP_ActiveText.Location = new System.Drawing.Point(263, 149);
			this.CP_ActiveText.Margin = new System.Windows.Forms.Padding(15);
			this.CP_ActiveText.Name = "CP_ActiveText";
			this.CP_ActiveText.Size = new System.Drawing.Size(188, 37);
			this.CP_ActiveText.TabIndex = 7;
			this.CP_ActiveText.Text = "Active Text";
			this.CP_ActiveText.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Label
			// 
			this.CP_Label.ColorName = "LabelColor";
			this.CP_Label.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Label.Location = new System.Drawing.Point(481, 149);
			this.CP_Label.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Label.Name = "CP_Label";
			this.CP_Label.Size = new System.Drawing.Size(188, 37);
			this.CP_Label.TabIndex = 8;
			this.CP_Label.Text = "Label";
			this.CP_Label.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Info
			// 
			this.CP_Info.ColorName = "InfoColor";
			this.CP_Info.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Info.Location = new System.Drawing.Point(45, 216);
			this.CP_Info.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Info.Name = "CP_Info";
			this.CP_Info.Size = new System.Drawing.Size(188, 37);
			this.CP_Info.TabIndex = 9;
			this.CP_Info.Text = "Info Text";
			this.CP_Info.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Accent
			// 
			this.CP_Accent.ColorName = "AccentColor";
			this.CP_Accent.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Accent.Location = new System.Drawing.Point(263, 216);
			this.CP_Accent.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Accent.Name = "CP_Accent";
			this.CP_Accent.Size = new System.Drawing.Size(188, 37);
			this.CP_Accent.TabIndex = 10;
			this.CP_Accent.Text = "Accent Background";
			this.CP_Accent.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Icon
			// 
			this.CP_Icon.ColorName = "IconColor";
			this.CP_Icon.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Icon.Location = new System.Drawing.Point(481, 216);
			this.CP_Icon.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Icon.Name = "CP_Icon";
			this.CP_Icon.Size = new System.Drawing.Size(188, 37);
			this.CP_Icon.TabIndex = 11;
			this.CP_Icon.Text = "Icon";
			this.CP_Icon.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Red
			// 
			this.CP_Red.ColorName = "RedColor";
			this.CP_Red.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Red.Location = new System.Drawing.Point(45, 283);
			this.CP_Red.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Red.Name = "CP_Red";
			this.CP_Red.Size = new System.Drawing.Size(188, 37);
			this.CP_Red.TabIndex = 12;
			this.CP_Red.Text = "Red";
			this.CP_Red.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Green
			// 
			this.CP_Green.ColorName = "GreenColor";
			this.CP_Green.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Green.Location = new System.Drawing.Point(263, 283);
			this.CP_Green.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Green.Name = "CP_Green";
			this.CP_Green.Size = new System.Drawing.Size(188, 37);
			this.CP_Green.TabIndex = 13;
			this.CP_Green.Text = "Green";
			this.CP_Green.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CP_Yellow
			// 
			this.CP_Yellow.ColorName = "YellowColor";
			this.CP_Yellow.DefaultColor = System.Drawing.Color.Empty;
			this.CP_Yellow.Location = new System.Drawing.Point(481, 283);
			this.CP_Yellow.Margin = new System.Windows.Forms.Padding(15);
			this.CP_Yellow.Name = "CP_Yellow";
			this.CP_Yellow.Size = new System.Drawing.Size(188, 37);
			this.CP_Yellow.TabIndex = 14;
			this.CP_Yellow.Text = "Yellow";
			this.CP_Yellow.ColorChanged += new System.Action<object, bool>(this.CP_ColorChanged);
			// 
			// CB_UseSystemTheme
			// 
			this.CB_UseSystemTheme.AutoSize = true;
			this.CB_UseSystemTheme.Checked = false;
			this.CB_UseSystemTheme.CheckedText = null;
			this.CB_UseSystemTheme.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_UseSystemTheme.DefaultValue = false;
			this.CB_UseSystemTheme.EnterTriggersClick = false;
			this.CB_UseSystemTheme.Location = new System.Drawing.Point(12, 97);
			this.CB_UseSystemTheme.Name = "CB_UseSystemTheme";
			this.CB_UseSystemTheme.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.CB_UseSystemTheme.Size = new System.Drawing.Size(209, 67);
			this.CB_UseSystemTheme.SpaceTriggersClick = true;
			this.CB_UseSystemTheme.TabIndex = 1;
			this.CB_UseSystemTheme.TabStop = false;
			this.CB_UseSystemTheme.Text = "Use the System\'s Theme";
			this.CB_UseSystemTheme.UncheckedText = null;
			this.CB_UseSystemTheme.CheckChanged += new System.EventHandler(this.CB_NightMode_CheckChanged);
			// 
			// B_Save
			// 
			this.B_Save.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Save.AutoSize = true;
			this.B_Save.ColorShade = null;
			this.B_Save.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon4.Name = "I_Ok";
			this.B_Save.ImageName = dynamicIcon4;
			this.B_Save.Location = new System.Drawing.Point(901, 630);
			this.B_Save.Name = "B_Save";
			this.B_Save.Size = new System.Drawing.Size(100, 30);
			this.B_Save.SpaceTriggersClick = true;
			this.B_Save.TabIndex = 2;
			this.B_Save.Text = "Apply";
			this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
			// 
			// SS_Scale
			// 
			this.SS_Scale.AnimatedValue = 0;
			this.SS_Scale.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SS_Scale.FromValue = 75D;
			this.SS_Scale.Location = new System.Drawing.Point(502, 87);
			this.SS_Scale.MaxValue = 300D;
			this.SS_Scale.MinValue = 75D;
			this.SS_Scale.Name = "SS_Scale";
			this.SS_Scale.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
			this.SS_Scale.Percentage = 0.16666666666666666D;
			this.SS_Scale.PercFrom = 0D;
			this.SS_Scale.PercTo = 0.16666666666666666D;
			this.P_UI.SetRowSpan(this.SS_Scale, 2);
			this.SS_Scale.Size = new System.Drawing.Size(382, 48);
			this.SS_Scale.SliderStyle = SlickControls.SliderStyle.SingleHorizontal;
			this.SS_Scale.TabIndex = 4;
			this.SS_Scale.TargetAnimationValue = 0;
			this.SS_Scale.ToValue = 112.5D;
			this.SS_Scale.Value = 112.5D;
			this.SS_Scale.ValueOutput = null;
			this.SS_Scale.ValuesChanged += new System.EventHandler(this.SS_Scale_ValuesChanged);
			// 
			// L_UiScale
			// 
			this.L_UiScale.AutoSize = true;
			this.L_UiScale.Location = new System.Drawing.Point(509, 54);
			this.L_UiScale.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.L_UiScale.Name = "L_UiScale";
			this.L_UiScale.Size = new System.Drawing.Size(90, 30);
			this.L_UiScale.TabIndex = 68;
			this.L_UiScale.Text = "UI Scale";
			this.L_UiScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CB_DisableAnimations
			// 
			this.CB_DisableAnimations.AutoSize = true;
			this.CB_DisableAnimations.Checked = false;
			this.CB_DisableAnimations.CheckedText = null;
			this.CB_DisableAnimations.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_DisableAnimations.DefaultValue = false;
			this.CB_DisableAnimations.EnterTriggersClick = false;
			this.CB_DisableAnimations.Location = new System.Drawing.Point(12, 170);
			this.CB_DisableAnimations.Name = "CB_DisableAnimations";
			this.CB_DisableAnimations.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.CB_DisableAnimations.Size = new System.Drawing.Size(233, 38);
			this.CB_DisableAnimations.SpaceTriggersClick = true;
			this.CB_DisableAnimations.TabIndex = 3;
			this.CB_DisableAnimations.TabStop = false;
			this.CB_DisableAnimations.Text = "Disable Animations";
			this.CB_DisableAnimations.UncheckedText = null;
			this.CB_DisableAnimations.CheckChanged += new System.EventHandler(this.CB_DisableAnimations_CheckChanged);
			// 
			// CB_NightMode
			// 
			this.CB_NightMode.AutoSize = true;
			this.CB_NightMode.Checked = false;
			this.CB_NightMode.CheckedText = null;
			this.CB_NightMode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_NightMode.DefaultValue = false;
			this.CB_NightMode.EnterTriggersClick = false;
			this.CB_NightMode.Location = new System.Drawing.Point(257, 97);
			this.CB_NightMode.Name = "CB_NightMode";
			this.CB_NightMode.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.CB_NightMode.Size = new System.Drawing.Size(220, 38);
			this.CB_NightMode.SpaceTriggersClick = true;
			this.CB_NightMode.TabIndex = 2;
			this.CB_NightMode.TabStop = false;
			this.CB_NightMode.Text = "Auto Night-Mode";
			this.CB_NightMode.UncheckedText = null;
			this.CB_NightMode.CheckChanged += new System.EventHandler(this.CB_NightMode_CheckChanged);
			// 
			// P_UI
			// 
			this.P_UI.AddOutline = true;
			this.P_UI.AutoSize = true;
			this.P_UI.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_UI.ColumnCount = 3;
			this.P_UI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.P_UI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.P_UI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.P_UI.Controls.Add(this.DD_Font, 0, 0);
			this.P_UI.Controls.Add(this.L_UiScale, 2, 0);
			this.P_UI.Controls.Add(this.CB_UseSystemTheme, 0, 2);
			this.P_UI.Controls.Add(this.SS_Scale, 2, 1);
			this.P_UI.Controls.Add(this.CB_DisableAnimations, 0, 3);
			this.P_UI.Controls.Add(this.CB_NightMode, 1, 2);
			this.P_UI.Dock = System.Windows.Forms.DockStyle.Top;
			dynamicIcon5.Name = "I_UserInterface";
			this.P_UI.ImageName = dynamicIcon5;
			this.P_UI.Location = new System.Drawing.Point(3, 3);
			this.P_UI.Name = "P_UI";
			this.P_UI.Padding = new System.Windows.Forms.Padding(9, 54, 9, 9);
			this.P_UI.RowCount = 4;
			this.P_UI.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_UI.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_UI.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_UI.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_UI.Size = new System.Drawing.Size(998, 220);
			this.P_UI.TabIndex = 0;
			this.P_UI.Text = "User Interface";
			// 
			// DD_Font
			// 
			this.P_UI.SetColumnSpan(this.DD_Font, 2);
			this.DD_Font.Conversion = null;
			this.DD_Font.EnterTriggersClick = false;
			this.DD_Font.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DD_Font.Image = ((System.Drawing.Image)(resources.GetObject("DD_Font.Image")));
			this.DD_Font.Items = null;
			this.DD_Font.LabelText = "Font Family";
			this.DD_Font.Location = new System.Drawing.Point(12, 57);
			this.DD_Font.Name = "DD_Font";
			this.DD_Font.Placeholder = null;
			this.P_UI.SetRowSpan(this.DD_Font, 2);
			this.DD_Font.SelectedItem = null;
			this.DD_Font.SelectedText = "";
			this.DD_Font.SelectionLength = 0;
			this.DD_Font.SelectionStart = 0;
			this.DD_Font.Size = new System.Drawing.Size(246, 34);
			this.DD_Font.TabIndex = 0;
			this.DD_Font.TextChanged += new System.EventHandler(this.DD_Font_TextChanged);
			// 
			// P_Theme
			// 
			this.P_Theme.AddOutline = true;
			this.P_Theme.ColumnCount = 3;
			this.P_Theme.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Theme.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Theme.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.P_Theme.Controls.Add(this.B_Reset, 2, 0);
			this.P_Theme.Controls.Add(this.panel1, 0, 1);
			this.P_Theme.Controls.Add(this.B_Random, 1, 0);
			this.P_Theme.Controls.Add(this.UD_BaseTheme, 0, 0);
			this.P_Theme.Dock = System.Windows.Forms.DockStyle.Fill;
			dynamicIcon3.Name = "I_Paint";
			this.P_Theme.ImageName = dynamicIcon3;
			this.P_Theme.Location = new System.Drawing.Point(3, 229);
			this.P_Theme.Name = "P_Theme";
			this.P_Theme.Padding = new System.Windows.Forms.Padding(9, 54, 9, 9);
			this.P_Theme.RowCount = 2;
			this.P_Theme.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.P_Theme.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.P_Theme.Size = new System.Drawing.Size(998, 395);
			this.P_Theme.TabIndex = 1;
			this.P_Theme.Text = "Custom Theme";
			// 
			// B_Reset
			// 
			this.B_Reset.AutoSize = true;
			this.B_Reset.ColorShade = null;
			this.B_Reset.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon1.Name = "I_Undo";
			this.B_Reset.ImageName = dynamicIcon1;
			this.B_Reset.Location = new System.Drawing.Point(886, 57);
			this.B_Reset.Name = "B_Reset";
			this.B_Reset.Size = new System.Drawing.Size(100, 30);
			this.B_Reset.SpaceTriggersClick = true;
			this.B_Reset.TabIndex = 2;
			this.B_Reset.Text = "Reset";
			this.B_Reset.Click += new System.EventHandler(this.B_Reset_Click);
			// 
			// B_Random
			// 
			this.B_Random.AutoSize = true;
			this.B_Random.ColorShade = null;
			this.B_Random.Cursor = System.Windows.Forms.Cursors.Hand;
			dynamicIcon2.Name = "I_Random";
			this.B_Random.ImageName = dynamicIcon2;
			this.B_Random.Location = new System.Drawing.Point(780, 57);
			this.B_Random.Name = "B_Random";
			this.B_Random.Size = new System.Drawing.Size(100, 30);
			this.B_Random.SpaceTriggersClick = true;
			this.B_Random.TabIndex = 1;
			this.B_Random.Text = "Randomize";
			this.B_Random.Click += new System.EventHandler(this.B_Random_Click);
			// 
			// UD_BaseTheme
			// 
			this.UD_BaseTheme.Conversion = null;
			this.UD_BaseTheme.EnterTriggersClick = false;
			this.UD_BaseTheme.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UD_BaseTheme.Image = ((System.Drawing.Image)(resources.GetObject("UD_BaseTheme.Image")));
			this.UD_BaseTheme.Items = new string[] {
        "Dark",
        "Grey",
        "Light",
        "Chic"};
			this.UD_BaseTheme.LabelText = "Base Theme";
			this.UD_BaseTheme.Location = new System.Drawing.Point(12, 57);
			this.UD_BaseTheme.Name = "UD_BaseTheme";
			this.UD_BaseTheme.Placeholder = "Select how the app interacts with your theme";
			this.UD_BaseTheme.SelectedItem = null;
			this.UD_BaseTheme.SelectedText = "";
			this.UD_BaseTheme.SelectionLength = 0;
			this.UD_BaseTheme.SelectionStart = 0;
			this.UD_BaseTheme.Size = new System.Drawing.Size(235, 34);
			this.UD_BaseTheme.TabIndex = 0;
			this.UD_BaseTheme.TextChanged += new System.EventHandler(this.UD_BaseTheme_TextChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.B_Save, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.P_Theme, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.P_UI, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1004, 663);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// PC_ThemeChanger
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "PC_ThemeChanger";
			this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			this.Size = new System.Drawing.Size(1004, 693);
			this.Text = "Theme Changer";
			this.Load += new System.EventHandler(this.Theme_Changer_Load);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.FLP_Pickers.ResumeLayout(false);
			this.P_UI.ResumeLayout(false);
			this.P_UI.PerformLayout();
			this.P_Theme.ResumeLayout(false);
			this.P_Theme.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
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
		private SlickControls.SlickButton B_Reset;
		private SlickControls.SlickDropdown DD_Font;
		private SlickControls.SlickSlider SS_Scale;
		private SlickControls.SlickButton B_Save;
		private System.Windows.Forms.Label L_UiScale;
		private SlickButton B_Random;
		private SlickCheckbox CB_NightMode;
		private SlickCheckbox CB_DisableAnimations;
		private SlickCheckbox CB_UseSystemTheme;
		private RoundedGroupTableLayoutPanel P_UI;
		private RoundedGroupTableLayoutPanel P_Theme;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
