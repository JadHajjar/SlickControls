using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickPictureBox : PictureBox, IHoverControl
	{
		private long epoch;
		private long lastTick;
		private bool loading;

		[DefaultValue(false), Category("Behavior")]
		public bool Loading
		{
			get => loading;
			set
			{
				loading = value;

				if (value)
				{
					epoch = DateTime.Now.Ticks;
					lastTick = 0;
				}

				if (Live)
				{
					InvalidateForLoading();
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Live { get; private set; }

		[DefaultValue(false), Category("Behavior"), DisplayName("User Draw")]
		public bool UserDraw { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public double LoaderPercentage { get; private set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual HoverState HoverState { get; internal set; } = HoverState.Normal;

		[DefaultValue(true), Category("Behavior"), DisplayName("Auto Invalidate")]
		public bool AutoInvalidate { get; set; } = true;

		[DefaultValue(null)]
		public new Image Image
		{
			get => base.Image; 
			set => this.TryInvoke(() =>
			{
				base.Image = value;
				Loading = false;
			});
		}

		[DefaultValue(1), Category("Behavior"), DisplayName("Loader Speed")]
		public double LoaderSpeed { get; set; } = 1;

		public SlickPictureBox()
		{
			DoubleBuffered = ResizeRedraw = true;

			LoadCompleted += (s, e) => Loading = false;
		}

		public void OnImageLoaded(AsyncCompletedEventArgs e = null)
		{
			this.TryInvoke(() => OnLoadCompleted(e ?? new AsyncCompletedEventArgs(null, false, null)));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (loading && !UserDraw)
			{
				e.Graphics.Clear(BackColor);

				DrawLoader(e.Graphics, new Rectangle(Point.Empty, Size));
			}
			else
			{
				base.OnPaint(e);
			}
		}

		protected override async void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			if (loading && Live)
			{
				var oldTick = lastTick;
				lastTick = (DateTime.Now.Ticks - epoch) / TimeSpan.TicksPerMillisecond;

				var val = lastTick / 600D % Math.PI;
				LoaderPercentage = 100 - (100 * Math.Cos(val));

				await Task.Delay(Math.Max(2, 25 - (int)(lastTick - oldTick)));

				InvalidateForLoading();
			}
		}

		protected virtual void InvalidateForLoading()
		{
			Invalidate(ClientRectangle);
		}

		public void DrawLoader(Graphics g, Rectangle rectangle, Color? color = null)
		{
			g.DrawLoader(LoaderPercentage, rectangle, color);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			HoverState |= HoverState.Hovered;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			HoverState &= ~HoverState.Hovered;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			HoverState |= HoverState.Pressed;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			HoverState &= ~HoverState.Pressed;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseUp(e);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			Live = !DesignMode;
		}
	}
}