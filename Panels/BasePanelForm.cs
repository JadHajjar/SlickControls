using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls;

public partial class BasePanelForm : SlickForm
{
	protected internal readonly PanelItemControl base_P_Tabs;
	private Image formIcon;
	private readonly List<PanelContent> panelHistory = [];
	private readonly List<PanelContent> panelsToDispose = [];
	private List<PanelItem> AllSidebarItems = [];
	private PanelItem[] sidebarItems = new PanelItem[0];
	private bool autoHideMenu;
	private MouseDetector mouseDetector;
	private bool smallMenu;
	private bool hideMenu;
	private bool menuSetUp;
	private object lastPanelData;

	public event Func<Message, Keys, bool> HandleKeyPress;

	public PanelContent CurrentPanel { get; private set; }

	[Category("Appearance")]
	public override Image FormIcon { get => formIcon; set => base_PB_Icon.Image = formIcon = value.Color(FormDesign.Design.IconColor); }

	[Category("Appearance")]
	public override Rectangle IconBounds { get => base_PB_Icon.Bounds; set => base_PB_Icon.Bounds = value; }

	public new bool MaximizeBox { get => base_B_Max.Visible; set => base_B_Max.Visible = value; }

	public new bool MinimizeBox { get => base_B_Min.Visible; set => base_B_Min.Visible = value; }

