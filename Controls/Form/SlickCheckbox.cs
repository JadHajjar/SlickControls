using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("CheckChanged")]
	public partial class SlickCheckbox : SlickLabel, ISupportsReset
	{
		private bool @checked;

		private string checkedText;

		private string uncheckedText;
		private bool _useToggleIcon = true;

		public SlickCheckbox()
		{
			InitializeComponent();
		}

		public event EventHandler CheckChanged;

		[Category("Appearance"), DefaultValue(true), DisplayName("Use Toggle Icon")]
		public bool UseToggleIcon { get => _useToggleIcon; set { _useToggleIcon = value; Checked = Checked; } }

		[Category("Behavior")]
		public bool Checked
		{
			get => @checked;
			set
			{
				var chkChanged = @checked == !value;
				@checked = value;

				if (CheckedIcon != null && UnCheckedIcon != null)
					Image = @checked ? CheckedIcon : UnCheckedIcon;
				else if (UseToggleIcon)
					Image = @checked ? Properties.Resources.Tiny_ToggleOn : Properties.Resources.Tiny_ToggleOff;
				else
					Image = @checked ? Properties.Resources.Tiny_Checked : Properties.Resources.Tiny_Unchecked;

				if (!string.IsNullOrEmpty(CheckedText))
					Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);

				if (DefaultValue == null)
					DefaultValue = value;

				if (chkChanged)
					CheckChanged?.Invoke(this, new EventArgs());
			}
		}

		[Category("Appearance"), DisplayName("Checked Icon"), DefaultValue(null)]
		public Bitmap CheckedIcon { get; set; }

		[Category("Appearance"), DisplayName("Unchecked Icon"), DefaultValue(null)]
		public Bitmap UnCheckedIcon { get; set; }

		[Category("Behavior"), DefaultValue(null)]
		public bool? DefaultValue { get; set; } = null;

		[Category("Behavior"), DisplayName("Fade when Unchecked"), DefaultValue(false)]
		public bool FadeUnchecked { get; set; }

		[Category("Appearance"), DisplayName("Checked Text")]
		public string CheckedText
		{
			get => checkedText;
			set
			{
				checkedText = value;
				if (!string.IsNullOrEmpty(value))
					Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
			}
		}

		[Category("Appearance"), DisplayName("Unchecked Text")]
		public string UncheckedText
		{
			get => uncheckedText;
			set
			{
				uncheckedText = value;
				if (!string.IsNullOrEmpty(value))
					Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
			}
		}

		public void ResetValue()
			=> Checked = DefaultValue ?? true;

		private void SlickCheckbox_Click(object sender, EventArgs e) => Checked = !Checked;

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (FadeUnchecked && HoverState == HoverState.Normal)
			{
				if (Checked)
					using (var pen = new Pen(Color.FromArgb(175, ActiveColor == null ? FormDesign.Design.ActiveColor : ActiveColor()), 1.5F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
						e.Graphics.DrawRoundedRectangle(pen, new Rectangle(1, 1, Width - 3, Height - 3), 7);
				else
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(85, BackColor)), new Rectangle(Point.Empty, Size));
			}
		}
	}
}