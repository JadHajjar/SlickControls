using Extensions;

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

internal enum IoSortingOption
{
	Name,
	DateModified,
	DateCreated,
	Size
}

internal class IoSortDropdown : SlickSelectionDropDown<IoSortingOption>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(IoSortingOption)).Cast<IoSortingOption>().ToArray();
		}
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IoSortingOption item)
	{
		var di = item switch
		{
			IoSortingOption.DateModified => (DynamicIcon)"SortEdit",
			IoSortingOption.DateCreated => (DynamicIcon)"SortCreated",
			IoSortingOption.Size => (DynamicIcon)"MicroSd",
			_ => (DynamicIcon)"FileName",
		};
		using var icon = di.Get(rectangle.Height - 2).Color(foreColor);
		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(LocaleHelper.GetGlobalText(item.ToString().FormatWords()), Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}
}
