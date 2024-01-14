using Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls;

public partial class SlickToolStrip : Form
{
	public readonly SlickStripItem[] Items;

	private readonly SlickForm _form;
	private long epoch;
	private long lastTick;
	private bool loaded;
	private double percentage;
	private bool mouseDown;
	private Point startingCursorPosition;
	private Point lastCursorPosition;
	private bool reversed;
	private readonly WaitIdentifier leaveIdentifier = new();

	private SlickToolStrip(IEnumerable<SlickStripItem> items, SlickForm form = null, Point? location = null)
	{
		_form = form;
		Items = items.ToArray();
		startingCursorPosition = location ?? Cursor.Position;
		loaded = AnimationHandler.NoAnimations;
		percentage = loaded ? 1 : 0;

		FormBorderStyle = FormBorderStyle.None;
		AutoScaleMode = AutoScaleMode.None;
		StartPosition = FormStartPosition.Manual;
		DoubleBuffered = true;
		ResizeRedraw = true;
		ShowIcon = false;
		ShowInTaskbar = false;
		Font = UI.Font(8.25F);
		Padding = UI.Scale(new Padding(3), UI.FontScale);
		TransparencyKey = BackColor = Color.FromArgb(64, 64, 0);
		Size = Size.Empty;

		var first = Items.FirstOrDefault(x => !x.Disabled && !x.IsEmpty);

		if (first != null)
		{
			first.IsFocused = true;
		}

		if (form == null || form.TopMost)
		{
			TopMost = true;
		}
		else
		{
			form.CurrentFormState = FormState.ForcedFocused;
			form.FormIsActive = false;
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		//new BackgroundAction(() => this.TryInvoke(this.ShowUp)).RunIn(100);

		var size = GetSize(Items, default);
		var startLocation = startingCursorPosition;

		if (startingCursorPosition.Y + size.Height > SystemInformation.WorkingArea.Bottom)
		{
			reversed = true;

			if (startingCursorPosition.X + size.Width > SystemInformation.WorkingArea.Right)
			{
				startLocation = new Point(startingCursorPosition.X - size.Width, startingCursorPosition.Y - size.Height);
			}
			else
			{
				startLocation = new Point(startingCursorPosition.X, startingCursorPosition.Y - size.Height);
			}
		}
		else if (startingCursorPosition.X + size.Width > SystemInformation.WorkingArea.Right)
		{
			startLocation = new Point(startingCursorPosition.X - size.Width, startingCursorPosition.Y);
		}

		GetSize(Items, startLocation);

		UpdateSize();

		epoch = DateTime.Now.Ticks;
	}

	protected override async void OnPaintBackground(PaintEventArgs e)
	{
		base.OnPaintBackground(e);

		if (!loaded)
		{
			var oldTick = lastTick;

			lastTick = (DateTime.Now.Ticks - epoch) / TimeSpan.TicksPerMillisecond;
			percentage = (1 - Math.Cos(lastTick / 50D)) / 2d;

			if (lastTick > Math.PI * 50)
			{
				percentage = 1;
				loaded = true;
				return;
			}

			await Task.Delay(Math.Max(2, 20 - (int)(lastTick - oldTick)));

			Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.Clear(BackColor);

		var clip = new Rectangle(PointToClient(startingCursorPosition), default).CenterR(new Size((int)(Width * percentage * 2), (int)(Height * percentage * 2)));
		e.Graphics.SetClip(clip);

		var design = FormDesign.Design;
		var mousePos = PointToClient(Cursor.Position);

		drawItems(Items);

		void drawItems(IEnumerable<SlickStripItem> items)
		{
			var backRectangle = PointToClient(items.Select(x => x.Rectangle).Aggregate(Rectangle.Union));

			using var gradientBrush = SlickControl.Gradient(backRectangle, design.BackColor);
			e.Graphics.FillRectangle(gradientBrush, backRectangle);

			foreach (var item in items.Where(x => !x.IsEmpty))
			{
				var rectangle = PointToClient(item.Rectangle);
				var isHovered = rectangle.Contains(mousePos);
				using var brush = new SolidBrush(item.IsOpened ? design.ForeColor : item.Disabled && item.SubItems.Count == 0 ? design.InfoColor : isHovered && mouseDown ? design.ActiveForeColor : design.ForeColor);
				using var image = item.Image?.Get(rectangle.Height * 3 / 4);

				if (isHovered && !item.Disabled)
				{
					using var backBrush = new SolidBrush(mouseDown ? design.ActiveColor : Color.FromArgb(65, design.ForeColor.MergeColor(design.AccentColor)));

					e.Graphics.FillRectangle(backBrush, rectangle);
				}

				if (item.IsOpened)
				{
					using var icon = IconManager.GetIcon("I_ArrowRight", rectangle.Height * 3 / 4).Color(design.ForeColor);
					using var backBrush = new LinearGradientBrush(rectangle, Color.FromArgb(50, design.ActiveColor), Color.FromArgb(200, design.ActiveColor), 0f);

					e.Graphics.FillRectangle(backBrush, rectangle);
					e.Graphics.DrawImage(icon, rectangle.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleRight));
				}
				else if (item.SubItems.Count > 0)
				{
					using var icon = IconManager.GetIcon("I_ArrowRight", rectangle.Height * 3 / 4).Color(design.AccentColor);

					e.Graphics.DrawImage(icon, rectangle.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleRight));
				}

				if (image != null)
				{
					e.Graphics.DrawImage(image.Color(brush.Color), rectangle.Pad(Padding).Align(image.Size, ContentAlignment.MiddleLeft));

					if (!string.IsNullOrWhiteSpace(item.Text))
					{
						e.Graphics.DrawString(item.Text, Font, brush, rectangle.Pad(Padding).Pad(image.Width + Padding.Left, 0, 0, 0));
					}
				}
				else
				{
					e.Graphics.DrawString(item.Text, Font, brush, rectangle.Pad(Padding));
				}

				if (item.IsOpened)
				{
					drawItems(item.SubItems);
				}
				else if (item.IsFocused)
				{
					SlickControl.DrawFocus(e.Graphics, rectangle.Pad(1), HoverState.Focused, 0);
				}
			}

			using var pen = new Pen(design.AccentColor, (float)Math.Floor(UI.UIScale)) { Alignment = PenAlignment.Inset };
			e.Graphics.DrawRectangle(pen, Rectangle.Intersect(backRectangle, clip));
		}
	}

