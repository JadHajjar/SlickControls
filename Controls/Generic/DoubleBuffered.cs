using System.Windows.Forms;

namespace SlickControls;

public class DBPanel : Panel
{
	public DBPanel()
	{
		DoubleBuffered = true;
		ResizeRedraw = true;
	}
}

public class DBTableLayoutPanel : TableLayoutPanel
{
	public DBTableLayoutPanel()
	{
		DoubleBuffered = true;
		ResizeRedraw = true;
	}
}

public class DBFlowLayoutPanel : FlowLayoutPanel
{
	public DBFlowLayoutPanel()
	{
		DoubleBuffered = true;
		ResizeRedraw = true;
	}
}

public class DBPictureBox : PictureBox
{
	public DBPictureBox()
	{
		DoubleBuffered = true;
		ResizeRedraw = true;
	}
}