using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

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

	[Category("Appearance"), DisplayName("Auto-size Icon"), DefaultValue(false)]
	public bool AutoSizeIcon { get; set; }

	[Category("Appearance"), DisplayName("Match Background Color"), DefaultValue(false)]
	public bool MatchBackgroundColor { get; set; }

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
	public override string Text
	{
		get => base.Text; set
		{
			base.Text = value;
			UIChanged();
		}
	}

	[Category("Appearance"), DefaultValue(null)]
	public Color? ColorShade
	{
		get => colorShade; set
		{
			colorShade = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), DefaultValue(false)]
	public bool AlignLeft { get; set; }

	[Category("Appearance"), DisplayName("Multi-Line"), DefaultValue(false)]
	public bool MultiLine { get; set; }

	[Category("Appearance"), DisplayName("Colored Icon"), DefaultValue(false)]
	public bool ColoredIcon { get; set; }

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

	public new void OnClick(EventArgs e)
	{
		base.OnClick(e);
	}

	public new void PerformAutoScale()
	{
		Size = GetAutoSize(true);
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

		lastAvailableSize = availableSize;

		return cachedSize = CalculateAutoSize(availableSize);
	}

	public virtual Size CalculateAutoSize(Size availableSize)
	{
		using var graphics = Graphics.FromHwnd(IntPtr.Zero);
		using var args = new ButtonDrawArgs
		{
			Control = this,
			Font = Font,
			Icon = ImageName,
			Image = image,
			Text = Text,
			Padding = Padding,
			AvailableSize = MultiLine ? availableSize : default
		};

		if (Dock is DockStyle.Left or DockStyle.Right || Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom))
		{
			args.RequiredHeight = Height;
		}
		else if (Dock is DockStyle.Top or DockStyle.Bottom || Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right))
		{
			args.RequiredWidth = Width;
		}

		PrepareLayout(graphics, args);

		return args.Rectangle.Size;
	}

	protected override void DesignChanged(FormDesign design)
	{
		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		Draw(e.Graphics, new ButtonDrawArgs
		{
			Control = this,
			Font = Font,
			Icon = ImageName,
			Image = image,
			Text = Text,
			Padding = Padding,
			ButtonType = ButtonType,
			ColorShade = ColorShade,
			ColorStyle = ColorStyle,
			Enabled = Enabled,
			HoverState = HoverState,
			ColoredIcon = ColoredIcon,
			Rectangle = ClientRectangle,
			BackgroundColor = MatchBackgroundColor ? BackColor : default
		});
	}

	[Obsolete]
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
		Draw(e, new ButtonDrawArgs
		{
			Rectangle = rectangle,
			Text = text,
			Font = font,
			BackColor = backColor,
			ButtonType = buttonType,
			ColorStyle = colorStyle,
			HoverState = hoverState,
			Image = icon,
			Padding = padding ?? default,
		});
	}

	public static void GetColors(out Color fore, out Color back, HoverState HoverState, ColorStyle ColorStyle = ColorStyle.Active, Color? ColorShade = null, Color? clearColor = null, Color? BackColor = null, ButtonType buttonType = ButtonType.Normal, Color? activeColor = null)
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
			back = ColorShade == null ? (activeColor ?? ColorStyle.GetColor().MergeColor(FormDesign.Design.BackColor, 85)) : ColorStyle.GetColor().Tint(ColorShade?.GetHue()).MergeColor((Color)ColorShade);

			if ((buttonType == ButtonType.Active && HoverState.HasFlag(HoverState.Hovered)) || !HoverState.HasFlag(HoverState.Pressed))
			{
				back = Color.FromArgb(150, back);
			}
		}
		else if (HoverState.HasFlag(HoverState.Hovered))
		{
			fore = FormDesign.Design.ButtonForeColor.Tint(Lum: buttonType == ButtonType.Dimmed ? 0 : !FormDesign.Design.IsDarkTheme ? -7 : 7);
			back = FormDesign.Design.ButtonColor.Tint(Lum: buttonType == ButtonType.Dimmed ? 0 : !FormDesign.Design.IsDarkTheme ? -7 : 7);
		}
		else
		{
			fore = FormDesign.Design.ButtonForeColor;
			back = buttonType == ButtonType.Hidden ? default : (clearColor == null || BackColor == null || (Color)clearColor == (Color)BackColor) ? FormDesign.Design.ButtonColor : (Color)BackColor;
		}
	}

	public static Size GetSize(Graphics graphics, Image image, string text, Font font, Padding? padding = null, int maxWidth = 0, bool isLoading = false, int fixedWidth = 0, int fixedHeight = 0)
	{
		return GetSize(out _, graphics, image, text, font, padding, maxWidth, isLoading, fixedWidth, fixedHeight);
	}

	public static Size GetSize(out Padding textPaddingForAvailableSpace, Graphics graphics, Image image, string text, Font font, Padding? padding = null, int maxWidth = 0, bool isLoading = false, int fixedWidth = 0, int fixedHeight = 0)
	{
		var padding_ = padding ?? UI.Scale(new Padding(4), UI.UIScale);
		var font_ = font ?? UI.Font(8.25F);

		var iconSize = image?.Size ?? new Size(font_.Height * 5 / 4, font_.Height * 5 / 4);

		if (string.IsNullOrWhiteSpace(text))
		{
			textPaddingForAvailableSpace = default;

			if (fixedHeight > 0)
			{
				return new Size(fixedHeight, fixedHeight);
			}

			if (fixedWidth > 0)
			{
				return new Size(fixedWidth, fixedWidth);
			}

			var pad = Math.Max(padding_.Horizontal, padding_.Vertical);

			return new Size(iconSize.Width + pad, iconSize.Height + pad);
		}

		var size = new Size(padding_.Horizontal, padding_.Vertical);

		textPaddingForAvailableSpace = padding_;

		if (isLoading || image != null)
		{
			size.Width += iconSize.Width + padding_.Left;
			size.Height += iconSize.Height;

			textPaddingForAvailableSpace.Left += iconSize.Width + padding_.Left;
		}

		if (maxWidth == 0 || maxWidth > int.MaxValue - size.Width)
		{
			maxWidth = int.MaxValue;
		}

		var textSize = graphics.Measure(LocaleHelper.GetGlobalText(text), font_, maxWidth - size.Width).ToSize();

		size.Width = Math.Min(maxWidth, padding_.Right + size.Width + (maxWidth == 0 ? (int)(textSize.Width * 1.00f) : textSize.Width));
		size.Height = Math.Max(size.Height, textSize.Height + padding_.Vertical);

		if (font is null)
		{
			font_.Dispose();
		}

		if (fixedWidth > 0)
		{
			size.Width = fixedWidth;
		}

		if (fixedHeight > 0)
		{
			size.Height = fixedHeight;
		}

		return size;
	}

	public static ButtonDrawArgs AlignAndDraw(Graphics graphics, Rectangle area, ContentAlignment alignment, ButtonDrawArgs buttonArgs)
	{
		try
		{
			using (buttonArgs)
			{
				PrepareLayout(graphics, buttonArgs);

				buttonArgs.Rectangle = area.Align(buttonArgs.Rectangle.Size, alignment);

				SetUpColors(buttonArgs);

				DrawButton(graphics, buttonArgs);
			}
		}
		catch { }

		return buttonArgs;
	}

	public static ButtonDrawArgs Draw(PaintEventArgs e, ButtonDrawArgs buttonArgs)
	{
		return Draw(e.Graphics, buttonArgs);
	}

	public static ButtonDrawArgs Draw(Graphics graphics, ButtonDrawArgs buttonArgs)
	{
		try
		{
			using (buttonArgs)
			{
				PrepareLayout(graphics, buttonArgs);

				SetUpColors(buttonArgs);

				DrawButton(graphics, buttonArgs);
			}
		}
		catch { }

		return buttonArgs;
	}

	public static void PrepareLayout(Graphics graphics, ButtonDrawArgs arg)
	{
		if (arg.Rectangle == default)
		{
			arg.Rectangle = new Rectangle(arg.Location, arg.Size);
		}

		if (arg.Padding == default)
		{
			arg.Padding = UI.Scale(new Padding(4), UI.UIScale);
		}

		if (arg.Font == null)
		{
			arg.DisposeFont = true;
			arg.Font = UI.Font(8.25F);
		}

		if (arg.Icon != null)
		{
			arg.DisposeImage = true;

			if (!string.IsNullOrWhiteSpace(arg.Text) || arg.Rectangle.Width <= 0)
			{
				arg.Image = arg.Icon.Get(arg.Font.Height * 5 / 4);
			}
			else
			{
				arg.Image = arg.Icon.Get(Math.Min(arg.Rectangle.Width, arg.Rectangle.Height) * 3 / 4);
			}
		}

		if (arg.Rectangle.Width <= 0)
		{
			arg.Rectangle = new Rectangle(arg.Rectangle.Location, GetSize(graphics, arg.Image, arg.Text, arg.Font, arg.Padding, arg.AvailableSize.Width, isLoading: arg.Control?.Loading ?? false, fixedWidth: arg.RequiredWidth, fixedHeight: arg.RequiredHeight));
		}
		else
		{
			var width = arg.Rectangle.Width;
			var maxWidth = GetSize(out var textPad, graphics, arg.Image, arg.Text, arg.Font, arg.Padding, isLoading: arg.Control?.Loading ?? false).Width;

			if (width < maxWidth)
			{
				if (arg.DisposeFont)
				{
					arg.Font = arg.Font.FitTo(LocaleHelper.GetGlobalText(arg.Text), arg.Rectangle.Pad(textPad), graphics);
				}
				else
				{
					arg.Font = new Font(arg.Font, arg.Font.Style).FitTo(LocaleHelper.GetGlobalText(arg.Text), arg.Rectangle.Pad(textPad), graphics);
				}

				arg.DisposeFont = true;
			}
		}
	}

	public static void SetUpColors(ButtonDrawArgs arg)
	{
		if (arg.Cursor.HasValue && !arg.Rectangle.Contains(arg.Cursor.Value))
		{
			arg.HoverState = default;
		}

		if (arg.BackgroundColor.A != 0 && (!arg.HoverState.HasFlag(HoverState.Pressed) || arg.ActiveColor.HasValue))
		{
			arg.BackColor = arg.HoverState.HasFlag(HoverState.Pressed) ? arg.ActiveColor.Value
				: arg.HoverState.HasFlag(HoverState.Hovered) ? arg.BackgroundColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 18 : -14)
				: arg.BackgroundColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 10 : -8);
		}

		if (arg.BackColor.A == 0 && arg.ForeColor.A == 0)
		{
			GetColors(out var fore, out var back, arg.HoverState, arg.ColorStyle, arg.ColorShade, null, arg.BackColor, arg.ButtonType, arg.ActiveColor);

			arg.ForeColor = fore;
			arg.BackColor = back;
		}
		else if (arg.BackColor.A == 0)
		{
			GetColors(out var fore, out var back, arg.HoverState, arg.ColorStyle, arg.ColorShade, null, arg.BackColor, arg.ButtonType, arg.ActiveColor);

			if (arg.ForeColor.A == 0 || arg.HoverState.HasFlag(HoverState.Pressed))
			{
				arg.ForeColor = fore;
			}

			arg.BackColor = back;
		}
		else if (arg.ForeColor.A == 0)
		{
			arg.ForeColor = arg.BackColor.GetTextColor();
		}

		if (!arg.Enabled)
		{
			arg.ForeColor = arg.ForeColor.MergeColor(FormDesign.Design.BackColor, 50);

			if (arg.BackColor.A != 0)
			{
				arg.BackColor = Color.FromArgb(100, arg.BackColor);
			}
		}
		else if (arg.ButtonType == ButtonType.Dimmed && arg.HoverState < HoverState.Pressed)
		{
			arg.ForeColor = arg.ForeColor.MergeColor(FormDesign.Design.BackColor, 90);

			arg.BackColor = Color.FromArgb(arg.HoverState.HasFlag(HoverState.Hovered) ? 150 : 50, arg.BackColor.MergeColor(FormDesign.Design.BackColor, 75));
		}
	}

	public static void DrawButton(Graphics graphics, ButtonDrawArgs arg)
	{
		using (var backBrush = Gradient(arg.Rectangle, arg.BackColor))
		{
			graphics.FillRoundedRectangle(backBrush, arg.Rectangle, arg.BorderRadius ?? arg.Padding.Top);
		}

		if (!arg.HoverState.HasFlag(HoverState.Pressed))
		{
			DrawFocus(graphics,
				arg.Rectangle,
				arg.HoverState,
				arg.BorderRadius ?? arg.Padding.Top,
				arg.ColorShade == null ? (arg.ActiveColor ?? arg.ColorStyle.GetColor()) : arg.ColorStyle.GetColor().Tint(arg.ColorShade?.GetHue()).MergeColor((Color)arg.ColorShade));
		}

		if (arg.ButtonType == ButtonType.Dimmed && arg.HoverState <= HoverState.Normal)
		{
			using var pen = new Pen(Color.FromArgb(135, arg.BackColor), (float)Math.Max(1.5, UI.FontScale)) { Alignment = System.Drawing.Drawing2D.PenAlignment.Inset };
			graphics.DrawRoundedRectangle(pen, arg.Rectangle, arg.BorderRadius ?? arg.Padding.Top);
		}

		var noText = string.IsNullOrWhiteSpace(arg.Text);
		var iconRect = arg.Rectangle
			.Pad(arg.Padding)
			.Align(arg.Image?.Size ?? new Size(arg.Font.Height * 6 / 5, arg.Font.Height * 6 / 5), noText ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft);

		if (arg.Control?.Loading ?? false)
		{
			var color = arg.ColorShade == null ? (arg.ActiveColor ?? arg.ColorStyle.GetColor()) : arg.ColorStyle.GetColor().Tint(arg.ColorShade?.GetHue()).MergeColor((Color)arg.ColorShade);

			if (color.MergeColor(FormDesign.Design.BackColor, 85) == arg.BackColor)
			{
				color = arg.ForeColor;
			}

			arg.Control.DrawLoader(graphics, iconRect, color);
		}
		else if (arg.Image != null)
		{
			if (!arg.DoNotDrawIcon)
			{
				if (arg.ColoredIcon)
				{
					graphics.DrawImage(arg.Image, iconRect);
				}
				else
				{
					graphics.DrawImage(arg.Image.Color(arg.ForeColor), iconRect);
				}
			}
		}
		else
		{
			iconRect = default;
		}

		arg.IconRectangle = iconRect;

		if (noText)
		{
			return;
		}

		var textRect = arg.Rectangle.Pad(arg.Padding.Left + (iconRect.Width > 0 ? (iconRect.Width + arg.Padding.Left) : 0), arg.Padding.Top, arg.Padding.Right, arg.Padding.Bottom);

		textRect.Width += arg.Padding.Right;

		using var brush = new SolidBrush(arg.ForeColor);
		using var stl = new StringFormat
		{
			Alignment = arg.LeftAlign ? StringAlignment.Near : StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		};

		graphics.DrawString(LocaleHelper.GetGlobalText(arg.Text), arg.Font, brush, textRect, stl);
	}
}

public class ButtonDrawArgs : IDisposable
{
	internal bool DisposeFont;
	internal bool DisposeImage;

	public Rectangle IconRectangle { get; internal set; }

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
	public int? BorderRadius { get; set; }
	public Point? Cursor { get; set; }
	public bool LeftAlign { get; set; }
	public Size AvailableSize { get; set; }
	public bool ColoredIcon { get; set; }
	public bool DoNotDrawIcon { get; set; }
	public Color BackgroundColor { get; set; }
	public Color? ActiveColor { get; set; }
	public int RequiredHeight { get; set; }
	public int RequiredWidth { get; set; }

	public void Dispose()
	{
		if (DisposeImage)
		{
			Image?.Dispose();
		}

		if (DisposeFont)
		{
			Font?.Dispose();
		}
	}

	public static implicit operator Rectangle(ButtonDrawArgs args)
	{
		return args.Rectangle;
	}

	public static implicit operator Size(ButtonDrawArgs args)
	{
		return args.Rectangle.Size;
	}
}