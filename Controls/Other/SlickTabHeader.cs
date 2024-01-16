using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public partial class SlickTabHeader : Panel
{
	private SlickTab[] tabs;

	[DefaultValue(typeof(Padding), "0, 5, 0, 0")]
	public new Padding Padding { get => base.Padding; set => base.Padding = value; }

	[Category("Behavior")]
	public SlickTab[] Tabs
	{
		get => tabs ?? new SlickTab[0]; set
		{
			tabs = value;
			if (IsHandleCreated)
			{
				GenerateTabs();
			}
		}
	}

	public SlickTabHeader()
	{
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		GenerateTabs();

		Padding = UI.Scale(Padding, UI.FontScale);

		Height = (int)(32 * UI.FontScale) + Padding.Vertical;
	}

	private void GenerateTabs()
	{
		if (!IsHandleCreated)
		{
			return;
		}

		foreach (var tab in Tabs)
		{
			tab.Size = new Size(TabWidth(), Height);
			tab.Dock = DockStyle.Left;
			tab.Selected = false;

			if (tab.LinkedControl != null)
			{
				tab.LinkedControl.Parent = null;
				tab.LinkedControl.Dock = DockStyle.None;
				tab.LinkedControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			}
		}

		Controls.Clear(true);
		Controls.AddRange(Tabs.Reverse().ToArray());

		foreach (var tab in Tabs)
		{
			tab.Width = TabWidth();
		}

		if (Tabs.Length > 0)
		{
			Tabs[0].Selected = true;
		}
	}

	protected override void OnResize(EventArgs eventargs)
	{
		base.OnResize(eventargs);

		foreach (var tab in Tabs)
		{
			tab.Width = TabWidth();
		}
	}

	private int TabWidth()
	{
		return ((Width - Padding.Horizontal) / Tabs.Length).Between((int)(32 * UI.FontScale), (int)(200 * UI.FontScale));
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == (Keys.Control | Keys.Tab))
		{
			Next();
			return true;
		}

		if (keyData == (Keys.Control | Keys.Shift | Keys.Tab))
		{
			Previous();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	public void Next()
	{
		var next = Tabs.Where(x => x.Enabled).Next(Tabs.FirstOrDefault(x => x.Selected)) ?? Tabs.FirstOrDefault(x => x.Enabled);
		if (next != null)
		{
			next.Selected = true;
		}
	}

	public void Previous()
	{
		var prev = Tabs.Where(x => x.Enabled).Previous(Tabs.FirstOrDefault(x => x.Selected)) ?? Tabs.LastOrDefault(x => x.Enabled);
		if (prev != null)
		{
			prev.Selected = true;
		}
	}
}