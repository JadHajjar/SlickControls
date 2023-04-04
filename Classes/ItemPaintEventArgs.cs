using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class ItemPaintEventArgs<T> : PaintEventArgs
	{
		public T Item { get; }
		public DrawableItem<T> DrawableItem { get; }
		public HoverState HoverState { get; set; }

		public ItemPaintEventArgs(DrawableItem<T> item, Graphics graphics, Rectangle bounds, HoverState hoverState) : base(graphics, bounds)
		{
			DrawableItem = item;
			Item = item.Item;
			HoverState = hoverState;
		}
	}
}