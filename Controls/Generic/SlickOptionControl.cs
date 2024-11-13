using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;
[DefaultEvent("ValueChanged")]
public class SlickOptionControl : SlickImageControl
{
	private bool @checked;
	private bool? defaultValue;
	private Font TitleFont;
	private int _selectedOption;

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

	protected override void UIChanged()
	{
		Margin = default;
		Padding = UI.Scale(new Padding(3));
		TitleFont = UI.Font(9F, FontStyle.Bold);
		Font = UI.Font(7F);
	}

	public SlickOptionControl()
	{
		Dock = DockStyle.Top;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			TitleFont?.Dispose();
		}

		base.Dispose(disposing);
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
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (Options is null)
		{
			Cursor = ClientRectangle.Pad(Padding.Left).Contains(e.Location) ? Cursors.Hand : Cursors.Default;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var rectangle = ClientRectangle.Pad(Padding.Left * 3);

		TitleFont ??= Font;

		if (Options is null)
		{
			if (HoverState.HasFlag(HoverState.Hovered) && ClientRectangle.Pad(Padding.Left).Contains(CursorLocation))
			{
				e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Left), Padding.Horizontal, Padding.Left, HoverState.HasFlag(HoverState.Pressed) ? Color.Empty : BackColor, HoverState.HasFlag(HoverState.Pressed) ? Color.FromArgb(7, ColorStyle.GetColor()) : null, true);
			}

			using var titleBrush = new SolidBrush(FormDesign.Design.ForeColor);
			using var descriptionBrush = new SolidBrush(FormDesign.Design.InfoColor);
			using var image = IconManager.GetIcon(Checked ? "Checked_ON" : "Checked_OFF", TitleFont.Height * 5 / 4).Color(Checked ? ColorStyle.GetColor() : FormDesign.Design.ForeColor);

			var titleSize = e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), TitleFont, rectangle.Width - image.Width - Padding.Left);
			var titleRect = rectangle.Pad(image.Width + Padding.Left, titleSize.Height < image.Height ? (image.Height - (int)titleSize.Height) / 2 : -Padding.Top, 0, 0);

			e.Graphics.DrawImage(image, rectangle.Align(image.Size, ContentAlignment.TopLeft));

			e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), TitleFont, titleBrush, titleRect);

			if (!string.IsNullOrEmpty(Description))
			{
				var descriptionRect = rectangle.Pad(image.Width + Padding.Left, Math.Max(image.Height, (int)titleSize.Height) + Padding.Top / 2, 0, 0);
				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Description), base.Font, descriptionBrush, descriptionRect);

				Height = (int)(e.Graphics.Measure(LocaleHelper.GetGlobalText(Description), Font, descriptionRect.Width).Height + Math.Max(image.Height, (int)titleSize.Height) + 6.5 * Padding.Left);
			}
			else
			{
				Height = Math.Max(image.Height, (int)titleSize.Height) + 6 * Padding.Left;
			}
		}
		else
		{
			using var font = UI.Font(8.25f);
			using var titleBrush = new SolidBrush(FormDesign.Design.ForeColor);
			using var descriptionBrush = new SolidBrush(FormDesign.Design.InfoColor);
			using var image = ImageName.Get(TitleFont.Height * 5 / 4)?.Color(FormDesign.Design.ForeColor);
			using var imageOn = IconManager.GetIcon("Circle_ON", TitleFont.Height * 5 / 4).Color(ColorStyle.GetColor());
			using var imageOff = IconManager.GetIcon("Circle_OFF", TitleFont.Height * 5 / 4).Color(FormDesign.Design.ForeColor);

			var titleSize = e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), TitleFont, rectangle.Width - imageOn.Width - Padding.Left);
			var titleRect = rectangle.Pad(imageOn.Width + Padding.Left, titleSize.Height < imageOn.Height ? (imageOn.Height - (int)titleSize.Height) / 2 : -Padding.Top, 0, 0);

			if(image is not null)
			e.Graphics.DrawImage(image, rectangle.Align(image.Size, ContentAlignment.TopLeft));

			e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), TitleFont, titleBrush, titleRect);

			int y;

			if (!string.IsNullOrEmpty(Description))
			{
				var descriptionRect = rectangle.Pad(imageOn.Width + Padding.Left, Math.Max(imageOn.Height, (int)titleSize.Height) + Padding.Top / 2, 0, 0);
				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Description), base.Font, descriptionBrush, descriptionRect);

				y = (int)(e.Graphics.Measure(LocaleHelper.GetGlobalText(Description), Font, descriptionRect.Width).Height + Math.Max(imageOn.Height, (int)titleSize.Height) + 5.5 * Padding.Left);
			}
			else
			{
				y = Math.Max(imageOn.Height, (int)titleSize.Height) + 5 * Padding.Left;
			}

			using var format = new StringFormat { LineAlignment = StringAlignment.Center };
			for (var i = 0; i < Options.Length; i++)
			{
				var rect = new Rectangle(rectangle.X + imageOn.Width + Padding.Left * 3, y, rectangle.Width, imageOn.Height + Padding.Vertical);

				if (HoverState.HasFlag(HoverState.Hovered) && rect.Contains(CursorLocation))
				{
					e.Graphics.FillRoundedRectangleWithShadow(rect.Pad(-Padding.Left, 0, 0, 0), Padding.Horizontal, Padding.Left, HoverState.HasFlag(HoverState.Pressed) ? Color.Empty : BackColor, HoverState.HasFlag(HoverState.Pressed) ? Color.FromArgb(7, ColorStyle.GetColor()) : null, true);
				}

				e.Graphics.DrawImage(SelectedOption == i ? imageOn : imageOff, rect.Align(imageOn.Size, ContentAlignment.MiddleLeft));

				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Options[i]), font, titleBrush, rect.Pad(imageOn.Width + Padding.Left, 0, 0, 0), format);

				y += imageOn.Height + Padding.Vertical;
			}

			Height = y + Padding.Vertical;
		}
	}
}
