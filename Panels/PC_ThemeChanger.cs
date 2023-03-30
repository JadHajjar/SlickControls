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
		private FormDesign savedCustom = FormDesign.Custom;

		public PC_ThemeChanger()
		{
			InitializeComponent();
			verticalScroll1.LinkedControl = FLP_Pickers;
			UD_BaseTheme.Items = FormDesign.List.ToArray();
			CB_NightMode.Checked = savedNightModeSetting = FormDesign.NightModeEnabled;
			FormDesign.NightModeEnabled = false;
			FormDesign.Custom.Temporary = true;

			if (FormDesign.IsCustomEligible())
				FormDesign.Switch(FormDesign.Custom, forceRefresh: true);

			UD_BaseThemeIdentifier.Disable();
			UD_BaseTheme.Text = FormDesign.List[FormDesign.Design.ID].Name;
			UD_BaseThemeIdentifier.Enable();

			using (var fontsCollection = new InstalledFontCollection())
			{
				DD_Font.Items = fontsCollection.Families;
				DD_Font.Conversion = (x) => (x as FontFamily)?.Name ?? x?.ToString();
			}

			DD_Font.FontDropdown = true;
			DD_Font.SelectedItem = UI.FontFamily;
			SS_Scale.Value = UI._instance.fontScale * 100;
			SS_Scale.ValueOutput = (x) => $"{x}%";
			CB_DisableAnimations.Checked = AnimationHandler.NoAnimations;

			changesMade = false;
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			DD_Font.Width = UD_BaseTheme.Width = (int)(300 * UI.FontScale);
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			label1.ForeColor = design.LabelColor;
		}

		public override bool CanExit(bool toBeDisposed)
		{
			if (changesMade && ShowPrompt("Do you want to apply your changes before leaving?", PromptButtons.YesNo, PromptIcons.Hand) == System.Windows.Forms.DialogResult.Yes)
			{
				B_Save_Click(null, null);
			}
			else
				FormDesign.Custom = savedCustom;

			FormDesign.NightModeEnabled = savedNightModeSetting;
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
			if (UI._instance.fontScale != SS_Scale.Value || UI._instance.noAnimations != CB_DisableAnimations.Checked || UI.FontFamily != DD_Font.Conversion(DD_Font.SelectedItem).IfEmpty("Nirmala UI"))
			{
				var previous = new UI
				{
					fontFamily = UI._instance.fontFamily,
					fontScale = UI._instance.fontScale,
					uiScale = UI._instance.uiScale,
				};

				UI._instance.fontFamily = DD_Font.Conversion(DD_Font.SelectedItem).IfEmpty("Nirmala UI");
				UI._instance.fontScale = SS_Scale.Value / 100;
				UI._instance.uiScale = SS_Scale.Value.If(x => x > 100, x => x * .9 + 10, x => x * 1.1 - 10) / 100;
				UI._instance.noAnimations = CB_DisableAnimations.Checked;

				UI.OnUiChanged();
			}

			UI._instance.Save(appName: "Shared");

			savedNightModeSetting = CB_NightMode.Checked;

			if (!FormDesign.IsCustomEligible())
				FormDesign.Switch(FormDesign.List[UD_BaseTheme.Text], true, true);
			else
			{
				FormDesign.Custom.Temporary = false;
				FormDesign.Custom.DarkMode = null;
				FormDesign.Switch(FormDesign.Custom, true, true);
			}

			savedCustom = FormDesign.Custom;

			changesMade = false;

			if (sender != null)
				Form.PushBack();
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
							item.Refresh();
					}

					if (!FormDesign.IsCustomEligible())
						FormDesign.Switch(FormDesign.List[UD_BaseTheme.Text]);
				}

				ColorLoopIdentifier.Enable();

				changesMade = true;
			}
		}

		private void Theme_Changer_Load(object sender, EventArgs e)
		{
			var settings = ISave.LoadRaw("Settings.tf", "Shared");
			if (settings == null || !(bool)settings.TutorialShown)
			{
				ISave.Save(new { TutorialShown = true }, "Settings.tf", appName: "Shared");
				Notification.Create("Welcome to Theme Changer!", "Customize the size and colors in the App to fit your desire.\nClick on any color-square to change it, right-click the square to reset it.", PromptIcons.Info, null)
					.Show(Form, 10);
			}

			changesMade = false;
		}

		private void UD_BaseTheme_TextChanged(object sender, EventArgs e)
		{
			if (UD_BaseThemeIdentifier.Disabled) return;

			if (!FormDesign.IsCustomEligible())
				B_Reset_Click(null, null);
			else
				FormDesign.SetCustomBaseDesign(FormDesign.List[UD_BaseTheme.Text]);
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
		}
	}
}