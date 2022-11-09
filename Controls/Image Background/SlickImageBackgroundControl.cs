using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickImageBackgroundControl : IDisposable
	{
		public SlickImageBackgroundContainer Container { get; internal set; }

		[Category("Layout")]
		public AnchorStyles Anchor { get; set; }
		[Category("Layout")]
		public DockStyle Dock { get; set; }
		[Category("Layout")]
		public bool AutoSize { get; set; }
		[Category("Layout")]
		public Rectangle Bounds { get; set; }
		[Category("Layout")]
		public Point Location { get => Bounds.Location; set => Bounds = new Rectangle(value, Bounds.Size); }
		[Category("Layout")]
		public Size Size { get => Bounds.Size; set => Bounds = new Rectangle(Bounds.Location, value); }
		[Category("Appearance")]
		public string Text { get; set; }
		[Category("Appearance")]
		public Image Image { get; set; }
		[Category("Appearance")]
		public Font Font { get; set; }

		#region Dispose
		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					Image?.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
