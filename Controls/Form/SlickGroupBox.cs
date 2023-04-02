using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickGroupBox : GroupBox
	{
		[Category("Appearance"), DefaultValue(null)]
		public Image Icon { get; set; }

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			if (!DesignMode)
			{
				using (var g = Graphics.FromHwnd(IntPtr.Zero))
					Padding = new Padding(
						Padding.Left,
						(int)g.Measure(Text, UI.Font(8.25F * (float)UI.WindowsScale, FontStyle.Bold)).Height / 2 - 6,
						Padding.Right,
						Padding.Bottom);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			using (var font = UI.Font(8.25F * (float)UI.WindowsScale, FontStyle.Bold))
			{
				if (string.IsNullOrWhiteSpace(Text))
				{
					e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor), new Rectangle(0, 0, Width - 1, Height - 1), 4);
					return;
				}

				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor), new Rectangle(0, font.Height / 2 - 1, Width - 1, Height - font.Height / 2), 4);
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

				var x = Icon == null ? 5 : 26;

				e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(new Point(x, -1), UI.Scale(e.Graphics.Measure(Text, font).ToSize(), 1 / UI.WindowsScale)).Pad(5 - x, 0, 0, 0));

				if (Icon != null)
					e.Graphics.DrawImage(new Bitmap(Icon).Color(FormDesign.Design.IconColor), new Rectangle(new Point(9, font.Height / 2 - 8), Icon.Size));

				e.Graphics.DrawString(Text, font, new SolidBrush(FormDesign.Design.LabelColor), x + 1, Icon != null ? -1 : -2);
			}
		}
	}
}
