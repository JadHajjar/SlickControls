using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public class RoundedGroupPanel : DBPanel
{
	private Image image;

	[Category("Appearance"), DefaultValue(null)]
	public virtual Image Image
	{
		get => image; set
		{
			image = value;
			UIChanged();
		}
	}

	[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
	public DynamicIcon ImageName { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool AddOutline { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool AddShadow { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool AddPaddingForIcon { get; set; }

	[Category("Appearance"), DefaultValue(null)]
	public string Info { get; set; }

	[Category("Appearance"), DefaultValue(ColorStyle.Text)]
	public ColorStyle ColorStyle { get; set; } = ColorStyle.Text;

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

	public RoundedGroupPanel()
	{
		UI.UIChanged += UIChanged;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			UI.UIChanged -= UIChanged;
		}

		base.Dispose(disposing);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		UIChanged();
	}

	private void UIChanged()
	{
		try
		{
			var padding = UI.Scale(new Padding(AddShadow ? 12 : 5), UI.FontScale);
			var pad = (int)(4 * UI.FontScale);

			var iconWidth = 0;
			if (Image != null)
			{
				iconWidth = Image.Width;
			}
			else if (ImageName != null)
			{
				iconWidth = ImageName.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16)?.Width ?? 0;
			}

			if (AddPaddingForIcon)
			{
				padding.Left = (pad * 2) + iconWidth;
			}

			var titleHeight = iconWidth * 4 / 3;

			if (titleHeight > 0)
			{
				padding.Top += pad + titleHeight;
			}

			Padding = padding;
			Invalidate();
		}
		catch { }
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		if (AddShadow)
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Right / 2), Padding.Right / 2, Padding.Right / 2, BackColor, ColorStyle == ColorStyle.Text ? null : Color.FromArgb(8, ColorStyle.GetColor()), addOutline: AddOutline);
		}
		else
		{
			using (var brush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(2) : ClientRectangle.Pad(1), Padding.Right);
			}

			if (AddOutline)
			{
				using var pen = new Pen(ColorStyle == ColorStyle.Text ? FormDesign.Design.AccentColor : ColorStyle.GetColor(), AddShadow ? 1.5F : (float)(1.5 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(1).Pad((int)pen.Width), Padding.Right);
			}
		}

		try
		{
			var text = LocaleHelper.GetGlobalText(Text);
			var info = LocaleHelper.GetGlobalText(Info);
			using var icon = Image == null ? ImageName?.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16) : new Bitmap(Image);
			var iconWidth = icon?.Width ?? 0;
			using var font = UI.Font(9F, FontStyle.Bold).FitToWidth(text + info, new Rectangle(iconWidth + (Padding.Right * 2), Padding.Bottom, Width - Padding.Horizontal, Height), e.Graphics);
			var textColor = ColorStyle == ColorStyle.Text ? FormDesign.Design.LabelColor : ColorStyle.GetColor().MergeColor(FormDesign.Design.IconColor, 85);
			var titleHeight = Math.Max(iconWidth, (int)e.Graphics.Measure(" ", font).Height);
			var rectangle = new Rectangle(Padding.Right, Padding.Bottom, Width - (Padding.Right * 2), titleHeight);
			var iconRectangle = rectangle.Align(new Size(iconWidth, iconWidth), ContentAlignment.MiddleLeft);
			var textRectangle = rectangle.Pad(icon != null ? iconWidth + (Padding.Right / 2) : 0, 0, 0, 0);
			using var brush = new SolidBrush(textColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center };

			if (icon != null)
			{
				e.Graphics.DrawImage(icon.Color(textColor), iconRectangle);
			}

			e.Graphics.DrawString(text, font, brush, textRectangle, format);

			if (!string.IsNullOrWhiteSpace(Info))
			{
				using var brush2 = new SolidBrush(Color.FromArgb(175, textColor));
				using var font2 = new Font(font.FontFamily, font.Size - 1.5f);
				var bnds = e.Graphics.Measure(text, font);

				e.Graphics.DrawString(info, font2, brush2, textRectangle.Pad((int)bnds.Width + (Padding.Right / 2), 0, 0, 0), format);
			}
		}
		catch { }
	}
}

public class RoundedGroupFlowLayoutPanel : DBFlowLayoutPanel
{
	private Image image;

	[Category("Appearance"), DefaultValue(null)]
	public virtual Image Image
	{
		get => image; set
		{
			image = value;
			UIChanged();
		}
	}

