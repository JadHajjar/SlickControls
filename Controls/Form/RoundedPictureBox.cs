using Extensions;

using System.Windows.Forms;

namespace SlickControls.Controls.Form;

public class RoundedPictureBox : PictureBox
{
	protected override void OnPaint(PaintEventArgs pe)
	{
		pe.Graphics.SetUp(BackColor);
		pe.Graphics.DrawRoundImage(Image, ClientRectangle);
	}
}
