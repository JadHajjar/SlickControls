using Extensions;

using MechanikaDesign.WinForms.UI.ColorPicker;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SlickControls;

public partial class SlickColorPicker : SlickForm
{
	public Color Color => Color.FromArgb(255, colorRgb);

	#region Fields

	private HslColor colorHsl = HslColor.FromAhsl(0xff);
	private Color colorRgb = Color.Empty;
	private readonly Color originColor = Color.Empty;
	private bool lockUpdates = false;
	private readonly List<Color> LastColors;

	#endregion Fields

	public event EventHandler ColorChanged;

	public SlickColorPicker(Color color)
	{
		InitializeComponent();

		TB_Hex.ValidationCustom = x => Regex.IsMatch(x, @"#?([a-f]|[0-9]){6}", RegexOptions.IgnoreCase);

		ISave.Load(out LastColors, "LastColors.tf", "SlickUI");
		LastColors = LastColors?.Take(21).ToList() ?? [];
		ShowLastColors();

		originColor = color;
		SetColor(color.If(Color.Empty, Color.Black), null);

		ColorChanged += SlickColorPicker_ColorChanged;
		TLP_Main.MouseDown += Form_MouseDown;
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_Main.BackColor = design.BackColor;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		colorPreview.Size = UI.Scale(new Size(48, 48), UI.FontScale);
		TLP_Main.ColumnStyles[0].Width = (int)(108 * UI.FontScale);
		TLP_Main.Padding = UI.Scale(new Padding(10), UI.FontScale);
		B_Cancel.Margin = B_Confirm.Margin = UI.Scale(new Padding(5, 5, 0, 0), UI.FontScale);
		TLP_Main.Controls.OfType<SlickTextBox>().Foreach(x => x.Width = (int)(60 * UI.FontScale));
	}

	private void SlickColorPicker_ColorChanged(object sender, EventArgs e)
	{
		this.TryInvoke(() => ShowLastColors(true));
	}

	private void ShowLastColors(bool incremental = false)
	{
		if (!incremental)
		{
			FLP_LastColors.Controls.Clear();
			foreach (var color in LastColors)
			{
				AddColor(color);
			}
		}
		else
		{
			if (LastColors.Any(x => x == Color))
			{
				LastColors.RemoveAll(x => x == Color);
			}

			foreach (var item in FLP_LastColors.Controls.Where(x => x.BackColor == Color))
			{
				FLP_LastColors.Controls.Remove(item);
			}

			if (FLP_LastColors.Controls.Count >= 21)
			{
				FLP_LastColors.Controls.RemoveAt(0);
			}

			LastColors.Insert(0, Color);
			AddColor(LastColors[0]);
		}
	}

	private void AddColor(Color color)
	{
		var ctrl = new Panel() { Size = UI.Scale(new Size(28, 28), UI.FontScale), BackColor = color, Cursor = Cursors.Hand, Margin = UI.Scale(new Padding(4), UI.FontScale) };
		ctrl.Paint += colorPreview_Paint;
		ctrl.Click += (s, e) => SetColor(color, null);
		FLP_LastColors.Controls.Add(ctrl);
		ctrl.BringToFront();
	}

	private void RGB_TextChanged(object sender, EventArgs e)
	{
		(sender as SlickTextBox).Text = (sender as SlickTextBox).Text.SmartParse().Between(0, 255).ToString();
		if (!lockUpdates)
		{
			SetColor(Color.FromArgb(TB_Red.Text.SmartParse(), TB_Green.Text.SmartParse(), TB_Blue.Text.SmartParse()), null);
		}
	}

	private void HSL_TextChanged(object sender, EventArgs e)
	{
		(sender as SlickTextBox).Text = (sender as SlickTextBox).Text.SmartParse().Between(0, sender == TB_Hue ? 360 : 100).ToString();
		if (!lockUpdates)
		{
			SetColor(null, HslColor.FromAhsl(TB_Hue.Text.SmartParse() / 360D, TB_Sat.Text.SmartParse() / 100D, TB_Lum.Text.SmartParse() / 100D));
		}
	}

