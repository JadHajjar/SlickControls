using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

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
	public bool HasAction { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool Selected
	{
		get => selected; set
		{
			selected = value;
			Invalidate();
		}
	}

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

		using var image = ImageName?.Get(Math.Min(Height - Padding.Vertical, Width - Padding.Horizontal)) ?? Image;
		if (Loading)
		{
			DrawLoader(e.Graphics, ClientRectangle.CenterR(image?.Size ?? Size));
			return;
		}

		if (image == null)
		{
			return;
		}

		try
		{
			var activeColor = ActiveColor?.Invoke() ?? ColorStyle.GetColor();

			var color =
				Selected ? activeColor :
				!base.Enabled && HasAction ? ForeColor.MergeColor(BackColor, 40) :
				!Enabled ? ForeColor :
				HoverState.HasFlag(HoverState.Pressed) ? activeColor :
				HoverState.HasFlag(HoverState.Hovered) ? activeColor.MergeColor(ForeColor) :
				ForeColor;

			if (Enabled && HoverState.HasFlag(HoverState.Hovered))
			{
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(50, color.MergeColor(ForeColor, 65))), ClientRectangle.Pad(1), (int)(4 * UI.FontScale));
			}

			if (Enabled)
			{
				DrawFocus(e.Graphics, activeColor);
			}

			e.Graphics.DrawImage(image.Color(color), ClientRectangle.CenterR(image.Size));
		}
		catch { }
	}
}