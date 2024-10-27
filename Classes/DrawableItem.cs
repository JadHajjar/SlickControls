using System.Drawing;

namespace SlickControls;

public class DrawableItem<T> : IDrawableItem<T>
{
    public DrawableItem()
    {
        
    }

    public DrawableItem(T item)
	{
		Item = item;
	}

	public IDrawableItemRectangles<T> Rectangles { get; set; }
	public T Item { get; set; }
	public Rectangle Bounds { get; set; }
	public HoverState HoverState { get; set; }
	public int CachedHeight { get; set; }
	public bool Loading { get; set; }
	public bool Hidden { get; set; }
	public object Tag { get; set; }
}

public class DrawableItem<T, TRectangle> : IDrawableItem<T> where TRectangle : IDrawableItemRectangles<T>
{
	public DrawableItem()
	{

	}

	public DrawableItem(T item)
	{
		Item = item;
	}

	public TRectangle Rectangles { get; set; }
	public T Item { get; set; }
	public Rectangle Bounds { get; set; }
	public HoverState HoverState { get; set; }
	public int CachedHeight { get; set; }
	public bool Loading { get; set; }
	public bool Hidden { get; set; }
	public object Tag { get; set; }
	IDrawableItemRectangles<T> IDrawableItem<T>.Rectangles { get => Rectangles; set => Rectangles = (TRectangle)value; }
}

public interface IDrawableItem<T>
{
	IDrawableItemRectangles<T> Rectangles { get; set; }
	T Item { get; set; }
	Rectangle Bounds { get; set; }
	HoverState HoverState { get; set; }
	int CachedHeight { get; set; }
	bool Loading { get; set; }
	bool Hidden { get; set; }
	object Tag { get; set; }
}
