using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace SlickControls;

public class SlickDateRange : SlickControl, ISupportsReset
{
	protected internal SlickForm _form;
	protected internal DateRangePopup listDropDown;

	public event EventHandler RangeChanged;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text
	{
		get => base.Text; set
		{
			base.Text = value;
			UIChanged();
		}
	}

	internal DateTime MaxDate { get; private set; } = new DateTime(3000, 1, 1);
	internal DateTime MinDate { get; private set; } = new DateTime(1900, 1, 1);
	internal DateTime Value { get; private set; } = DateTime.Today;
	public DateRangeType RangeType { get; internal set; }
	public bool Set { get; private set; }

	public SlickDateRange()
	{
		Cursor = Cursors.Hand;
	}

	public bool Match(DateTime date)
	{
		if (!Set)
		{
			return true;
		}

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
					Padding = UI.Scale(new Padding(5)),
					Location = _form.PointToClient(PointToScreen(new Point(0, Height - 3))),
					Font = UI.Font(7.5F),
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
		if (Live)
		{
			Font = UI.Font(8.25F);
			Padding = UI.Scale(new Padding(5));

			if (Dock == DockStyle.Fill)
			{
				return;
			}

			var size = UI.Scale(new Size(150, string.IsNullOrEmpty(Text) ? 26 : 32), UI.UIScale);

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
				Set = true;
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

		if (listDropDown != null)
		{
			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, false, false);

			using (var brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(75, FormDesign.Design.AccentColor), Color.Empty, 90))
			{
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 2), Padding.Left, true, true, false, false);
			}

			e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, 1, 2, -2), Padding.Left, true, true, false, false);
		}
		else
		{
			e.Graphics.FillRoundedRectangle(new SolidBrush(Focused ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), ClientRectangle.Pad(1, 1, 2, 1), 4);

			e.Graphics.FillRoundedRectangle(new SolidBrush(HoverState.HasFlag(HoverState.Hovered) ? back : FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 3), 4);
		}

		var labelSize = string.IsNullOrWhiteSpace(Text) ? Size.Empty : e.Graphics.Measure(Text, UI.Font(6.5F));

		e.Graphics.DrawString(Text, UI.Font(6.5F), new SolidBrush(Color.FromArgb(200, fore)), ClientRectangle.Pad(Padding.Left, Padding.Top / 4, 0, 0));

		PaintSelectedItem(e, fore, string.IsNullOrWhiteSpace(Text) ? ClientRectangle.Pad(Padding) : ClientRectangle.Pad(Padding).Pad(0, (int)(labelSize.Height * 0.65), 0, -Padding.Bottom / 2));

		using var chevron = IconManager.GetIcon("DropChevron", (ClientRectangle.Height - Padding.Vertical) / 2).Color(fore.MergeColor(back, 90));
		if (listDropDown != null)
		{
			chevron.Rotate(RotateFlipType.RotateNoneFlipY);
		}

		e.Graphics.DrawImage(chevron, ClientRectangle.Pad(Padding).Align(chevron.Size, ContentAlignment.MiddleRight));
	}

	private void PaintSelectedItem(PaintEventArgs e, Color fore, Rectangle rectangle)
	{
		string text;

		if (Set)
		{
			text = LocaleHelper.GetGlobalText(RangeType.ToString()) + " ";

			if (Value == DateTime.Today)
			{
				text += LocaleHelper.GetGlobalText("Today");
			}
			else if (Value == DateTime.Today.AddDays(1))
			{
				text += LocaleHelper.GetGlobalText("Tomorrow");
			}
			else if (Value == DateTime.Today.AddDays(-1))
			{
				text += LocaleHelper.GetGlobalText("Yesterday");
			}
			else
			{
				text += Value.ToReadableString(true, ExtensionClass.DateFormat.TDMY);
			}
		}
		else
		{
			text = LocaleHelper.GetGlobalText("Any Date");
		}

		e.Graphics.DrawString(text, Font, new SolidBrush(fore), rectangle.AlignToFontSize(Font, ContentAlignment.MiddleLeft));
	}

	public void SetValue(DateRangeType range, DateTime value)
	{
		Value = value.Date;
		RangeType = range;
		Set = true;
		RangeChanged?.Invoke(this, EventArgs.Empty);
		Invalidate();
	}

	public virtual void ResetValue()
	{
		Value = DateTime.Today;
		RangeType = DateRangeType.After;
		Set = false;
		RangeChanged?.Invoke(this, EventArgs.Empty);
		Invalidate();
	}
}
