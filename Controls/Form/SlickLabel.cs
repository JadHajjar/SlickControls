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
			e.Graphics.SetUp(BackColor);

			if (HoverState >= HoverState.Hovered || Selected)
				e.Graphics.FillRoundedRectangle(Gradient(back), new Rectangle(1, 1, Width - 3, Height - 3), 7);

			if (!HoverState.HasFlag(HoverState.Pressed))
				DrawFocus(e.Graphics, new Rectangle(1, 1, Width - 3, Height - 3), 7, ActiveColor == null ? FormDesign.Design.ActiveColor : ActiveColor());

			var iconSize = Image?.Width ?? 16;
			var iconRect = (HideText || string.IsNullOrWhiteSpace(Text)) ? ClientRectangle.CenterR(Image?.Size ?? new Size(iconSize, iconSize)) : ClientRectangle.Pad(Padding).Align(Image?.Size ?? new Size(iconSize, iconSize), ContentAlignment.MiddleLeft);
			var textRect = ClientRectangle.Pad(Padding).Pad(Image?.Width ?? (Loading ? iconSize:0),0,0,0);

			if (Loading)
			{
				DrawLoader(e.Graphics, iconRect, fore);
			}
			else if (Image != null)
			{
				using (var icon = new Bitmap(Image).Color(fore))
					e.Graphics.DrawImage(icon, iconRect);
			}

			if (!HideText && !string.IsNullOrWhiteSpace(Text))
			{
				var stl = new StringFormat { LineAlignment = StringAlignment.Center };

				//TextRenderer.DrawText(e.Graphics, LocaleHelper.GetGlobalText(Text), Font, textRect, fore, TextFormatFlags.VerticalCenter);
				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), Font, Gradient(fore), textRect, stl);
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

		private double lastUiScale;
		private string lastText;
		private Size lastAvailableSize;
		private Size lastSize;

		public override Size GetPreferredSize(Size proposedSize)
		{
			return GetAutoSize();
		}

		private Size GetAutoSize()
		{
			if (!Live || Anchor == (AnchorStyles)15 || Dock == DockStyle.Fill || (string.IsNullOrWhiteSpace(Text) && Image == null))
				return Size;

			var availableSize = GetAvailableSize();

			if (lastUiScale == UI.FontScale && lastText == Text && (availableSize == lastAvailableSize || availableSize.Width <= 0))
				return lastSize;

			var IconSize = Image?.Width ?? 16;

			if (string.IsNullOrWhiteSpace(Text) || (AutoSize && availableSize.Width.IsWithin(0, (int)(64 * UI.FontScale))))
			{
				lastUiScale = UI.FontScale;
				lastText = Text;
				lastAvailableSize = availableSize;

				if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
				{
					return lastSize = new Size(Height, Height);
				}
				else if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
				{
					return lastSize = new Size(Width, Width);
				}
				else
				{
					var pad = Math.Max(Padding.Horizontal, Padding.Vertical);

					return lastSize = new Size(IconSize + pad, IconSize + pad);
				}
			}

			var extraWidth = (Image == null ? 0 : (IconSize + Padding.Left)) + (int)(3 * UI.FontScale) + Padding.Horizontal;
			var bnds = FontMeasuring.Measure(LocaleHelper.GetGlobalText(Text), Font, availableSize.Width - extraWidth);
			var h = Math.Max(IconSize + 6, (int)(bnds.Height) + Padding.Top + 3);
			var w = (int)Math.Ceiling(bnds.Width) + extraWidth;

			if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
				h = Height;

			if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
				w = Width;

			if (w > availableSize.Width)
				w = availableSize.Width;

			if (h > availableSize.Height)
				h = availableSize.Height;

			w = w.Between(MinimumSize.Width, MaximumSize.Width.If(0, w));
			h = h.Between(MinimumSize.Height, MaximumSize.Height.If(0, h));

			lastUiScale = UI.FontScale;
			lastText = Text;
			lastAvailableSize = availableSize;

			return lastSize = new Size(w, h);
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