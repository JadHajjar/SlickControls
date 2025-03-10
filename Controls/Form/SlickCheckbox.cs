﻿using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

[DefaultEvent("CheckChanged")]
public partial class SlickCheckbox : SlickButton, ISupportsReset
{
	private bool @checked;

	private string checkedText;

	private string uncheckedText;
	private bool _useToggleIcon;

	public SlickCheckbox()
	{
		Cursor = Cursors.Hand;
		SpaceTriggersClick = true;
		EnterTriggersClick = false;
	}

	[Category("Behavior")]
	public event EventHandler CheckChanged;

	[Category("Appearance"), DefaultValue(false), DisplayName("Use Toggle Icon")]
	public bool UseToggleIcon
	{
		get => _useToggleIcon; set
		{
			_useToggleIcon = value;
			Checked = Checked;
		}
	}

	[Category("Behavior"), DefaultValue(false)]
	public bool OverrideCheckClick { get; set; }

	[Category("Behavior")]
	public bool Checked
	{
		get => @checked;
		set
		{
			var chkChanged = @checked != value;
			@checked = value;

			if (!string.IsNullOrEmpty(CheckedText))
			{
				Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
			}

			DefaultValue ??= value;

			if (chkChanged)
			{
				CheckChanged?.Invoke(this, new EventArgs());
			}

			Invalidate();
		}
	}

	[Category("Appearance"), DisplayName("Checked Icon"), DefaultValue(null)]
	public Bitmap CheckedIcon { get; set; }

	[Category("Appearance"), DisplayName("Unchecked Icon"), DefaultValue(null)]
	public Bitmap UnCheckedIcon { get; set; }

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool? DefaultValue { get; set; } = null;

	[Category("Behavior"), DisplayName("Fade when Unchecked"), DefaultValue(false)]
	public bool FadeUnchecked { get; set; }

	[Category("Appearance"), DisplayName("Checked Text"), DefaultValue(null)]
	public string CheckedText
	{
		get => checkedText;
		set
		{
			checkedText = value;
			if (!string.IsNullOrEmpty(value))
			{
				Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
			}
		}
	}

	[Category("Appearance"), DisplayName("Unchecked Text"), DefaultValue(null)]
	public string UncheckedText
	{
		get => uncheckedText;
		set
		{
			uncheckedText = value;
			if (!string.IsNullOrEmpty(value))
			{
				Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
			}
		}
	}

	public void ResetValue()
	{
		Checked = DefaultValue ?? true;
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (!OverrideCheckClick && (e.Button == MouseButtons.Left || e.Button == MouseButtons.None))
		{
			Checked = !Checked;
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		base.AutoSize = true;
	}

	public override Size CalculateAutoSize(Size availableSize)
	{
		using var image = GetIcon();
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

		PrepareLayout(graphics, args);

		return args.Rectangle.Size;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		try
		{
			GetColors(out var fore, out var back);

			if (FadeUnchecked && Checked && HoverState == HoverState.Normal)
			{
				fore = back.GetTextColor();
			}

			using var image = GetIcon();

			var args = new ButtonDrawArgs
			{
				Control = this,
				Font = Font,
				BackColor = back,
				ForeColor = fore,
				Image = image,
				Text = Text,
				Padding = Padding,
				ButtonType = ButtonType.Hidden,
				ColorShade = ColorShade,
				ColorStyle = ColorStyle,
				Enabled = Enabled,
				HoverState = HoverState,
				LeftAlign = true,
				ColoredIcon = true,
				DoNotDrawIcon = true,
				Rectangle = ClientRectangle
			};

			Draw(e.Graphics, args);

			if (!Loading && image != null)
			{
				if (!HoverState.HasFlag(HoverState.Pressed) && @checked && !UseToggleIcon && CheckedIcon == null && UnCheckedIcon == null)
				{
					using var brush = new SolidBrush(ColorStyle.GetBackColor());
					e.Graphics.FillRectangle(brush, args.IconRectangle.CenterR(args.IconRectangle.Width * 2 / 3, args.IconRectangle.Width * 2 / 3));
					image.Color(ColorStyle.GetColor());
				}
				else
				{
					image.Color(fore);
				}

				if (string.IsNullOrWhiteSpace(Text))
				{
					e.Graphics.DrawImage(image, args.IconRectangle);
				}
				else
				{
					e.Graphics.DrawImage(image, args.IconRectangle);
				}
			}

			if (FadeUnchecked && !HoverState.HasFlag(HoverState.Hovered))
			{
				if (Checked)
				{
					using var pen = new Pen(Color.FromArgb(175, ColorStyle.GetColor()), UI.Scale(1.5F)) { Alignment = System.Drawing.Drawing2D.PenAlignment.Center };
					using var brush = new SolidBrush(Color.FromArgb(65, ColorStyle.GetColor()));
					e.Graphics.FillRoundedRectangle(brush, args.Rectangle, Padding.Top);
					e.Graphics.DrawRoundedRectangle(pen, args.Rectangle.Pad((int)(pen.Width / 2)), Padding.Top);
				}
				else
				{
					using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
					e.Graphics.FillRectangle(brush, ClientRectangle);
				}
			}
		}
		catch { }
	}

	private Bitmap GetIcon()
	{
		return GetIcon(@checked, UseToggleIcon, CheckedIcon, UnCheckedIcon);
	}

	public static Bitmap GetIcon(bool @checked, bool useToggleIcon = false, Bitmap checkedIcon = null, Bitmap unCheckedIcon = null)
	{
		Bitmap image;

		if (checkedIcon != null && unCheckedIcon != null)
		{
			image = new Bitmap(@checked ? checkedIcon : unCheckedIcon);
		}
		else if (useToggleIcon)
		{
			image = IconManager.GetIcon(@checked ? "Toggle_ON" : "Toggle_OFF");
		}
		else
		{
			image = IconManager.GetIcon(@checked ? "Checked_ON" : "Checked_OFF");
		}

		return image;
	}

	protected virtual void GetColors(out Color fore, out Color back)
	{
		if (HoverState.HasFlag(HoverState.Pressed))
		{
			fore = ColorStyle.GetBackColor();
			back = ColorStyle.GetColor();
		}
		else if (HoverState.HasFlag(HoverState.Hovered))
		{
			fore = FormDesign.Design.ForeColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? -7 : 7);
			back = FormDesign.Design.ButtonColor.MergeColor(BackColor, 75);
		}
		else
		{
			fore = Enabled ? ForeColor : ForeColor.MergeColor(BackColor);
			back = Color.Empty;
		}
	}
}
