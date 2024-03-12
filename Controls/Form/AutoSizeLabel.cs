using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;
public class AutoSizeLabel : SlickControl
{
	[DefaultValue(StringAlignment.Near)]
	public StringAlignment HorizontalAlignment { get; set; }
	[DefaultValue(StringAlignment.Near)]
	public StringAlignment VerticalAlignment { get; set; }

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override Font Font { get => base.Font; set => base.Font = value; }

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text { get => base.Text; set => base.Text = value; }

	public AutoSizeLabel()
	{
		DoubleBuffered = ResizeRedraw = true;
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!AutoSize)
		{
			return base.GetPreferredSize(proposedSize);
		}

		var availableSize = GetAvailableSize();

		return new Size(proposedSize.Width, Math.Min(availableSize.Height, Padding.Vertical + (int)FontMeasuring.Measure(Text, Font, availableSize.Width - Padding.Horizontal).Height));
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
