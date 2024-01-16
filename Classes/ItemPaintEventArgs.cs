using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public class ItemPaintEventArgs<T, R> : PaintEventArgs where R : IDrawableItemRectangles<T>
{
	public T Item { get; }
	public DrawableItem<T, R> DrawableItem { get; }
	public HoverState HoverState { get; set; }
	public Color BackColor { get; set; }
	public bool IsSelected { get; }
	public R Rects => DrawableItem.Rectangles;

	public ItemPaintEventArgs(DrawableItem<T, R> item, Graphics graphics, Rectangle bounds, HoverState hoverState, bool isSelected) : base(graphics, bounds)
	{
		DrawableItem = item;
		Item = item.Item;
		HoverState = hoverState;
		IsSelected = isSelected;
	}
}

public class ItemPaintEventArgs<T> : PaintEventArgs
{
	public T Item { get; }
	public DrawableItem<T> DrawableItem { get; }
	public HoverState HoverState { get; set; }
	public Color BackColor { get; set; }

	public ItemPaintEventArgs(DrawableItem<T> item, Graphics graphics, Rectangle bounds, HoverState hoverState) : base(graphics, bounds)
	{
		DrawableItem = item;
		Item = item.Item;
		HoverState = hoverState;
	}
}