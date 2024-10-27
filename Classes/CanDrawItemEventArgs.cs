namespace SlickControls;

public class CanDrawItemEventArgs<T>
{
	public IDrawableItem<T> DrawableItem { get; }
	public T Item { get; }
	public bool DoNotDraw { get; set; }

	public CanDrawItemEventArgs(T item)
	{
		Item = item;
	}

	public CanDrawItemEventArgs(IDrawableItem<T> drawableItem) : this(drawableItem.Item)
	{
		DrawableItem = drawableItem;
	}
}