	private void TB_Hex_TextChanged(object sender, EventArgs e)
	{
		if (!lockUpdates && TB_Hex.ValidInput)
		{
			var grps = Regex.Match(TB_Hex.Text.ToLower(), @"#?((?:[a-f]|[0-9]){2})((?:[a-f]|[0-9]){2})((?:[a-f]|[0-9]){2})", RegexOptions.IgnoreCase).Groups;
			SetColor(Color.FromArgb(
				int.Parse(grps[1].Value, System.Globalization.NumberStyles.HexNumber),
				int.Parse(grps[2].Value, System.Globalization.NumberStyles.HexNumber),
				int.Parse(grps[3].Value, System.Globalization.NumberStyles.HexNumber)), null);
		}
	}

	private void SetColor(Color? color, HslColor? colorhsl, bool changeSlider = true)
	{
		lockUpdates = true;

		if (changeSlider)
		{
			if (color == null)
			{
				colorSlider.ColorHSL = (HslColor)colorhsl;
				colorBox2D.ColorHSL = (HslColor)colorhsl;
			}
			else
			{
				colorSlider.ColorRGB = (Color)color;
				colorBox2D.ColorRGB = (Color)color;
			}
		}

		colorHsl = colorhsl ?? HslColor.FromColor((Color)color);
		colorRgb = color ?? colorHsl.RgbValue;

		colorPreview.BackColor = colorRgb;
		TB_Hex.Text = ColorTranslator.ToHtml(colorRgb);

		TB_Red.Text = colorRgb.R.ToString();
		TB_Green.Text = colorRgb.G.ToString();
		TB_Blue.Text = colorRgb.B.ToString();

		TB_Hue.Text = ((int)Math.Round(colorHsl.H * 360D)).ToString();
		TB_Sat.Text = ((int)Math.Round(colorHsl.S * 100D)).ToString();
		TB_Lum.Text = ((int)Math.Round(colorHsl.L * 100D)).ToString();

		lockUpdates = false;

		WaitIdentifier.Wait(() => ColorChanged?.Invoke(this, new EventArgs()), 250);
	}

	private readonly WaitIdentifier WaitIdentifier = new();

	private void colorBox2D_ColorChanged(object sender, ColorChangedEventArgs args)
	{
		if (sender == colorSlider)
		{
			TB_Hue.Text = ((int)Math.Round(colorSlider.ColorHSL.H * 360D)).ToString();
		}
		else if (sender == colorBox2D)
		{
			lockUpdates = true;
			TB_Sat.Text = ((int)Math.Round(colorBox2D.ColorHSL.S * 100D)).ToString();
			lockUpdates = false;
			TB_Lum.Text = ((int)Math.Round(colorBox2D.ColorHSL.L * 100D)).ToString();
		}
	}

	private void B_Confirm_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;
		Close();

		try
		{
			ISave.Load(out List<Color> colors, "LastColors.tf", "SlickUI");
			colors ??= [];

			colors.Insert(0, Color);
			ISave.Save(colors.Take(21), "LastColors.tf", appName: "SlickUI");
		}
		catch { }
	}

	private void B_Cancel_Click(object sender, EventArgs e)
	{
		SetColor(originColor, null);
		DialogResult = DialogResult.Cancel;
		Close();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Enter)
		{
			B_Confirm_Click(null, null);
			return true;
		}

		if (keyData == Keys.Escape)
		{
			B_Cancel_Click(null, null);
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void colorPreview_Paint(object sender, PaintEventArgs e)
	{
		var size = (sender as Control).Size;
		var color = (sender as Control).BackColor;
		if (color == null)
		{
			return;
		}

		e.Graphics.Clear(color.GetAccentColor());
		e.Graphics.FillRectangle(new SolidBrush(color), new Rectangle(1, 1, size.Width - 2, size.Height - 2));
	}

	private void colorBox2D_SizeChanged(object sender, EventArgs e)
	{
		colorBox2D.Width = colorBox2D.Height;
	}
}