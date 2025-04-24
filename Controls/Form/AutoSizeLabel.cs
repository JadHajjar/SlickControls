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
	[DefaultValue(true)]
	public bool AutoFit { get; set; } = true;

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

		using var format = new StringFormat
		{
			Alignment = HorizontalAlignment,
			LineAlignment = VerticalAlignment
		};

		if (RightToLeft == RightToLeft.Yes)
		{
			format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
		}

		var width = Math.Min(availableSize.Width - Padding.Horizontal, (int)FontMeasuring.Measure(Text, Font).Width);

		return new Size(width, Math.Min(availableSize.Height, Padding.Vertical + (int)FontMeasuring.Measure(Text, Font, width, format).Height));
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		base.OnPaint(e);

		var font = AutoFit ? new Font(Font, Font.Style).FitTo(Text, ClientRectangle.Pad(Padding), e.Graphics) : Font;
		using var brush = new SolidBrush(ForeColor);
		using var format = new StringFormat
		{
			Alignment = HorizontalAlignment,
			LineAlignment = VerticalAlignment
		};

		if (RightToLeft == RightToLeft.Yes)
		{
			format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
		}

		e.Graphics.DrawString(Text, font, brush, ClientRectangle.Pad(Padding), format);

		if (AutoFit)
		{
			font.Dispose();
		}
	}
}
