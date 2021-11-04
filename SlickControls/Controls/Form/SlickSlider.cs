using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public enum SliderStyle { SingleVertical = 11, SingleHorizontal = 12, DoubleVertical = 21, DoubleHorizontal = 22 }

	[DefaultEvent("ValuesChanged")]
	public partial class SlickSlider : SlickControl, IAnimatable
	{
		[Category("Property Changed")]
		public event EventHandler ValuesChanged;

		private double maxValue = 100;
		private double minValue = 0;
		private int mouseDownCode = 0;
		private int mouseHoverCode = -1;
		private SliderStyle sliderStyle = SliderStyle.SingleHorizontal;

		public SlickSlider()
		{
			InitializeComponent();
			ResizeRedraw = true;
			DoubleBuffered = true;
			Paint += ActiveSlider_Paint;
			MouseLeave += SlickSlider_MouseLeave;
			MouseDown += ActiveSlider_MouseDown;
			MouseUp += ActiveSlider_MouseUp;
			MouseMove += ActiveSlider_MouseMove;
		}

		protected override void DesignChanged(FormDesign design) => Invalidate();

		protected override void UIChanged()
		{
			Font = UI.Font(8F);
			Height = (int)Math.Round(14 * UI.UIScale) + Padding.Vertical + (ShowValues ? Font.Height + 6 : 0);

			Padding = new Padding((int)Math.Max(Padding.Left, Math.Round(14 * UI.UIScale) / 2 + penSize), ShowValues ? Height / 4 : Padding.Top, (int)Math.Max(Padding.Right, Math.Round(14 * UI.UIScale) / 2 + penSize), ShowValues ? Height / 4 : Padding.Bottom);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl(); Cursor = Cursors.Hand;
		}

		[Category("Design"), DisplayName("Value From")]
		public double FromValue { get => (Values[0] * (MaxValue - MinValue)) + MinValue; set => PercFrom = (value - MinValue) / (MaxValue - MinValue); }

		[Browsable(false)]
		public bool Horizontal => (int)SliderStyle % 2 == 0;

		[Browsable(false)]
		public Func<double, string> ValueOutput { get; set; }

		[Category("Behavior"), DefaultValue(true), DisplayName("Show Values")]
		public bool ShowValues { get; set; } = true;

		[Category("Design"), DisplayName("Max Value")]
		public double MaxValue { get => maxValue; set { maxValue = value; Invalidate(); } }

		[Category("Design"), DisplayName("Min Value")]
		public double MinValue { get => minValue; set { minValue = value; Invalidate(); } }

		[Category("Design"), DisplayName("Percentage")]
		public double Percentage { get => Values[1]; set { var c = Values[1] != value.Between(0, 1); Values[1] = value.Between(0, 1); Invalidate(); if (c) ValuesChanged?.Invoke(this, new EventArgs()); } }

		[Category("Design"), DisplayName("Percentage From")]
		public double PercFrom { get => Values[0]; set { var c = Values[0] != value.Between(0, 1); Values[0] = value.Between(0, 1); Invalidate(); if (c) ValuesChanged?.Invoke(this, new EventArgs()); } }

		[Category("Design"), DisplayName("Percentage To")]
		public double PercTo { get => Values[1]; set { var c = Values[1] != value.Between(0, 1); Values[1] = value.Between(0, 1); Invalidate(); if (c) ValuesChanged?.Invoke(this, new EventArgs()); } }

		[Browsable(false)]
		public bool Single => (int)SliderStyle < 20;

		[Category("Appearance"), DisplayName("Slider Style")]
		public SliderStyle SliderStyle { get => sliderStyle; set { sliderStyle = value; Invalidate(); } }

		[Category("Design"), DisplayName("Value To")]
		public double ToValue { get => (Values[1] * (MaxValue - MinValue)) + MinValue; set => PercTo = (value - MinValue) / (MaxValue - MinValue); }

		[Category("Design"), DisplayName("Value")]
		public double Value { get => (Values[1] * (MaxValue - MinValue)) + MinValue; set => PercTo = (value - MinValue) / (MaxValue - MinValue); }

		[Browsable(false)]
		public double[] Values { get; private set; } = { 0, 0 };

		private void ActiveSlider_MouseDown(object sender, MouseEventArgs e)
		{
			double? p = null;

			if (Horizontal)
			{
				if (new RectangleF(0, Height - Padding.Vertical - 14, Width, Padding.Vertical + 14).Contains(e.Location))
					p = (e.X - Padding.Left) / (double)(Width - Padding.Horizontal);
			}
			else
			{
				if (new RectangleF(0, 0, Padding.Horizontal - 14, Height).Contains(e.Location))
					p = (e.Y - Padding.Top) / (double)(Height - Padding.Vertical);
			}

			if (p != null)
			{
				if (!Single && Math.Abs(PercFrom - (double)p) < Math.Abs(PercTo - (double)p))
				{
					PercFrom = (double)p;
					mouseDownCode = mouseHoverCode = 1;
					Invalidate();
				}
				else
				{
					PercTo = (double)p;
					mouseDownCode = mouseHoverCode = 2;
					Invalidate();
				}
			}
		}

		private void ActiveSlider_MouseMove(object sender, MouseEventArgs e)
		{
			switch (mouseDownCode)
			{
				case 0:
					if (GetRec(1)?.Contains(e.Location) ?? false)
					{
						mouseHoverCode = 1;
					}
					else if (GetRec(2)?.Contains(e.Location) ?? false)
					{
						mouseHoverCode = 2;
					}
					else
					{
						mouseHoverCode = 0;
					}
					Invalidate();
					break;

				case 1:
					if (Horizontal)
						PercFrom = (e.X - Padding.Left) / (double)(Width - Padding.Horizontal);
					else
						PercFrom = (e.Y - Padding.Top) / (double)(Height - Padding.Vertical);

					if (PercFrom > PercTo)
					{
						mouseDownCode = mouseHoverCode = 2;
						ExtensionClass.Swap(ref Values[0], ref Values[1]);
					}
					break;

				case 2:
					if (Horizontal)
						PercTo = (e.X - Padding.Left) / (double)(Width - Padding.Horizontal);
					else
						PercTo = (e.Y - Padding.Top) / (double)(Height - Padding.Vertical);

					if (PercFrom > PercTo)
					{
						mouseDownCode = mouseHoverCode = 1;
						ExtensionClass.Swap(ref Values[0], ref Values[1]);
					}
					break;

				default:
					break;
			}

			if (TargetAnimationValue == 0)
			{
				TargetAnimationValue = 100;
				AnimationHandler.Animate(this, 1.5);
			}
		}

		private void SlickSlider_MouseLeave(object sender, EventArgs e)
		{
			mouseHoverCode = -1;
			Invalidate();
			TargetAnimationValue = 0;
			AnimationHandler.Animate(this, 1.5);
		}

		private void ActiveSlider_MouseUp(object sender, MouseEventArgs e)
		{
			mouseDownCode = 0;
			Invalidate();
			Values = Values.OrderBy(x => x).ToArray();
			ActiveSlider_MouseMove(sender, e);
		}

		private float penSize => (float)(UI.UIScale * 1.25);

		private int LinePoint => Horizontal
			? ShowValues ? Font.Height + 6 + (Height - Font.Height - 6) / 2 : Height / 2
			: ShowValues ? Width / 2 : Width / 2;

		public int AnimatedValue { get; set; }
		public int TargetAnimationValue { get; set; }

		private void ActiveSlider_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			if (Horizontal)
			{
				e.Graphics.DrawLine(new Pen(Color.FromArgb(175, FormDesign.Design.ForeColor), penSize), Padding.Left + (float)((Width - Padding.Horizontal) * PercTo), LinePoint, Width - Padding.Left, LinePoint);

				if (Single)
					e.Graphics.DrawLine(new Pen(FormDesign.Design.ActiveColor, penSize), Padding.Left, LinePoint - .25F, Padding.Left + (float)((Width - Padding.Horizontal) * PercTo), LinePoint - .25F);
				else
					e.Graphics.DrawLine(new Pen(FormDesign.Design.ActiveColor, penSize), Padding.Left + (float)((Width - Padding.Horizontal) * PercFrom), LinePoint - .25F, Padding.Left + (float)((Width - Padding.Horizontal) * PercTo), LinePoint - .25F);
			}
			else
			{
				e.Graphics.DrawLine(new Pen(Color.FromArgb(175, FormDesign.Design.ForeColor), penSize), Padding.Left, Padding.Top, Padding.Left, Height - Padding.Top);

				if (Single)
					e.Graphics.DrawLine(new Pen(FormDesign.Design.ActiveColor, penSize), Padding.Left - .25F, Padding.Top, Padding.Left - .25F, Padding.Top + (float)((Height - Padding.Vertical) * PercTo));
				else
					e.Graphics.DrawLine(new Pen(FormDesign.Design.ActiveColor, penSize), Padding.Left - .25F, Padding.Top + (float)((Height - Padding.Vertical) * PercFrom), Padding.Left - .25F, Padding.Top + (float)((Height - Padding.Vertical) * PercTo));
			}

			if (!Single)
			{
				DrawDot(e.Graphics, PercFrom, 1);
				DrawValue(e.Graphics, PercFrom, ValueOutput?.Invoke(FromValue) ?? Math.Floor(FromValue).ToString());
			}

			DrawDot(e.Graphics, PercTo, 2);

			if (mouseHoverCode == 0)
				DrawDot(e.Graphics, ((PointToClient(Cursor.Position).X - Padding.Left) / (double)(Width - Padding.Horizontal)).Between(0, 1), 0);

			if (ShowValues)
				DrawValue(e.Graphics, PercTo, ValueOutput?.Invoke(ToValue) ?? Math.Ceiling(ToValue).ToString());

			if (!Enabled)
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, BackColor)), 0, 0, Width, Height);
		}

		private void DrawDot(Graphics graphics, double val, int dot)
		{
			var size = Math.Min((int)Math.Round((dot == 0 ? 8 : 12) * UI.UIScale), Height - 4);
			size -= (int)(size * 4D / 10 * (100 - AnimatedValue) / 100);
			var half = size / 2;

			var pt = Horizontal
				? new PointF((float)((Width - Padding.Horizontal) * val) + Padding.Left, LinePoint)
				: new PointF(Padding.Left, (float)((Height - Padding.Vertical) * val) + Padding.Top);

			var rect = new RectangleF(pt.X - half, pt.Y - half, size, size);

			if (dot == 0)
			{
				graphics.FillEllipse(new SolidBrush(FormDesign.Design.ActiveColor.MergeColor(BackColor)), rect);
			}
			else if (mouseDownCode == dot)
			{
				graphics.FillEllipse(new SolidBrush(FormDesign.Design.ActiveColor), rect);
			}
			else
			{
				graphics.FillEllipse(new SolidBrush(mouseHoverCode != dot ? BackColor : BackColor.MergeColor(FormDesign.Design.ActiveColor, 85)), rect);

				graphics.DrawEllipse(new Pen(mouseDownCode == dot ? ForeColor : FormDesign.Design.ActiveColor, 1F * penSize), rect);
			}
		}

		private void DrawValue(Graphics graphics, double val, string txt)
		{
			if (!ShowValues)
				return;

			var format = new StringFormat()
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (Horizontal)
			{
				var pt = new PointF(
					(float)((Width - Padding.Horizontal) * val) + Padding.Left - 7,
					0);

				var bnds = graphics.MeasureString(txt, Font);

				var rect = new Rectangle(
					(int)(pt.X - (Math.Ceiling(bnds.Width) / 2) + 6),
					(int)Math.Max(0, (Height / 2) - bnds.Height - 3),
					(int)Math.Ceiling(bnds.Width) + 6,
					(int)Math.Ceiling(bnds.Height) +3);

				rect.X -= Math.Max(0, rect.Height - rect.Width);
				rect.Width = Math.Max(rect.Width, rect.Height);
				rect.X += Math.Min(0, Width - rect.Width - rect.X - 1).If(0, -Math.Min(0, rect.X));

				graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect, 5);
				graphics.DrawString(txt, Font, Brushes.White, rect, format);
			}
			else
			{
				var pt = new PointF(15 + Padding.Left - 7, (float)((Height - Padding.Vertical) * val) + Padding.Top);
				var bnds = graphics.MeasureString(txt, Font);
				var rect = new Rectangle((int)(pt.X), (int)(pt.Y - bnds.Height / 2), (int)Math.Max(16, bnds.Width) + 3, (int)bnds.Height);

				if (txt.Length < 2)
				{
					bnds.Width += 6.5F;
					pt.X += 4F;
					rect.X += 1;
				}
				else if (txt.Length < 3)
				{
					bnds.Width += 4F;
					pt.X += 1;
					rect.X += 1;
				}
				else if (txt.Length > 6)
				{
					bnds.Width -= 3.25F * (txt.Length - 3);
				}
				else if (txt.Length > 3)
				{
					bnds.Width -= 2.5F * (txt.Length - 3);
				}

				if (ShowValues)
				{
					graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect, 7);
					graphics.DrawString(txt, Font, Brushes.White, new PointF(pt.X + 2, (pt.Y - bnds.Height / 2) + 1), format);
				}
			}
		}

		private RectangleF? GetRec(int v)
		{
			if (v == 1 && (int)SliderStyle < 20)
				return null;

			var val = v == 1 ? PercFrom : PercTo;
			var size = Math.Min((int)Math.Round(12 * UI.UIScale), Height - 4);
			var half = size / 2;

			if (Horizontal)
			{
				return new RectangleF(
						new PointF((float)((Width - Padding.Horizontal) * val) + Padding.Left - half, LinePoint - half),
						new SizeF(size, size));
			}
			else
			{
				return new RectangleF(
						new PointF(Padding.Left - half, (float)((Height - Padding.Vertical) * val) + Padding.Top - half),
						new SizeF(size, size));
			}
		}
	}
}