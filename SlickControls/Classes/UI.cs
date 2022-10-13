using Extensions;

using System;
using System.Drawing;
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
		public static double UIScale => FontScale.If(x => x > 1, x => x * .9 + 0.1, x => x * 1.1 - 0.1);
		public static double WindowsScale { get; } = (double)Graphics.FromHwnd(IntPtr.Zero).DpiX / 96;

		public string fontFamily { get; set; } = "Nirmala UI";
		public double fontScale { get; set; } = 1;
		public double uiScale { get; set; } = 1;

		internal static readonly UI _instance = Load<UI>("UI.tf", "Shared");

		internal static void OnUiChanged() => UIChanged?.Invoke();

		#region Factoring

		public static Font Font(float size, FontStyle style = FontStyle.Regular) => new Font(
			(style.HasFlag(FontStyle.Italic) && FontFamily == "Nirmala UI") ? "Century Gothic" : FontFamily,
			(float)(FontScale * size / WindowsScale).RoundToMultipleOf(0.75F),
			style);

		public static Font Font(float size, Graphics g, FontStyle style = FontStyle.Regular) => new Font(
			(style.HasFlag(FontStyle.Italic) && FontFamily == "Nirmala UI") ? "Century Gothic" : FontFamily,
			(float)(FontScale * size * g.DpiX / 96).RoundToMultipleOf(0.75F),
			style);

		public static Font Font(string fontFamily, float size, FontStyle style = FontStyle.Regular) => new Font(
			fontFamily,
			(float)(FontScale * size / WindowsScale).RoundToMultipleOf(0.75F),
			style);

		public static Font Font(string fontFamily, float size, Graphics g, FontStyle style = FontStyle.Regular) => new Font(
			fontFamily,
			(float)(FontScale * size * g.DpiX / 96).RoundToMultipleOf(0.75F),
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
			rect.X,
			rect.Y,
			(float)(rect.Width * scale),
			(float)(rect.Height * scale));

		public static Padding Scale(Padding padding, double scale) => new Padding(
			(int)(padding.Left * scale),
			(int)(padding.Top * scale),
			(int)(padding.Right * scale),
			(int)(padding.Bottom * scale));

		#endregion Factoring

		#region Windows

		//public static Size VirutalScreen { get; }
		//public static Size PhysicalScreen { get; }

		//static UI()
		//{
		//	using (var g = Graphics.FromHwnd(IntPtr.Zero))
		//	{
		//		var desktop = g.GetHdc();

		//		WindowsScale = (double)GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES)
		//			/ GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
		//	}
		//}

		[DllImport("gdi32.dll")]
		private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		public enum DeviceCap
		{
			VERTRES = 10,
			DESKTOPVERTRES = 117
		}

		#endregion Windows
	}
}