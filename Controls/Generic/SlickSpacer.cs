using Extensions;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public class SlickSpacer : Control
{
	[Category("Appearance"), DefaultValue(ColorStyle.Text)]
	public ColorStyle Style { get; set; } = ColorStyle.Text;

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

			using var brush = new SolidBrush(Style is ColorStyle.Text ? FormDesign.Design.AccentColor : Color.FromArgb(200, Style.GetColor()));

			e.Graphics.FillRectangle(brush, new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical));
		}
	}
}