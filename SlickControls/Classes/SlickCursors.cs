using System.Reflection;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickCursors
	{
		public static void Initialize()
		{
			typeof(Cursors).GetField("defaultCursor", BindingFlags.Static | BindingFlags.NonPublic)
				.SetValue(null, new Cursor(Properties.Resources.Cursor_Default.GetHicon()));

			typeof(Cursors).GetField("hand", BindingFlags.Static | BindingFlags.NonPublic)
				.SetValue(null, new Cursor(Properties.Resources.Cursor_Hand.GetHicon()));
		}
	}
}