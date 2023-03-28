using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SlickControls.Controls.Advanced
{
	internal class SlickDateRange : SlickControl
	{
        private readonly SlickDateTime _pickerFrom;
        private readonly SlickDateTime _pickerTo;

		public SlickDateRange()
		{
			_pickerFrom = new SlickDateTime
			{
				Parent = this,
				Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
			};

			_pickerTo = new SlickDateTime
			{
				Parent = this,
				Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
			};
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			if (Live)
			{
				Margin = Padding = UI.Scale(new Padding(5), UI.FontScale);
			}
		}
	}
}
