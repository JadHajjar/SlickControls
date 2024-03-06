using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls;

public partial class TopIcon : SlickPictureBox, IAnimatable
{
	public enum IconStyle { Minimize, Maximize, Close }

	[Category("Appearance")]
	public IconStyle Color { get; set; }
	[Category("Appearance"), DefaultValue(false)]
	public bool AutomaticSize { get; set; }
	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int AnimatedValue { get; set; }
	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		if (AutomaticSize)
		{
			var size = new Size(6 + (int)((FormDesign.WindowsButtons ? 30 : 20) * UI.UIScale), 6 + (int)(20 * UI.UIScale));

			if (size != Size)
			{
				Size = size;
			}
		}

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
			using var brush = SlickControl.Gradient(new Rectangle(Point.Empty, Size), BackColor.MergeColor(color, 90 * (100 - AnimatedValue) / 100), 2);
			e.Graphics.FillRoundedRectangle(brush, rect, (rect.Width / 2 * (100 - AnimatedValue) / 100) + (int)(3 * UI.FontScale * AnimatedValue / 100));

			using var pen = new Pen(System.Drawing.Color.FromArgb((byte)(255 - (2.55 * AnimatedValue)), color), Math.Max(1.5F, (float)(1.25 * UI.FontScale))) { Alignment = PenAlignment.Outset };
			e.Graphics.DrawRoundedRectangle(pen, rect, (rect.Width / 2 * (100 - AnimatedValue) / 100) + (int)(3 * UI.FontScale * AnimatedValue / 100));

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
			using var brush = SlickControl.Gradient(new Rectangle(Point.Empty, Size), BackColor.MergeColor(color, 90), 2)
				;
			e.Graphics.FillEllipse(brush, rect);

			using var pen = new Pen(color, Math.Max(1.5F, (float)(1.25 * UI.FontScale)));
			e.Graphics.DrawEllipse(pen, rect);
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