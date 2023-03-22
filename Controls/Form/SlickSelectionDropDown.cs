using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace SlickControls.Controls.Form
{
	public abstract class SlickSelectionDropDown<T> : SlickControl, ISupportsReset
	{
		private SlickForm _form;
		private T[] _items;
		private T selectedItem;
		private CustomStackedListControl listDropDown;

		public event EventHandler SelectedItemChanged;

		[Category("Data"), DefaultValue(null)]
		public T[] Items { get => _items; set { _items = value; if (_items?.Length > 0) { selectedItem = _items[0]; } } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public T SelectedItem { get => selectedItem; set { selectedItem = value; SelectedItemChanged?.Invoke(this, EventArgs.Empty); Invalidate(); } }

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		public SlickSelectionDropDown()
		{
			Cursor = Cursors.Hand;
		}

		public void ShowDropdown()
		{
			if (listDropDown == null)
			{
				if (Items != null && _form != null)
				{
					listDropDown = new CustomStackedListControl(OrderItems)
					{
						BackColor = FormDesign.Design.ButtonColor,
						Padding = UI.Scale(new Padding(5), UI.FontScale),
						Location = _form.PointToClient(PointToScreen(new Point(0, Height - 3))),
						Font = Font,
						Cursor = Cursor,
						SeparateWithLines = true,
						MaximumSize = new Size(Width - 1, 9999),
						MinimumSize = new Size(Width - 1, 0),
						Size = new Size(Width - 1, 0)
					};

					listDropDown.PaintItem += ListDropDown_PaintItem;
					listDropDown.ItemMouseClick += DropDownItems_ItemMouseClick;
					listDropDown.Parent = _form;
					listDropDown.BringToFront();
					listDropDown.SetItems(Items);

					new AnimationHandler(listDropDown, new Size(Width, Math.Min((listDropDown.ItemHeight + listDropDown.Padding.Vertical + (int)UI.FontScale) * Math.Min(10, Items.Length), _form.Height - listDropDown.Top - 15)), 2.5).StartAnimation();
				}
				else
				{
					SystemSounds.Exclamation.Play();
				}
			}
			else
			{
				CloseDropDown();
			}
		}

		protected override void UIChanged()
		{
			Padding = UI.Scale(new Padding(5), UI.FontScale);
			Height = Font.Height + Padding.Vertical;
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			SetUpForm();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			SetUpForm();
		}

		private void SetUpForm()
		{
			if (_form == null && FindForm() is SlickForm slickForm)
			{
				_form = slickForm;
				_form.OnWndProc += Frm_OnWndProc;
				Disposed += (s, _) => _form.OnWndProc -= Frm_OnWndProc;

				LocationChanged += (s, ea) =>
				{
					if (listDropDown != null)
					{
						listDropDown.Location = _form.PointToClient(PointToScreen(new Point(0, Height - 2)));
					}
				};

				_form.LocationChanged += (s, ea) =>
				{
					if (listDropDown != null)
					{
						listDropDown.Location = _form.PointToClient(PointToScreen(new Point(0, Height - 2)));
					}
				};

				_form.Resize += (s, ea) =>
				{
					if (listDropDown != null && listDropDown.Visible)
					{
						if (_form.WindowState == FormWindowState.Minimized)
						{
							CloseDropDown();
						}
						else
						{
							listDropDown.Location = _form.PointToClient(PointToScreen(new Point(0, Height - 2)));
							listDropDown.MaximumSize = new Size(Width, 9999);
							listDropDown.MinimumSize = new Size(Width, 0);
						}
					}
				};
			}
		}

		protected virtual IEnumerable<DrawableItem<T>> OrderItems(IEnumerable<DrawableItem<T>> items)
		{
			return items;
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);

			CloseDropDown();
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Left)
			{
				ShowDropdown();
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (Keys.Down == keyData && listDropDown == null)
			{
				ShowDropdown();
				return true;
			}

			if (Keys.Up == keyData && listDropDown != null)
			{
				CloseDropDown();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void CloseDropDown()
		{
			if (listDropDown != null)
			{
				var ctrl = listDropDown;
				new AnimationHandler(ctrl, new Size(Width, 0), 3).StartAnimation(ctrl.Dispose);

				listDropDown = null;

				Invalidate();
			}
		}

		private void DropDownItems_ItemMouseClick(object sender, MouseEventArgs e)
		{
			SelectedItem = (T)sender;

			CloseDropDown();
		}

		private bool Frm_OnWndProc(Message arg)
		{
			if (Visible
				&& listDropDown != null
				&& arg.Msg == 0x21
				&& !new Rectangle(PointToScreen(Point.Empty), Size).Contains(Cursor.Position)
				&& !new Rectangle(listDropDown.PointToScreen(Point.Empty), listDropDown.Size).Contains(Cursor.Position))
			{
				CloseDropDown();
			}

			return false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			SlickButton.GetColors(out var fore, out var back, listDropDown != null ? HoverState.Hovered : HoverState);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			using (var brush = ClientRectangle.Gradient(back, 0.5F))
			{
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);
			}

			if (listDropDown != null)
			{
				e.Graphics.DrawString(Text, new Font(Font, FontStyle.Bold), new SolidBrush(fore), ClientRectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				
				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(150, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, 1, 2, -2), Padding.Left);
			}
			else
			{
				var labelSize = string.IsNullOrWhiteSpace(Text) ? Size.Empty : e.Graphics.Measure(Text, UI.Font(6.75F, FontStyle.Bold));

				e.Graphics.DrawString(Text, UI.Font(6.75F, FontStyle.Bold), new SolidBrush(fore), ClientRectangle.Pad(Padding.Left, Padding.Top / 2, 0, 0));

				PaintItem(e, ClientRectangle.Pad(Padding).Pad(0, (int)(labelSize.Height * 1.2), 0, 0), fore, listDropDown != null ? HoverState.Pressed : HoverState, SelectedItem);

				using (var chevron = (UI.FontScale >= 1.25 ? Properties.Resources.I_DropChevron : Properties.Resources.I_DropChevron_16).Color(fore.MergeColor(back, 90)))
					e.Graphics.DrawImage(chevron, ClientRectangle.Pad(Padding).Align(chevron.Size, ContentAlignment.MiddleRight));
			}
		}

		private void ListDropDown_PaintItem(object sender, ItemPaintEventArgs<T> e)
		{
			SlickButton.GetColors(out var fore, out var back, e.HoverState);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (e.HoverState.HasFlag(HoverState.Hovered) || e.HoverState.HasFlag(HoverState.Pressed))
			{
				using (var brush = new SolidBrush(back))
				{
					e.Graphics.SetClip(e.ClipRectangle.Pad(0, -Padding.Top, 0, -Padding.Bottom));
					e.Graphics.FillRectangle(brush, e.Graphics.ClipBounds);
				}
			}

			PaintItem(e, e.ClipRectangle.Pad(Padding.Left, 0, Padding.Right, 0), fore, e.HoverState, e.Item);
		}

		protected abstract void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item);

		public void ResetValue()
		{
			if (Items != null)
			{
				SelectedItem = Items[0];
			}
		}

		private class CustomStackedListControl : SlickStackedListControl<T>
		{
			private readonly Func<IEnumerable<DrawableItem<T>>, IEnumerable<DrawableItem<T>>> _orderMethod;

			public CustomStackedListControl(Func<IEnumerable<DrawableItem<T>>, IEnumerable<DrawableItem<T>>> orderMethod = null)
            {
				_orderMethod = orderMethod;
			}

            protected override bool IsItemActionHovered(DrawableItem<T> item, Point location)
			{
				return true;
			}

			protected override IEnumerable<DrawableItem<T>> OrderItems(IEnumerable<DrawableItem<T>> items)
			{
				if (_orderMethod == null)
				{
					return base.OrderItems(items);
				}

				return _orderMethod(items);
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);

				e.Graphics.ResetClip();
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(150, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, -2, 1, 1));
			}
		}
	}
}
