using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("TabSelected")]
	public partial class SlickTab : SlickControl, IAnimatable
	{
		[Category("Action")]
		public event EventHandler TabSelected;

		private bool hovered;
		private bool selected;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get; set; }

		[Category("Behavior"), DisplayName("Linked Control")]
		public Control LinkedControl { get; set; }

		[Category("Appearance")]
		public Image Icon { get; set; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int AnimatedValue { get; set; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int TargetAnimationValue => (Selected || Hovered) ? 90 : 0;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Hovered
		{
			get => hovered;
			set
			{
				hovered = value;

				if (!IsHandleCreated || DesignMode)
					AnimatedValue = TargetAnimationValue;
				else if (AnimatedValue != TargetAnimationValue)
					AnimationHandler.Animate(this);

				Invalidate();
			}
		}

		[Category("Behavior")]
		public bool Selected
		{
			get => selected;
			set
			{
				if (value && Parent != null)
				{
					foreach (var item in Parent.Controls.ThatAre<SlickTab>())
						item.Selected = false;
				}

				selected = value;

				if (!IsHandleCreated || DesignMode)
					AnimatedValue = TargetAnimationValue;
				else if (AnimatedValue != TargetAnimationValue)
					AnimationHandler.Animate(this);

				Invalidate();

				if (value)
					TabSelected?.Invoke(this, new EventArgs());
			}
		}

		public SlickTab()
		{
			InitializeComponent();
			DoubleBuffered = ResizeRedraw = true;
			TabStop = false;
		}

		protected override void UIChanged()
		{
			Font = UI.Font(8.25F);

			if (!(Parent?.Parent is SlickTabControl))
				Size = new Size((int)(DefaultSize.Width * UI.UIScale), Font.Height + 4);
		}

		private void SlickTab_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (AnimatedValue != 0)
				e.Graphics.FillRectangle(new SolidBrush(Selected ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), (Width - (Width * AnimatedValue / 100)) / 2, Height - 1, (Width * AnimatedValue / 100), 1);

			var img = Icon?.Color(Selected ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor);

			if (Width > 100)
			{
				var bnds = e.Graphics.MeasureString(Text, Font);
				e.Graphics.DrawString(Text, Font, new SolidBrush(Selected ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), (Width - bnds.Width - (img == null ? 0 : img.Width + 3)) / 2 + (img == null ? 0 : img.Width + 3), (Height - bnds.Height) / 2);

				if (img != null)
					e.Graphics.DrawImage(img, new Rectangle((int)(Width - bnds.Width - (img == null ? 0 : img.Width + 3)) / 2, (Height - img.Height) / 2, img.Width, img.Height));
			}
			else if (img != null)
				e.Graphics.DrawImage(img, new Rectangle(Size.Center(img.Size), img.Size));
		}

		private void SlickTab_MouseEnter(object sender, EventArgs e) => Hovered = true;

		private void SlickTab_MouseLeave(object sender, EventArgs e) => Hovered = false;

		private void SlickTab_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !Selected)
				Selected = true;
		}
	}
}