using System.Drawing;

namespace SlickControls;

public class SizeSourceEventArgs<T>
{
	public T Item { get; }
	public Graphics Graphics { get; }
	public int Size { get; set; }
	public bool Handled { get; set; }

	public SizeSourceEventArgs(T item, Graphics graphics)
	{
		Item = item;
		Graphics = graphics;
	}
}