using System.Drawing;

namespace SlickControls;

public interface IHoverControl
{
	HoverState HoverState { get; }
}

public interface ILoaderControl
{
	bool Loading { get; set; }
	void DrawLoader(Graphics g, Rectangle rectangle, Color? color = null);
}