	[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
	public DynamicIcon ImageName { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool AddOutline { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool AddShadow { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool AddPaddingForIcon { get; set; }

	[Category("Appearance"), DefaultValue(null)]
	public string Info { get; set; }

	[Category("Appearance"), DefaultValue(ColorStyle.Text)]
	public ColorStyle ColorStyle { get; set; } = ColorStyle.Text;

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

	public RoundedGroupFlowLayoutPanel()
	{
		UI.UIChanged += UIChanged;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			UI.UIChanged -= UIChanged;
		}

		base.Dispose(disposing);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		UIChanged();
	}

	private void UIChanged()
	{
		try
		{
			var padding = UI.Scale(new Padding(AddShadow ? 12 : 5), UI.FontScale);
			var pad = (int)(4 * UI.FontScale);

			var iconWidth = 0;
			if (Image != null)
			{
				iconWidth = Image.Width;
			}
			else if (ImageName != null)
			{
				iconWidth = ImageName.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16)?.Width ?? 0;
			}

			if (AddPaddingForIcon)
			{
				padding.Left = (pad * 2) + iconWidth;
			}

			var titleHeight = iconWidth * 4 / 3;

			if (titleHeight > 0)
			{
				padding.Top += pad + titleHeight;
			}

			Padding = padding;
			Invalidate();
		}
		catch { }
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		if (AddShadow)
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Right / 2), Padding.Right / 2, Padding.Right / 2, BackColor, ColorStyle == ColorStyle.Text ? null : Color.FromArgb(8, ColorStyle.GetColor()), addOutline: AddOutline);
		}
		else
		{
			using (var brush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(2) : ClientRectangle.Pad(1), Padding.Right);
			}

			if (AddOutline)
			{
				using var pen = new Pen(ColorStyle == ColorStyle.Text ? FormDesign.Design.AccentColor : ColorStyle.GetColor(), AddShadow ? 1.5F : (float)(1.5 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(1).Pad((int)pen.Width), Padding.Right);
			}
		}

		try
		{
			var text = LocaleHelper.GetGlobalText(Text);
			var info = LocaleHelper.GetGlobalText(Info);
			using var icon = Image == null ? ImageName?.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16) : new Bitmap(Image);
			var iconWidth = icon?.Width ?? 0;
			using var font = UI.Font(9F, FontStyle.Bold).FitToWidth(text + info, new Rectangle(iconWidth + (Padding.Right * 2), Padding.Bottom, Width - Padding.Horizontal, Height), e.Graphics);
			var textColor = ColorStyle == ColorStyle.Text ? FormDesign.Design.LabelColor : ColorStyle.GetColor().MergeColor(FormDesign.Design.IconColor, 85);
			var titleHeight = Math.Max(iconWidth, (int)e.Graphics.Measure(" ", font).Height);
			var rectangle = new Rectangle(Padding.Right, Padding.Bottom, Width - (Padding.Right * 2), titleHeight);
			var iconRectangle = rectangle.Align(new Size(iconWidth, iconWidth), ContentAlignment.MiddleLeft);
			var textRectangle = rectangle.Pad(icon != null ? iconWidth + (Padding.Right / 2) : 0, 0, 0, 0);
			using var brush = new SolidBrush(textColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center };

			if (icon != null)
			{
				e.Graphics.DrawImage(icon.Color(textColor), iconRectangle);
			}

			e.Graphics.DrawString(text, font, brush, textRectangle, format);

			if (!string.IsNullOrWhiteSpace(Info))
			{
				using var brush2 = new SolidBrush(Color.FromArgb(175, textColor));
				using var font2 = new Font(font.FontFamily, font.Size - 1.5f);
				var bnds = e.Graphics.Measure(text, font);

				e.Graphics.DrawString(info, font2, brush2, textRectangle.Pad((int)bnds.Width + (Padding.Right / 2), 0, 0, 0), format);
			}
		}
		catch { }
	}
}

public class RoundedGroupTableLayoutPanel : DBTableLayoutPanel
{
	private Image image;

	[Category("Appearance"), DefaultValue(null)]
	public virtual Image Image
	{
		get => image; set
		{
			image = value;
			UIChanged();
		}
	}

