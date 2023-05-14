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
		public override string Text { get => base.Text; set { base.Text = value; Invalidate(); } }

		[Category("Behavior"), DisplayName("Linked Control")]
		public Control LinkedControl { get; set; }

		[Category("Appearance"), DefaultValue(null)]
		public Image Icon { get; set; }

		[Category("Appearance"), DisplayName("Icon Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
		public DynamicIcon IconName { get; set; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int AnimatedValue { get; set; } = 15;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int TargetAnimationValue => (Selected || Hovered) ? 100 : 15;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Hovered
		{
			get => hovered;
			set
			{
				hovered = value;

				if (!IsHandleCreated || DesignMode)
				{
					AnimatedValue = TargetAnimationValue;
				}
				else if (AnimatedValue != TargetAnimationValue)
				{
					AnimationHandler.Animate(this, 1.25);
				}

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
					{
						item.Selected = false;
					}
				}

				selected = value;

				if (!IsHandleCreated || DesignMode)
				{
					AnimatedValue = TargetAnimationValue;
				}
				else if (AnimatedValue != TargetAnimationValue)
				{
					AnimationHandler.Animate(this, 1.25);
				}

				Invalidate();

				if (value)
				{
					TabSelected?.Invoke(this, new EventArgs());
				}
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
			{
				Size = UI.Scale(new Size(DefaultSize.Width, 32), UI.FontScale);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SetUp(BackColor);

			e.Graphics.FillRoundedRectangle(ClientRectangle.Gradient(Selected || Hovered ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor, 0.007F * AnimatedValue),
				ClientRectangle.Pad((int)(5 * UI.FontScale)).Pad(1, 1, 1, 0), (int)(4 * UI.FontScale));

			e.Graphics.FillRoundedRectangle(ClientRectangle.Gradient((Selected ? FormDesign.Design.ActiveColor : FormDesign.Design.ButtonColor).MergeColor(BackColor, AnimatedValue), 0.007F * AnimatedValue),
				ClientRectangle.Pad((int)(5 * UI.FontScale)).Pad(0, 0, 0, 1), (int)(4 * UI.FontScale));

			var fore = Selected ? FormDesign.Design.ActiveForeColor.MergeColor(FormDesign.Design.ForeColor, AnimatedValue) : FormDesign.Design.ForeColor;

			using (var img = (Icon != null ? new Bitmap(Icon) : IconName)?.Color(fore))
			{
				if (Width > (int)(120 * UI.FontScale))
				{
					var width = (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), Font).Width + (img?.Width ?? 0) + (int)(10 * UI.FontScale);
					var bounds = ClientRectangle.CenterR(width, Height);

					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), Font, new SolidBrush(fore)
						, img != null ? bounds.Pad(img.Width + (int)(5 * UI.FontScale), 0, 0, 0) : bounds, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

					if (img != null)
					{
						e.Graphics.DrawImage(img, bounds.Align(img.Size, ContentAlignment.MiddleLeft));
					}
				}
				else if (img != null)
				{
					e.Graphics.DrawImage(img, ClientRectangle.CenterR(img.Size));
				}
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			Hovered = true;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			Hovered = false;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Left && !Selected)
			{
				Selected = true;
			}
		}
	}
}