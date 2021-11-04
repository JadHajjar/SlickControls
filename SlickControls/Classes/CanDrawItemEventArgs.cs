using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickControls
{
	public class CanDrawItemEventArgs<T>
	{
		public T Item { get; }
		public bool DoNotDraw { get; set; }

		public CanDrawItemEventArgs(T item)
		{
			Item = item;
		}
	}
}
