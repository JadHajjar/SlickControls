using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickImageBackgroundContainer : SlickAdvancedImageControl
	{
		private List<SlickImageBackgroundControl> content = new List<SlickImageBackgroundControl>();
		public SlickImageBackgroundControl[] Content { get => content.ToArray(); set => content = value.ToList(); }


		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
		}
	}
}
