using Extensions;

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
		private Size cachedSize;
		private Size lastAvailableSize;
		private bool live;

		public SlickButton()
		{
			AutoSize = true;
			Cursor = Cursors.Hand;
			Size = new Size(100, 30);
			SpaceTriggersClick = true;
		}

		[Category("Appearance"), DisplayName("Color Style"), DefaultValue(ColorStyle.Active)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Active;

		[Category("Appearance"), DisplayName("Handle UI Scale"), DefaultValue(true)]
		public bool HandleUiScale { get; set; } = true;

		[Category("Appearance"), DisplayName("Button Type"), DefaultValue(ButtonType.Normal)]
		public ButtonType ButtonType { get; set; }

		[Category("Appearance"), DisplayName("Auto-hide Text"), DefaultValue(true)]
		public bool AutoHideText { get; set; } = true;

		[Category("Appearance"), DisplayName("Auto-size Icon"), DefaultValue(false)]
		public bool AutoSizeIcon { get; set; }

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

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool AlignLeft { get; set; }

		protected override void OnParentFontChanged(EventArgs e)
		{
			cachedSize = default;

			base.OnParentFontChanged(e);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			cachedSize = default;

			base.OnFontChanged(e);
		}

		protected override void UIChanged()
		{
			cachedSize = default;

			if (Live && Padding == Padding.Empty)
			{
				Padding = UI.Scale(new Padding(4), UI.UIScale);
			}

			if (live && HandleUiScale)
			{
				PerformLayout();
			}
		}

		protected override void LocaleChanged()
		{
			if (live)
			{
				cachedSize = default;

				PerformLayout();
				Invalidate();
			}
		}

		protected override void OnImageChanged(EventArgs e)
		{
			cachedSize = default;

			base.OnImageChanged(e);

			PerformLayout();
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			return GetAutoSize();
		}

		protected override void OnPaddingChanged(EventArgs e)
		{
			cachedSize = default;

			base.OnPaddingChanged(e);
		}

		protected override void OnMarginChanged(EventArgs e)
		{
			cachedSize = default;

			base.OnMarginChanged(e);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			cachedSize = default;

			base.OnTextChanged(e);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			live = true;

			PerformLayout();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (live && cachedSize != default && cachedSize != Size)
			{
				cachedSize = default;
			}
		}

		public Size GetAutoSize(bool forced = false)
		{
			if (!live || Anchor == (AnchorStyles)15 || Dock == DockStyle.Fill)
			{
				return Size;
			}

			var availableSize = GetAvailableSize();

			if (!forced && cachedSize != default && lastAvailableSize == availableSize)
			{
				return cachedSize;
			}

			using (var image = Image)
			{
				lastAvailableSize = availableSize;

				var IconSize = image?.Width ?? 16;

				if (string.IsNullOrWhiteSpace(Text) || (AutoSize && availableSize.Width.IsWithin(0, (int)(64 * UI.FontScale))))
				{
					if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
					{
						return cachedSize = new Size(Height, Height);
					}
					else if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
					{
						return cachedSize = new Size(Width, Width);
					}
					else
					{
						var pad = Math.Max(Padding.Horizontal, Padding.Vertical);

						return cachedSize = new Size(IconSize + pad, IconSize + pad);
					}
				}

				var extraWidthForIcon = (image == null ? 0 : (image.Width + Padding.Left)) + (int)(2 * UI.FontScale);
				var bnds = FontMeasuring.Measure(LocaleHelper.GetGlobalText(Text), Font, availableSize.Width - extraWidthForIcon - Padding.Horizontal - 2);
				var h = Math.Max(IconSize + (int)(4 * UI.FontScale) + 2, (int)bnds.Height + Padding.Horizontal + 2);
				var w = (int)Math.Ceiling(bnds.Width) + extraWidthForIcon + Padding.Horizontal + 2;

				if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
				{
					h = Height;
				}

				if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
				{
					w = Width;
				}

				if (w > availableSize.Width)
				{
					w = availableSize.Width;
				}

				if (h > availableSize.Height)
				{
					h = availableSize.Height;
				}

				w = w.Between(MinimumSize.Width, MaximumSize.Width.If(0, w));
				h = h.Between(MinimumSize.Height, MaximumSize.Height.If(0, h));

				return cachedSize = new Size(w, h);
			}
		}

		public static Size GetSize(Graphics g, Image image, string text, Font font, Padding? padding = null, int maxWidth = int.MaxValue)
		{
			var iconSize = image?.Width ?? 16;

			padding = padding ?? UI.Scale(new Padding(4), UI.UIScale);

			if (string.IsNullOrWhiteSpace(text))
			{
				var pad = Math.Max(padding.Value.Horizontal, padding.Value.Vertical);

				return new Size(iconSize + pad, iconSize + pad);
			}

			var extraWidthForIcon = (image == null ? 0 : (image.Width + padding.Value.Left)) + (int)(2 * UI.FontScale);
			var bnds = g.Measure(LocaleHelper.GetGlobalText(text), font, maxWidth - extraWidthForIcon - padding.Value.Horizontal - 2);
			var h = Math.Max(iconSize + (int)(4 * UI.FontScale) + 2, (int)bnds.Height + padding.Value.Horizontal + 2);
			var w = (int)Math.Ceiling(bnds.Width) + extraWidthForIcon + padding.Value.Horizontal + 2;

			return new Size(w, h);
		}

		protected override void DesignChanged(FormDesign design)
		{
			Invalidate();
		}

		public static void GetColors(out Color fore, out Color back, HoverState HoverState, ColorStyle ColorStyle = ColorStyle.Active, Color? ColorShade = null, Color? clearColor = null, Color? BackColor = null, ButtonType buttonType = ButtonType.Normal)
		{
			if (buttonType == ButtonType.Active)
			{
				if (HoverState.HasFlag(HoverState.Pressed))
				{
					HoverState &= HoverState.Focused;
				}
				else
				{
					HoverState |= HoverState.Pressed;
				}
			}

			if (HoverState.HasFlag(HoverState.Pressed) || (buttonType == ButtonType.Hidden && HoverState.HasFlag(HoverState.Hovered)))
			{
				fore = ColorStyle.GetBackColor().Tint(ColorShade?.GetHue());
				back = ColorShade == null ? ColorStyle.GetColor() : ColorStyle.GetColor().Tint(ColorShade?.GetHue()).MergeColor((Color)ColorShade);

				if ((buttonType == ButtonType.Active && HoverState.HasFlag(HoverState.Hovered)) || !HoverState.HasFlag(HoverState.Pressed))
				{
					back = Color.FromArgb(150, back);
				}
			}
			else if (HoverState.HasFlag(HoverState.Hovered))
			{
				fore = FormDesign.Design.ButtonForeColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Light ? -7 : 7);
				back = FormDesign.Design.ButtonColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Light ? -7 : 7);
			}
			else
			{
				fore = FormDesign.Design.ButtonForeColor;
				back = buttonType == ButtonType.Hidden ? default : (clearColor == null || BackColor == null || (Color)clearColor == (Color)BackColor) ? FormDesign.Design.ButtonColor : (Color)BackColor;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

			using (var img = AutoSizeIcon ? ImageName.Get(Height - Padding.Vertical) : Image)
			{
				GetColors(out var fore, out var back, HoverState, ColorStyle, colorShade, Parent?.BackColor ?? BackColor, BackColor, ButtonType);

				DrawButton(e,
					Point.Empty,
					Size,
					LocaleHelper.GetGlobalText(Text),
					Font,
					back,
					fore,
					img,
					Padding,
					Enabled,
					HoverState,
					ColorStyle,
					ColorShade,
					this);
			}
		}

		public static void DrawButton(
			PaintEventArgs e,
			Rectangle rectangle,
			string text,
			Font font,
			Image icon,
			Padding? padding = null,
			HoverState hoverState = HoverState.Normal,
			ColorStyle colorStyle = ColorStyle.Active,
			Color backColor = default,
			ButtonType buttonType = ButtonType.Normal)
		{
			GetColors(out var fore, out var back, hoverState, colorStyle, null, Color.Empty, backColor, buttonType);

			DrawButton(e, rectangle.Location, rectangle.Size, text, font, back, fore, icon, padding ?? UI.Scale(new Padding(7), UI.UIScale), true, hoverState, colorStyle);
		}

		public static void DrawButton(
			PaintEventArgs e,
			Point location,
			Size size,
			string text,
			Font font,
			Color back,
			Color fore,
			Image image,
			Padding padding,
			bool enabled = true,
			HoverState hoverState = HoverState.Normal,
			ColorStyle colorStyle = ColorStyle.Active,
			Color? colorShade = null,
			SlickButton slickButton = null)
		{
			Draw(e, new ButtonDrawArgs
			{
				Control = slickButton,
				Location = location,
				Size = size,
				Text = text,
				Font = font,
				ColorShade = colorShade,
				BackColor = back,
				ColorStyle = colorStyle,
				Enabled = enabled,
				ForeColor = fore,
				HoverState = hoverState,
				Image = image,
				Padding = padding,
			});
		}

		public static void Draw(PaintEventArgs e, ButtonDrawArgs buttonArgs)
		{
			if (buttonArgs.ForeColor.A == 0 && buttonArgs.BackColor.A == 0)
			{
				GetColors(out var fore, out var back, buttonArgs.HoverState, buttonArgs.ColorStyle, buttonArgs.ColorShade, null, buttonArgs.BackColor, buttonArgs.ButtonType);

				buttonArgs.ForeColor = fore;
				buttonArgs.BackColor = back;
			}

			if (!buttonArgs.Enabled)
			{
				buttonArgs.ForeColor = buttonArgs.ForeColor.MergeColor(FormDesign.Design.BackColor, 50);
				buttonArgs.BackColor = Color.FromArgb(100, buttonArgs.BackColor);
			}

			var rect = (buttonArgs.Rectangle == default ? new Rectangle(buttonArgs.Location, buttonArgs.Size) : buttonArgs.Rectangle).Pad(1);

			using (var brush = Gradient(rect, buttonArgs.BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, rect, (int)(4 * UI.FontScale));
			}

			if (!buttonArgs.HoverState.HasFlag(HoverState.Pressed))
			{
				DrawFocus(e.Graphics, rect, buttonArgs.HoverState, (int)(4 * UI.FontScale), buttonArgs.ColorShade == null ? buttonArgs.ColorStyle.GetColor() : buttonArgs.ColorStyle.GetColor().Tint(buttonArgs.ColorShade?.GetHue()).MergeColor((Color)buttonArgs.ColorShade));
			}

			if (buttonArgs.Icon != null && buttonArgs.Image == null)
			{
				buttonArgs.Image = buttonArgs.Icon;
			}

			if (buttonArgs.Padding == default)
			{
				buttonArgs.Padding = UI.Scale(new Padding(4), UI.UIScale);
			}

			rect = rect.Pad(buttonArgs.Padding);

			var extraWidthForIcon = (buttonArgs.Image == null ? 0 : (buttonArgs.Image.Width + buttonArgs.Padding.Left)) + (int)(2 * UI.FontScale);
			var noText = string.IsNullOrWhiteSpace(buttonArgs.Text) || (((buttonArgs.Control as SlickButton)?.AutoHideText ?? false) && rect.Width.IsWithin(0, (int)(50 * UI.FontScale)) && buttonArgs.Text.Length > 4);
			var iconRect = rect.Align(buttonArgs.Image?.Size ?? UI.Scale(new Size(16, 16), UI.FontScale), ContentAlignment.MiddleLeft);
			//var bnds = e.Graphics.Measure(buttonArgs.Text, buttonArgs.Font, rect.Width - extraWidthForIcon - buttonArgs.Padding.Horizontal);

			try
			{
				if (buttonArgs.Control?.Loading ?? false)
				{
					var color = buttonArgs.ColorShade == null ? buttonArgs.ColorStyle.GetColor() : buttonArgs.ColorStyle.GetColor().Tint(buttonArgs.ColorShade?.GetHue()).MergeColor((Color)buttonArgs.ColorShade);

					if (color == buttonArgs.BackColor)
					{
						color = buttonArgs.ForeColor;
					}

					if (noText)
					{
						buttonArgs.Control.DrawLoader(e.Graphics, iconRect, color);
					}
					else
					{
						buttonArgs.Control.DrawLoader(e.Graphics, iconRect, color);
					}
				}
				else if (buttonArgs.Image != null)
				{
					if (noText)
					{
						e.Graphics.DrawImage(buttonArgs.Image.Color(buttonArgs.ForeColor), iconRect);
					}
					else
					{
						e.Graphics.DrawImage(buttonArgs.Image.Color(buttonArgs.ForeColor), iconRect);
					}
				}
			}
			catch { }

			if (noText)
			{
				return;
			}

			var textRect = rect.Pad(extraWidthForIcon, 0, 0, 0).AlignToFontSize(buttonArgs.Font, ContentAlignment.MiddleCenter, e.Graphics, true);

			//if (textRect.Height < bnds.Height)
			//{
			//	textRect.Y -= ((int)bnds.Height - textRect.Height + 2) / 2;
			//	textRect.Height = 3 + (int)bnds.Height;
			//}

			using (var stl = new StringFormat()
			{
				Alignment = buttonArgs.Image == null && buttonArgs.Control is SlickButton button && button.AlignLeft ? StringAlignment.Near : StringAlignment.Center,
				LineAlignment = StringAlignment.Center,
				Trimming = StringTrimming.EllipsisCharacter
			})
			{
				e.Graphics.DrawString(buttonArgs.Text, buttonArgs.Font, new SolidBrush(buttonArgs.ForeColor), textRect, stl);
			}

			if (buttonArgs.Icon != null)
			{
				buttonArgs.Image?.Dispose();
			}
		}

		public new void OnClick(EventArgs e)
		{
			base.OnClick(e);
		}
	}

	public class ButtonDrawArgs
	{
		public Rectangle Rectangle { get; set; }
		public Point Location { get; set; }
		public Size Size { get; set; }
		public string Text { get; set; }
		public Font Font { get; set; }
		public Color BackColor { get; set; }
		public Color ForeColor { get; set; }
		public DynamicIcon Icon { get; set; }
		public Image Image { get; set; }
		public Padding Padding { get; set; }
		public bool Enabled { get; set; } = true;
		public HoverState HoverState { get; set; } = HoverState.Normal;
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Active;
		public Color? ColorShade { get; set; }
		public ILoaderControl Control { get; set; }
		public ButtonType ButtonType { get; set; }
	}
}