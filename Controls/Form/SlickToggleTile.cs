using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace SlickControls;

[DefaultEvent("SelectedChanged")]
public class SlickToggleTile : SlickImageControl
{
	[Category("Behavior")]
	public event EventHandler SelectedChanged;

	[Category("Behavior"), DefaultValue(false)]
	public bool Selected { get; set; }

	public SlickToggleTile()
	{
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(12));
		Font = UI.Font(9F, FontStyle.Bold);
		Size = UI.Scale(new Size(100, 100));

		base.UIChanged();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		Cursor = ClientRectangle.Pad(Padding).Contains(e.Location) ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left && ClientRectangle.Pad(Padding).Contains(e.Location))
		{
			Selected = !Selected;
			SelectedChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		if (Selected)
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Left / 2), Padding.Left / 2, Padding.Left / 2, FormDesign.Design.BackColor.MergeColor(FormDesign.Design.ActiveColor, 90), Color.FromArgb(8, FormDesign.Design.ActiveColor), true);

			if (HoverState.HasFlag(HoverState.Hovered) && ClientRectangle.Pad(Padding).Contains(PointToClient(Cursor.Position)))
			{
				using var pen = new Pen(FormDesign.Design.IsDarkTheme ? Color.FromArgb(150, 255, 255, 255) : Color.FromArgb(150, FormDesign.Design.AccentColor), UI.Scale(1.5f)) { Alignment = PenAlignment.Center };
				e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(Padding.Left / 2), Padding.Left / 2);
			}
		}
		else if (HoverState.HasFlag(HoverState.Hovered) && ClientRectangle.Pad(Padding).Contains(PointToClient(Cursor.Position)))
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Left / 2), Padding.Left / 2, Padding.Left / 2, FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 8 : -8), FormDesign.Design.IsDarkTheme ? Color.FromArgb(2, 255, 255, 255) : Color.FromArgb(15, FormDesign.Design.AccentColor), true);
		}
		else
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Left / 2), Padding.Left / 2, Padding.Left / 2, FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? -2 : 2));
		}

		using var font = UI.Font(7F, FontStyle.Bold).FitTo(Text, ClientRectangle.ClipTo(Height / 2).Pad(Padding.Left, Padding.Left / 2, Padding.Left, Padding.Left / 2), e.Graphics);
		var textHeight = (int)e.Graphics.Measure(Text, font, Width - Padding.Horizontal).Height;
		using var img = ImageName.Get(Height / 2)?.Color(FormDesign.Design.IconColor);
		using var textBrush = new SolidBrush(FormDesign.Design.ForeColor);

		if (img == null)
		{
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(Text, font, textBrush, ClientRectangle.Pad(Padding), format);
		}
		else
		{
			var rect = ClientRectangle.Pad(Padding).CenterR(ClientRectangle.Pad(Padding).Width, textHeight + img.Height + (int)(2.5 * UI.FontScale));

			if (Loading)
			{
				DrawLoader(e.Graphics, rect.Align(img.Size, ContentAlignment.TopCenter));
			}
			else
			{
				e.Graphics.DrawImage(img, rect.Align(img.Size, ContentAlignment.TopCenter));
			}

			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
			e.Graphics.DrawString(Text, font, textBrush, rect, format);
		}
	}
}