	protected override void OnLeave(EventArgs e)
	{
		base.OnLeave(e);

		this.TryInvoke(Dispose);
	}

	protected override void OnDeactivate(EventArgs e)
	{
		base.OnDeactivate(e);

		this.TryInvoke(Dispose);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);

		mouseDown = true;
		Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);

		mouseDown = false;
		Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (lastCursorPosition == PointToScreen(e.Location))
		{
			return;
		}

		if (CheckSubItemHover(Items, e.Location))
		{
			leaveIdentifier.Cancel();
		}
		else
		{
			CloseAll();
		}

		var stripItem = GetHovered(Items, e.Location);

		if (stripItem != null && !stripItem.IsEmpty && !stripItem.Disabled)
		{
			Cursor = Cursors.Hand;
		}
		else
		{
			Cursor = Cursors.Default;
		}

		Invalidate();
		lastCursorPosition = PointToScreen(e.Location);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);

		CloseAll();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			leaveIdentifier?.Dispose();

			if (_form != null)
			{
				if (_form.Bounds.Contains(System.Windows.Forms.Cursor.Position))
				{
					_form.TryBeginInvoke(() =>
					{
						_form.Focus();
						_form.CurrentFormState = FormState.NormalFocused;
					});
				}

				_form.CurrentFormState = FormState.NormalFocused;
				_form.FormIsActive = true;
			}
		}

		base.Dispose(disposing);
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
			case Keys.Shift | Keys.Tab:
				var items = GetAll(Items).Where(x => !x.IsEmpty && !x.Disabled).ToList();
				var current = items.FirstOrDefault(x => x.IsFocused);

				if (items.Count == 0)
				{
					return false;
				}

				var next = (reversed != (keyData == Keys.Up || keyData == (Keys.Shift | Keys.Tab)) ? items.Previous(current, true) : items.Next(current, true)) ?? items.FirstOrDefault();

				if (current != null)
				{
					current.IsFocused = false;
				}

				next.IsFocused = true;

				leaveIdentifier.Cancel();
				CloseAll(Items);

				var parent = next.Parent;
				while (parent != null)
				{
					parent.IsOpened = true;

					parent = parent.Parent;
				}

				UpdateSize();
				Invalidate();
				return true;

			case Keys.Enter:
			case Keys.Space:
				var stripItem = GetAll(Items).FirstOrDefault(x => x.IsFocused);

				if (stripItem != null && !stripItem.Disabled && !stripItem.IsEmpty)
				{
					stripItem.Action?.Invoke();

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

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		var stripItem = GetHovered(Items, e.Location);

		if (stripItem != null && !stripItem.IsEmpty && !stripItem.Disabled)
		{
			if (e.Button != MouseButtons.Left)
			{
				return;
			}

			stripItem.Action?.Invoke();

			if (stripItem.CloseOnClick)
			{
				Dispose();
			}
			else
			{
				Invalidate();
			}
		}

		else if (stripItem == null)
		{
			Dispose();
		}
	}

	private IEnumerable<SlickStripItem> GetAll(IEnumerable<SlickStripItem> items)
	{
		foreach (var item in items)
		{
			yield return item;

			foreach (var subItem in GetAll(item.SubItems))
			{
				yield return subItem;
			}
		}
	}

	private void CloseAll()
	{
		leaveIdentifier.Wait(() =>
		{
			CloseAll(Items);
			UpdateSize();
		}, 750);
	}

	private void CloseAll(IEnumerable<SlickStripItem> items)
	{
		foreach (var item in items)
		{
			item.IsOpened = false;

			CloseAll(item.SubItems);
		}
	}

	private bool CheckSubItemHover(IEnumerable<SlickStripItem> subItems, Point point)
	{
		SlickStripItem found = null;

		foreach (var grp in subItems)
		{
			if (grp.IsOpened && grp.SubItems.Count > 0)
			{
				if (CheckSubItemHover(grp.SubItems, point))
				{
					found = grp;
					continue;
				}
			}

			if (PointToClient(grp.Rectangle).Contains(point))
			{
				found = grp;

				if (!grp.IsOpened && grp.SubItems.Count > 0)
				{
					grp.IsOpened = true;
					UpdateSize();
				}

				continue;
			}

			//if (grp.IsOpened)
			//{
			//	grp.IsOpened = false;
			//	UpdateSize();
			//}
		}

		if (found != null)
		{
			foreach (var grp in subItems)
			{

				if (grp.IsOpened && found != grp)
				{
					grp.IsOpened = false;
					UpdateSize();
				}
			}
		}

		return found != null;
	}

	private void UpdateSize()
	{
		UpdateSize(Items);

		var bounds = Items[0].Rectangle;

		run(Items);

		void run(IEnumerable<SlickStripItem> items)
		{
			foreach (var item in items)
			{
				bounds = Rectangle.Union(bounds, item.Rectangle);

				if (item.IsOpened)
				{
					run(item.SubItems);
				}
			}
		}

		if (!IsDisposed)
		{
			this.TryInvoke(() => Bounds = bounds.Pad(-(int)Math.Floor(UI.UIScale)));
		}
	}

	private void UpdateSize(IEnumerable<SlickStripItem> items)
	{
		foreach (var item in items)
		{
			if (item.IsOpened)
			{
				var size = GetSize(item.SubItems, default);
				var itemRect = item.Rectangle;
				var left = itemRect.Right + size.Width > SystemInformation.WorkingArea.Right;
				int y;

				if (reversed)
				{
					if (itemRect.Bottom - size.Height < 0)
					{
						y = 0;
					}
					else
					{
						y = itemRect.Bottom - size.Height + (int)Math.Floor(UI.UIScale);
					}
				}
				else
				{
					if (itemRect.Y + size.Height > SystemInformation.WorkingArea.Bottom)
					{
						y = SystemInformation.WorkingArea.Bottom - size.Height;
					}
					else
					{
						y = itemRect.Y - (int)Math.Floor(UI.UIScale);
					}
				}

				GetSize(item.SubItems, new Point(left ? itemRect.X - size.Width : itemRect.Right, y));

				UpdateSize(item.SubItems);
			}
		}
	}

	private SlickStripItem GetHovered(IEnumerable<SlickStripItem> items, Point location)
	{
		foreach (var item in items)
		{
			if (PointToClient(item.Rectangle).Contains(location))
			{
				return item;
			}

			if (item.IsOpened)
			{
				var subItem = GetHovered(item.SubItems, location);

				if (subItem != null)
				{
					return subItem;
				}
			}
		}

		return null;
	}

	private Size GetSize(IEnumerable<SlickStripItem> items, Point startPosition)
	{
		using var graphics = Graphics.FromHwnd(IntPtr.Zero);

		var fontHeight = (int)graphics.Measure(" ", Font).Height;
		var height = 2 * (int)Math.Floor(UI.UIScale);
		var width = 0;

		startPosition.Y += height / 2;

		foreach (var item in items)
		{
			var itemHeight = Padding.Vertical + (int)Math.Floor(UI.UIScale) + (item.IsEmpty ? 0 : fontHeight);
			var itemWidth = getWidth(item, itemHeight);

			item.Rectangle = new Rectangle(startPosition, new Size(itemWidth, itemHeight));

			startPosition.Y += itemHeight;
			height += itemHeight;
			width = Math.Max(width, itemWidth);
		}

		foreach (var item in items)
		{
			item.Rectangle.Width = width;
		}

		return new Size(width, height);

		int getWidth(SlickStripItem item, int height)
		{
			var textSize = (int)(graphics.Measure(item.Text, Font).Width * 1.05f);
			var imageSize = 0;

			if (item.Image != null)
			{
				using var img = item.Image.Get(height * 3 / 4);

				imageSize = img.Width;
			}

			if (item.SubItems.Count > 0)
			{
				using var img = IconManager.GetIcon("I_ArrowRight", height * 3 / 4);

				imageSize += img.Width + Padding.Horizontal;
			}

			return textSize + imageSize + Padding.Horizontal + Padding.Left;
		}
	}

	private Rectangle PointToClient(Rectangle rectangle)
	{
		return new Rectangle(PointToClient(rectangle.Location), rectangle.Size);
	}

	private Rectangle PointToScreen(Rectangle rectangle)
	{
		return new Rectangle(PointToScreen(rectangle.Location), rectangle.Size);
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

		(new SlickToolStrip(PrepareItems(stripItems), form, location) as Form).Show();
	}

	private static IEnumerable<SlickStripItem> PrepareItems(IEnumerable<SlickStripItem> stripItems)
	{
		foreach (var item in stripItems.Where(x => x != null && x.Visible).Trim(x => x.IsEmpty))
		{
			item.Text = LocaleHelper.GetGlobalText(item.Text);

			yield return item;

			if (item.SubItems?.Any() ?? false)
			{
				item.Text += "..";
				item.SubItems = new List<SlickStripItem>(PrepareItems(item.SubItems));

				foreach (var subItem in item.SubItems)
				{
					subItem.Parent = item;
				}
			}
			else
			{
				item.SubItems = [];
			}
		}
	}
}