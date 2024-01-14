using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	internal partial class DateTimePickerPrompt : Form, IAnimatable
	{
		private enum DateView { Years, Months, Days }

		private ExtensionClass.action action;
		private Dictionary<DateView, int> values;
		private DateView view = DateView.Days;

		public SlickDateTime DateBox { get; }
		public Point Mouse { get; private set; }
		public int AnimatedValue { get => (int)(Opacity * 100); set => Opacity = value / 100D; }
		public int TargetAnimationValue => 100;

		public DateTimePickerPrompt(SlickDateTime dateBox)
		{
			InitializeComponent();

			Font = UI.Font(8.25F);
			Size = UI.Scale(new Size(300, 170), UI.FontScale);
			Opacity = 0;
			DateBox = dateBox;
			DoubleBuffered = true;
			values = new Dictionary<DateView, int>
			{
				{ DateView.Years, dateBox.Value.Year },
				{ DateView.Months, dateBox.Value.Month },
				{ DateView.Days, dateBox.Value.Day },
			};

			AnimationHandler.Animate(this, 1.5);
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);

			Close();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			var tfrm = DateBox.FindForm();
			if (tfrm != null && tfrm.MdiParent != null)
				tfrm = tfrm.MdiParent.FindForm();

			tfrm?.BeginInvoke(new Action(() => tfrm.ShowUp()));
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (e.Button == MouseButtons.Left)
				action?.Invoke();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(FormDesign.Design.BackColor);
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			action = null;
			Mouse = PointToClient(Cursor.Position);
			paintButtons(e);

			var topRect = new Rectangle(3, 3, Width - (int)(100 * UI.FontScale), UI.Font(9F, FontStyle.Bold).Height + 6);
			var mainRect = new Rectangle(3, 3, Width - (int)(100 * UI.FontScale), Height - 10).Pad(0, topRect.Height, 0, 0);

			if (!canIncrement(-1))
				e.Graphics.DrawImage(IconManager.GetIcon("I_ArrowLeft").Alpha(100), new Rectangle(4, topRect.Y + topRect.Height / 2 - 8, 16, 16));
			else
			{
				e.Graphics.DrawImage(IconManager.GetIcon("I_ArrowLeft").Color(new Rectangle(4, topRect.Y + topRect.Height / 2 - 8, 16, 16).Contains(Mouse) ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor), new Rectangle(4, topRect.Y + topRect.Height / 2 - 8, 16, 16));

				if (new Rectangle(4, topRect.Y + topRect.Height / 2 - 8, 16, 16).Contains(Mouse))
					action = () => increment(-1);
			}

			if (!canIncrement(1))
				e.Graphics.DrawImage(IconManager.GetIcon("I_ArrowRight").Alpha(100), new Rectangle(mainRect.Width - 22, topRect.Y + topRect.Height / 2 - 8, 16, 16));
			else
			{
				e.Graphics.DrawImage(IconManager.GetIcon("I_ArrowRight").Color(new Rectangle(mainRect.Width - 22, topRect.Y + topRect.Height / 2 - 8, 16, 16).Contains(Mouse) ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor), new Rectangle(mainRect.Width - 22, topRect.Y + topRect.Height / 2 - 8, 16, 16));

				if (new Rectangle(mainRect.Width - 22, topRect.Y + topRect.Height / 2 - 8, 16, 16).Contains(Mouse))
					action = () => increment(1);
			}

			if (topRect.Pad(24, 0, 24, 0).Contains(Mouse) && view != DateView.Years)
			{
				action = () => { view = (DateView)((int)view - 1); Invalidate(); };
				e.Graphics.DrawString(getViewTitle(), UI.Font(9F, FontStyle.Bold), SlickControl.Gradient(topRect, FormDesign.Design.ActiveColor), topRect.Pad(24, 0, 24, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
			else
				e.Graphics.DrawString(getViewTitle(), UI.Font(9F, FontStyle.Bold), SlickControl.Gradient(topRect, FormDesign.Design.LabelColor), topRect.Pad(24, 0, 24, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

			drawView(e, mainRect);

			e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), mainRect.Width + 5, 5, mainRect.Width + 5, Height - 5);

			e.Graphics.DrawRectangle(new Pen(FormDesign.Design.ActiveColor), new Rectangle(0, 0, Width - 1, Height - 1));

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
					Close();
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
					return val.Year - (val.Year % 12) <= DateBox.MaxDate.Year && val.Year - (val.Year % 12) >= DateBox.MinDate.Year - 11;

				case DateView.Months:
					val = val.AddYears(v);
					return val.Year == val.Year.Between(DateBox.MinDate.Year, DateBox.MaxDate.Year);

				case DateView.Days:
					val = val.AddMonths(v);
					return new DateTime(val.Year, val.Month, DateTime.DaysInMonth(val.Year, val.Month)) >= DateBox.MinDate && new DateTime(val.Year, val.Month, 1) <= DateBox.MaxDate;
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
						if (year == year.Between(DateBox.MinDate.Year, DateBox.MaxDate.Year))
						{
							if (year == DateBox.Value.Year)
								e.Graphics.FillRectangle(SlickControl.Gradient(rect, Color.FromArgb(50, FormDesign.Design.ActiveColor)), rect.Pad(1, 0, 0, 1));

							e.Graphics.DrawString(year.ToString(), Font, SlickControl.Gradient(rect, FormDesign.Design.ForeColor), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

							if (year == DateTime.Today.Year || rect.Contains(Mouse))
								using (var pen = new Pen(FormDesign.Design.ActiveColor, 1F))
								{
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
							rect = new Rectangle(mainRect.X, rect.Y + h, w, h);
						else
							rect.X += rect.Width;
					}
					break;

				case DateView.Months:
					w /= 4;
					h /= 3;
					rect = new Rectangle(mainRect.X, y, w, h);

					for (var i = 1; i <= 12; i++)
					{
						if (new DateTime(values[DateView.Years], i, DateTime.DaysInMonth(values[DateView.Years], i)) >= DateBox.MinDate && new DateTime(values[DateView.Years], i, 1) <= DateBox.MaxDate)
						{
							var month = new DateTime(1, i, 1).ToString("MMM");

							if (i == DateBox.Value.Month && values[DateView.Years] == DateBox.Value.Year)
								e.Graphics.FillRectangle(SlickControl.Gradient(rect, Color.FromArgb(50, FormDesign.Design.ActiveColor)), rect.Pad(1, 0, 0, 1));

							e.Graphics.DrawString(month, Font, SlickControl.Gradient(rect, FormDesign.Design.ForeColor), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

							if ((i == DateTime.Today.Month && values[DateView.Years] == DateTime.Now.Year) || rect.Contains(Mouse))
								using (var pen = new Pen(FormDesign.Design.ActiveColor, 1F))
								{
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
							rect = new Rectangle(mainRect.X, rect.Y + h, w, h);
						else
							rect.X += rect.Width;
					}
					break;

				case DateView.Days:
					w /= 7;
					h /= 7;
					rect = new Rectangle(mainRect.X, y, w, h);

					for (var i = 1; i <= 7; i++)
					{
						e.Graphics.DrawString(((DayOfWeek)i.If(7, 0)).ToString().Substring(0, 3), Font, SlickControl.Gradient(rect, FormDesign.Design.InfoColor), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
						rect.X += rect.Width;
					}
					e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), 5, y + h, mainRect.Width - 5, y + h);

					var days = getDaysIndex();
					rect = new Rectangle(mainRect.X, y + h + 3, w, h);

					foreach (var item in days)
					{
						if (item.Key >= DateBox.MinDate && item.Key <= DateBox.MaxDate)
						{
							if (item.Key == DateBox.Value)
								e.Graphics.FillRectangle(SlickControl.Gradient(rect, Color.FromArgb(50, FormDesign.Design.ActiveColor)), rect.Pad(1, 0, 0, 1));

							e.Graphics.DrawString(item.Key.Day.ToString(), Font, SlickControl.Gradient(rect, item.Key.Month == values[DateView.Months] ? FormDesign.Design.ForeColor : FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor)), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

							if (item.Key == DateTime.Today || rect.Contains(Mouse))
								using (var pen = new Pen(FormDesign.Design.ActiveColor, 1F))
								{
									if (rect.Contains(Mouse))
									{
										pen.DashPattern = new[] { 2F, 2F };
										action = () => setDate(item.Key);
									}
									e.Graphics.DrawRectangle(pen, rect.Pad(1, 0, 0, 1));
								}
						}

						if (item.Value == 0)
							rect = new Rectangle(mainRect.X, rect.Y + h, w, h);
						else
							rect.X += rect.Width;
					}
					break;
			}
		}

		private Dictionary<DateTime, int> getDaysIndex()
		{
			var dic = new Dictionary<DateTime, int>();

			var current = new DateTime(values[DateView.Years], values[DateView.Months], 1);
			while (current.DayOfWeek != DayOfWeek.Monday)
				current = current.AddDays(-1);

			for (var i = 0; i < 7 * 6; i++)
			{
				dic.Add(current, (int)current.DayOfWeek);
				current = current.AddDays(1);
			}

			return dic;
		}

		private string getViewTitle()
		{
			switch (view)
			{
				case DateView.Years:
					return $"{(values[DateView.Years] - (values[DateView.Years] % 12)).Between(DateBox.MinDate.Year, DateBox.MaxDate.Year)} - {(values[DateView.Years] - (values[DateView.Years] % 12) + 11).Between(DateBox.MinDate.Year, DateBox.MaxDate.Year)}";

				case DateView.Months:
					return values[DateView.Years].ToString();

				case DateView.Days:
					return $"{new DateTime(values[DateView.Years], values[DateView.Months], 1):MMMM yyyy}";

				default:
					return string.Empty;
			}
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

		private void paintButtons(PaintEventArgs e)
		{
			var rect = new Rectangle(Width - (int)(100 * UI.FontScale) + 5, 0, (int)(100 * UI.FontScale) - 5, Height / 6);

			e.Graphics.FillRectangle(SlickControl.Gradient(new Rectangle(Width + 5 - (int)(100 * UI.FontScale), 0, (int)(100 * UI.FontScale), Height), FormDesign.Design.AccentBackColor), new Rectangle(Width + 5 - (int)(100 * UI.FontScale), 0, (int)(100 * UI.FontScale), Height));
			
			drawButton("Today", DateTime.Today);
			drawButton("Yesterday", DateTime.Today.AddDays(-1));
			drawButton("Start of Month", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
			drawButton("End of Month", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1));
			drawButton("Start of Year", new DateTime(DateTime.Now.Year, 1, 1));
			drawButton("End of Year", new DateTime(DateTime.Now.Year, 12, 31));

			void drawButton(string text, DateTime val)
			{
				var valid = val >= DateBox.MinDate && val <= DateBox.MaxDate;
				var hovered = valid && rect.Contains(Mouse);

				if (hovered)
				{
					e.Graphics.FillRectangle(SlickControl.Gradient(rect, FormDesign.Design.ActiveColor), rect);
					action = () => setDate(val);
				}

				e.Graphics.DrawString(hovered ? val.ToString("dd \\/ MM \\/ yyyy") : text, Font, SlickControl.Gradient(rect, hovered ? FormDesign.Design.ActiveForeColor : valid ? FormDesign.Design.ForeColor : FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor)), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), rect.X + 5, rect.Y, rect.X + rect.Width - 10, rect.Y);

				rect.Y += rect.Height;
			}
		}

		private void setDate(DateTime val)
		{
			DateBox.Value = val;
			Close();
		}
	}
}