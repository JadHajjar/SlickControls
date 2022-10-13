using Extensions;

using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickSpacer : Control
	{
		public SlickSpacer()
		{
			Height = 1;
			Dock = DockStyle.Top;
			ResizeRedraw = DoubleBuffered = true;
			TabStop = false;

			Paint += SlickSpacer_Paint;
		}

		private void SlickSpacer_Paint(object sender, PaintEventArgs e)
		{
			if (!DesignMode)
			{
				e.Graphics.Clear(BackColor);
				e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.AccentColor), new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical));
			}
		}
	}
}