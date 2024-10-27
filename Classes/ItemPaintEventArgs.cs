using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class ItemPaintEventArgs<T, R> : ItemPaintEventArgs<T> where R : IDrawableItemRectangles<T>
{
	public Rectangle[] InvalidRects { get; }
	public R Rects => (R)DrawableItem.Rectangles;

	public ItemPaintEventArgs(DrawableItem<T, R> item, Graphics graphics, IEnumerable< Rectangle > invalidRects, Rectangle bounds, HoverState hoverState, bool isSelected)
		: base(item, graphics, bounds, hoverState, isSelected)
	{
		InvalidRects = invalidRects.ToArray();
		HoverState = hoverState;

		if (InvalidRects.Length == 0)
			InvalidRects = [bounds];
	}
}

public class ItemPaintEventArgs<T> : PaintEventArgs
{
	public T Item { get; }
	public IDrawableItem<T> DrawableItem { get; }
	public HoverState HoverState { get; set; }
	public Color BackColor { get; set; }
	public bool IsSelected { get; set; }

	public ItemPaintEventArgs(IDrawableItem<T> item, Graphics graphics, Rectangle bounds, HoverState hoverState, bool isSelected = false) : base(graphics, bounds)
	{
		DrawableItem = item;
		Item = item.Item;
		HoverState = hoverState;
		IsSelected = isSelected;
	}
}