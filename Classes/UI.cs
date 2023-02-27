using Extensions;

using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls
{
	public class UI : ISave
	{
		public static event UIEventHandler UIChanged;

		public delegate void UIEventHandler();

		public static string FontFamily => _instance.fontFamily;
		public static double FontScale => _instance.fontScale * WindowsScale;
		public static double UIScale => Math.Round(FontScale.If(x => x > 1, x => x * .9 + 0.1, x => x * 1.1 - 0.1), 2);
		public static double WindowsScale { get; }

		public string fontFamily { get; set; } = "Nirmala UI";
		public double fontScale { get; set; } = 1;
		public double uiScale { get; set; } = 1;
		public bool noAnimations { get => _noAnimations; set => AnimationHandler.NoAnimations = _noAnimations = value; }

		internal static readonly UI _instance = Load<UI>("UI.tf", "Shared");

		private bool _noAnimations;

		#region Factoring

		public static Font Font(float size, FontStyle style = FontStyle.Regular) => new Font(
			(style.HasFlag(FontStyle.Italic) && FontFamily == "Nirmala UI") ? "Century Gothic" : FontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);

		public static Font Font(float size, Graphics g, FontStyle style = FontStyle.Regular) => new Font(
			(style.HasFlag(FontStyle.Italic) && FontFamily == "Nirmala UI") ? "Century Gothic" : FontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);

		public static Font Font(string fontFamily, float size, FontStyle style = FontStyle.Regular) => new Font(
			fontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);

		public static Font Font(string fontFamily, float size, Graphics g, FontStyle style = FontStyle.Regular) => new Font(
			fontFamily,
			(float)(_instance.fontScale * size).RoundToMultipleOf(0.75F),
			style);

		public static Size Scale(Size size, double scale) => new Size(
			(int)(size.Width * scale),
			(int)(size.Height * scale));

		public static Rectangle Scale(Rectangle rect, double scale) => new Rectangle(
			rect.X - (int)(((rect.Width * scale) - rect.Width) * 0.5),
			rect.Y - (int)(((rect.Height * scale) - rect.Height) * 0.5),
			(int)(rect.Width * scale),
			(int)(rect.Height * scale));

		public static SizeF Scale(SizeF size, double scale) => new SizeF(
			(float)(size.Width * scale),
			(float)(size.Height * scale));

		public static RectangleF Scale(RectangleF rect, double scale) => new RectangleF(
			rect.X - (float)(((rect.Width * scale) - rect.Width) * 0.5),
			rect.Y - (float)(((rect.Height * scale) - rect.Height) * 0.5),
			(float)(rect.Width * scale),
			(float)(rect.Height * scale));

		public static Padding Scale(Padding padding, double scale) => new Padding(
			(int)(padding.Left * scale),
			(int)(padding.Top * scale),
			(int)(padding.Right * scale),
			(int)(padding.Bottom * scale));

		#endregion Factoring

		#region Windows

		internal static void OnUiChanged() => UIChanged?.Invoke();
		[DllImport("User32.dll")]
		private static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("User32.dll")]
		private static extern int GetDpiForSystem();

		[DllImport("gdi32.dll")]
		private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		private const int LOGPIXELSX = 88;

		static UI()
		{
			var pfc = new PrivateFontCollection();

			add(Properties.Resources.Nirmala);
			add(Properties.Resources.NirmalaB);
			add(Properties.Resources.GOTHICI);

			void add(byte[] streamData)
			{
				var data = Marshal.AllocCoTaskMem(streamData.Length);
				Marshal.Copy(streamData, 0, data, streamData.Length);
				pfc.AddMemoryFont(data, streamData.Length);
				Marshal.FreeCoTaskMem(data);
			}

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
		public static SizeF Measure(this Graphics graphics, string text, Font font, int width = int.MaxValue)
		{
			return graphics.MeasureString(
				text,
				new Font(font.FontFamily, font.Size * 96 * (float)UI.WindowsScale / graphics.DpiX, font.Style, font.Unit),
				width);
		}
	}
}