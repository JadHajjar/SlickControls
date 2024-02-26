using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls;
public class AutoSizeLabel : SlickControl
{
	[DefaultValue(StringAlignment.Near)]
    public StringAlignment HorizontalAlignment { get; set; }
	[DefaultValue(StringAlignment.Near)]
    public StringAlignment VerticalAlignment { get; set; }

    public AutoSizeLabel()
    {
        DoubleBuffered = ResizeRedraw = true;
    }

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		base.OnPaint(e);

		using var font = new Font(Font, Font.Style).FitTo(Text, ClientRectangle.Pad(Padding), e.Graphics);
		using var brush = new SolidBrush(ForeColor);
		using var format = new StringFormat
		{
			Alignment = HorizontalAlignment,
			LineAlignment = VerticalAlignment
		};

		e.Graphics.DrawString(Text, font, brush, ClientRectangle.Pad(Padding), format);
	}
}
