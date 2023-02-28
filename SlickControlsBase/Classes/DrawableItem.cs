using System.Drawing;

namespace SlickControls
{
	public class DrawableItem<T>
	{
		public DrawableItem(T item)
		{
			Item = item;
		}

		public T Item { get; set; }
		public Rectangle Bounds { get; set; }
		public HoverState HoverState { get; set; }
	}
}