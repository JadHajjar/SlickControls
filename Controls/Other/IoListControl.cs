using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls.Controls.Other;

internal class IoListControl : SlickStackedListControl<IOControl, IoListControl.Rectangles>
{
	public IOControl SelectedItem { get; internal set; }
	internal IO.IController Controller { get; set; }
	internal IoSortingOption IoSortingOption { get; set; }
	internal bool SortDesc { get; set; }

	public IoListControl()
	{
		ItemHeight = 18;
		GridItemSize = new Size(75, 75);
		GridView = true;
	}

	protected override bool IsItemActionHovered(DrawableItem<IOControl, Rectangles> item, Point location)
	{
		if (item.HoverState.HasFlag(HoverState.Hovered))
		{
			SlickTip.SetTo(this, item.Item.Name, offset: new Point(0, Height + Parent.Parent.Padding.Bottom), alignToBottom: true);
		}

		return true;
	}

	protected override IEnumerable<DrawableItem<IOControl, Rectangles>> OrderItems(IEnumerable<DrawableItem<IOControl, Rectangles>> items)
	{
		switch (IoSortingOption)
		{
			case IoSortingOption.Name:
				items = items.OrderBy(x => SortDesc != (x.Item.FileObject != null)).ThenBy(x => x.Item.Name);
				break;
			case IoSortingOption.DateModified:
				items = items.OrderBy(x => SortDesc != (x.Item.FileObject != null)).ThenBy(x => x.Item.FileObject?.LastWriteTime ?? DateTime.MinValue);
				break;
			case IoSortingOption.DateCreated:
				items = items.OrderBy(x => SortDesc != (x.Item.FileObject != null)).ThenBy(x => x.Item.FileObject?.CreationTime ?? DateTime.MinValue);
				break;
			case IoSortingOption.Size:
				items = items.OrderBy(x => SortDesc != (x.Item.FileObject != null)).ThenBy(x => x.Item.FileObject?.Length ?? 0);
				break;
		}

		return SortDesc ? items.Reverse() : items;
	}

	protected override void OnItemMouseClick(DrawableItem<IOControl, Rectangles> item, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			if (e.Clicks == 1)
			{
				SelectedItem = item.Item;
				Invalidate();
			}
			else if (e.Clicks == 2)
			{
				if (item.Item.FileObject != null)
				{
					Controller.fileOpened(item.Item.FileObject);
				}
				else if (item.Item.FolderObject != null)
				{
					Controller.folderOpened(item.Item.FolderObject);
				}
			}

			Focus();
		}
		else if (e.Button == MouseButtons.Right)
		{
			SlickToolStrip.Show(FindForm() as SlickForm, (Controller.RightClickContext?.Invoke(item.Item) ?? new[]
			{
				new SlickStripItem(
					item.Item.FileObject != null ? "Select File" : "Open Folder",
					item.Item.FileObject != null ? "Ok" : "Folder",
					action: () =>
					{
						if (item.Item.FileObject != null) { Controller.fileOpened(item.Item.FileObject); } else if (item.Item.FolderObject != null) { Controller.folderOpened(item.Item.FolderObject); } })
			})
			.Concat(new[]
			{
				SlickStripItem.Empty,

				item.Item.FileObject == null ? null : new SlickStripItem("Open File", "Search", () =>
				{
					new BackgroundAction(() => System.Diagnostics.Process.Start(item.Item.FileObject.FullName)).Run();
				}),

				new SlickStripItem("View in Explorer", "Folder", () =>
				{
					if (item.Item.FileObject != null) { new BackgroundAction(() => System.Diagnostics.Process.Start("explorer.exe", $"/select, \"{item.Item.FileObject.FullName}\"")).Run(); } else { new BackgroundAction(() => System.Diagnostics.Process.Start(item.Item.FolderObject.FullName)).Run(); } }),

				new SlickStripItem("Delete", "Trash", () =>
				{
					if (MessagePrompt.Show($"Are you sure you want to delete '{Text}'", "Confirm Action", PromptButtons.OKCancel, PromptIcons.Warning, FindForm() as SlickForm) == DialogResult.OK)
					{
						new BackgroundAction(() => FileOperationAPIWrapper.MoveToRecycleBin(item.Item.FileObject?.FullName ?? item.Item.FolderObject.FullName)).Run();
						Dispose();
					}
				})
			}).ToArray());
		}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<IOControl, Rectangles> e)
	{
		e.Item.OnPaintList(e, SelectedItem == e.Item);
	}

	protected override void OnPaintItemGrid(ItemPaintEventArgs<IOControl, Rectangles> e)
	{
		e.Item.OnPaintGrid(e, SelectedItem == e.Item);
	}

	internal class Rectangles : IDrawableItemRectangles<IOControl>
	{
		public IOControl Item { get; set; }

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
