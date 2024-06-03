using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public enum StyleType { Vertical, Horizontal }

[DefaultEvent("Scroll")]
public partial class SlickScroll : Control
{
	public event ScrollEventHandler Scroll;

	public int BAR_SIZE_MAX => SmallHandle ? BAR_SIZE_MIN : ((int)(12 * UI.UIScale + (Style == StyleType.Vertical ? Padding.Horizontal : Padding.Vertical)) / 2 * 2);
	public int BAR_SIZE_MIN => (int)((SmallHandle ? 4:6) * UI.UIScale + (Style == StyleType.Vertical ? Padding.Horizontal : Padding.Vertical)) / 2 * 2;

	private static readonly List<SlickScroll> activeScrolls = [];
	private readonly Timer ScrollTimer = new() { Interval = 14 };
	private AnimationHandler animationHandler;
	private Control linkedControl;
	private Point mouseDownLocation;
	private bool mouseIn;
	private double speedModifier;
	private double targetPercentage;
	private bool Live;

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Active
	{
		get
		{
			if (Style == StyleType.Vertical)
			{
				return linkedControl?.Parent != null
					&& ControlSize > linkedControl.Parent.Height
					&& linkedControl.Parent.Height > 1;
			}
			else
			{
				return linkedControl?.Parent != null
					&& ControlSize > linkedControl.Parent.Width
					&& linkedControl.Parent.Width > 1;
			}
		}
	}
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
	public MouseDetector MouseDetector { get; private set; }

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point MouseDownLocation { get => mouseDownLocation; set => mouseDownLocation = value; }

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public double Percentage { get; private set; } = 0;

	[Category("Behavior"), DefaultValue(true)]
	public bool ShowHandle { get; set; } = true;

