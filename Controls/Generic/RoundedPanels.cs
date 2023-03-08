using Extensions;

using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class RoundedPanel : DBPanel
	{
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			using (var brush = new SolidBrush(BackColor))
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
		}
	}

	public class RoundedTableLayoutPanel : DBTableLayoutPanel
	{
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			using (var brush = new SolidBrush(BackColor))
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
		}
	}

	public class RoundedFlowLayoutPanel : DBFlowLayoutPanel
	{
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			using (var brush = new SolidBrush(BackColor))
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
		}
	}
}