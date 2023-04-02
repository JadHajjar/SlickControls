using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace SlickControls
{
	public abstract class SlickSelectionDropDown<T> : SlickControl, ISupportsReset
	{
		internal protected readonly TextBox _searchBox;
		internal protected readonly Label _searchLabel;
		internal protected SlickForm _form;
		internal protected T[] _items;
		internal protected T selectedItem;
		internal protected CustomStackedListControl listDropDown;

		public event EventHandler SelectedItemChanged;

		[Category("Data"), DefaultValue(null)]
		public T[] Items { get => _items; set { _items = value; if (_items?.Length > 0) { selectedItem = _items[0]; } } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public T SelectedItem { get => selectedItem; set { selectedItem = value; OnSelectedItemChanged(); Invalidate(); } }

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		public SlickSelectionDropDown()
		{
			Cursor = Cursors.Hand;

			_searchBox = new TextBox
			{
				BorderStyle = BorderStyle.None
			};

			_searchLabel = new Label
			{
				AutoSize = false,
				Cursor = Cursors.IBeam,
				TextAlign = ContentAlignment.MiddleLeft
			};

			_searchBox.TextChanged += SearchBox_TextChanged;
		}

		private void SearchBox_TextChanged(object sender, EventArgs e)
		{
			_searchLabel.Visible = _searchBox.Text.Length == 0;
			listDropDown?.FilterOrSortingChanged();
		}

		protected override void LocaleChanged()
		{
			base.LocaleChanged();

			_searchLabel.Text = LocaleHelper.GetGlobalText("Search") + "..";
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			_searchBox.BackColor = _searchLabel.BackColor = design.BackColor;
			_searchBox.ForeColor = design.ForeColor;
			_searchLabel.ForeColor = design.InfoColor;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_searchBox?.Dispose();
				listDropDown?.Dispose();
			}

			base.Dispose(disposing);
		}

		public virtual void ShowDropdown()
		{
			if (listDropDown == null)
			{
				if (Items != null && _form != null)
				{
					_searchBox.Font = UI.Font((float)(7.5F*UI.WindowsScale));
					_searchLabel.Font = UI.Font(7.5F, FontStyle.Italic);
					_searchBox.Text = string.Empty;
					_searchBox.Bounds = ClientRectangle.Pad(0, 0, 0, Padding.Bottom + (int)(3 * UI.FontScale)).Align(new Size(Width - (2 * Padding.Horizontal), _searchBox.Height), ContentAlignment.BottomCenter);
					_searchLabel.Bounds = _searchBox.Bounds.Pad(Padding.Left, 0, 0, 0);
					_searchBox.Parent = this;
					_searchLabel.Parent = this;
					_searchLabel.BringToFront();

					_searchBox.Focus();

					listDropDown = new CustomStackedListControl(OrderItems)
					{
						BackColor = FormDesign.Design.AccentBackColor,
						Padding = UI.Scale(new Padding(5), UI.FontScale),
						Location = _form.PointToClient(PointToScreen(new Point(0, Height - 3))),
						Font = Font,
						Cursor = Cursor,
						SeparateWithLines = true,
						MaximumSize = new Size(Width - 1, 9999),
						MinimumSize = new Size(Width - 1, 0),
						Size = new Size(Width - 1, 0)
					};

					listDropDown.CanDrawItem += ListDropDown_CanDrawItem;
					listDropDown.PaintItem += ListDropDown_PaintItem;
					listDropDown.ItemMouseClick += DropDownItems_ItemMouseClick;
					listDropDown.MouseClick += ListDropDown_MouseClick;
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

		private void ListDropDown_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				CloseDropDown();
			}
		}

		private void ListDropDown_CanDrawItem(object sender, CanDrawItemEventArgs<T> e)
		{
			if (!string.IsNullOrWhiteSpace(_searchBox.Text))
			{
				e.DoNotDraw = !SearchMatch(_searchBox.Text, e.Item);
			}
		}

		protected virtual bool SearchMatch(string searchText, T item)
		{
			return searchText.SearchCheck(item.ToString());
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

		protected virtual void CloseDropDown()
		{
			if (listDropDown != null)
			{
				var ctrl = listDropDown;
				new AnimationHandler(ctrl, new Size(Width, 0), 3).StartAnimation(ctrl.Dispose);

				listDropDown = null;
				_searchBox.Parent = null;
				_searchLabel.Parent = null;

				Invalidate();
			}
		}

		protected internal void OnSelectedItemChanged()
		{
			SelectedItemChanged?.Invoke(this, EventArgs.Empty);
		}

		private void DropDownItems_ItemMouseClick(object sender, MouseEventArgs e)
		{
			ItemSelected((T)sender);
		}

		internal virtual void ItemSelected(T item)
		{
			SelectedItem = item;

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
			SlickButton.GetColors(out var fore, out var back, listDropDown != null ? HoverState.Normal : HoverState);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (listDropDown != null)
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);

				using (var brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(150, FormDesign.Design.ButtonColor), Color.Empty, 90))
				{
					e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);
				}

				e.Graphics.DrawString(Text, UI.Font(6.75F, FontStyle.Bold), new SolidBrush(fore), ClientRectangle.Pad(Padding.Left, Padding.Top / 2, 0, 0), new StringFormat { Alignment = StringAlignment.Center });

				var pad = (int)(3 * UI.FontScale);

				if (_searchBox.Focused)
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), _searchBox.Bounds.Pad(-pad + 2).Pad(0, 0, 0, -2), pad);
					e.Graphics.FillRoundedRectangle(new SolidBrush(_searchBox.BackColor), _searchBox.Bounds.Pad(-pad).Pad(0, 0, 0, 2), pad);
				}
				else
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(_searchBox.BackColor), _searchBox.Bounds.Pad(-pad), pad);
				}

				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, 1, 2, -2), Padding.Left);
			}
			else
			{
				using (var brush = ClientRectangle.Gradient(back, 0.5F))
				{
					e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);
				}

				var labelSize = string.IsNullOrWhiteSpace(Text) ? Size.Empty : e.Graphics.Measure(Text, UI.Font(6.75F, FontStyle.Bold));

				e.Graphics.DrawString(Text, UI.Font(6.75F, FontStyle.Bold), new SolidBrush(fore), ClientRectangle.Pad(Padding.Left, Padding.Top / 2, 0, 0));

				PaintItem(e, ClientRectangle.Pad(Padding).Pad(0, (int)(labelSize.Height * 1.2), 0, 0), fore, listDropDown != null ? HoverState.Pressed : HoverState, SelectedItem);

				using (var chevron = (UI.FontScale >= 1.25 ? Properties.Resources.I_DropChevron : Properties.Resources.I_DropChevron_16).Color(fore.MergeColor(back, 90)))
				{
					e.Graphics.DrawImage(chevron, ClientRectangle.Pad(Padding).Align(chevron.Size, ContentAlignment.MiddleRight));
				}
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

		public virtual void ResetValue()
		{
			if (Items != null)
			{
				SelectedItem = Items[0];
			}
		}

		internal protected class CustomStackedListControl : SlickStackedListControl<T>
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
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, -2, 1, 1));
			}
		}
	}
}
