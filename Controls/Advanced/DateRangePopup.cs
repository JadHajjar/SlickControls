using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public enum DateRangeType
{
	After,
	Before,
	Between
}
public partial class DateRangePopup : SlickControl
{
	private enum DateView { Years, Months, Days }

	private ExtensionClass.action action;
	private Point Mouse;
	private Dictionary<DateView, int> values;
	private DateView view = DateView.Days;
	private readonly SlickDateRange _dateBox;

	public DateRangeType RangeType { get; set; }
	public DateTime Value { get; internal set; }
	public bool DateSet { get; internal set; }

	public DateRangePopup(SlickDateRange dateBox)
	{
		RangeType = dateBox.RangeType;
		Height = UI.Scale(190);
		Value = dateBox.Value;
		_dateBox = dateBox;
		values = new Dictionary<DateView, int>
		{
			{ DateView.Years, dateBox.Value.Year },
			{ DateView.Months, dateBox.Value.Month },
			{ DateView.Days, dateBox.Value.Day },
		};
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			action?.Invoke();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		action = null;
		Mouse = PointToClient(Cursor.Position);

		var tabHeight = UI.Scale(28);
		var typeRects = new Rectangle[]
		{
			new(0, 0, Width / 3, tabHeight),
			new(Width / 3, 0, Width / 3, tabHeight),
			new(Width * 2 / 3, 0, Width / 3, tabHeight),
		};
		var texts = new string[]
		{
			LocaleHelper.GetGlobalText("After"),
			LocaleHelper.GetGlobalText("Before"),
			LocaleHelper.GetGlobalText("Clear"),
		};

		e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.AccentBackColor.Tint(Lum: !FormDesign.Design.IsDarkTheme ? 3 : -3)), new Rectangle(1, 0, Width - 3, tabHeight));

		for (var i = 0; i < typeRects.Length; i++)
		{
			var rangeType = (DateRangeType)i;
			var selected = RangeType == rangeType;
			var hovered = typeRects[i].Contains(PointToClient(MousePosition));
			var back = selected ? FormDesign.Design.ActiveColor : Color.FromArgb(hovered ? 255 : 150, FormDesign.Design.ButtonColor);
			var fore = selected ? FormDesign.Design.ActiveForeColor : Color.FromArgb(hovered ? 230 : 185, FormDesign.Design.ForeColor);

			if (hovered && !selected)
			{
				if (i == 2)
				{
					action = () =>
					{
						Dispose();
						_dateBox.ResetValue();
					};
				}
				else
				{
					action = () =>
					{
						RangeType = rangeType;
						Invalidate();
					};
				}
			}

			e.Graphics.FillRoundedRectangle(new SolidBrush(back), typeRects[i].Pad(Padding), UI.Scale(4));
			e.Graphics.DrawString(texts[i], new Font(Font, FontStyle.Bold), new SolidBrush(fore), typeRects[i].Pad(Padding), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		e.Graphics.DrawRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveColor), 1.5F), ClientRectangle.Pad(1, -2, 2, 2));

		paintButtons(e, tabHeight);

		var topRect = new Rectangle(3, 3 + tabHeight, Width - UI.Scale(100), (int)e.Graphics.Measure(" ", UI.Font(8.25F, FontStyle.Bold)).Height + 6);
		var mainRect = new Rectangle(3, 3 + tabHeight, Width - UI.Scale(100), Height - 10 - tabHeight).Pad(0, topRect.Height, 0, 0);

		using (var leftArrow = IconManager.GetSmallIcon("ArrowLeft"))
		using (var rightArrow = IconManager.GetSmallIcon("ArrowRight"))
		{
			var leftRect = topRect.Align(leftArrow.Size, ContentAlignment.MiddleLeft);
			var rightRect = topRect.Align(rightArrow.Size, ContentAlignment.MiddleRight);

			if (!canIncrement(-1))
			{
				e.Graphics.DrawImage(leftArrow.Color(FormDesign.Design.LabelColor).Alpha(100), leftRect);
			}
			else
			{
				e.Graphics.DrawImage(leftArrow.Color(leftRect.Contains(Mouse) ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor), leftRect);

				if (leftRect.Contains(Mouse))
				{
					action = () => increment(-1);
				}
			}

			if (!canIncrement(1))
			{
				e.Graphics.DrawImage(rightArrow.Color(FormDesign.Design.LabelColor).Alpha(100), rightRect);
			}
			else
			{
				e.Graphics.DrawImage(rightArrow.Color(rightRect.Contains(Mouse) ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor), rightRect);

				if (rightRect.Contains(Mouse))
				{
					action = () => increment(1);
				}
			}
		}

		if (topRect.Pad(24, 0, 24, 0).Contains(Mouse) && view != DateView.Years)
		{
			action = () =>
			{
				view = (DateView)((int)view - 1);
				Invalidate();
			};
			e.Graphics.DrawString(getViewTitle(), UI.Font(8.25F, FontStyle.Bold), SlickControl.Gradient(topRect, FormDesign.Design.ActiveColor), topRect.Pad(24, 0, 24, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
		else
		{
			e.Graphics.DrawString(getViewTitle(), UI.Font(8.25F, FontStyle.Bold), SlickControl.Gradient(topRect, FormDesign.Design.LabelColor), topRect.Pad(24, 0, 24, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		drawView(e, mainRect);

		e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), mainRect.Width + 5, 5 + tabHeight, mainRect.Width + 5, Height - 5);

		Cursor = action == null ? Cursors.Default : Cursors.Hand;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		switch (keyData)
		{
			case Keys.Left:
				increment(-1);
				return true;

			case Keys.Right:
				increment(1);
				return true;

			case Keys.Escape:
				Dispose();
				return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private bool canIncrement(int v)
	{
		var val = new DateTime(values[DateView.Years], values[DateView.Months], values[DateView.Days]);

		switch (view)
		{
			case DateView.Years:
				val = val.AddYears(12 * v);
				return val.Year - (val.Year % 12) <= _dateBox.MaxDate.Year && val.Year - (val.Year % 12) >= _dateBox.MinDate.Year - 11;

			case DateView.Months:
				val = val.AddYears(v);
				return val.Year == val.Year.Between(_dateBox.MinDate.Year, _dateBox.MaxDate.Year);

			case DateView.Days:
				val = val.AddMonths(v);
				return new DateTime(val.Year, val.Month, DateTime.DaysInMonth(val.Year, val.Month)) >= _dateBox.MinDate && new DateTime(val.Year, val.Month, 1) <= _dateBox.MaxDate;
		}

		return false;
	}

	private void drawView(PaintEventArgs e, Rectangle mainRect)
	{
		var w = mainRect.Width;
		var h = mainRect.Height;
		var y = mainRect.Y;
		var rect = new Rectangle(mainRect.X, y, w, h);

		switch (view)
		{
			case DateView.Years:
				w /= 4;
				h /= 3;
				rect = new Rectangle(mainRect.X, y, w, h);

				var year = values[DateView.Years] - (values[DateView.Years] % 12);
				for (var i = 1; i <= 12; i++, year++)
				{
					if (year == year.Between(_dateBox.MinDate.Year, _dateBox.MaxDate.Year))
					{
						if (year == _dateBox.Value.Year)
						{
							e.Graphics.FillRectangle(SlickControl.Gradient(rect, Color.FromArgb(50, FormDesign.Design.ActiveColor)), rect.Pad(1, 0, 0, 1));
						}

						e.Graphics.DrawString(year.ToString(), Font, SlickControl.Gradient(rect, FormDesign.Design.ForeColor), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

						if (year == DateTime.Today.Year || rect.Contains(Mouse))
						{
							using var pen = new Pen(FormDesign.Design.ActiveColor, 1F);
							if (rect.Contains(Mouse))
							{
								pen.DashPattern = new[] { 2F, 2F };
								var ye = year.Between(1900, 3000);
								action = () =>
								{
									values[DateView.Years] = ye;
									view = DateView.Months;
									Invalidate();
								};
							}

							e.Graphics.DrawRectangle(pen, rect.Pad(1, 0, 0, 1));
						}
					}

					if (i % 4 == 0)
					{
						rect = new Rectangle(mainRect.X, rect.Y + h, w, h);
					}
					else
					{
						rect.X += rect.Width;
					}
				}

				break;

			case DateView.Months:
				w /= 4;
				h /= 3;
				rect = new Rectangle(mainRect.X, y, w, h);
				for (var i = 1; i <= 12; i++)
				{
					if (new DateTime(values[DateView.Years], i, DateTime.DaysInMonth(values[DateView.Years], i)) >= _dateBox.MinDate && new DateTime(values[DateView.Years], i, 1) <= _dateBox.MaxDate)
					{
						var month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMM", LocaleHelper.CurrentCulture);

						if (i == _dateBox.Value.Month && values[DateView.Years] == _dateBox.Value.Year)
						{
							e.Graphics.FillRectangle(SlickControl.Gradient(rect, Color.FromArgb(50, FormDesign.Design.ActiveColor)), rect.Pad(1, 0, 0, 1));
						}

						e.Graphics.DrawString(month, Font, SlickControl.Gradient(rect, FormDesign.Design.ForeColor), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

						if ((i == DateTime.Today.Month && values[DateView.Years] == DateTime.Now.Year) || rect.Contains(Mouse))
						{
							using var pen = new Pen(FormDesign.Design.ActiveColor, 1F);
							if (rect.Contains(Mouse))
							{
								pen.DashPattern = new[] { 2F, 2F };
								var m = i;
								action = () =>
								{
									values[DateView.Months] = m;
									view = DateView.Days;
									Invalidate();
								};
							}

							e.Graphics.DrawRectangle(pen, rect.Pad(1, 0, 0, 1));
						}
					}

					if (i % 4 == 0)
					{
						rect = new Rectangle(mainRect.X, rect.Y + h, w, h);
					}
					else
					{
						rect.X += rect.Width;
					}
				}

				break;

			case DateView.Days:
				w /= 7;
				h /= 7;
				rect = new Rectangle(mainRect.X, y, w, h);

				var days = getDaysIndex();

				for (var i = 0; i < 7; i++)
				{
					e.Graphics.DrawString(days.ElementAt(i).Key.ToString("ddd", LocaleHelper.CurrentCulture).ToCapital(false), Font, new SolidBrush(FormDesign.Design.InfoColor), rect.AlignToFontSize(Font), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
					rect.X += rect.Width;
				}

				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), 5, y + h, mainRect.Width - 5, y + h);

				rect = new Rectangle(mainRect.X, y + h + 3, w, h);

				foreach (var item in days)
				{
					if (item.Key >= _dateBox.MinDate && item.Key <= _dateBox.MaxDate)
					{
						if (item.Key == _dateBox.Value)
						{
							e.Graphics.FillRectangle(SlickControl.Gradient(rect, Color.FromArgb(50, FormDesign.Design.ActiveColor)), rect.Pad(1, 0, 0, 1));
						}

						e.Graphics.DrawString(item.Key.Day.ToString(), Font, SlickControl.Gradient(rect, item.Key.Month == values[DateView.Months] ? FormDesign.Design.ForeColor : FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor)), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

						if (item.Key == DateTime.Today || rect.Contains(Mouse))
						{
							using var pen = new Pen(FormDesign.Design.ActiveColor, 1F);
							if (rect.Contains(Mouse))
							{
								pen.DashPattern = new[] { 2F, 2F };
								action = () => setDate(item.Key);
							}

							e.Graphics.DrawRectangle(pen, rect.Pad(1, 0, 0, 1));
						}
					}

					if (item.Value == 0)
					{
						rect = new Rectangle(mainRect.X, rect.Y + h, w, h);
					}
					else
					{
						rect.X += rect.Width;
					}
				}

				break;
		}
	}

	private Dictionary<DateTime, int> getDaysIndex()
	{
		var dic = new Dictionary<DateTime, int>();

		var current = new DateTime(values[DateView.Years], values[DateView.Months], 1);
		while (current.DayOfWeek != DayOfWeek.Monday)
		{
			current = current.AddDays(-1);
		}

		for (var i = 0; i < 7 * 6; i++)
		{
			dic.Add(current, (int)current.DayOfWeek);
			current = current.AddDays(1);
		}

		return dic;
	}

	private string getViewTitle()
	{
		return view switch
		{
			DateView.Years => $"{(values[DateView.Years] - (values[DateView.Years] % 12)).Between(_dateBox.MinDate.Year, _dateBox.MaxDate.Year)} - {(values[DateView.Years] - (values[DateView.Years] % 12) + 11).Between(_dateBox.MinDate.Year, _dateBox.MaxDate.Year)}",
			DateView.Months => values[DateView.Years].ToString(),
			DateView.Days => new DateTime(values[DateView.Years], values[DateView.Months], 1).ToString("MMMM yyyy", LocaleHelper.CurrentCulture).ToCapital(false),
			_ => string.Empty,
		};
	}

	private void increment(int v)
	{
		if (canIncrement(v))
		{
			var val = new DateTime(values[DateView.Years], values[DateView.Months], values[DateView.Days]);
			switch (view)
			{
				case DateView.Years:
				case DateView.Months:
					val = val.AddYears(view == DateView.Years ? 12 * v : v);
					break;
				case DateView.Days:
					val = val.AddMonths(v);
					break;
			}

			values = new Dictionary<DateView, int>
			{
				{ DateView.Years, val.Year },
				{ DateView.Months, val.Month },
				{ DateView.Days, val.Day },
			};

			Invalidate();
		}
	}

	private void paintButtons(PaintEventArgs e, int tabHeight)
	{
		var rect = new Rectangle(Width - UI.Scale(100) + 5, tabHeight, UI.Scale(100) - 5, (Height - tabHeight) / 6);

		drawButton("Today", DateTime.Today);
		drawButton("Yesterday", DateTime.Today.AddDays(-1));
		drawButton("Start of Month", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
		drawButton("End of Month", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1));
		drawButton("Start of Year", new DateTime(DateTime.Now.Year, 1, 1));
		drawButton("End of Year", new DateTime(DateTime.Now.Year, 12, 31));

		void drawButton(string text, DateTime val)
		{
			var valid = val >= _dateBox.MinDate && val <= _dateBox.MaxDate;
			var hovered = valid && rect.Contains(Mouse);

			if (hovered)
			{
				e.Graphics.FillRectangle(SlickControl.Gradient(rect, FormDesign.Design.ActiveColor), rect);
				action = () => setDate(val);
			}

			e.Graphics.DrawString(hovered ? val.ToString("d", LocaleHelper.CurrentCulture) : (string)LocaleHelper.GetGlobalText(text), Font, SlickControl.Gradient(rect, hovered ? FormDesign.Design.ActiveForeColor : valid ? FormDesign.Design.ForeColor : FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor)), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

			if (rect.Y != tabHeight)
			{
				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), rect.X + 5, rect.Y, rect.X + rect.Width - 10, rect.Y);
			}

			rect.Y += rect.Height;
		}
	}

	private void setDate(DateTime val)
	{
		DateSet = true;
		Value = val;
		Dispose();
	}
}
