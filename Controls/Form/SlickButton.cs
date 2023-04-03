﻿using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("Click")]
	public partial class SlickButton : SlickImageControl
	{
		private Color? colorShade = null;

		public SlickButton()
		{
			Cursor = Cursors.Hand;
			Size = new Size(100, 30);
			SpaceTriggersClick = true;
		}

		[Category("Appearance"), DisplayName("Color Style"), DefaultValue(ColorStyle.Active)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Active;

		[Category("Appearance"), DisplayName("Handle UI Scale"), DefaultValue(true)]
		public bool HandleUiScale { get; set; } = true;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public new bool TabStop { get; set; } = true;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override Font Font { get => base.Font; set => base.Font = value; }

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		[Category("Appearance")]
		public Color? ColorShade { get => colorShade; set { colorShade = value; Invalidate(); } }

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			UIChanged();
		}

		protected override void OnParentFontChanged(EventArgs e)
		{
			base.OnParentFontChanged(e);
			UIChanged();
			Invalidate();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			UIChanged();
			Invalidate();
		}

		protected override void UIChanged()
		{
			if (Live && HandleUiScale)
				PerformAutoSize();

			if (Live && Padding == Padding.Empty)
				Padding = UI.Scale(new Padding(7), UI.UIScale);
		}

		protected override void OnImageChanged(EventArgs e)
		{
			base.OnImageChanged(e);

			PerformAutoSize();
		}

		public void PerformAutoSize()
		{	
			if (Anchor == (AnchorStyles)15 || Dock == DockStyle.Fill || (string.IsNullOrWhiteSpace(Text) && Image == null))
				return;

			using (var g = Graphics.FromHwnd(IntPtr.Zero))
			{
				var IconSize = Image?.Width ?? 16;

				if (string.IsNullOrWhiteSpace(Text))
				{
					if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
					{
						Width = Height;
					}
					else if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
					{
						Height = Width;
					}
					else
					{
						var pad = Math.Max(Padding.Horizontal, Padding.Vertical);

						Size = new Size(IconSize + pad, IconSize + pad);
					}

					return;
				}

				var bnds = g.Measure(LocaleHelper.GetGlobalText(Text), Font);
				var h = Math.Max(IconSize + 6, (int)(bnds.Height) + Padding.Top + 3);
				var w = (int)bnds.Width + (Image == null ? 0 : IconSize + Padding.Left) + Padding.Horizontal + 3;

				if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
					h = Height;

				if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
					w = Width;

				Size = new Size(w, h);
			}
		}

		public static Size GetSize(Graphics g, Image image, string text, Font font, Padding? padding = null)
		{
			var iconSize = image?.Width ?? 16;

			padding = padding ?? UI.Scale(new Padding(7), UI.UIScale);

			if (string.IsNullOrWhiteSpace(text))
			{
				var pad = Math.Max(padding.Value.Horizontal, padding.Value.Vertical);
				
				return new Size(iconSize + pad, iconSize + pad);
			}

			var bnds = g.Measure(LocaleHelper.GetGlobalText(text), font);
			var h = Math.Max(iconSize + 6, (int)(bnds.Height) + padding.Value.Top + 3);
			var w = (int)bnds.Width + (image == null ? 0 : iconSize + padding.Value.Left) + padding.Value.Horizontal + 3;

			return new Size(w, h);
		}

		protected override void DesignChanged(FormDesign design) => Invalidate();

		public static void GetColors(out Color fore, out Color back, HoverState HoverState, ColorStyle ColorStyle = ColorStyle.Active, Color? ColorShade = null, Color? clearColor = null, Color? BackColor = null, bool Enabled = true)
		{
			if (HoverState.HasFlag(HoverState.Pressed))
			{
				fore = ColorStyle.GetBackColor().Tint(ColorShade?.GetHue());
				back = ColorShade == null ? ColorStyle.GetColor() : ColorStyle.GetColor().Tint(ColorShade?.GetHue()).MergeColor((Color)ColorShade);
			}
			else if (HoverState.HasFlag(HoverState.Hovered))
			{
				fore = FormDesign.Design.ButtonForeColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Light ? -7 : 7);
				back = FormDesign.Design.ButtonColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Light ? -7 : 7);
			}
			else
			{
				fore = Enabled ? FormDesign.Design.ButtonForeColor : FormDesign.Design.ButtonForeColor.MergeColor(FormDesign.Design.ButtonColor);
				back = (clearColor == null || BackColor == null || (Color)clearColor == (Color)BackColor) ? FormDesign.Design.ButtonColor : (Color)BackColor;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			Bitmap img = null;
			try
			{ img = Image == null ? null : new Bitmap(Image); }
			catch { }

			using (img)
			{
				DrawButton(e,
					Point.Empty,
					Size,
					LocaleHelper.GetGlobalText(Text),
					Font,
					Parent?.BackColor ?? BackColor,
					BackColor,
					img,
					Padding,
					Enabled,
					HoverState,
					ColorStyle,
					ColorShade,
					this);
			}
		}

		public static void DrawButton(PaintEventArgs e,
			Rectangle rectangle,
			string text,
			Font font,
			Image icon,
			Padding? padding = null,
			HoverState HoverState = HoverState.Normal,
			ColorStyle ColorStyle = ColorStyle.Active)
			=> DrawButton(e, rectangle.Location, rectangle.Size, text, font, Color.Empty, Color.Empty, icon, padding ?? UI.Scale(new Padding(7), UI.UIScale), true, HoverState, ColorStyle);

		public static void DrawButton(PaintEventArgs e,
								Point location,
								Size size,
								string Text,
								Font Font,
								Color clearColor,
								Color BackColor,
								Image Image,
								Padding Padding,
								bool Enabled = true,
								HoverState HoverState = HoverState.Normal,
								ColorStyle ColorStyle = ColorStyle.Active,
								Color? ColorShade = null,
								SlickControl slickButton = null)
		{
			GetColors(out var fore, out var back, HoverState, ColorStyle, ColorShade, clearColor, BackColor, Enabled);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			e.Graphics.FillRoundedRectangle(Gradient(new Rectangle(1 + location.X, 1 + location.Y, size.Width - 3, size.Height - 3), back), new Rectangle(1 + location.X, 1 + location.Y, size.Width - 3, size.Height - 3), 5);

			if (!HoverState.HasFlag(HoverState.Pressed))
				DrawFocus(e.Graphics, new Rectangle(1 + location.X, 1 + location.Y, size.Width - 3, size.Height - 3), HoverState, 5, ColorShade == null ? ColorStyle.GetColor() : ColorStyle.GetColor().Tint(ColorShade?.GetHue()).MergeColor((Color)ColorShade));

			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var iconSize = Image?.Width ?? 16;
			var bnds = e.Graphics.Measure(Text, Font);

			try
			{

				if (slickButton?.Loading ?? false)
				{
					if (string.IsNullOrWhiteSpace(Text))
						slickButton.DrawLoader(e.Graphics, new Rectangle(location.X + (size.Width - iconSize) / 2, location.Y + (size.Height - iconSize) / 2, iconSize, iconSize), HoverState.HasFlag(HoverState.Pressed) ? (Color?)fore : ColorShade == null ? ColorStyle.GetColor() : ColorStyle.GetColor().Tint(ColorShade?.GetHue()).MergeColor((Color)ColorShade));
					else
						slickButton.DrawLoader(e.Graphics, new Rectangle(location.X + Padding.Left, location.Y + (size.Height - iconSize) / 2, iconSize, iconSize), HoverState.HasFlag(HoverState.Pressed) ? (Color?)fore : ColorShade == null ? ColorStyle.GetColor() : ColorStyle.GetColor().Tint(ColorShade?.GetHue()).MergeColor((Color)ColorShade));
				}
				else if (Image != null)
				{
					var img = slickButton?.Live ?? false ? Image.SafeColor(fore) : Image.Color(fore);

					if (string.IsNullOrWhiteSpace(Text))
						e.Graphics.DrawImage(img, new Rectangle(location.X + (size.Width - iconSize) / 2, location.Y + (size.Height - iconSize) / 2, iconSize, iconSize));
					else
						e.Graphics.DrawImage(img, new Rectangle(location.X + Padding.Left, location.Y + (size.Height - iconSize) / 2, iconSize, iconSize));
				}
			}
			catch { }

			var stl = new StringFormat()
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center,
				Trimming = StringTrimming.EllipsisCharacter
			};

			var textRect = new Rectangle(location, size).Pad(Padding).Pad((Image != null || (slickButton?.Loading ?? false)) ? (iconSize + Padding.Left) : 0, 0, 0, 0);

			if (textRect.Height < bnds.Height)
			{
				textRect.Y -= ((int)bnds.Height - textRect.Height + 2) / 2;
				textRect.Height = 3 + (int)bnds.Height;
			}

			e.Graphics.DrawString(Text, Font, new SolidBrush(fore), textRect, stl);
		}

		public new void OnClick(EventArgs e) => base.OnClick(e);
	}
}