using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickAdvancedImageControl : SlickImageControl, IAnimatable
	{
		protected int yIndex;

		public bool EnableDots { get; set; }
		public ImageSizeMode ImageSizeMode { get; set; }
		public virtual Rectangle ImageBounds { get; protected set; }
		protected virtual IEnumerable<Bitmap> HoverIcons { get; }
		protected virtual IEnumerable<Banner> Banners { get; }
		protected virtual Bitmap DefaultImage { get; set; }
		public virtual Rectangle DotsBounds => new Rectangle(Width - 20, Width - ImageBounds.Width < 20 ? ImageBounds.Top + ImageBounds.Height + 4 : 4, 16, 16);

		protected virtual bool ImageHovered => HoverState.HasFlag(HoverState.Focused) || (HoverState.HasFlag(HoverState.Hovered) && ImageBounds.Contains(PointToClient(Cursor.Position)));
		protected virtual bool DotsHovered => HoverState.HasFlag(HoverState.Hovered) && DotsBounds.Contains(PointToClient(Cursor.Position));

		public static bool AlwaysShowBanners { get; set; }
		public Image BlurredImage { get; protected set; }
		public int AnimatedValue { get; set; }
		public int TargetAnimationValue { get; set; }

		protected virtual void OnImageMouseClick(MouseEventArgs e)
		{ }

		protected virtual void OnDotsMouseClick(MouseEventArgs e)
		{ }

		protected virtual void OnDraw(PaintEventArgs e)
		{ }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				BlurredImage?.Dispose();

			base.Dispose(disposing);
		}

		protected override void OnHoverStateChanged()
		{
			base.OnHoverStateChanged();

			TargetAnimationValue = HoverState.HasFlag(HoverState.Hovered) && ImageBounds.Contains(PointToClient(Cursor.Position))
				? 100
				: 00;

			if (TargetAnimationValue != AnimatedValue)
				AnimationHandler.Animate(this, TargetAnimationValue.If(0, 0.7, 1.35));
		}

		protected override void OnImageChanged(EventArgs e)
		{
			if (Image == null)
			{
				BlurredImage?.Dispose();
				BlurredImage = null;
			}
			else
				new BackgroundAction(() =>
				{
					BlurredImage = new Bitmap(Image).Blur(40);

					if (HoverState.HasFlag(HoverState.Hovered))
						Invalidate();
				}).Run();

			base.OnImageChanged(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Apps)
			{
				OnMouseClick(new MouseEventArgs(MouseButtons.Right, 1, ImageBounds.X + ImageBounds.Width - 1, ImageBounds.Y, 0));
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Cursor = Enabled && (ImageBounds.Contains(e.Location) || (EnableDots && DotsBounds.Contains(e.Location)))
				? Cursors.Hand
				: Cursors.Default;

			base.OnMouseMove(e);

			if (AutoInvalidate)
				Invalidate();

			TargetAnimationValue = HoverState.HasFlag(HoverState.Hovered) && ImageBounds.Contains(PointToClient(Cursor.Position))
				? 100
				: 00;

			if (TargetAnimationValue != AnimatedValue)
				AnimationHandler.Animate(this, TargetAnimationValue.If(0, 0.7, 1.35));
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.None || ImageBounds.Contains(e.Location))
				OnImageMouseClick(e);

			if (DotsBounds.Contains(e.Location))
				OnDotsMouseClick(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (DesignMode) return;

			e.Graphics.Clear(BackColor);

			try
			{
				if (Image != null || Loading)
				{
					if (AnimatedValue < 100 || BlurredImage == null)
						e.Graphics.DrawBorderedImage(Image, ImageBounds, ImageSizeMode);
					
					if (AnimatedValue > 0 && BlurredImage != null)
						e.Graphics.DrawBorderedImage(new Bitmap(BlurredImage).Alpha((int)(255 * AnimatedValue / 100D)), ImageBounds, ImageSizeMode, null, AnimatedValue < 100);
				}
				else
					e.Graphics.DrawBorderedImage(DefaultImage.Color(ImageHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), ImageBounds, ImageSizeMode.Center);

				if (Loading)
				{
					var loaderSize = Math.Min(Math.Min(ImageBounds.Width, ImageBounds.Height) / 2, 32);
					DrawLoader(e.Graphics, new Rectangle(ImageBounds.Center(loaderSize, loaderSize), new Size(loaderSize, loaderSize)));
				}

				if (ImageHovered || AnimatedValue > 0)
					e.Graphics.DrawIconsOverImage(ImageBounds, PointToClient(MousePosition), AnimatedValue / 100D, HoverIcons.ToArray());

				if (Enabled && EnableDots)
					e.Graphics.DrawImage(
						(Width - ImageBounds.Width < 20 ? IconManager.GetSmallIcon("I_More") : IconManager.GetSmallIcon("I_VerticalMore"))
							.Color(DotsHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor),
						DotsBounds);

				yIndex = Width - ImageBounds.Width < 20 ? ImageBounds.Top + ImageBounds.Height + 4 : 2;

				OnDraw(e);

				DrawFocus(e.Graphics, ImageBounds, 0);

				if (AlwaysShowBanners || ImageHovered || AnimatedValue > 0)
					e.Graphics.DrawBannersOverImage(this, ImageBounds, Banners, 7F, HoverState.HasFlag(HoverState.Focused) ? 1 : (AnimatedValue / 100D));
			}
			catch { }
		}

		protected virtual void DrawTextOnImage(PaintEventArgs e, string text, bool leftAlign, int pad = 0)
		{
			if (ImageHovered || AnimatedValue > 0)
				using (var brush = new SolidBrush(Color.FromArgb(HoverState.HasFlag(HoverState.Focused) ? 255 : (int)(255 * AnimatedValue / 100D), ForeColor)))
				using (var font = UI.Font(6.75F, FontStyle.Bold))
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
						extra = 3;
					else if (font.Size == fSize)
						extra = 2;

					var rect = new Rectangle(
								left + ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 3),
								yIndex,
								Width - rigthPad - ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 6),
								(int)Math.Ceiling(fill ? extra + font.GetHeight().ClosestMultipleTo(Height - yIndex - 6 - bottomPad) : e.Graphics.Measure(text, font).Height));

					using (var brush = new SolidBrush(color))
						e.Graphics.DrawString(
							text,
							font,
							brush,
							rect,
							new StringFormat() { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = bottom ? StringAlignment.Far : StringAlignment.Near });

					if (fill)
						yIndex += extra + (int)e.Graphics.Measure(text, font, Width - rigthPad - ((Width - ImageBounds.Width < 20) ? 0 : ImageBounds.Left + ImageBounds.Width + 6)).Height;
					else
						yIndex += extra + font.Height;

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
						extra = 3;
					else if (font.Size == fSize)
						extra = 2;

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
				return DrawIcon(e, icon, new Rectangle(point, icon.Size), hovered, sizeMode);
			return false;
		}

		protected bool DrawIcon(PaintEventArgs e, Bitmap icon, Rectangle rectangle, bool hovered = false, ImageSizeMode sizeMode = ImageSizeMode.Center)
		{
			if (icon != null)
			{
				var color = FormDesign.Design.IconColor;

				if (hovered)
					color = FormDesign.Design.ActiveColor;
				else if (hovered = rectangle.Contains(PointToClient(Cursor.Position)))
					color = HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ActiveColor;

				e.Graphics.DrawImage(icon.Color(color), rectangle, sizeMode);

				return hovered;
			}

			return false;
		}
	}
}