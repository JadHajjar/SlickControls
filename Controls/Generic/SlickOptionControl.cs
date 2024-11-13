using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;
[DefaultEvent("ValueChanged")]
public class SlickOptionControl : SlickImageControl
{
	private bool @checked;
	private bool? defaultValue;
	private Font TitleFont;
	private int _selectedOption;
	private Size cachedSize;
	private Size lastAvailableSize;
	private bool live;
	private readonly Dictionary<int, Rectangle> _optionRects = [];

	[Category("Behavior")]
	public event EventHandler ValueChanged;

	[Category("Behavior")]
	public bool Checked
	{
		get => @checked;
		set
		{
			var chkChanged = @checked != value;

			@checked = value;
			defaultValue ??= value;

			if (chkChanged)
			{
				ValueChanged?.Invoke(this, new EventArgs());
			}

			Invalidate();
		}
	}

	[Category("Behavior"), DefaultValue(0)]
	public int SelectedOption
	{
		get => _selectedOption;
		set
		{
			var chkChanged = _selectedOption != value;

			_selectedOption = value;

			if (chkChanged)
			{
				ValueChanged?.Invoke(this, new EventArgs());
			}

			Invalidate();
		}
	}

	[Category("Behavior"), DefaultValue(null)]
	public string[] Options { get; set; }

	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
	public override string Text { get => base.Text; set => base.Text = value; }

	[Category("Appearance"), DefaultValue(null)]
	public string Description { get; set; }

	[Category("Appearance"), DefaultValue(ColorStyle.Active)]
	public ColorStyle ColorStyle { get; set; }

	[Category("Data"), DefaultValue(null)]
	public string OptionName { get; set; }

	public SlickOptionControl()
	{
		AutoSize = true;
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(3));
		TitleFont = UI.Font(9F, FontStyle.Bold);
		Font = UI.Font(7F);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		live = true;

		if (!DesignMode)
		{
			PerformLayout();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			TitleFont?.Dispose();
		}