	[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
	public DynamicIcon ImageName { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool AddOutline { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool AddShadow { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool AddPaddingForIcon { get; set; }

	[Category("Appearance"), DefaultValue(null)]
	public string Info { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool UseFirstRowForPadding { get; set; }

	[Category("Appearance"), DefaultValue(ColorStyle.Text)]
	public ColorStyle ColorStyle { get; set; } = ColorStyle.Text;

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

	public RoundedGroupTableLayoutPanel()
	{
		UI.UIChanged += UIChanged;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			UI.UIChanged -= UIChanged;
		}

		base.Dispose(disposing);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		UIChanged();
	}

	private void UIChanged()
	{
		try
		{
			var padding = UI.Scale(new Padding(AddShadow ? 12 : 5), UI.FontScale);
			var pad = (int)(4 * UI.FontScale);

			var iconWidth = 0;
			if (Image != null)
			{
				iconWidth = Image.Width;
			}
			else if (ImageName != null)
			{
				iconWidth = ImageName.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16)?.Width ?? 0;
			}

			if (AddPaddingForIcon)
			{
				if (UseFirstRowForPadding)
				{
					ColumnStyles[0].Width = pad + iconWidth;
				}
				else
				{
					padding.Left = (pad * 2) + iconWidth;
				}
			}

			var titleHeight = iconWidth * 4 / 3;

			if (titleHeight > 0)
			{
				if (UseFirstRowForPadding)
				{
					RowStyles[0].Height = pad + titleHeight;
				}
				else
				{
					padding.Top += pad + titleHeight;
				}
			}

			Padding = padding;
			Invalidate();
		}
		catch { }
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent?.BackColor ?? BackColor);

		if (AddShadow)
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Right / 2), Padding.Right / 2, Padding.Right / 2, BackColor, ColorStyle == ColorStyle.Text ? null : Color.FromArgb(8, ColorStyle.GetColor()), addOutline: AddOutline);
		}
		else
		{
			using (var brush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(2) : ClientRectangle.Pad(1), Padding.Right);
			}

			if (AddOutline)
			{
				using var pen = new Pen(ColorStyle == ColorStyle.Text ? FormDesign.Design.AccentColor : ColorStyle.GetColor(), AddShadow ? 1.5F : (float)(1.5 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(1).Pad((int)pen.Width), Padding.Right);
			}
		}

		try
		{
			var text = LocaleHelper.GetGlobalText(Text);
			var info = LocaleHelper.GetGlobalText(Info);
			using var icon = Image == null ? ImageName?.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16) : new Bitmap(Image);
			var iconWidth = icon?.Width ?? 0;
			using var font = UI.Font(9F, FontStyle.Bold).FitToWidth(text + info, new Rectangle(iconWidth + (Padding.Right * 2), Padding.Bottom, Width - Padding.Horizontal, Height), e.Graphics);
			var textColor = ColorStyle == ColorStyle.Text ? FormDesign.Design.LabelColor : ColorStyle.GetColor().MergeColor(FormDesign.Design.IconColor, 85);
			var titleHeight = Math.Max(iconWidth, (int)e.Graphics.Measure(" ", font).Height);
			var rectangle = new Rectangle(Padding.Right, Padding.Bottom, Width - (Padding.Right * 2), titleHeight);

			if (UseFirstRowForPadding)
			{
				titleHeight = (int)RowStyles[0].Height;
				rectangle = rectangle.Pad((titleHeight - rectangle.Height) / 2, 0, 0, 0);
				rectangle.Height = titleHeight;
			}

			var iconRectangle = rectangle.Align(new Size(iconWidth, iconWidth), ContentAlignment.MiddleLeft);
			var textRectangle = rectangle.Pad(icon != null ? iconWidth + (Padding.Right / 2) : 0, 0, 0, 0);
			using var brush = new SolidBrush(textColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center };

			if (icon != null)
			{
				e.Graphics.DrawImage(icon.Color(textColor), iconRectangle);
			}

			e.Graphics.DrawString(text, font, brush, textRectangle, format);

			if (!string.IsNullOrWhiteSpace(Info))
			{
				using var brush2 = new SolidBrush(Color.FromArgb(175, textColor));
				using var font2 = new Font(font.FontFamily, font.Size - 1.5f);
				var bnds = e.Graphics.Measure(text, font);

				e.Graphics.DrawString(info, font2, brush2, textRectangle.Pad((int)bnds.Width + (Padding.Right / 2), 0, 0, 0), format);
			}
		}
		catch { }
	}
}
