using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class ChangeLogForm : BaseForm
	{
		private readonly IEnumerable<VersionInfo> VerInfo;
		private readonly VersionInfo Current;

		public ChangeLogForm(IEnumerable<string> changelog, string currentVersion)
		{
			InitializeComponent();
			VerInfo = VersionInfo.GenerateInfo(changelog);
			var s = Newtonsoft.Json.JsonConvert.SerializeObject(VerInfo.Select(x =>
			new VersionChangeLog()
			{
				VersionString = x.Version.ToString(),
				Date = null,
				Tagline = x.Descriptions.FirstOrDefault(y => !y.Info.Any())?.Title ?? string.Empty,
				ChangeGroups = x.Descriptions.Where(y => y.Info.Any()).Select(z => new VersionChangeLogGroup()
				{
					Changes = z.Info.Select(ds => ds.Remove("  •  ")).Where(h => !string.IsNullOrWhiteSpace(h)).ToArray(),
					Name = z.Title
				}).ToArray()
			}));
			Current = VerInfo.FirstOrDefault(x => x.Version.ToString() == currentVersion);

			foreach (var item in VerInfo.Distinct((x, y) => x.Version.Major == y.Version.Major && x.Version.Minor == y.Version.Minor))
				AddVersion(item);

			if (Current != null)
				AddVersion(VerInfo.Last(), "Latest Version");

			DesignChanged(FormDesign.Design);
		}

		private void AddVersion(VersionInfo versionInfo, string text = null)
		{
			var st = new SlickTile()
			{
				Dock = DockStyle.Top,
				DrawLeft = false,
				Font = UI.Font(9.75F, FontStyle.Bold | FontStyle.Italic),
				IconSize = 14,
				Image = Properties.Resources.ArrowRight,
				Padding = new Padding(10),
				Size = new Size(175, 35),
				TabStop = false,
				Text = text.IfNull($"Versions {versionInfo.Version.Major}.{versionInfo.Version.Minor}.x"),
				Selected = text != null,
				Tag = text != null ? null : versionInfo
			};

			st.Click += Tile_Click;

			P_LeftTabs.Controls.Add(st);

			if (text != null)
				Tile_Click(st, null);
		}

		private void Tile_Click(object sender, EventArgs e)
		{
			var inf = (VersionInfo)(sender as Control).Tag;

			P_VersionInfo.SuspendDrawing();
			P_VersionInfo.Controls.Clear();
			//if (inf == null)
			//{
			//	if (Current != null)
			//		P_VersionInfo.Controls.Add(new ChangeLogVersion(Current));
			//}
			//else
			//{
			//	foreach (var item in VerInfo.Where(x => x.Version.Major == inf.Version.Major && x.Version.Minor == inf.Version.Minor))
			//		P_VersionInfo.Controls.Add(new ChangeLogVersion(item));
			//}
			P_LeftTabs.Controls.ThatAre<SlickTile>().Foreach(x => x.Selected = x == sender);
			P_VersionInfo.ResumeDrawing();
		}

		private void SetTooltips() => toolTip.AdvancedSetTooltip(B_Done, "Dismiss this window");

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Enter)
			{ base_B_Close_Click(this, new EventArgs()); return true; }
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void B_Done_Click(object sender, EventArgs e) => Close();

		private void P_Spacer_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(FormDesign.Design.BackColor);
			e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.ActiveColor), 0, 0, 1, P_Spacer.Height - 150);

			e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(
				new RectangleF(0, P_Spacer.Height - 175, 1, 150),
				FormDesign.Design.ActiveColor,
				FormDesign.Design.BackColor,
				90),
				new RectangleF(0, P_Spacer.Height - 175, 1, 150));
		}

		private void panel2_Resize(object sender, EventArgs e)
		{
			P_VersionInfo.MaximumSize = new Size(panel2.Width, 9999);
			P_VersionInfo.MinimumSize = new Size(panel2.Width, 0);
		}

		private void panel1_Resize(object sender, EventArgs e)
		{
			P_LeftTabs.MaximumSize = new Size(panel1.Width, 9999);
			P_LeftTabs.MinimumSize = new Size(panel1.Width, 0);
		}
	}
}