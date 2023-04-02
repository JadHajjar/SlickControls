using Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
	public abstract class SlickMultiSelectionDropDown<T> : SlickSelectionDropDown<T>
	{
		private Rectangle ClearRectangle => new Rectangle(_searchBox.Right + Padding.Right, _searchBox.Top - (int)(3 * UI.FontScale), _searchBox.Height + (int)(6 * UI.FontScale), _searchBox.Height + (int)(6 * UI.FontScale));
		private readonly List<T> _selectedItems = new List<T>();

		public IEnumerable<T> SelectedItems =>new List<T>(_selectedItems);

		public override void ResetValue()
		{
			_selectedItems.Clear();
			listDropDown?.Invalidate();
			OnSelectedItemChanged();
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle && listDropDown == null)
			{
				ResetValue();
				return;
			}

			if (e.Button == MouseButtons.Left && ClearRectangle.Contains(e.Location) && listDropDown != null)
			{
				ResetValue();
				return;
			}

			base.OnMouseClick(e);
		}

		internal override void ItemSelected(T item)
		{
			if (_selectedItems.Contains(item))
				_selectedItems.Remove(item);
			else
				_selectedItems.Add(item);

			OnSelectedItemChanged();
		}

		public override void ShowDropdown()
		{
			base.ShowDropdown();

			_searchBox.Width -= _searchBox.Height + Padding.Right;
			_searchLabel.Width -= _searchLabel.Height + Padding.Right;
		}

		protected abstract void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item, bool selected);
		protected abstract void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<T> items);

		protected sealed override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item)
		{
			PaintItem(e, rectangle, foreColor, hoverState, item, _selectedItems.Contains(item));
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

				SlickButton.DrawButton(e
					, ClearRectangle
					, string.Empty
					, Font
					, Properties.Resources.Tiny_Cancel
					, null
					, ClearRectangle.Contains(PointToClient(MousePosition)) ? (HoverState & ~HoverState.Focused) : HoverState.Normal);

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

				PaintSelectedItems(e, ClientRectangle.Pad(Padding).Pad(0, (int)(labelSize.Height * 1.2), 0, 0), fore, listDropDown != null ? HoverState.Pressed : HoverState, _selectedItems);

				using (var chevron = (UI.FontScale >= 1.25 ? Properties.Resources.I_DropChevron : Properties.Resources.I_DropChevron_16).Color(fore.MergeColor(back, 90)))
				{
					e.Graphics.DrawImage(chevron, ClientRectangle.Pad(Padding).Align(chevron.Size, ContentAlignment.MiddleRight));
				}
			}
		}
	}
}
