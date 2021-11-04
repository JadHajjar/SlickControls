using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MechanikaDesign.WinForms.UI.ColorPicker
{
	[DefaultEvent("ColorChanged")]
	internal partial class ColorHexagon : UserControl
	{
		#region Fields

		private const float COEFFCIENT = 0.824f;
		private readonly ColorHexagonElement[] hexagonElements = new ColorHexagonElement[0x93];
		private readonly float[] matrix1 = new float[] { -0.5f, -1f, -0.5f, 0.5f, 1f, 0.5f };
		private readonly float[] matrix2 = new float[] { 0.824f, 0f, -0.824f, -0.824f, 0f, 0.824f };
		private int oldSelectedHexagonIndex = -1;
		private readonly int sectorMaximum = 7;
		private int selectedHexagonIndex = -1;

		#endregion Fields

		#region Events

		public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs args);

		public event ColorChangedEventHandler ColorChanged;

		#endregion Events

		#region Properties

		public Color SelectedColor
		{
			get
			{
				if (selectedHexagonIndex < 0)
				{
					return Color.Empty;
				}
				return hexagonElements[selectedHexagonIndex].CurrentColor;
			}
		}

		#endregion Properties

		#region Constructors

		public ColorHexagon()
		{
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.Opaque, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			for (var i = 0; i < hexagonElements.Length; i++)
			{
				hexagonElements[i] = new ColorHexagonElement();
			}

			InitializeComponent();
		}

		#endregion Constructors

		#region Methods/Functions

		private void DrawHexagonHighlighter(int selectedHexagonIndex)
		{
			if (selectedHexagonIndex != oldSelectedHexagonIndex)
			{
				if (oldSelectedHexagonIndex >= 0)
				{
					hexagonElements[oldSelectedHexagonIndex].IsHovered = false;
					base.Invalidate(hexagonElements[oldSelectedHexagonIndex].BoundingRectangle);
				}
				oldSelectedHexagonIndex = selectedHexagonIndex;
				if (oldSelectedHexagonIndex >= 0)
				{
					hexagonElements[oldSelectedHexagonIndex].IsHovered = true;
					base.Invalidate(hexagonElements[oldSelectedHexagonIndex].BoundingRectangle);
				}
			}
		}

		private int GetHexagonIndexFromCoordinates(int xCoordinate, int yCoordinate)
		{
			for (var i = 0; i < hexagonElements.Length; i++)
			{
				if (hexagonElements[i].BoundingRectangle.Contains(xCoordinate, yCoordinate))
				{
					return i;
				}
			}
			return -1;
		}

		private int GetHexgaonWidth(int availableHeight)
		{
			var num = availableHeight / (2 * sectorMaximum);
			if ((((int)Math.Floor(num / 2.0)) * 2) < num)
			{
				num--;
			}
			return num;
		}

		private void InitializeGrayscaleHexagons(ref Rectangle clientRectangle, int hexagonWidth,
												 ref int centerOfMiddleHexagonX, ref int centerOfMiddleHexagonY,
												 ref int index)
		{
			var red = 0xff;
			var num4 = 0x11;
			var num3 = 0x10;
			var num5 = (((clientRectangle.Width - (7 * hexagonWidth)) / 2) + clientRectangle.X) - (hexagonWidth / 3);

			centerOfMiddleHexagonX = num5;
			centerOfMiddleHexagonY = clientRectangle.Bottom;
			for (var i = 0; i < num3; i++)
			{
				hexagonElements[index].CurrentColor = Color.FromArgb(red, red, red);
				hexagonElements[index].SetHexagonPoints(centerOfMiddleHexagonX, centerOfMiddleHexagonY, hexagonWidth);
				centerOfMiddleHexagonX += hexagonWidth;
				index++;
				if (i == 7)
				{
					centerOfMiddleHexagonX = num5 + (hexagonWidth / 2);
					centerOfMiddleHexagonY += (int)(hexagonWidth * 0.824f);
				}
				red -= num4;
			}
		}

		private void InitializeHexagons()
		{
			var clientRectangle = base.ClientRectangle;
			clientRectangle.Offset(0, -8);
			if (clientRectangle.Height < clientRectangle.Width)
			{
				clientRectangle.Inflate(-(clientRectangle.Width - clientRectangle.Height) / 2, 0);
			}
			else
			{
				clientRectangle.Inflate(0, -(clientRectangle.Height - clientRectangle.Width) / 2);
			}

			var hexagonWidth = GetHexgaonWidth(Math.Min(clientRectangle.Height, clientRectangle.Width));
			var centerOfMiddleHexagonX = (clientRectangle.Left + clientRectangle.Right) / 2;
			var centerOfMiddleHexagonY = (clientRectangle.Top + clientRectangle.Bottom) / 2;

			centerOfMiddleHexagonY -= hexagonWidth;
			hexagonElements[0].CurrentColor = Color.White;
			hexagonElements[0].SetHexagonPoints(centerOfMiddleHexagonX, centerOfMiddleHexagonY, hexagonWidth);
			var index = 1;
			for (var i = 1; i < sectorMaximum; i++)
			{
				float yCoordinate = centerOfMiddleHexagonY;
				float xCoordinate = centerOfMiddleHexagonX + (hexagonWidth * i);
				for (var j = 0; j < (sectorMaximum - 1); j++)
				{
					var num9 = (int)(hexagonWidth * matrix2[j]);
					var num10 = (int)(hexagonWidth * matrix1[j]);
					for (var k = 0; k < i; k++)
					{
						var num12 = ((0.936 * (sectorMaximum - i)) / sectorMaximum) + 0.12;
						var colorQuotient = GetColorQuotient(xCoordinate - centerOfMiddleHexagonX, yCoordinate - centerOfMiddleHexagonY);
						hexagonElements[index].SetHexagonPoints(xCoordinate, yCoordinate, hexagonWidth);
						hexagonElements[index].CurrentColor = ColorFromRGBRatios(colorQuotient, num12, 1.0);
						yCoordinate += num9;
						xCoordinate += num10;
						index++;
					}
				}
			}
			clientRectangle.Y -= hexagonWidth + (hexagonWidth / 2);
			InitializeGrayscaleHexagons(ref clientRectangle, hexagonWidth, ref centerOfMiddleHexagonX, ref centerOfMiddleHexagonY, ref index);
		}

		#endregion Methods/Functions

		#region Overridden Methods

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (selectedHexagonIndex >= 0)
				{
					hexagonElements[selectedHexagonIndex].IsSelected = false;
					base.Invalidate(hexagonElements[selectedHexagonIndex].BoundingRectangle);
				}
				selectedHexagonIndex = -1;
				if (oldSelectedHexagonIndex >= 0)
				{
					selectedHexagonIndex = oldSelectedHexagonIndex;
					hexagonElements[selectedHexagonIndex].IsSelected = true;
					if (ColorChanged != null)
					{
						ColorChanged(this, new ColorChangedEventArgs(SelectedColor));
					}
					base.Invalidate(hexagonElements[selectedHexagonIndex].BoundingRectangle);
				}
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			DrawHexagonHighlighter(-1);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			DrawHexagonHighlighter(GetHexagonIndexFromCoordinates(e.X, e.Y));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (BackColor == Color.Transparent)
			{
				base.OnPaintBackground(e);
			}

			var g = e.Graphics;
			using (var brush = new SolidBrush(BackColor))
			{
				g.FillRectangle(brush, base.ClientRectangle);
			}

			g.SmoothingMode = SmoothingMode.AntiAlias;
			foreach (var element in hexagonElements)
			{
				element.Paint(g);
			}

			if (oldSelectedHexagonIndex >= 0)
			{
				hexagonElements[oldSelectedHexagonIndex].Paint(g);
			}
			if (selectedHexagonIndex >= 0)
			{
				hexagonElements[selectedHexagonIndex].Paint(g);
			}
			base.OnPaint(e);
		}

		protected override void OnResize(EventArgs e)
		{
			InitializeHexagons();
			base.OnResize(e);
		}

		#endregion Overridden Methods

		#region Color Helper Functions

		private static float GetColorQuotient(float value1, float value2) => (float)((Math.Atan2(value2, value1) * 180.0) / 3.1415926535897931);

		private static int GetColorChannelValue(float value1, float value2, float value3)
		{
			if (value3 > 360f)
			{
				value3 -= 360f;
			}
			else if (value3 < 0f)
			{
				value3 += 360f;
			}
			if (value3 < 60f)
			{
				value1 += ((value2 - value1) * value3) / 60f;
			}
			else if (value3 < 180f)
			{
				value1 = value2;
			}
			else if (value3 < 240f)
			{
				value1 += ((value2 - value1) * (240f - value3)) / 60f;
			}
			return (int)(value1 * 255f);
		}

		private static Color ColorFromRGBRatios(double value1, double value2, double value3)
		{
			int num;
			int num2;
			int num3;
			if (value3 == 0.0)
			{
				num = num2 = num3 = (int)(value2 * 255.0);
			}
			else
			{
				float num4;
				if (value2 <= 0.5)
				{
					num4 = (float)(value2 + (value2 * value3));
				}
				else
				{
					num4 = (float)((value2 + value3) - (value2 * value3));
				}
				var num5 = ((float)(2.0 * value2)) - num4;
				num = GetColorChannelValue(num5, num4, (float)(value1 + 120.0));
				num2 = GetColorChannelValue(num5, num4, (float)value1);
				num3 = GetColorChannelValue(num5, num4, (float)(value1 - 120.0));
			}
			return Color.FromArgb(num, num2, num3);
		}

		#endregion Color Helper Functions
	}

	#region HexagaonElement Class

	internal class ColorHexagonElement
	{
		#region Fields

		private Rectangle boundingRect = Rectangle.Empty;
		private Color hexagonColor = Color.Empty;
		private readonly Point[] hexagonPoints = new Point[6];
		private bool isHovered;
		private bool isSelected;

		#endregion Fields

		#region Methods

		public void Paint(Graphics g)
		{
			var path = new GraphicsPath();
			path.AddPolygon(hexagonPoints);
			path.CloseAllFigures();
			using (var brush = new SolidBrush(hexagonColor))
			{
				g.FillPath(brush, path);
			}
			if (isHovered || isSelected)
			{
				var smoothingMode = g.SmoothingMode;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				using (var pen = new Pen(Color.FromArgb(0x2a, 0x5b, 150), 2f))
				{
					g.DrawPath(pen, path);
				}
				using (var pen2 = new Pen(Color.FromArgb(150, 0xb1, 0xef), 1f))
				{
					g.DrawPath(pen2, path);
				}
				g.SmoothingMode = smoothingMode;
			}
			path.Dispose();
		}

		public void SetHexagonPoints(float xCoordinate, float yCoordinate, int hexagonWidth)
		{
			var num = hexagonWidth * 0.5773503f;
			hexagonPoints[0] = new Point((int)Math.Floor(xCoordinate - (hexagonWidth / 2)), ((int)Math.Floor(yCoordinate - (num / 2f))) - 1);
			hexagonPoints[1] = new Point((int)Math.Floor(xCoordinate), ((int)Math.Floor(yCoordinate - (hexagonWidth / 2))) - 1);
			hexagonPoints[2] = new Point((int)Math.Floor(xCoordinate + (hexagonWidth / 2)), ((int)Math.Floor(yCoordinate - (num / 2f))) - 1);
			hexagonPoints[3] = new Point((int)Math.Floor(xCoordinate + (hexagonWidth / 2)), ((int)Math.Floor(yCoordinate + (num / 2f))) + 1);
			hexagonPoints[4] = new Point((int)Math.Floor(xCoordinate), ((int)Math.Floor(yCoordinate + (hexagonWidth / 2))) + 1);
			hexagonPoints[5] = new Point((int)Math.Floor(xCoordinate - (hexagonWidth / 2)), ((int)Math.Floor(yCoordinate + (num / 2f))) + 1);
			using (var path = new GraphicsPath())
			{
				path.AddPolygon(hexagonPoints);
				boundingRect = Rectangle.Round(path.GetBounds());
				boundingRect.Inflate(2, 2);
			}
		}

		#endregion Methods

		#region Properties

		public Rectangle BoundingRectangle => boundingRect;

		public Color CurrentColor
		{
			get => hexagonColor;
			set => hexagonColor = value;
		}

		public bool IsHovered
		{
			get => isHovered;
			set => isHovered = value;
		}

		public bool IsSelected
		{
			get => isSelected;
			set => isSelected = value;
		}

		#endregion Properties
	}

	#endregion HexagaonElement Class
}