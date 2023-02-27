using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class BasePanelForm : SlickForm
	{
		#region Private Fields

		private TableLayoutPanel base_TLP_PanelItems;
		private Image formIcon;
		private readonly List<PanelContent> panelHistory = new List<PanelContent>();
		private PanelItem[] sidebarItems = new PanelItem[0];
		private bool autoHideMenu;
		private MouseDetector mouseDetector;
		private bool smallMenu;
		private bool hideMenu;

		#endregion Private Fields

		#region Public Properties

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
					base_P_Side.BringToFront();

				if (IsHandleCreated)
					ISave.Save(new { AutoHideMenu, SmallMenu }, "PanelForm.tf", true, "Shared");
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
					mouseDetector_MouseMove(null, Cursor.Position);

				if (IsHandleCreated)
					ISave.Save(new { AutoHideMenu, SmallMenu }, "PanelForm.tf", true, "Shared");
			}
		}

		[DefaultValue(false)]
		public bool HideMenu
		{
			get => hideMenu;
			set
			{
				base_P_Side.Visible = !(hideMenu = value);
			}
		}

		#endregion Public Properties

		#region Protected Properties

		protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				cp.Style |= 0x20000; // <--- use 0x20000
				return cp;
			}
		}

		#endregion Protected Properties

		#region Public Constructors

		public BasePanelForm() : this(false)
		{ }

		public BasePanelForm(bool initialized)
		{
			InitializeComponent();

			if (DesignMode)
				SetPanel<PanelContent>(null);

			if (!initialized)
				FormDesign.DesignChanged += DesignChanged;

			base_P_Tabs.MouseDown += Form_MouseDown;
			base_P_Icon.MouseDown += Form_MouseDown;
			base_P_SideControls.MouseDown += Form_MouseDown;
		}

		#endregion Public Constructors

		#region Public Methods

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
					panelHistory.Remove(panel);
			}
		}

		public bool PushPanel<T>(PanelItem panelItem) where T : PanelContent, new()
		{
			if (CurrentPanel != null)
			{
				if (!panelHistory.Contains(CurrentPanel))
					panelHistory.Add(CurrentPanel);
				else
				{
					panelHistory.Remove(CurrentPanel);
					panelHistory.Insert(panelHistory.Count - 1, CurrentPanel);
				}
				base_P_PanelContent.Controls.Remove(CurrentPanel);
				CurrentPanel.CanExit(false);
				CurrentPanel = null;
			}

			return SetPanel<T>(panelItem, false, false);
		}

		public bool PushPanel(PanelItem panelItem, PanelContent panelContent)
		{
			if (CurrentPanel == panelContent) return false;

			if (CurrentPanel != null)
			{
				if (!panelHistory.Contains(CurrentPanel))
					panelHistory.Add(CurrentPanel);
				else
				{
					panelHistory.Remove(CurrentPanel);
					panelHistory.Insert(panelHistory.Count - 1, CurrentPanel);
				}
				base_P_PanelContent.Controls.Remove(CurrentPanel);
				CurrentPanel.CanExit(false);
				CurrentPanel = null;
			}

			return SetPanel(panelItem, panelContent, false, false);
		}

		public bool SetPanel<T>(PanelItem panelItem, bool dispose = true, bool clearHistory = true) where T : PanelContent, new()
		{
			if (CurrentPanel != null && ((CurrentPanel.PanelItem == panelItem && !(panelItem?.ForceReopen ?? false)) || !CurrentPanel.CanExit(dispose)))
				return false;

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
							return false;
					}
				}

				var newPanel = new T
				{
					Size = base_P_PanelContent.Size,
					Dock = DockStyle.Fill,
					PanelItem = panelItem,
					Form = this
				};

				base_P_PanelContent.SuspendDrawing();

				if (CurrentPanel != null)
					base_P_PanelContent.Controls.Remove(CurrentPanel);

				if (dispose)
					handleDispose(CurrentPanel);

				CurrentPanel = newPanel;
			}
			catch { return false; }

			base_B_Close.Visible = base_B_Max.Visible = base_B_Min.Visible = !CurrentPanel.HideWindowIcons;

			if (!CurrentPanel.PanelWasSetUp)
			{
				RecursiveMouseDown(CurrentPanel);
				CurrentPanel.PanelWasSetUp = true;
			}

			if (CurrentPanel.AcceptButton != null)
			{
				var btn = new Button();
				AcceptButton = btn;
				btn.Click += (s, e) => { if (CurrentPanel.AcceptButton.Enabled) CurrentPanel.AcceptButton.OnClick(e); };
			}
			else
			{
				AcceptButton = null;
			}

			if (CurrentPanel.CancelButton != null)
			{
				var btn = new Button();
				CancelButton = btn;
				btn.Click += (s, e) => { if (CurrentPanel.CancelButton.Enabled) CurrentPanel.CancelButton.OnClick(e); };
			}
			else
			{
				CancelButton = null;
			}

			base_P_PanelContent.Controls.Add(CurrentPanel);

			if (base_TLP_PanelItems != null && panelItem != null)
			{
				foreach (var item in base_TLP_PanelItems.Controls.ThatAre<PanelItemControl>())
					item.Selected = item.PanelItem == panelItem;
			}

			CurrentPanel.OnShown();

			base_P_PanelContent.ResumeDrawing();

			if (!CurrentPanel.Focus() && IsHandleCreated)
				BeginInvoke(new Action(() => CurrentPanel.Focus()));

			return true;
		}

		public bool SetPanel(PanelItem panelItem, PanelContent panelContent, bool dispose = true, bool clearHistory = true)
		{
			if (CurrentPanel != null && (
					(CurrentPanel.PanelItem != null && panelItem != PanelItem.Empty && CurrentPanel.PanelItem == panelItem && !panelItem.ForceReopen && CurrentPanel.GetType() == panelContent.GetType())
					|| (dispose && !CurrentPanel.CanExit(dispose))
				))
				return false;

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
						return false;
				}
			}

			base_P_PanelContent.SuspendDrawing();

			if (dispose)
				handleDispose(CurrentPanel);
			else if (CurrentPanel != null)
				base_P_PanelContent.Controls.Remove(CurrentPanel);

			CurrentPanel = panelContent;

			CurrentPanel.Size = base_P_PanelContent.Size;
			CurrentPanel.Dock = DockStyle.Fill;
			CurrentPanel.PanelItem = panelItem;
			CurrentPanel.Form = this;

			base_B_Close.Visible = base_B_Max.Visible = base_B_Min.Visible = !CurrentPanel.HideWindowIcons;

			if (!CurrentPanel.PanelWasSetUp)
			{
				RecursiveMouseDown(CurrentPanel);
				CurrentPanel.PanelWasSetUp = true;
			}

			if (CurrentPanel.AcceptButton != null)
			{
				var btn = new Button();
				AcceptButton = btn;
				btn.Click += (s, e) => { if (CurrentPanel.AcceptButton.Enabled) CurrentPanel.AcceptButton.OnClick(e); };
			}
			else
			{
				AcceptButton = null;
			}

			if (CurrentPanel.CancelButton != null)
			{
				var btn = new Button();
				CancelButton = btn;
				btn.Click += (s, e) => { if (CurrentPanel.CancelButton.Enabled) CurrentPanel.CancelButton.OnClick(e); };
			}
			else
			{
				CancelButton = null;
			}

			base_P_PanelContent.Controls.Add(CurrentPanel);

			if (base_TLP_PanelItems != null && panelItem != null)
			{
				foreach (var item in base_TLP_PanelItems.Controls.OfType<PanelItemControl>())
					item.Selected = item.PanelItem == panelItem;
			}

			panelContent.Visible = true;

			CurrentPanel.OnShown();

			base_P_PanelContent.ResumeDrawing();

			if (!CurrentPanel.Focus() && IsHandleCreated)
				BeginInvoke(new Action(() => CurrentPanel.Focus()));

			return true;
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void UIChanged()
		{
			base.UIChanged();

			base_P_Side.Width = (int)(165 * UI.UIScale);

			if (base_TLP_PanelItems != null)
			{
				base_TLP_PanelItems.MaximumSize = new Size(base_P_Side.Width, 9999);
				base_TLP_PanelItems.MinimumSize = new Size(base_P_Side.Width, 0);
				base_TLP_PanelItems.Width = base_P_Side.Width;
			}

			base_P_Tabs.Font = UI.Font(8.25F, FontStyle.Bold);
			base_P_Icon.Height = (int)(70 * UI.UIScale);
			base_PB_Icon.Size = UI.Scale(new Size(32, 32), UI.UIScale);
			base_B_Close.Size = base_B_Max.Size = base_B_Min.Size = new Size(6 + (int)(16 * UI.UIScale), 6 + (int)(16 * UI.UIScale));

			if (SmallMenu)
				setSmallMenu();
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			base_P_PanelContent.BackColor = base_TLP_TopButtons.BackColor = design.BackColor;
			base_P_Content.BackColor = design.MenuColor;
			base_P_Side.ForeColor = design.LabelColor;
			base_P_SideControls.ForeColor = design.LabelColor.MergeColor(design.ID.If(0, design.AccentColor, design.MenuColor), 80);

			if (base_TLP_PanelItems != null)
			{
				foreach (var item in base_TLP_PanelItems.Controls.ThatAre<Panel>())
					item.BackColor = design.AccentColor;
			}

			base_PB_Icon.Color(design.MenuForeColor);
		}

		protected void DisableSideBar()
		{
			foreach (var item in base_TLP_PanelItems.Controls.ThatAre<PanelItemControl>())
				item.Enabled = false;
		}

		protected void EnableSideBar()
		{
			foreach (var item in base_TLP_PanelItems.Controls.ThatAre<PanelItemControl>())
				item.Enabled = true;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (!DesignMode)
			{
				mouseDetector = new MouseDetector();
				mouseDetector.MouseMove += mouseDetector_MouseMove;

				var options = ISave.LoadRaw("PanelForm.tf", "Shared");

				if (options != null)
				{
					AutoHideMenu = options.AutoHideMenu;
					SmallMenu = options.SmallMenu;
				}
			}

			base_P_SideControls?.BringToFront();
			base_TLP_PanelItems?.BringToFront();
			OnNextIdle(() => CurrentPanel?.Focus());
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (CurrentPanel != null && CurrentPanel.KeyPressed(e.KeyChar))
				return;

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
				return true;

			if (HandleKeyPress?.Invoke(msg, keyData) ?? false)
				return true;

			if (keyData == Keys.Escape && CancelButton == null)
			{
				if (PanelHistory.Any())
				{ PushBack(); return true; }
				else if (WindowState == FormWindowState.Maximized)
				{ WindowState = FormWindowState.Normal; return true; }
			}

			if (keyData == (Keys.Shift | Keys.Escape) && (SidebarItems?.All(x => x.Control != null) ?? false))
			{
				foreach (var item in SidebarItems)
				{
					item.Highlighted = false;
					item.Control.Invalidate();
				}

				return true;
			}

			if ((keyData == (Keys.Shift | Keys.Up) || keyData == (Keys.Shift | Keys.Down)) && (SidebarItems?.All(x => x.Control != null) ?? false))
			{
				var prev = SidebarItems.FirstOrDefault(x => x.Highlighted) ?? SidebarItems.FirstOrDefault(x => x.Control.Selected);
				var item = (keyData == (Keys.Shift | Keys.Up) ? SidebarItems.Previous(prev, true) : SidebarItems.Next(prev, true)) ?? SidebarItems.FirstOrDefault();

				if (prev != null)
				{
					prev.Highlighted = false;
					prev.Control.Invalidate();
				}

				if (item != null)
				{
					item.Highlighted = true;
					item.Control.Invalidate();
				}

				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void WndProc(ref Message m)
		{
			if (!(CurrentPanel?.OnWndProc(m) ?? false) && !HandleWndProc(ref m))
			{
				if (m.Msg == 0x210 && m.WParam == (IntPtr)0x1020b && PanelHistory.Any())
					PushBack();

				if (m.Msg == 0x20e && PanelHistory.Any())
				{
					var newPad = new Padding(Math.Min((int)(120 * UI.UIScale), base_P_PanelContent.Padding.Left - m.WParam.ToInt32() / 65536), 0, 0, 0); ;
					if (newPad != base_P_PanelContent.Padding)
					{
						base_P_PanelContent.Padding = newPad;
						base_P_PanelContent.Invalidate();
					}
					else
					{
						base_P_PanelContent.Padding = Padding.Empty;
						PushBack();
					}
				}
				else if (base_P_PanelContent.Padding.Left != 0 && m.Msg != 0x20)
					AnimationHandler.Animate(base_P_PanelContent, Padding.Empty, 2);
			}
		}

		#endregion Protected Methods

		#region Private Methods

		[DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		private void handleDispose(PanelContent panel)
		{
			if (panel != null)
			{
				panel.Parent = null;
				OnNextIdle(() =>
				{
					panel.Dispose();
					GC.Collect();
				});
			}
		}

		private void GenerateTabs()
		{
			base_TLP_PanelItems?.Dispose();
			base_TLP_PanelItems = new TableLayoutPanel()
			{
				MaximumSize = new Size(base_P_Side.Width, 9999),
				MinimumSize = new Size(base_P_Side.Width, 0),
				AutoSize = true
			};

			base_TLP_PanelItems.MouseDown += Form_MouseDown;

			foreach (var group in sidebarItems.Select(x => x.Group).Distinct())
			{
				if (!string.IsNullOrWhiteSpace(group))
				{
					var label = new Label()
					{
						Text = group.ToUpper(),
						Margin = new Padding(7, 10, 0, 4),
						AutoSize = true,
						Visible = !smallMenu
					};

					base_TLP_PanelItems.Controls.Add(label);
				}

				var items = sidebarItems.Where(x => x.Group == group);

				foreach (var item in items)
				{
					var panelitem = new PanelItemControl(item)
					{
						Margin = new Padding(0)
					};

					base_TLP_PanelItems.Controls.Add(panelitem);
				}

				if (group != sidebarItems.Select(x => x.Group).Distinct().Last())
				{
					base_TLP_PanelItems.Controls.Add(new SlickSpacer
					{
						Size = new Size(base_P_Side.Width, 6),
						Dock = DockStyle.Top,
						Padding = smallMenu ? new Padding(0, 3, 0, 2) : new Padding(15, 5, 15, 0),
						Margin = new Padding(0)
					});
				}
			}

			for (var i = 0; i < base_TLP_PanelItems.RowCount; i++)
				base_TLP_PanelItems.RowStyles[i].SizeType = SizeType.AutoSize;

			base_P_Tabs.Controls.Add(base_TLP_PanelItems);
			base_SideScroll.LinkedControl = base_TLP_PanelItems;
		}

		private void RecursiveMouseDown(Control ctrl)
		{
			if (ctrl is Panel || ctrl is UserControl)
			{
				ctrl.MouseDown += Form_MouseDown;

				foreach (var item in ctrl.Controls.ThatAre<Panel>().Where(x => x.Tag?.ToString() != "NoMouseDown"))
					RecursiveMouseDown(item);

				foreach (var item in ctrl.Controls.ThatAre<Label>().Where(x => x.Tag?.ToString() != "NoMouseDown"))
					item.MouseDown += Form_MouseDown;
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
			if (e.KeyCode == Keys.ShiftKey && (SidebarItems?.All(x => x.Control != null) ?? false))
			{
				foreach (var item in SidebarItems)
				{
					item.Highlighted = item.Control.Selected;
					item.Control.Invalidate();
				}
			}

			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ShiftKey && (SidebarItems?.All(x => x.Control != null) ?? false))
			{
				foreach (var item in SidebarItems)
				{
					if (item.Highlighted && !item.Control.Selected)
						item.MouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));

					item.Highlighted = false;
					item.Control.Invalidate();
				}
			}

			base.OnKeyUp(e);
		}

		private void base_P_PanelContent_Paint(object sender, PaintEventArgs e)
		{
			if (base_P_PanelContent.Padding.Left != 0)
			{
				e.Graphics.DrawImage(Properties.Resources.Huge_Back.Color(FormDesign.Design.ActiveColor), new Rectangle(base_P_PanelContent.Padding.Left / 2 - 32, base_P_PanelContent.Height / 2 - 32, 64, 64));
				if (base_P_PanelContent.Padding.Left < 120)
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255 - (255 * Math.Max(0, (base_P_PanelContent.Padding.Left - 20)) / 100), base_P_PanelContent.BackColor)), new Rectangle(base_P_PanelContent.Padding.Left / 2 - 32, base_P_PanelContent.Height / 2 - 32, 64, 64));
			}
		}

		private void mouseDetector_MouseMove(object sender, Point p)
		{
			if (AutoHideMenu && CurrentFormState != FormState.NormalUnfocused && WindowState != FormWindowState.Minimized)
			{
				var close = !base_P_Side.Bounds.Pad(-30).Contains(base_P_Side.PointToClient(p));
				var animation = AnimationHandler.GetAnimation(base_P_Side, AnimationOption.IgnoreHeight);
				var newSize = new Size(close ? 0 : smallMenu ? 46 : (int)(165 * UI.UIScale), 0);

				if (!IsHandleCreated || DesignMode)
					base_P_Side.Size = newSize;
				else if (animation == null || close != (animation.NewBounds.Width == 0))
					new AnimationHandler(base_P_Side, newSize, 2.25, AnimationOption.IgnoreHeight)
						.StartAnimation();
			}
		}

		private void setSmallMenu()
		{
			PanelItemControl.DrawText = !smallMenu;
			base_P_SideControls.Visible = !smallMenu;
			base_PB_Icon.Size = smallMenu ? new Size(26, 26) : UI.Scale(new Size(32, 32), UI.UIScale);

			if (base_TLP_PanelItems != null)
				foreach (Control item in base_TLP_PanelItems.Controls)
				{
					if (item is Label)
						item.Visible = !smallMenu;
					else if (item is SlickSpacer)
						item.Padding = smallMenu ? new Padding(0, 3, 0, 2) : new Padding(15, 5, 15, 0);
				}

			var newSize = new Size(smallMenu ? 46 : (int)(165 * UI.UIScale), 0);

			if (!IsHandleCreated)
				base_P_Side.Size = newSize;
			else
			{
				var handler = new AnimationHandler(base_P_Side, newSize, 2.25, AnimationOption.IgnoreHeight);
				handler.OnAnimationTick += (_, control, finished) =>
				{
					base_TLP_PanelItems.Width = base_P_Side.Width;
					foreach (Control item in base_TLP_PanelItems.Controls)
					{
						if (item is PanelItemControl c)
							item.Width = base_TLP_PanelItems.Width;
					}
				};
				handler.StartAnimation();
			}
		}

		#endregion Private Methods
	}
}