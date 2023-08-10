using Extensions;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class ChangeLogVersion : SlickControl
	{
		private readonly VersionChangeLog _changelog;

		public ChangeLogVersion(VersionChangeLog changelog)
		{
			InitializeComponent();
			_changelog = changelog;
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
			using (var font1 = UI.Font(15.5F, FontStyle.Bold))
			using (var font2 = UI.Font(8.25F))
			using (var font3 = UI.Font(8.25F, FontStyle.Italic))
			using (var font4 = UI.Font(9.75F, FontStyle.Bold))
			{
				var versionSize = g.DrawStringItem("v" + _changelog.Version
					, font1
					, FormDesign.Design.ForeColor
					, Width
					, tab
					, ref h
					, draw);

				if (draw)
				{
					using (var pen = new Pen(FormDesign.Design.AccentColor, (float)Math.Ceiling(UI.FontScale * 1.5)))
					{
						g.DrawLine(pen, (int)(12 * UI.FontScale), h, (int)(12 * UI.FontScale), Height - 13);
					}
				}

				if (draw && _changelog.Date != null)
				{
					var bnds = g.Measure($"on {_changelog.Date?.ToReadableString(_changelog.Date?.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)}", font2);

					using (var brush = new SolidBrush(FormDesign.Design.LabelColor))
					{
						g.DrawString($"on {_changelog.Date?.ToReadableString(_changelog.Date?.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)}",
							font2,
							brush,
							new Point((int)(9 * UI.FontScale) + versionSize.Width, (int)((h - 4 * UI.FontScale - bnds.Height) / 2 + 4 * UI.FontScale)));
					}
				}

				tab = 1;

				if (!string.IsNullOrWhiteSpace(_changelog.Tagline))
				{
					h += (int)(2 * UI.FontScale);

					g.DrawStringItem(LocaleHelper.GetGlobalText(_changelog.Tagline)
						 , font3
						 , FormDesign.Design.ButtonForeColor
						 , Width
						 , tab
						 , ref h
						, draw);

					h += (int)(4 * UI.FontScale);
				}

				h += (int)(2 * UI.FontScale);

				foreach (var item in _changelog.ChangeGroups)
				{
					tab = 1;

					h += (int)(2 * UI.FontScale);

					g.DrawStringItem(LocaleHelper.GetGlobalText(item.Name)
						, font4
						, FormDesign.Design.LabelColor
						, Width
						, tab
						, ref h
						, draw);

					tab++;

					foreach (var ch in item.Changes)
					{
						g.DrawStringItem("•  " + LocaleHelper.GetGlobalText(ch).One.Replace("\r\n", "\r\n    ")
							, font2
							, FormDesign.Design.InfoColor
							, Width
							, tab
							, ref h
							, draw);
					}

					h += (int)(10 * UI.FontScale);
				}
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