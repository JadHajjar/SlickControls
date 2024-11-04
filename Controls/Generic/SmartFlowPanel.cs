using Extensions;

using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SmartFlowPanel : FlowLayoutPanel
{
	[Category("Behavior"), DefaultValue(false)]
	public bool Horizontal { get; set; }

	protected override void OnLayout(LayoutEventArgs levent)
	{
		base.OnLayout(levent);

		if (Horizontal)
		{
			var width = Controls.Count == 0 ? 0 : Controls.Max(x => x.Visible ? x.Right + x.Margin.Right : 0);

			width += Padding.Right;

			if (Width != width)
			{
				Width = width;
			}
		}
		else
		{
			var height = Controls.Count == 0 ? 0 : Controls.Max(x => x.Visible ? x.Bottom + x.Margin.Bottom : 0);

			height += Padding.Bottom;

			if (Height != height)
			{
				Height = height;
			}
		}
	}
}
