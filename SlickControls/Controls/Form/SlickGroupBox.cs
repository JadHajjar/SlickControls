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

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			using (var font = UI.Font(8.25F, FontStyle.Bold))
			{
				if (string.IsNullOrWhiteSpace(Text))
				{
					e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor), new Rectangle(0, 0, Width - 1, Height - 1), 4);
					return;
				}

				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor), new Rectangle(0, Font.Height / 2 - 1, Width - 1, Height - Font.Height / 2), 4);
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

				var x = Icon == null ? 5 : 26;

				e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(new Point(x, -1), e.Graphics.MeasureString(Text, Font).ToSize()).Pad(5 - x, 0, 0, 0));

				if (Icon != null)
					e.Graphics.DrawImage(new Bitmap(Icon).Color(FormDesign.Design.IconColor), new Rectangle(new Point(9, Font.Height / 2 - 8), Icon.Size));

				e.Graphics.DrawString(Text, Font, new SolidBrush(FormDesign.Design.LabelColor), x + 1, Icon != null ? -1 : -2);
			}
		}
	}
}
