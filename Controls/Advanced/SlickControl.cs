using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickControl : UserControl, IHoverControl
	{
		public event Extensions.EventHandler<HoverState> HoverStateChanged;

		private readonly Timer timer = new Timer();
		private bool loading;
		private static bool focusFromTab;
		private HoverState hoverState = HoverState.Normal;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double LoaderPercentage { get; private set; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual HoverState HoverState { get => hoverState; protected set { hoverState = value; OnHoverStateChanged(); } }

		[DefaultValue(true), Category("Behavior"), DisplayName("Auto Invalidate")]
		public bool AutoInvalidate { get; set; } = true;

		[DefaultValue(true), Category("Behavior"), DisplayName("Enter Triggers Click")]
		public bool EnterTriggersClick { get; set; } = true;

		[DefaultValue(false), Category("Behavior"), DisplayName("Space Triggers Click")]
		public bool SpaceTriggersClick { get; set; }

		[DefaultValue(true), Category("Behavior"), DisplayName("Scroll To On Focus")]
		public bool ScrollToOnFocus { get; set; } = true;

		[DefaultValue(false), Category("Behavior")]
		public bool Loading
		{
			get => loading;
			set
			{
				loading = value;

				if (Live)
				{
					if (!loading)
					{
						LoaderPercentage = 0;
					}

					this.TryInvoke(() =>
					{
						timer.Enabled = loading && Visible && Parent != null;
						Invalidate();
					});
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Live { get; private set; }

		public SlickControl()
		{
			AutoScaleMode = AutoScaleMode.None;
			DoubleBuffered = ResizeRedraw = true;
			timer = new Timer { Interval = 30 };
			timer.Tick += timer_Tick;

			FormDesign.DesignChanged += DesignChanged;
			UI.UIChanged += UIChanged;
			LocaleHelper.LanguageChanged += LocaleChanged;
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			LoaderPercentage += 1 + (Math.Abs(50 - LoaderPercentage) / 25);
			if (LoaderPercentage >= 200)
			{
				LoaderPercentage = 0;
			}

			if (!loading)
			{
				timer.Enabled = false;
			}

			Invalidate();
		}

		public void DrawLoader(Graphics g, Rectangle rectangle, Color? color = null)
		{
			g.DrawLoader(LoaderPercentage, rectangle, color);
		}

		protected void DrawFocus(Graphics g, Color? color = null)
		{
			DrawFocus(g, new Rectangle(0, 0, Width - 2, Height - 2), 7, color);
		}

		protected void DrawFocus(Graphics g, Rectangle rectangle, int border, Color? color = null)
		{
			DrawFocus(g, rectangle, HoverState, border, color);
		}

		public static void DrawFocus(Graphics g, Rectangle rectangle, HoverState state, int border, Color? color = null)
		{
			if (state.HasFlag(HoverState.Focused))
			{
				color = color ?? FormDesign.Design.ActiveColor;
				var sm = g.SmoothingMode;

				g.SmoothingMode = SmoothingMode.HighQuality;

				using (var brush = new SolidBrush(Color.FromArgb(25, color.Value)))
				{
					g.FillRoundedRectangle(brush, rectangle, border);
				}

				using (var pen = new Pen(Color.FromArgb(100, color.Value), 1.5F) { DashStyle = DashStyle.Dash })
				{
					g.DrawRoundedRectangle(pen, rectangle, border);
				}

				g.SmoothingMode = sm;
			}
		}

		protected virtual void UIChanged()
		{
			Invalidate();
		}

		protected virtual void DesignChanged(FormDesign design)
		{
			Invalidate();
		}

		protected virtual void LocaleChanged()
		{
			Invalidate();
		}

		protected virtual void OnHoverStateChanged()
		{
			HoverStateChanged?.Invoke(this, hoverState);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			Live = !DesignMode;

			if (Live)
			{
				DesignChanged(FormDesign.Design);

				UIChanged();

				LocaleChanged();

				if (loading)
				{
					Loading = true;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			HoverState |= HoverState.Hovered;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			HoverState &= ~HoverState.Hovered;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			HoverState |= HoverState.Pressed;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			HoverState &= ~HoverState.Pressed;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnMouseUp(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			HoverState &= ~HoverState.Focused;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			base.OnLeave(e);
		}

		protected override void OnEnter(EventArgs e)
		{
			HoverState |= HoverState.Focused;

			if (AutoInvalidate)
			{
				Invalidate();
			}

			if (focusFromTab && ScrollToOnFocus)
			{
				focusFromTab = false;
				SlickScroll.GlobalScrollTo(this);
			}

			base.OnEnter(e);
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);

			timer.Enabled = loading && Visible && Parent != null;
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			timer.Enabled = loading && Visible && Parent != null;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (HoverState.HasFlag(HoverState.Focused) && (
				(EnterTriggersClick && keyData == Keys.Enter) ||
				(SpaceTriggersClick && keyData == Keys.Space)))
			{
				OnMouseClick(new MouseEventArgs(MouseButtons.None, 1, 0, 0, 0));
				OnClick(EventArgs.Empty);
				return true;
			}

			if (Focused && (focusFromTab = keyData == Keys.Tab || keyData == (Keys.Shift | Keys.Tab)))
			{
				BeginInvoke(new Action(() =>
				{
					if (Focused)
					{
						PanelContent.GetParentPanel(this)?.ResetFocus();
					}
				}));
			}

			if (!focusFromTab && (focusFromTab = keyData == Keys.Tab || keyData == (Keys.Shift | Keys.Tab)))
			{
				BeginInvoke(new Action(() => focusFromTab = false));
			}

			if (keyData == Keys.Escape && HoverState.HasFlag(HoverState.Focused))
			{
				HoverState &= ~HoverState.Focused;

				if (AutoInvalidate)
				{
					Invalidate();
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void Dispose(bool disposing)
		{
			try
			{ base.Dispose(disposing); }
			catch { }

			if (disposing)
			{
				timer?.Dispose();
				FormDesign.DesignChanged -= DesignChanged;
				UI.UIChanged -= UIChanged;
				LocaleHelper.LanguageChanged -= LocaleChanged;
			}
		}

		protected Size GetAvailableSize()
		{
			// Calculate the maximum available space for autosizing the control
			var availableWidth = 0;
			var availableHeight = 0;
			var parent = Parent;

			if (parent == null)
			{
				return Size;
			}

			// Check if the control is inside a TableLayoutPanel
			if (parent is TableLayoutPanel tableLayoutPanel)
			{
				var cellPosition = tableLayoutPanel.GetCellPosition(this);
				var columnSpan = tableLayoutPanel.GetColumnSpan(this);
				var rowSpan = tableLayoutPanel.GetRowSpan(this);

				var columnWidths = tableLayoutPanel.GetColumnWidths();
				var rowHeights = tableLayoutPanel.GetRowHeights();

				for (var i = cellPosition.Column; i < cellPosition.Column + columnSpan; i++)
				{
					if (tableLayoutPanel.ColumnStyles[i].SizeType == SizeType.AutoSize)
					{
						availableWidth = int.MaxValue;
						break;
					}

					availableWidth += columnWidths[i];
				}

				for (var i = cellPosition.Row; i < cellPosition.Row + rowSpan; i++)
				{
					if (tableLayoutPanel.RowStyles[i].SizeType == SizeType.AutoSize)
					{
						availableHeight = int.MaxValue;
						break;
					}

					availableHeight += rowHeights[i];
				}

				availableWidth -= Margin.Horizontal;
				availableHeight -= Margin.Vertical;
			}
			else
			{
				availableWidth = parent.ClientSize.Width;
				availableHeight = parent.ClientSize.Height;

				if (Dock == DockStyle.Fill || Dock == DockStyle.Bottom || Dock == DockStyle.Top)
				{
					availableWidth -= parent.Padding.Horizontal;
				}

				if (Dock == DockStyle.Fill || Dock == DockStyle.Left || Dock == DockStyle.Right)
				{
					availableHeight -= parent.Padding.Vertical;
				}
			}

			// Set the maximum available space for the control
			return new Size(availableWidth, availableHeight);
		}

		protected Brush Gradient(Color color, float caliber = 1F)
		{
			return Gradient(new Rectangle(Point.Empty, Size), color, caliber);
		}

		protected Brush Gradient(Color color, Rectangle rectangle, float caliber = 1F)
		{
			return Gradient(rectangle, color, caliber);
		}

		public static Brush Gradient(Rectangle rect, Color color, float caliber = 1F)
		{
			return rect.Gradient(color, caliber);
		}
	}
}