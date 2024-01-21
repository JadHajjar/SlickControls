using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls;

public class UI : ISave
{
	public static event UIEventHandler UIChanged;

	public delegate void UIEventHandler();

	public static string FontFamily => _instance.fontFamily ?? (CrossIO.CurrentPlatform == Platform.MacOSX ? "Helvetica" : "Segoe UI");
	public static double FontScale => _instance.fontScale * WindowsScale;
	public static double UIScale => Math.Round(FontScale.If(x => x > 1, x => (x * .9) + 0.1, x => (x * 1.1) - 0.1), 2);
	public static double WindowsScale { get; }

	public string fontFamily { get; set; }
	public double fontScale { get; set; } = 1.25;
	public bool noAnimations { get => _noAnimations; set => AnimationHandler.NoAnimations = _noAnimations = value; }

	internal static readonly UI _instance;

	private bool _noAnimations;

	#region Factoring

	public static Font Font(float size, FontStyle style = FontStyle.Regular)
	{
		return new Font(
		(style.HasFlag(FontStyle.Italic) && FontFamily == "Nirmala UI") ? "Century Gothic" : FontFamily,
		(float)(_instance.fontScale * size).RoundToMultipleOf(0.25F),
		style);
	}

	public static Font Font(string fontFamily, float size, FontStyle style = FontStyle.Regular)
	{
		return new Font(
		fontFamily,
		(float)(_instance.fontScale * size).RoundToMultipleOf(0.25F),
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
		{
			UIChanged?.Invoke();
		}
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
		if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SlickUI")))
		{
			try
			{
				if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Shared")))
				{
					Directory.Move(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Shared"), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SlickUI"));
				}
			}
			catch { }

			Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SlickUI"));
		}

		_instance = Load<UI>("UI.tf", "SlickUI");

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
	private static readonly Dictionary<StringCache, SizeF> _cache = [];

	public static SizeF Measure(this Graphics graphics, string text, Font font, int width = int.MaxValue)
	{
		var key = new StringCache(text, font.GetHashCode(), width);

		if (_cache.ContainsKey(key))
		{
			return _cache[key];
		}

		using var dpiFont = new Font(font.FontFamily, font.Size * 96 * (float)UI.WindowsScale / graphics.DpiX, font.Style, font.Unit);

		return _cache[key] = graphics.MeasureString(text, dpiFont, width);
	}

	public static SizeF Measure(string text, Font font, int width = int.MaxValue)
	{
		var key = new StringCache(text, font.GetHashCode(), width);

		if (_cache.ContainsKey(key))
		{
			return _cache[key];
		}

		using var graphics = Graphics.FromHwnd(IntPtr.Zero);
		using var dpiFont = new Font(font.FontFamily, font.Size * 96 * (float)UI.WindowsScale / graphics.DpiX, font.Style, font.Unit);
		
		return _cache[key] = graphics.MeasureString(text, dpiFont, width);
	}

	public static Rectangle AlignToFontSize(this Rectangle rectangle, Font font, ContentAlignment contentAlignment = ContentAlignment.MiddleCenter, Graphics graphics = null, bool upperBounds = false)
	{
		var newSize = new Size(rectangle.Width, 0);

		if (graphics == null)
		{
			newSize.Height = (int)Measure(" ", font, rectangle.Width).Height.ClosestMultipleTo(rectangle.Height, upperBounds);
		}
		else
		{
			newSize.Height = (int)Measure(graphics, " ", font, rectangle.Width).Height.ClosestMultipleTo(rectangle.Height, upperBounds);
		}

		return rectangle.Align(newSize, contentAlignment);
	}

	public static Font FitToHeight(this Font font, string text, Rectangle rectangle, Graphics graphics = null)
	{
		var _graphics = graphics ?? Graphics.FromHwnd(IntPtr.Zero);

		try
		{
			while (font.Size > 0.75f && Measure(_graphics, text, font, rectangle.Width).Height > rectangle.Height)
			{
				font.Dispose();
				font = new Font(font.FontFamily, font.Size - 0.75f, font.Style);
			}

			return font;
		}
		finally
		{
			if (graphics is null)
			{
				_graphics.Dispose();
			}
		}
	}

	public static Font FitToWidth(this Font font, string text, Rectangle rectangle, Graphics graphics = null)
	{
		var _graphics = graphics ?? Graphics.FromHwnd(IntPtr.Zero);

		try
		{
			while (font.Size > 0.75f && Measure(_graphics, text, font).Width * 1.1f > rectangle.Width)
			{
				font.Dispose();
				font = new Font(font.FontFamily, font.Size - 0.75f, font.Style);
			}

			return font;
		}
		finally
		{
			if (graphics is null)
			{
				_graphics.Dispose();
			}
		}
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