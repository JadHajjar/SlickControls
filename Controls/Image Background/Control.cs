using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickImageBackgroundControl : IDisposable
	{
		public event PaintEventHandler Paint;

		public event PaintEventHandler CalculateAutoSize;

		public event PaintEventHandler PrePaint;

		public event Extensions.EventHandler<HoverState> HoverStateChanged;

		public event MouseEventHandler MouseClick;

		public event MouseEventHandler MouseWheel;

		public event EventHandler ImageChanged;

		private readonly Timer timer = new Timer();
		private bool loading;
		private Rectangle bounds;
		private HoverState hoverState = HoverState.Normal;

		[Browsable(false)]
		public SlickImageBackgroundPanel Parent { get; internal set; }

		[Browsable(false)]
		public SlickImageBackgroundContainer Container { get; internal set; }

		[Category("Layout")]
		public AnchorStyles Anchor { get; set; } = AnchorStyles.Top | AnchorStyles.Left;

		[Category("Layout")]
		public DockStyle Dock { get; set; }

		[Category("Layout")]
		public bool Visible { get; set; } = true;

		[Category("Layout")]
		public bool Enabled { get; set; } = true;

		[Category("Layout")]
		public ImageSizeMode ImageSizeMode { get; set; }

		[Category("Layout")]
		public Rectangle Bounds
		{
			get
			{
				var loc = Dock != DockStyle.None ? bounds.Location :
					   new Point(Anchor.HasFlag(AnchorStyles.Right) ? Parent.Width - bounds.X : !Anchor.HasFlag(AnchorStyles.Left) ? (Parent.Width - bounds.Width) / 2 : bounds.X
						   , Anchor.HasFlag(AnchorStyles.Bottom) ? Parent.Height - bounds.Y : !Anchor.HasFlag(AnchorStyles.Top) ? (Parent.Height - bounds.Height) / 2 : bounds.Y);

				loc.X += Margin.Left;
				loc.Y += Margin.Top;

				return new Rectangle(loc, bounds.Size).Pad(Padding);
			}
			set => bounds = value;
		}

		public Rectangle DrawBounds => new Rectangle(Point.Empty, Bounds.Size);

		[Category("Layout")]
		public Padding Padding { get; set; }

		[Category("Layout")]
		public Padding Margin { get; set; }

		[Category("Layout")]
		public Point Location { get => bounds.Location; set => bounds = new Rectangle(value, bounds.Size); }

		[Category("Layout")]
		public Size Size { get => bounds.Size; set => bounds = new Rectangle(bounds.Location, value); }

		[Browsable(false)]
		public int Width { get => bounds.Width; set => bounds = new Rectangle(bounds.Location, new Size(value, Height)); }

		[Browsable(false)]
		public int Height { get => bounds.Height; set => bounds = new Rectangle(bounds.Location, new Size(Width, value)); }

		[Category("Appearance")]
		public string Text { get; set; }

		[Category("Appearance")]
		public Image Image { get => image; set { image = value; OnImageChanged(EventArgs.Empty); } }

		[Category("Appearance")]
		public Image DefaultImage { get; set; }

		[Category("Appearance")]
		public Font Font { get; set; }

		[Category("Appearance")]
		public Cursor Cursor { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public dynamic Data { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public double LoaderPercentage { get; private set; }

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public virtual HoverState HoverState { get => hoverState; protected set { hoverState = value; HoverStateChanged?.Invoke(this, value); } }

		[DefaultValue(true), Category("Behavior"), DisplayName("Auto Invalidate")]
		public bool AutoInvalidate { get; set; } = true;

		[DefaultValue(false), Category("Behavior")]
		public bool Loading
		{
			get => loading;
			set
			{
				loading = value;

				if (!value || Container == null)
					timer.Enabled = loading;
				else
					Container.TryInvoke(() => timer.Enabled = loading);

				Invalidate();
			}
		}

		public SlickImageBackgroundControl()
		{
			timer = new Timer { Interval = 30 };
			timer.Tick += timer_Tick;
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			LoaderPercentage += 1 + (Math.Abs(50 - LoaderPercentage) / 25);
			if (LoaderPercentage >= 200)
				LoaderPercentage = 0;

			if (!loading)
				timer.Enabled = false;

			Invalidate();
		}

		public void LoadImage(Func<Bitmap> p)
		{
			Loading = true;
			new BackgroundAction("Loading Image", () =>
			{
				try
				{
					Image = p();
					Loading = false;
					OnLoadCompleted(new AsyncCompletedEventArgs(null, false, this));
				}
				catch (Exception ex) { OnLoadCompleted(new AsyncCompletedEventArgs(ex, false, this)); }
			}).Run();
		}

		protected virtual void OnImageChanged(EventArgs e)
		{
			ImageChanged?.Invoke(this, e);
		}

		protected virtual void OnLoadCompleted(AsyncCompletedEventArgs e)
		{
		}

		public void Invalidate()
			=> Container?.TryInvoke(() => Container?.Invalidate(new Rectangle(ContainerLocation(), Size)));

		public virtual void Invalidate(Rectangle rectangle)
		{
			rectangle = Rectangle.Intersect(DrawBounds, rectangle);
			rectangle.Offset(ContainerLocation());

			Container?.TryInvoke(() => Container?.Invalidate(rectangle));
		}

		public virtual void OnPaint(PaintEventArgs e)
		{
			if (Paint != null)
				Paint(this, e);
			else
			{
				if (!string.IsNullOrWhiteSpace(Text))
					e.Graphics.DrawString(Text, UI.Font(Font?.Size ?? 8.25F, Font?.Style ?? FontStyle.Regular), new SolidBrush(FormDesign.Design.ForeColor), DrawBounds, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

				if (Image != null)
					e.Graphics.DrawImage(Image, DrawBounds, ImageSizeMode);

				if (Loading)
					e.Graphics.DrawLoader(LoaderPercentage, DrawBounds);
			}
		}

		public virtual void CalculateSize(PaintEventArgs e)
		{
			CalculateAutoSize?.Invoke(this, e);
		}

		public Point ContainerLocation()
		{
			if (Parent == null) return Bounds.Location;

			var pLoc = Parent.ContainerLocation();

			pLoc.Offset(Bounds.Location);

			return pLoc;
		}

		internal virtual SlickImageBackgroundControl HandleMouseMove(Point point)
		{
			if (!Visible)
			{ HoverState = HoverState.Normal; return null; }

			var e = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);

			if (DrawBounds.Contains(point))
			{
				OnMouseMove(e);

				if (!HoverState.HasFlag(HoverState.Hovered))
					OnMouseEnter(e);
			}
			else if (HoverState.HasFlag(HoverState.Hovered))
				OnMouseLeave(e);

			if (HoverState.HasFlag(HoverState.Pressed) && (!HoverState.HasFlag(HoverState.Hovered) || !Container.HoverState.HasFlag(HoverState.Pressed)))
				OnMouseUp(e);
			else if (!HoverState.HasFlag(HoverState.Pressed) && (HoverState.HasFlag(HoverState.Hovered) && Container.HoverState.HasFlag(HoverState.Pressed)))
				OnMouseDown(e);

			return HoverState.HasFlag(HoverState.Hovered) ? this : null;
		}

		internal void OnPrePaint(PaintEventArgs e) => PrePaint?.Invoke(this, e);

		public virtual void OnMouseClick(MouseEventArgs e)
		{
			MouseClick?.Invoke(this, e);

			if (AutoInvalidate)
				Invalidate();
		}

		public virtual bool OnMouseWheel(MouseEventArgs e)
		{
			MouseWheel?.Invoke(this, e);

			return false;
		}

		protected virtual void OnMouseMove(MouseEventArgs e)
		{
			if (AutoInvalidate)
				Invalidate();
		}

		protected virtual void OnMouseEnter(EventArgs e)
		{
			HoverState |= HoverState.Hovered;

			if (AutoInvalidate)
				Invalidate();
		}

		protected virtual void OnMouseLeave(EventArgs e)
		{
			HoverState &= ~HoverState.Hovered;

			if (AutoInvalidate)
				Invalidate();
		}

		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			HoverState |= HoverState.Pressed;

			if (AutoInvalidate)
				Invalidate();
		}

		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			HoverState &= ~HoverState.Pressed;

			if (AutoInvalidate)
				Invalidate();
		}

		protected virtual void OnLeave(EventArgs e)
		{
			HoverState &= ~HoverState.Focused;

			if (AutoInvalidate)
				Invalidate();
		}

		public Point PointToClient(Point point)
		{
			point = Container?.PointToClient(point) ?? Point.Empty;
			var pnt = ContainerLocation();
			point.Offset(-pnt.X, -pnt.Y);
			return point;
		}

		public Point PointToScreen(Point point)
		{
			var pnt = ContainerLocation();
			point.Offset(pnt.X, pnt.Y);
			return Container?.PointToScreen(point) ?? Point.Empty;
		}

		#region Dispose

		private bool disposedValue;
		private Image image;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					Image?.Dispose();
					timer?.Dispose();
					Parent?.Controls.Remove(this);
					Invalidate();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion Dispose
	}
}