using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SlickDateTime : SlickControl, IValidationControl, ISupportsReset
{
	public event EventHandler ValueChanged;

	private DateTime _value = DateTime.Now;

	private DateType dateType;

	private readonly Dictionary<DatePart, Rectangle> partRects = [];

	private Dictionary<DatePart, string> partValues;

	private DatePart selectedPart;

	private Rectangle iconRect;

	private DateTime minimumValue = new(1900, 1, 1);

	private DateTime maximumValue = new(3000, 1, 1);
	private DateTime _defaultValue;

	public SlickDateTime()
	{
		AutoScaleMode = AutoScaleMode.None;
		Size = new Size(100, 25);
		DateType = DateType.Date;
		DoubleBuffered = ResizeRedraw = true;
	}

	public enum DatePart { Day, Month, Year, Hour, Minute, Second }

	[Category("Date"), DisplayName("Date Type"), DefaultValue(DateType.Date)]
	public DateType DateType
	{
		get => dateType;
		set
		{
			dateType = value;

			if (value == DateType.Date)
			{
				_value = _value.Date;
			}

			partValues = Enum.GetValues(typeof(DatePart))
				.Cast<DatePart>()
				.Skip(DateType == DateType.Time ? 3 : 0)
				.Take(DateType == DateType.Date ? 3 : 5)
				.ToDictionary(x => x, x => val(Value, x).ToString("00"));

			Invalidate();
		}
	}

	[Category("Behavior")]
	public DatePart SelectedPart
	{
		get => ReadOnly ? (DatePart)(-1) : selectedPart;
		set
		{
			if (value != selectedPart)
			{
				handlePartLeave();
				selectedPart = value;
			}
		}
	}

	[Category("Date")]
	public DateTime Value
	{
		get => _value;
		set
		{
			if (ReadOnly)
			{
				return;
			}

			var prev = _value;

			if (value < MinDate)
			{
				_value = MinDate;
			}
			else if (value > MaxDate)
			{
				_value = MaxDate;
			}
			else
			{
				_value = value;
			}

			DateType = DateType;

			if (prev != _value)
			{
				ValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public override string Text { get => Value.ToString(); set => Value = DateTime.TryParse(value, out var newVal) ? newVal : _value; }

	[Category("Date"), DisplayName("Minimum Date"), DefaultValue(typeof(DateTime), "1900-01-01")]
	public DateTime MinDate
	{
		get => minimumValue; set
		{
			minimumValue = value.Year < 1900 ? new DateTime(1900, 1, 1) : value;
			Value = Value;
		}
	}

	[Category("Date"), DisplayName("Maximum Date"), DefaultValue(typeof(DateTime), "3000-01-01")]
	public DateTime MaxDate
	{
		get => maximumValue; set
		{
			maximumValue = value.Year > 3000 ? new DateTime(3000, 1, 1) : value;
			Value = Value;
		}
	}

	[Category("Appearance"), DisplayName("Label Text"), DefaultValue("Date")]
	public string LabelText { get; set; } = "Date";

	[Category("Behavior"), DisplayName("Show Text"), DefaultValue(true)]
	public bool ShowLabel { get; set; } = true;

	[Category("Behavior"), DisplayName("Read Only"), DefaultValue(false)]
	public bool ReadOnly { get; set; }

	public bool ValidInput => true;

	public bool Required { get; set; }

	protected override void UIChanged()
	{
		using var g = Graphics.FromHwnd(IntPtr.Zero);
		var showLabel = !string.IsNullOrEmpty(LabelText);
		var iconWidth = DateType != DateType.Time ? (UI.FontScale >= 1.25 ? 24 : 16) : 0;
		Padding = new Padding(4, showLabel ? (int)(g.Measure(nameof(SlickTextBox), UI.Font(6.75F, FontStyle.Bold)).Height + 4) : 4, iconWidth > 0 ? (iconWidth + 8) : 4, 4);
		Font = UI.Font(8.25F * (float)UI.WindowsScale);
		var height = Font.Height + Padding.Vertical;

		MaximumSize = Size.Empty;
		if (Live)
		{
			MinimumSize = new Size(Padding.Horizontal, height);
		}
		else
		{
			MinimumSize = Size.Empty;
		}

		Height = height;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		_defaultValue = _value;
	}

	protected override void OnEnter(EventArgs e)
	{
		SelectedPart = partValues.Keys.First();

		base.OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		Value = getValue();

		base.OnLeave(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (!ReadOnly && e.Button == MouseButtons.Left)
		{
			if (iconRect.Contains(e.Location))
			{
				Value = getValue();
				var pnt = PointToScreen(Point.Empty);
				var prompt = new DateTimePickerPrompt(this);

				prompt.Location = new Point(
					pnt.X + Width - prompt.Width > 0
					? pnt.X + Width - prompt.Width
					: pnt.X,
					pnt.Y + Height + prompt.Height > Screen.PrimaryScreen.WorkingArea.Height
					? pnt.Y - prompt.Height + 1
					: pnt.Y + Height - 1);

				prompt.Show(FindForm());
			}
			else if (partRects.Any(x => x.Value.Contains(e.Location)))
			{
				SelectedPart = partRects.First(x => x.Value.Contains(e.Location)).Key;
			}
		}

		base.OnMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		Cursor = iconRect.Contains(e.Location) || partRects.Any(x => x.Value.Contains(e.Location))
			? Cursors.Hand
			: Cursors.Default;

		Invalidate(iconRect);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.Clear(BackColor);

		e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
		e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

		var error = false;
		var iconWidth = DateType != DateType.Time ? (UI.FontScale >= 1.25 ? 24 : 16) : 0;
		iconRect = new Rectangle(Width - (iconWidth * 11 / 8), 0, iconWidth * 11 / 8, Height - 2);
		var barColor =
			error ? FormDesign.Design.RedColor :
			Focused ? FormDesign.Design.ActiveColor :
			FormDesign.Design.AccentColor;

		e.Graphics.FillRoundedRectangle(new SolidBrush(barColor), ClientRectangle.Pad(1, 1, 2, 1), 4);

		e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 3), 4);

		if (ShowLabel && !string.IsNullOrEmpty(LabelText))
		{
			var font = UI.Font(6.75F, FontStyle.Bold);
			e.Graphics.DrawString(LocaleHelper.GetGlobalText(LabelText), font, new SolidBrush(FormDesign.Design.LabelColor), new Rectangle(2, 2, Width - Padding.Right, (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(LabelText), font).Height), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
		}

		if (DateType != DateType.Time)
		{
			using var Image = IconManager.GetIcon("I_Calendar");
			var active = iconRect.Contains(PointToClient(Cursor.Position));

			if (active)
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(20, FormDesign.Design.ForeColor)), iconRect, 4);
			}

			e.Graphics.DrawImage(Image.Color(active ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), iconRect.CenterR(Image.Size));
		}

		var charWidth = (int)e.Graphics.Measure("0", Font).Width;
		var left = 1;

		foreach (var part in partValues)
		{
			var rect = new Rectangle(left, Height - Font.Height - 2, (part.Key == DatePart.Year ? 4 : 2) * charWidth, Font.Height - 1);
			var delimiter = string.Empty;

			switch (part.Key)
			{
				case DatePart.Month:
				case DatePart.Year:
					delimiter = "/";
					left -= 2;
					break;

				case DatePart.Hour:
					if (DateType == DateType.DateTime)
					{
						delimiter = "at";
					}

					break;

				case DatePart.Minute:
				case DatePart.Second:
					delimiter = ":";
					break;
			}

			if (!string.IsNullOrEmpty(delimiter))
			{
				var size = e.Graphics.Measure(delimiter, Font);
				if (part.Key > DatePart.Hour)
				{
					size.Width = 0;
				}

				e.Graphics.DrawString(delimiter, Font, Gradient(Enabled ? FormDesign.Design.ForeColor : Color.Gray), new Rectangle(left - (part.Key == DatePart.Hour ? 5 : 1), rect.Y, (int)Math.Ceiling(size.Width), rect.Height), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

				left = rect.X += ((int)Math.Ceiling(size.Width) / 2) - (part.Key == DatePart.Year ? 5 : 2) + (delimiter == "at" ? 3 : 0);
			}

			var fade = HoverState.HasFlag(HoverState.Focused) & shouldFade(out var fadedValue);
			error |= part.Key != SelectedPart && fade;

			e.Graphics.DrawString(fade ? fadedValue : part.Value, Font, Gradient(Color.FromArgb(fade ? 125 : 255, Enabled ? FormDesign.Design.ForeColor : Color.Gray)), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

			if (SelectedPart == part.Key && HoverState.HasFlag(HoverState.Focused))
			{
				var textsize = e.Graphics.Measure(string.IsNullOrEmpty(part.Value) ? ((char)0x200b).ToString() : fade ? fadedValue : part.Value, Font).ToSize();
				var highlightrect = new Rectangle(rect.Center(textsize), textsize).Pad(fade && part.Key == selectedPart ? textsize.Width - (int)e.Graphics.Measure(part.Value, Font).Width + 2 : 2, 1, 0, 1);

				e.Graphics.FillRectangle(Gradient(FormDesign.Design.ActiveColor), highlightrect);
				e.Graphics.SetClip(highlightrect);
				e.Graphics.DrawString(fade ? fadedValue : part.Value, Font, Gradient(FormDesign.Design.ActiveForeColor), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
				e.Graphics.ResetClip();
			}

			partRects.TryAdd(part.Key, rect);
			left += rect.Width;

			bool shouldFade(out string _val)
			{
				if (part.Key != SelectedPart)
				{
					try
					{
						var currentVal = getValue();

						if (currentVal < MinDate)
						{
							_val = val(MinDate, part.Key).ToString(part.Key == DatePart.Year ? "0000" : "00");
							return true;
						}

						if (currentVal > MaxDate)
						{
							_val = val(MaxDate, part.Key).ToString(part.Key == DatePart.Year ? "0000" : "00");
							return true;
						}
					}
					catch { }

					if (part.Key == DatePart.Day)
					{
						var daysInMonth = DateTime.DaysInMonth(partValues.TryGet(DatePart.Year).SmartParse().Between(1900, 3000),
							partValues.TryGet(DatePart.Month).SmartParse().Between(1, 12));

						if (SelectedPart != DatePart.Day && part.Key == DatePart.Day && daysInMonth < part.Value.SmartParse())
						{
							_val = daysInMonth.ToString();
							return true;
						}
					}
				}
				else if (part.Key == DatePart.Year && part.Value.Length.IsWithin(0, 4))
				{
					_val = "2000".TrimEnd(part.Value.Length) + part.Value;
					return true;
				}

				_val = string.Empty;
				return false;
			}
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (ReadOnly)
		{
			return base.ProcessCmdKey(ref msg, keyData);
		}

		switch (keyData)
		{
			case Keys.Escape:
				partValues[SelectedPart] = string.Empty;
				Invalidate();
				return true;

			case Keys.Left:
				SelectedPart = SelectedPart == partValues.Keys.First() ? partValues.Keys.Last() : partValues.Keys.Previous(SelectedPart);
				Invalidate();
				return true;

			case Keys.Right:
			case Keys.Divide:
			case Keys.Subtract:
			case Keys.OemMinus:
			case Keys.Oem2:
			case Keys.Oem1 | Keys.Shift:
				SelectedPart = SelectedPart == partValues.Keys.Last() ? partValues.Keys.First() : partValues.Keys.Next(SelectedPart);
				Invalidate();
				return true;

			case Keys.Enter:
				Value = getValue();
				return true;

			case Keys.Back:
				var _val = partValues[SelectedPart];

				if (!string.IsNullOrEmpty(_val))
				{
					partValues[SelectedPart] = partValues[SelectedPart].Substring(0, partValues[SelectedPart].Length - 1);
					Invalidate();
				}

				return true;

			case Keys.Down:
			case Keys.Up:
			case Keys.Control | Keys.Down:
			case Keys.Control | Keys.Up:
				var inc = keyData.HasFlag(Keys.Down) ? -1 : 1;
				var val = partValues[SelectedPart];

				if (keyData.HasFlag(Keys.Control))
				{
					inc *= -3000;
				}

				if (inc == -1 && val.SmartParse() <= 2000 && SelectedPart == DatePart.Year)
				{
					partValues[SelectedPart] = "1900";
				}
				else
				{
					var _range = range(SelectedPart);
					var next = val.SmartParse() + inc;
					if (next > _range[1])
					{
						next = _range[0];
					}
					else if (next < _range[0])
					{
						next = _range[1];
					}

					partValues[SelectedPart] = next.ToString();
				}

				handlePartLeave();
				Invalidate();
				return true;
		}

		if (keyData.IsDigit())
		{
			var targetLength = SelectedPart == DatePart.Year ? 4 : 2;
			var val = partValues[SelectedPart];

			if (val.Length == targetLength)
			{
				val = string.Empty;
			}

			val += keyData.ToString().SmartParse().ToString();

			partValues[SelectedPart] = val;

			if (val.Length >= targetLength)
			{
				SelectedPart = partValues.Keys.Next(SelectedPart);
			}

			Invalidate();

			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private DateTime getValue()
	{
		return new DateTime(
		partValues.TryGet(DatePart.Year).IfEmpty("1900", $"{"2000".TrimEnd(partValues.TryGet(DatePart.Year)?.Length ?? 0)}{partValues.TryGet(DatePart.Year)}").SmartParse().Between(1900, 3000),
		partValues.TryGet(DatePart.Month).SmartParse().Between(1, 12),
		partValues.TryGet(DatePart.Day).SmartParse().Between(1, DateTime.DaysInMonth(partValues.TryGet(DatePart.Year).SmartParse().Between(1900, 3000), partValues.TryGet(DatePart.Month).SmartParse().Between(1, 12))),
		partValues.TryGet(DatePart.Hour).SmartParse().Between(0, 23),
		partValues.TryGet(DatePart.Minute).SmartParse().Between(0, 59),
		partValues.TryGet(DatePart.Second).SmartParse().Between(0, 59));
	}

	private void handlePartLeave()
	{
		if (partValues.ContainsKey(SelectedPart))
		{
			var _range = range(SelectedPart);
			var value = partValues[SelectedPart];

			if (SelectedPart == DatePart.Year)
			{
				value = "2000".TrimEnd(value.Length) + value;
			}

			partValues[SelectedPart] = value.SmartParse().Between(_range[0], _range[1]).ToString("00");
		}
	}

	private int[] range(DatePart part)
	{
		return part switch
		{
			DatePart.Day => new int[] { 1, 31 },
			DatePart.Month => new int[] { 1, 12 },
			DatePart.Year => new int[] { 1900, 3000 },
			DatePart.Hour => new int[] { 0, 23 },
			DatePart.Minute or DatePart.Second => new int[] { 0, 59 },
			_ => new int[0],
		};
	}

	private int val(DateTime date, DatePart part)
	{
		return part switch
		{
			DatePart.Day => date.Day,
			DatePart.Month => date.Month,
			DatePart.Year => date.Year,
			DatePart.Hour => date.Hour,
			DatePart.Minute => date.Minute,
			DatePart.Second => date.Second,
			_ => 0,
		};
	}

	public void SetError(bool warning = false) { }

	public void ResetValue()
	{
		Value = _defaultValue;
	}
}