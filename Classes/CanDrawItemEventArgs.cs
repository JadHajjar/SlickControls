namespace SlickControls;

public class CanDrawItemEventArgs<T>
{
	public T Item { get; }
	public bool DoNotDraw { get; set; }

	public CanDrawItemEventArgs(T item)
	{
		Item = item;
	}
}