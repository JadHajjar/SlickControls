using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickStackedListControl<T> : SlickControl
	{
		private readonly object _sync = new object();
		private readonly List<DrawableItem<T>> _items;
		private int visibleItems;
		protected bool scrollVisible;
		private int scrollIndex;
		protected Rectangle scrollThumbRectangle;
		protected int scrollMouseDown = -1;
		protected DrawableItem<T> mouseDownItem;
		private int baseHeight;
		private List<DrawableItem<T>> _sortedItems;

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
		public bool SeparateWithLines { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool DoubleSizeOnHover { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool HighlightOnHover { get; set; }

		[Category("Appearance"), DefaultValue(22)]
		public int ItemHeight { get; set; }

		[Category("Behavior"), DisplayName("Can Draw Item")]
		public event Extensions.EventHandler<CanDrawItemEventArgs<T>> CanDrawItem;

		[Category("Appearance"), DisplayName("Paint Item")]
		public event Extensions.EventHandler<ItemPaintEventArgs<T>> PaintItem;

		[Category("Behavior"), DisplayName("Item Mouse Click")]
		public event Extensions.EventHandler<MouseEventArgs> ItemMouseClick;

		protected Point CursorLocation { get; set; }

		protected int StartHeight { get; set; }

		public SlickStackedListControl()
		{
			_items = new List<DrawableItem<T>>();
			ItemHeight = 22;
			AutoInvalidate = false;
			AutoScroll = true;
		}

		public void FilterOrSortingChanged()
		{
			lock (_sync)
			{
				_sortedItems = new List<DrawableItem<T>>(OrderItems(_items));
			}

			if (CanDrawItem == null)
			{
				this.TryInvoke(Invalidate);

				return;
			}

			List<DrawableItem<T>> itemCopy;

			lock (_sync)
			{
				itemCopy = new List<DrawableItem<T>>(_items);
			}

			Parallelism.ForEach(itemCopy, x =>
			{
				var canDraw = new CanDrawItemEventArgs<T>(x.Item);

				CanDrawItem(this, canDraw);

				x.Bounds = Rectangle.Empty;
				x.Hidden = canDraw.DoNotDraw;
			});

			this.TryInvoke(Invalidate);
		}

		public virtual void Invalidate(T item)
		{
			if (DoubleSizeOnHover)
			{
				Invalidate();
				return;
			}

			lock (_sync)
			{
				var selectedItem = _items.FirstOrDefault(x => x.Item.Equals(item));

				if (selectedItem != null)
				{
					Invalidate(selectedItem.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
				}
			}
		}

		public virtual void Add(T item)
		{
			lock (_sync)
			{
				_items.Add(new DrawableItem<T>(item));
			}

			FilterOrSortingChanged();
		}

		public virtual void AddRange(IEnumerable<T> items)
		{
			lock (_sync)
			{
				_items.AddRange(items.Select(item => new DrawableItem<T>(item)));
			}

			FilterOrSortingChanged();
		}

		public virtual void SetItems(IEnumerable<T> items)
		{
			lock (_sync)
			{
				_items.Clear();
				_items.AddRange(items.Select(item => new DrawableItem<T>(item)));
			}

			FilterOrSortingChanged();
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

			FilterOrSortingChanged();
		}

		public virtual void Clear()
		{
			lock (_sync)
			{
				_items.Clear();
			}

			FilterOrSortingChanged();
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

		protected virtual void OnItemMouseClick(DrawableItem<T> item, MouseEventArgs e)
		{
			ItemMouseClick?.Invoke(item.Item, e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var itemActionHovered = false;

			lock (_sync)
			{
				foreach (var item in _items)
				{
					if (!item.Hidden && item.Bounds.Contains(e.Location))
					{
						item.HoverState |= HoverState.Hovered;
						itemActionHovered |= (mouseDownItem == null || mouseDownItem == item) && IsItemActionHovered(item, e.Location);
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

				scrollIndex = (itemList.Count - visibleItems) * (e.Location.Y - scrollMouseDown) / (Height - scrollThumbRectangle.Height - StartHeight).If(0, 1);
				Invalidate();
			}

			if (scrollVisible)
			{
				Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
			}

			Cursor = itemActionHovered || scrollMouseDown >= 0 || scrollThumbRectangle.Contains(e.Location) ? Cursors.Hand : Cursors.Default;

			if (!itemActionHovered)
			{
				SlickTip.SetTo(this, string.Empty);
			}
		}

		private void Invalidate(DrawableItem<T> item)
		{
			if (DoubleSizeOnHover)
			{
				Invalidate();
			}
			else
			{
				Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
			}
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
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			scrollIndex -= e.Delta / ItemHeight;
			Invalidate();
		}

		protected virtual bool IsItemActionHovered(DrawableItem<T> item, Point location)
		{
			return false;
		}

		protected virtual IEnumerable<DrawableItem<T>> OrderItems(IEnumerable<DrawableItem<T>> items)
		{
			return items;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			CursorLocation = PointToClient(Cursor.Position);

			e.Graphics.Clear(BackColor);
		}

		protected virtual void OnPaintItem(ItemPaintEventArgs<T> e)
		{
			if (HighlightOnHover && e.HoverState.HasFlag(HoverState.Hovered))
			{
				var rect = e.ClipRectangle;
				var filledRect = rect.Pad(0, -Padding.Top, 0, -Padding.Bottom);

				e.Graphics.SetClip(filledRect);
				e.Graphics.FillRectangle(filledRect.Gradient(Color.FromArgb(e.HoverState.HasFlag(HoverState.Pressed) ? 255 : 30, FormDesign.Design.ActiveColor)), filledRect);
				e.Graphics.SetClip(rect);
			}

			PaintItem?.Invoke(this, e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			lock (_sync)
			{
				if (Loading && _items.Count == 0)
				{
					DrawLoader(e.Graphics, ClientRectangle.CenterR(UI.Scale(new Size(32, 32), UI.FontScale)));
					return;
				}
			}

			var y = StartHeight;
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

			for (var i = scrollIndex; i < itemList.Count; i++)
			{
				var item = itemList[i];
				var doubleSize = DoubleSizeOnHover && (mouseDownItem == item || mouseDownItem == null) && (item.HoverState.HasFlag(HoverState.Hovered) || item.HoverState.HasFlag(HoverState.Pressed));
				item.Bounds = new Rectangle(0, y, Width - (scrollVisible ? scrollThumbRectangle.Width + 1 : 0), ((doubleSize ? 2 : 1) * ItemHeight) + Padding.Vertical + (SeparateWithLines ? (int)UI.FontScale : 0));

				e.Graphics.SetClip(item.Bounds);

				OnPaintItem(new ItemPaintEventArgs<T>(
					item,
					e.Graphics,
					item.Bounds.Pad(0, Padding.Top, 0, Padding.Bottom),
					mouseDownItem == item ? (HoverState.Pressed | HoverState.Hovered) : mouseDownItem == null ? item.HoverState : HoverState.Normal));

				e.Graphics.ResetClip();

				y += item.Bounds.Height;

				if (SeparateWithLines)
				{
					e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor, (int)UI.FontScale), Padding.Left, y, Width - Padding.Right - (int)(scrollVisible ? 6 * UI.FontScale : 0), y);
				}

				if (y > Height)
				{
					break;
				}
			}
		}

		private void HandleScrolling(List<DrawableItem<T>> itemList)
		{
			var totalHeight = GetTotalHeight(itemList);

			var validHeight = Height - StartHeight;

			if (totalHeight > validHeight)
			{
				visibleItems = (int)Math.Floor((float)validHeight / (ItemHeight + Padding.Vertical + (SeparateWithLines ? (int)UI.FontScale : 0)));
				scrollVisible = true;
				scrollIndex = Math.Max(0, Math.Min(scrollIndex, itemList.Count - visibleItems));

				var thumbHeight = Math.Max(validHeight * visibleItems / itemList.Count, validHeight / 24);

				scrollThumbRectangle = new Rectangle(Width - (int)(10 * UI.FontScale), StartHeight + ((validHeight - thumbHeight) * scrollIndex / (itemList.Count - visibleItems).If(0, 1)), (int)(10 * UI.FontScale), thumbHeight);
			}
			else
			{
				scrollVisible = false;
				scrollIndex = 0;
			}
		}

		public int GetTotalHeight(List<DrawableItem<T>> itemList)
		{
			var totalHeight = itemList.Count * (ItemHeight + Padding.Vertical);

			if (SeparateWithLines)
			{
				totalHeight += (itemList.Count - 1) * (int)UI.FontScale;
			}

			if (DoubleSizeOnHover && itemList.Any(item => (mouseDownItem == item || mouseDownItem == null) && (item.HoverState.HasFlag(HoverState.Hovered) || item.HoverState.HasFlag(HoverState.Pressed))))
			{
				totalHeight += ItemHeight + Padding.Vertical;
			}

			return totalHeight;
		}

		public List<DrawableItem<T>> SafeGetItems()
		{
			lock (_sync)
			{
				return _sortedItems?.Where(x => !x.Hidden).ToList() ?? new List<DrawableItem<T>>();
			}
		}

		protected void ScrollTo(T item)
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