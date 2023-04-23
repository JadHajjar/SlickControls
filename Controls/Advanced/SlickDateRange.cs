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
	public class SlickDateRange : SlickControl, ISupportsReset
	{
		internal protected SlickForm _form;
		internal protected DateRangePopup listDropDown;
		private bool set;

		public event EventHandler RangeChanged;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		internal DateTime MaxDate { get; private set; } = new DateTime(3000, 1, 1);
		internal DateTime MinDate { get; private set; } = new DateTime(1900, 1, 1);
		internal DateTime Value { get; private set; } = DateTime.Today;
		public DateRangeType RangeType { get; internal set; }
		public bool Set => set;

		public SlickDateRange()
		{
			Cursor = Cursors.Hand;
		}

		public bool Match(DateTime date)
		{
			if (!set)
				return true;

			switch (RangeType)
			{
				case DateRangeType.After:
					return date >= Value;
				case DateRangeType.Before:
					return date <= Value;
				case DateRangeType.Between:
					break;
			}

			return false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				listDropDown?.Dispose();
			}

			base.Dispose(disposing);
		}

		public virtual void ShowDropdown()
		{
			if (listDropDown == null)
			{
				if (_form != null)
				{
					listDropDown = new DateRangePopup(this)
					{
						BackColor = FormDesign.Design.AccentBackColor,
						Padding = UI.Scale(new Padding(5), UI.FontScale),
						Location = _form.PointToClient(PointToScreen(new Point(0, Height - 3))),
						Font = Font,
						Cursor = Cursor,
						MaximumSize = new Size(Width, 9999),
						MinimumSize = new Size(Width, 0),
					};

					listDropDown.MouseClick += ListDropDown_MouseClick;
					listDropDown.Parent = _form;
					listDropDown.BringToFront();
					listDropDown.Disposed += ListDropDown_Leave;
					listDropDown.Focus();

					//new AnimationHandler(listDropDown, new Size(Width, Math.Min((listDropDown.ItemHeight + listDropDown.Padding.Vertical + (int)UI.FontScale) * Math.Min(10, Items.Length), _form.Height - listDropDown.Top - 15)), 2.5).StartAnimation();
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

		private void ListDropDown_Leave(object sender, EventArgs e)
		{
			CloseDropDown();
		}

		private void ListDropDown_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				CloseDropDown();
			}
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
				if (listDropDown.DateSet)
				{
					RangeType = listDropDown.RangeType;
					Value = listDropDown.Value;
					set = true;
					RangeChanged?.Invoke(this, EventArgs.Empty);
				}

				listDropDown.Dispose();

				listDropDown = null;

				Invalidate();
			}
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

			var textRect = ClientRectangle.Pad(Padding);

			if (listDropDown != null)
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);

				using (var brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(150, FormDesign.Design.ButtonColor), Color.Empty, 90))
				{
					e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);
				}

				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, 1, 2, -2), Padding.Left);
			}
			else
			{
				using (var brush = ClientRectangle.Gradient(back, 0.5F))
				{
					e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, listDropDown == null, listDropDown == null);
				}
			}

			e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), UI.Font(7.5F, FontStyle.Bold), new SolidBrush(fore), ClientRectangle.Pad(Padding), new StringFormat { LineAlignment = StringAlignment.Center });

			textRect = textRect.Pad((int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(7.5F, FontStyle.Bold)).Width, 0, 0, 0);

			using (var chevron = (UI.FontScale >= 1.5 ? Properties.Resources.I_DropChevron_24 : Properties.Resources.I_DropChevron_16).Color(fore.MergeColor(back, 90)))
			{
				e.Graphics.DrawImage(chevron, ClientRectangle.Pad(Padding).Align(chevron.Size, ContentAlignment.MiddleRight));

				textRect = textRect.Pad(0, 0, chevron.Width, 0);
			}

			string text;

			if (set)
			{
				text = LocaleHelper.GetGlobalText(RangeType.ToString()) + " ";

				if (Value == DateTime.Today)
					text += LocaleHelper.GetGlobalText("Today");
				else if (Value == DateTime.Today.AddDays(1))
					text += LocaleHelper.GetGlobalText("Tomorrow");
				else if (Value == DateTime.Today.AddDays(-1))
					text += LocaleHelper.GetGlobalText("Yesterday");
				else
					text += Value.ToReadableString(true, ExtensionClass.DateFormat.TDMY);
			}
			else
			{
				text = LocaleHelper.GetGlobalText("Any Date");
			}

			e.Graphics.DrawString(text, UI.Font(9F), new SolidBrush(fore), textRect.Pad(0,0,Padding.Right,0), new StringFormat { Alignment = StringAlignment.Far });
		}

		public void SetValue(DateRangeType range, DateTime value)
		{
			Value = value.Date;
			RangeType = range;
			set = true;
			RangeChanged?.Invoke(this, EventArgs.Empty);
			Invalidate();
		}

		public virtual void ResetValue()
		{
			Value = DateTime.Today;
			RangeType = DateRangeType.After;
			set = false;
			RangeChanged?.Invoke(this, EventArgs.Empty);
			Invalidate();
		}
	}
}
