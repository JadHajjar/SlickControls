using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls;

public static class DesignExtensions
{
	public static Rectangle DrawLabel(this Graphics graphics, string text, Bitmap icon, Color color, Rectangle rectangle, ContentAlignment alignment, bool smaller = false, bool large = false, Point? mousePosition = null, bool recolor = true)
	{
		if (text == null)
		{
			return Rectangle.Empty;
		}

		using (var font = UI.Font((large ? 9F : 7.5F) - (smaller ? 1F : 0F), large ? FontStyle.Bold : FontStyle.Regular))
		{
			var padding = UI.Scale(new Padding(3, 2, 3, 2));
			var size = graphics.Measure(text, font).ToSize();

			if (icon != null)
			{
				size.Width += icon.Width + padding.Left;
			}

			size.Width += padding.Left + 1;

			if (rectangle.Width > 0 && size.Width > rectangle.Width)
			{
				if (alignment == ContentAlignment.TopLeft)
				{
					alignment = ContentAlignment.TopRight;
				}
				else if (alignment == ContentAlignment.MiddleLeft)
				{
					alignment = ContentAlignment.MiddleRight;
				}
				else if (alignment == ContentAlignment.BottomLeft)
				{
					alignment = ContentAlignment.BottomRight;
				}
			}

			rectangle = rectangle.Pad(smaller ? padding.Left / 2 : padding.Left).Align(size, alignment);

			if (mousePosition.HasValue)
			{
				if (color == default && rectangle.Contains(mousePosition.Value))
				{
					color = FormDesign.Design.BackColor.MergeColor(FormDesign.Design.ForeColor, 75);
				}
				else if (!rectangle.Contains(mousePosition.Value))
				{
					color = color.MergeColor(FormDesign.Design.BackColor, 50);
				}
			}

			using (var backBrush = rectangle.Gradient(color, 0.35F))
			using (var foreBrush = new SolidBrush(color.GetTextColor()))
			using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
			{
				graphics.FillRoundedRectangle(backBrush, rectangle, UI.Scale(3));
				graphics.DrawString(text, font, foreBrush, icon is null ? rectangle : rectangle.Pad(icon.Width + padding.Left, 0, 0, 0), stringFormat);
			}

			if (icon != null)
			{
				graphics.DrawImage(recolor ? icon.Color(color.GetTextColor()) : icon, rectangle.Pad(padding.Left, 0, 0, 0).Align(icon.Size, ContentAlignment.MiddleLeft));
			}
		}

		return rectangle;
	}

	public static Size MeasureLabel(this Graphics graphics, string text, Bitmap icon, bool smaller = false, bool large = false)
	{
		if (text == null)
		{
			return default;
		}

		using var font = UI.Font((large ? 9F : 7.5F) - (smaller ? 1F : 0F), large ? FontStyle.Bold : FontStyle.Regular);
		var padding = UI.Scale(new Padding(3, 2, 3, 2));
		var size = graphics.Measure(text, font).ToSize();

		size.Width = (int)(size.Width * 1.1f);

		if (icon != null)
		{
			size.Width += icon.Width + padding.Left;
		}

		size.Width += padding.Left;

		return size;
	}


	public static Rectangle DrawLargeLabel(this Graphics graphics, Point point, string text, Bitmap bitmap, Color? color = null, ContentAlignment alignment = ContentAlignment.TopLeft, Padding? padding = null, int height = 0, Point? cursorLocation = null)
	{
		using var font = UI.Font(8.25F, FontStyle.Bold);
		if (height == 0)
		{
			height = UI.Scale(24);
		}

		var pad = padding ?? UI.Scale(new Padding(3));
		var size = new Size(string.IsNullOrEmpty(text) ? height : ((int)graphics.Measure(text, font).Width + pad.Horizontal + (height * 3 / 4)), height);
		var rect = new Rectangle(point.X, point.Y, 0, 0).Align(size, alignment);
		var iconRect = rect.Pad(pad).Align(new Size(height * 3 / 4, height * 3 / 4), text == "" ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft);
		using (var brush = new SolidBrush(color.HasValue ? Color.FromArgb(!cursorLocation.HasValue || rect.Contains(cursorLocation.Value) ? 255 : 160, color.Value) : Color.FromArgb(120, !cursorLocation.HasValue || rect.Contains(cursorLocation.Value) ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor.MergeColor(FormDesign.Design.AccentBackColor, 40))))
		using (var textBrush = new SolidBrush(brush.Color.GetTextColor()))
		using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
		{
			graphics.FillRoundedRectangle(brush, rect, UI.Scale(4));
			graphics.DrawString(text, font, textBrush, rect.Pad(iconRect.Width, 0, 0, 0), stringFormat);
		}

		graphics.DrawRoundImage(bitmap, iconRect);

		return rect;
	}

