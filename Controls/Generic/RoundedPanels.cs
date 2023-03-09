using Extensions;

using System.ComponentModel;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SlickControls
{
	public class RoundedPanel : DBPanel
	{
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			using (var brush = new SolidBrush(BackColor))
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
		}
	}

	public class RoundedTableLayoutPanel : DBTableLayoutPanel
	{
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			using (var brush = new SolidBrush(BackColor))
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
		}
	}

	public class RoundedFlowLayoutPanel : DBFlowLayoutPanel
	{
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			using (var brush = new SolidBrush(BackColor))
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
		}
	}

	public class RoundedGroupBox : DBPanel
	{
		private Image image;

		[Category("Appearance"), DefaultValue(null)]
		public virtual Image Image { get => image; set { image = value; UIChanged(); } }

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		public RoundedGroupBox()
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
			var padding = UI.Scale(new Padding(5), UI.FontScale);

			using (var g = CreateGraphics())
			{
				var iconWidth = Image?.Width ?? 16;
				var titleHeight = padding.Top + Math.Max(iconWidth, (int)g.Measure(Text, UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);
				padding.Top +=  titleHeight;
			}

			Padding = padding;
			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			using (var brush = new SolidBrush(BackColor == Parent?.BackColor ? FormDesign.Design.ButtonColor : BackColor))
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);

			var iconWidth = Image?.Width ?? 16;
			var titleHeight = Math.Max(iconWidth, (int)e.Graphics.Measure(Text, UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);
			var iconRectangle = new Rectangle(Padding.Left, Padding.Bottom + ((titleHeight - iconWidth) / 2), iconWidth, iconWidth);

			if (Image != null)
				using (var icon = new Bitmap(Image))
					e.Graphics.DrawImage(icon.Color(ForeColor), iconRectangle);

			e.Graphics.DrawString(Text, UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), new SolidBrush(ForeColor), new Rectangle(iconWidth + (Padding.Left * 2), Padding.Bottom, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });
		}
	}
}