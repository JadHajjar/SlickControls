using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickListControl<T> : SlickControl
	{
		private readonly List<DrawableItem<T>> items;

		[Category("Data"), Browsable(false)]
		public IEnumerable<T> Items
		{
			get
			{
				lock (items)
					foreach (var item in items)
						yield return item.Item;
			}
		}

		[Category("Appearance"), DefaultValue(22)]
		public int ItemHeight { get; set; }

		[Category("Behavior"), DisplayName("Calculate Item Size")]
		public event
#if !NET47
			Extensions.
#endif
			EventHandler<SizeSourceEventArgs<T>> CalculateItemSize;

		[Category("Behavior"), DisplayName("Calculate Item Size")]
		public event
#if !NET47
			Extensions.
#endif
			EventHandler<CanDrawItemEventArgs<T>> CanDrawItem;

		[Category("Appearance"), DisplayName("Paint Item")]
		public event
#if !NET47
			Extensions.
#endif
			EventHandler<ItemPaintEventArgs<T>> PaintItem;

		[Category("Behavior"), DisplayName("Item Mouse Click")]
		public event
#if !NET47
			Extensions.
#endif
			EventHandler<MouseEventArgs> ItemMouseClick;

		protected Point CursorLocation { get; set; }

		public SlickListControl()
		{
			items = new List<DrawableItem<T>>();
			ItemHeight = 22;
			AutoInvalidate = false;
		}

		public virtual void Add(T item)
		{
			lock (items)
				items.Add(new DrawableItem<T>(item));
		}

		public virtual void AddRange(IEnumerable<T> items)
		{
			lock (this.items)
				this.items.AddRange(items.Select(item => new DrawableItem<T>(item)));
		}

		public virtual void Remove(T item)
		{
			lock (items)
				items.Remove(new DrawableItem<T>(item));
		}

		public virtual void RemoveAll(Predicate<T> predicate)
		{
			lock (items)
				items.RemoveAll(item => predicate(item.Item));
		}

		public virtual void Clear()
		{
			lock (items)
				items.Clear();
		}

		public void Invalidate(T item)
		{
			lock (items)
				foreach (var i in items)
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
			lock (items)
				foreach (var item in items)
					if (item.Bounds.Contains(e.Location))
						OnItemMouseClick(item, e);

			base.OnMouseClick(e);
		}

		protected virtual void OnItemMouseClick(DrawableItem<T> item, MouseEventArgs e)
		{
			ItemMouseClick?.Invoke(item.Item, e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			lock (items)
			{
				foreach (var item in items)
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
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			HoverState |= HoverState.Hovered;
			var mouse = PointToClient(Cursor.Position);

			lock (items)
			{
				foreach (var item in items)
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
			lock (items)
			{
				foreach (var item in items)
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

			lock (items)
			{
				foreach (var item in items)
				{
					if (item.Bounds.Contains(e.Location))
						Invalidate(item.Bounds);
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			HoverState &= ~HoverState.Pressed;

			lock (items)
			{
				foreach (var item in items)
				{
					if (item.Bounds.Contains(e.Location))
						Invalidate(item.Bounds);
				}
			}
		}

		protected virtual IEnumerable<DrawableItem<T>> OrderItems(IEnumerable<DrawableItem<T>> items) => items;

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			CursorLocation = PointToClient(Cursor.Position);
		}

		protected virtual void OnPaintItem(ItemPaintEventArgs<T> e)
		{
			PaintItem?.Invoke(this, e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SetUp(BackColor);

			var y = 0;
			var rect = e.ClipRectangle;

			lock (items)
				foreach (var item in OrderItems(items.ToList()))
				{
					var visible = new CanDrawItemEventArgs<T>(item.Item);

					CanDrawItem?.Invoke(this, visible);

					if (visible.DoNotDraw)
					{
						item.Bounds = Rectangle.Empty;
						continue;
					}

					var args = new SizeSourceEventArgs<T>(item.Item, e.Graphics);

					CalculateItemSize?.Invoke(this, args);

					item.Bounds = new Rectangle(0, y, Width, args.Handled ? args.Size : ItemHeight);

					if (item.Bounds.IntersectsWith(rect))
					{
						e.Graphics.SetClip(item.Bounds);
						OnPaintItem(new ItemPaintEventArgs<T>(item, e.Graphics, item.Bounds, item.HoverState | (HoverState & HoverState.Pressed)));
					}

					y += args.Handled ? args.Size : ItemHeight;
				}

			Height = y + 1;
		}
	}
}