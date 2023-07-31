using Extensions;

using System;
using System.Drawing;
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
			TabStop = false;
			Height = GetHeight();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SetUp(BackColor);

			DrawItems(e.Graphics, true);
		}

		private int GetHeight()
		{
			return DrawItems(Graphics.FromHwnd(IntPtr.Zero), false);
		}

		private int DrawItems(Graphics g, bool draw)
		{
			var tab = 0D;
			var h = (int)(4 * UI.FontScale);

			var versionSize = g.DrawStringItem("v" + inf.Version
				, UI.Font(15.5F, FontStyle.Bold)
				, FormDesign.Design.ForeColor
				, Width
				, tab
				, ref h
				, draw);

			if (draw)
			{
				g.DrawLine(new Pen(FormDesign.Design.AccentColor, (float)Math.Ceiling(UI.FontScale * 1.5)), (int)(12 * UI.FontScale), h, (int)(12 * UI.FontScale), Height - 13);
			}

			if (draw && inf.Date != null)
			{
				var bnds = g.Measure($"on {inf.Date?.ToReadableString(inf.Date?.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)}", UI.Font(8.25F));
				g.DrawString($"on {inf.Date?.ToReadableString(inf.Date?.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)}",
					UI.Font(8.25F),
					new SolidBrush(FormDesign.Design.LabelColor),
					new Point((int)(9 * UI.FontScale) + versionSize.Width, (int)(h - bnds.Height - (4 * UI.FontScale))));
			}

			tab = 1;

			if (!string.IsNullOrWhiteSpace(inf.Tagline))
			{
				h += (int)(2 * UI.FontScale);

				g.DrawStringItem(LocaleHelper.GetGlobalText(inf.Tagline)
					 , UI.Font(8.25F, FontStyle.Italic)
					 , FormDesign.Design.ButtonForeColor
					 , Width
					 , tab
					 , ref h
					, draw);

				h += (int)(4 * UI.FontScale);
			}

			h += (int)(2 * UI.FontScale);

			foreach (var item in inf.ChangeGroups)
			{
				tab = 1;

				h += (int)(2 * UI.FontScale);

				g.DrawStringItem(LocaleHelper.GetGlobalText(item.Name)
					, UI.Font(9.75F, FontStyle.Bold)
					, FormDesign.Design.LabelColor
					, Width
					, tab
					, ref h
					, draw);

				tab++;

				foreach (var ch in item.Changes)
				{
					g.DrawStringItem("•  " + LocaleHelper.GetGlobalText(ch).One.Replace("\r\n", "\r\n    ")
						, UI.Font(8.25F)
						, FormDesign.Design.InfoColor
						, Width
						, tab
						, ref h
						, draw);
				}

				h += (int)(10 * UI.FontScale);
			}

			return h;
		}

		private void ChangeLogVersion_Resize(object sender, EventArgs e)
		{
			var ch = GetHeight();
			if (Height != ch)
			{
				Height = ch;
			}
		}
	}
}