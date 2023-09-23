using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickControls
{
	public interface IHoverControl
	{
		HoverState HoverState { get; }
	}

	public interface ILoaderControl
	{
		bool Loading { get; set; }
		void DrawLoader(Graphics g, Rectangle rectangle, Color? color = null);
	}
}
