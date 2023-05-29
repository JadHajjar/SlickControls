using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickCursors
	{
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

		const uint SPI_SETFONTSMOOTHING = 0x004B;
		const uint SPIF_UPDATEINIFILE = 0x01;
		const uint SPIF_SENDCHANGE = 0x02;

		public static void Initialize()
		{
			typeof(Cursors).GetField("defaultCursor", BindingFlags.Static | BindingFlags.NonPublic)
				.SetValue(null, new Cursor(Properties.Resources.Cursor_Default.GetHicon()));

			typeof(Cursors).GetField("hand", BindingFlags.Static | BindingFlags.NonPublic)
				.SetValue(null, new Cursor(Properties.Resources.Cursor_Hand.GetHicon()));

			// Added in response to neinew's post
			// seems useless, user complained it changes the windows' settings
			//SystemParametersInfo(SPI_SETFONTSMOOTHING, 1, IntPtr.Zero, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
		}
	}
}