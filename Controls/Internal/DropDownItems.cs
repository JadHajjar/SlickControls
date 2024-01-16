using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

internal partial class DropDownItems : SlickControl
{
	public event Action<object> ItemSelected;

	public object CurrentItem { get; private set; }
	public int ITEM_SIZE { get; private set; }

	private readonly Func<object, string> conversion;
	private readonly List<string> listText = [];
	private readonly List<object> listItems = [];
	private readonly bool fontDropdown;
	private bool mouseDown;
	private bool mouseMoved;
	private Point lastMousePoint;

	public DropDownItems(Func<object, string> conversion = null, bool fontDropdown = false)
	{
		InitializeComponent();
		this.conversion = conversion;
		this.fontDropdown = fontDropdown;

		Height = 0;
		Font = UI.Font(8.25F);

		P_Items.MouseMove += (s, e) =>
		{
			if (lastMousePoint != Cursor.Position)
			{
				lastMousePoint = Cursor.Position;
				mouseMoved = true;
				P_Items.Invalidate();
			}
		};
		P_Items.MouseDown += (s, e) =>
		{
			mouseDown = true;
			P_Items.Invalidate();
		};
		P_Items.MouseUp += (s, e) =>
		{
			mouseDown = false;
			P_Items.Invalidate();
		};
	}

	protected override void DesignChanged(FormDesign design)
	{
		BackColor = design.ActiveColor;
		panel.BackColor = design.AccentBackColor;
	}

	internal void SetItems(IEnumerable<object> list)
	{
		listText.Clear();
		listItems.Clear();
		listItems.AddRange(list);
		listText.AddRange(listItems, item => conversion == null ? item.ToString() : conversion(item));

		ITEM_SIZE = Font.Height * 4 / 3;
		P_Items.Height = fontDropdown ? listText.Sum(x => new Font(x, Font.Size).Height * 4 / 3) : listItems.Count * ITEM_SIZE;
		P_Items.Invalidate();

		CurrentItem = list.FirstOrDefault();

		if (IsHandleCreated)
		{
			new AnimationHandler(this, new Size(Width, Parent == null ? P_Items.Height + 2 : Math.Min(P_Items.Height + 2, Parent.Height - Top - 15))) { Speed = 2 }
				.StartAnimation();
		}
	}

	public new void Dispose()
	{
		new AnimationHandler(this, new Size(Width, 0)) { Speed = 2 }
			.StartAnimation(base.Dispose);
	}

	internal bool KeyPressed(ref Message msg, Keys keyData)
	{
		if (Keys.Up == keyData)
		{
			CurrentItem = listItems.Previous(CurrentItem) ?? listItems.LastOrDefault();
			fixScroll();
			P_Items.Invalidate();
			return true;
		}
		else if (Keys.Down == keyData)
		{
			CurrentItem = listItems.Next(CurrentItem) ?? listItems.FirstOrDefault();
			fixScroll();
			P_Items.Invalidate();
			return true;
		}
		else if (Keys.Enter == keyData)
		{
			if (CurrentItem != null)
			{
				ItemSelected?.Invoke(CurrentItem);
			}

			Dispose();
			return true;
		}

		return false;
	}

	private void fixScroll()
	{
		if (((listItems.IndexOf(CurrentItem) * (double)ITEM_SIZE) + P_Items.Top) < ITEM_SIZE)
		{
			scroll.SetPercentage(-100D * (listItems.IndexOf(CurrentItem) * ITEM_SIZE) / (panel.Height - P_Items.Height).If(0, 1), false);
		}
		else if ((panel.Height - (listItems.IndexOf(CurrentItem) * (double)ITEM_SIZE) - P_Items.Top) < ITEM_SIZE)
		{
			scroll.SetPercentage(-100D * (-panel.Height + ITEM_SIZE + (listItems.IndexOf(CurrentItem) * (double)ITEM_SIZE)) / (panel.Height - P_Items.Height).If(0, 1), false);
		}
	}

	private void P_Items_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.Clear(FormDesign.Design.AccentBackColor);
		e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

		var w = P_Items.Width;
		var h = 0;
		for (var i = 0; i < listItems.Count; i++)
		{
			var font = fontDropdown ? new Font(listText[i], Font.Size) : Font;
			var itemsize = font.Height * 4 / 3;
			var mouseIn = mouseMoved
				? new Rectangle(P_Items.PointToScreen(new Point(0, h)), new Size(w, itemsize)).Contains(Cursor.Position)
				: CurrentItem == listItems[i];

			if (mouseIn)
			{
				CurrentItem = listItems[i];

				if (mouseDown)
				{
					e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(new Point(w * 5 / 10, h), new Point(w * 9 / 10, h), FormDesign.Design.ActiveColor, Color.Empty)
					   , new Rectangle(w * 5 / 10, h, w * 4 / 10, itemsize));

					e.Graphics.FillRectangle(Gradient(FormDesign.Design.ActiveColor), new Rectangle(0, h, (w * 5 / 10) + 1, itemsize));
				}
				else
				{
					e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(new Point(w * 6 / 10, h), new Point(w * 9 / 10, h), FormDesign.Design.BackColor.MergeColor(FormDesign.Design.ActiveColor, 85), Color.Empty)
					, new Rectangle(w * 6 / 10, h, w * 3 / 10, itemsize));

					e.Graphics.FillRectangle(Gradient(FormDesign.Design.BackColor.MergeColor(FormDesign.Design.ActiveColor, 85)), new Rectangle(0, h, (w * 6 / 10) + 1, itemsize));
					e.Graphics.FillRectangle(Gradient(FormDesign.Design.ActiveColor), 0, h, 2, itemsize);
				}
			}

			e.Graphics.DrawString(listText[i], font, Gradient(mouseIn && mouseDown ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor), new RectangleF(5, h, w - 10, itemsize), new StringFormat() { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center });

			h += itemsize;
		}

		mouseMoved = false;
	}

	private void P_Items_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			if (CurrentItem != null)
			{
				ItemSelected?.Invoke(CurrentItem);
			}

			Dispose();
		}
	}
}