using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickControls
{
	public class DrawableItem<T>
	{
		public DrawableItem(T item)
		{
			Item = item;
		}

		public T Item { get; set; }
		public Rectangle Bounds { get; set; }
		public HoverState HoverState { get; set; }
	}
}
