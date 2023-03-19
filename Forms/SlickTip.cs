using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class SlickTip : Form
	{
		public string Title { get; private set; }

		public SlickTip(Control control, string title, string text, int timeout)
		{
			InitializeComponent();

			if (control.FindForm() == null) { Dispose(); return; }

			Control = control;
			SetData(title, text);

			Paint += ToolTip_Draw;
			control.MouseLeave += Control_MouseLeave;
			control.Disposed += Control_MouseLeave;

			FormDesign.DesignChanged += DesignChanged;

			new BackgroundAction(() =>
			{
				if (!IsDisposed)
					this.TryInvoke(Dismiss);
				if (currentControl?.Value == this)
					currentControl = null;
			}).RunIn(timeout);
		}

		private void DesignChanged(FormDesign design) => Invalidate();

		private void SetData(string title, string text)
		{
			if (Control == null) return;

			try
			{
				Title = title;
				Text = text;

				var frm = Control.FindForm();

				if (frm == null)
				{
					Dismiss();

					return;
				}

				var ctrlPos = Control.PointToScreen(Point.Empty);
				var bnds = SizeF.Empty;

				using (var g = CreateGraphics())
				{
					bnds = g.Measure(text, UI.Font(8.25F), Math.Max(200, Control.Width));
					if (!string.IsNullOrWhiteSpace(title))
					{
						var titleBnds = g.Measure(Title, UI.Font(9F, FontStyle.Bold), Math.Max(200, Control.Width));
						bnds = new SizeF(Math.Max(titleBnds.Width, bnds.Width), bnds.Height + titleBnds.Height);
					}
				}

				Size = new Size(8 + (int)Math.Ceiling(bnds.Width), 6 + (int)Math.Ceiling(bnds.Height));
				Location = new Point(ctrlPos.X, ctrlPos.Y - Height - 3);

				if (Location.X + Width > Math.Min(SystemInformation.VirtualScreen.Width, frm.Width + frm.Location.X))
					Left = ctrlPos.X + Control.Width - Width;
				if (Location.Y < Math.Max(0, frm.Location.Y))
					Top = ctrlPos.Y + Control.Height + 3;

				Invalidate();
			}
			catch { }
		}

		private void Control_MouseLeave(object sender, EventArgs e)
		{
			if (!IsDisposed)
				this.TryInvoke(Dismiss);
			if (currentControl?.Value == this)
				currentControl = null;
		}

		public void Reveal(Control control)
		{
			var frm = control.FindForm(); if (frm == null) { Dispose(); return; }

			TopMost = frm.TopMost;
			Show(frm);
			animationTimer = new System.Timers.Timer(30) { Enabled = true };
			animationTimer.Elapsed += (s, e) =>
			{
				if (IsDisposed || Opacity >= 1)
					animationTimer.Dispose();
				else
					this.TryInvoke(() => Opacity += .15);
			};
		}

		public void Dismiss()
		{
			animationTimer?.Dispose();
			animationTimer = new System.Timers.Timer(30) { Enabled = true };
			animationTimer.Elapsed += (s, e) =>
			{
				if (IsDisposed || Opacity <= 0)
				{ animationTimer.Dispose(); this.TryInvoke(Dispose); }
				else
				{
					this.TryInvoke(() => Opacity -= .18);
				}
			};
		}

		private void ToolTip_Draw(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(SlickControl.Gradient(new Rectangle(Point.Empty, Size), FormDesign.Design.AccentColor), new Rectangle(Point.Empty, Size));

			e.Graphics.FillRectangle(SlickControl.Gradient(new Rectangle(Point.Empty, Size), FormDesign.Design.BackColor), new Rectangle(1, 1, Width - 2, Height - 2));

			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (string.IsNullOrWhiteSpace(Title))
			{
				e.Graphics.DrawString(Text, UI.Font(8.25F), SlickControl.Gradient(new Rectangle(Point.Empty, Size), FormDesign.Design.ForeColor), new Rectangle(4, 3, Width - 4, Height));
			}
			else
			{
				var bnds = e.Graphics.Measure(Title, UI.Font(9F, FontStyle.Bold), Width - 4);

				e.Graphics.DrawString(Title, UI.Font(9F, FontStyle.Bold), SlickControl.Gradient(new Rectangle(Point.Empty, Size), FormDesign.Design.ForeColor), new Rectangle(4, 3, Width - 4, Height));
				e.Graphics.DrawString(Text, UI.Font(8.25F), SlickControl.Gradient(new Rectangle(Point.Empty, Size), FormDesign.Design.LabelColor), new Rectangle(4, (int)bnds.Height + 4, Width - 4, Height));
			}
		}

		protected override bool ShowWithoutActivation => true;

		protected override CreateParams CreateParams
		{
			get
			{
				var baseParams = base.CreateParams;

				const int WS_EX_NOACTIVATE = 0x08000000;
				const int WS_EX_TOOLWINDOW = 0x00000080;
				baseParams.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;

				return baseParams;
			}
		}

		public Control Control { get; }

		#region Statics

		private static readonly Dictionary<Control, Tuple<string, string, int>> controlsDictionary = new Dictionary<Control, Tuple<string, string, int>>();
		private static KeyValuePair<Control, SlickTip>? currentControl;
		private System.Timers.Timer animationTimer;

		public static void SetTo(Control control, string text, bool recursive = true, int timeout = 10000)
			=> SetTo(control, null, text, recursive, timeout);

		public static void SetTo(Control control, string title, string text, bool recursive = true, int timeout = 10000)
		{
			if (control == null || (string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(title)))
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
				controlsDictionary.Add(control, new Tuple<string, string, int>(title, text, timeout));
			}
			else
			{
				controlsDictionary[control] = new Tuple<string, string, int>(title, text, timeout);

				if (currentControl?.Key == control)
				{
					currentControl?.Value.SetData(title, text);
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
					SetTo(ctrl, title, text);
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

			if (control.FindForm() is SlickForm frm && frm.FormIsActive)
			{
				var timer = new System.Timers.Timer(1000) { Enabled = true, AutoReset = false }; timer.Elapsed += (s, et) =>

				control.TryInvoke(() =>
				{
					if (frm.FormIsActive && mouseIsIn(control, MousePosition))
					{
						if (currentControl != null)
						{
							if (currentControl?.Key == control)
								return;
							else
								currentControl?.Value.Dismiss();
						}

						currentControl = new KeyValuePair<Control, SlickTip>(control, new SlickTip(control, controlsDictionary[control].Item1, controlsDictionary[control].Item2, controlsDictionary[control].Item3));
						frm.CurrentFormState = FormState.ForcedFocused;
						currentControl?.Value.Reveal(currentControl?.Key);
						frm.Focus();
						frm.CurrentFormState = FormState.NormalFocused;
						timer.Dispose();
					}
				});
			}
		}

		private static bool mouseIsIn(Control control, Point point)
		{
			try
			{ return control.ClientRectangle.Contains(control.PointToClient(point)); }
			catch
			{ return false; }
		}

		#endregion Statics
	}
}