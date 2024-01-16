using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public class SlickImageBackgroundContainer : SlickAdvancedImageControl
{
	private readonly SlickImageBackgroundPanel Panel;
	private bool panelMoved;

	public SlickImageBackgroundControlCollection Content => Panel.Controls;

	[Category("Layout")]
	public Padding ContentPadding { get; set; }

	public SlickImageBackgroundControl HoveredControl { get; private set; }

	public SlickImageBackgroundContainer()
	{
		Panel = new SlickImageBackgroundPanel { Dock = DockStyle.Fill, Container = this };
		AutoInvalidate = false;
	}

	protected override void OnImageChanged(EventArgs e)
	{ }

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		base.OnPaintBackground(e);

		if (Image != null)
		{
			e.Graphics.DrawImage(Image, new Rectangle(Point.Empty, Size), ImageSizeMode.CenterScaled);

			e.Graphics.FillRectangle(Gradient(Color.FromArgb(185, FormDesign.Design.BackColor), 2), new Rectangle(Point.Empty, Size));
		}
		else
		{
			e.Graphics.FillRectangle(Gradient(BackColor), new Rectangle(Point.Empty, Size));
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Panel.Padding = ContentPadding;
		Panel.Bounds = new Rectangle(Point.Empty, Size);
		Panel.OnPaint(e);

		if (panelMoved)
		{
			var pnt = PointToClient(Cursor.Position);
			OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, pnt.X, pnt.Y, 0));
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		HoveredControl = Panel.HandleMouseMove(e.Location);

		Cursor = HoveredControl?.Cursor ?? Cursors.Default;
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		Panel.OnMouseClick(e);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		panelMoved = Panel.OnMouseWheel(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);

		Panel.HandleMouseMove(new Point(-1, -1));
		HoveredControl = null;
		Cursor = Cursors.Default;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);

		Panel.HandleMouseMove(e.Location);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);

		Panel.HandleMouseMove(e.Location);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Panel?.Dispose();
		}

		base.Dispose(disposing);
	}
}