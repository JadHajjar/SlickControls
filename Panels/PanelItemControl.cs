using Extensions;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls
{
	internal class PanelItemControl : SlickStackedListControl<PanelTab>
	{
		public event MouseEventHandler OnFormMove;

		public BasePanelForm Form { get; }

		public PanelItemControl(BasePanelForm form)
        {
			Form = form;
			ItemHeight = 24;
			TabStop = false;

			CanDrawItem += PanelItemControl_CanDrawItem;
		}

		private void PanelItemControl_CanDrawItem(object sender, CanDrawItemEventArgs<PanelTab> e)
		{
			e.DoNotDraw = e.Item.IsGroupHeader && (Form?.SmallMenu ?? false);
		}

		protected override void OnPaintItem(ItemPaintEventArgs<PanelTab> e)
		{
			e.Item.Paint(e, (Form?.SmallMenu ?? false));
		}

		protected override void OnItemMouseClick(DrawableItem<PanelTab> item, MouseEventArgs e)
		{
			item.Item.PanelItem?.MouseClick(e);
		}

		protected override bool IsItemActionHovered(DrawableItem<PanelTab> item, Point location)
		{
			return item.Item.PanelItem != null;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (mouseDownItem == null && scrollMouseDown < 0)
			{
				OnFormMove?.Invoke(this, e);
			}
		}
	}
}