using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;
public class SlickStackedPanel : Panel
{
	[Category("Behavior"), DefaultValue(0)]
	public int ColumnWidth { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool ColumnWidthIsPercentage { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool Center { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool SmartAutoSize { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool MaximizeSpaceUsage { get; set; }

	protected override void OnLayout(LayoutEventArgs levent)
	{
		this.SuspendDrawing();

		base.OnLayout(levent);

		DoLayout();

		this.ResumeDrawing();
	}

	private void DoLayout()
	{
		if (Controls.Count == 0 || !IsHandleCreated)
		{
			return;
		}

		var columnWidth = Math.Min(Width - Padding.Horizontal, ColumnWidth == 0 ? Controls[0].Width.If(0, 1) : ColumnWidthIsPercentage ? ((Width - Padding.Horizontal) * ColumnWidth / 100) : (int)(ColumnWidth * UI.FontScale));
		var columns = (int)Math.Max(1, Math.Floor((Width - Padding.Horizontal) / (float)columnWidth));

		if (MaximizeSpaceUsage)
		{
			columnWidth = (Width - Padding.Horizontal) / columns;
		}

		var startingX = Padding.Left + (Center ? (Width - Padding.Horizontal - (columns * columnWidth)) / 2 : 0);
		var currentY = new int[columns];
		var index = 0;

		for (var i = 0; i < Controls.Count; i++)
		{
			var control = Controls[i];

			if (!control.Visible)
			{
				continue;
			}

			if (MaximizeSpaceUsage)
			{
				var min = currentY.Min();

				for (var j = 0; j < columns; j++)
				{
					if (currentY[j] == min)
					{
						index = j;
						break;
					}
				}
			}

			var bounds = new Rectangle(startingX + (index * columnWidth), currentY[index] + Padding.Top, columnWidth, control.Height + control.Margin.Vertical).Pad(control.Margin);

			if (control.Bounds != bounds)
			{
				control.Bounds = bounds;
			}

			currentY[index] += Padding.Top + control.Height + control.Margin.Vertical;

			index = (index + 1) % columns;
		}

		if (SmartAutoSize)
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
