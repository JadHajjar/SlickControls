using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class SlickTip : Control
	{
		private readonly Timer _timer;
		public Control Control { get; }
		public Form Form { get; }
		private TipInfo Info { get; set; }

		private SlickTip(Control control, Form form, TipInfo tipInfo)
		{
			DoubleBuffered = true;
			ResizeRedraw = true;
			Control = control;
			Form = form;

			control.MouseLeave += Control_MouseLeave;
			control.Disposed += Control_MouseLeave;

			_timer = new Timer
			{
				Interval = tipInfo.Timeout
			};

			_timer.Tick += _timer_Tick;
			_timer.Start();

			SetData(tipInfo);
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			this.TryInvoke(Dismiss);
		}

		private void SetData(TipInfo tipInfo)
		{
			try
			{
				Info = tipInfo;
				_timer.Stop();
				_timer.Start();

				var ctrlPos = Form.PointToClient(Control.PointToScreen(Point.Empty));
				var bnds = SizeF.Empty;
				var padding = (int)(3 * UI.FontScale);

				using (var g = Graphics.FromHwnd(IntPtr.Zero))
				{
					bnds = g.Measure(LocaleHelper.GetGlobalText(Info.Text), UI.Font(8.25F), Math.Max(200, Control.Width));
					if (!string.IsNullOrWhiteSpace(Info.Title))
					{
						var titleBnds = g.Measure(LocaleHelper.GetGlobalText(Info.Title), UI.Font(9F, FontStyle.Bold), Math.Max(200, Control.Width));
						bnds = new SizeF(Math.Max(titleBnds.Width, bnds.Width), bnds.Height + titleBnds.Height);
					}
				}

				var size = new Size(8 + (int)Math.Ceiling(bnds.Width), 6 + (int)Math.Ceiling(bnds.Height));
				var location = new Point(ctrlPos.X + Info.Offset.X + padding, ctrlPos.Y + Info.Offset.Y - size.Height - padding);

				if (location.X + size.Width > Form.Width)
				{
					location.X = ctrlPos.X + Control.Width - size.Width - padding;
				}

				if (location.Y < 0)
				{
					location.Y = ctrlPos.Y + Control.Height - size.Height - padding;
				}

				Bounds = new Rectangle(location, size);
			}
			catch { }
		}

		private void Control_MouseLeave(object sender, EventArgs e)
		{
			this.TryInvoke(Dismiss);
		}

		public void Reveal()
		{
			Form.Controls.Add(this);

			BringToFront();
		}

		public void Dismiss()
		{
			if (currentControl?.Value == this)
			{
				currentControl = null;
			}

			this.TryInvoke(Dispose);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_timer?.Dispose();

				if (Control != null)
				{
					Control.MouseLeave -= Control_MouseLeave;
					Control.Disposed -= Control_MouseLeave;
				}
			}

			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SetUp(FormDesign.Design.AccentColor);

			e.Graphics.FillRectangle(SlickControl.Gradient(ClientRectangle, FormDesign.Design.BackColor), ClientRectangle.Pad((int)UI.FontScale));

			if (string.IsNullOrWhiteSpace(Info.Title))
			{
				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Info.Text), UI.Font(8.25F), SlickControl.Gradient(ClientRectangle, FormDesign.Design.ForeColor), ClientRectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else
			{
				var bnds = e.Graphics.Measure(LocaleHelper.GetGlobalText(Info.Title), UI.Font(9F, FontStyle.Bold), Width - 4);

				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Info.Title), UI.Font(9F, FontStyle.Bold), SlickControl.Gradient(ClientRectangle, FormDesign.Design.ForeColor), new Rectangle(4, 3, Width - 4, Height));
				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Info.Text), UI.Font(8.25F), SlickControl.Gradient(ClientRectangle, FormDesign.Design.LabelColor), new Rectangle(4, (int)bnds.Height + 4, Width - 4, Height));
			}
		}

		#region Statics

		private static readonly Dictionary<Control, TipInfo> controlsDictionary = new Dictionary<Control, TipInfo>();
		private static KeyValuePair<Control, SlickTip>? currentControl;

		public static void SetTo(Control control, string text, bool recursive = true, int timeout = 10000, Point offset = default)
		{
			SetTo(control, null, text, recursive, timeout, offset);
		}

		public static void SetTo(Control control, string title, string text, bool recursive = true, int timeout = 10000, Point offset = default)
		{
			if (control == null)
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(title))
			{
				if (controlsDictionary.ContainsKey(control))
				{
					controlsDictionary.Remove(control);
					control.MouseEnter -= Control_MouseEnter;
					control.Disposed -= Control_Disposed;
					if (currentControl?.Key == control)
					{
						currentControl.Value.Value.Dismiss();
					}
				}
				return;
			}

			if (!controlsDictionary.ContainsKey(control))
			{
				control.MouseEnter += Control_MouseEnter;
				control.Disposed += Control_Disposed;
				controlsDictionary.Add(control, new TipInfo(title, text, timeout, offset));

				if (mouseIsIn(control, Cursor.Position))
				{
					Control_MouseEnter(control, null);
				}
			}
			else
			{
				controlsDictionary[control] = new TipInfo(title, text, timeout, offset);

				if (currentControl?.Key == control)
				{
					currentControl?.Value.SetData(controlsDictionary[control]);
					Control_MouseEnter(control, null);
				}
				else
				{
					if (control.FindForm() is SlickForm frm && frm.FormIsActive && mouseIsIn(control, MousePosition))
					{
						Control_MouseEnter(control, null);
					}
				}
			}

			if (recursive && control.Controls.Count > 0)
			{
				foreach (Control ctrl in control.Controls)
				{
					SetTo(ctrl, title, text, recursive, timeout, offset);
				}
			}
		}

		private static void Control_Disposed(object sender, EventArgs e)
		{
			var control = sender as Control;

			control.MouseEnter -= Control_MouseEnter;
			control.Disposed -= Control_Disposed;
		}

		private static void Control_MouseEnter(object sender, EventArgs e)
		{
			var control = sender as Control;

			if (!(control.FindForm() is SlickForm frm) || !frm.FormIsActive)
			{
				return;
			}

			var timer = new System.Timers.Timer(1500) { Enabled = true, AutoReset = false };

			timer.Elapsed += (s, et) =>
			{
				control.TryInvoke(() =>
				{
					if (frm.FormIsActive && mouseIsIn(control, MousePosition))
					{
						if (currentControl != null)
						{
							if (currentControl?.Key == control)
							{
								return;
							}
							else
							{
								currentControl?.Value.Dismiss();
							}
						}

						var tip = new SlickTip(control, frm, controlsDictionary[control]);
						currentControl = new KeyValuePair<Control, SlickTip>(control, tip);
						tip.Reveal();
						timer.Dispose();
					}
				});
			};
		}

		private static bool mouseIsIn(Control control, Point point)
		{
			try
			{ return control.ClientRectangle.Contains(control.PointToClient(point)); }
			catch
			{ return false; }
		}

		#endregion Statics

		private class TipInfo
		{
			public TipInfo(string title, string text, int timeout, Point offset)
			{
				Title = title;
				Text = text;
				Timeout = timeout;
				Offset = offset;
			}

			public string Title { get; }
			public string Text { get; }
			public int Timeout { get; }
			public Point Offset { get; }
		}
	}
}