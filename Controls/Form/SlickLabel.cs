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

		[Category("Appearance"), DefaultValue(true)]
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
					Enabled,
					HoverState,
					ColorStyle,
					ColorShade,
					this);
			}
		}
	}
}