	public static Rectangle DrawLargeLabel(this Graphics graphics, Point point, string text, DynamicIcon icon, Color? color = null, ContentAlignment alignment = ContentAlignment.TopLeft, Padding? padding = null, int height = 0, Point? cursorLocation = null, bool smaller = false)
	{
		using var font = UI.Font(smaller ? 7.5F : 8.25F, FontStyle.Bold);
		if (height == 0)
		{
			height = UI.Scale(24);
		}

		var pad = padding ?? UI.Scale(new Padding(3));
		var size = new Size(string.IsNullOrEmpty(text) ? height : ((int)graphics.Measure(text, font).Width + (icon == null ? pad.Left : (pad.Horizontal + (height * 3 / 4)))), height);
		var rect = new Rectangle(point.X, point.Y, 0, 0).Align(size, alignment);
		var iconRect = icon == null ? default : rect.Pad(pad).Align(new Size(height * 3 / 4, height * 3 / 4), text == "" ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft);

		using (var brush = new SolidBrush(color.HasValue ? Color.FromArgb(!cursorLocation.HasValue || rect.Contains(cursorLocation.Value) ? 255 : 160, color.Value) : Color.FromArgb(120, !cursorLocation.HasValue || rect.Contains(cursorLocation.Value) ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor.MergeColor(FormDesign.Design.AccentBackColor, 40))))
		using (var textBrush = new SolidBrush(brush.Color.GetTextColor()))
		using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
		{
			graphics.FillRoundedRectangle(brush, rect, UI.Scale(4));
			graphics.DrawString(text, font, textBrush, rect.Pad(iconRect.Width, 0, 0, 0), stringFormat);

			if (icon != null)
			{
				using var bitmap = icon.Get(iconRect.Height).Color(textBrush.Color);
				graphics.DrawImage(bitmap, iconRect.CenterR(bitmap.Size));
			}
		}

		return rect;
	}

	public static void SetUp(this Graphics graphics, Color? backColor = null)
	{
		if (backColor.HasValue)
		{
			graphics.Clear(backColor.Value);
		}

		graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
		graphics.SmoothingMode = SmoothingMode.HighQuality;
		graphics.TextRenderingHint = UI.WindowsScale >= 1.5 ? System.Drawing.Text.TextRenderingHint.AntiAliasGridFit : System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
		graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
	}

	public static Size DrawStringItem(this Graphics graphics, object item, Font font, Color foreColor, int maxWidth, double tab, ref int height, bool draw = true, DynamicIcon dIcon = null)
	{
		var margin = UI.Scale(6);
		using var icon = dIcon?.Get(font.Height + margin);
		var x = (int)(((tab * 12) + 6) * UI.FontScale);
		var bnds = graphics.Measure(item?.ToString(), font, maxWidth - x - (icon == null ? 0 : (icon.Width + margin)));

		if (draw)
		{
			var textRect = new Rectangle(x, height, maxWidth - x, (int)Math.Ceiling(bnds.Height));

			if (icon != null)
			{
				graphics.DrawImage(icon.Color(foreColor), textRect.Align(icon.Size, ContentAlignment.MiddleLeft));

				textRect = textRect.Pad(icon.Width + (margin / 2), 0, 0, 0);
			}

			using var brush = new SolidBrush(foreColor);
			graphics.DrawString(item?.ToString(), font, brush, textRect);
		}

		height += (int)(bnds.Height + margin);

		return bnds.ToSize();
	}

	public static Size DrawStringItem(this Graphics graphics, object item, Font font, Color foreColor, Rectangle rectangle, ref int height, bool draw = true, DynamicIcon dIcon = null, StringFormat stringFormat = null)
	{
		var margin = UI.Scale(6);
		using var icon = dIcon?.Get(font.Height + margin);
		var bnds = graphics.Measure(item?.ToString(), font, rectangle.Width - (icon == null ? 0 : (icon.Width + margin)));

		if (draw)
		{
			var textRect = new Rectangle(rectangle.X, height, rectangle.Width, (int)Math.Ceiling(bnds.Height));

			if (icon != null)
			{
				graphics.DrawImage(icon.Color(foreColor), textRect.Align(icon.Size, ContentAlignment.MiddleLeft));

				textRect = textRect.Pad(icon.Width + (margin / 2), 0, 0, 0);
			}

			using var brush = new SolidBrush(foreColor);
			graphics.DrawString(item?.ToString(), font, brush, textRect, stringFormat);
		}

		height += (int)(bnds.Height + margin);

		return bnds.ToSize();
	}

