using System;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickImageBackgroundFlowLayoutPanel : SlickImageBackgroundPanel
	{
		private Rectangle lastControlBounds;
		public FlowDirection FlowDirection { get; set; }

		public SlickImageBackgroundFlowLayoutPanel()
		{
		}

		public override void OnPaint(PaintEventArgs e)
		{
			lastControlBounds = new Rectangle(0, 0, 0, 0);

			base.OnPaint(e);
		}

		public override void CalculateSize(PaintEventArgs e)
		{
			lastControlBounds = new Rectangle(0, 0, 0, 0);

			base.CalculateSize(e);
		}

		protected override void SetControlBounds(SlickImageBackgroundControl control)
		{
			if (control.Dock == DockStyle.None)
			{
				control.Location = new Point(lastControlBounds.X + lastControlBounds.Width, lastControlBounds.Y);
				if (control.Bounds.X + control.Width > Width)
					control.Location = new Point(0, lastControlBounds.Y + lastControlBounds.Height);
			}
			else
				base.SetControlBounds(control);
		}

		protected override void PostControlPaint(SlickImageBackgroundControl control, bool painted)
		{
			if (control.Dock == DockStyle.None)
				lastControlBounds = new Rectangle(control.Location,
					new Size(control.Width + control.Margin.Horizontal
					, Math.Max(control.Height + control.Margin.Vertical, lastControlBounds.Height)));
			else
				base.PostControlPaint(control, painted);
		}
	}
}