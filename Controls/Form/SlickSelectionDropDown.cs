using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace SlickControls
{
	public abstract class SlickSelectionDropDown<T> : SlickControl, ISupportsReset
	{
		protected internal readonly TextBox _searchBox;
		protected internal readonly Label _searchLabel;
		protected internal SlickForm _form;
		protected internal T[] _items;
		protected internal T selectedItem;
		protected internal CustomStackedListControl listDropDown;

		public event EventHandler SelectedItemChanged;

		[Category("Data"), DefaultValue(null)]
		public T[] Items { get => _items; set { _items = value; if (_items?.Length > 0 && (selectedItem?.Equals(default) ?? false)) { selectedItem = _items[0]; } } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public T SelectedItem { get => selectedItem; set { selectedItem = value; OnSelectedItemChanged(); Invalidate(); } }

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		[DefaultValue(false), Category("Appearance")]
		public bool AccentBackColor { get; set; }
		[DefaultValue(false), Category("Appearance")]
		public bool HideLabel { get; set; }
		[DefaultValue(14), Category("Appearance")]
		public int ItemHeight { get; set; } = 14;

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
			listDropDown?.FilterChanged();
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
					if (Items.Length > 12)
					{
						_searchBox.Font = UI.Font((float)(6.5F * UI.WindowsScale * UI.UIScale / UI.FontScale));
						_searchLabel.Font = UI.Font(6.5F, FontStyle.Italic);
						_searchLabel.ForeColor = FormDesign.Design.InfoColor;
						_searchBox.Text = string.Empty;
						_searchBox.Bounds = ClientRectangle.Pad(0, 0, 0, (Padding.Bottom / 2) + 4).Align(new Size(Width - (2 * Padding.Horizontal), _searchBox.Height), ContentAlignment.BottomCenter).Pad(0, 0, _searchBox.Height + Padding.Horizontal, 0);
						_searchLabel.Bounds = _searchBox.Bounds.Pad(2, 0, 0, 0);
						_searchBox.Parent = this;
						_searchLabel.Parent = this;
						_searchLabel.BringToFront();

						_searchBox.Focus();
					}

					listDropDown = new CustomStackedListControl(OrderItems)
					{
						ItemHeight = ItemHeight,
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
					listDropDown.PaintItemList += ListDropDown_PaintItem;
					listDropDown.ItemMouseClick += DropDownItems_ItemMouseClick;
					listDropDown.MouseClick += ListDropDown_MouseClick;
					listDropDown.Parent = _form;
					listDropDown.BringToFront();
					listDropDown.SetItems(Items);

					new AnimationHandler(listDropDown, new Size(Width, Math.Min((listDropDown.ItemHeight + listDropDown.Padding.Vertical + (int)UI.FontScale) * Math.Min(11, Items.Length), _form.Height - listDropDown.Top - 15)), 3).StartAnimation();
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
			if (Live)
			{
				Font = UI.Font(8.25F);
				Padding = UI.Scale(new Padding(5), UI.FontScale);

				if (Dock == DockStyle.Fill)
				{
					return;
				}

				var size = UI.Scale(new Size(150, string.IsNullOrEmpty(Text) || HideLabel ? 26 : 32), UI.UIScale);

				if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
				{
					size.Height = 0;
				}

				if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
				{
					size.Width = 0;
				}

				Size = size;
			}
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

		protected virtual IEnumerable<T> OrderItems(IEnumerable<T> items)
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

			if (e.Button == MouseButtons.Middle && listDropDown == null)
			{
				ResetValue();
				return;
			}

			if (e.Button == MouseButtons.Left)
			{
				ShowDropdown();
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (Keys.Down == keyData)
			{
				if (listDropDown == null)
				{
					ShowDropdown();
				}

				return true;
			}

			if (Keys.Up == keyData)
			{
				if (listDropDown != null)
				{
					CloseDropDown();
				}

				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected virtual void CloseDropDown()
		{
			if (listDropDown != null)
			{
				var ctrl = listDropDown;
				new AnimationHandler(ctrl, new Size(Width, 0), 3.5).StartAnimation(ctrl.Dispose);

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

			e.Graphics.SetUp(BackColor);

			if (listDropDown != null)
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, false, false);

				using (var brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(75, FormDesign.Design.AccentColor), Color.Empty, 90))
				{
					e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, false, false);
				}

				if (_searchBox.Parent != null)
				{
					PaintForListOpen(e, fore);
				}
				else
				{
					PaintForListOpenNoSearch(e, fore);
				}

				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, 1, 2, -2), Padding.Left, true, true, false, false);
			}
			else
			{
				if (AccentBackColor)
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(Focused ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), ClientRectangle.Pad(1, 1, 2, 1), 4);

					e.Graphics.FillRoundedRectangle(new SolidBrush(HoverState.HasFlag(HoverState.Hovered) ? back : FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 3), 4);
				}
				else
				{
					using (var brush = AccentBackColor ? new SolidBrush(back) : ClientRectangle.Gradient(back, 0.5F))
					{
						e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);
					}
				}

				var labelSize = string.IsNullOrWhiteSpace(Text) || HideLabel ? Size.Empty : e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(6.5F));

				if (!HideLabel)
				{
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), UI.Font(6.5F), new SolidBrush(Color.FromArgb(200, fore)), ClientRectangle.Pad(Padding.Left, Padding.Top / 4, 0, 0));
				}

				using (var chevron = IconManager.GetIcon("I_DropChevron", (ClientRectangle.Height - Padding.Vertical) / 2).Color(fore.MergeColor(back, 90)))
				{
					PaintSelectedItem(e, fore, (string.IsNullOrWhiteSpace(Text) || HideLabel ? ClientRectangle.Pad(Padding) : ClientRectangle.Pad(Padding).Pad(0, (int)(labelSize.Height * 0.65), 0, -Padding.Bottom / 2)).Pad(0, 0, chevron.Width, 0));

					e.Graphics.DrawImage(chevron, ClientRectangle.Pad(Padding).Align(chevron.Size, ContentAlignment.MiddleRight));
				}
			}
		}

		private void PaintForListOpenNoSearch(PaintEventArgs e, Color fore)
		{
			e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), UI.Font(6.5F, FontStyle.Bold), new SolidBrush(fore), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		protected virtual void PaintSelectedItem(PaintEventArgs e, Color fore, Rectangle rectangle)
		{
			PaintItem(e, rectangle, fore, HoverState, SelectedItem);
		}

		protected virtual void PaintForListOpen(PaintEventArgs e, Color fore)
		{
			e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), UI.Font(6.5F), new SolidBrush(Color.FromArgb(200, fore)), ClientRectangle.Pad(_searchBox.Left, 1, 0, 0));

			var pad = 3;

			if (_searchBox.Focused)
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), _searchBox.Bounds.Pad(-pad + 2).Pad(0, 0, -_searchBox.Height - Padding.Right, -2), pad);
				e.Graphics.FillRoundedRectangle(new SolidBrush(_searchBox.BackColor), _searchBox.Bounds.Pad(-pad).Pad(0, 0, -_searchBox.Height - Padding.Right, 2), pad);
			}
			else
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(_searchBox.BackColor), _searchBox.Bounds.Pad(-pad).Pad(0, 0, -_searchBox.Height - Padding.Right, 0), pad);
			}

			using (var icon = IconManager.GetIcon("I_Search", _searchBox.Height))
			{
				e.Graphics.DrawImage(icon.Color(FormDesign.Design.IconColor), new Rectangle(new Point(_searchBox.Right + Padding.Left, _searchBox.Top + ((_searchBox.Height - icon.Height) / 2)), icon.Size));
			}
		}

		private void ListDropDown_PaintItem(object sender, ItemPaintEventArgs<T, Rectangles> e)
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

			var selected = !(this is SlickMultiSelectionDropDown<T>) && e.Item.Equals(selectedItem);

			if (selected && !e.HoverState.HasFlag(HoverState.Pressed))
			{
				var bar = (int)(4 * UI.FontScale);
				using (var brush = new LinearGradientBrush(e.ClipRectangle.Pad(e.ClipRectangle.Width / 4, 0, 0, 0), Color.Empty, Color.FromArgb(50, FormDesign.Design.ActiveColor), LinearGradientMode.Horizontal))
				{
					e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.Pad(0, -Padding.Top, 0, -Padding.Bottom).Pad((e.ClipRectangle.Width / 4) + 1, 1, bar, 1), bar);
				}

				using (var brush = new SolidBrush(FormDesign.Design.ActiveColor))
				{
					e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.ActiveColor), new Rectangle(e.ClipRectangle.Right - (3 * bar / 2) - 1, e.ClipRectangle.Y + 1, bar * 3 / 2, e.ClipRectangle.Height - 2).Pad(0, -Padding.Top, 0, -Padding.Bottom));
				}

				fore = FormDesign.Design.ActiveColor;
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

		protected internal class CustomStackedListControl : SlickStackedListControl<T, Rectangles>
		{
			private readonly Func<IEnumerable<T>, IEnumerable<T>> _orderMethod;

			public CustomStackedListControl(Func<IEnumerable<T>, IEnumerable<T>> orderMethod = null)
			{
				_orderMethod = orderMethod;
			}

			protected override bool IsItemActionHovered(DrawableItem<T, Rectangles> item, Point location)
			{
				return true;
			}

			protected override IEnumerable<DrawableItem<T, Rectangles>> OrderItems(IEnumerable<DrawableItem<T, Rectangles>> items)
			{
				if (_orderMethod == null)
				{
					return base.OrderItems(items);
				}

				var sortedItems = _orderMethod( items.Select(x => x.Item)).ToList();

				return items.OrderBy(x => sortedItems.IndexOf(x.Item));
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);

				e.Graphics.ResetClip();
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(120, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, -2, 1, 1));
			}
		}

		public class Rectangles : IDrawableItemRectangles<T>
		{
			public T Item { get; set; }

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
}
