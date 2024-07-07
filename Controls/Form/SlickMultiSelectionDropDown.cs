using Extensions;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SlickControls;

public abstract class SlickMultiSelectionDropDown<T> : SlickSelectionDropDown<T>
{
	private Rectangle ClearRectangle => new(_searchBox.Right + Padding.Horizontal + _searchBox.Height, _searchBox.Top - UI.Scale(3), _searchBox.Height + UI.Scale(6), _searchBox.Height + UI.Scale(6));
	private readonly List<T> _selectedItems = [];

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
	public IEnumerable<T> SelectedItems
	{
		get => new List<T>(_selectedItems);
		set
		{
			_selectedItems.Clear();

			if (value != null)
			{
				_selectedItems.AddRange(value);
			}

			Invalidate();
		}
	}

	public override void ResetValue()
	{
		_selectedItems.Clear();
		listDropDown?.Invalidate();
		OnSelectedItemChanged();
		Invalidate();
	}

	public void Select(T obj)
	{
		if (_selectedItems.Contains(obj))
		{
			_selectedItems.Remove(obj);
		}
		else
		{
			_selectedItems.Add(obj);
		}

		listDropDown?.Invalidate();
		OnSelectedItemChanged();
		Invalidate();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
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
		{
			_selectedItems.Remove(item);
		}
		else
		{
			_selectedItems.Add(item);
		}

		OnSelectedItemChanged();

		_searchBox.Focus();
		_searchBox.SelectAll();
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
		var selected = _selectedItems.Contains(item);

		if (selected && !hoverState.HasFlag(HoverState.Pressed))
		{
			var bar = UI.Scale(4);
			using (var brush = new LinearGradientBrush(e.ClipRectangle.Pad(e.ClipRectangle.Width / 4, 0, 0, 0), Color.Empty, Color.FromArgb(50, FormDesign.Design.ActiveColor), LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.Pad(0, -Padding.Top, 0, -Padding.Bottom).Pad((e.ClipRectangle.Width / 4) + 1, 1, bar, 1), bar);
			}

			using (var brush = new SolidBrush(FormDesign.Design.ActiveColor))
			{
				e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.ActiveColor), new Rectangle(e.ClipRectangle.Right - (3 * bar / 2) - 1, e.ClipRectangle.Y + 1, bar * 3 / 2, e.ClipRectangle.Height - 2).Pad(0, -Padding.Top, 0, -Padding.Bottom));
			}

			foreColor = FormDesign.Design.ActiveColor;
		}

		PaintItem(e, rectangle, foreColor, hoverState, item, selected);
	}

	protected override void PaintForListOpen(PaintEventArgs e, Color fore)
	{
		base.PaintForListOpen(e, fore);

		using var image = IconManager.GetIcon("Cancel", _searchBox.Height);
		var hoverState = ClearRectangle.Contains(PointToClient(MousePosition)) ? (HoverState & ~HoverState.Focused) : HoverState.Normal;

		var color =
			hoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor :
			hoverState.HasFlag(HoverState.Hovered) ? Color.FromArgb(100, FormDesign.Design.ActiveColor) :
			ForeColor;

		if (hoverState.HasFlag(HoverState.Hovered))
		{
			using var brush = new SolidBrush(hoverState.HasFlag(HoverState.Pressed) ? Color.FromArgb(25, FormDesign.Design.ActiveColor) : Color.FromArgb(75, FormDesign.Design.AccentColor));
			e.Graphics.FillRoundedRectangle(brush, ClearRectangle, UI.Scale(4));
		}

		e.Graphics.DrawImage(image.Color(color), ClearRectangle.CenterR(image.Size));
	}

	protected override void PaintSelectedItem(PaintEventArgs e, Color fore, Rectangle rectangle)
	{
		PaintSelectedItems(e, rectangle, fore, HoverState, _selectedItems);
	}
}
