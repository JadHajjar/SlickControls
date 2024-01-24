using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SlickStackedListControl<T, R> : SlickControl where R : IDrawableItemRectangles<T>
{
	private readonly object _sync = new();
	private readonly List<DrawableItem<T, R>> _items;
	private int visibleItems;
	protected bool scrollVisible;
	private int scrollIndex;
	protected Rectangle scrollThumbRectangle;
	protected int scrollMouseDown = -1;
	protected DrawableItem<T, R> mouseDownItem;
	protected int baseHeight;
	private List<DrawableItem<T, R>> _sortedItems;
	private Size baseSize;
	private bool scrollHovered;
	private Size lastSize;
	private bool _gridView;

	[Category("Data"), Browsable(false)]
	public IEnumerable<T> Items
	{
		get
		{
			lock (_sync)
			{
				foreach (var item in _items)
				{
					yield return item.Item;
				}
			}
		}
	}

	[Category("Data"), Browsable(false)]
	public IEnumerable<T> FilteredItems
	{
		get
		{
			lock (_sync)
			{
				foreach (var item in _items)
				{
					if (!item.Hidden)
					{
						yield return item.Item;
					}
				}
			}
		}
	}

	[Category("Data"), Browsable(false)]
	public IEnumerable<T> SelectedOrFilteredItems
	{
		get
		{
			lock (_sync)
			{
				if (SelectedItems.Count > 0)
				{
					foreach (var item in SelectedItems)
					{
						if (!item.Hidden)
						{
							yield return item.Item;
						}
					}

					yield break;
				}

				foreach (var item in _items)
				{
					if (!item.Hidden)
					{
						yield return item.Item;
					}
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
	public int ItemCount
	{
		get
		{
			lock (_sync)
			{
				return _items.Count;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
	public int FilteredCount
	{
		get
		{
			var count = 0;

			lock (_sync)
			{
				foreach (var item in _items)
				{
					if (!item.Hidden)
					{
						count++;
					}
				}
			}

			return count;
		}
	}

	[Category("Appearance"), DefaultValue(false)]
	public bool GridView
	{
		get => _gridView; set
		{
			_gridView = value;
			OnViewChanged();
			Invalidate();
		}
	}

	[Category("Appearance"), DefaultValue(typeof(Size), "0, 0")]
	public Size GridItemSize { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool SeparateWithLines { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool DynamicSizing { get; set; }

	[Category("Appearance"), DefaultValue(false)]
	public bool HighlightOnHover { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool HorizontalScrolling { get; set; }

	[Category("Behavior"), DefaultValue(false)]
	public bool EnableSelection { get; set; }

	[Category("Appearance"), DefaultValue(22)]
	public int ItemHeight { get; set; }

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedItemsCount => SelectedItems.Count;
#if NET47
	[Category("Behavior"), DisplayName("Can Draw Item")]
	public event EventHandler<CanDrawItemEventArgs<T>> CanDrawItem;

	[Category("Appearance"), DisplayName("Paint Item List")]
	public event EventHandler<ItemPaintEventArgs<T, R>> PaintItemList;

	[Category("Appearance"), DisplayName("Paint Item Grid")]
	public event EventHandler<ItemPaintEventArgs<T, R>> PaintItemGrid;

	[Category("Behavior"), DisplayName("Item Mouse Click")]
	public event EventHandler<MouseEventArgs> ItemMouseClick;
#else
	[Category("Behavior"), DisplayName("Can Draw Item")]
	public event Extensions.EventHandler<CanDrawItemEventArgs<T>> CanDrawItem;

	[Category("Appearance"), DisplayName("Paint Item List")]
	public event Extensions.EventHandler<ItemPaintEventArgs<T, R>> PaintItemList;

	[Category("Appearance"), DisplayName("Paint Item Grid")]
	public event Extensions.EventHandler<ItemPaintEventArgs<T, R>> PaintItemGrid;

	[Category("Behavior"), DisplayName("Item Mouse Click")]
	public event Extensions.EventHandler<MouseEventArgs> ItemMouseClick;
#endif
	[Category("Behavior"), DisplayName("Selected Items Changed")]
	public event EventHandler SelectedItemsChanged;

	protected Point CursorLocation { get; set; }
	protected int StartHeight { get; set; }
	protected Padding GridPadding { get; set; }
	protected bool SelectionMode { get; private set; }
	protected List<DrawableItem<T, R>> SelectedItems { get; } = [];

	public SlickStackedListControl()
	{
		_items = [];
		ItemHeight = 22;
		AutoInvalidate = false;
		ResizeRedraw = false;
		AutoScroll = true;
	}

	public virtual void SortingChanged(bool resetScroll = true, bool invalidate = true)
	{
		lock (_sync)
		{
			_sortedItems = new List<DrawableItem<T, R>>(OrderItems(_items));
		}

		if (resetScroll)
		{
			ResetScroll(invalidate);
		}
		else if (invalidate)
		{
			Invalidate();
		}
	}

	public virtual void FilterChanged()
	{
		List<DrawableItem<T, R>> itemCopy;

		lock (_sync)
		{
			itemCopy = new List<DrawableItem<T, R>>(_items);
		}

		Parallelism.ForEach(itemCopy, x =>
		{
			var canDraw = new CanDrawItemEventArgs<T>(x.Item);

			CanDrawItemInternal(canDraw);

			x.Bounds = Rectangle.Empty;
			x.Hidden = canDraw.DoNotDraw;
		});

		SelectedItems.RemoveAll(x => x.Hidden);

		this.TryInvoke(() => SelectedItemsChanged?.Invoke(this, EventArgs.Empty));

		Invalidate();
	}

	protected virtual void CanDrawItemInternal(CanDrawItemEventArgs<T> args)
	{
		CanDrawItem?.Invoke(this, args);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);

		if (lastSize.Width != Width)
		{
			Invalidate();
		}

		lastSize = Size;
	}

	public virtual void Invalidate(T item)
	{
		lock (_sync)
		{
			var selectedItem = _items.FirstOrDefault(x => x.Item.Equals(item));

			if (selectedItem != null)
			{
				var rect = selectedItem.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom);

				if (rect.IntersectsWith(ClientRectangle))
				{
					Invalidate(rect);
				}
			}
		}
	}

	public virtual void Add(T item)
	{
		lock (_sync)
		{
			_items.Add(new DrawableItem<T, R>(item));
		}

		SortingChanged(false, false);
		FilterChanged();
	}

	public virtual void AddRange(IEnumerable<T> items)
	{
		lock (_sync)
		{
			_items.AddRange(items.Select(item => new DrawableItem<T, R>(item)));
		}

		SortingChanged(false, false);
		FilterChanged();
	}

	public virtual void SetItems(IEnumerable<T> items)
	{
		lock (_sync)
		{
			_items.Clear();
			_items.AddRange(items.Select(item => new DrawableItem<T, R>(item)));
		}

		SortingChanged(false, false);
		FilterChanged();
	}

	public virtual void Remove(T item)
	{
		RemoveAll(x => x.Equals(item));
	}

	public virtual void RemoveAll(Predicate<T> predicate)
	{
		lock (_sync)
		{
			_items.RemoveAll(item => predicate(item.Item));
		}

		SortingChanged(false, false);
		FilterChanged();
	}

	public virtual void Clear()
	{
		lock (_sync)
		{
			_items.Clear();
		}

		SortingChanged(false);
	}

	public void ResetScroll(bool invalidate = true)
	{
		scrollIndex = 0;

		if (invalidate)
		{
			Invalidate();
		}
	}

	public void SelectAll()
	{
		SelectedItems.Clear();
		SelectedItems.AddRange(SafeGetItems());
		Invalidate();
		Focus();
		SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
	}

	public void DeselectAll()
	{
		SelectedItems.Clear();
		Invalidate();
		Focus();
		SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
	}

	protected override void UIChanged()
	{
		if (Live)
		{
			if (baseHeight == 0)
			{
				baseHeight = ItemHeight;
			}

			if (baseSize == Size.Empty)
			{
				baseSize = GridItemSize;
			}

			GridItemSize = UI.Scale(baseSize, UI.FontScale);
			ItemHeight = (int)(baseHeight * UI.FontScale);

			Invalidate();
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (scrollMouseDown >= 0)
		{
			return;
		}

		if (mouseDownItem != null)
		{
			if (!mouseDownItem.Hidden && mouseDownItem.Bounds.Contains(e.Location))
			{
				TriggerItemMouseClick(mouseDownItem, e);
			}
		}

		base.OnMouseClick(e);
	}

	protected override void OnMouseDoubleClick(MouseEventArgs e)
	{
		if (scrollMouseDown >= 0)
		{
			return;
		}

		if (mouseDownItem != null)
		{
			if (!mouseDownItem.Hidden && mouseDownItem.Bounds.Contains(e.Location))
			{
				TriggerItemMouseClick(mouseDownItem, e);
			}
		}

		base.OnMouseDoubleClick(e);
	}

	private void TriggerItemMouseClick(DrawableItem<T, R> item, MouseEventArgs e)
	{
		if (EnableSelection && e.Button == MouseButtons.Left)
		{
			if (ModifierKeys.HasFlag(Keys.Shift) && SelectedItems.Count > 0)
			{
				var items = SafeGetItems();
				var min = SelectedItems.Min(items.IndexOf);
				var max = SelectedItems.Max(items.IndexOf);
				var index = items.IndexOf(item);

				if (min > index)
				{
					for (var i = index; i < min; i++)
					{
						SelectedItems.Add(items[i]);
						Invalidate(items[i]);
					}
				}

				else if (max < index)
				{
					for (var i = max + 1; i <= index; i++)
					{
						SelectedItems.Add(items[i]);
						Invalidate(items[i]);
					}
				}

				else if (max != index)
				{
					for (var i = index; i <= max; i++)
					{
						SelectedItems.Remove(items[i]);
						Invalidate(items[i]);
					}
				}

				Focus();
				SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
				return;
			}

			if (ModifierKeys.HasFlag(Keys.Control) || ModifierKeys.HasFlag(Keys.Shift))
			{
				if (SelectedItems.Contains(item))
				{
					SelectedItems.Remove(item);
				}
				else
				{
					SelectedItems.Add(item);
				}

				Invalidate(item);
				Focus();
				SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
				return;
			}
		}

		OnItemMouseClick(item, e);
	}

	protected virtual void OnItemMouseClick(DrawableItem<T, R> item, MouseEventArgs e)
	{
		ItemMouseClick?.Invoke(item.Item, e);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape && SelectedItems.Any())
		{
			SelectedItems.Clear();
			Invalidate();
			SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
			return true;
		}

		if (keyData == (Keys.Control | Keys.A))
		{
			SelectedItems.Clear();
			SelectedItems.AddRange(SafeGetItems());
			Invalidate();
			SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		var itemActionHovered = false;
		var isToolTipSet = false;

		lock (_sync)
		{
			foreach (var item in _items)
			{
				if (!item.Hidden && item.Bounds.Contains(e.Location))
				{
					item.HoverState |= HoverState.Hovered;
					itemActionHovered |= (mouseDownItem == null || mouseDownItem == item) && IsItemActionHovered(item, e.Location);

					if (!isToolTipSet && item.Rectangles != null && item.Rectangles.GetToolTip(this, e.Location, out var text, out var point))
					{
						isToolTipSet = true;
						SlickTip.SetTo(this, text, offset: new Point(point.X, point.Y));
					}

					Invalidate(item);
				}
				else if (item.HoverState.HasFlag(HoverState.Hovered))
				{
					item.HoverState &= ~HoverState.Hovered;
					Invalidate(item);
				}
			}
		}

		if (scrollMouseDown >= 0)
		{
			var itemList = SafeGetItems();

			scrollIndex = (GetNumRows(itemList) - visibleItems) * (e.Location.Y - scrollMouseDown) / (Height - scrollThumbRectangle.Height - StartHeight).If(0, 1);
			Invalidate();
		}

		if (scrollVisible && (scrollHovered || scrollThumbRectangle.Contains(e.Location)))
		{
			scrollHovered = scrollThumbRectangle.Contains(e.Location);
			Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
		}

		Cursor = itemActionHovered || scrollMouseDown >= 0 || scrollThumbRectangle.Contains(e.Location) || IsHeaderActionHovered(e.Location) ? Cursors.Hand : Cursors.Default;

		if (!isToolTipSet)
		{
			SlickTip.SetTo(this, string.Empty);
		}

		if (AutoInvalidate)
		{
			Invalidate();
		}
	}

	protected virtual bool IsHeaderActionHovered(Point location)
	{
		return false;
	}

	private void Invalidate(DrawableItem<T, R> item)
	{
		Invalidate(item.Bounds);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		HoverState |= HoverState.Hovered;
		var mouse = PointToClient(Cursor.Position);

		lock (_sync)
		{
			foreach (var item in _items)
			{
				if (!item.Hidden && item.Bounds.Contains(mouse))
				{
					item.HoverState |= HoverState.Hovered;
					Invalidate(item);
				}
				else if (item.HoverState.HasFlag(HoverState.Hovered))
				{
					item.HoverState &= ~HoverState.Hovered;
					Invalidate(item);
				}
			}
		}

		if (scrollVisible)
		{
			Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);

		HoverState &= ~HoverState.Hovered;

		lock (_sync)
		{
			foreach (var item in _items)
			{
				if (item.HoverState.HasFlag(HoverState.Hovered))
				{
					item.HoverState &= ~HoverState.Hovered;
					Invalidate(item);
				}
			}
		}

		if (scrollVisible)
		{
			Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		HoverState |= HoverState.Pressed;

		lock (_sync)
		{
			foreach (var item in _items)
			{
				if (!item.Hidden && item.Bounds.Contains(e.Location))
				{
					mouseDownItem = item;
					item.HoverState |= HoverState.Pressed;
					Invalidate(item);
				}
			}
		}

		if (scrollVisible && e.Location.X >= scrollThumbRectangle.X)
		{
			if (scrollThumbRectangle.Contains(e.Location))
			{
				scrollMouseDown = e.Location.Y - scrollThumbRectangle.Y + StartHeight;
			}
			else
			{
				if (e.Location.Y < scrollThumbRectangle.Y)
				{
					scrollIndex -= visibleItems;
				}
				else
				{
					scrollIndex += visibleItems;
				}

				scrollMouseDown = (scrollThumbRectangle.Height / 2) + StartHeight;
			}

			Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
		}
		else
		{
			scrollMouseDown = -1;
		}

		if (AutoInvalidate)
		{
			Invalidate();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		HoverState &= ~HoverState.Pressed;

		if (mouseDownItem != null)
		{
			mouseDownItem.HoverState &= ~HoverState.Pressed;
			Invalidate(mouseDownItem);
			mouseDownItem = null;
		}

		if (scrollMouseDown >= 0)
		{
			scrollMouseDown = -1;
			Invalidate();
		}

		if (AutoInvalidate)
		{
			Invalidate();
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		base.OnMouseWheel(e);

		if (HorizontalScrolling || !scrollVisible)
		{
			return;
		}

		scrollIndex -= e.Delta.Sign() * (int)Math.Ceiling(Math.Abs(e.Delta) / (double)(GridView ? GridItemSize.Height : ItemHeight));
		Invalidate();

		SlickTip.SetTo(this, string.Empty);
	}

	protected virtual bool IsItemActionHovered(DrawableItem<T, R> item, Point location)
	{
		return item.Rectangles?.IsHovered(this, location) ?? false;
	}

	protected virtual R GenerateRectangles(T item, Rectangle rectangle)
	{
		return default;
	}

	protected virtual IEnumerable<DrawableItem<T, R>> OrderItems(IEnumerable<DrawableItem<T, R>> items)
	{
		return items;
	}

	protected virtual void OnViewChanged()
	{
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		base.OnPaintBackground(e);

		CursorLocation = PointToClient(Cursor.Position);

		e.Graphics.Clear(BackColor);
	}

	private void OnPaintItem(ItemPaintEventArgs<T, R> e)
	{
		e.DrawableItem.Rectangles = GenerateRectangles(e.Item, e.ClipRectangle);

		if (GridView)
		{
			OnPaintItemGrid(e);
		}
		else
		{
			OnPaintItemList(e);
		}
	}

	protected virtual void OnPaintItemList(ItemPaintEventArgs<T, R> e)
	{
		if (e.BackColor == Color.Empty)
		{
			e.BackColor = BackColor;
		}

		if (HighlightOnHover && e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.BackColor = e.BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);
		}

		if (e.BackColor != BackColor)
		{
			var rect = e.ClipRectangle;
			var filledRect = rect.Pad(0, -Padding.Top, 0, -Padding.Bottom);

			using var brush = new SolidBrush(e.BackColor);
			e.Graphics.FillRectangle(brush, filledRect);
		}

		PaintItemList?.Invoke(this, e);
	}

	protected virtual void OnPaintItemGrid(ItemPaintEventArgs<T, R> e)
	{
		if (e.BackColor == Color.Empty)
		{
			e.BackColor = BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 4 : -5);
		}

		if (HighlightOnHover && e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.BackColor = e.BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);
		}

		using (var brush = new SolidBrush(e.BackColor))
		{
			e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.Pad(-GridPadding.Left, -GridPadding.Top, -GridPadding.Right, -GridPadding.Bottom), (int)(5 * UI.FontScale));
		}

		PaintItemGrid?.Invoke(this, e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		var invalidRect = e.ClipRectangle;
		var invalidRects = allInvalidated || _invalidatedRectangles.Count == 0 || invalidRect == ClientRectangle ? [invalidRect] : _invalidatedRectangles.ToArray();

		allInvalidated = false;
		_invalidatedRectangles.Clear();

		e.Graphics.SetUp();

		lock (_sync)
		{
			if (Loading && _items.Count == 0)
			{
				DrawLoader(e.Graphics, ClientRectangle.CenterR(UI.Scale(new Size(32, 32), UI.FontScale)));
				return;
			}
		}

		var loc = new Point(0, StartHeight);
		var itemList = SafeGetItems();

		itemList.ForEach(x => x.Bounds = Rectangle.Empty);

		HandleScrolling(itemList);

		if (scrollVisible)
		{
			var isMouseDown = HoverState.HasFlag(HoverState.Pressed) && (scrollThumbRectangle.Contains(CursorLocation) || scrollMouseDown >= 0);

			if (isMouseDown || (HoverState.HasFlag(HoverState.Hovered) && CursorLocation.X >= scrollThumbRectangle.X))
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(7, FormDesign.Design.IsDarkTheme ? Color.White : Color.Black)), new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
			}

			e.Graphics.FillRoundedRectangle(scrollThumbRectangle.Gradient(isMouseDown ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), scrollThumbRectangle.Pad(2, 0, 2, 0), 3);
		}

		if (StartHeight > 0)
		{
			e.Graphics.SetClip(new Rectangle(0, 0, Width, StartHeight));

			DrawHeader(e);
		}

		var start = scrollIndex;
		var maxHeight = 0;

		if (GridView)
		{
			start *= (int)Math.Floor((double)Width / GridItemSize.Width);
		}

		for (var i = start; i < itemList.Count; i++)
		{
			var item = itemList[i];
			var height = DynamicSizing && item.CachedHeight != 0 ? item.CachedHeight : GridView ? GridItemSize.Height : ItemHeight;
			maxHeight = Math.Max(maxHeight, height);

			if (GridView)
			{
				item.Bounds = new Rectangle(loc, new Size(GridItemSize.Width, height)).Pad(Padding);
			}
			else
			{
				item.Bounds = new Rectangle(loc, new Size(Width - (scrollVisible ? scrollThumbRectangle.Width + 1 : 0), height + Padding.Vertical + (SeparateWithLines ? (int)UI.FontScale : 0)));
			}

			if (invalidRect.IntersectsWith(item.Bounds))
			{
				e.Graphics.SetClip(item.Bounds);

				var currentHeight = item.CachedHeight;

				OnPaintItem(new ItemPaintEventArgs<T, R>(
					item,
					e.Graphics,
					invalidRects.Where(x => x.IntersectsWith(item.Bounds)),
					GridView ? item.Bounds.Pad(GridPadding) : item.Bounds.Pad(0, Padding.Top, 0, Padding.Bottom),
					mouseDownItem == item ? (HoverState.Pressed | HoverState.Hovered) : mouseDownItem == null ? item.HoverState : HoverState.Normal,
					SelectedItems.Contains(item)));

				if (DynamicSizing && currentHeight != item.CachedHeight)
				{
					Invalidate();
				}

				e.Graphics.ResetClip();
			}

			if (GridView)
			{
				loc.X += item.Bounds.Width + Padding.Horizontal;

				if (loc.X + item.Bounds.Width + Padding.Horizontal > Width - (scrollVisible ? scrollThumbRectangle.Width + 1 : 0) || IsFlowBreak(i, item, i == itemList.Count - 1 ? default : itemList[i + 1]))
				{
					loc.X = 0;
					loc.Y += maxHeight;
					maxHeight = 0;
				}
			}
			else
			{
				loc.Y += item.Bounds.Height;
			}

			if (SeparateWithLines && !GridView)
			{
				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor, (int)UI.FontScale), Padding.Left, loc.Y, Width - Padding.Right - (int)(scrollVisible ? 6 * UI.FontScale : 0), loc.Y);
			}

			if (loc.Y > Height)
			{
				break;
			}
		}

		base.OnPaint(e);
	}

	protected virtual void DrawHeader(PaintEventArgs e)
	{
	}

	protected virtual bool IsFlowBreak(int index, DrawableItem<T, R> currentItem, DrawableItem<T, R> nextItem)
	{
		return false;
	}

	private void HandleScrolling(List<DrawableItem<T, R>> itemList)
	{
		var totalHeight = GetTotalHeight(itemList);
		var validHeight = Height - StartHeight;

		if (scrollVisible != (totalHeight > validHeight))
		{
			Invalidate();
		}

		if (totalHeight > validHeight)
		{
			var rowCount = GetNumRows(itemList);
			visibleItems = (int)Math.Floor((float)validHeight / (GridView ? GridItemSize.Height : (ItemHeight + Padding.Vertical + (SeparateWithLines ? (int)UI.FontScale : 0))));
			scrollIndex = Math.Max(0, Math.Min(scrollIndex, rowCount - visibleItems));

			var thumbHeight = Math.Max(validHeight * visibleItems / rowCount, validHeight / 24);

			scrollThumbRectangle = new Rectangle(Width - (int)(10 * UI.FontScale), StartHeight + ((validHeight - thumbHeight) * scrollIndex / (rowCount - visibleItems).If(0, 1)), (int)(10 * UI.FontScale), thumbHeight);
			scrollVisible = true;
		}
		else
		{
			scrollIndex = 0;
			scrollVisible = false;
		}
	}

	public int GetTotalHeight(List<DrawableItem<T, R>> itemList)
	{
		if (DynamicSizing)
		{
			if (GridView)
			{
				var height = 0;
				var columns = Math.Floor((double)(Width / GridItemSize.Width.If(0, 1)));

				for (var i = 0; i < itemList.Count;)
				{
					var maxHeight = 0;

					for (var j = 0; j < columns && i < itemList.Count; j++, i++)
					{
						maxHeight = Math.Max(maxHeight, itemList[i].CachedHeight.If(0, GridItemSize.Height));
					}

					height += maxHeight;
				}

				return height;
			}

			return itemList.Sum(x => x.CachedHeight.If(0, ItemHeight));
		}

		if (GridView)
		{
			var numRows = GetNumRows(itemList);

			return numRows * GridItemSize.Height;
		}

		var totalHeight = itemList.Count * (ItemHeight + Padding.Vertical);

		if (SeparateWithLines)
		{
			totalHeight += (itemList.Count - 1) * (int)UI.FontScale;
		}

		return totalHeight;
	}

	private int GetNumRows<i>(IEnumerable<i> itemList)
	{
		if (!GridView)
		{
			return itemList.Count();
		}

		return (int)Math.Ceiling(itemList.Count() / Math.Floor((double)(Width / GridItemSize.Width)));
	}

	public List<DrawableItem<T, R>> SafeGetItems()
	{
		lock (_sync)
		{
			return _sortedItems?.Where(x => !x.Hidden).ToList() ?? [];
		}
	}

	public void ScrollTo(T item)
	{
		var items = SafeGetItems();

		var scrollTo = items.FirstOrDefault(x => x.Item.Equals(item));

		if (scrollTo != null)
		{
			scrollIndex = items.IndexOf(scrollTo);
			Invalidate();
		}
	}

	public new void Invalidate()
	{
		if (!allInvalidated)
		{
			allInvalidated = true;

			base.Invalidate();
		}
	}

	private bool allInvalidated;
	private List<Rectangle> _invalidatedRectangles = new List<Rectangle>();

	public new void Invalidate(Rectangle rectangle)
	{
		if (!allInvalidated)
		{
			_invalidatedRectangles.Add(rectangle);
			base.Invalidate(rectangle);
		}
	}

	protected override void InvalidateForLoading()
	{
		List<DrawableItem<T, R>> items;

		lock (_sync)
		{
			items = _items.Where(x => x.Loading).ToList();
		}

		if (items.Count == 0)
		{
			Invalidate();
			return;
		}

		foreach (var item in items)
		{
			Invalidate(item);
		}
	}
}