	[Category("Behavior"), DefaultValue(false)]
	public bool SmallHandle { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool AlwaysOpen { get; set; }

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
			{
				return activeScrolls.Any(x => x.ScrollTimer.Enabled);
			}
		}
	}

	private Rectangle Bar
	{
		get
		{
			if (Style == StyleType.Vertical)
			{
				return new(
					Padding.Left + 1,
					Padding.Top + (int)(Percentage * (Height - Padding.Vertical - ((Height - Padding.Vertical) * (linkedControl?.Parent?.Height ?? 0) / ControlSize.If(0, 1))) / 100),
					Width - Padding.Horizontal - 2,
					(Height - Padding.Vertical) * (linkedControl?.Parent?.Height ?? 0) / ControlSize.If(0, 1));
			}
			else
			{
				return new(
					Padding.Left + (int)(Percentage * (Width - Padding.Horizontal - ((Width - Padding.Horizontal) * (linkedControl?.Parent?.Width ?? 0) / ControlSize.If(0, 1))) / 100),
					Padding.Top + 1,
					(Width - Padding.Horizontal) * (linkedControl?.Parent?.Width ?? 0) / ControlSize.If(0, 1),
					Height - Padding.Vertical - 2);
			}
		}
	}

	private int ControlSize
	{
		get
		{
			if (CustomSizeSource != null)
			{
				return CustomSizeSource();
			}

			if (Style == StyleType.Vertical)
			{
				if (!AutoSizeSource)
				{
					return linkedControl?.Height ?? 0;
				}

				if (linkedControl != null)
				{
					return getControlHeight(linkedControl) - linkedControl.Top;
				}
			}
			else
			{
				if (!AutoSizeSource)
				{
					return linkedControl?.Width ?? 0;
				}

				if (linkedControl != null)
				{
					return getControlWidth(linkedControl) - linkedControl.Left;
				}
			}

			return 0;
		}
	}

	private bool Open => AlwaysOpen || (Style == StyleType.Vertical ? Width != BAR_SIZE_MIN : Height != BAR_SIZE_MIN);
	private bool IsMouseIn { get => AlwaysOpen || mouseIn; set => mouseIn = value; }

	public SlickScroll()
	{
		InitializeComponent();
		ResizeRedraw = DoubleBuffered = true;

		MouseWheel += SlickScroll_OnMouseWheel;
		ScrollTimer.Tick += ScrollTimer_Elapsed;

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

	public static SlickScroll GlobalGetScrollbar(Control control)
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
				return scroll;
			}

			p = p.Parent;
		}

		return null;
	}

	public void Reset()
	{
		if (Parent != null)
		{
			if (Style == StyleType.Vertical)
			{
				Height = Parent.Height;
			}
			else
			{
				Width = Parent.Width;
			}
		}

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
		if (Style == StyleType.Vertical)
		{
			var h = 0;

			while (control != LinkedControl)
			{
				h += control.Top;
				control = control.Parent;
			}

			ScrollTo(-100 * h / (linkedControl.Parent.Height - ControlSize).If(0, 1), intensity);
		}
		else
		{
			var w = 0;

			while (control != LinkedControl)
			{
				w += control.Left;
				control = control.Parent;
			}

			ScrollTo(-100 * w / (linkedControl.Parent.Width - ControlSize).If(0, 1), intensity);
		}
	}

	public void SetPercentage(Control control, bool target = false)
	{
		if (Style == StyleType.Vertical)
		{
			var h = 0;

			while (control != LinkedControl)
			{
				h += control.Top;
				control = control.Parent;
			}

			SetPercentage(-100 * h / ((linkedControl?.Parent?.Height ?? 0) - ControlSize).If(0, 1), target);
		}
		else
		{
			var w = 0;

			while (control != LinkedControl)
			{
				w += control.Left;
				control = control.Parent;
			}

			SetPercentage(-100 * w / ((linkedControl?.Parent?.Width ?? 0) - ControlSize).If(0, 1), target);
		}
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
			if (Style == StyleType.Vertical)
			{
				LinkedControl.Top = (int)(Percentage * ((linkedControl.Parent?.Height ?? 0) - ControlSize).If(0, 1) / 100);
			}
			else
			{
				LinkedControl.Left = (int)(Percentage * ((linkedControl.Parent?.Width ?? 0) - ControlSize).If(0, 1) / 100);
			}
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
			Width = AlwaysOpen ? BAR_SIZE_MAX : BAR_SIZE_MIN;
		}
		else
		{
			Height = AlwaysOpen ? BAR_SIZE_MAX : BAR_SIZE_MIN;
		}

		TabStop = false;
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		base.OnEnabledChanged(e);

		IsMouseDown = IsMouseIn = false;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live = !DesignMode)
		{
			LinkedControl = LinkedControl;
			MouseDetector = new MouseDetector();
			MouseDetector.MouseMove += mouseDetector_MouseMove;
			BeginInvoke(new Action(Reset));
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.Clear(BackColor);
		e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

		if (!Active || DesignMode)
		{
			return;
		}

		if (Style == StyleType.Vertical)
		{
			DrawVertical(e);
		}
		else
		{
			DrawHorizontal(e);
		}
	}

	private void DrawVertical(PaintEventArgs e)
	{
		var bar = Bar;

		using var brush = new SolidBrush(Color.FromArgb(Open ? (IsMouseDown ? 170 : 85) : 40, FormDesign.Design.AccentColor));

		if (SmallHandle)
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(Padding).CenterR(BAR_SIZE_MIN, Height - 2), BAR_SIZE_MIN / 2);
		else
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(Padding).CenterR(BAR_SIZE_MIN / 2, Height - 2), BAR_SIZE_MIN / 4);

		if (bar.Height > 0 && bar.Width > 0)
		{
			using var barBrush = new SolidBrush(IsMouseDown ? FormDesign.Design.ActiveColor : IsMouseIn ? FormDesign.Design.AccentColor : BackColor.MergeColor(FormDesign.Design.AccentColor));

			e.Graphics.FillRoundedRectangle(barBrush, bar, bar.Height < bar.Width ? 1 : (SmallHandle ? BAR_SIZE_MIN : bar.Width) / 2);
		}
	}
	private void DrawHorizontal(PaintEventArgs e)
	{
		var bar = Bar;

		using var brush = new SolidBrush(Color.FromArgb(Open ? (IsMouseDown ? 170 : 85) : 40, FormDesign.Design.AccentColor));

		if (SmallHandle)
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(Padding).CenterR(Width - 2, BAR_SIZE_MIN), BAR_SIZE_MIN / 2);
		else
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(Padding).CenterR(Width - 2, BAR_SIZE_MIN / 2), BAR_SIZE_MIN / 4);

		if (bar.Height > 0 && bar.Width > 0)
		{
			using var barBrush = new SolidBrush(IsMouseDown ? FormDesign.Design.ActiveColor : IsMouseIn ? FormDesign.Design.AccentColor : BackColor.MergeColor(FormDesign.Design.AccentColor));

			e.Graphics.FillRoundedRectangle(barBrush, bar, bar.Width < bar.Height ? 1 : (SmallHandle ? BAR_SIZE_MIN : bar.Height) / 2);
		}
	}

	private void Dismiss()
	{
		if (!IsMouseDown && !IsMouseIn)
		{
			animationHandler?.Dispose();
			animationHandler = Style == StyleType.Vertical
				? new AnimationHandler(this, new Size(BAR_SIZE_MIN, 0), AnimationOption.IgnoreHeight) { Speed = .5 }
				: new AnimationHandler(this, new Size(0, BAR_SIZE_MIN), AnimationOption.IgnoreWidth) { Speed = .5 };
			animationHandler.StartAnimation();
		}
	}

	private int getControlHeight(Control c)
	{
		return c.Visible ? (c.Top + c.Margin.Vertical +
			((c.Controls.Count == 0 || c is not Panel)
			? c.Height
			: c.Padding.Bottom + c.Controls.Max(getControlHeight))) : 0;
	}

	private int getControlWidth(Control c)
	{
		return c.Visible ? (c.Left + c.Margin.Horizontal +
			((c.Controls.Count == 0 || c is not Panel)
			? c.Width
			: c.Padding.Right + c.Controls.Max(getControlWidth))) : 0;
	}

	private void LinkedControl_Resize(object sender, EventArgs e)
	{
		if (DesignMode || linkedControl?.Parent == null || (linkedControl.FindForm()?.WindowState ?? FormWindowState.Minimized) == FormWindowState.Minimized)
		{
			return;
		}

		if (Style == StyleType.Vertical)
		{
			linkedControl.MaximumSize = new Size(linkedControl.Parent.Width - (linkedControl.Parent == Parent && TakeUpSpaceFromPanel ? BAR_SIZE_MAX : 0), int.MaxValue);
			linkedControl.MinimumSize = new Size(linkedControl.Parent.Width - (linkedControl.Parent == Parent && TakeUpSpaceFromPanel ? BAR_SIZE_MAX : 0), 0);

			foreach (var item in linkedControl.Controls.OfType<SlickSectionPanel>().Where(x => x.Dock == DockStyle.Top))
			{
				item.MaximumSize = linkedControl.MaximumSize;
				item.MinimumSize = new Size(linkedControl.MinimumSize.Width, item.MinimumSize.Height);
			}
		}
		else
		{
			linkedControl.MaximumSize = new Size(int.MaxValue, linkedControl.Parent.Width - (linkedControl.Parent == Parent && TakeUpSpaceFromPanel ? BAR_SIZE_MAX : 0));
			linkedControl.MinimumSize = new Size(0, linkedControl.Parent.Height - (linkedControl.Parent == Parent && TakeUpSpaceFromPanel ? BAR_SIZE_MAX : 0));

			foreach (var item in linkedControl.Controls.OfType<SlickSectionPanel>().Where(x => x.Dock == DockStyle.Left))
			{
				item.MaximumSize = linkedControl.MaximumSize;
				item.MinimumSize = new Size(linkedControl.MinimumSize.Width, item.MinimumSize.Height);
			}
		}

		Visible = ShowHandle && Active;

		SetPercentage(Active ? Percentage : 0, !ScrollTimer.Enabled);

		Invalidate();
	}

	private void mouseDetector_MouseMove(object sender, Point p)
	{
		if (IsDisposed)
		{
			return;
		}

		if (new Rectangle(PointToScreen(Point.Empty), Size).Pad((int)(-22 * UI.UIScale)).Contains(p))
		{
			if (!IsMouseIn && ShowHandle)
			{
				IsMouseIn = true;

				if (!SmallHandle)
				{
					animationHandler = Style == StyleType.Vertical
						? new AnimationHandler(this, new Size(BAR_SIZE_MAX, 0), AnimationOption.IgnoreHeight) { Speed = .5 }
						: new AnimationHandler(this, new Size(0, BAR_SIZE_MAX), AnimationOption.IgnoreWidth) { Speed = .5 };
					animationHandler.StartAnimation();
				}
			}
		}
		else
		{
			IsMouseIn = false;
			Dismiss();
		}
	}

	private void ScrollTimer_Elapsed(object sender, EventArgs e)
	{
		if (!Active)
		{
			ScrollTimer.Stop();
			SetPercentage(0, true);
			return;
		}

		if (Style == StyleType.Vertical)
		{
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
			var size = ControlSize - linkedControl.Parent.Width;
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

			if (Style == StyleType.Vertical)
			{
				if (!e.Y.IsWithin(Bar.Top, Bar.Top + Bar.Height))
				{
					SetPercentage(100D * (e.Y - (Bar.Height / 2) - Padding.Top).Between(0, Height - Padding.Vertical) / (Height - Padding.Vertical - Bar.Height), true);
					MouseDownLocation = new Point(e.X - Bar.Left, Bar.Height / 2);
				}
				else
				{
					MouseDownLocation = new Point(e.X - Bar.Left, e.Y - Bar.Top);
				}
			}
			else
			{
				if (!e.X.IsWithin(Bar.Left, Bar.Left + Bar.Width))
				{
					SetPercentage(100D * (e.X - (Bar.Width / 2) - Padding.Left).Between(0, Width - Padding.Horizontal) / (Width - Padding.Horizontal - Bar.Width), true);
					MouseDownLocation = new Point(e.X - Bar.Left, Bar.Height / 2);
				}
				else
				{
					MouseDownLocation = new Point(e.X - Bar.Left, e.Y - Bar.Top);
				}
			}

			Invalidate();
		}
	}

	private void SlickScroll_MouseMove(object sender, MouseEventArgs e)
	{
		if (Live)
		{
			if (Style == StyleType.Vertical)
			{
				if (e.Button == MouseButtons.Left)
				{
					SetPercentage(100D * (e.Y - MouseDownLocation.Y - Padding.Top).Between(0, Height - Padding.Vertical) / (Height - Padding.Vertical - Bar.Height), true);
				}

				Cursor = e.Y.IsWithin(Bar.Top, Bar.Top + Bar.Height) ? Cursors.Hand : Cursors.Default;
			}
			else
			{

				if (e.Button == MouseButtons.Left)
				{
					SetPercentage(100D * (e.X - MouseDownLocation.X - Padding.Left).Between(0, Width - Padding.Horizontal) / (Width - Padding.Horizontal - Bar.Width), true);
				}

				Cursor = e.Y.IsWithin(Bar.Left, Bar.Left + Bar.Width) ? Cursors.Hand : Cursors.Default;
			}
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
		{
			return;
		}

		if (Active && !IsMouseDown && Live)
		{
			TargetPercentage -= e.Delta * 100D / (ControlSize - (Style == StyleType.Vertical ? linkedControl.Parent.Height : linkedControl.Parent.Width));

			if (e is HandledMouseEventArgs handle)
			{
				handle.Handled = true;
			}
		}
	}
}