using Extensions;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MechanikaDesign.WinForms.UI.ColorPicker
{
	[DefaultEvent("ColorChanged")]
	internal partial class ColorSliderVertical : UserControl
	{
		#region Events

		public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs args);

		public event ColorChangedEventHandler ColorChanged;

		#endregion Events

		#region Fields

		private HslColor colorHSL = HslColor.FromAhsl(0xff);
		private ColorModes colorMode;
		private Color colorRGB = Color.Empty;
		private bool mouseMoving;
		private int position;
		private bool setHueSilently;
		private Color nubColor;

		#endregion Fields

		#region Properties

		public Color ColorRGB
		{
			get => colorRGB;
			set
			{
				colorRGB = value;
				if (!setHueSilently)
				{
					colorHSL = HslColor.FromColor(ColorRGB);
				}
				ResetSlider();
				Refresh();
			}
		}

		public HslColor ColorHSL
		{
			get => colorHSL;
			set
			{
				colorHSL = value;
				colorRGB = colorHSL.RgbValue;
				ResetSlider();
				Refresh();
			}
		}

		public ColorModes ColorMode
		{
			get => colorMode;
			set
			{
				colorMode = value;
				ResetSlider();
				Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the color of the selection nub.
		/// </summary>
		/// <value>
		/// The color of the nub.
		/// </value>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "Black")]
		public Color NubColor
		{
			get => nubColor;
			set => nubColor = value;
		}

		/// <summary>
		/// Gets or sets the position of the selection nub.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public int Position
		{
			get => position;
			set
			{
				var num = value;
				num = MathExtensions.LimitToRange(num, 0, base.Height - 9);
				if (num != position)
				{
					position = num;
					ResetHSLRGB();
					Refresh();
					if (ColorChanged != null)
					{
						ColorChanged(this, new ColorChangedEventArgs(colorRGB));
					}
				}
			}
		}

		#endregion Properties

		#region Constructors

		public ColorSliderVertical()
		{
			InitializeComponent();
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			ResizeRedraw = true;
			colorHSL = HslColor.FromAhsl(1.0, 1.0, 1.0);
			colorRGB = colorHSL.RgbValue;
			colorMode = ColorModes.Hue;
		}

		#endregion Constructors

		#region Overridden Methods

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			mouseMoving = true;
			Position = e.Y - 4;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			mouseMoving = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (mouseMoving)
			{
				Position = e.Y - 4;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var color = HslColor.FromAhsl(0xff);
			switch (ColorMode)
			{
				case ColorModes.Hue:
					color.L = color.S = 1.0;
					break;

				case ColorModes.Saturation:
					color.H = ColorHSL.H;
					color.L = ColorHSL.L;
					break;

				case ColorModes.Luminance:
					color.H = ColorHSL.H;
					color.S = ColorHSL.S;
					break;
			}
			for (var i = 0; i < (base.Height - 8); i++)
			{
				var num2 = 0.0;
				if (ColorMode < ColorModes.Hue)
				{
					num2 = 255.0 - MathExtensions.Round((255.0 * i) / (base.Height - 8.0));
				}
				else
				{
					num2 = 1.0 - (i / ((double)(base.Height - 8)));
				}
				var empty = Color.Empty;
				switch (ColorMode)
				{
					case ColorModes.Red:
						empty = Color.FromArgb((int)num2, ColorRGB.G, ColorRGB.B);
						break;

					case ColorModes.Green:
						empty = Color.FromArgb(ColorRGB.R, (int)num2, ColorRGB.B);
						break;

					case ColorModes.Blue:
						empty = Color.FromArgb(ColorRGB.R, ColorRGB.G, (int)num2);
						break;

					case ColorModes.Hue:
						color.H = num2;
						empty = color.RgbValue;
						break;

					case ColorModes.Saturation:
						color.S = num2;
						empty = color.RgbValue;
						break;

					case ColorModes.Luminance:
						color.L = num2;
						empty = color.RgbValue;
						break;
				}

				using (var pen = new Pen(empty))
				{
					e.Graphics.DrawLine(pen, 11, i + 4, base.Width - 11, i + 4);
				}
			}
			DrawSlider(e.Graphics);
		}

		#endregion Overridden Methods

		#region Private Methods

		private void DrawSlider(Graphics g)
		{
			using (var pen = new Pen(Extensions.FormDesign.Design.ForeColor))
			{
				var fill = new SolidBrush(Extensions.FormDesign.Design.ForeColor);
				var points = new Point[] { new Point(1, position), new Point(3, position), new Point(7, position + 4), new Point(3, position + 8), new Point(1, position + 8), new Point(0, position + 7), new Point(0, position + 1) };
				g.FillPolygon(fill, points);
				g.DrawPolygon(pen, points);

				points[0] = new Point(base.Width - 2, position);
				points[1] = new Point(base.Width - 4, position);
				points[2] = new Point(base.Width - 8, position + 4);
				points[3] = new Point(base.Width - 4, position + 8);
				points[4] = new Point(base.Width - 2, position + 8);
				points[5] = new Point(base.Width - 1, position + 7);
				points[6] = new Point(base.Width - 1, position + 1);

				g.FillPolygon(fill, points);
				g.DrawPolygon(pen, points);
			}
		}

		private void ResetSlider()
		{
			var h = 0.0;
			switch (ColorMode)
			{
				case ColorModes.Red:
					h = colorRGB.R / 255.0;
					break;

				case ColorModes.Green:
					h = colorRGB.G / 255.0;
					break;

				case ColorModes.Blue:
					h = colorRGB.B / 255.0;
					break;

				case ColorModes.Hue:
					h = colorHSL.H;
					break;

				case ColorModes.Saturation:
					h = colorHSL.S;
					break;

				case ColorModes.Luminance:
					h = colorHSL.L;
					break;
			}
			position = (base.Height - 8) - MathExtensions.Round((base.Height - 8) * h);
		}

		private void ResetHSLRGB()
		{
			setHueSilently = true;
			switch (ColorMode)
			{
				case ColorModes.Red:
					ColorRGB = Color.FromArgb(0xff - MathExtensions.Round((255.0 * position) / (base.Height - 9)), ColorRGB.G, ColorRGB.B);
					ColorHSL = HslColor.FromColor(ColorRGB);
					break;

				case ColorModes.Green:
					ColorRGB = Color.FromArgb(ColorRGB.R, 0xff - MathExtensions.Round((255.0 * position) / (base.Height - 9)), ColorRGB.B);
					ColorHSL = HslColor.FromColor(ColorRGB);
					break;

				case ColorModes.Blue:
					ColorRGB = Color.FromArgb(ColorRGB.R, ColorRGB.G, 0xff - MathExtensions.Round((255.0 * position) / (base.Height - 9)));
					ColorHSL = HslColor.FromColor(ColorRGB);
					break;

				case ColorModes.Hue:
					colorHSL.H = 1.0 - (position / ((double)(base.Height - 9)));
					ColorRGB = ColorHSL.RgbValue;
					break;

				case ColorModes.Saturation:
					colorHSL.S = 1.0 - (position / ((double)(base.Height - 9)));
					ColorRGB = ColorHSL.RgbValue;
					break;

				case ColorModes.Luminance:
					colorHSL.L = 1.0 - (position / ((double)(base.Height - 9)));
					ColorRGB = ColorHSL.RgbValue;
					break;
			}
			setHueSilently = false;
		}

		#endregion Private Methods
	}
}