	[Category("Design")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public PanelItem[] SidebarItems
	{
		get => sidebarItems;
		set
		{
			sidebarItems = value;
			GenerateTabs();
		}
	}

	public IEnumerable<PanelContent> PanelHistory => panelHistory;

	internal bool AutoHideMenu
	{
		get => autoHideMenu;
		set
		{
			autoHideMenu = value;
			mouseDetector_MouseMove(null, Cursor.Position);

			if (!value)
			{
				setSmallMenu();
				base_P_Side.SendToBack();
			}
			else
			{
				base_P_Side.BringToFront();
			}

			if (IsHandleCreated)
			{
				ISave.Save(new { AutoHideMenu, SmallMenu }, "PanelForm.tf", true, "SlickUI");
			}
		}
	}

	internal bool SmallMenu
	{
		get => smallMenu;
		set
		{
			smallMenu = value;
			setSmallMenu();

			if (autoHideMenu)
			{
				mouseDetector_MouseMove(null, Cursor.Position);
			}

			base_P_Tabs.FilterChanged();

			if (IsHandleCreated)
			{
				ISave.Save(new { AutoHideMenu, SmallMenu }, "PanelForm.tf", true, "SlickUI");
			}
		}
	}

	[DefaultValue(false)]
	public bool HideMenu
	{
		get => hideMenu;
		set => base_P_Side.Visible = !(hideMenu = value);
	}

	protected override CreateParams CreateParams
	{
		get
		{
			var cp = base.CreateParams;
			cp.Style |= 0x20000; // <--- use 0x20000
			return cp;
		}
	}

	public BasePanelForm() : this(false)
	{ }

	public BasePanelForm(bool initialized)
	{
		InitializeComponent();

		base_P_Tabs = new PanelItemControl(this) { Dock = DockStyle.Fill };
		base_TLP_Side.Controls.Add(base_P_Tabs);

		if (DesignMode)
		{
			SetPanel<PanelContent>(null);
		}

		if (!initialized)
		{
			FormDesign.DesignChanged += DesignChanged;
		}

		base_P_Tabs.OnFormMove += Form_MouseDown;
		base_P_Icon.MouseDown += Form_MouseDown;
		base_P_SideControls.MouseDown += Form_MouseDown;
		base_TLP_Side.MouseDown += Form_MouseDown;
	}

	public void PushBack(bool dispose = true)
	{
		var panel = panelHistory.LastOrDefault();

		if (panel != null)
		{
			if (panel.IsDisposed)
			{
				panelHistory.Remove(panel);
				PushBack(dispose);
			}
			else if (SetPanel(panel.PanelItem, panel, dispose, false))
			{
				panelHistory.Remove(panel);
			}
		}
	}

	public bool PushPanel<T>() where T : PanelContent, new()
	{
		return PushPanel<T>(null);
	}

	public bool PushPanel<T>(PanelItem panelItem) where T : PanelContent, new()
	{
		if (CurrentPanel != null)
		{
			if (!panelHistory.Contains(CurrentPanel))
			{
				panelHistory.Add(CurrentPanel);
			}
			else
			{
				panelHistory.Remove(CurrentPanel);
				panelHistory.Insert(panelHistory.Count - 1, CurrentPanel);
			}

			CurrentPanel.SuspendLayout();
			base_P_PanelContent.Controls.Remove(CurrentPanel);
			CurrentPanel.CanExit(false);
			CurrentPanel = null;
		}

		return SetPanel<T>(panelItem, false, false);
	}

	public bool PushPanel(PanelContent panelContent)
	{
		return PushPanel(null, panelContent);
	}

	public bool PushPanel(PanelItem panelItem, PanelContent panelContent)
	{
		if (CurrentPanel == panelContent)
		{
			return false;
		}

		if (CurrentPanel != null)
		{
			if (!panelHistory.Contains(CurrentPanel))
			{
				panelHistory.Add(CurrentPanel);
			}
			else
			{
				panelHistory.Remove(CurrentPanel);
				panelHistory.Insert(panelHistory.Count - 1, CurrentPanel);
			}

			CurrentPanel.SuspendLayout();
			base_P_PanelContent.Controls.Remove(CurrentPanel);
			CurrentPanel.CanExit(false);
			CurrentPanel = null;
		}

		return SetPanel(panelItem, panelContent, false, false);
	}

	public bool SetPanel<T>(PanelItem panelItem, bool dispose = true, bool clearHistory = true) where T : PanelContent, new()
	{
		if (CurrentPanel != null && ((CurrentPanel.PanelItem == panelItem && !(panelItem?.ForceReopen ?? false)) || !CurrentPanel.CanExit(dispose)))
		{
			return false;
		}

		try
		{
			if (clearHistory && panelHistory != null)
			{
				foreach (var panel in panelHistory.Reverse<PanelContent>().ToList())
				{
					if (panel.CanExit(true))
					{
						handleDispose(panel);
						panelHistory.Remove(panel);
					}
					else
					{
						return false;
					}
				}
			}

			var newPanel = new T
			{
				Size = base_P_PanelContent.Size,
				Dock = DockStyle.Fill,
				PanelItem = panelItem,
				Form = this
			};

			newPanel.UseTransitoryData(lastPanelData);

			base_P_Content.SuspendDrawing();
			base_P_PanelContent.SuspendDrawing();

			if (dispose)
			{
				handleDispose(CurrentPanel);
			}
			else if (CurrentPanel != null)
			{
				CurrentPanel.SuspendDrawing();
				base_P_PanelContent.Controls.Remove(CurrentPanel);
			}

			CurrentPanel = newPanel;
		}
		catch (Exception ex)
		{
			MessagePrompt.Show(ex, form: this);
			return false;
		}

		base_B_Close.Visible = base_B_Max.Visible = base_B_Min.Visible = !CurrentPanel.HideWindowIcons;
		base_P_Content.BackColor = CurrentPanel.GetTopBarColor();
		base_TLP_Side.Invalidate();

		if (!CurrentPanel.PanelWasSetUp)
		{
			RecursiveMouseDown(CurrentPanel);
			CurrentPanel.PanelWasSetUp = true;
		}

		if (CurrentPanel.AcceptButton != null)
		{
			var btn = new Button();
			AcceptButton = btn;
			btn.Click += (s, e) =>
			{
				if (CurrentPanel.AcceptButton.Enabled)
				{
					CurrentPanel.AcceptButton.OnClick(e);
				}
			};
		}
		else
		{
			AcceptButton = null;
		}

		if (CurrentPanel.CancelButton != null)
		{
			var btn = new Button();
			CancelButton = btn;
			btn.Click += (s, e) =>
			{
				if (CurrentPanel.CancelButton.Enabled)
				{
					CurrentPanel.CancelButton.OnClick(e);
				}
			};
		}
		else
		{
			CancelButton = null;
		}

		base_P_PanelContent.Controls.Add(CurrentPanel);

		if (SidebarItems != null && panelItem != null)
		{
			foreach (var item in AllSidebarItems)
			{
				item.Selected = item == panelItem;
			}

			base_P_Tabs.FilterChanged();
		}

		CurrentPanel.OnShown();
		CurrentPanel.ResumeDrawing(false);

		base_P_Content.ResumeDrawing();
		base_P_PanelContent.ResumeDrawing();

		if (!CurrentPanel.Focus() && IsHandleCreated)
		{
			BeginInvoke(new Action(() => CurrentPanel.Focus()));
		}

		return true;
	}

	public bool SetPanel(PanelItem panelItem, PanelContent panelContent, bool dispose = true, bool clearHistory = true)
	{
		if (CurrentPanel != null && (
				(CurrentPanel.PanelItem != null && panelItem != PanelItem.Empty && CurrentPanel.PanelItem == panelItem && !panelItem.ForceReopen && CurrentPanel.GetType() == panelContent.GetType())
				|| (dispose && !CurrentPanel.CanExit(dispose))
			))
		{
			return false;
		}

		if (clearHistory && panelHistory != null)
		{
			foreach (var panel in panelHistory.Reverse<PanelContent>().ToList())
			{
				if (panel.CanExit(true))
				{
					handleDispose(panel);
					panelHistory.Remove(panel);
				}
				else
				{
					return false;
				}
			}
		}

		base_P_Content.SuspendDrawing();
		base_P_PanelContent.SuspendDrawing();

		if (dispose)
		{
			handleDispose(CurrentPanel);
		}
		else if (CurrentPanel != null)
		{
			CurrentPanel.SuspendLayout();
			base_P_PanelContent.Controls.Remove(CurrentPanel);
		}

		CurrentPanel = panelContent;

		CurrentPanel.Size = base_P_PanelContent.Size;
		CurrentPanel.Dock = DockStyle.Fill;
		CurrentPanel.PanelItem = panelItem;
		CurrentPanel.Form = this;

		CurrentPanel.UseTransitoryData(lastPanelData);

		base_B_Close.Visible = base_B_Max.Visible = base_B_Min.Visible = !CurrentPanel.HideWindowIcons;
		base_P_Content.BackColor = CurrentPanel.GetTopBarColor();
		base_TLP_Side.Invalidate();

		if (!CurrentPanel.PanelWasSetUp)
		{
			RecursiveMouseDown(CurrentPanel);
			CurrentPanel.PanelWasSetUp = true;
		}

		if (CurrentPanel.AcceptButton != null)
		{
			var btn = new Button();
			AcceptButton = btn;
			btn.Click += (s, e) =>
			{
				if (CurrentPanel.AcceptButton.Enabled)
				{
					CurrentPanel.AcceptButton.OnClick(e);
				}
			};
		}
		else
		{
			AcceptButton = null;
		}

		if (CurrentPanel.CancelButton != null)
		{
			var btn = new Button();
			CancelButton = btn;
			btn.Click += (s, e) =>
			{
				if (CurrentPanel.CancelButton.Enabled)
				{
					CurrentPanel.CancelButton.OnClick(e);
				}
			};
		}
		else
		{
			CancelButton = null;
		}

		base_P_PanelContent.Controls.Add(CurrentPanel);

		if (SidebarItems != null && panelItem != null)
		{
			foreach (var item in AllSidebarItems)
			{
				item.Selected = item == panelItem;
			}

			base_P_Tabs.FilterChanged();
		}

		panelContent.Visible = true;

		CurrentPanel.OnShown();
		CurrentPanel.ResumeLayout(true);

		base_P_Content.ResumeDrawing();
		base_P_PanelContent.ResumeDrawing();

		if (!CurrentPanel.Focus() && IsHandleCreated)
		{
			BeginInvoke(new Action(() => CurrentPanel.Focus()));
		}

		return true;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		base_P_Side.Width = (int)(180 * UI.FontScale);
		base_TLP_Side.Padding = UI.Scale(new Padding(5), UI.FontScale);
		base_P_Side.Padding = UI.Scale(new Padding(5, 5, 0, 5), UI.FontScale);

		base_P_SideControls.Font = UI.Font(6.75F);
		base_P_Icon.Height = (int)(70 * UI.UIScale);
		base_PB_Icon.Size = UI.Scale(new Size(32, 32), UI.UIScale);
		base_B_Close.Size = base_B_Max.Size = base_B_Min.Size = new Size(6 + (int)(20 * UI.UIScale), 6 + (int)(20 * UI.UIScale));

		if (SmallMenu)
		{
			setSmallMenu();
		}
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		base_P_Content.BackColor = CurrentPanel?.GetTopBarColor() ?? design.BackColor;
		base_TLP_Side.BackColor = design.MenuColor;
		base_P_Side.ForeColor = design.LabelColor;
		base_P_SideControls.ForeColor = design.LabelColor.MergeColor(design.ID.If(0, design.AccentColor, design.MenuColor), 80);

		base_PB_Icon.Color(design.MenuForeColor);
	}

	protected void DisableSideBar()
	{
		base_P_Tabs.Enabled = false;
	}

	protected void EnableSideBar()
	{
		base_P_Tabs.Enabled = true;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		base_TLP_TopButtons.Location = new Point(base_P_Content.Width - base_TLP_TopButtons.Width, 0);

		if (!DesignMode)
		{
			mouseDetector = new MouseDetector();
			mouseDetector.MouseMove += mouseDetector_MouseMove;

			var options = ISave.LoadRaw("PanelForm.tf", "SlickUI");

			if (options != null)
			{
				AutoHideMenu = options.AutoHideMenu;
				SmallMenu = options.SmallMenu;
			}

			menuSetUp = true;
		}

		base_P_Tabs.BringToFront();

		OnNextIdle(() => CurrentPanel?.Focus());
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (CurrentPanel != null && CurrentPanel.KeyPressed(e.KeyChar))
		{
			return;
		}

		base.OnKeyPress(e);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//if (keyData == Keys.Apps)
		//{
		//	mouse_event(0x00000008, Cursor.Position.X, Cursor.Position.Y, 0, 0);
		//	mouse_event(0x00000010, Cursor.Position.X, Cursor.Position.Y, 0, 0);
		//}

		if (CurrentPanel != null && CurrentPanel.KeyPressed(ref msg, keyData))
		{
			return true;
		}

		if (HandleKeyPress?.Invoke(msg, keyData) ?? false)
		{
			return true;
		}

		if (keyData == Keys.Escape && CancelButton == null)
		{
			if (PanelHistory.Any())
			{
				PushBack();
				return true;
			}
			else if (WindowState == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Normal;
				return true;
			}
		}

		if (keyData == (Keys.ControlKey | Keys.Escape) && SidebarItems != null)
		{
			foreach (var item in SidebarItems)
			{
				item.Highlighted = false;
			}

			base_P_Tabs.Invalidate();

			return true;
		}

		if ((keyData == (Keys.Control | Keys.Up) || keyData == (Keys.Control | Keys.Down)) && SidebarItems != null)
		{
			var prev = SidebarItems.FirstOrDefault(x => x.Highlighted) ?? SidebarItems.FirstOrDefault(x => x.Selected);
			var item = (keyData.HasFlag(Keys.Up) ? SidebarItems.Previous(prev, true) : SidebarItems.Next(prev, true)) ?? SidebarItems.FirstOrDefault();

			if (prev != null)
			{
				prev.Highlighted = false;
			}

			if (item != null)
			{
				item.Highlighted = true;
			}

			base_P_Tabs.Invalidate();

			return true;
		}

		if (keyData.HasFlag(Keys.Control) && (keyData & ~Keys.Control).IsDigit())
		{
			var numb = (keyData & ~Keys.Control).ToString().TakeLast(1).First();

			if (SidebarItems != null)
			{
				foreach (var item in SidebarItems)
				{
					item.Highlighted = false;

					if (item.ShowKey[0] == numb)
					{
						item.Highlighted = true;
						item.MouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
					}
				}

				base_P_Tabs.Invalidate();
			}

			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void WndProc(ref Message m)
	{
		try
		{
			if (!(CurrentPanel?.OnWndProc(m) ?? false) && !HandleWndProc(ref m))
			{
				if (m.Msg == 0x210 && m.WParam == (IntPtr)0x1020b && PanelHistory.Any())
				{
					PushBack();
				}
			}
		}
		catch { }
	}

	protected override void OnDeactivate(EventArgs e)
	{
		base.OnDeactivate(e);

		if (SidebarItems != null)
		{
			foreach (var item in SidebarItems)
			{
				item.Highlighted = false;
				item.ShowKey = null;
			}

			base_P_Tabs.Invalidate();
		}

		if (CurrentFormState != FormState.ForcedFocused)
		{
			foreach (var panel in panelsToDispose)
			{
				panel.Dispose();
			}

			GC.Collect();
			panelsToDispose.Clear();
		}
	}

	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	private void handleDispose(PanelContent panel)
	{
		if (panel != null)
		{
			if (CurrentPanel == panel)
			{
				lastPanelData = panel.GetTransitoryData();
			}

			panel.SoftDispose();
			panel.SuspendDrawing();
			panel.Parent = null;
			panelsToDispose.Add(panel);
		}
	}

	private void GenerateTabs()
	{
		var tabs = new List<PanelTab>();
		AllSidebarItems = [];

		foreach (var group in sidebarItems.Select(x => x.Group).Distinct())
		{
			if (!string.IsNullOrWhiteSpace(group))
			{
				tabs.Add(PanelTab.GroupName(group));
			}

			var items = sidebarItems.Where(x => x.Group == group);

			foreach (var item in items)
			{
				tabs.Add(new PanelTab(item));

				AllSidebarItems.Add(item);

				if (item.SubItems.Length > 0)
				{
					item.OnClick += OpenTabSubItems;
				}

				foreach (var subItem in item.SubItems)
				{
					tabs.Add(new PanelTab(subItem, item));

					AllSidebarItems.Add(subItem);
				}
			}

			if (group != sidebarItems.Select(x => x.Group).Distinct().Last())
			{
				tabs.Add(PanelTab.Separator());
			}
		}

		base_P_Tabs.SetItems(tabs);
	}

	private void OpenTabSubItems(object sender, MouseEventArgs e)
	{
		if (((PanelItem)sender).SubItems.Any(x => x.Selected))
		{
			return;
		}

		foreach (var item in AllSidebarItems)
		{
			item.Selected = item == sender;
		}

		base_P_Tabs.FilterChanged();
	}

	private void RecursiveMouseDown(Control ctrl)
	{
		if (ctrl is Panel or UserControl)
		{
			ctrl.MouseDown += Form_MouseDown;

			foreach (var item in ctrl.Controls.ThatAre<Panel>().Where(x => x.Tag?.ToString() != "NoMouseDown"))
			{
				RecursiveMouseDown(item);
			}

			foreach (var item in ctrl.Controls.ThatAre<Label>().Where(x => x.Tag?.ToString() != "NoMouseDown"))
			{
				item.MouseDown += Form_MouseDown;
			}
		}
	}

	private void base_P_Container_Paint(object sender, PaintEventArgs e)
	{
		switch (CurrentFormState)
		{
			case FormState.NormalUnfocused:
				e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.MenuColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Dark ? 3 : -3)), new Rectangle(0, 0, base_P_Side.Width + 1, base_P_Side.Height + 2));
				break;

			case FormState.ForcedFocused:
			case FormState.NormalFocused:
				e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.MenuColor.MergeColor(FormDesign.Design.ActiveColor, 70)), new Rectangle(0, 0, base_P_Side.Width + 1, base_P_Side.Height + 2));
				break;
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.KeyCode == Keys.ControlKey && SidebarItems != null)
		{
			var i = 0;
			foreach (var item in SidebarItems)
			{
				if (i++ <= 10)
				{
					item.ShowKey = (i % 10).ToString();
				}

				item.Highlighted = item.Selected;
			}

			base_P_Tabs.Invalidate();
		}

		base.OnKeyDown(e);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		if (e.KeyCode == Keys.ControlKey && SidebarItems != null)
		{
			foreach (var item in SidebarItems)
			{
				if (item.Highlighted && !item.Selected)
				{
					item.MouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
				}

				item.Highlighted = false;
			}

			base_P_Tabs.Invalidate();
		}

		if (e.KeyCode == Keys.ControlKey && SidebarItems != null)
		{
			foreach (var item in SidebarItems)
			{
				item.ShowKey = null;
			}

			base_P_Tabs.Invalidate();
		}

		base.OnKeyUp(e);
	}

	private void mouseDetector_MouseMove(object sender, Point p)
	{
		if (AutoHideMenu && CurrentFormState != FormState.NormalUnfocused && WindowState != FormWindowState.Minimized)
		{
			var close = !base_P_Side.Bounds.Pad(-30).Contains(base_P_Side.PointToClient(p));
			var animation = AnimationHandler.GetAnimation(base_P_Side, AnimationOption.IgnoreHeight);
			var newSize = new Size(close ? 0 : (int)((smallMenu ? 55 : 180) * UI.UIScale), 0);

			if (!IsHandleCreated || DesignMode || !menuSetUp)
			{
				base_P_Side.Size = newSize;
			}
			else if (animation == null || close != (animation.NewBounds.Width == 0))
			{
				new AnimationHandler(base_P_Side, newSize, 2.25, AnimationOption.IgnoreHeight)
					.StartAnimation();
			}
		}

		CurrentPanel?.GlobalMouseMove(p);
	}

	private void setSmallMenu()
	{
		base_P_SideControls.Visible = !smallMenu;
		base_PB_Icon.Size = UI.Scale(smallMenu ? new Size(26, 26) : new Size(32, 32), UI.UIScale);

		var newSize = new Size((int)((smallMenu ? 55 : 180) * UI.FontScale), 0);

		if (!IsHandleCreated || !menuSetUp)
		{
			base_P_Side.Size = newSize;
		}
		else
		{
			var handler = new AnimationHandler(base_P_Side, newSize, 2.25, AnimationOption.IgnoreHeight);
			handler.StartAnimation();
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		base.OnFormClosing(e);

		if (e.CloseReason == CloseReason.UserClosing)
		{
			e.Cancel = !(CurrentPanel?.CanExit(true) ?? true);
		}
	}
}