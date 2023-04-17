using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("Click")]
	public class SlickIcon : SlickImageControl
	{
		private bool enableGraphics = true;

		private bool selected = false;

		public SlickIcon()
		{
			Cursor = Cursors.Hand;
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Func<Color> ActiveColor { get; set; }

		[Category("Behavior")]
		public new bool Enabled
		{
			get => enableGraphics;
			set
			{
				enableGraphics = value;
				Cursor = value ? Cursors.Hand : Cursors.Default;
			}
		}

		[Category("Appearance"), DisplayName("Color Style"), DefaultValue(ColorStyle.Active)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Active;

		[Category("Behavior"), DefaultValue(false)]
		public bool Selected { get => selected; set { selected = value; Invalidate(); } }

		public void Hold()
		{
			Selected = true;
			Invalidate();
		}

		public void Release()
		{
			Selected = false;
			Invalidate();
		}

		public void Disable()
		{
			Enabled = false;
		}

		public void Enable()
		{
			Enabled = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);

			if (Loading)
			{
				DrawLoader(e.Graphics, ClientRectangle.CenterR(Image?.Size ?? Size));
				return;
			}

			if (Image == null)
			{
				return;
			}

			try
			{
				var activeColor = ActiveColor?.Invoke() ?? ColorStyle.GetColor();

				var color =
					Selected ? activeColor :
					!Enabled ? ForeColor :
					HoverState.HasFlag(HoverState.Pressed) ? activeColor :
					HoverState.HasFlag(HoverState.Hovered) ? activeColor.MergeColor(ForeColor) :
					ForeColor;

				using (var img = new Bitmap(Image).Color(color))
				{
					e.Graphics.DrawImage(img, ClientRectangle.CenterR(img.Size));
				}
			}
			catch { }
		}
	}
}