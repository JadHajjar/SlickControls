using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SlickControls
{
	[DefaultEvent("CheckChanged")]
	public partial class SlickRadioButton : SlickLabel
	{
		public SlickRadioButton()
		{
			InitializeComponent();
			Click += (s, e) => Checked = !Checked;
			Cursor = System.Windows.Forms.Cursors.Hand;
		}

		public event EventHandler CheckChanged;

		private bool @checked;
		private readonly DisableIdentifier checkIdentifier = new DisableIdentifier();

		[Browsable(false)]
		public object Data { get; set; }

		[Category("Appearance")]
		public bool Checked
		{
			get => @checked;
			set
			{
				if (@checked != value && !checkIdentifier.Disabled)
				{
					if (!value && @checked && (RadioGroup?.All(x => x == this || !x.Checked) ?? false))
						return;

					@checked = value;
					checkIdentifier.Disable();
					CheckChanged?.Invoke(this, new EventArgs());
					checkIdentifier.Enable();
				}

				if (checkIdentifier.Disabled)
					return;

				if (@checked && RadioGroup != null)
				{
					foreach (var item in RadioGroup.Where(x => x != this && x.Checked))
						item.Checked = false;
				}

				ImageName = @checked ? "I_Circle_ON" : "I_Circle_OFF";
			}
		}

		public IEnumerable<SlickRadioButton> RadioGroup
			=> CustomGroup ?? Parent?.Controls.ThatAre<SlickRadioButton>();

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable<SlickRadioButton> CustomGroup { get; set; }
	}

	public static class SlickRadiobuttonExtensions
	{
		public static bool GroupChecked(this IEnumerable<SlickRadioButton> group)
			=> group.Any(x => x.Checked);

		public static object GetSelectedData(this IEnumerable<SlickRadioButton> group)
			=> group.FirstOrDefault(x => x.Checked)?.Data;
	}
}