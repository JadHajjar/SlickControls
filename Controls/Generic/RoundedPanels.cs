using Extensions;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public class RoundedPanel : DBPanel
{
	[Category("Appearance"), DefaultValue(false)]
	public bool AddOutline { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool AddShadow { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool TopLeft { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool TopRight { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool BotRight { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool BotLeft { get; set; }

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.Clear(Parent?.BackColor ?? BackColor);

		e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

		if (AddShadow)
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Top / 2), Padding.Top / 2, Padding.Top / 2, BackColor, addOutline: AddOutline);
		}
		else
		{
			using (var brush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(2) : ClientRectangle.Pad(1), Padding.Top, !TopLeft, !TopRight, !BotRight, !BotLeft);
			}

			if (AddOutline)
			{
				using var pen = new Pen(FormDesign.Design.AccentColor, AddShadow ? 1.5F : (float)(1.5 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(1).Pad((int)pen.Width), Padding.Top, !TopLeft, !TopRight, !BotRight, !BotLeft);
			}
		}
	}
}

public class RoundedTableLayoutPanel : DBTableLayoutPanel
{
	[Category("Appearance"), DefaultValue(false)]
	public bool AddOutline { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool AddShadow { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool TopLeft { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool TopRight { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool BotRight { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool BotLeft { get; set; }

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.Clear(Parent?.BackColor ?? BackColor);

		e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

		if (AddShadow)
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Top / 2), Padding.Top / 2, Padding.Top / 2, BackColor, addOutline: AddOutline);
		}
		else
		{
			using (var brush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(2) : ClientRectangle.Pad(1), Padding.Top, !TopLeft, !TopRight, !BotRight, !BotLeft);
			}

			if (AddOutline)
			{
				using var pen = new Pen(FormDesign.Design.AccentColor, AddShadow ? 1.5F : (float)(1.5 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(1).Pad((int)pen.Width), Padding.Top, !TopLeft, !TopRight, !BotRight, !BotLeft);
			}
		}
	}
}

public class RoundedFlowLayoutPanel : DBFlowLayoutPanel
{
	[Category("Appearance"), DefaultValue(false)]
	public bool AddOutline { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool AddShadow { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool TopLeft { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool TopRight { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool BotRight { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool BotLeft { get; set; }

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.Clear(Parent?.BackColor ?? BackColor);

		e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

		if (AddShadow)
		{
			e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Top / 2), Padding.Top / 2, Padding.Top / 2, BackColor, addOutline: AddOutline);
		}
		else
		{
			using (var brush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(2) : ClientRectangle.Pad(1), Padding.Top, !TopLeft, !TopRight, !BotRight, !BotLeft);
			}

			if (AddOutline)
			{
				using var pen = new Pen(FormDesign.Design.AccentColor, AddShadow ? 1.5F : (float)(1.5 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(1).Pad((int)pen.Width), Padding.Top, !TopLeft, !TopRight, !BotRight, !BotLeft);
			}
		}
	}
}