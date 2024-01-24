using Extensions;

using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public partial class SideBarPanelContent : PanelContent
{
	public SideBarPanelContent()
	{
		InitializeComponent();
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		base_P_Sidebar.BackColor = base_Text.BackColor = design.AccentBackColor;
	}

	private void SideBarPanelContent_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.Clear(BackColor);

		e.Graphics.FillRectangle(new SolidBrush(base_P_Sidebar.BackColor), new Rectangle(0, 0, base_P_Sidebar.Width + base_P_Sidebar.Left, Height));
		e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.AccentColor), new Rectangle(base_P_Sidebar.Width + base_P_Sidebar.Left, 0, 1, Height));
	}
}