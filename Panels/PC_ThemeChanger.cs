using Extensions;

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace SlickControls;

public partial class PC_ThemeChanger : PanelContent
{
	private readonly DisableIdentifier ColorLoopIdentifier = new();

	private readonly DisableIdentifier UD_BaseThemeIdentifier = new();

	private bool changesMade;
	private bool savedNightModeSetting;
	private bool savedUseSystemThemeSetting;
	private bool savedWindowsButtons;
	private FormDesign savedCustom = FormDesign.Custom;
	internal string savedDesignName = FormDesign.Design.Name;

	public PC_ThemeChanger()
	{
		InitializeComponent();
		verticalScroll1.LinkedControl = FLP_Pickers;
		CB_NightMode.Checked = savedNightModeSetting = FormDesign.NightModeEnabled;
		CB_UseSystemTheme.Checked = savedUseSystemThemeSetting = FormDesign.UseSystemTheme;
		CB_WindowsButtons.Checked = savedWindowsButtons = FormDesign.WindowsButtons;
		FormDesign.Custom = FormDesign.Custom.CloneTo<IFormDesign, FormDesign>();
		FormDesign.Custom.Temporary = true;

		if (FormDesign.IsCustomEligible())
		{
			FormDesign.Switch(FormDesign.Custom, forceRefresh: true);
		}
		else
		{
			FormDesign.ForceRefresh();
		}

		UD_BaseThemeIdentifier.Disable();
		UD_BaseThemeIdentifier.Enable();

		using (var fontsCollection = new InstalledFontCollection())
		{
			DD_Font.Items = fontsCollection.Families;
			DD_Font.Conversion = (x) => (x as FontFamily)?.Name ?? x?.ToString();
		}

		DD_Font.Placeholder = CrossIO.CurrentPlatform == Platform.MacOSX ? "San Francisco" : "Segoe UI";
		DD_Font.FontDropdown = true;
		DD_Font.SelectedItem = UI._instance.fontFamily;
		SS_Scale.Value = UI._instance.fontScale * 100;
		SS_Scale.ValueOutput = (x) => $"{x}%";
		CB_DisableAnimations.Checked = AnimationHandler.NoAnimations;

		SlickTip.SetTo(CB_UseSystemTheme, "Converts your theme into Light or Dark mode based on your computer's theme");
		SlickTip.SetTo(CB_NightMode, "Automatically switches to a Light mode during the day and Dark mode during the night");

		if (CrossIO.CurrentPlatform != Platform.Windows)
		{
			CB_DisableAnimations.Text = "Enable Animations";
		}

		L_UiScale.Text = LocaleHelper.GetGlobalText(L_UiScale.Text);

		changesMade = false;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		SS_Scale.Width = (int)(350 * UI.FontScale);
		DD_Font.Margin = CB_DisableAnimations.Margin = CB_NightMode.Margin = CB_WindowsButtons.Margin = CB_UseSystemTheme.Margin = UI.Scale(new System.Windows.Forms.Padding(5), UI.FontScale);
		DD_Font.Width = (int)(250 * UI.FontScale);
		B_Random.Margin = B_Reset.Margin = B_Save.Margin = UI.Scale(new System.Windows.Forms.Padding(10), UI.FontScale);
		P_UI.Margin = P_Theme.Margin = UI.Scale(new System.Windows.Forms.Padding(10), UI.FontScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		L_UiScale.ForeColor = design.LabelColor;
		P_UI.BackColor = P_Theme.BackColor = design.BackColor.Tint(Lum: design.IsDarkTheme ? -1 : 1);
	}

	public override bool CanExit(bool toBeDisposed)
	{
		if (changesMade && ShowPrompt(LocaleHelper.GetGlobalText("Do you want to apply your changes before leaving?"), PromptButtons.YesNo, PromptIcons.Hand) == System.Windows.Forms.DialogResult.Yes)
		{
			B_Save_Click(null, null);
		}
		else
		{
			FormDesign.Custom = savedCustom;
			FormDesign.Switch(FormDesign.List[savedDesignName]);
		}

		FormDesign.NightModeEnabled = savedNightModeSetting;
		FormDesign.UseSystemTheme = savedUseSystemThemeSetting;
		FormDesign.WindowsButtons = savedWindowsButtons;
		FormDesign.ForceRefresh();
		FormDesign.Save();

		return true;
	}

	private void SS_Scale_ValuesChanged(object sender, EventArgs e)
	{
		SS_Scale.Value -= SS_Scale.Value % 5;
		changesMade = true;
	}

	private void B_Save_Click(object sender, EventArgs e)
	{
		changesMade = false;

		if (UI._instance.fontScale != SS_Scale.Value || UI._instance.noAnimations != CB_DisableAnimations.Checked || UI.FontFamily != DD_Font.Conversion(DD_Font.SelectedItem).IfEmpty("Nirmala UI"))
		{
			var previous = new UI
			{
				fontFamily = UI._instance.fontFamily,
				fontScale = UI._instance.fontScale,
			};

			UI._instance.fontFamily = DD_Font.Conversion(DD_Font.SelectedItem);
			UI._instance.fontScale = SS_Scale.Value / 100;
			UI._instance.noAnimations = CB_DisableAnimations.Checked;

			UI.OnUiChanged();
		}

		UI._instance.Save(appName: "SlickUI");

		savedNightModeSetting = CB_NightMode.Checked;
		savedUseSystemThemeSetting = CB_UseSystemTheme.Checked;
		savedWindowsButtons = CB_WindowsButtons.Checked;

		FormDesign.Custom.Temporary = false;
		FormDesign.Custom.DarkMode = null;
		savedCustom = FormDesign.Custom;

		FormDesign.Custom = FormDesign.Custom.CloneTo<IFormDesign, FormDesign>();

		if (FormDesign.IsCustomEligible())
		{
			savedDesignName = FormDesign.Custom.Name;
			FormDesign.Switch(FormDesign.Custom, true, true);
		}
	}

	private void B_Reset_Click(object sender, EventArgs e)
	{
		if (sender == null || ShowPrompt("Are you sure you want to reset your theme?", PromptButtons.YesNo, PromptIcons.Warning) == System.Windows.Forms.DialogResult.Yes)
		{
			FormDesign.ResetCustomTheme();
			FormDesign.Switch(FormDesign.List[savedDesignName]);
		}
	}

	private void CP_ColorChanged(object sender, bool userChange)
	{
		if (ColorLoopIdentifier.Enabled)
		{
			ColorLoopIdentifier.Disable();

			if (userChange)
			{
				foreach (ColorPicker item in FLP_Pickers.Controls)
				{
					if (item != sender)
					{
						item.Refresh();
					}
				}

				if (!FormDesign.IsCustomEligible())
				{
					FormDesign.Switch(FormDesign.List[savedDesignName]);
				}
			}

			ColorLoopIdentifier.Enable();

			changesMade = true;
		}
	}

	private void Theme_Changer_Load(object sender, EventArgs e)
	{
		var settings = ISave.Load<SlickUISettings>("Settings.tf", "SlickUI");

		if (!settings.TutorialShown)
		{
			settings.TutorialShown = true;
			settings.Save("Settings.tf", appName: "SlickUI");

			var notification = Notification.Create(
				LocaleHelper.GetGlobalText("Welcome to Theme Changer!"),
				LocaleHelper.GetGlobalText("In here, you can customize the scale and colors of the App to your liking.") + "\r\n" + LocaleHelper.GetGlobalText("Click on any color to change it, or middle-click to reset it."),
				PromptIcons.Info,
				null,
				size: new Size(350, 90));

			notification.Show(Form, 30);
		}

		changesMade = false;
	}

	private void B_Random_Click(object sender, EventArgs e)
	{
		FormDesign.ResetCustomTheme();
		FormDesign.SetCustomBaseDesign(FormDesign.List.Random());

		var baseColor = Color.FromArgb(ExtensionClass.RNG.Next(256), ExtensionClass.RNG.Next(256), ExtensionClass.RNG.Next(256));

		foreach (var item in FLP_Pickers.Controls.OfType<ColorPicker>())
		{
			if (!item.ColorName.AnyOf("RedColor", "YellowColor", "GreenColor"))
			{
				item.Color = item.GetDefaultColor().Tint(baseColor);
				item.ColorSetter(item.Color);
			}
		}

		FormDesign.Switch(FormDesign.Custom, true, true);
	}

	private void DD_Font_TextChanged(object sender, EventArgs e)
	{
		changesMade = true;
	}

	private void CB_NightMode_CheckChanged(object sender, EventArgs e)
	{
		changesMade = true;
		FormDesign.NightModeEnabled = CB_NightMode.Checked;
		
		if (CB_NightMode.Checked && CB_UseSystemTheme.Checked)
		{
			CB_UseSystemTheme.Checked = false;
		}
		else
		{
			FormDesign.ForceRefresh();
		}
	}

	private void CB_UseSystemTheme_CheckChanged(object sender, EventArgs e)
	{
		changesMade = true;
		FormDesign.UseSystemTheme = CB_UseSystemTheme.Checked;

		if (CB_NightMode.Checked && CB_UseSystemTheme.Checked)
		{
			CB_NightMode.Checked = false;
		}
		else
		{
			FormDesign.ForceRefresh();
		}
	}

	private void CB_DisableAnimations_CheckChanged(object sender, EventArgs e)
	{
		changesMade = true;
	}

	private void CB_WindowsButtons_CheckChanged(object sender, EventArgs e)
	{
		FormDesign.WindowsButtons = CB_WindowsButtons.Checked;
		Form?.Invalidate(true);
		changesMade = true;
	}
}