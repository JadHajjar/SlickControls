using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls;

public partial class SlickForm : Form, ISlickForm
{

	public event Func<Message, bool> OnWndProc;

	[Category("Property Changed"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public event EventHandler WindowStateChanged;

	[Category("Property Changed"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public event StateChangingEventHandler WindowStateChanging;

	[Category("Property Changed"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public event EventHandler FormStateChanged;

	[Category("Behavior"), EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Bindable(true)]
	public bool CloseForm { get; set; } = true;

	[Category("Appearance"), DefaultValue(false)]
	public bool NoBorder { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool FullScreenIgnoresTaskbar { get; set; }

	public virtual Image FormIcon { get; set; }
	public virtual Rectangle IconBounds { get; set; }

	public new FormWindowState WindowState
	{
		get => base.WindowState;
		set
		{
			var change = base.WindowState != value;
			if (change)
			{
				var args = new StateChangingEventArgs(value);
				WindowStateChanging?.Invoke(this, args);

				if (args.Cancel)
				{
					return;
				}
			}

			SuspendLayout();
			Padding = value == FormWindowState.Maximized ? new Padding(0) : value == FormWindowState.Normal ? new Padding(4, 4, 7, 7) : Padding;
			base_P_Container.Padding = value == FormWindowState.Maximized || NoBorder ? new Padding(0) : new Padding(1);

			var screen = IsHandleCreated ? Screen.FromHandle(Handle) : null;

			if (screen != null)
			{
				MaximizedBounds = FullScreenIgnoresTaskbar ? screen.Bounds : new Rectangle(screen.Bounds.X - screen.WorkingArea.X, screen.Bounds.Y - screen.WorkingArea.Y, screen.WorkingArea.Width, screen.WorkingArea.Height);
			}

			base.WindowState = value;

			if (change)
			{
				WindowStateChanged?.Invoke(this, new EventArgs());
			}

			ResumeLayout(true);
		}
	}

	public new Rectangle MaximizedBounds { get => base.MaximizedBounds; set => base.MaximizedBounds = value; }

	private readonly List<ExtensionClass.action> nextIdleActions = [];
	private readonly object idleLock = new();
	protected double LastUiScale { get; set; } = 1;

	public SlickForm()
	{
		InitializeComponent();

		UI.UIChanged += UIChanged;
		var md = new MouseDetector();
		md.MouseMove += Md_MouseMove;

		Disposed += (s, e) =>
		{
			md.Dispose();
			UI.UIChanged -= UIChanged;
			LocaleHelper.LanguageChanged -= LocaleChanged;
		};

		SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		SetStyle(ControlStyles.ResizeRedraw, true);

		LocaleHelper.LanguageChanged += LocaleChanged;

		Application.Idle += Application_Idle;

		OnNextIdle(() => Opacity = 1);
	}

	protected virtual void LocaleChanged()
	{
	}

	private void Application_Idle(object sender, EventArgs e)
	{
		ExtensionClass.action[] actions = null;

		lock (idleLock)
		{
			actions = nextIdleActions.ToArray();
			nextIdleActions.Clear();
		}

		foreach (var item in actions)
		{
			item();
		}
	}

	public void OnNextIdle(ExtensionClass.action action)
	{
		lock (idleLock)
		{
			nextIdleActions.Add(action);
		}
	}

	protected void ClearNextIdle()
	{
		lock (idleLock)
		{
			nextIdleActions.Clear();
		}
	}

	protected virtual void UIChanged()
	{
		if (!DesignMode)
		{
			Font = UI.Font(8.25F);

			Bounds = Rectangle.Intersect(UI.Scale(Bounds, UI.UIScale / LastUiScale), Screen.FromHandle(Handle).WorkingArea);

			LastUiScale = UI.UIScale;
		}
	}

	protected void base_B_Close_Click(object sender, EventArgs e)
	{
		if (CloseForm)
		{
			Close();
		}
		else
		{
			Hide();
		}
	}

	protected virtual void DesignChanged(FormDesign design)
	{
		ForeColor = design.ForeColor;
		base_P_Container.BackColor = CurrentFormState.Color();

		if (!DesignMode)
		{
			base_PB_Icon.Color(design.MenuForeColor);
			base_B_Close.Color(design.RedColor);
			base_B_Max.Color(design.YellowColor);
			base_B_Min.Color(design.GreenColor);
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		Opacity = 0;

		if (StartPosition == FormStartPosition.CenterParent && Owner != null)
		{
			Location = Owner.Bounds.Center(Size);
		}

		if (NoBorder)
		{
			base_P_Container.Padding = new Padding(0);
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (!DesignMode)
		{
			base_PB_Icon.Click += base_PB_Icon_Click;
			base_B_Close.Click += base_B_Close_Click;
			base_B_Max.Click += base_B_Max_Click;
			base_B_Min.Click += base_B_Min_Click;
		}

		SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		SetStyle(ControlStyles.ResizeRedraw, true);
		DesignChanged(FormDesign.Design);
		UIChanged();
		LocaleChanged();

		MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		e.Graphics.Clear(BackColor);
	}

	private void base_B_Max_Click(object sender, EventArgs e)
	{
		WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
	}

	private void base_B_Min_Click(object sender, EventArgs e)
	{
		WindowState = FormWindowState.Minimized;
	}

	private void base_PB_Icon_Click(object sender, EventArgs e)
	{
		if (this is MessagePrompt)
		{
			return;
		}

		if ((e as MouseEventArgs).Button == MouseButtons.Right)
		{
			var panelForm = this is BasePanelForm;
			var bpf = panelForm ? this as BasePanelForm : null;
			var isThemeChanger = panelForm && bpf.CurrentPanel is PC_ThemeChanger;
			var canSwitch = !isThemeChanger || !FormDesign.IsCustomEligible();

			var items = new List<SlickStripItem>
			{
				new("Minimize", "Minimize", () => WindowState = FormWindowState.Minimized, visible: MinimizeBox),

				new(WindowState == FormWindowState.Maximized ? "Restore" : "Maximize", WindowState == FormWindowState.Maximized ? "Restore" : "Maximize", () => WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized, visible: MaximizeBox),

				new("Close", "Close", Close),

				panelForm && !bpf.HideMenu ? SlickStripItem.Empty : null,

				new("Smaller Menu", panelForm && bpf.SmallMenu ? "Checked_ON" : "Checked_OFF", () => bpf.SmallMenu = !bpf.SmallMenu, visible: panelForm && !bpf.HideMenu),
				new("Auto-Hide Menu", panelForm && bpf.AutoHideMenu ? "Checked_ON" : "Checked_OFF", () => bpf.AutoHideMenu = !bpf.AutoHideMenu, visible: panelForm && !bpf.HideMenu),

				SlickStripItem.Empty,

				new("Theme Changer", "Paint",  () =>
				{
					if (panelForm)
					{
						(this as BasePanelForm).PushPanel<PC_ThemeChanger>(PanelItem.Empty);
					}
					else
					{
						Theme_Changer.ThemeForm = Theme_Changer.ThemeForm.ShowUp(true);
					}
				}, visible: !isThemeChanger),
			};

			if (canSwitch)
			{
				items.Add(new SlickStripItem("Switch To", "Switch", true)
				{
					SubItems = FormDesign.List.ToList(item => new SlickStripItem(item.Name, item.Name == FormDesign.Design.Name ? "Checked_ON" : "Checked_OFF", () =>
					{
						Cursor = Cursors.WaitCursor;

						if (isThemeChanger)
						{
							(bpf.CurrentPanel as PC_ThemeChanger).savedDesignName = item.Name;
						}

						FormDesign.Switch(item, true, true);

						Cursor = Cursors.Default;
					}, item.Name == FormDesign.Design.Name))
				});
			}

			SlickToolStrip.Show(this, items.ToArray());
		}
		else
		{
			OnAppIconClicked();
		}
	}

	protected virtual void OnAppIconClicked()
	{
		Cursor = Cursors.WaitCursor;
		FormDesign.Switch();
		Cursor = Cursors.Default;
	}

	private void BaseForm_Resize(object sender, EventArgs e)
	{
		WindowState = WindowState;
	}

	public bool FormIsActive { get; internal set; } = true;
	private FormState currentFormState = FormState.NormalFocused;

	public bool FreezeFocus { get; set; }

	public virtual FormState CurrentFormState
	{
		get => currentFormState;
		set
		{
			if (currentFormState != value && !FreezeFocus)
			{
				currentFormState = value;
				this.TryInvoke(() =>
				{
					base_P_Container.BackColor = value.Color();
					FormStateChanged?.Invoke(this, EventArgs.Empty);
				});
			}
		}
	}

	private void Form_Activated(object sender, EventArgs e)
	{
		if (CurrentFormState.IsNormal())
		{
			CurrentFormState = FormState.NormalFocused;
		}

		FormIsActive = true;
	}

	private void Form_Deactivate(object sender, EventArgs e)
	{
		if (CurrentFormState.IsNormal() && !FreezeFocus)
		{
			try
			{
				BeginInvoke(new Action(() =>
				{
					if (CurrentFormState.IsNormal())
					{
						CurrentFormState = FormState.NormalUnfocused;
					}

					FormIsActive = false;
				}));
			}
			catch { }
		}
	}

	public const int HT_CAPTION = 0x2;
	public const int WM_NCLBUTTONDOWN = 0xA1;

	//protected override CreateParams CreateParams
	//{
	//	get
	//	{
	//		var cp = base.CreateParams;
	//		cp.Style |= 0x20000; // <--- use 0x20000
	//		return cp;
	//	}
	//}

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern bool ReleaseCapture();

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	protected void Form_MouseDown(object sender, MouseEventArgs e)
	{
		ForceWindowMove(e);
	}

	public void ForceWindowMove(MouseEventArgs e = null)
	{
		if (e == null || e.Button == MouseButtons.Left)
		{
			ReleaseCapture();
			SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

			if (e != null && e.Clicks == 2)
			{
				this.SuspendDrawing();
				WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
				this.ResumeDrawing();
			}
			else
			{
				foreach (var screen in Screen.AllScreens)
				{
					if (!screen.WorkingArea.Contains(MousePosition))
					{
						continue;
					}

					const int snapGap = 10; // Change this value to adjust the snapping threshold

					if (MousePosition.Y >= screen.WorkingArea.Top && MousePosition.Y <= screen.WorkingArea.Top + snapGap)
					{
						this.SuspendDrawing();
						WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
						this.ResumeDrawing();
					}
				}
			}
		}
		else if (e != null && e.Button == MouseButtons.Right)
		{
			base_PB_Icon_Click(this, e);
		}
	}

	protected override void WndProc(ref Message m)
	{
		HandleWndProc(ref m);
	}

	protected virtual bool HandleWndProc(ref Message m)
	{
		const int RESIZE_HANDLE_SIZE = 10;

		if (OnWndProc?.Invoke(m) ?? false)
		{
			return true;
		}

		switch (m.Msg)
		{
			case 0x86:
			case 0x6:
				if (currentFormState <= FormState.ForcedFocused)
				{
					base_P_Container.BackColor = FormState.NormalFocused.Color();
				}

				break;

			case 0x0084/*NCHITTEST*/ :
				base.WndProc(ref m);

				if ((int)m.Result == 0x01/*HTCLIENT*/)
				{
					var screenPoint = new Point(m.LParam.ToInt32());
					var clientPoint = PointToClient(screenPoint);
					if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
					{
						if (clientPoint.X <= RESIZE_HANDLE_SIZE)
						{
							m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
						}
						else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
						{
							m.Result = (IntPtr)12/*HTTOP*/ ;
						}
						else
						{
							m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
						}
					}
					else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
					{
						if (clientPoint.X <= RESIZE_HANDLE_SIZE)
						{
							m.Result = (IntPtr)10/*HTLEFT*/ ;
						}
						else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
						{
							m.Result = (IntPtr)2/*HTCAPTION*/ ;
						}
						else
						{
							m.Result = (IntPtr)11/*HTRIGHT*/ ;
						}
					}
					else
					{
						if (clientPoint.X <= RESIZE_HANDLE_SIZE)
						{
							m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
						}
						else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
						{
							m.Result = (IntPtr)15/*HTBOTTOM*/ ;
						}
						else
						{
							m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
						}
					}
				}

				return true;
		}

		try
		{
			base.WndProc(ref m);
		}
		catch { }

		return false;
	}

	private void Md_MouseMove(object sender, Point p)
	{
		if (WindowState == FormWindowState.Maximized)
		{
			var show = false;

			switch (CurrentTaskbarLocation)
			{
				case TaskbarLocation.Top:
					show = p.Y < 2;
					break;

				case TaskbarLocation.Left:
					show = p.X < 2;
					break;

				case TaskbarLocation.Bottom:
					show = p.Y > Screen.PrimaryScreen.Bounds.Height - 2;
					break;

				case TaskbarLocation.Right:
					show = p.X > Screen.PrimaryScreen.Bounds.Width - 2;
					break;

				case TaskbarLocation.None:
					break;

				default:
					break;
			}

			if (show)
			{
				ShowTaskbar();
			}
		}
	}

	public static readonly TaskbarLocation CurrentTaskbarLocation = GetTaskbarLocation();

	public enum TaskbarLocation
	{ Top, Left, Bottom, Right, None }

	public static TaskbarLocation GetTaskbarLocation()
	{
		var sc = Screen.PrimaryScreen;

		if (sc.WorkingArea.Top > 0)
		{
			return TaskbarLocation.Top;
		}
		else if (sc.WorkingArea.Left != sc.Bounds.X)
		{
			return TaskbarLocation.Left;
		}
		else if ((sc.Bounds.Height - sc.WorkingArea.Height) > 0)
		{
			return TaskbarLocation.Bottom;
		}
		else if (sc.WorkingArea.Right != 0)
		{
			return TaskbarLocation.Right;
		}

		return TaskbarLocation.None;
	}

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
	private static extern IntPtr FindWindow(
	string lpClassName,
	string lpWindowName);

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
	private static extern int SetWindowPos(
		IntPtr hWnd,
		IntPtr hWndInsertAfter,
		int x,
		int y,
		int cx,
		int cy,
		uint uFlags
	);

	[Flags]
	private enum SetWindowPosFlags : uint
	{
		HideWindow = 128,
		ShowWindow = 64
	}

	public static void ShowTaskbar()
	{
		var window = FindWindow("Shell_traywnd", "");
		SetWindowPos(window, IntPtr.Zero, 0, 0, 0, 0, (uint)SetWindowPosFlags.ShowWindow);
	}

	public static void HideTaskbar()
	{
		var window = FindWindow("Shell_traywnd", "");
		SetWindowPos(window, IntPtr.Zero, 0, 0, 0, 0, (uint)SetWindowPosFlags.HideWindow);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		base.OnKeyDown(e);

		if (e.KeyCode.HasFlag(Keys.Menu))
		{
			SlickControl.OnAltPressed();
		}

		if (e.KeyCode.HasFlag(Keys.ControlKey))
		{
			SlickControl.OnCtrlPressed();
		}
	}
	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyUp(e);

		if (e.KeyCode.HasFlag(Keys.Menu))
		{
			SlickControl.OnAltReleased();
		}

		if (e.KeyCode.HasFlag(Keys.ControlKey))
		{
			SlickControl.OnCtrlReleased();
		}
	}
}