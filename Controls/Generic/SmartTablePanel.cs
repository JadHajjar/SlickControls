﻿using Extensions;

using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SmartTablePanel : TableLayoutPanel
{
	protected override void OnLayout(LayoutEventArgs levent)
	{
		base.OnLayout(levent);

		var height = Controls.Count == 0 ? 0 : Controls.Max(x => x.Visible ? x.Bottom + x.Margin.Bottom : 0);

		height += Padding.Bottom;

		if (Height != height)
		{
			Height = height;
		}
	}
}
