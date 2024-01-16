using Extensions;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SlickImageBackgroundPanel : SlickImageBackgroundControl
{
	private int scrollY;
	private Padding paintPadding;
	public SlickImageBackgroundControlCollection Controls { get; }
	public bool AutoSize { get; set; }
	public bool AutoScroll { get; set; }

	public SlickImageBackgroundPanel()
	{
		Controls = new SlickImageBackgroundControlCollection(this);
		AutoInvalidate = false;
	}

	public override void CalculateSize(PaintEventArgs e)
	{
		if (AutoSize && Controls.Count > 0)
		{
			foreach (var control in Controls)
			{
				control.CalculateSize(e);
				SetControlBounds(control);
				PostControlPaint(control, false);
			}

			var calculatedSize = new Size(Padding.Horizontal + Controls.Max(x => x.Bounds.X + x.Width), Padding.Vertical + Controls.Max(x => x.Bounds.Y + x.Height));

			switch (Dock)
			{
				case DockStyle.None:
					Size = calculatedSize;
					break;
				case DockStyle.Top:
					Height = calculatedSize.Height;
					break;
				case DockStyle.Bottom:
					Height = calculatedSize.Height;
					break;
				case DockStyle.Left:
					Width = calculatedSize.Width;
					break;
				case DockStyle.Right:
					Width = calculatedSize.Width;
					break;
			}
		}

		base.CalculateSize(e);
	}

	protected virtual void SetControlBounds(SlickImageBackgroundControl control)
	{
		switch (control.Dock)
		{
			case DockStyle.Top:
				control.Bounds = new Rectangle(paintPadding.Left, paintPadding.Top, Width - paintPadding.Horizontal, control.Height);
				break;
			case DockStyle.Left:
				control.Bounds = new Rectangle(paintPadding.Left, paintPadding.Top, control.Width, Height - paintPadding.Vertical);
				break;
			case DockStyle.Fill:
				control.Bounds = new Rectangle(paintPadding.Left, paintPadding.Top, Width - paintPadding.Horizontal, Height - paintPadding.Vertical);
				break;
			case DockStyle.Bottom:
				control.Bounds = new Rectangle(paintPadding.Left, Height - paintPadding.Bottom - control.Height - Padding.Top, Width - paintPadding.Horizontal, control.Height);
				break;
			case DockStyle.Right:
				control.Bounds = new Rectangle(Width - paintPadding.Right - control.Width - Padding.Left, paintPadding.Top, control.Width, Height - paintPadding.Vertical);
				break;
		}
	}

	protected virtual void PostControlPaint(SlickImageBackgroundControl control, bool painted)
	{
		switch (control.Dock)
		{
			case DockStyle.Top:
				paintPadding.Top += control.Height + control.Margin.Vertical;
				break;
			case DockStyle.Bottom:
				paintPadding.Bottom += control.Height + control.Margin.Vertical;
				break;
			case DockStyle.Left:
				paintPadding.Left += control.Width + control.Margin.Horizontal;
				break;
			case DockStyle.Right:
				paintPadding.Right += control.Width + control.Margin.Horizontal;
				break;
		}
	}

	public override void OnPaint(PaintEventArgs e)
	{
		paintPadding = Padding;
		paintPadding.Top += scrollY;
		var parentLoc = ContainerLocation();

		foreach (var dockGrp in Controls.GroupBy(x => x.Dock).OrderBy(x => x.Key))
		{
			foreach (var control in dockGrp)
			{
				try
				{
					SetControlBounds(control);

					control.CalculateSize(e);
					control.OnPrePaint(e);

					if (control.Visible)
					{
						var parent = this;
						var drawRectangle = new Rectangle(control.ContainerLocation(), control.Size);

						while (parent != null)
						{
							drawRectangle = Rectangle.Intersect(drawRectangle, new Rectangle(parent.ContainerLocation(), parent.Size));
							parent = parent.Parent;
						}

						if (e.ClipRectangle.IntersectsWith(drawRectangle))
						{
							e.Graphics.ResetTransform();
							e.Graphics.SetClip(drawRectangle);
							e.Graphics.TranslateTransform(parentLoc.X + control.Bounds.X, parentLoc.Y + control.Bounds.Y);
							e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
							e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

							try
							{
								control.OnPaint(e);
							}
							catch { }
						}

						if (control.Visible)
						{
							PostControlPaint(control, e.ClipRectangle.IntersectsWith(drawRectangle));
						}
					}
				}
				catch { }
			}
		}

		if (AutoScroll && HoverState.HasFlag(HoverState.Hovered))
		{
			var maxHeight = Parent.Margin.Top + Controls.Max(x => x.Bounds.Y + x.Bounds.Height - scrollY);
			if (scrollY != scrollY.Between(Height - maxHeight, 0))
			{
				scrollY = scrollY.Between(Height - maxHeight, 0);
				Invalidate();
			}

			if (maxHeight > Height)
			{
				e.Graphics.ResetTransform();
				e.Graphics.ResetClip();
				e.Graphics.TranslateTransform(ContainerLocation().X, ContainerLocation().Y);
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, FormDesign.Design.ForeColor)), new Rectangle(Width - 2, 0, 1, Height).Pad(0, 7, 0, 7));
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, FormDesign.Design.ForeColor)), new Rectangle(Width - 2, -scrollY * Height / maxHeight, 1, Height * Height / maxHeight).Pad(0, 7, 0, 7));

				if (scrollY != 0)
				{
					e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.InfoColor), new RectangleF(5, 0, Width - 10, 1));
				}
			}
		}
	}

	internal override SlickImageBackgroundControl HandleMouseMove(Point point)
	{
		var c = (SlickImageBackgroundControl)null;

		if (Visible)
		{
			var hoverState = DrawBounds.Contains(point) ? HoverState.Hovered : HoverState.Normal;

			if (hoverState.HasFlag(HoverState.Hovered))
			{
				foreach (var item in Controls)
				{
					c = item.HandleMouseMove(new Point(point.X - item.Bounds.X, point.Y - item.Bounds.Y)) ?? c;
				}
			}
			else
			{
				foreach (var item in Controls)
				{
					item.HandleMouseMove(new Point(int.MinValue, int.MinValue));
				}
			}

			if (hoverState != HoverState)
			{
				HoverState = hoverState;

				if (AutoScroll)
				{
					var maxHeight = Parent.Margin.Top + Controls.Max(x => x.Bounds.Y + x.Bounds.Height - scrollY);

					if (maxHeight > Height)
					{
						Invalidate(new Rectangle(Width - 2, 0, 2, Height));
					}
				}
			}
		}

		return c;
	}

	public override bool OnMouseWheel(MouseEventArgs e)
	{
		if (AutoScroll)
		{
			var maxHeight = Parent.Margin.Top + Controls.Max(x => x.Bounds.Y + x.Bounds.Height - scrollY);
			if (maxHeight > Height)
			{
				var val = (scrollY + e.Delta).Between(Height - maxHeight, 0);

				if (scrollY != val)
				{
					scrollY = val;
					Invalidate();
					return true;
				}
			}
		}

		foreach (var item in Controls)
		{
			if (item.Visible
				&& item.Bounds.Contains(e.Location)
				&& item.OnMouseWheel(new MouseEventArgs(e.Button, e.Clicks, e.X - item.Bounds.X, e.Y - item.Bounds.Y, e.Delta)))
			{
				return true;
			}
		}

		return false;
	}

	public override void OnMouseClick(MouseEventArgs e)
	{
		foreach (var item in Controls)
		{
			if (item.Visible && item.Bounds.Contains(e.Location))
			{
				item.OnMouseClick(new MouseEventArgs(e.Button, e.Clicks, e.X - item.Bounds.X, e.Y - item.Bounds.Y, e.Delta));
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			foreach (var item in Controls.ToList())
			{
				item.Dispose();
			}
		}

		base.Dispose(disposing);
	}
}