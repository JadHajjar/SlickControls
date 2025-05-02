using Extensions;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

[DefaultEvent("Click")]
public partial class SlickLabel : SlickButton
{
	private bool _selected;

	[Category("Appearance"), DefaultValue(false)]
	public bool Display { get; set; }

	[Category("Appearance"), DefaultValue(null)]
	public Color? CustomBackColor { get; set; }

	public bool Selected
	{
		get => _selected; set
		{
			_selected = value;
			Invalidate();
		}
	}

	public SlickLabel()
	{
		AlignLeft = true;
	}

	protected virtual void GetColors(out Color fore, out Color back)
	{
		if (Selected || HoverState.HasFlag(HoverState.Pressed))
		{
			fore = ColorStyle.GetBackColor().Tint(ColorShade?.GetHue());
			back = ColorShade == null ? ColorStyle.GetColor() : ColorStyle.GetColor().Tint(ColorShade?.GetHue()).MergeColor((Color)ColorShade);
		}
		else if (HoverState.HasFlag(HoverState.Hovered))
		{
			fore = FormDesign.Design.ForeColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? -7 : 7);
			back = FormDesign.Design.ButtonColor.MergeColor(BackColor, 75);
		}
		else
		{
			fore = Enabled || Display ? ForeColor : ForeColor.MergeColor(BackColor);
			back = Color.Empty;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		GetColors(out var fore, out var back);

		if (CustomBackColor.HasValue)
		{
			back = CustomBackColor.Value;
			fore = back.GetTextColor();
		}

		Draw(e.Graphics, new ButtonDrawArgs
		{
			LeftAlign = true,
			ForeColor = fore,
			BackColor = back,
			Control = this,
			Font = Font,
			Icon = ImageName,
			Image = image,
			Text = Text,
			Padding = Padding,
			ButtonType = ButtonType == ButtonType.Active ? ButtonType.Active : ButtonType.Hidden,
			ColorShade = ColorShade,
			ColorStyle = ColorStyle,
			HoverState = HoverState,
			Rectangle = ClientRectangle
		});
	}

	public static void DrawLabel(
		PaintEventArgs e,
		Rectangle rectangle,
		string text,
		Font font,
		Image icon,
		Padding? padding = null,
		Color? foreColor = null,
		HoverState hoverState = HoverState.Normal,
		ColorStyle colorStyle = ColorStyle.Active)
	{
		Color back, fore;
		if (hoverState.HasFlag(HoverState.Pressed))
		{
			fore = colorStyle.GetBackColor();
			back = colorStyle.GetColor();
		}
		else if (hoverState.HasFlag(HoverState.Hovered))
		{
			fore = colorStyle.GetBackColor().Tint(Lum: FormDesign.Design.IsDarkTheme ? -7 : 7);
			back = Color.FromArgb(160, colorStyle.GetColor());
		}
		else
		{
			fore = foreColor ?? FormDesign.Design.ForeColor;
			back = Color.Empty;
		}

		Draw(e.Graphics, new ButtonDrawArgs
		{
			LeftAlign = true,
			ForeColor = fore,
			BackColor = back,
			Font = font,
			Image = icon,
			Text = text,
			Padding = padding ?? default,
			ColorStyle = colorStyle,
			HoverState = hoverState,
			Rectangle = rectangle,
			ButtonType = ButtonType.Hidden
		});
	}
}