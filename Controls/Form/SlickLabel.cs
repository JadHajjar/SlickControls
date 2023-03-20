using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("Click")]
	public partial class SlickLabel : SlickImageControl
	{
		private Func<Color> activeColor;

		private bool center = false;

		protected int iconSize = 16;

		private bool selected = false;

		public SlickLabel()
		{
			InitializeComponent();
		}

		protected override void DesignChanged(FormDesign design) => Invalidate();

		protected override void UIChanged() => ResizeForAutoSize();

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Func<Color> ActiveColor { get => activeColor; set { activeColor = value; Invalidate(); } }

		[Category("Design")]
		public bool Center { get => center; set { center = value; Invalidate(); } }

		[Category("Appearance"), DefaultValue(false)]
		public bool Display { get; set; }

		[Category("Appearance")]
		public override Image Image
		{
			get => base.Image;
			set { base.Image = value; ResizeForAutoSize(); }
		}

		private bool hideText = false;

		[Category("Design")]
		public int IconSize { get => iconSize; set { iconSize = value; ResizeForAutoSize(); Invalidate(); } }

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		public override string Text
		{
			get => base.Text;
			set
			{
				base.Text = value;
				ResizeForAutoSize();
				SlickTip.SetTo(this, value);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		public override Font Font { get => base.Font; set => base.Font = value; }

		[Category("Design")]
		public bool HideText { get => hideText; set { hideText = value; ResizeForAutoSize(); Invalidate(); } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public bool Selected { get => selected; set { selected = value; Invalidate(); } }

		protected override void LocaleChanged()
		{
			ResizeForAutoSize();
		}

		protected virtual void GetColors(out Color fore, out Color back)
		{
			if (Selected || HoverState.HasFlag(HoverState.Pressed))
			{
				fore = ActiveColor == null ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ActiveForeColor.Tint(ActiveColor());
				back = ActiveColor == null ? FormDesign.Design.ActiveColor : ActiveColor();
			}
			else if (HoverState.HasFlag(HoverState.Hovered))
			{
				fore = FormDesign.Design.ForeColor.Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, -7, 7));
				back = FormDesign.Design.ButtonColor.MergeColor(BackColor, 75);
			}
			else
			{
				fore = Enabled || Display ? ForeColor : ForeColor.MergeColor(BackColor);
				back = BackColor;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			GetColors(out var fore, out var back);
			e.Graphics.Clear(BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (HoverState >= HoverState.Hovered || Selected)
				e.Graphics.FillRoundedRectangle(Gradient(back), new Rectangle(1, 1, Width - 3, Height - 3), 7);

			if (!HoverState.HasFlag(HoverState.Pressed))
				DrawFocus(e.Graphics, new Rectangle(1, 1, Width - 3, Height - 3), 7, ActiveColor == null ? FormDesign.Design.ActiveColor : ActiveColor());

			if (Loading)
			{
				if (HideText || string.IsNullOrWhiteSpace(Text))
					DrawLoader(e.Graphics, new Rectangle((Width - iconSize) / 2, (int)((Height - iconSize) / 2F), iconSize, iconSize), fore);
				else
					DrawLoader(e.Graphics, new Rectangle(Padding.Left, (int)((Height - iconSize) / 2F), iconSize, iconSize), fore);
			}
			else if ((DesignMode ? Image.SafeColor(fore) : Image.Color(fore)) != null)
			{
				if (HideText || string.IsNullOrWhiteSpace(Text))
					e.Graphics.DrawImage(Image, new Rectangle((Width - iconSize) / 2, (int)((Height - iconSize) / 2F), iconSize, iconSize));
				else
					e.Graphics.DrawImage(Image, new Rectangle(Padding.Left, (int)((Height - iconSize) / 2F), iconSize, iconSize));
			}

			if (!HideText && !string.IsNullOrWhiteSpace(Text))
			{
				var stl = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
				if (Image != null)
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), Font, Gradient(fore), new Rectangle(iconSize + 2 * Padding.Left, 0, Width - (iconSize + Padding.Left + Padding.Horizontal), Height), stl);
				else
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), Font, Gradient(fore), new Rectangle(Padding.Left, 0, Width - (Padding.Left), Height), stl);
			}
		}

		public void Hold()
		{
			Selected = true;
		}

		public void Release()
		{
			Selected = false;
		}

		public override Size GetPreferredSize(Size proposedSize) => GetAutoSize();

		private void ResizeForAutoSize()
		{
			try
			{
				if (AutoSize && IsHandleCreated)
					SetBoundsCore(Left, Top, Width, Height, BoundsSpecified.Size);
			}
			catch { }
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			ResizeForAutoSize();
		}

		public Size GetAutoSize()
		{
			using (var g = Graphics.FromHwnd(IntPtr.Zero))
			{
				var w = 3;
				var h = 0;

				if (Image != null)
					w += Padding.Left + iconSize;

				if (!string.IsNullOrWhiteSpace(Text) && !HideText)
				{
					var bnds = g.Measure(LocaleHelper.GetGlobalText(Text), Font);
					w += (int)bnds.Width + Padding.Horizontal;
					h = Math.Max(IconSize + Padding.Vertical, (int)bnds.Height + Padding.Vertical);
				}
				else
				{
					w += Padding.Right;
					h = IconSize + Padding.Vertical;
				}

				return new Size(w, h);
			}
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (AutoSize && (specified & BoundsSpecified.Size) != 0)
			{
				var size = GetAutoSize();

				width = size.Width;
				height = size.Height;
			}

			base.SetBoundsCore(x, y, width, height, specified);
		}
	}
}