	public static Size DrawStringItem(this Graphics graphics, object item, Font font, Color foreColor, ref Rectangle rectangle, bool draw = true, DynamicIcon dIcon = null, StringFormat stringFormat = null)
	{
		var margin = UI.Scale(6);
		using var icon = dIcon?.Get(font.Height + margin);
		var bnds = graphics.Measure(item?.ToString(), font, rectangle.Width - (icon == null ? 0 : (icon.Width + margin)));

		if (draw)
		{
			var textRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, (int)Math.Ceiling(bnds.Height));

			if (icon != null)
			{
				graphics.DrawImage(icon.Color(foreColor), textRect.Align(icon.Size, ContentAlignment.MiddleLeft));

				textRect = textRect.Pad(icon.Width + (margin / 2), 0, 0, 0);
			}

			using var brush = new SolidBrush(foreColor);
			graphics.DrawString(item?.ToString(), font, brush, textRect, stringFormat);
		}

		rectangle.Y += (int)(bnds.Height + margin);

		return bnds.ToSize();
	}

	public static void DrawLoader(this Graphics g, double loaderPercentage, Rectangle rectangle, Color? color = null)
	{
		if (rectangle.Height <= 0 || rectangle.Width <= 0)
		{
			return;
		}

		var width = Math.Min(Math.Min(rectangle.Width, rectangle.Height), (int)(32 * UI.UIScale));
		var size = (float)Math.Max(2, width / (8D - (Math.Abs(100 - loaderPercentage) / 50)));
		var arc = 100 + (1.7 * (50 - Math.Abs(100 - loaderPercentage)));
		var angle = ((loaderPercentage * 36 / 20) + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond / 2.5)) % 360D;
		var drawSize = new SizeF(width - size, width - size);
		var rect = new RectangleF(new PointF(rectangle.X + ((rectangle.Width - drawSize.Width) / 2), rectangle.Y + ((rectangle.Height - drawSize.Height) / 2)), drawSize).Pad(size / 2);
		var sm = g.SmoothingMode;

		g.SmoothingMode = SmoothingMode.HighQuality;

		using (var pen = new Pen(color ?? FormDesign.Design.ActiveColor, size) { StartCap = LineCap.Round, EndCap = LineCap.Round })
		{
			g.DrawArc(pen, rect, (float)angle, (float)arc);
		}

		g.SmoothingMode = sm;
	}

	public static void DrawBannersOverImage(this Graphics g, Control control, Rectangle rectangle, IEnumerable<Banner> banners, float fontSize = 7F, double opacity = 1)
	{
		DrawBannersOverImage(g, control?.PointToClient(Cursor.Position) ?? new Point(-1, -1), rectangle, banners, fontSize, opacity);
	}

	public static void DrawBannersOverImage(this Graphics g, Point cursorLocation, Rectangle rectangle, IEnumerable<Banner> banners, float fontSize = 7F, double opacity = 1)
	{
		if (banners == null)
		{
			return;
		}

		var sm = g.SmoothingMode;
		var tab = 0;

		try
		{
			g.SmoothingMode = SmoothingMode.HighQuality;

			using var font = UI.Font(fontSize);
			var transparent = !SlickAdvancedImageControl.AlwaysShowBanners || rectangle.Contains(cursorLocation);

			foreach (var banner in banners)
			{
				if (banner?.Icon == null && string.IsNullOrWhiteSpace(banner?.Text))
				{
					continue;
				}

				using var foreBrush = new SolidBrush(Color.FromArgb((int)(255 * opacity), banner.Style == BannerStyle.Custom ? banner.Color : banner.Style.ForeColor()));
				var iconSize = banner.Icon?.Size ?? Size.Empty;
				var noText = string.IsNullOrWhiteSpace(banner.Text);
				var size = g.Measure(banner.Text, font, rectangle.Width - 12 - (font.Height / 2) - (banner.Icon == null ? 0 : (iconSize.Width + 5)));
				var h = Math.Max(iconSize.Height, (int)size.Height) + 4;
				h += 1 - (h % 2);

				var bannerSize = noText
					? new Size((h * 3 / 2) - 4, h)
					: new Size((int)size.Width + (font.Height / 2) + (banner.Icon == null ? 0 : (iconSize.Width + 5)), h);

				bannerSize.Width = Math.Min(bannerSize.Width, rectangle.Width - 12);

				var bannerRect = new Rectangle(new Point(rectangle.X + 6, rectangle.Y + 6 + tab), bannerSize);

				if (bannerRect.Y + bannerRect.Height > rectangle.Height + rectangle.Y)
				{
					return;
				}

				using (var backBrush = bannerRect.Gradient(Color.FromArgb((int)((transparent ? 200 : 245) * opacity), banner.Style.BackColor()), 2))
				{
					g.FillRoundedRectangle(backBrush, bannerRect, h / 3);
				}

				g.DrawString(banner.Text, font, foreBrush, bannerRect.Pad(banner.Icon == null ? 6 : (iconSize.Width + 8), 0, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center });

				if (banner.Icon != null)
				{
					g.DrawImage(banner.Icon.Color(banner.Style == BannerStyle.Custom ? banner.Color : banner.Style.ForeColor(), (byte)(int)(255 * opacity)), noText
						? new Rectangle(bannerRect.X + 1 + ((bannerRect.Width - iconSize.Width) / 2), bannerRect.Y + 1 + ((h - iconSize.Height) / 2), iconSize.Width, iconSize.Height)
						: new Rectangle(bannerRect.X - 2 + (iconSize.Width / 2), bannerRect.Y + 1 + ((h - iconSize.Height) / 2), iconSize.Width, iconSize.Height));
				}

				tab += bannerRect.Height + 4;
			}
		}
		finally
		{
			g.SmoothingMode = sm;
		}
	}

	public static void FillRoundedRectangleWithShadow(this Graphics graphics, Rectangle rectangle, int cornerRadius, int shadowLayers, Color? backColor = null, Color? shadow = null, bool addOutline = false)
	{
		var shadowColor = shadow ?? (FormDesign.Design.IsDarkTheme ? Color.FromArgb(2, 255, 255, 255) : Color.FromArgb(8, FormDesign.Design.AccentColor));

		for (var i = shadowLayers; i > 0; i--)
		{
			if (backColor?.A is null or 255)
			{
				using var pen = new Pen(Color.FromArgb(GetShadowIntensity(i, shadowLayers, shadowColor.A), shadowColor), shadowLayers) { Alignment = PenAlignment.Inset };
				graphics.DrawRoundedRectangle(pen, rectangle.Pad(-i), cornerRadius + i);
			}
			else
			{
				using var brush = new SolidBrush(Color.FromArgb(GetShadowIntensity(i, shadowLayers, shadowColor.A), shadowColor));
				graphics.FillRoundedRectangle(brush, rectangle.Pad(-i), cornerRadius + i);
			}
		}

		using var brushBack = new SolidBrush(backColor ?? FormDesign.Design.BackColor);
		graphics.FillRoundedRectangle(brushBack, rectangle, cornerRadius);

		if (addOutline)
		{
			using var pen = new Pen(Color.FromArgb(255, shadow ?? FormDesign.Design.AccentColor), UI.Scale(1.5f)) { Alignment = PenAlignment.Center };
			graphics.DrawRoundedRectangle(pen, rectangle, cornerRadius);
		}
	}

	private static byte GetShadowIntensity(double x, double layers, byte alpha)
	{
		var transformedX = x / (layers + 1);
		var result = (((1 / (2 * transformedX)) - (transformedX / 2)) * alpha * 2) + 1;

		if (result >= 125)
		{
			return 125;
		}

		return (byte)result;
	}

	public static void FillRoundShadow(this Graphics graphics, Rectangle rectangle, Color? shadow = null)
	{
		var shadowColor = shadow ?? (FormDesign.Design.IsDarkTheme ? Color.FromArgb(50, 255, 255, 255) : Color.FromArgb(75, FormDesign.Design.AccentColor));

		using var ellipsePath = new GraphicsPath();
		ellipsePath.AddEllipse(rectangle);

		using var brush = new PathGradientBrush(ellipsePath);

		brush.CenterPoint = new PointF(rectangle.X + (rectangle.Width / 2f), rectangle.Y + (rectangle.Height / 2f));
		brush.CenterColor = shadowColor;
		brush.SurroundColors = [shadowColor, default];
		brush.FocusScales = new PointF(0, 0);
		brush.InterpolationColors = new ColorBlend(3)
		{
			Colors = [shadowColor, shadowColor, default],
			Positions = [0, 0.85f, 1]
		};

		graphics.FillRectangle(brush, rectangle);
	}

	public static Color BackColor(this BannerStyle style)
	{
		return style switch
		{
			BannerStyle.Active => FormDesign.Design.ActiveForeColor,
			BannerStyle.Text => FormDesign.Design.MenuColor,
			_ => !FormDesign.Design.IsDarkTheme ? FormDesign.Design.ForeColor : FormDesign.Design.MenuColor,
		};
	}

	public static Color ForeColor(this BannerStyle style)
	{
		return style switch
		{
			BannerStyle.Active => FormDesign.Design.ActiveColor,
			BannerStyle.Green => FormDesign.Design.GreenColor,
			BannerStyle.Yellow => FormDesign.Design.YellowColor,
			BannerStyle.Red => FormDesign.Design.RedColor,
			BannerStyle.Text => FormDesign.Design.MenuForeColor,
			_ => FormDesign.Design.ActiveColor,
		};
	}
}