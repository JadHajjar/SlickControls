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
		private readonly List<DrawableItem<T>> _items;
		private int visibleItems;
		private bool scrollVisible;
		private int scrollIndex;
		private Rectangle scrollThumbRectangle;
		private int scrollMouseDown = -1;

		[Category("Data"), Browsable(false)]
		public IEnumerable<T> Items
		{
			get
			{
				lock (_items)
					foreach (var item in _items)
						yield return item.Item;
			}
		}

		[Category("Appearance"), DefaultValue(false)]
		public bool SeparateWithLines { get; set; }

		[Category("Appearance"), DefaultValue(22)]
		public int ItemHeight { get; set; }

		[Category("Behavior"), DisplayName("Calculate Item Size")]
		public event Extensions.EventHandler<CanDrawItemEventArgs<T>> CanDrawItem;

		[Category("Appearance"), DisplayName("Paint Item")]
		public event Extensions.EventHandler<ItemPaintEventArgs<T>> PaintItem;

		[Category("Behavior"), DisplayName("Item Mouse Click")]
		public event Extensions.EventHandler<MouseEventArgs> ItemMouseClick;

		protected Point CursorLocation { get; set; }

		public SlickStackedListControl()
		{
			_items = new List<DrawableItem<T>>();
			ItemHeight = 22;
			AutoInvalidate = false;
		}

		public virtual void Add(T item)
		{
			lock (_items)
				_items.Add(new DrawableItem<T>(item));
		}

		public virtual void AddRange(IEnumerable<T> items)
		{
			lock (this._items)
				this._items.AddRange(items.Select(item => new DrawableItem<T>(item)));
		}

		public virtual void Remove(T item)
		{
			lock (_items)
				_items.Remove(new DrawableItem<T>(item));
		}

		public virtual void RemoveAll(Predicate<T> predicate)
		{
			lock (_items)
				_items.RemoveAll(item => predicate(item.Item));
		}

		public virtual void Clear()
		{
			lock (_items)
				_items.Clear();
		}

		public void Invalidate(T item)
		{
			lock (_items)
				foreach (var i in _items)
					if (i.Item.Equals(item))
						Invalidate(i.Bounds);
		}

		protected override void UIChanged()
		{
			if (Live)
				ItemHeight = (int)(ItemHeight * UI.FontScale);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			lock (_items)
				foreach (var item in _items)
					if (item.Bounds.Contains(e.Location))
						OnItemMouseClick(item.Item, e);

			base.OnMouseClick(e);
		}

		protected virtual void OnItemMouseClick(T item, MouseEventArgs e)
		{
			ItemMouseClick?.Invoke(item, e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(e.Location))
					{
						item.HoverState |= HoverState.Hovered;
						Invalidate(item.Bounds);
					}
					else if (item.HoverState.HasFlag(HoverState.Hovered))
					{
						item.HoverState &= ~HoverState.Hovered;
						Invalidate(item.Bounds);
					}
				}
			}

			if (scrollMouseDown >= 0)
			{
				scrollIndex = (e.Location.Y - scrollMouseDown) / (Height - scrollThumbRectangle.Height);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			HoverState |= HoverState.Hovered;
			var mouse = PointToClient(Cursor.Position);

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(mouse))
					{
						item.HoverState |= HoverState.Hovered;
						Invalidate(item.Bounds);
					}
					else if (item.HoverState.HasFlag(HoverState.Hovered))
					{
						item.HoverState &= ~HoverState.Hovered;
						Invalidate(item.Bounds);
					}
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			HoverState &= ~HoverState.Hovered;

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.HoverState.HasFlag(HoverState.Hovered))
					{
						item.HoverState &= ~HoverState.Hovered;
						Invalidate(item.Bounds);
					}
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			HoverState |= HoverState.Pressed;

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(e.Location))
						Invalidate(item.Bounds);
				}
			}

			if (scrollVisible && e.Location.X > Width - (int)(6 * UI.FontScale))
			{
				if (scrollThumbRectangle.Contains(e.Location))
				{
					scrollMouseDown = e.Location.Y - scrollThumbRectangle.Y;
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
				}
				
				Invalidate(scrollThumbRectangle);
			}
			else
			{
				scrollMouseDown = -1;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			HoverState &= ~HoverState.Pressed;

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(e.Location))
						Invalidate(item.Bounds);
				}
			}

			if (scrollMouseDown >= 0)
			{
				scrollMouseDown = -1;
				Invalidate(scrollThumbRectangle);
			}
		}

		protected override void OnScroll(ScrollEventArgs se)
		{
			base.OnScroll(se);

			//scrollIndex -= se % 1
		}

		protected virtual IEnumerable<DrawableItem<T>> OrderItems(IEnumerable<DrawableItem<T>> items) => items;

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			CursorLocation = PointToClient(Cursor.Position);

			e.Graphics.Clear(BackColor);
		}

		protected virtual void OnPaintItem(ItemPaintEventArgs<T> e)
		{
			PaintItem?.Invoke(this, e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var y = 0;

			var itemList = SafeGetItems();

			if (CanDrawItem != null)
			{
				itemList.RemoveAll(x =>
				{
					var canDraw = new CanDrawItemEventArgs<T>(x.Item);

					CanDrawItem(this, canDraw);

					return canDraw.DoNotDraw;
				});
			}

			HandleScrolling(itemList);

			if (scrollVisible)
			{
				var isMouseDown = HoverState.HasFlag(HoverState.Pressed) && scrollThumbRectangle.Contains(CursorLocation);

				e.Graphics.FillRoundedRectangle(scrollThumbRectangle.Gradient(isMouseDown ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), scrollThumbRectangle, 2);
			}

			foreach (var item in OrderItems(itemList).Skip(scrollIndex))
			{
				item.Bounds = new Rectangle(0, y + Padding.Top, Width - (int)(scrollVisible ? 6 * UI.FontScale : 0), ItemHeight);

				e.Graphics.SetClip(item.Bounds);

				OnPaintItem(new ItemPaintEventArgs<T>(
					item.Item,
					e.Graphics,
					item.Bounds,
					item.HoverState | (HoverState & (HoverState.Pressed | HoverState.Focused))));

				e.Graphics.ResetClip();

				y += ItemHeight + Padding.Vertical;

				if (SeparateWithLines)
				{
					e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor, (int)UI.FontScale), Padding.Left, y, Width - Padding.Right, y);

					y += (int)UI.FontScale;
				}

				if (y > Height)
					break;
			}
		}

		private void HandleScrolling(List<DrawableItem<T>> itemList)
		{
			var totalHeight = itemList.Count * (ItemHeight + Padding.Vertical);

			if (SeparateWithLines)
			{
				totalHeight += (itemList.Count - 1) * (int)UI.FontScale;
			}

			if (totalHeight > Height)
			{
				visibleItems = (int)Math.Floor((float)Height / (ItemHeight + Padding.Vertical + (int)UI.FontScale));
				scrollVisible = true;
				scrollIndex = Math.Max(0, Math.Min(scrollIndex, itemList.Count - visibleItems));
				scrollThumbRectangle = new Rectangle(Width - (int)(6 * UI.FontScale), Height * scrollIndex / itemList.Count, (int)(6 * UI.FontScale), Height * visibleItems / itemList.Count);
			}
			else
			{
				scrollVisible = false;
			}
		}

		private List<DrawableItem<T>> SafeGetItems()
		{
			lock (_items)
			{
				return _items.ToList();
			}
		}
	}
}