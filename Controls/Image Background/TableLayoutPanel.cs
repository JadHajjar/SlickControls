using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls;

public class SlickImageBackgroundTableLayoutPanel : SlickImageBackgroundPanel
{
	public List<RowStyle> RowStyles { get; } = [];
	public List<ColumnStyle> ColumnStyles { get; } = [];

	private readonly Dictionary<SlickImageBackgroundControl, Point> controlCells = [];
	private Rectangle[][] cells;

	public SlickImageBackgroundTableLayoutPanel()
	{
	}

	public void Add(SlickImageBackgroundControl control, int columnIndex, int rowIndex)
	{
		Controls.Add(control);
		SetIndex(control, columnIndex, rowIndex);
	}

	public void SetIndex(SlickImageBackgroundControl control, int columnIndex, int rowIndex)
	{
		controlCells.TryAdd(control, new Point(columnIndex, rowIndex));
	}

	public override void OnPaint(PaintEventArgs e)
	{
		foreach (var item in Controls)
		{
			item.CalculateSize(e);
		}

		calculateCellBounds(out cells);

		base.OnPaint(e);
	}

	public override void CalculateSize(PaintEventArgs e)
	{
		if (AutoSize && Controls.Count > 0)
		{
			for (var i = 0; i < 2; i++)
			{
				foreach (var item in Controls)
				{
					item.CalculateSize(e);
				}

				calculateCellBounds(out cells);

				foreach (var control in Controls)
				{
					control.CalculateSize(e);
					SetControlBounds(control);
					PostControlPaint(control, false);
				}
			}

			var calculatedSize = new Size(Padding.Horizontal + Controls.Max(x => x.Bounds.X + x.Width), Padding.Vertical + Controls.Max(x => x.Bounds.Y + x.Height));

			switch (Dock)
			{
				case DockStyle.None:
					Size = calculatedSize;
					break;
				case DockStyle.Top:
					Height = calculatedSize.Height;
					break;
				case DockStyle.Bottom:
					Height = calculatedSize.Height;
					break;
				case DockStyle.Left:
					Width = calculatedSize.Width;
					break;
				case DockStyle.Right:
					Width = calculatedSize.Width;
					break;
			}
		}
	}

	protected override void SetControlBounds(SlickImageBackgroundControl control)
	{
		if (controlCells.TryGetValue(control, out var cell))
		{
			var bnd = cells[cell.X][cell.Y];
			control.Bounds = new Rectangle(bnd.X, bnd.Y, bnd.Width - control.Margin.Horizontal, bnd.Height - control.Margin.Vertical);
		}
		else
		{
			base.SetControlBounds(control);
		}
	}

	private void calculateCellBounds(out Rectangle[][] cells)
	{
		cells = new Rectangle[ColumnStyles.Count][];

		if (ColumnStyles.Count == 0 || RowStyles.Count == 0)
		{
			return;
		}

		for (var i = 0; i < ColumnStyles.Count; i++)
		{
			cells[i] = new Rectangle[RowStyles.Count];

			var w = 0;
			switch (ColumnStyles[i].SizeType)
			{
				case SizeType.AutoSize:
					if (controlCells.Any(c => c.Value.X == i && c.Key.Visible))
					{
						w = controlCells.Where(c => c.Value.X == i && c.Key.Visible).Max(c => c.Key.Width);
					}

					break;

				case SizeType.Absolute:
					w = (int)ColumnStyles[i].Width;
					break;
			}

			for (var j = 0; j < RowStyles.Count; j++)
			{
				switch (RowStyles[j].SizeType)
				{
					case SizeType.AutoSize:
						if (controlCells.Any(c => c.Value.Y == j && c.Key.Visible))
						{
							cells[i][j].Height = controlCells.Where(c => c.Value.Y == j && c.Key.Visible).Max(c => c.Key.Height);
						}

						break;

					case SizeType.Absolute:
						cells[i][j].Height = (int)RowStyles[j].Height;
						break;

					default:
						cells[i][j].Height = 0;
						break;
				}

				cells[i][j].Width = w;
			}
		}

		var maxW = Width - Padding.Horizontal - cells.Sum(c => c[0].Width);
		for (var i = 0; i < ColumnStyles.Count; i++)
		{
			var w = cells[i][0].Width;
			if (ColumnStyles[i].SizeType == SizeType.Percent)
			{
				w = (int)(maxW * ColumnStyles[i].Width / 100);
			}

			var maxH = Height - Padding.Vertical - cells[i].Sum(c => c.Height);
			for (var j = 0; j < RowStyles.Count; j++)
			{
				if (RowStyles[j].SizeType == SizeType.Percent)
				{
					cells[i][j].Height = (int)(maxH * RowStyles[j].Height / 100);
				}

				cells[i][j].Width = w;
			}
		}

		var x = Padding.Left;
		for (var i = 0; i < ColumnStyles.Count; i++)
		{
			var y = Padding.Top;
			for (var j = 0; j < RowStyles.Count; j++)
			{
				cells[i][j].X = x;
				cells[i][j].Y = y;

				y += cells[i][j].Height;
			}

			x += cells[i][0].Width;
		}
	}

	public void Add(object p, int v1, int v2)
	{
		throw new NotImplementedException();
	}
}