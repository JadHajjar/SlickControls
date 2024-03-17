using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class ItemPaintEventArgs<T, R> : PaintEventArgs where R : IDrawableItemRectangles<T>
{
	public T Item { get; }
	public DrawableItem<T, R> DrawableItem { get; }
	public Rectangle[] InvalidRects { get; }
	public HoverState HoverState { get; set; }
	public Color BackColor { get; set; }
	public bool IsSelected { get; set; }
	public R Rects => DrawableItem.Rectangles;

	public ItemPaintEventArgs(DrawableItem<T, R> item, Graphics graphics, IEnumerable< Rectangle > invalidRects, Rectangle bounds, HoverState hoverState, bool isSelected) : base(graphics, bounds)
	{
		DrawableItem = item;
		InvalidRects = invalidRects.ToArray();
		Item = item.Item;
		HoverState = hoverState;
		IsSelected = isSelected;

		if (InvalidRects.Length == 0)
			InvalidRects = [bounds];
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