using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("Click")]
	public partial class SlickLabel : SlickButton
	{
		private bool _selected;

		[Category("Appearance"), DefaultValue(false)]
		public bool Display { get; set; }

		public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }

		public SlickLabel()
		{
			AutoHideText = false;
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
				fore = FormDesign.Design.ForeColor.Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, -7, 7));
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

			using (var img = AutoSizeIcon ? ImageName.Get(Height - Padding.Vertical) : Image)
			{
				GetColors(out var fore, out var back);

				DrawButton(e,
					Point.Empty,
					Size,
					LocaleHelper.GetGlobalText(Text),
					Font,
					back,
					fore,
					img,
					Padding,
					true,
					HoverState,
					ColorStyle,
					ColorShade,
					this);
			}
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
				fore = colorStyle.GetBackColor().Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, -7, 7));
				back = Color.FromArgb(160, colorStyle.GetColor());
			}
			else
			{
				fore = foreColor?? FormDesign.Design.ForeColor;
				back = Color.Empty;
			}

			DrawButton(e,
				rectangle.Location	,
				rectangle.Size,
				text,
				font,
				back,
				fore,
				icon,
				padding ?? UI.Scale(new Padding(7), UI.UIScale),
				true,
				hoverState,
				colorStyle,
				null);
		}
	}
}