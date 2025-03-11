using Extensions;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public partial class SlickGrid : DataGridView
{
	public bool Live { get; private set; }

	public SlickGrid()
	{
		Disposed += (s, e) => FormDesign.DesignChanged -= DesignChanged;
	}

	protected virtual void DesignChanged(FormDesign design)
	{
		BackgroundColor = GridColor = design.BackColor;
		Font = UI.Font(9.75F, FontStyle.Bold);

		ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
		{
			BackColor = design.MenuColor,
			Font = UI.Font(9.75F, FontStyle.Bold),
			ForeColor = design.MenuForeColor,
			SelectionBackColor = design.MenuColor,
			SelectionForeColor = design.MenuForeColor,
			Alignment = DataGridViewContentAlignment.MiddleCenter
		};

		RowsDefaultCellStyle = new DataGridViewCellStyle()
		{
			BackColor = design.ButtonColor.MergeColor(design.BackColor),
			Font = UI.Font(8.25F),
			ForeColor = design.ButtonForeColor,
			SelectionBackColor = design.ActiveColor,
			SelectionForeColor = design.ActiveForeColor,
			Alignment = DataGridViewContentAlignment.MiddleLeft
		};

		if (!DesignMode)
		{
			using var g = Graphics.FromHwnd(IntPtr.Zero);
			RowTemplate.Height = UI.Font(8.25F).Height + 6;
			ColumnHeadersHeight = UI.Font(9.75F, FontStyle.Bold).Height + 6;
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		BorderStyle = BorderStyle.None;
		CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
		ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
		ColumnHeadersHeight = 28;
		EnableHeadersVisualStyles = false;
		ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		RowHeadersVisible = false;

		if (!DesignMode)
		{
			Live = true;

			FormDesign.DesignChanged += DesignChanged;

			DesignChanged(FormDesign.Design);
		}
	}
}