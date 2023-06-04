using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickStackedListControl<T, R> : SlickControl where R : IDrawableItemRectangles<T>
	{
		private readonly object _sync = new object();
		private readonly List<DrawableItem<T, R>> _items;
		private int visibleItems;
		protected bool scrollVisible;
		private int scrollIndex;
		protected Rectangle scrollThumbRectangle;
		protected int scrollMouseDown = -1;
		protected DrawableItem<T, R> mouseDownItem;
		private int baseHeight;
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

		[Category("Appearance"), DefaultValue(false)]
		public bool GridView { get => _gridView; set { _gridView = value; OnViewChanged(); Invalidate(); } }

		[Category("Appearance"), DefaultValue(typeof(Size), "0, 0")]
		public Size GridItemSize { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool SeparateWithLines { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool HighlightOnHover { get; set; }

		[Category("Behavior"), DefaultValue(false)]
		public bool HorizontalScrolling { get; set; }

		[Category("Appearance"), DefaultValue(22)]
		public int ItemHeight { get; set; }
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

		protected Point CursorLocation { get; set; }

		protected int StartHeight { get; set; }

		public SlickStackedListControl()
		{
			_items = new List<DrawableItem<T, R>>();
			ItemHeight = 22;
			AutoInvalidate = false;
			ResizeRedraw = false;
			AutoScroll = true;
		}

		public virtual void SortingChanged(bool resetScroll = true)
		{
			lock (_sync)
			{
				_sortedItems = new List<DrawableItem<T, R>>(OrderItems(_items));
			}

			if (resetScroll)
			{
				ResetScroll();
			}

			Invalidate();
		}

		public virtual void FilterChanged()
		{
			if (CanDrawItem == null)
			{
				return;
			}

			List<DrawableItem<T, R>> itemCopy;

			lock (_sync)
			{
				itemCopy = new List<DrawableItem<T, R>>(_items);
			}

			Parallelism.ForEach(itemCopy, x =>
			{
				var canDraw = new CanDrawItemEventArgs<T>(x.Item);

				CanDrawItem(this, canDraw);

				x.Bounds = Rectangle.Empty;
				x.Hidden = canDraw.DoNotDraw;
			});

			Invalidate();
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

			SortingChanged(false);
			FilterChanged();
		}

		public virtual void AddRange(IEnumerable<T> items)
		{
			lock (_sync)
			{
				_items.AddRange(items.Select(item => new DrawableItem<T, R>(item)));
			}

			SortingChanged(false);
			FilterChanged();
		}

		public virtual void SetItems(IEnumerable<T> items)
		{
			lock (_sync)
			{
				_items.Clear();
				_items.AddRange(items.Select(item => new DrawableItem<T, R>(item)));
			}

			SortingChanged(false);
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

			SortingChanged(false);
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

		public void ResetScroll()
		{
			scrollIndex = 0;
			Invalidate();
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
					OnItemMouseClick(mouseDownItem, e);
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
					OnItemMouseClick(mouseDownItem, e);
				}
			}

			base.OnMouseDoubleClick(e);
		}

		protected virtual void OnItemMouseClick(DrawableItem<T, R> item, MouseEventArgs e)
		{
			ItemMouseClick?.Invoke(item.Item, e);
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

			Cursor = itemActionHovered || scrollMouseDown >= 0 || scrollThumbRectangle.Contains(e.Location) ? Cursors.Hand : Cursors.Default;

			if (!isToolTipSet)
			{
				SlickTip.SetTo(this, string.Empty);
			}

			if (AutoInvalidate)
			{
				Invalidate();
			}
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
			if (HighlightOnHover && e.HoverState.HasFlag(HoverState.Hovered))
			{
				var rect = e.ClipRectangle;
				var filledRect = rect.Pad(0, -Padding.Top, 0, -Padding.Bottom);

				using (var brush = new SolidBrush(e.BackColor = BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90)))
				{
					e.Graphics.FillRectangle(brush, filledRect);
				}
			}
			else
			{
				e.BackColor = BackColor;
			}

			PaintItemList?.Invoke(this, e);
		}

		protected virtual void OnPaintItemGrid(ItemPaintEventArgs<T, R> e)
		{
			e.BackColor = BackColor.Tint(Lum: FormDesign.Design.Type == FormDesignType.Dark ? 4 : -5);

			if (HighlightOnHover && e.HoverState.HasFlag(HoverState.Hovered))
			{
				e.BackColor = e.BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);
			}

			using (var brush = new SolidBrush(e.BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.Pad(-Padding.Left, -Padding.Top, -Padding.Right, -Padding.Bottom), (int)(5 * UI.FontScale));
			}
			PaintItemGrid?.Invoke(this, e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var invalidRect = e.ClipRectangle;

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
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(7, FormDesign.Design.Type == FormDesignType.Dark ? Color.White : Color.Black)), new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
				}

				e.Graphics.FillRoundedRectangle(scrollThumbRectangle.Gradient(isMouseDown ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), scrollThumbRectangle.Pad(2, 0, 2, 0), 3);
			}

			var start = scrollIndex;

			if (GridView)
			{
				start *= (int)Math.Floor((double)Width / GridItemSize.Width);
			}

			for (var i = start; i < itemList.Count; i++)
			{
				var item = itemList[i];

				if (GridView)
				{
					item.Bounds = new Rectangle(loc, GridItemSize).Pad(Padding);
				}
				else
				{
					item.Bounds = new Rectangle(loc, new Size(Width - (scrollVisible ? scrollThumbRectangle.Width + 1 : 0), ItemHeight + Padding.Vertical + (SeparateWithLines ? (int)UI.FontScale : 0)));
				}

				if (invalidRect.IntersectsWith(item.Bounds))
				{
					e.Graphics.SetClip(item.Bounds);

					OnPaintItem(new ItemPaintEventArgs<T, R>(
						item,
						e.Graphics,
						GridView ? item.Bounds.Pad(Padding) : item.Bounds.Pad(0, Padding.Top, 0, Padding.Bottom),
						mouseDownItem == item ? (HoverState.Pressed | HoverState.Hovered) : mouseDownItem == null ? item.HoverState : HoverState.Normal));

					e.Graphics.ResetClip();
				}

				if (GridView)
				{
					loc.X += item.Bounds.Width + Padding.Horizontal;

					if (loc.X + item.Bounds.Width > Width)
					{
						loc.X = 0;
						loc.Y += item.Bounds.Height + Padding.Vertical;
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
				return _sortedItems?.Where(x => !x.Hidden).ToList() ?? new List<DrawableItem<T, R>>();
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
	}
}