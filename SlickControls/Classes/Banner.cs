using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickControls
{
	public class Banner
	{
		public string Text { get; set; }
		public BannerStyle Style { get; set; }
		public Bitmap Icon { get; set; }
		public Color Color { get; set; }

		public Banner() { }

		public Banner(string text, BannerStyle style, Bitmap icon = null)
		{
			Text = text;
			Style = style;
			Icon = icon;
		}

		public Banner(string text, Color color, Bitmap icon = null)
		{
			Text = text;
			Style = BannerStyle.Custom;
			Color = color;
			Icon = icon;
		}
	}
}
