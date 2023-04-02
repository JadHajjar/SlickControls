using Extensions;

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class ChangeLogVersion : SlickControl
	{
		private readonly VersionChangeLog inf;

		public ChangeLogVersion(VersionChangeLog inf)
		{
			InitializeComponent();
			this.inf = inf;
			Dock = DockStyle.Top;
			ResizeRedraw = DoubleBuffered = true;
			TabStop = false;
			Height = GetHeight();
		}

		private void ChangeLogVersion_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(BackColor);

			DrawItems(e.Graphics, true);
		}

		private int GetHeight() => DrawItems(Graphics.FromHwnd(IntPtr.Zero), false);

		private int DrawItems(Graphics g, bool draw)
		{
			var tab = 0.25D;
			var h = 4;

			g.DrawStringItem(inf.Version
				, UI.Font(14F, FontStyle.Bold)
				, FormDesign.Design.ForeColor
				, Width
				, tab
				, ref h
				, draw);

			if (draw)
				g.DrawLine(new Pen(FormDesign.Design.AccentColor, 1), 20, h, 20, Height - 13);

			if (draw && inf.Date != null)
			{
				var bnds = g.Measure($"on {inf.Date?.ToReadableString(inf.Date?.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)}", UI.Font(8.25F));
				g.DrawString($"on {inf.Date?.ToReadableString(inf.Date?.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)}",
					UI.Font(8.25F),
					new SolidBrush(FormDesign.Design.LabelColor),
					new Point((int)(56 * UI.FontScale), (int)(h - bnds.Height - (4 * UI.FontScale))));
			}

			tab = 2;

			if (!string.IsNullOrWhiteSpace(inf.Tagline))
			{
				h += 2;

				g.DrawStringItem(inf.Tagline
					 , UI.Font(8.25F, FontStyle.Italic)
					 , FormDesign.Design.ButtonForeColor
					 , Width
					 , tab
					 , ref h
					, draw);

				h += 4;
			}

			h += 2;

			foreach (var item in inf.ChangeGroups.OrderBy(x => x.Order))
			{
				tab = 2;

				g.DrawStringItem(item.Name
					, UI.Font(8.25F, FontStyle.Bold)
					, FormDesign.Design.LabelColor
					, Width
					, tab
					, ref h
					, draw);

				tab++;

				foreach (var ch in item.Changes)
				{
					g.DrawStringItem((ch.StartsWith("-") ? "     " : "•  ") + ch
						, UI.Font(8.25F)
						, FormDesign.Design.InfoColor
						, Width
						, tab
						, ref h
						, draw);
				}

				h += 10;
			}

			return h;
		}

		private void ChangeLogVersion_Resize(object sender, EventArgs e)
		{
			var ch = GetHeight();
			if (Height != ch)
				Height = ch;
		}
	}
}