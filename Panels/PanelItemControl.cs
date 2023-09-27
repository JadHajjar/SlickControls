using Extensions;

using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	internal class PanelItemControl : SlickStackedListControl<PanelTab, PanelItemControl.Rectangles>
	{
		public event MouseEventHandler OnFormMove;

		public BasePanelForm Form { get; }

		public PanelItemControl(BasePanelForm form)
		{
			Form = form;
			ItemHeight = 24;
			TabStop = false;
			DynamicSizing = true;
		}

		protected override void CanDrawItemInternal(CanDrawItemEventArgs<PanelTab> args)
		{
			args.DoNotDraw =
				(args.Item.IsGroupHeader && (Form?.SmallMenu ?? false)) ||
				args.Item.PanelItem?.Hidden == true ||
				(args.Item.IsSubItem && !(args.Item.ParentItem.Selected || args.Item.ParentItem.SubItems.Any(x => x.Selected)));

			base.CanDrawItemInternal(args);
		}

		protected override void OnPaintItemList(ItemPaintEventArgs<PanelTab, Rectangles> e)
		{
			e.DrawableItem.CachedHeight = ItemHeight;

			e.Item.Paint(e, Form?.SmallMenu ?? false);
		}

		protected override void OnItemMouseClick(DrawableItem<PanelTab, Rectangles> item, MouseEventArgs e)
		{
			item.Item.PanelItem?.MouseClick(e);
		}

		protected override bool IsItemActionHovered(DrawableItem<PanelTab, Rectangles> item, Point location)
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

		internal class Rectangles : IDrawableItemRectangles<PanelTab>
		{
			public PanelTab Item { get; set; }

			public bool GetToolTip(Control instance, Point location, out string text, out Point point)
			{
				text = null;
				point = default;
				return false;
			}

			public bool IsHovered(Control instance, Point location)
			{
				return true;
			}
		}
	}
}