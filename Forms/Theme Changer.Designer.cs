﻿namespace SlickControls
{
	partial class Theme_Changer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Theme_Changer));
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
			this.SS_Scale = new SlickControls.SlickSlider();
			this.B_Save = new SlickControls.SlickButton();
			this.label1 = new System.Windows.Forms.Label();
			this.B_Random = new SlickControls.SlickButton();
			this.base_P_Content.SuspendLayout();
			this.base_P_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			this.panel1.SuspendLayout();
			this.FLP_Pickers.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.Controls.Add(this.tableLayoutPanel1);
			this.base_P_Content.Location = new System.Drawing.Point(1, 31);
			this.base_P_Content.Size = new System.Drawing.Size(897, 543);
			// 
			// base_P_Controls
			// 
			this.base_P_Controls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
			this.base_P_Controls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(202)))), ((int)(((byte)(218)))));
			this.base_P_Controls.Location = new System.Drawing.Point(1, 32);
			// 
			// base_P_Top_Spacer
			// 
			this.base_P_Top_Spacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
			this.base_P_Top_Spacer.Size = new System.Drawing.Size(897, 2);
			// 
			// base_P_Container
			// 
			this.base_P_Container.Size = new System.Drawing.Size(899, 575);
			// 
			// base_PB_Icon
			// 
			this.base_PB_Icon.Size = new System.Drawing.Size(20, 20);
			// 
			// panel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.verticalScroll1);
			this.panel1.Controls.Add(this.FLP_Pickers);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 251);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(897, 292);
			this.panel1.TabIndex = 59;
			// 
			// verticalScroll1
			// 
			this.verticalScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.verticalScroll1.LinkedControl = this.FLP_Pickers;
			this.verticalScroll1.Location = new System.Drawing.Point(891, 0);
			this.verticalScroll1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.verticalScroll1.Name = "verticalScroll1";
			this.verticalScroll1.Size = new System.Drawing.Size(6, 292);
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
			this.FLP_Pickers.MaximumSize = new System.Drawing.Size(897, 2147483647);
			this.FLP_Pickers.MinimumSize = new System.Drawing.Size(897, 0);
			this.FLP_Pickers.Name = "FLP_Pickers";
			this.FLP_Pickers.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this.FLP_Pickers.Size = new System.Drawing.Size(897, 335);
			this.FLP_Pickers.TabIndex = 0;
			// 
			// CP_Back
			// 
			this.CP_Back.ColorName = "BackColor";
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
			this.CP_Button.Location = new System.Drawing.Point(263, 82);
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
			this.CP_ButtonText.Location = new System.Drawing.Point(481, 82);
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
			this.CP_Active.Location = new System.Drawing.Point(45, 149);
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
			this.CP_ActiveText.Location = new System.Drawing.Point(263, 149);
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
			this.CP_Label.Location = new System.Drawing.Point(481, 149);
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
			this.CP_Info.Location = new System.Drawing.Point(45, 216);
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
			this.CP_Accent.Location = new System.Drawing.Point(263, 216);
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
			this.CP_Icon.Location = new System.Drawing.Point(481, 216);
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
			this.CP_Red.Location = new System.Drawing.Point(45, 283);
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
			this.CP_Green.Location = new System.Drawing.Point(263, 283);
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
			this.CP_Yellow.Location = new System.Drawing.Point(481, 283);
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
			this.UD_BaseTheme.Image = ((System.Drawing.Image)(resources.GetObject("UD_BaseTheme.Image")));
			this.UD_BaseTheme.Items = new string[] {
        "Dark",
        "Grey",
        "Light",
        "Chic"};
			this.UD_BaseTheme.LabelText = "Base Theme";
			this.UD_BaseTheme.Location = new System.Drawing.Point(30, 202);
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
			this.UD_BaseTheme.ValidationRegex = "";
			this.UD_BaseTheme.TextChanged += new System.EventHandler(this.UD_BaseTheme_TextChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.slickSectionPanel3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.B_Reset, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.UD_BaseTheme, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.B_Random, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(897, 543);
			this.tableLayoutPanel1.TabIndex = 62;
			// 
			// slickSectionPanel3
			// 
			this.slickSectionPanel3.Active = false;
			this.slickSectionPanel3.AutoHide = false;
			this.slickSectionPanel3.AutoSize = true;
			this.slickSectionPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan(this.slickSectionPanel3, 3);
			this.slickSectionPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSectionPanel3.Flavor = null;
			this.slickSectionPanel3.Icon = ((System.Drawing.Image)(resources.GetObject("slickSectionPanel3.Icon")));
			this.slickSectionPanel3.Info = "";
			this.slickSectionPanel3.Location = new System.Drawing.Point(3, 138);
			this.slickSectionPanel3.MaximumSize = new System.Drawing.Size(897, 2147483647);
			this.slickSectionPanel3.MinimumSize = new System.Drawing.Size(150, 55);
			this.slickSectionPanel3.Name = "slickSectionPanel3";
			this.slickSectionPanel3.Padding = new System.Windows.Forms.Padding(43, 54, 0, 0);
			this.slickSectionPanel3.Size = new System.Drawing.Size(891, 55);
			this.slickSectionPanel3.TabIndex = 63;
			this.slickSectionPanel3.Text = "Color Customization";
			// 
			// B_Reset
			// 
			this.B_Reset.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_Reset.ColorShade = null;
			this.B_Reset.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Reset.Image = global::SlickControls.Properties.Resources.I_Retry_16;
			this.B_Reset.Location = new System.Drawing.Point(767, 206);
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
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 3);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.DD_Font, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.slickSectionPanel1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.SS_Scale, 3, 1);
			this.tableLayoutPanel2.Controls.Add(this.B_Save, 4, 1);
			this.tableLayoutPanel2.Controls.Add(this.label1, 2, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(897, 135);
			this.tableLayoutPanel2.TabIndex = 62;
			// 
			// DD_Font
			// 
			this.DD_Font.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.DD_Font.Conversion = null;
			this.DD_Font.EnterTriggersClick = false;
			this.DD_Font.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DD_Font.Image = ((System.Drawing.Image)(resources.GetObject("DD_Font.Image")));
			this.DD_Font.Items = null;
			this.DD_Font.LabelText = "Font Family";
			this.DD_Font.Location = new System.Drawing.Point(30, 71);
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
			this.DD_Font.Size = new System.Drawing.Size(299, 34);
			this.DD_Font.TabIndex = 65;
			this.DD_Font.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.DD_Font.Validation = SlickControls.ValidationType.None;
			this.DD_Font.ValidationRegex = "";
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
			this.slickSectionPanel1.Icon = ((System.Drawing.Image)(resources.GetObject("slickSectionPanel1.Icon")));
			this.slickSectionPanel1.Info = "";
			this.slickSectionPanel1.Location = new System.Drawing.Point(3, 3);
			this.slickSectionPanel1.MaximumSize = new System.Drawing.Size(897, 2147483647);
			this.slickSectionPanel1.MinimumSize = new System.Drawing.Size(150, 55);
			this.slickSectionPanel1.Name = "slickSectionPanel1";
			this.slickSectionPanel1.Padding = new System.Windows.Forms.Padding(43, 54, 0, 0);
			this.slickSectionPanel1.Size = new System.Drawing.Size(891, 55);
			this.slickSectionPanel1.TabIndex = 64;
			this.slickSectionPanel1.Text = "UI Customization";
			// 
			// SS_Scale
			// 
			this.SS_Scale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SS_Scale.AnimatedValue = 0;
			this.SS_Scale.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SS_Scale.FromValue = 80D;
			this.SS_Scale.Location = new System.Drawing.Point(435, 64);
			this.SS_Scale.Margin = new System.Windows.Forms.Padding(0, 3, 30, 3);
			this.SS_Scale.MaxValue = 200D;
			this.SS_Scale.MinValue = 80D;
			this.SS_Scale.Name = "SS_Scale";
			this.SS_Scale.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
			this.SS_Scale.Percentage = 0.16666666666666666D;
			this.SS_Scale.PercFrom = 0D;
			this.SS_Scale.PercTo = 0.16666666666666666D;
			this.SS_Scale.Size = new System.Drawing.Size(302, 48);
			this.SS_Scale.SliderStyle = SlickControls.SliderStyle.SingleHorizontal;
			this.SS_Scale.TabIndex = 66;
			this.SS_Scale.TargetAnimationValue = 0;
			this.SS_Scale.ToValue = 100D;
			this.SS_Scale.Value = 100D;
			this.SS_Scale.ValueOutput = null;
			this.SS_Scale.ValuesChanged += new System.EventHandler(this.SS_Scale_ValuesChanged);
			// 
			// B_Save
			// 
			this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Save.ColorShade = null;
			this.B_Save.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Save.Image = ((System.Drawing.Image)(resources.GetObject("B_Save.Image")));
			this.B_Save.Location = new System.Drawing.Point(767, 75);
			this.B_Save.Margin = new System.Windows.Forms.Padding(0, 10, 30, 10);
			this.B_Save.Name = "B_Save";
			this.B_Save.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Save.Size = new System.Drawing.Size(100, 30);
			this.B_Save.SpaceTriggersClick = true;
			this.B_Save.TabIndex = 67;
			this.B_Save.Text = "SAVE";
			this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(382, 81);
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
			this.B_Random.Image = global::SlickControls.Properties.Resources.I_Retry_16;
			this.B_Random.Location = new System.Drawing.Point(637, 206);
			this.B_Random.Margin = new System.Windows.Forms.Padding(0, 10, 30, 10);
			this.B_Random.Name = "B_Random";
			this.B_Random.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Random.Size = new System.Drawing.Size(100, 30);
			this.B_Random.SpaceTriggersClick = true;
			this.B_Random.TabIndex = 61;
			this.B_Random.Text = "RANDOM";
			this.B_Random.Click += new System.EventHandler(this.B_Random_Click);
			// 
			// Theme_Changer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(910, 586);
			this.IconBounds = new System.Drawing.Rectangle(3, 4, 20, 20);
			this.Name = "Theme_Changer";
			this.Text = "Theme Changer";
			this.Load += new System.EventHandler(this.Theme_Changer_Load);
			this.Resize += new System.EventHandler(this.Theme_Changer_Resize);
			this.base_P_Content.ResumeLayout(false);
			this.base_P_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.FLP_Pickers.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

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
	}
}