		base.Dispose(disposing);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		return CalculateSize();
	}

	private Size CalculateSize()
	{
		if (!live || !Live || Anchor == (AnchorStyles)15 || Dock == DockStyle.Fill)
		{
			return Size;
		}

		var availableSize = GetAvailableSize();

		if (cachedSize != default && lastAvailableSize == availableSize)
		{
			return cachedSize;
		}

		lastAvailableSize = availableSize;

		using var graphics = Graphics.FromHwnd(IntPtr.Zero);

		return cachedSize = DoDrawing(graphics, new(default, availableSize), false);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			if (Options is null && ClientRectangle.Pad(Padding.Left).Contains(e.Location))
			{
				Checked = !Checked;
			}
			else if (Options is not null)
			{
				foreach (var item in _optionRects)
				{
					if (item.Value.Contains(e.Location))
					{
						SelectedOption = item.Key;
					}
				}
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (Options is null)
		{
			Cursor = ClientRectangle.Pad(Padding.Left).Contains(e.Location) ? Cursors.Hand : Cursors.Default;
		}
		else
		{
			Cursor = _optionRects.Values.Any(x => x.Contains(e.Location)) ? Cursors.Hand : Cursors.Default;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		DoDrawing(e.Graphics, ClientRectangle, true);
	}

	private Size DoDrawing(Graphics graphics, Rectangle availableSize, bool applyDrawing)
	{
		var rectangle = availableSize.Pad(Padding.Left * 3);

		if (applyDrawing && Options is null && HoverState.HasFlag(HoverState.Hovered) && availableSize.Pad(Padding.Left).Contains(CursorLocation))
		{
			graphics.FillRoundedRectangleWithShadow(availableSize.Pad(Padding.Left), Padding.Horizontal, Padding.Left, HoverState.HasFlag(HoverState.Pressed) ? Color.Empty : BackColor, HoverState.HasFlag(HoverState.Pressed) ? Color.FromArgb(7, ColorStyle.GetColor()) : null, true);
		}

		using var titleBrush = new SolidBrush(FormDesign.Design.ForeColor);
		using var descriptionBrush = new SolidBrush(FormDesign.Design.InfoColor);
		using var image = (ImageName ?? new DynamicIcon(Checked ? "Checked_ON" : "Checked_OFF"))?.Get((TitleFont ?? Font).Height * 5 / 4)?.Color(Checked ? ColorStyle.GetColor() : FormDesign.Design.ForeColor);
		var imageSize = image?.Size ?? Size.Empty;

		var text = LocaleHelper.GetGlobalText(Text);
		var titleSize = graphics.Measure(text, TitleFont ?? Font, rectangle.Width - imageSize.Width - Padding.Left);
		var titleRect = rectangle.Pad(imageSize.Width + Padding.Left, titleSize.Height < imageSize.Height ? (imageSize.Height - (int)titleSize.Height) / 2 : -Padding.Top, 0, 0);

		if (applyDrawing)
		{
			if (image is not null)
			{
				graphics.DrawImage(image, rectangle.Align(imageSize, ContentAlignment.TopLeft));
			}

			graphics.DrawString(text, TitleFont ?? Font, titleBrush, titleRect);
		}

		Size size;

		if (!string.IsNullOrEmpty(Description))
		{
			var descriptionRect = rectangle.Pad(imageSize.Width + Padding.Left, Math.Max(imageSize.Height, (int)titleSize.Height), 0, 0);

			var description = LocaleHelper.GetGlobalText(Description);

			if (applyDrawing)
			{
				graphics.DrawString(description, Font, descriptionBrush, descriptionRect);
			}

			size = new Size(Math.Max(descriptionRect.X + (int)graphics.Measure(description, Font, descriptionRect.Width).Width, titleRect.X + (int)titleSize.Width) + (4 * Padding.Left)
				, descriptionRect.Y + (int)(graphics.Measure(description, Font, descriptionRect.Width).Height + (2 * Padding.Left)));
		}
		else
		{
			size = new Size(titleRect.X + (int)titleSize.Width + Padding.Horizontal, Math.Max(imageSize.Height, (int)titleSize.Height) + (5 * Padding.Left));
		}

		if (Options is null)
		{
			size.Height += Padding.Left;

			return size;
		}

		using var imageOn = IconManager.GetIcon("Circle_ON", (TitleFont ?? Font).Height * 5 / 4).Color(ColorStyle.GetColor());
		using var imageOff = IconManager.GetIcon("Circle_OFF", (TitleFont ?? Font).Height * 5 / 4).Color(FormDesign.Design.ForeColor);

		using var format = new StringFormat { LineAlignment = StringAlignment.Center };

		for (var i = 0; i < Options.Length; i++)
		{
			var optionText = LocaleHelper.GetGlobalText(Options[i]);
			var rect = new Rectangle(rectangle.X + imageOn.Width + (Padding.Left * 3), size.Height, rectangle.Width - +imageOn.Width - (Padding.Left * 3), imageOn.Height + Padding.Vertical);
			using var font = UI.Font(8.25f).FitToWidth(optionText, rect.Pad(imageOn.Width + Padding.Left, 0, 0, 0), graphics);

			var textRect = new Rectangle(rect.X + imageOn.Width + Padding.Left, rect.Y, (int)graphics.Measure(optionText, font, rect.Width - imageOn.Width - Padding.Left).Width + Padding.Left, rect.Height);

			size.Width = Math.Max(size.Width, textRect.Right);

			_optionRects[i] = textRect.Pad(-Padding.Left - imageOn.Width - Padding.Left, 0, 0, 0);

			if (applyDrawing)
			{
				if (HoverState.HasFlag(HoverState.Hovered) && _optionRects[i].Contains(CursorLocation))
				{
					graphics.FillRoundedRectangleWithShadow(_optionRects[i], Padding.Horizontal, Padding.Left, HoverState.HasFlag(HoverState.Pressed) ? Color.Empty : BackColor, HoverState.HasFlag(HoverState.Pressed) ? Color.FromArgb(7, ColorStyle.GetColor()) : null, true);
				}

				graphics.DrawImage(SelectedOption == i ? imageOn : imageOff, rect.Align(imageOn.Size, ContentAlignment.MiddleLeft));

				graphics.DrawString(optionText, font, titleBrush, textRect, format);
			}

			size.Height += imageOn.Height + (Padding.Left * 3);
		}

		size.Height += Padding.Vertical;

		return size;
	}
}
