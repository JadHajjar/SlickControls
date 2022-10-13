using Extensions;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class TopIcon : SlickPictureBox
	{
		public enum IconStyle { Minimize, Maximize, Close }

		public IconStyle Color { get; set; }

		public TopIcon()
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var color = FormDesign.Design.IconColor;

			switch (Color)
			{
				case IconStyle.Maximize:
					color = FormDesign.Design.GreenColor; break;

				case IconStyle.Minimize:
					color = FormDesign.Design.YellowColor; break;

				case IconStyle.Close:
					color = FormDesign.Design.RedColor; break;
			}

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			e.Graphics.FillEllipse(
				SlickControl.Gradient(new Rectangle(Point.Empty, Size), HoverState.HasFlag(HoverState.Hovered) ? color : BackColor.MergeColor(color, 90), HoverState.HasFlag(HoverState.Hovered) ? 3 : 1.5F),
				new RectangleF(Padding.Left, Padding.Top, Width - 1F - Padding.Horizontal, Height - 1F - Padding.Vertical));

			e.Graphics.DrawEllipse(
				new Pen(color, 1.5F),
				new RectangleF(Padding.Left, Padding.Top, Width - 1.5F - Padding.Horizontal, Height - 1.5F - Padding.Vertical));
		}
	}
}