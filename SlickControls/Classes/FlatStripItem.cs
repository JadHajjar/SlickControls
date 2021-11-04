using System.Drawing;

namespace SlickControls
{
	public class SlickStripItem
	{
		public delegate void action();

		public string Text { get; set; }
		public Bitmap Image { get; set; }
		public action Action { get; set; }
		public bool Fade { get; set; }
		public bool Show { get; set; }
		public bool CloseOnClick { get; set; }
		public int Tab { get; set; }
		public bool IsEmpty => string.IsNullOrWhiteSpace(Text) && Image == null;

		internal bool IsOpenable { get; set; }
		internal bool IsOpened { get; set; }
		internal bool IsContent { get; set; }
		internal bool IsFocused { get; set; }
		internal bool IsVisible { get; set; }
		internal Rectangle DrawRectangle { get; set; }

		public SlickStripItem(string text, action action = null, Bitmap image = null, bool show = true, bool fade = false, int tab = 0, bool closeOnClick = true)
		{
			Text = text;
			Image = image;
			Action = action;
			Fade = fade;
			Show = show;
			Tab = tab;
			CloseOnClick = closeOnClick;
		}

		public static SlickStripItem Empty => new SlickStripItem("");
	}
}