using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
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
			DynamicIcon di;

			switch (item)
			{
				default:
				case IoSortingOption.Name: di = "I_FileName";
					break;
				case IoSortingOption.DateModified: di = "I_SortEdit";
					break;
				case IoSortingOption.DateCreated: di = "I_SortCreated";
					break;
				case IoSortingOption.Size: di = "I_MicroSd";
					break;
			}

			using (var icon = di.Get(rectangle.Height - 2).Color(foreColor))
			{
				e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

				e.Graphics.DrawString(LocaleHelper.GetGlobalText(item.ToString().FormatWords()), Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
			}
		}
	}
}
