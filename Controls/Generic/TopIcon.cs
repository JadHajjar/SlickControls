using Extensions;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls;

public partial class TopIcon : SlickPictureBox, IAnimatable
{
	public enum IconStyle { Minimize, Maximize, Close }

	public IconStyle Color { get; set; }
	public int AnimatedValue { get; set; }
	public int TargetAnimationValue => !FormDesign.WindowsButtons && HoverState.HasFlag(HoverState.Hovered) ? 100 : 0;

	public TopIcon()
	{
		Cursor = Cursors.Hand;
	}

	public override HoverState HoverState
	{
		get => base.HoverState; internal set
		{
			base.HoverState = value;
			AnimationHandler.Animate(this);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var color = FormDesign.Design.IconColor;

		switch (Color)
		{
			case IconStyle.Maximize:
				color = FormDesign.Design.GreenColor;
				break;

			case IconStyle.Minimize:
				color = FormDesign.Design.YellowColor;
				break;

			case IconStyle.Close:
				color = FormDesign.Design.RedColor;
				break;
		}

		if (FormDesign.WindowsButtons)
		{
			DrawWindowsButton(e, color);
			return;
		}

		var rect = ClientRectangle.Pad(Padding).Pad((int)Math.Ceiling(1.25 * UI.FontScale));

		if (AnimatedValue != 0 || CrossIO.CurrentPlatform != Platform.Windows)
		{
			e.Graphics.FillRoundedRectangle(
				SlickControl.Gradient(new Rectangle(Point.Empty, Size), BackColor.MergeColor(color, 90 * (100 - AnimatedValue) / 100), 2),
				rect,
				(rect.Width / 2 * (100 - AnimatedValue) / 100) + (int)(3 * UI.FontScale * AnimatedValue / 100));

			e.Graphics.DrawRoundedRectangle(
				new Pen(System.Drawing.Color.FromArgb((byte)(255 - (2.55 * AnimatedValue)), color), Math.Max(1.5F, (float)(1.25 * UI.FontScale))) { Alignment = PenAlignment.Outset },
				rect,
				(rect.Width / 2 * (100 - AnimatedValue) / 100) + (int)(3 * UI.FontScale * AnimatedValue / 100));

			Bitmap icon = null;
			switch (Color)
			{
				case IconStyle.Maximize:
					icon = FindForm().WindowState == FormWindowState.Maximized ? Properties.Resources.Icon_Restore : Properties.Resources.Icon_Maximize;
					break;

				case IconStyle.Minimize:
					icon = Properties.Resources.Icon_Minimize;
					break;

				case IconStyle.Close:
					icon = Properties.Resources.Icon_Close;
					break;
			}

			if (icon != null)
			{
				using (icon)
				{
					e.Graphics.DrawImage(icon.Color(color.GetTextColor(), (byte)(2.55 * AnimatedValue)), rect.CenterR(icon.Size).Pad(1, 1, -1, -1));
				}
			}
		}
		else
		{
			e.Graphics.FillEllipse(
				SlickControl.Gradient(new Rectangle(Point.Empty, Size), BackColor.MergeColor(color, 90), 2),
				rect);

			e.Graphics.DrawEllipse(
				new Pen(color, Math.Max(1.5F, (float)(1.25 * UI.FontScale))) { },
				rect);
		}
	}

	private void DrawWindowsButton(PaintEventArgs e, Color color)
	{
		Bitmap icon = null;
		switch (Color)
		{
			case IconStyle.Maximize:
				icon = FindForm().WindowState == FormWindowState.Maximized ? Properties.Resources.Icon_Restore : Properties.Resources.Icon_Maximize;
				break;

			case IconStyle.Minimize:
				icon = Properties.Resources.Icon_Minimize;
				break;

			case IconStyle.Close:
				icon = Properties.Resources.Icon_Close;
				break;
		}

		Color fore;

		if (HoverState.HasFlag(HoverState.Pressed))
		{
			using (var brush = new SolidBrush(color))
			{
				e.Graphics.FillRectangle(brush, ClientRectangle);
			}

			fore = color.GetAccentColor();
		}
		else if (HoverState.HasFlag(HoverState.Hovered))
		{
			using (var brush = new SolidBrush(FormDesign.Design.ButtonColor))
			{
				e.Graphics.FillRectangle(brush, ClientRectangle);
			}

			fore = FormDesign.Design.ButtonForeColor;
		}
		else
		{
			fore = FormDesign.Design.IconColor;
		}

		using (icon)
		{
			e.Graphics.DrawImage(icon.Color(fore), ClientRectangle.CenterR(icon.Size));
		}
	}
}