using Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class SlickToolStrip : Form
	{
		public Dictionary<SlickStripItem, List<SlickStripItem>> Items = new Dictionary<SlickStripItem, List<SlickStripItem>>();

		private readonly SlickForm form;
		private bool mouseDown;
		private Point startingCursorPosition;
		private Point lastCursorPosition;
		private bool reversed;
		private AnimationHandler animation;
		private readonly WaitIdentifier leaveIdentifier = new WaitIdentifier();

		private SlickToolStrip(SlickStripItem[] items, SlickForm form = null, Point? location = null)
		{
			InitializeComponent();

			Location = Cursor.Position;
			BackColor = FormDesign.Design.AccentColor;
			DoubleBuffered = ResizeRedraw = true;

			this.form = form;
			startingCursorPosition = location ?? Cursor.Position;
			Font = UI.Font(8.25F);
			Size = Size.Empty;

			var prevIsOpenable = false;
			foreach (var item in items)
			{
				if (item.Fade && item.Tab == 0 && (items.Next(item)?.Tab ?? 0) > 0)
				{
					item.IsOpenable = prevIsOpenable = true;
					item.Fade = false;
					Items.Add(item, new List<SlickStripItem> { item });
				}
				else if (prevIsOpenable && item.Tab > 0)
				{
					item.IsContent = true;
					Items.Last().Value.Add(item);
				}
				else
				{
					prevIsOpenable = false;
					Items.Add(item, new List<SlickStripItem> { item });
				}
			}

			Items.First(x => !x.Key.Fade && !x.Key.IsEmpty).Value.First().IsFocused = true;

			if (form == null || form.TopMost)
			{
				TopMost = true;
			}
			else
			{
				form.CurrentFormState = FormState.ForcedFocused;
				form.FormIsActive = false;
			}

			Disposed += FlatToolStrip_Disposed;
			Paint += FlatToolStrip_Paint;
		}

		private void updateHeight()
		{
			var bounds = getBounds();

			if (!(animation?.Animating ?? false) || bounds != animation.NewBounds)
			{
				Bounds = bounds;
				animation?.Dispose();
			}
		}

		private Rectangle getBounds()
		{
			var h = (int)UI.FontScale * 2;
			var padding = UI.Scale(new Padding(3), UI.FontScale);

			using (var g = Graphics.FromHwnd(IntPtr.Zero))
			{
				foreach (var stripItem in Items)
				{
					foreach (var item in stripItem.Key.IsOpenable && stripItem.Key.IsOpened ? stripItem.Value : new List<SlickStripItem> { stripItem.Key })
					{
						h += padding.Vertical + (item.IsEmpty ? 0 : (int)g.Measure(" ", Font).Height);
					}
				}

				var diff = UI.FontScale >= 3.5 ? 48 : UI.FontScale >= 1.8 ? 24 : 16;
				var size = new Size(Math.Max(150, padding.Horizontal + padding.Left + diff + (int)(1 * UI.FontScale) + (int)Items.SelectMany(x => x.Value).Max(x => (x.Tab * 12 * UI.FontScale) + g.Measure(LocaleHelper.GetGlobalText(x.Text), Font).Width))
					, h);

				if (startingCursorPosition.Y + Items.SelectMany(x => x.Value).Sum(x => x.IsEmpty ? (int)(7 * UI.UIScale) : ((int)g.Measure(x.Text, Font).Height + 4)) + 2 > SystemInformation.WorkingArea.Height)
				{
					reversed = true;
					if (startingCursorPosition.X + size.Width > SystemInformation.WorkingArea.Width)
					{
						return new Rectangle(new Point(startingCursorPosition.X - size.Width, startingCursorPosition.Y - size.Height), size);
					}
					else
					{
						return new Rectangle(new Point(startingCursorPosition.X, startingCursorPosition.Y - size.Height), size);
					}
				}
				else if (startingCursorPosition.X + size.Width > SystemInformation.WorkingArea.Width)
				{
					return new Rectangle(new Point(startingCursorPosition.X - size.Width, startingCursorPosition.Y), size);
				}
				else
				{
					return new Rectangle(startingCursorPosition, size);
				}
			}
		}

		public static void Show(params SlickStripItem[] stripItems)
		{
			Show(null, null, stripItems);
		}

		public static void Show(SlickForm form, params SlickStripItem[] stripItems)
		{
			Show(form, null, stripItems);
		}

		public static void Show(SlickForm form, Point? location, params SlickStripItem[] stripItems)
		{
			if (location.HasValue && !Screen.PrimaryScreen.Bounds.Contains(location.Value))
			{
				location = null;
			}

			stripItems = stripItems.Where(x => x.Show).Trim(x => x.IsEmpty).ToArray();

			if (stripItems.Length > 0)
			{
				(new SlickToolStrip(stripItems, form, location) as Form).Show();
			}
		}

		private void FlatToolStrip_Paint(object sender, PaintEventArgs e)
		{
			var d = FormDesign.Design;
			var workRect = reversed ? new Rectangle(1, ((animation?.Animating ?? false) ? animation.NewBounds.Height : Height) - 1, 0, 0) : new Rectangle(1, 1, 0, 0);
			var mousePos = PointToClient(Cursor.Position);
			var mousedIn = false;
			var diff = (UI.FontScale >= 3.5 ? 48 : UI.FontScale >= 1.8 ? 24 : 16) / 2;
			var padding = UI.Scale(new Padding(3), UI.FontScale);

			e.Graphics.SetUp(d.AccentColor);

			e.Graphics.FillRectangle(SlickControl.Gradient(ClientRectangle, d.BackColor), ClientRectangle.Pad((int)UI.FontScale));

			foreach (var grp in Items)
			{
				if (grp.Key.IsOpenable && grp.Key.IsOpened)
				{
					var start = 0;
					foreach (var item in grp.Value)
					{
						drawItem(item);
						if (start == 0)
						{
							start = workRect.Y + (reversed ? 0 : workRect.Height);
						}
					}

					e.Graphics.DrawLine(new Pen(FormDesign.Design.ActiveColor, (float)(2.5 * UI.FontScale)) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot, DashCap = System.Drawing.Drawing2D.DashCap.Round },
						diff, start,
						diff, workRect.Y + (grp.Value.Last().IsEmpty != reversed ? 0 : workRect.Height));
				}
				else if (grp.Key.IsOpenable)
				{
					foreach (var item in grp.Value)
					{
						item.IsVisible = false;
					}
				}

				if (!grp.Key.IsOpened)
				{
					drawItem(grp.Key);
				}

				void drawItem(SlickStripItem stripItem)
				{
					var text = LocaleHelper.GetGlobalText(stripItem.Text);
					var h = padding.Vertical + (stripItem.IsEmpty ? 0 : (int)e.Graphics.Measure(" ", Font).Height);
					stripItem.DrawRectangle = workRect = new Rectangle(0, reversed ? (workRect.Y - h) : (workRect.Y + workRect.Height), Width, h);

					var mouseIn = !stripItem.IsOpenable && !stripItem.Fade && !stripItem.IsEmpty && workRect.Contains(mousePos);

					if (mouseIn && workRect.Width > 0)
					{
						e.Graphics.FillRectangle(SlickControl.Gradient(workRect, mouseIn.If(d.ButtonColor.If(mouseDown, d.ActiveColor), d.BackColor)), workRect);
					}

					if (stripItem.IsFocused)
					{
						SlickControl.DrawFocus(e.Graphics, workRect.Pad(-10, 0, -10, 1), HoverState.Focused, 0);
					}

					if (stripItem.IsEmpty)
					{
						return;
					}

					mousedIn |= mouseIn;

					workRect.X += (int)(stripItem.Tab * 12 * UI.FontScale);

					if (stripItem.Image != null)
					{
						e.Graphics.DrawImage(stripItem.Image.Color(stripItem.IsOpened ? d.ActiveColor : stripItem.Fade ? d.InfoColor : mouseIn && mouseDown ? d.ActiveForeColor : d.ForeColor), workRect.Pad(padding).Align(stripItem.Image.Size, ContentAlignment.MiddleLeft));
						workRect.X += stripItem.Image.Width + padding.Left;
					}
					else if (stripItem.ImageName != null)
					{
						using (var img = stripItem.ImageName.Small)
						{
							e.Graphics.DrawImage(img.Color(stripItem.IsOpened ? d.ActiveColor : stripItem.Fade ? d.InfoColor : mouseIn && mouseDown ? d.ActiveForeColor : d.ForeColor), workRect.Pad(padding).Align(img.Size, ContentAlignment.MiddleLeft));
							workRect.X += img.Width + padding.Left;
						}
					}

					if (!string.IsNullOrEmpty(text))
					{
						if (workRect.Width > 0)
						{
							e.Graphics.DrawString(text + (stripItem.IsOpenable && !stripItem.IsOpened ? ".." : ""), Font, new SolidBrush(stripItem.IsOpened ? d.ActiveColor : stripItem.Fade ? d.InfoColor : mouseIn && mouseDown ? d.ActiveForeColor : d.ForeColor)
								, workRect.Pad(padding).Pad(0,-(int)UI.FontScale,0,0));
						}
					}

					stripItem.IsVisible = true;
				}
			}

			Cursor = mousedIn ? Cursors.Hand : Cursors.Default;
		}

		private void FlatToolStrip_Disposed(object sender, EventArgs e)
		{
			if (form != null)
			{
				if (form.Bounds.Contains(Cursor.Position))
				{
					form.TryBeginInvoke(() => { form.Focus(); form.CurrentFormState = FormState.NormalFocused; });
				}

				form.CurrentFormState = FormState.NormalFocused;
				form.FormIsActive = true;
			}
		}

		private void FlatToolStrip_Leave(object sender, EventArgs e)
		{
			this.TryInvoke(Dispose);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Escape:
					Dispose();
					return true;

				case Keys.Up:
				case Keys.Down:
				case Keys.Tab:
					var items = Items.SelectMany(x => x.Value).Where(x => x.IsVisible && !x.IsEmpty && !x.Fade).ToList();
					var current = items.FirstOrDefault(x => x.IsFocused);

					if (items.Count == 0)
					{
						return false;
					}

					var next = (reversed != (keyData == Keys.Up) ? items.Previous(current, true) : items.Next(current, true)) ?? items.FirstOrDefault();

					if (current != null)
					{
						current.IsFocused = false;
					}

					next.IsFocused = true;

					foreach (var grp in Items)
					{
						if (grp.Key.IsOpenable)
						{
							grp.Key.IsOpened = grp.Value.Any(x => x.IsFocused);
						}
					}

					updateHeight();
					Invalidate();
					return true;

				case Keys.Enter:
				case Keys.Space:
					var stripItem = Items.SelectMany(x => x.Value).FirstOrDefault(x => !x.IsOpenable && x.IsVisible && x.IsFocused);

					if (stripItem != null && !stripItem.Fade && !stripItem.IsEmpty)
					{
						stripItem.Action?.Invoke(stripItem);

						if (stripItem.CloseOnClick)
						{
							Dispose();
						}
						else
						{
							Invalidate();
						}
					}
					return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void FlatToolStrip_Load(object sender, EventArgs e)
		{
			new BackgroundAction(() => this.TryInvoke(this.ShowUp)).RunIn(100);

			Bounds = new Rectangle(startingCursorPosition, Size.Empty);
			Opacity = 0;

			animation = new AnimationHandler(this, getBounds()) { Speed = 3 };
			animation.OnAnimationTick += animation_OnAnimationTick;
			animation.StartAnimation();
		}

		private void animation_OnAnimationTick(AnimationHandler handler, Control control, bool finished)
		{
			Opacity = animation.NewBounds.Height > animation.NewBounds.Width
				? (double)Height / animation.NewBounds.Height
				: (double)Width / animation.NewBounds.Width;
		}

		private void FlatToolStrip_MouseDown(object sender, MouseEventArgs e)
		{
			mouseDown = true;
			Invalidate();
		}

		private void FlatToolStrip_MouseUp(object sender, MouseEventArgs e)
		{
			mouseDown = false;
			Invalidate();
		}

		private void FlatToolStrip_MouseMove(object sender, MouseEventArgs e)
		{
			if (lastCursorPosition != PointToScreen(e.Location))
			{
				try
				{
					foreach (var grp in Items)
					{
						if (grp.Key.IsOpenable)
						{
							if (grp.Value.Any(x => x.IsVisible && x.DrawRectangle.Contains(e.Location)))
							{
								foreach (var item in Items)
								{
									item.Key.IsOpened = item.Key == grp.Key;
								}

								leaveIdentifier.Cancel();
								return;
							}
						}
					}

					closeAll();
				}
				finally
				{
					updateHeight();
					Invalidate();
					lastCursorPosition = PointToScreen(e.Location);
				}
			}
		}

		private void FlatToolStrip_MouseLeave(object sender, EventArgs e)
		{
			closeAll();
		}

		private void closeAll()
		{
			leaveIdentifier.Wait(() =>
			{
				Items.Foreach(x => x.Key.IsOpened = false);

				if (!IsDisposed)
				{
					this.TryInvoke(() =>
					{
						updateHeight();
						Invalidate();
					});
				}
			}, 750);
		}

		private void FlatToolStrip_MouseClick(object sender, MouseEventArgs e)
		{
			var stripItem = Items.SelectMany(x => x.Value).FirstOrDefault(x => !x.IsOpenable && x.IsVisible && x.DrawRectangle.Contains(e.Location));

			if (stripItem != null && !stripItem.IsEmpty && !stripItem.Fade)
			{
				stripItem.Action?.Invoke(stripItem);

				if (stripItem.CloseOnClick)
				{
					Dispose();
				}
				else
				{
					Invalidate();
				}
			}
		}
	}
}