using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public static class DesignExtensions
	{
		public static void DrawLoader(this Graphics g, double loaderPercentage, Rectangle rectangle, Color? color = null)
		{
			var width = Math.Min(Math.Min(rectangle.Width, rectangle.Height), (int)(64 * UI.UIScale));
			var size = (float)Math.Max(2, width / (8D - Math.Abs(100 - loaderPercentage) / 50));
			var arc = 100 + 1.7 * (50 - Math.Abs(100 - loaderPercentage));
			var angle = ((loaderPercentage * 36 / 20) + DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond / 3) % 360D;
			var drawSize = new SizeF(width - size, width - size);
			var rect = new RectangleF(new PointF(rectangle.X + (rectangle.Width - drawSize.Width) / 2, rectangle.Y + (rectangle.Height - drawSize.Height) / 2), drawSize).Pad(size / 2);
			var sm = g.SmoothingMode;

			g.SmoothingMode = SmoothingMode.HighQuality;

			using (var pen = new Pen(color ?? FormDesign.Design.ActiveColor, size) { StartCap = LineCap.Round, EndCap = LineCap.Round })
				g.DrawArc(pen, rect, (float)angle, (float)arc);

			g.SmoothingMode = sm;
		}

		public static void DrawBannersOverImage(this Graphics g, Control control, Rectangle rectangle, IEnumerable<Banner> banners, float fontSize = 7F, double opacity = 1)
			=> DrawBannersOverImage(g, control?.PointToClient(Cursor.Position) ?? new Point(-1, -1), rectangle, banners, fontSize, opacity);

		public static void DrawBannersOverImage(this Graphics g, Point cursorLocation, Rectangle rectangle, IEnumerable<Banner> banners, float fontSize = 7F, double opacity = 1)
		{
			if (banners == null) return;

			var sm = g.SmoothingMode;
			var tab = 0;

			try
			{
				g.SmoothingMode = SmoothingMode.HighQuality;

				using (var font = UI.Font(fontSize))
				{
					var transparent = !SlickAdvancedImageControl.AlwaysShowBanners || rectangle.Contains(cursorLocation);
					
					foreach (var banner in banners)
					{
						if (banner?.Icon == null && string.IsNullOrWhiteSpace(banner?.Text)) continue;

						using (var foreBrush = new SolidBrush(Color.FromArgb((int)(255 * opacity), banner.Style == BannerStyle.Custom ? banner.Color : banner.Style.ForeColor())))
						{
							var iconSize = banner.Icon?.Size ?? Size.Empty;
							var noText = string.IsNullOrWhiteSpace(banner.Text);
							var size = g.MeasureString(banner.Text, font, rectangle.Width - 12 - font.Height / 2 - (banner.Icon == null ? 0 : (iconSize.Width + 5)));
							var h = Math.Max(iconSize.Height, (int)size.Height) + 4; h += 1 - h % 2;

							var bannerSize = noText
								? new Size(h * 3 / 2 - 4, h)
								: new Size((int)size.Width + font.Height / 2 + (banner.Icon == null ? 0 : (iconSize.Width + 5)), h);

							bannerSize.Width = Math.Min(bannerSize.Width, rectangle.Width - 12);

							var bannerRect = new Rectangle(new Point(rectangle.X + 6, rectangle.Y + 6 + tab), bannerSize);

							if (bannerRect.Y + bannerRect.Height > rectangle.Height + rectangle.Y)
								return;

							using (var backBrush = bannerRect.Gradient(Color.FromArgb((int)((transparent ? 200 : 245) * opacity), banner.Style.BackColor()), 2))
								g.FillRoundedRectangle(backBrush, bannerRect, h / 3);

							g.DrawString(banner.Text, font, foreBrush, bannerRect.Pad(banner.Icon == null ? 6 : (iconSize.Width + 8), 0, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center });

							if (banner.Icon != null)
								g.DrawImage(banner.Icon.Color(banner.Style == BannerStyle.Custom ? banner.Color : banner.Style.ForeColor(), (byte)(int)(255 * opacity)), noText
									? new Rectangle(bannerRect.X + 1 + ((bannerRect.Width - iconSize.Width) / 2), bannerRect.Y + 1 + ((h - iconSize.Height) / 2), iconSize.Width, iconSize.Height)
									: new Rectangle(bannerRect.X - 2 + (iconSize.Width / 2), bannerRect.Y + 1 + ((h - iconSize.Height) / 2), iconSize.Width, iconSize.Height));

							tab += bannerRect.Height + 4;
						}
					}
				}
			}
			finally
			{
				g.SmoothingMode = sm;
			}
		}

		public static Color BackColor(this BannerStyle style)
		{
			switch (style)
			{
				case BannerStyle.Active:
					return FormDesign.Design.ActiveForeColor;

				case BannerStyle.Text:
					return FormDesign.Design.MenuColor;

				default:
					return FormDesign.Design.Type.If(FormDesignType.Light, FormDesign.Design.ForeColor, FormDesign.Design.MenuColor);
			}
		}

		public static Color ForeColor(this BannerStyle style)
		{
			switch (style)
			{
				case BannerStyle.Active:
					return FormDesign.Design.ActiveColor;

				case BannerStyle.Green:
					return FormDesign.Design.GreenColor;

				case BannerStyle.Yellow:
					return FormDesign.Design.YellowColor;

				case BannerStyle.Red:
					return FormDesign.Design.RedColor;

				case BannerStyle.Text:
					return FormDesign.Design.MenuForeColor;

				default:
					return FormDesign.Design.ActiveColor;
			}
		}
	}
}