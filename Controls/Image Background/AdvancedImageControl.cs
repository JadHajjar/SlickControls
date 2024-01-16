using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SlickAdvancedImageImageBackgroundControl : SlickImageBackgroundControl
{
	protected int yIndex;

	public bool EnableDots { get; set; }
	public Image BlurredImage { get; protected set; }
	public virtual Rectangle ImageBounds { get; protected set; }
	protected virtual IEnumerable<Bitmap> HoverIcons { get; }
	protected virtual IEnumerable<Banner> Banners { get; }
	public virtual Rectangle DotsBounds => new(Bounds.Width - 20, Bounds.Width - ImageBounds.Width < 20 ? ImageBounds.Top + ImageBounds.Height + 4 : 4, 16, 16);

	protected virtual bool ImageHovered => HoverState.HasFlag(HoverState.Focused) || (HoverState.HasFlag(HoverState.Hovered) && ImageBounds.Contains(PointToClient(Cursor.Position)));
	protected virtual bool DotsHovered => HoverState.HasFlag(HoverState.Hovered) && DotsBounds.Contains(PointToClient(Cursor.Position));

	protected override void OnImageChanged(EventArgs e)
	{
		if (Image == null)
		{
			BlurredImage?.Dispose();
			BlurredImage = null;
		}
		else
		{
			new BackgroundAction("Loading Image", () =>
			{
				BlurredImage = Image.Blur(40, true);

				if (HoverState.HasFlag(HoverState.Hovered))
				{
					Invalidate();
				}
			}).Run();
		}

		base.OnImageChanged(e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			BlurredImage?.Dispose();
		}

		base.Dispose(disposing);
	}

	protected virtual void OnImageMouseClick(MouseEventArgs e)
	{ }

	protected virtual void OnDotsMouseClick(MouseEventArgs e)
	{ }

	protected virtual void OnDraw(PaintEventArgs e)
	{ }

	protected override void OnMouseMove(MouseEventArgs e)
	{
		Cursor = Enabled && (ImageBounds.Contains(e.Location) || (EnableDots && DotsBounds.Contains(e.Location)))
			? Cursors.Hand
			: Cursors.Default;

		base.OnMouseMove(e);
	}

	public override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.None || ImageBounds.Contains(e.Location))
		{
			OnImageMouseClick(e);
		}

		if (DotsBounds.Contains(e.Location))
		{
			OnDotsMouseClick(e);
		}
	}

	public override void OnPaint(PaintEventArgs e)
	{
		try
		{
			if (Image != null || Loading)
			{
				e.Graphics.DrawBorderedImage(HoverState.HasFlag(HoverState.Hovered) && ImageBounds.Contains(PointToClient(Cursor.Position)) ? BlurredImage ?? Image : Image, ImageBounds, ImageSizeMode, FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.ForeColor, 65));
			}
			else
			{
				e.Graphics.DrawBorderedImage(DefaultImage?.Color(ImageHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), ImageBounds, ImageSizeMode.Center, FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.ForeColor, 65));
			}

			if (Loading)
			{
				var loaderSize = Math.Min(Math.Min(ImageBounds.Width, ImageBounds.Height) / 2, 32);
				e.Graphics.DrawLoader(LoaderPercentage, new Rectangle(ImageBounds.Center(loaderSize, loaderSize), new Size(loaderSize, loaderSize)));
			}

			if (ImageHovered)
			{
				e.Graphics.DrawIconsOverImage(ImageBounds, PointToClient(Cursor.Position), HoverIcons.ToArray());
			}

			if (Enabled && EnableDots)
			{
				e.Graphics.DrawImage(
					(Width - ImageBounds.Width < 20 ? IconManager.GetSmallIcon("I_More") : IconManager.GetSmallIcon("I_VerticalMore"))
						.Color(DotsHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor),
					DotsBounds);
			}

			yIndex = Width - ImageBounds.Width < 20 ? ImageBounds.Top + ImageBounds.Height + 4 : 2;

			OnDraw(e);

			if (SlickAdvancedImageControl.AlwaysShowBanners || ImageHovered)
			{
				e.Graphics.DrawBannersOverImage(PointToClient(Cursor.Position), ImageBounds, Banners);
			}
		}
		catch { }
	}

	protected virtual void DrawTextOnImage(PaintEventArgs e, string text, bool leftAlign, int pad = 0)
	{
		if (ImageHovered)
		{
			using var brush = new SolidBrush(FormDesign.Design.ForeColor);
			using var font = UI.Font(6.75F, FontStyle.Bold);
			e.Graphics.DrawString(
				text,
				font,
				brush,
				ImageBounds.Pad(3, 0, 3, 2 + pad),
				new StringFormat
				{
					Alignment = leftAlign
						? StringAlignment.Near
						: StringAlignment.Far,
					LineAlignment = StringAlignment.Far
				});
		}
	}

	protected virtual Rectangle DrawText(PaintEventArgs e, string text, Font font, Color color, int left = 3, int rigthPad = 9, bool fill = false, bool bottom = false, int bottomPad = 0)
	{
		using (font)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				fill |= bottom;

				var extra = 1;
				var fSize = (float)(UI.FontScale * 8.25F / UI.WindowsScale).RoundToMultipleOf(0.75F);

				if (font.Size < fSize)
				{
					extra = 3;
				}
				else if (font.Size == fSize)
				{
					extra = 2;
				}

				var rect = new Rectangle(
							left + ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 3),
							yIndex,
							Width - rigthPad - ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 6),
							(int)Math.Ceiling(fill ? extra + font.GetHeight().ClosestMultipleTo(Height - yIndex - 6 - bottomPad) : e.Graphics.Measure(text, font).Height));

				using (var brush = new SolidBrush(color))
				{
					e.Graphics.DrawString(
						text,
						font,
						brush,
						rect,
						new StringFormat() { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = bottom ? StringAlignment.Far : StringAlignment.Near });
				}

				if (fill)
				{
					yIndex += extra + (int)e.Graphics.Measure(text, font, Width - rigthPad - ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 6)).Height;
				}
				else
				{
					yIndex += extra + font.Height;
				}

				var bnds = e.Graphics.Measure(text, font, rect.Width);

				return new Rectangle(
					left + ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 3),
					yIndex,
					Math.Min(rect.Width, (int)bnds.Width),
					Math.Min(rect.Height, (int)bnds.Height));
			}
		}

		return Rectangle.Empty;
	}

	protected virtual Rectangle MeasureText(PaintEventArgs e, string text, Font font, int left = 3, int rigthPad = 9, bool fill = false, bool bottom = false, int bottomPad = 0)
	{
		using (font)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				fill |= bottom;

				var extra = 1;
				var fSize = (float)(UI.FontScale * 8.25F / UI.WindowsScale).RoundToMultipleOf(0.75F);

				if (font.Size < fSize)
				{
					extra = 3;
				}
				else if (font.Size == fSize)
				{
					extra = 2;
				}

				var size = new Size(Width - rigthPad - ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 6),
							(int)Math.Ceiling(fill ? extra + font.GetHeight().ClosestMultipleTo(Height - yIndex - 6 - bottomPad) : e.Graphics.Measure(text, font).Height));

				var bnds = e.Graphics.Measure(text, font, size.Width);

				return new Rectangle(
					left + ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 3),
					yIndex,
					Math.Min(size.Width, (int)bnds.Width),
					Math.Min(size.Height, (int)bnds.Height));
			}
		}

		return Rectangle.Empty;
	}

	protected bool DrawIcon(PaintEventArgs e, Bitmap icon, Point point, bool hovered = false, ImageSizeMode sizeMode = ImageSizeMode.Center)
	{
		if (icon != null)
		{
			return DrawIcon(e, icon, new Rectangle(point, icon.Size), hovered, sizeMode);
		}

		return false;
	}

	protected bool DrawIcon(PaintEventArgs e, Bitmap icon, Rectangle rectangle, bool hovered = false, ImageSizeMode sizeMode = ImageSizeMode.Center)
	{
		if (icon != null)
		{
			var color = FormDesign.Design.IconColor;

			if (hovered)
			{
				color = FormDesign.Design.ActiveColor;
			}
			else if (hovered = rectangle.Contains(PointToClient(Cursor.Position)))
			{
				color = HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ActiveColor;
			}

			e.Graphics.DrawImage(icon.Color(color), rectangle, sizeMode);

			return hovered;
		}

		return false;
	}
}