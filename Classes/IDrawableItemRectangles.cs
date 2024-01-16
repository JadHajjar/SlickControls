using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public interface IDrawableItemRectangles<T>
{
	T Item { get; set; }

	bool IsHovered(Control instance, Point location);
	bool GetToolTip(Control instance, Point location, out string text, out Point point);
}