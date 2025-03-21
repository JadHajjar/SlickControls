﻿using Extensions;

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class PanelItemControl : SlickStackedListControl<PanelTab>
{
	public event MouseEventHandler OnFormMove;

	public BasePanelForm Form { get; }

	public PanelItemControl(BasePanelForm form)
	{
		Form = form;
		ItemHeight = 24;
		TabStop = false;
	}

	public override void SetItems(IEnumerable<PanelTab> items)
	{
		base.SetItems(items);

		foreach (var item in items)
		{
			if (item.PanelItem != null)
			{
				item.PanelItem.LoadingStateChanged += PanelItem_LoadingStateChanged;
			}
		}
	}

	private void PanelItem_LoadingStateChanged(object sender, System.EventArgs e)
	{
		Loading = Items.Any(x => x.PanelItem?.Loading == true);
	}

	protected override void CanDrawItemInternal(CanDrawItemEventArgs<PanelTab> args)
	{
		if (args.Item.IsGroupHeader && (Form?.SmallMenu ?? false))
		{
			args.DoNotDraw = true; // Don't draw group headers if SmallMenu is enabled
		}
		else if (args.Item.PanelItem?.Hidden == true)
		{
			args.DoNotDraw = true; // Don't draw if the PanelItem is hidden
		}
		else if ((args.Item.IsGroupHeader || args.Item.IsSeparator) && args.Item.GroupItems.Length > 0 && args.Item.GroupItems.All(x => x.Hidden))
		{
			args.DoNotDraw = true; // Don't draw group headers with no visible sub-items
		}
		else if (args.Item.IsSubItem && !(args.Item.ParentItem.Selected || args.Item.ParentItem.SubItems.Any(x => x.Selected)))
		{
			args.DoNotDraw = true; // Don't draw sub-items unless the parent or any sub-item is selected
		}

		base.CanDrawItemInternal(args);
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<PanelTab, GenericDrawableItemRectangles<PanelTab>> e)
	{
		e.BackColor = BackColor;

		e.Item.Paint(e, this, Form?.SmallMenu ?? false);

		if (e.DrawableItem.CachedHeight == 0 || AnimationHandler.IsAnimated(Parent?.Parent))
		{
			e.DrawableItem.CachedHeight = ItemHeight;
		}
	}

	protected override void OnItemMouseClick(DrawableItem<PanelTab, GenericDrawableItemRectangles<PanelTab>> item, MouseEventArgs e)
	{
		item.Item.PanelItem?.MouseClick(e);
	}

	protected override bool IsItemActionHovered(DrawableItem<PanelTab, GenericDrawableItemRectangles<PanelTab>> item, Point location)
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