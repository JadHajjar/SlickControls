namespace SlickControls;

public class CanDrawItemEventArgs<T, TRectangle> : CanDrawItemEventArgs<T>  where TRectangle : IDrawableItemRectangles<T>
{
	public DrawableItem<T, TRectangle> DrawableItem { get; }

	public CanDrawItemEventArgs(DrawableItem<T, TRectangle> drawableItem) : base(drawableItem.Item)
	{
		DrawableItem = drawableItem;
	}
}

public class CanDrawItemEventArgs<T>
{
	public T Item { get; }
	public bool DoNotDraw { get; set; }

	public CanDrawItemEventArgs(T item)
	{
		Item = item;
	}
}