using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace SlickControls
{
	public enum StyleType { Vertical, Horizontal }

	[DefaultEvent("Scroll")]
	public partial class SlickScroll : Control
	{
		public event ScrollEventHandler Scroll;

		public int BAR_SIZE_MAX => (int)((SmallHandle ? 7 : 11) * UI.UIScale).If(x => x % 2 == 0, x => x - 1, x => x);
		public int BAR_SIZE_MIN => (int)(5 * UI.UIScale);

		private static readonly List<SlickScroll> activeScrolls = new List<SlickScroll>();
		private readonly Timer ScrollTimer = new Timer { Interval = 14 };
		private AnimationHandler animationHandler;
		private Control linkedControl;
		private Point mouseDownLocation;
		private bool mouseIn;
		private double speedModifier;
		private double targetPercentage;
		private bool Live;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Active => linkedControl?.Parent != null
			&& ControlSize > linkedControl.Parent.Height
			&& linkedControl.Parent.Height > 1;

		[Category("Behavior"), DefaultValue(false), DisplayName("Auto Size Source")]
		public bool AutoSizeSource { get; set; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Func<int> CustomSizeSource { get; set; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsMouseDown { get; private set; }

		[Category("Data")]
		public Control LinkedControl
		{
			get => linkedControl;
			set
			{
				linkedControl = value;

				if (value != null && Live)
				{
					linkedControl.Location = Point.Empty;
					linkedControl.MouseWheel += SlickScroll_OnMouseWheel;
					linkedControl.Resize += LinkedControl_Resize;
					linkedControl.VisibleChanged += LinkedControl_Resize;
					linkedControl.ParentChanged += (s, e) => SetParentEvents();
					SetParentEvents();
					LinkedControl_Resize(null, null);
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MouseDetector MouseDetector { get; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point MouseDownLocation { get => mouseDownLocation; set => mouseDownLocation = value; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double Percentage { get; private set; } = 0;

		[Category("Behavior"), DefaultValue(true)]
		public bool ShowHandle { get; set; } = true;

		[Category("Behavior"), DefaultValue(false)]
		public bool SmallHandle { get; set; }

		[Category("Behavior")]
		public StyleType Style { get; set; }

		[Category("Behavior"), DefaultValue(true)]
		public bool TakeUpSpaceFromPanel { get; set; } = true;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double TargetPercentage
		{
			get => targetPercentage;
			private set
			{
				if (value < 0.01)
				{
					targetPercentage = 0;
				}
				else if (value > 99.99)
				{
					targetPercentage = 100;
				}
				else
				{
					targetPercentage = value;
				}

				if (AnimationHandler.NoAnimations || DesignMode)
				{
					SetPercentage(targetPercentage);
				}
				else if (Percentage != targetPercentage)
				{
					speedModifier = speedModifier.If(0, 10, speedModifier / 1.05);
					ScrollTimer.Stop();
					ScrollTimer.Start();
				}
			}
		}

		public static bool IsAnimating
		{
			get
			{
				lock (activeScrolls)
					return activeScrolls.Any(x => x.ScrollTimer.Enabled);
			}
		}

		private Rectangle Bar => new Rectangle(
			0,
			Padding.Top + (int)(Percentage * ((Height - Padding.Vertical) - ((Height - Padding.Vertical) * (linkedControl?.Parent?.Height ?? 0) / ControlSize.If(0, 1))) / 100),
			Width - 4,
			(Height - Padding.Vertical) * (linkedControl?.Parent?.Height ?? 0) / ControlSize.If(0, 1));

		private int ControlSize
		{
			get
			{
				if (CustomSizeSource != null)
				{
					return CustomSizeSource();
				}

				if (!AutoSizeSource)
				{
					return linkedControl?.Height ?? 0;
				}

				if (linkedControl != null)
				{
					return getControlHeight(linkedControl) - linkedControl.Top;
				}

				return 0;
			}
		}

		private bool Open => Width != BAR_SIZE_MIN;

		private float PenSize => BAR_SIZE_MIN - 3 + (Open && Dock == DockStyle.None ? 1 : 0);

		public SlickScroll()
		{
			InitializeComponent();
			ResizeRedraw = DoubleBuffered = true;

			MouseWheel += SlickScroll_OnMouseWheel;
			ScrollTimer.Tick += ScrollTimer_Elapsed;

			if (Live)
			{
				MouseDetector = new MouseDetector();
				MouseDetector.MouseMove += mouseDetector_MouseMove;
			}

			lock (activeScrolls)
			{
				activeScrolls.Add(this);
			}
		}

		public static void GlobalScrollTo(Control control, int intensity = 1)
		{
			var p = control;

			while (p != null)
			{
				SlickScroll scroll;
				lock (activeScrolls)
				{
					scroll = activeScrolls.FirstOrDefault(x => x.linkedControl == p);
				}

				if (scroll != null)
				{
					if (scroll.Active)
					{
						scroll.ScrollTo(control, intensity);
					}

					return;
				}

				p = p.Parent;
			}
		}

		public void Reset()
		{
			if (Parent != null)
				Height = Parent.Height;
			SetPercentage(0, true);
			LinkedControl_Resize(null, null);
		}

		public void ScrollTo(double perc, int intensity = 1)
		{
			speedModifier = 0;
			TargetPercentage = perc;

			for (var i = 1; i < intensity - 1; i++)
			{
				speedModifier = speedModifier.If(0, 10, speedModifier / 1.05);
			}
		}

		public void ScrollTo(Control control, int intensity = 1)
		{
			var h = 0;

			while (control != LinkedControl)
			{
				h += control.Top;
				control = control.Parent;
			}

			ScrollTo(-100 * h / (linkedControl.Parent.Height - ControlSize).If(0, 1), intensity);
		}

		public void SetPercentage(Control control, bool target = false)
		{
			var h = 0;

			while (control != LinkedControl)
			{
				h += control.Top;
				control = control.Parent;
			}

			SetPercentage(-100 * h / ((linkedControl?.Parent?.Height ?? 0) - ControlSize).If(0, 1), target);
		}

		public void SetPercentage(double perc, bool target = false)
		{
			Percentage = perc.Between(0, 100);

			if (target || AnimationHandler.NoAnimations)
			{
				ScrollTimer.Stop();
				speedModifier = 0;
				targetPercentage = Percentage;
			}

			if (LinkedControl != null)
			{
				LinkedControl.Top = (int)(Percentage * ((linkedControl.Parent?.Height ?? 0) - ControlSize).If(0, 1) / 100);
			}

			Invalidate();

			Scroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, Percentage.IsWithin(0, 1) ? 1 : Percentage.IsWithin(99, 100) ? 100 : (int)Percentage));
		}

		public void TriggerMouseWheel(MouseEventArgs e)
		{
			SlickScroll_OnMouseWheel(this, e);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (Style == StyleType.Vertical)
			{
				Width = BAR_SIZE_MIN;
			}
			else
			{
				Height = BAR_SIZE_MIN;
			}

			TabStop = false;
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			IsMouseDown = mouseIn = false;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (Live = !DesignMode)
			{
				LinkedControl = LinkedControl;
				BeginInvoke(new Action(Reset));
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			var smallX = Dock == DockStyle.None ? 0 : Width - BAR_SIZE_MIN;

			if (SmallHandle || !Active || DesignMode)
			{
				e.Graphics.DrawLine(new Pen(Color.FromArgb(Open ? (IsMouseDown ? 170 : 85) : 40, FormDesign.Design.AccentColor), PenSize), smallX, Padding.Top, smallX, Height - Padding.Bottom);
			}
			else
			{
				e.Graphics.DrawLine(new Pen(Color.FromArgb(Open ? (IsMouseDown ? 170 : 85) : 40, FormDesign.Design.AccentColor), PenSize), Bar.Width / 2, Padding.Top, Bar.Width / 2, Height - Padding.Bottom);
			}

			if (!Active || DesignMode)
			{
				return;
			}

			if (Open && !SmallHandle)
			{
				var w = Math.Min(18, Bar.Width).If(x => x % 2 != 0, x => x - 1, x => x);

				var barRect = new Rectangle(Bar.Width / 2 - w / 2, Bar.Top, w, Bar.Height);

				if (barRect.Height > 0 && barRect.Width > 0)
				{
					e.Graphics.FillRoundedRectangle(barRect.Gradient(IsMouseDown ? FormDesign.Design.ActiveColor : BackColor.MergeColor(FormDesign.Design.AccentColor), 1F),
						barRect,
						Bar.Height < w ? 1 : w / 2);
				}
			}
			else
			{
				if (SmallHandle)
				{
					e.Graphics.DrawLine(new Pen(IsMouseDown ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor, PenSize), smallX, Bar.Top, smallX, Bar.Top + Bar.Height);
				}
				else
				{
					e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor, Bar.Width), 0, Bar.Top, 0, Bar.Top + Bar.Height);
				}
			}
		}

		private void Dismiss()
		{
			if (!IsMouseDown && !mouseIn)
			{
				animationHandler?.Dispose();
				animationHandler = new AnimationHandler(this, new Size(BAR_SIZE_MIN, 0), AnimationOption.IgnoreHeight) { Speed = .5 };
				animationHandler.StartAnimation();
			}
		}

		private int getControlHeight(Control c)
		{
			return c.Visible ? (c.Top + c.Margin.Vertical +
				((c.Controls.Count == 0 || !(c is Panel))
				? c.Height
				: c.Padding.Bottom + c.Controls.Max(getControlHeight))) : 0;
		}

		private void LinkedControl_Resize(object sender, EventArgs e)
		{
			if (DesignMode || linkedControl?.Parent == null || (linkedControl.FindForm()?.WindowState ?? FormWindowState.Minimized) == FormWindowState.Minimized)
			{
				return;
			}

			linkedControl.MaximumSize = new Size(linkedControl.Parent.Width - (linkedControl.Parent == Parent && TakeUpSpaceFromPanel ? BAR_SIZE_MAX : 0), int.MaxValue);
			linkedControl.MinimumSize = new Size(linkedControl.Parent.Width - (linkedControl.Parent == Parent && TakeUpSpaceFromPanel ? BAR_SIZE_MAX : 0), 0);

			foreach (var item in linkedControl.Controls.OfType<SlickSectionPanel>().Where(x => x.Dock == DockStyle.Top))
			{
				item.MaximumSize = linkedControl.MaximumSize;
				item.MinimumSize = new Size(linkedControl.MinimumSize.Width, item.MinimumSize.Height);
			}

			Visible = ShowHandle && Active;

			SetPercentage(Active ? Percentage : 0, !ScrollTimer.Enabled);

			Invalidate();
		}

		private void mouseDetector_MouseMove(object sender, Point p)
		{
			if (IsDisposed) return;

			if (new Rectangle(PointToScreen(Point.Empty), Size).Pad((int)(-22 * UI.UIScale)).Contains(p))
			{
				if (!mouseIn && ShowHandle)
				{
					mouseIn = true;
					animationHandler = new AnimationHandler(this, new Size((int)(BAR_SIZE_MAX * UI.UIScale), 0), AnimationOption.IgnoreHeight) { Speed = .5 };
					animationHandler.StartAnimation();
				}
			}
			else
			{
				mouseIn = false;
				Dismiss();
			}
		}

		private void ScrollTimer_Elapsed(object sender, EventArgs e)
		{
			if (Active)
			{
				//var newValues = linkedControl.Top + getStep((targetPercentage * ((linkedControl.Parent?.Height ?? 0) - ControlSize) / 100) - linkedControl.Top);

				//int getStep(double diff)
				//{
				//	if (diff == 0) return 0;

				//	var sign = diff.Sign();
				//	var x = Math.Abs(diff) + 500;

				//	return sign * (int)(1.5 * (15-speedModifier) * (x - 450 - (x * x / 50000)) / (8 * Math.Sqrt(x))).Between(1, Math.Abs(diff));
				//}

				//SetPercentage(100D * newValues / ((linkedControl.Parent?.Height ?? 0) - ControlSize));

				var size = ControlSize - linkedControl.Parent.Height;
				var incPerc = (TargetPercentage - Percentage) > 0;
				var minStep = 750D / size * speedModifier / 2;
				var maxStep = Math.Max(minStep, ControlSize / (75D / speedModifier) / 2);
				var perc = (TargetPercentage - Percentage).Between(incPerc.If(minStep, -maxStep), incPerc.If(maxStep, -minStep)) / speedModifier.If(0, 1);

				SetPercentage((Percentage + perc).Between(incPerc ? 0 : TargetPercentage, incPerc ? TargetPercentage : 100));

				if (Percentage == TargetPercentage)
				{
					ScrollTimer.Stop();
					speedModifier = 0;
				}
			}
			else
			{
				ScrollTimer.Stop();
				SetPercentage(0, true);
			}
		}

		private void SetParentEvents()
		{
			if (linkedControl?.Parent != null)
			{
				linkedControl.Parent.MouseWheel += SlickScroll_OnMouseWheel;
				linkedControl.Parent.Resize += LinkedControl_Resize;
			}
		}

		private void SlickScroll_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsMouseDown = true;
				if (!e.Y.IsWithin(Bar.Top, Bar.Top + Bar.Height))
				{
					SetPercentage(100D * (e.Y - (Bar.Height / 2) - Padding.Top).Between(0, Height - Padding.Vertical) / (Height - Padding.Vertical - Bar.Height), true);
					MouseDownLocation = new Point(e.X - Bar.Left, Bar.Height / 2);
				}
				else
				{
					MouseDownLocation = new Point(e.X - Bar.Left, e.Y - Bar.Top);
				}

				Invalidate();
			}
		}

		private void SlickScroll_MouseMove(object sender, MouseEventArgs e)
		{
			if (Live)
			{
				if (e.Button == MouseButtons.Left)
				{
					SetPercentage(100D * (e.Y - MouseDownLocation.Y - Padding.Top).Between(0, Height - Padding.Vertical) / (Height - Padding.Vertical - Bar.Height), true);
				}

				Cursor = e.Y.IsWithin(Bar.Top, Bar.Top + Bar.Height) ? Cursors.Hand : Cursors.Default;
			}
		}

		private void SlickScroll_MouseUp(object sender, MouseEventArgs e)
		{
			if (Live)
			{
				IsMouseDown = false;
				Dismiss();
				Invalidate();
			}
		}

		private void SlickScroll_OnMouseWheel(object sender, MouseEventArgs e)
		{
			if (e is HandledMouseEventArgs h && h.Handled)
				return;

			if (Active && !IsMouseDown && Live)
			{
				TargetPercentage -= e.Delta * 100D / (ControlSize - linkedControl.Parent.Height);

				if (e is HandledMouseEventArgs handle)
				{
					handle.Handled = true;
				}
			}
		}
	}
}