using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
	public class ItemPaintEventArgs<T> : PaintEventArgs
	{
		public T Item { get; }
		public HoverState HoverState { get; }

		public ItemPaintEventArgs(T item, Rectangle bounds, HoverState hoverState, Graphics graphics) : base(graphics, bounds)
		{
			Item = item;
			HoverState = hoverState;
		}
	}
}
