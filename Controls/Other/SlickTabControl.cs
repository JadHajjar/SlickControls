using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace SlickControls;

public partial class SlickTabControl : SlickControl
{
	private string noResults;
	private List<Tab> tabs = [];

	[DefaultValue(true)]
	public new bool Enabled { get => P_Tabs.Enabled; set => P_Tabs.Enabled = value; }

	[Category("Behavior")]
	public Tab[] Tabs
	{
		get => tabs.ToArray() ?? []; set
		{
			tabs = new(value);

			if (IsHandleCreated)
			{
				GenerateTabs();
			}
		}
	}

	public Size ContentSize => P_Content.Size;

	public SlickTabControl()
	{
		InitializeComponent();
	}

	public void RemoveTab(Tab tab)
	{
		tabs.Remove(tab);

		UIChanged();
	}

	protected override void DesignChanged(FormDesign design)
	{
		P_Tabs.BackColor = design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? -4 : 4);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		GenerateTabs();

		if (!Live)
		{
			P_Tabs.Height = UI.Scale(48) + P_Tabs.Padding.Vertical;
		}
	}

	protected override void UIChanged()
	{
		P_Tabs.Padding = UI.Scale(new Padding(10));
		P_Tabs.Height = UI.Scale(48) + P_Tabs.Padding.Vertical;
		panel1.Padding = UI.Scale(new Padding(3));

		foreach (Control item in P_Tabs.Controls)
		{
			item.Size = new Size(GetTabWidth(), P_Tabs.Height);
		}
	}

	private void GenerateTabs()
	{
		if (!IsHandleCreated)
		{
			return;
		}

		foreach (var tab in tabs)
		{
			tab.Size = new Size(GetTabWidth(), P_Tabs.Height);
			tab.Dock = DockStyle.Left;
			tab.Selected = false;
			tab.TabSelected -= Tab_TabSelected;
			tab.TabSelected += Tab_TabSelected;

			if (tab.LinkedControl != null)
			{
				tab.LinkedControl.Parent = null;
				tab.LinkedControl.Dock = DockStyle.None;
				tab.LinkedControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			}
		}

		P_Tabs.Controls.Clear();
		P_Tabs.Controls.AddRange((tabs as IEnumerable<Tab>).Reverse().ToArray());

		if (tabs.Count > 0)
		{
			tabs.Where(x => x.Enabled && x.Visible).FirstOrAny(x => x.PreSelected).Selected = true;
		}
	}

	private void P_Tabs_PaddingChanged(object sender, EventArgs e)
	{
		UIChanged();
	}

	private void SlickTabControl_Resize(object sender, EventArgs e)
	{
		foreach (var tab in tabs)
		{
			tab.Width = GetTabWidth();
		}
	}

	private void Tab_TabSelected(object sender, EventArgs e)
	{
		P_Content.TryInvoke(() =>
		{
			if (ScrollBar?.LinkedControl != null)
			{
				ScrollBar.LinkedControl.ControlAdded -= Ctrl_ControlAdded;
			}

			var tab = sender as Tab;
			var ctrl = tab.LinkedControl;
			noResults = tab.NoControlText.IfEmpty(LocaleSlickUI.NothingToSeeHere);

			P_Content.Controls.Clear(false, x => x != ScrollBar);
			P_Content.Invalidate();

			Application.DoEvents();

			if (ctrl != null)
			{
				ctrl.Location = Point.Empty;
				ctrl.Parent = P_Content;
				ctrl.ControlAdded += Ctrl_ControlAdded;

				if (!tab.FillTab)
				{
					if (!ScrollBar.IsHandleCreated)
					{
						ScrollBar.Show();
						ScrollBar.CreateControl();
					}

					ScrollBar.LinkedControl = ctrl;
					ScrollBar.Reset();
				}
				else
				{
					ScrollBar.Visible = false;
					ScrollBar.LinkedControl = null;
					ctrl.Dock = DockStyle.Fill;
				}

				ctrl.ResetFocus();
			}

			P_Content.Invalidate();
		});
	}

	private void Ctrl_ControlAdded(object sender, ControlEventArgs e)
	{
		P_Content.Invalidate();
	}

	private int GetTabWidth()
	{
		return ((P_Tabs.Width - Padding.Horizontal - P_Tabs.Padding.Left) / tabs.Count(x => x.Visible)).Between(UI.Scale(16), UI.Scale(110));
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == (Keys.Control | Keys.Tab))
		{
			var next = tabs.Where(x => x.Enabled && x.Visible).Next(tabs.FirstOrDefault(x => x.Selected)) ?? tabs.FirstOrDefault(x => x.Enabled && x.Visible);
			if (next != null)
			{
				next.Selected = true;
			}

			return true;
		}

		if (keyData == (Keys.Control | Keys.Shift | Keys.Tab))
		{
			var prev = tabs.Where(x => x.Enabled && x.Visible).Previous(tabs.FirstOrDefault(x => x.Selected)) ?? tabs.LastOrDefault(x => x.Enabled && x.Visible);
			if (prev != null)
			{
				prev.Selected = true;
			}

			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void P_Content_Paint(object sender, PaintEventArgs e)
	{
		if (P_Content.Controls.Count == 1)
		{
			e.Graphics.DrawString("Loading..", P_Content.Font, new SolidBrush(FormDesign.Design.InfoColor), new Rectangle(Point.Empty, P_Content.Size), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
		else if (ScrollBar?.LinkedControl?.Controls?.Cast<Control>()?.All(x => !x.Visible) ?? true)
		{
			e.Graphics.DrawString(noResults, P_Content.Font, new SolidBrush(FormDesign.Design.InfoColor), new Rectangle(Point.Empty, P_Content.Size), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
	}

	#region Design Click

	[DefaultEvent("TabSelected"), Designer(typeof(TabButtonDesigner))]
	public class Tab : SlickTab
	{
		[Category("Appearance"), DefaultValue(null)]
		public string NoControlText { get; set; }

		[Category("Behavior"), DefaultValue(false)]
		public bool FillTab { get; set; }

		[Category("Behavior"), DefaultValue(false)]
		public bool PreSelected { get; set; }
	}

	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	private class TabButtonDesigner : System.Windows.Forms.Design.ControlDesigner
	{
		private Adorner myAdorner;
		private ShowTabGlyph myGlyph = null;

		public TabButtonDesigner()
		{
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);

			BehaviorService.Adorners.Add(myAdorner = new Adorner());

			myGlyph = new ShowTabGlyph(BehaviorService, Control)
			{
				Callback = () =>
				{
					if (Control is SlickTab tab)
					{
						tab.Selected = true;
					}
				}
			};

			myAdorner.Glyphs.Add(myGlyph);
		}

		private class ShowTabGlyph : Glyph
		{
			private readonly BehaviorService behaviorSvc;
			private readonly Control control;

			public override Rectangle Bounds
				=> new Rectangle(behaviorSvc.ControlToAdornerWindow(control), control.Size).Pad(30, 1, 30, 1);

			public Action Callback
			{
				get;
				set;
			}

			public ShowTabGlyph(BehaviorService behaviorSvc, Control control)
				: base(new ShowTabBehavior())
			{
				this.behaviorSvc = behaviorSvc;
				this.control = control;
			}

			public override Cursor GetHitTest(Point p)
			{
				return Bounds.Contains(p) ? Cursors.Hand : null;
			}

			public override void Paint(PaintEventArgs pe)
			{
				using var pen = new Pen(FormDesign.Modern.AccentColor, 1.5F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
				pe.Graphics.DrawRectangle(pen, new Rectangle(behaviorSvc.ControlToAdornerWindow(control), control.Size).Pad(1));
				pen.Color = Color.FromArgb(100, FormDesign.Modern.ActiveColor);
				pe.Graphics.DrawRectangle(pen, Bounds);
			}

			private class ShowTabBehavior : Behavior
			{
				public override bool OnMouseUp(Glyph g, MouseButtons button)
				{
					((ShowTabGlyph)g).Callback?.Invoke();

					return base.OnMouseUp(g, button);
				}
			}
		}
	}

	#endregion Design Click
}