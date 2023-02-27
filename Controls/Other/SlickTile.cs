using Extensions;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("Click")]
	public partial class SlickTile : SlickImageControl
	{
		private bool colorIcon = true;
		private int corner = 5;
		private int iconSize = 16;
		private bool roundedCorner = false;
		private bool selected = false;

		public SlickTile()
		{
			Padding = new Padding(5);
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool ColorIcon { get => colorIcon; set { colorIcon = value; Invalidate(); } }

		[Category("Behavior"), DefaultValue(5)]
		public int CornerRadius { get => corner; set { corner = value; Invalidate(); } }

		[Category("Appearance"), DefaultValue(16)]
		public int IconSize { get => iconSize; set { iconSize = value; Invalidate(); } }

		[Category("Appearance"), DefaultValue(false)]
		public bool RoundedCorner { get => roundedCorner; set { roundedCorner = value; Invalidate(); } }

		[Category("Behavior"), DefaultValue(false)]
		public bool Selected { get => selected; set { selected = value; Invalidate(); } }

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; Invalidate(); } }

		[Category("Appearance"), DefaultValue(true)]
		public bool DrawLeft { get; set; } = true;

		protected override void OnPaint(PaintEventArgs e)
		{
			var back = BackColor;
			var fore = FormDesign.Design.ForeColor;

			if (HoverState.HasFlag(HoverState.Hovered))
				back = BackColor.MergeColor(FormDesign.Design.BackColor, 25);

			if (Selected)
			{
				fore = FormDesign.Design.ActiveColor;
				back = FormDesign.Design.BackColor;
			}
			else if (HoverState.HasFlag(HoverState.Pressed))
			{
				back = FormDesign.Design.ActiveColor;
				fore = FormDesign.Design.ActiveForeColor;
			}

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			if (RoundedCorner)
			{
				e.Graphics.Clear(BackColor);
				e.Graphics.FillRoundedRectangle(Gradient(back), new Rectangle(Point.Empty, Size), CornerRadius);
				DrawFocus(e.Graphics, new Rectangle(0, 0, Width - 2, Height - 2), CornerRadius);
			}
			else
			{
				e.Graphics.Clear(back);
				DrawFocus(e.Graphics, new Rectangle(0, 0, Width - 2, Height - 2), 0);
			}

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (Image != null)
			{
				e.Graphics.DrawImage(new Bitmap(Image, iconSize, iconSize).If(colorIcon, x => x.Color(fore)),
					DrawLeft ? new RectangleF(Padding.Left, (Height - iconSize) / 2F, iconSize, iconSize)
						: new RectangleF(Width - Padding.Right - IconSize, (Height - iconSize) / 2F, iconSize, iconSize));
			}

			var bnds = e.Graphics.Measure(Text, Font);
			var stl = new StringFormat()
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Near,
				Trimming = StringTrimming.EllipsisCharacter
			};

			e.Graphics.DrawString(Text,
				Font,
				Gradient(fore),
				new RectangleF(DrawLeft ? iconSize + Padding.Horizontal : Padding.Left, (Height - bnds.Height) / 2, Width - (iconSize + Padding.Horizontal + Padding.Left), bnds.Height),
				stl);
		}
	}
}