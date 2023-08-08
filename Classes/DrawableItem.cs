using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class DrawableItem<T, R> : DrawableItem<T> where R : IDrawableItemRectangles<T>
	{
		public DrawableItem(T item) : base(item) { }
		
		public R Rectangles { get; set; }
	}

	public class DrawableItem<T>
	{
		public DrawableItem(T item)
		{
			Item = item;
		}

		public T Item { get; set; }
		public Rectangle Bounds { get; set; }
		public HoverState HoverState { get; set; }
		public int CachedHeight { get; set; }
		internal bool Hidden { get; set; }
	}
}