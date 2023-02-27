using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("PercentageChanged")]
	public partial class SlickProgressBar : UserControl
	{
		private double perc = 0;
		private double targetPerc = 0;
		private readonly System.Timers.Timer timer = new System.Timers.Timer(35);

		public SlickProgressBar()
		{
			InitializeComponent();
			ResizeRedraw = true;
			DoubleBuffered = true;
			TabStop = false;
			timer.Elapsed += Timer_Elapsed;

			FormDesign.DesignChanged += d => Refresh();
		}

		public event EventHandler PercentageChanged;

		[Category("Behavior"), DefaultValue(0.5)]
		public double MinStep { get; set; } = 0.5;

		[Category("Behavior"), DefaultValue(ProgressBarType.Normal)]
		public ProgressBarType ProgressType { get; set; } = ProgressBarType.Normal;

		[Category("Behavior"), DefaultValue(0)]
		public double Percentage
		{
			get => targetPerc;
			set
			{
				targetPerc = value.Between(0, 100);
				timer.Start();
				PercentageChanged?.Invoke(this, new EventArgs());
			}
		}

		private int GetWidth => (int)((perc * Width / 100) - Padding.Horizontal);

		private void SlickProgressBar_Paint(object sender, PaintEventArgs e)
		{
			var barWidth = (int)((Width - Padding.Horizontal) * perc / 100).If(x => x == 0, x => 0, x => x.Between(10, Width - Padding.Horizontal));

			e.Graphics.Clear(BackColor);

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.FillRoundedRectangle(new SolidBrush(BackColor.MergeColor(FormDesign.Design.AccentColor)), new Rectangle(new Point(Padding.Left, Padding.Top), new Size(Width - Padding.Horizontal, Height - Padding.Vertical)), 5);
			if (barWidth > 0)
				e.Graphics.FillRoundedRectangle(new LinearGradientBrush(new PointF(0, 0), new PointF(Width, 0), BackColor.MergeColor(FormDesign.Design.AccentColor), FormDesign.Design.ActiveColor), new Rectangle(new Point(Padding.Left, Padding.Top), new Size(barWidth, Height - Padding.Vertical)), 5);

			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var txt = $"{Math.Floor(perc)} %";
			var bnds = e.Graphics.Measure(txt, Font);

			if (barWidth < bnds.Width *3/2)
				e.Graphics.DrawString(txt, Font, new SolidBrush(FormDesign.Design.ForeColor), new PointF(barWidth + 5, (Height - Padding.Vertical - bnds.Height) / 2));
			else
				e.Graphics.DrawString(txt, Font, new LinearGradientBrush(new PointF(0, 0), new PointF(Width / 2, 0), barWidth < Width/2?FormDesign.Design.ForeColor:FormDesign.Design.ActiveForeColor, FormDesign.Design.ActiveForeColor), new PointF(barWidth - bnds.Width - 5, (Height - Padding.Vertical - bnds.Height) / 2));
		}

		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (targetPerc != perc)
			{
				if (targetPerc - perc > 0)
					perc = Math.Min(targetPerc, perc + Math.Max(MinStep, (targetPerc - perc) / 8d / ProgressType.Switch(ProgressBarType.Fast, .7, ProgressBarType.Normal, 1, ProgressBarType.Slow, 2, 3.5)));
				else
					perc = Math.Max(targetPerc, perc - Math.Max(MinStep, (perc - targetPerc) / 8d / ProgressType.Switch(ProgressBarType.Fast, .7, ProgressBarType.Normal, 1, ProgressBarType.Slow, 2, 3.5)));

				if ((perc == 100 && targetPerc == 100) || (perc == 0 && targetPerc == 0))
					timer.Stop();

				this.TryInvoke(Refresh);
			}
			else
			{
				timer.Stop();
			}
		}
	}
}