using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;
public class GenericDrawableItemRectangles<T> : IDrawableItemRectangles<T>
{
	public T Item { get; set; }

	public virtual bool GetToolTip(Control instance, Point location, out string text, out Point point)
	{
		text = string.Empty;
		point = default;
		return false;
	}

	public virtual bool IsHovered(Control instance, Point location)
	{
		return false;
	}
}
