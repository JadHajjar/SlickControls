﻿using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public partial class ColorPicker : SlickControl
{
	private Color color;

	public ColorPicker()
	{
		AutoScaleMode = AutoScaleMode.None;
		Cursor = Cursors.Hand;
	}

	public event Action<object, bool> ColorChanged;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color Color
	{
		get => DesignMode ? Color.White : (FormDesign.IsCustomEligible() || string.IsNullOrWhiteSpace(ColorName) ? color : ResetColor());
		set
		{
			if (color != value)
			{
				color = value;
				ColorChanged?.Invoke(this, false);
				if (color == value)
				{
					Invalidate();
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Bindable(true)]
	public override string Text { get; set; }

	[Category("Appearance"), DisplayName("Default Color")]
	public Color DefaultColor { get; set; }

	[Category("Behavior"), DisplayName("Color Name")]
	public string ColorName { get; set; }

	protected override void UIChanged()
	{
		Size = UI.Scale(new Size(188, 37), UI.UIScale);
		Padding = UI.Scale(new Padding(5));
	}

	protected override void DesignChanged(FormDesign design)
	{
		Invalidate();

		if (!string.IsNullOrWhiteSpace(ColorName))
		{
			Color = GetDefaultColor();
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			EditColor_Click();
		}
		else if (e.Button == MouseButtons.Middle)
		{
			ResetColor_Click();
		}
		else if (e.Button == MouseButtons.Right)
		{
			SlickToolStrip.Show(new SlickStripItem("Edit Color", "Edit", EditColor_Click), new SlickStripItem("Reset Color", "Undo", ResetColor_Click));
		}
	}

	private void ResetColor_Click()
	{
		Color = ResetColor();
		ColorSetter(Color);

		ColorChanged?.Invoke(this, true);
		Invalidate();

		if (!string.IsNullOrWhiteSpace(ColorName))
		{
			FormDesign.Switch(FormDesign.Custom, true, true);
		}
	}

	private void EditColor_Click()
	{
		var colorDialog = new SlickColorPicker(Color);
		if (!string.IsNullOrWhiteSpace(ColorName))
		{
			colorDialog.ColorChanged += (s, ea) =>
				this.TryInvoke(() =>
				{
					ColorSetter(colorDialog.Color);
					FormDesign.Switch(FormDesign.Custom, false, true);
				});
		}

		if (colorDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		color = colorDialog.Color;
		ColorSetter(color);

		ColorChanged?.Invoke(this, true);
		Invalidate();

		if (!string.IsNullOrWhiteSpace(ColorName))
		{
			FormDesign.Switch(FormDesign.Custom, true, true);
		}
	}

	private Color ResetColor()
	{
		if (string.IsNullOrWhiteSpace(ColorName))
		{
			return DefaultColor;
		}

		var propertyInfo = typeof(FormDesign).GetProperty(ColorName);

		//if (FindForm() is Theme_Changer frm)
		//{
		//	return (Color)propertyInfo.GetValue(FormDesign.List[frm.UD_BaseTheme.Text], null);
		//}

		var p = PanelContent.GetParentPanel(this);

		if (p == null)
		{
			return Color.Empty;
		}

		var design = FormDesign.List[(p as PC_ThemeChanger).savedDesignName];

		if (FormDesign.NightModeEnabled && FormDesign.NightMode && !design.IsDarkTheme)
		{
			design = design.DarkMode;
		}

		if (FormDesign.UseSystemTheme && FormDesign.IsDarkMode && !design.IsDarkTheme)
		{
			design = design.DarkMode;
		}

		return (Color)propertyInfo.GetValue(design, null);
	}

	public Color GetDefaultColor()
	{
		if (string.IsNullOrWhiteSpace(ColorName) || FormDesign.Custom is null)
		{
			return Color.Empty;
		}

		var propertyInfo = typeof(FormDesign).GetProperty(ColorName);

#if NET47
		return (Color)propertyInfo.GetValue(FormDesign.Custom);
#else
		return (Color)propertyInfo.GetValue(FormDesign.Custom, null);
#endif
	}

	public void ColorSetter(Color color)
	{
		if (string.IsNullOrWhiteSpace(ColorName))
		{
			return;
		}

		if (!FormDesign.IsCustomEligible())
		{
			if (FindForm() is Theme_Changer frm)
			{
				FormDesign.SetCustomBaseDesign(FormDesign.List[frm.UD_BaseTheme.Text]);
			}

			var p = PanelContent.GetParentPanel(this);

			if (p != null)
			{
				FormDesign.SetCustomBaseDesign(FormDesign.List[(p as PC_ThemeChanger).savedDesignName]);
			}
		}

		var propertyInfo = typeof(FormDesign).GetProperty(ColorName);
		propertyInfo.SetValue(FormDesign.Custom, color, null);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		base.OnPaintBackground(e);

		e.Graphics.SetUp(Parent.BackColor);

		using var brush = new SolidBrush(Color);
		e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 1 + (int)(UI.FontScale), 1), Padding.Left);

		var bounds = ClientRectangle.Pad(0, 0, 1, 1 + (int)(2.5 * UI.FontScale));
		using var backBrush = new SolidBrush(HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.AccentBackColor.MergeColor(FormDesign.Design.ForeColor, 90) : FormDesign.Design.AccentBackColor);
		e.Graphics.FillRoundedRectangle(backBrush, bounds, Padding.Left);

		var colorRect = bounds.Pad(Padding).Align(UI.Scale(new Size(20, 20)), ContentAlignment.MiddleRight);
		using var brush2 = new SolidBrush(Color.FromArgb(175, WinExtensionClass.ColorFromHSL(Color.GetHue(), Color.GetSaturation(), (1f - Color.GetBrightness()).Between(.2f, .8f))));
		e.Graphics.FillEllipse(brush2, colorRect);
		e.Graphics.FillEllipse(brush, colorRect.Pad(1));

		var textRect = bounds.Pad(Padding.Left, Padding.Top, Padding.Horizontal + colorRect.Width, Padding.Bottom);
		var text = LocaleHelper.GetGlobalText(Text);
		using var font = UI.Font(9.75f).FitToWidth(text, textRect, e.Graphics);
		using var textBrush = new SolidBrush(backBrush.Color.GetTextColor());
		using var stringFormat = new StringFormat { LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(text, font, textBrush, textRect, stringFormat);
	}
}