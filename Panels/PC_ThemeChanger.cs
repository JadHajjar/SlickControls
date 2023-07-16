using Extensions;

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace SlickControls
{
	public partial class PC_ThemeChanger : PanelContent
	{
		private readonly DisableIdentifier ColorLoopIdentifier = new DisableIdentifier();

		private readonly DisableIdentifier UD_BaseThemeIdentifier = new DisableIdentifier();

		private bool changesMade;
		private bool savedNightModeSetting;
		private bool savedUseSystemThemeSetting;
		private FormDesign savedCustom = FormDesign.Custom;

		public PC_ThemeChanger()
		{
			InitializeComponent();
			verticalScroll1.LinkedControl = FLP_Pickers;
			UD_BaseTheme.Items = FormDesign.List.ToArray();
			CB_NightMode.Checked = savedNightModeSetting = FormDesign.NightModeEnabled;
			CB_UseSystemTheme.Checked = savedUseSystemThemeSetting = FormDesign.UseSystemTheme;
			FormDesign.UseSystemTheme = FormDesign.IsDarkMode = false;
			FormDesign.NightModeEnabled = false;
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
			UD_BaseTheme.Text = FormDesign.List[FormDesign.Design.ID].Name;
			UD_BaseThemeIdentifier.Enable();

			using (var fontsCollection = new InstalledFontCollection())
			{
				DD_Font.Items = fontsCollection.Families;
				DD_Font.Conversion = (x) => (x as FontFamily)?.Name ?? x?.ToString();
			}

			DD_Font.Placeholder = (CrossIO.CurrentPlatform == Platform.MacOSX ? "San Francisco" : "Segoe UI");
			DD_Font.FontDropdown = true;
			DD_Font.SelectedItem = UI._instance.fontFamily;
			SS_Scale.Value = UI._instance.fontScale * 100;
			SS_Scale.ValueOutput = (x) => $"{x}%";
			CB_DisableAnimations.Checked = AnimationHandler.NoAnimations;

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
			DD_Font.Margin = CB_DisableAnimations.Margin = CB_NightMode.Margin = CB_UseSystemTheme.Margin = UD_BaseTheme.Margin = UI.Scale(new System.Windows.Forms.Padding(5), UI.FontScale);
			DD_Font.Width = UD_BaseTheme.Width = (int)(250 * UI.FontScale);
			B_Random.Margin = B_Reset.Margin = B_Save.Margin = UI.Scale(new System.Windows.Forms.Padding(10,0,10,10), UI.FontScale);
			P_UI.Margin = P_Theme.Margin = UI.Scale(new System.Windows.Forms.Padding(10), UI.FontScale);
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			L_UiScale.ForeColor = design.LabelColor;
			P_UI.BackColor = P_Theme.BackColor = design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, -1, 1));
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
			}

			FormDesign.NightModeEnabled = savedNightModeSetting;
			FormDesign.UseSystemTheme = savedUseSystemThemeSetting;
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
					uiScale = UI._instance.uiScale,
				};

				UI._instance.fontFamily = DD_Font.Conversion(DD_Font.SelectedItem);
				UI._instance.fontScale = SS_Scale.Value / 100;
				UI._instance.uiScale = SS_Scale.Value.If(x => x > 100, x => x * .9 + 10, x => x * 1.1 - 10) / 100;
				UI._instance.noAnimations = CB_DisableAnimations.Checked;

				UI.OnUiChanged();
			}

			UI._instance.Save(appName: "SlickUI");

			savedNightModeSetting = CB_NightMode.Checked;
			savedUseSystemThemeSetting = CB_UseSystemTheme.Checked;

			if (!FormDesign.IsCustomEligible())
			{
				FormDesign.Switch(FormDesign.List[UD_BaseTheme.Text], true, true);
			}
			else
			{
				FormDesign.Custom.Temporary = false;
				FormDesign.Custom.DarkMode = null;
				FormDesign.Switch(FormDesign.Custom, true, true);
			}

			savedCustom = FormDesign.Custom;
		}

		private void B_Reset_Click(object sender, EventArgs e)
		{
			if (sender == null || ShowPrompt("Are you sure you want to reset your theme?", PromptButtons.YesNo, PromptIcons.Warning) == System.Windows.Forms.DialogResult.Yes)
			{
				FormDesign.ResetCustomTheme();
				FormDesign.Switch(FormDesign.List[UD_BaseTheme.Text]);
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
						FormDesign.Switch(FormDesign.List[UD_BaseTheme.Text]);
					}
				}

				ColorLoopIdentifier.Enable();

				changesMade = true;
			}
		}

		private void Theme_Changer_Load(object sender, EventArgs e)
		{
			var settings = ISave.LoadRaw("Settings.tf", "SlickUI");
			if (settings == null || !(bool)settings.TutorialShown)
			{
				ISave.Save(new { TutorialShown = true }, "Settings.tf", appName: "SlickUI");

				Notification.Create(LocaleHelper.GetGlobalText("Welcome to Theme Changer!"), LocaleHelper.GetGlobalText("Customize the size and colors in the App however you want to.") + "\r\n" + LocaleHelper.GetGlobalText("Click on any color to change it, or right-click to reset it."), PromptIcons.Info, null)
					.Show(Form, 30);
			}

			changesMade = false;
		}

		private void UD_BaseTheme_TextChanged(object sender, EventArgs e)
		{
			if (UD_BaseThemeIdentifier.Disabled)
			{
				return;
			}

			if (!FormDesign.IsCustomEligible())
			{
				B_Reset_Click(null, null);
			}
			else
			{
				FormDesign.SetCustomBaseDesign(FormDesign.List[UD_BaseTheme.Text]);
			}
		}

		private void B_Random_Click(object sender, EventArgs e)
		{
			UD_BaseTheme.SelectedItem = UD_BaseTheme.Items.Random();
			FormDesign.ResetCustomTheme();
			FormDesign.SetCustomBaseDesign(FormDesign.List[UD_BaseTheme.Text]);

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
		}

		private void CB_DisableAnimations_CheckChanged(object sender, EventArgs e)
		{
			changesMade = true;
		}
	}
}