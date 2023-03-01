using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class ItemPaintEventArgs<T> : PaintEventArgs
	{
		public T Item { get; }
		public HoverState HoverState { get; }

		public ItemPaintEventArgs(T item, Graphics graphics, Rectangle bounds, HoverState hoverState) : base(graphics, bounds)
		{
			Item = item;
			HoverState = hoverState;
		}
	}
}