using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("CheckChanged")]
	public partial class SlickCheckbox : SlickControl, ISupportsReset
	{
		private bool @checked;

		private string checkedText;

		private string uncheckedText;
		private bool _useToggleIcon;

		public SlickCheckbox()
		{
			Cursor = Cursors.Hand;
			SpaceTriggersClick = true;
			EnterTriggersClick = false;
		}

		public event EventHandler CheckChanged;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		[Category("Appearance"), DefaultValue(false), DisplayName("Use Toggle Icon")]
		public bool UseToggleIcon { get => _useToggleIcon; set { _useToggleIcon = value; Checked = Checked; } }

		[Category("Behavior")]
		public bool Checked
		{
			get => @checked;
			set
			{
				var chkChanged = @checked == !value;
				@checked = value;

				if (!string.IsNullOrEmpty(CheckedText))
				{
					Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
				}

				if (DefaultValue == null)
				{
					DefaultValue = value;
				}

				if (chkChanged)
				{
					CheckChanged?.Invoke(this, new EventArgs());
				}
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
				{
					Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
				}
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
				{
					Text = @checked ? CheckedText : UncheckedText.IfEmpty(CheckedText);
				}
			}
		}

		public void ResetValue()
		{
			Checked = DefaultValue ?? true;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
			{
				Checked = !Checked;
			}
		}

		protected override void LocaleChanged()
		{
			if (Live)
			{
				Invalidate();
			}
		}

		protected override void UIChanged()
		{
			if (Live)
			{
				Padding = UI.Scale(new Padding(5), UI.FontScale);
			}
		}

		public override Size GetPreferredSize(Size proposedSize) => GetAutoSize();

		private Size GetAutoSize()
		{
			using (var image = GetIcon())
			using (var g = Graphics.FromHwnd(IntPtr.Zero))
			{
				var iconSize = image?.Width ?? 16;

				if (string.IsNullOrWhiteSpace(Text))
				{
					var pad = Math.Max(Padding.Horizontal, Padding.Vertical);

					return new Size(iconSize + pad + 1, iconSize + pad + 1);
				}

				var bnds = g.Measure(LocaleHelper.GetGlobalText(Text), Font);
				var h = Math.Max(iconSize + 6, (int)bnds.Height + Padding.Top + 3);
				var w = (int)bnds.Width + (image == null ? 0 : iconSize + Padding.Left) + Padding.Horizontal + 3;

				if (Anchor.HasFlag(AnchorStyles.Top | AnchorStyles.Bottom) || Dock == DockStyle.Left || Dock == DockStyle.Right)
				{
					h = Height;
				}

				if (Anchor.HasFlag(AnchorStyles.Left | AnchorStyles.Right) || Dock == DockStyle.Top || Dock == DockStyle.Bottom)
				{
					w = Width;
				}

				return new Size(w, h);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SetUp(BackColor);

			try
			{
				GetColors(out var fore, out var back);

				var image = GetIcon();
				var bnds = e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), Font);
				var iconSize = image.Width;
				var corner = (int)(5 * UI.FontScale);
				var iconRect = ClientRectangle.Pad(Padding).Pad(1, 1, 2, 2).Align(new Size(iconSize, iconSize), ContentAlignment.MiddleLeft);
				var textRect = ClientRectangle.Pad(iconSize + Padding.Horizontal + 1, 1, 2, 2).Align(bnds.ToSize(), ContentAlignment.MiddleLeft);

				textRect.Width += Padding.Left;

				if (back != Color.Empty)
				{
					e.Graphics.FillRoundedRectangle(Gradient(ClientRectangle, back), ClientRectangle.Pad(1, 1, 2, 2), corner);
				}

				if (!HoverState.HasFlag(HoverState.Pressed))
				{
					DrawFocus(e.Graphics, ClientRectangle.Pad(1, 1, 2, 2), HoverState, corner);
				}

				using (image)
				{
					if (Loading)
					{
						if (string.IsNullOrWhiteSpace(Text))
						{
							DrawLoader(e.Graphics, iconRect, HoverState.HasFlag(HoverState.Pressed) ? (Color?)fore : null);
						}
						else
						{
							DrawLoader(e.Graphics, iconRect, HoverState.HasFlag(HoverState.Pressed) ? (Color?)fore : null);
						}
					}
					else if (image != null)
					{
						if (!HoverState.HasFlag(HoverState.Pressed) && @checked && !UseToggleIcon && CheckedIcon == null && UnCheckedIcon == null)
						{
							e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.ActiveForeColor), iconRect.CenterR(iconSize * 2 / 3, iconSize * 2 / 3));
							image.Color(FormDesign.Design.ActiveColor);
						}
						else
						{
							image.Color(fore);
						}

						if (string.IsNullOrWhiteSpace(Text))
						{
							e.Graphics.DrawImage(image, iconRect);
						}
						else
						{
							e.Graphics.DrawImage(image, iconRect);
						}
					}
				}

				var stl = new StringFormat()
				{
					Alignment = StringAlignment.Near,
					LineAlignment = StringAlignment.Center,
					Trimming = StringTrimming.EllipsisCharacter
				};

				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), Font, new SolidBrush(fore), textRect, stl);

				if (FadeUnchecked && HoverState == HoverState.Normal)
				{
					if (Checked)
					{
						using (var pen = new Pen(Color.FromArgb(175, FormDesign.Design.ActiveColor), 1.5F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
						{
							e.Graphics.DrawRoundedRectangle(pen, new Rectangle(1, 1, Width - 3, Height - 3), 7);
						}
					}
					else
					{
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(85, BackColor)), new Rectangle(Point.Empty, Size));
					}
				}
			}
			catch { }
		}

		private Bitmap GetIcon()
		{
			Bitmap image;

			if (CheckedIcon != null && UnCheckedIcon != null)
			{
				image = @checked ? CheckedIcon : UnCheckedIcon;
			}
			else if (UseToggleIcon)
			{
				image = @checked ? Properties.Resources.Tiny_ToggleOn : Properties.Resources.Tiny_ToggleOff;
			}
			else if (UI.FontScale < 1.25F)
			{
				image = @checked ? Properties.Resources.Tiny_CheckedFilled : Properties.Resources.Tiny_Unchecked;
			}
			else
			{
				image = @checked ? Properties.Resources.I_Checked : Properties.Resources.I_Unchecked;
			}

			return image;
		}

		protected virtual void GetColors(out Color fore, out Color back)
		{
			if (HoverState.HasFlag(HoverState.Pressed))
			{
				fore = FormDesign.Design.ActiveForeColor;
				back = FormDesign.Design.ActiveColor;
			}
			else if (HoverState.HasFlag(HoverState.Hovered))
			{
				fore = FormDesign.Design.ForeColor.Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, -7, 7));
				back = FormDesign.Design.ButtonColor.MergeColor(BackColor, 75);
			}
			else
			{
				fore = Enabled ? ForeColor : ForeColor.MergeColor(BackColor);
				back = Color.Empty;
			}
		}
	}
}