using Extensions;

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace SlickControls
{
	public partial class Theme_Changer : BaseForm
	{
		private readonly DisableIdentifier ColorLoopIdentifier = new DisableIdentifier();

		private readonly DisableIdentifier UD_BaseThemeIdentifier = new DisableIdentifier();

		public static Theme_Changer ThemeForm { get; internal set; }

		public Theme_Changer()
		{
			InitializeComponent();
			verticalScroll1.LinkedControl = FLP_Pickers;
			UD_BaseTheme.Items = FormDesign.List.ToArray();

			if (FormDesign.IsCustomEligible())
				FormDesign.Switch(FormDesign.Custom);

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
			SS_Scale.Value = UI.FontScale * 100;
			SS_Scale.ValueOutput = (x) => $"{x}%";
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			DD_Font.Width = UD_BaseTheme.Width = (int)(300 * UI.FontScale);
		}

		private void SS_FontSize_ValuesChanged(object sender, EventArgs e) => SS_Scale.Value -= SS_Scale.Value % 5;

		private void SS_Scale_ValuesChanged(object sender, EventArgs e) => SS_Scale.Value -= SS_Scale.Value % 5;

		private void B_Save_Click(object sender, EventArgs e)
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

			UI._instance.Save(appName: "Shared");
			UI.OnUiChanged();
		}

		private void B_Reset_Click(object sender, EventArgs e)
		{
			FormDesign.ResetCustomTheme();
			FormDesign.Switch(FormDesign.List[UD_BaseTheme.Text]);
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
			}
		}

		private void Theme_Changer_Load(object sender, EventArgs e)
		{
			var settings = ISave.LoadRaw("Settings.tf", "Shared");
			if (settings == null || !(bool)settings.TutorialShown)
			{
				ISave.Save(new { TutorialShown = true }, "Settings.tf", appName: "Shared");
				Notification.Create("Welcome to Theme Changer!", "Customize the size and colors in the App to fit your desire.\nClick on any color-square to change it, right-click the square to reset it.", PromptIcons.Info, null)
					.Show(this, 10);
			}
		}

		private void UD_BaseTheme_TextChanged(object sender, EventArgs e)
		{
			if (UD_BaseThemeIdentifier.Disabled) return;

			if (!FormDesign.IsCustomEligible())
				B_Reset_Click(null, null);
			else
				FormDesign.SetCustomBaseDesign(FormDesign.List[UD_BaseTheme.Text]);
		}

		private void Theme_Changer_Resize(object sender, EventArgs e)
		{
			FLP_Pickers.MaximumSize = new Size(panel1.Width, 9999);
			FLP_Pickers.Left = (panel1.Width - FLP_Pickers.Width) / 2;
		}

		private void B_Random_Click(object sender, EventArgs e)
		{
			UD_BaseTheme.SelectedItem = UD_BaseTheme.Items.Random();

			foreach (var item in FLP_Pickers.Controls.OfType<ColorPicker>())
			{
				item.Color = Color.FromArgb(ExtensionClass.RNG.Next(256), ExtensionClass.RNG.Next(256), ExtensionClass.RNG.Next(256));
				item.ColorSetter(Color.FromArgb(ExtensionClass.RNG.Next(256), ExtensionClass.RNG.Next(256), ExtensionClass.RNG.Next(256)));
			}
			FormDesign.Switch(FormDesign.Custom, false, true);
		}
	}
}