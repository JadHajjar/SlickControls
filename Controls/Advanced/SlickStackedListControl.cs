using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SlickStackedListControl<T, TRectangle> : SlickControl where TRectangle : IDrawableItemRectangles<T>
{
	private readonly object _sync = new();
	private readonly List<DrawableItem<T, TRectangle>> _items;
	private double scrollIndex;
	private List<DrawableItem<T, TRectangle>> _sortedItems;
	private Size baseSize;
	private bool scrollHovered;
	private Size lastSize;
	private bool _gridView;
	protected bool scrollVisible;
	protected Rectangle scrollThumbRectangle;
	protected int scrollMouseDown = -1;
	protected DrawableItem<T, TRectangle> mouseDownItem;
	protected int baseHeight;

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

	[Category("Data"), Browsable(false)]
	public IEnumerable<T> SortedItems
	{
		get
		{
			lock (_sync)
			{
				foreach (var item in _sortedItems)
				{
					yield return item.Item;
				}
			}
		}
	}

	[Category("Data"), Browsable(false)]
	public IEnumerable<T> SortedAndFilteredItems
	{
		get
		{
			lock (_sync)
			{
				foreach (var item in _sortedItems)
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
		get => _gridView;
		set
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
	public event EventHandler<ItemPaintEventArgs<T, TRectangle>> PaintItemList;

	[Category("Appearance"), DisplayName("Paint Item Grid")]
	public event EventHandler<ItemPaintEventArgs<T, TRectangle>> PaintItemGrid;

	[Category("Behavior"), DisplayName("Item Mouse Click")]
	public event EventHandler<MouseEventArgs> ItemMouseClick;
#else
	[Category("Behavior"), DisplayName("Can Draw Item")]
	public event Extensions.EventHandler<CanDrawItemEventArgs<T>> CanDrawItem;

	[Category("Appearance"), DisplayName("Paint Item List")]
	public event Extensions.EventHandler<ItemPaintEventArgs<T, TRectangle>> PaintItemList;

	[Category("Appearance"), DisplayName("Paint Item Grid")]
	public event Extensions.EventHandler<ItemPaintEventArgs<T, TRectangle>> PaintItemGrid;

	[Category("Behavior"), DisplayName("Item Mouse Click")]
	public event Extensions.EventHandler<MouseEventArgs> ItemMouseClick;
#endif
	[Category("Behavior"), DisplayName("Selected Items Changed")]
	public event EventHandler SelectedItemsChanged;

	[Category("Behavior"), DisplayName("Scroll Update")]
	public event Action<object, double, double> ScrollUpdate;

	protected Point CursorLocation { get; set; }
	protected int StartHeight { get; set; }
	protected Padding GridPadding { get; set; }
	protected bool SelectionMode { get; private set; }
	protected List<DrawableItem<T, TRectangle>> SelectedItems { get; } = [];

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
			_sortedItems = new List<DrawableItem<T, TRectangle>>(OrderItems(_items));
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
		List<DrawableItem<T, TRectangle>> itemCopy;

		lock (_sync)
		{
			itemCopy = new List<DrawableItem<T, TRectangle>>(_items);
		}

		Parallelism.ForEach(itemCopy, CanDrawItemWrapper);

		this.TryInvoke(() => SelectedItemsChanged?.Invoke(this, EventArgs.Empty));

		Invalidate();
	}

	private void CanDrawItemWrapper(DrawableItem<T, TRectangle> drawableItem)
	{
		CanDrawItemInternal(new CanDrawItemEventArgs<T, TRectangle>(drawableItem));
	}

	protected virtual void CanDrawItemInternal(CanDrawItemEventArgs<T, TRectangle> args)
	{
		OnCanDrawItem(args);

		args.DrawableItem.Bounds = Rectangle.Empty;
		args.DrawableItem.Hidden = args.DoNotDraw;

		if (args.DoNotDraw)
		{
			SelectedItems.Remove(args.DrawableItem);
		}
	}

	protected void OnCanDrawItem(CanDrawItemEventArgs<T, TRectangle> args)
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
			_items.Add(new DrawableItem<T, TRectangle>(item));
		}

		SortingChanged(false, false);
		FilterChanged();
	}

	public virtual void AddRange(IEnumerable<T> items)
	{
		lock (_sync)
		{
			_items.AddRange(items.Select(item => new DrawableItem<T, TRectangle>(item)));
		}

		SortingChanged(false, false);
		FilterChanged();
	}

	public virtual void SetItems(IEnumerable<T> items)
	{
		lock (_sync)
		{
			_items.Clear();
			_items.AddRange(items.Select(item => new DrawableItem<T, TRectangle>(item)));
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

			GridItemSize = UI.Scale(baseSize);
			ItemHeight = (int)(baseHeight * UI.FontScale) / 2 * 2;

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

	private void TriggerItemMouseClick(DrawableItem<T, TRectangle> item, MouseEventArgs e)
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

	protected virtual void OnItemMouseClick(DrawableItem<T, TRectangle> item, MouseEventArgs e)
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

			var maxIndex = GetMaxScrollIndex(itemList);
			var visibleItems = GetDisplayedRows();
			scrollIndex = maxIndex * (e.Location.Y - scrollMouseDown) / (Height - scrollThumbRectangle.Height - StartHeight).If(0, 1);
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

	private void Invalidate(DrawableItem<T, TRectangle> item)
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
				var visibleItems = GetDisplayedRows();
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

		scrollIndex -= (DynamicSizing ? e.Delta : (e.Delta / (double)(GridView ? GridItemSize.Height : ItemHeight))) * UI.UIScale;
		Invalidate();

		SlickTip.SetTo(this, string.Empty);
	}

	protected virtual bool IsItemActionHovered(DrawableItem<T, TRectangle> item, Point location)
	{
		return item.Rectangles?.IsHovered(this, location) ?? false;
	}

	protected virtual TRectangle GenerateRectangles(T item, Rectangle rectangle)
	{
		return default;
	}

	protected virtual IEnumerable<DrawableItem<T, TRectangle>> OrderItems(IEnumerable<DrawableItem<T, TRectangle>> items)
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

	protected virtual void OnPaintItem(ItemPaintEventArgs<T, TRectangle> e)
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

	protected virtual void OnPaintItemList(ItemPaintEventArgs<T, TRectangle> e)
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

	protected virtual void OnPaintItemGrid(ItemPaintEventArgs<T, TRectangle> e)
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
			e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.InvertPad(GridPadding), UI.Scale(5));
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

		var itemList = new List<DrawableItem<T, TRectangle>>();

		lock (_sync)
		{
			if (Loading && _items.Count == 0)
			{
				DrawLoader(e.Graphics, ClientRectangle.CenterR(UI.Scale(new Size(32, 32))));
				return;
			}

			if (_sortedItems is not null)
			{
				for (var i = 0; i < _sortedItems.Count; i++)
				{
					var item = _sortedItems[i];

					item.Bounds = Rectangle.Empty;

					if (!item.Hidden)
					{
						itemList.Add(item);
					}
				}
			}
		}

		HandleScrolling(itemList);

		if (scrollVisible)
		{
			var isMouseDown = HoverState.HasFlag(HoverState.Pressed) && (scrollThumbRectangle.Contains(CursorLocation) || scrollMouseDown >= 0);

			if (isMouseDown || (HoverState.HasFlag(HoverState.Hovered) && CursorLocation.X >= scrollThumbRectangle.X))
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(7, FormDesign.Design.IsDarkTheme ? Color.White : Color.Black)), new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
			}

			using var brush = scrollThumbRectangle.Gradient(isMouseDown ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor);
			e.Graphics.FillRoundedRectangle(brush, scrollThumbRectangle.Pad(2, 0, 2, 0), 3);
		}

		OnPrePaint(e);

		bool shouldInvalidate;

		if (DynamicSizing)
		{
			PaintItemsDynamic(e, invalidRect, invalidRects, itemList, out shouldInvalidate);
		}
		else
		{
			PaintItems(e, invalidRect, invalidRects, itemList, out shouldInvalidate);
		}

		if (StartHeight > 0)
		{
			e.Graphics.SetClip(new Rectangle(0, 0, Width, StartHeight));
			e.Graphics.Clear(BackColor);

			DrawHeader(e);
		}

		base.OnPaint(e);

		if (shouldInvalidate)
		{
			Invalidate();
		}
	}

	protected virtual void OnPrePaint(PaintEventArgs e)
	{
	}

	private void PaintItemsDynamic(PaintEventArgs e, Rectangle invalidRect, Rectangle[] invalidRects, List<DrawableItem<T, TRectangle>> itemList, out bool shouldInvalidate)
	{
		var availableWidth = Width - (scrollVisible ? scrollThumbRectangle.Width + (int)UI.FontScale : 0);
		var bounds = new Rectangle(0, StartHeight, availableWidth, Height - StartHeight);
		var loc = new Point(0, StartHeight - (int)scrollIndex);
		var maxHeight = 0;
		shouldInvalidate = false;

		for (var i = 0; i < itemList.Count; i++)
		{
			var item = itemList[i];
			var height = item.CachedHeight != 0 ? item.CachedHeight : GridView ? GridItemSize.Height : ItemHeight;
			maxHeight = Math.Max(maxHeight, height);

			if (GridView)
			{
				item.Bounds = new Rectangle(loc, new Size(GridItemSize.Width, height)).Pad(Padding);
			}
			else
			{
				item.Bounds = new Rectangle(loc, new Size(availableWidth, height));
			}

			if (invalidRect.IntersectsWith(item.Bounds) || (item.CachedHeight == 0 && i < 100))
			{
				if (!invalidRect.IntersectsWith(item.Bounds))
				{
					item.Bounds = new Rectangle(-99999, -99999, item.Bounds.Width, item.Bounds.Height);
				}

				e.Graphics.SetClip(item.Bounds);

				var currentHeight = item.CachedHeight;

				OnPaintItem(new ItemPaintEventArgs<T, TRectangle>(
					item,
					e.Graphics,
					invalidRects.Where(x => x.IntersectsWith(item.Bounds)),
					GridView ? item.Bounds.Pad(GridPadding) : item.Bounds.Pad(0, Padding.Top, 0, Padding.Bottom),
					mouseDownItem == item ? (HoverState.Pressed | HoverState.Hovered) : mouseDownItem == null ? item.HoverState : HoverState.Normal,
					SelectedItems.Contains(item)));

				shouldInvalidate |= currentHeight != item.CachedHeight;

				e.Graphics.ResetClip();
			}

			if (GridView)
			{
				loc.X += GridItemSize.Width;

				if (loc.X + GridItemSize.Width > availableWidth || IsFlowBreak(i, item, i == itemList.Count - 1 ? default : itemList[i + 1]))
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

			if (SeparateWithLines && !GridView && loc.Y >= 0 && loc.Y <= Height)
			{
				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor, (int)UI.FontScale), Padding.Left, loc.Y, Width - Padding.Right - (int)(scrollVisible ? 6 * UI.FontScale : 0), loc.Y);
			}

			if (loc.Y > Height && !(i + 1 < itemList.Count && itemList[i + 1].CachedHeight == 0))
			{
				break;
			}
		}
	}


	private void PaintItems(PaintEventArgs e, Rectangle invalidRect, Rectangle[] invalidRects, List<DrawableItem<T, TRectangle>> itemList, out bool shouldInvalidate)
	{
		var availableWidth = Width - (scrollVisible ? scrollThumbRectangle.Width + (int)UI.FontScale : 0);
		var start = GetStartingIndex();
		var loc = new Point(0, GetStartingY());
		shouldInvalidate = false;

		for (var i = start; i < itemList.Count; i++)
		{
			var item = itemList[i];
			var height = GridView ? GridItemSize.Height : ItemHeight;

			if (GridView)
			{
				item.Bounds = new Rectangle(loc, new Size(GridItemSize.Width, height)).Pad(Padding);
			}
			else
			{
				item.Bounds = new Rectangle(loc, new Size(availableWidth, height));
			}

			if (invalidRect.IntersectsWith(item.Bounds))
			{
				e.Graphics.SetClip(item.Bounds);

				var currentHeight = item.CachedHeight;

				OnPaintItem(new ItemPaintEventArgs<T, TRectangle>(
					item,
					e.Graphics,
					invalidRects.Where(x => x.IntersectsWith(item.Bounds)),
					GridView ? item.Bounds.Pad(GridPadding) : item.Bounds.Pad(0, Padding.Top, 0, Padding.Bottom),
					mouseDownItem == item ? (HoverState.Pressed | HoverState.Hovered) : mouseDownItem == null ? item.HoverState : HoverState.Normal,
					SelectedItems.Contains(item)));

				e.Graphics.ResetClip();
			}

			if (GridView)
			{
				loc.X += GridItemSize.Width;

				if (loc.X + GridItemSize.Width > availableWidth || IsFlowBreak(i, item, i == itemList.Count - 1 ? default : itemList[i + 1]))
				{
					loc.X = 0;
					loc.Y += height;
					height = 0;
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
	}

	protected virtual void DrawHeader(PaintEventArgs e)
	{
	}

	protected virtual bool IsFlowBreak(int index, DrawableItem<T, TRectangle> currentItem, DrawableItem<T, TRectangle> nextItem)
	{
		return false;
	}

	private void HandleScrolling(List<DrawableItem<T, TRectangle>> itemList)
	{
		var totalHeight = GetTotalHeight(itemList);
		var validHeight = Height - StartHeight;

		if (scrollVisible != (totalHeight > validHeight))
		{
			Invalidate();
		}

		if (totalHeight > validHeight)
		{
			var maxIndex = GetMaxScrollIndex(itemList);
			var visibleItems = GetDisplayedRows();
			scrollIndex = Math.Max(0, Math.Min(scrollIndex, maxIndex));

			var thumbHeight = (int)Math.Max(validHeight * visibleItems / (maxIndex + visibleItems), 32 * UI.FontScale);

			scrollThumbRectangle = new Rectangle(Width - UI.Scale(10), StartHeight + (int)((validHeight - thumbHeight) * scrollIndex / (maxIndex + visibleItems - visibleItems).If(0, 1)), UI.Scale(10), thumbHeight);
			scrollVisible = true;

			if (DynamicSizing)
			{
				ScrollUpdate?.Invoke(this, scrollIndex / (GridView ? (120 * UI.UIScale) : ItemHeight), maxIndex / (GridView ? (120 * UI.UIScale) : ItemHeight));
			}
			else
			{
				ScrollUpdate?.Invoke(this, scrollIndex, maxIndex);
			}
		}
		else
		{
			scrollIndex = 0;
			scrollVisible = false;

			ScrollUpdate?.Invoke(this, 0, 0);
		}
	}

	protected double GetMaxScrollIndex(List<DrawableItem<T, TRectangle>> itemList)
	{
		double availableHeight = Height - StartHeight;

		if (DynamicSizing)
		{
			return GetTotalHeight(itemList) - availableHeight;
		}

		if (!GridView)
		{
			return itemList.Count - (availableHeight / ItemHeight.If(0, 1)) + 0.25;
		}

		double availableWidth = Width - (scrollVisible ? scrollThumbRectangle.Width + (int)UI.FontScale : 0);

		return Math.Ceiling(itemList.Count / Math.Floor(availableWidth / GridItemSize.Width.If(0, 1))) - (availableHeight / GridItemSize.Height.If(0, 1)) + 0.25;
	}

	protected double GetDisplayedRows()
	{
		double availableHeight = Height - StartHeight;

		if (DynamicSizing)
		{
			return availableHeight;
		}

		if (!GridView)
		{
			return availableHeight / ItemHeight.If(0, 1);
		}

		return availableHeight / GridItemSize.Height.If(0, 1);
	}

	protected int GetStartingIndex()
	{
		if (DynamicSizing)
		{
			throw new Exception("Starting index should not be calculated for dynamic sizing");
		}

		if (!GridView)
		{
			return (int)Math.Floor(scrollIndex);
		}

		double availableWidth = Width - (scrollVisible ? scrollThumbRectangle.Width + (int)UI.FontScale : 0);
		var columns = (int)Math.Floor(availableWidth / GridItemSize.Width.If(0, 1));

		return (int)Math.Floor(scrollIndex) * columns;
	}

	protected int GetStartingY()
	{
		if (DynamicSizing)
		{
			throw new Exception("Starting y should not be calculated for dynamic sizing");
		}

		if (!GridView)
		{
			return StartHeight + (int)(scrollIndex % 1 * -ItemHeight);
		}

		return StartHeight + (int)(scrollIndex % 1 * -GridItemSize.Height);
	}

	public int GetTotalHeight(List<DrawableItem<T, TRectangle>> itemList)
	{
		if (DynamicSizing)
		{
			if (GridView)
			{
				var height = 0;
				double availableWidth = Width - (scrollVisible ? scrollThumbRectangle.Width + (int)UI.FontScale : 0);
				var columns = Math.Floor(availableWidth / GridItemSize.Width.If(0, 1)).If(0, 1);

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

			var sum = 0;

			for (var i = 0; i < itemList.Count; i++)
			{
				var height = itemList[i].CachedHeight;

				if (height == 0)
				{
					sum += ItemHeight;
				}
				else
				{
					sum += height;
				}
			}

			return sum;
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

	private int GetNumRows<TGeneric>(List<TGeneric> itemList)
	{
		if (!GridView)
		{
			return itemList.Count;
		}

		return (int)Math.Ceiling(itemList.Count() / Math.Floor((double)(Width / (GridItemSize.Width + Padding.Horizontal))).If(0, 1));
	}

	public List<DrawableItem<T, TRectangle>> SafeGetItems()
	{
		lock (_sync)
		{
			if (_sortedItems is null)
			{
				return [];
			}

			var list = new List<DrawableItem<T, TRectangle>>();

			for (var i = 0; i < _sortedItems.Count; i++)
			{
				var item = _sortedItems[i];

				if (!item.Hidden)
				{
					list.Add(item);
				}
			}

			return list;
		}
	}

	public bool AnyVisibleItems()
	{
		lock (_sync)
		{
			return _items.Any(x => !x.Hidden);
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
	private readonly List<Rectangle> _invalidatedRectangles = [];

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
		List<DrawableItem<T, TRectangle>> items;

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