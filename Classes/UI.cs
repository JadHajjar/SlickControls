using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls
{
	public class UI : ISave
	{
		public static event UIEventHandler UIChanged;

		public delegate void UIEventHandler();

		public static string FontFamily => _instance.fontFamily ?? (CrossIO.CurrentPlatform == Platform.MacOSX ? "Helvetica" : "Segoe UI");
		public static double FontScale => _instance.fontScale * WindowsScale;
		public static double UIScale => Math.Round(FontScale.If(x => x > 1, x => (x * .9) + 0.1, x => (x * 1.1) - 0.1), 2);
		public static double WindowsScale { get; }

		public string fontFamily { get; set; }
		public double fontScale { get; set; } = 1;
		public double uiScale { get; set; } = 1;
		public bool noAnimations { get => _noAnimations; set => AnimationHandler.NoAnimations = _noAnimations = value; }

		internal static readonly UI _instance = Load<UI>("UI.tf", "SlickUI");

		private bool _noAnimations;

		#region Factoring

		public static Font Font(float size, FontStyle style = FontStyle.Regular)
		{
			return new Font(
			(style.HasFlag(FontStyle.Italic) && FontFamily == "Nirmala UI") ? "Century Gothic" : FontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);
		}

		public static Font Font(float size, Graphics g, FontStyle style = FontStyle.Regular)
		{
			return new Font(
			(style.HasFlag(FontStyle.Italic) && FontFamily == "Nirmala UI") ? "Century Gothic" : FontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);
		}

		public static Font Font(string fontFamily, float size, FontStyle style = FontStyle.Regular)
		{
			return new Font(
			fontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);
		}

		public static Font Font(string fontFamily, float size, Graphics g, FontStyle style = FontStyle.Regular)
		{
			return new Font(
			fontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);
		}

		public static Size Scale(Size size, double scale)
		{
			return new Size(
			(int)(size.Width * scale),
			(int)(size.Height * scale));
		}

		public static Rectangle Scale(Rectangle rect, double scale)
		{
			return new Rectangle(
			rect.X - (int)(((rect.Width * scale) - rect.Width) * 0.5),
			rect.Y - (int)(((rect.Height * scale) - rect.Height) * 0.5),
			(int)(rect.Width * scale),
			(int)(rect.Height * scale));
		}

		public static SizeF Scale(SizeF size, double scale)
		{
			return new SizeF(
			(float)(size.Width * scale),
			(float)(size.Height * scale));
		}

		public static RectangleF Scale(RectangleF rect, double scale)
		{
			return new RectangleF(
			rect.X - (float)(((rect.Width * scale) - rect.Width) * 0.5),
			rect.Y - (float)(((rect.Height * scale) - rect.Height) * 0.5),
			(float)(rect.Width * scale),
			(float)(rect.Height * scale));
		}

		public static Padding Scale(Padding padding, double scale)
		{
			return new Padding(
			(int)(padding.Left * scale),
			(int)(padding.Top * scale),
			(int)(padding.Right * scale),
			(int)(padding.Bottom * scale));
		}

		#endregion Factoring

		#region Windows

		internal static void OnUiChanged()
		{
			try
			{ UIChanged?.Invoke(); }
			catch { }
		}

		[DllImport("User32.dll")]
		private static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("User32.dll")]
		private static extern int GetDpiForSystem();

		[DllImport("gdi32.dll")]
		private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		private const int LOGPIXELSX = 88;

		static UI()
		{
			int dpiX;
			using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				var desktop = GetDC(IntPtr.Zero);
				dpiX = GetDeviceCaps(desktop, LOGPIXELSX);
			}

			WindowsScale = dpiX / 96D;
		}

		#endregion Windows
	}

	public static class FontMeasuring
	{
		private static readonly Dictionary<StringCache, SizeF> _cache = new Dictionary<StringCache, SizeF>();

		public static SizeF Measure(this Graphics graphics, string text, Font font, int width = int.MaxValue)
		{
			var key = new StringCache(text, font.GetHashCode(), width);

			if (_cache.ContainsKey(key))
			{
				return _cache[key];
			}

			return _cache[key] = graphics.MeasureString(
				text,
				new Font(font.FontFamily, font.Size * 96 * (float)UI.WindowsScale / graphics.DpiX, font.Style, font.Unit),
				width);
		}

		public static SizeF Measure(string text, Font font, int width = int.MaxValue)
		{
			var key = new StringCache(text, font.GetHashCode(), width);

			if (_cache.ContainsKey(key))
			{
				return _cache[key];
			}

			using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				return _cache[key] = graphics.MeasureString(
					text,
					new Font(font.FontFamily, font.Size * 96 * (float)UI.WindowsScale / graphics.DpiX, font.Style, font.Unit),
					width);
			}
		}

		public static Rectangle AlignToFontSize(this Rectangle rectangle, Font font, ContentAlignment contentAlignment = ContentAlignment.MiddleCenter, Graphics graphics = null)
		{
			var newSize = new Size(rectangle.Width, 0);

			if (graphics == null)
			{
				newSize.Height = (int)Measure(" ", font).Height.ClosestMultipleTo(rectangle.Height);
			}
			else
			{
				newSize.Height = (int)Measure(graphics, " ", font).Height.ClosestMultipleTo(rectangle.Height);
			}

			return rectangle.Align(newSize, contentAlignment);
		}

		private struct StringCache
		{
			public string Text { get; set; }
			public int Font { get; set; }
			public int Width { get; set; }

			public StringCache(string text, int font, int width)
			{
				Text = text;
				Font = font;
				Width = width;
			}
		}
	}
}