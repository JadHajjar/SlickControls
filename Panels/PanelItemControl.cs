using Extensions;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class PanelItemControl : SlickPictureBox
	{
		public PanelItem PanelItem { get; private set; }
		public bool Selected { get => selected; set { selected = value; this.TryInvoke(Invalidate); } }

		private bool selected;

		private bool _hovered = false;
		private bool _pressed = false;

		internal static bool DrawText { get; set; } = true;

		public PanelItemControl(PanelItem item)
		{
			InitializeComponent();
			Dock = DockStyle.Top;
			PanelItem = item;
			item.Control = this;
			UserDraw = true;

			UI.UIChanged += UIChanged;
			UIChanged();
		}

		protected void UIChanged()
		{
			Font = UI.Font(8.25F);
			Size = new Size((int)(165 * UI.UIScale), Font.Height + 10);
		}

		private void PanelItemControl_Paint(object sender, PaintEventArgs e)
		{
			var back = FormDesign.Design.MenuColor;
			var fore = FormDesign.Design.MenuForeColor;

			if (_hovered || PanelItem.Highlighted)
				back = FormDesign.Design.MenuColor.Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, 10, -5));

			if (PanelItem.Highlighted)
			{
				back = back.MergeColor(FormDesign.Design.ActiveColor, 30);
				fore = FormDesign.Design.ActiveForeColor;
			}
			else if (Selected)
			{
				fore = FormDesign.Design.ActiveColor;
			}
			else if (_pressed)
			{
				back = FormDesign.Design.ActiveColor;
				fore = FormDesign.Design.ActiveForeColor;
			}

			e.Graphics.FillRectangle(SlickControl.Gradient(new Rectangle(Point.Empty, Size), back, HoverState != HoverState.Normal ? 1F : 0F), new Rectangle(Point.Empty, Size));

			if (Selected)
				e.Graphics.FillRectangle(SlickControl.Gradient(new Rectangle(Point.Empty, Size), FormDesign.Design.ActiveColor), new Rectangle(Width - (int)(2 * UI.UIScale), 0, (int)(2 * UI.UIScale), Height));

			if (PanelItem.Highlighted)
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveForeColor), 2F) { DashStyle = DashStyle.Dash }, new Rectangle(1, 1, Width - 2 - (Selected ? (int)(2 * UI.UIScale) : 0), Height - 2));

			var bnds = e.Graphics.MeasureString(PanelItem.Text, Font);

			if (Loading)
				DrawLoader(e.Graphics, new Rectangle(15, (Height - 16) / 2, 16, 16), _pressed ? (Color?)fore : null);
			else if (PanelItem.Icon != null)
				e.Graphics.DrawImage(new Bitmap(PanelItem.Icon, 16, 16).Color(fore), 15, (Height - 16) / 2);

			if (DrawText)
			{
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

				e.Graphics.DrawString(PanelItem.Text, Font, SlickControl.Gradient(new Rectangle(Point.Empty, Size), fore), 15 + 16 + 5, (Height - bnds.Height) / 2);
			}
		}

		private void PanelItemControl_MouseClick(object sender, MouseEventArgs e) => PanelItem.MouseClick(e);

		private void PanelItemControl_MouseDown(object sender, MouseEventArgs e)
		{
			_pressed = true;
			Invalidate();
		}

		private void PanelItemControl_MouseEnter(object sender, EventArgs e)
		{
			_hovered = true;
			Invalidate();
		}

		private void PanelItemControl_MouseLeave(object sender, EventArgs e)
		{
			_hovered = false;
			Invalidate();
		}

		private void PanelItemControl_MouseUp(object sender, MouseEventArgs e)
		{
			_pressed = false;
			Invalidate();
		}
	}
}