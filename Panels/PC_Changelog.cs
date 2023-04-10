using Extensions;

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class PC_Changelog : PanelContent
	{
		private readonly VersionChangeLog Current;
		private readonly VersionChangeLog[] ChangeLogs;
		private readonly PanelItemControl base_P_Tabs;

		public PC_Changelog(Assembly assembly, string resourceName, Version currentVersion)
		{
			InitializeComponent();

			base_P_Tabs = new PanelItemControl(null) { Dock = DockStyle.Fill };
			base_TLP_Side.Controls.Add(base_P_Tabs);

			using (var stream = assembly.GetManifestResourceStream(resourceName))
			using (var reader = new StreamReader(stream))
			{
				ChangeLogs = Newtonsoft.Json.JsonConvert.DeserializeObject<VersionChangeLog[]>(reader.ReadToEnd());
			}

			Current = ChangeLogs.FirstOrDefault(x => x.Version == currentVersion);

			if (Current != null)
			{
#if DEBUG
				Clipboard.SetText(Current.VersionString + "\r\n\r\n" + Current.ChangeGroups.ListStrings(x => $"{x.Name}\r\n{x.Changes.ListStrings(y => $"  • {y}", "\r\n")}", "\r\n"));
#endif
				base_P_Tabs.Add(PanelTab.GroupName("Current Version"));
				AddVersion(Current, $"v{Current.Version}");
			}

			base_P_Tabs.Add(PanelTab.Separator());
			base_P_Tabs.Add(PanelTab.GroupName("All Versions"));

			foreach (var grp in ChangeLogs
				.Where(x => Current == null || x != Current)
				.Distinct((x, y) => x.Version.Major == y.Version.Major && x.Version.Minor == y.Version.Minor)
				.OrderByDescending(x => x.Version)
				.GroupBy(x => x.Version.Major))
			{
				foreach (var item in grp)
				{
					AddVersion(item);
				}
			}
		}

		public override bool CanExit(bool toBeDisposed)
		{
			Form.base_TLP_Side.TopRight = false;
			Form.base_TLP_Side.BotRight = false;
			Form.base_TLP_Side.Invalidate();

			return base.CanExit(toBeDisposed);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (Form != null)
			{
				Form.base_TLP_Side.TopRight = true;
				Form.base_TLP_Side.BotRight = true;
				Form.base_TLP_Side.Invalidate();
			}
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			base_TLP_Side.BackColor = design.MenuColor;
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			base_P_Side.Width = (int)(140 * UI.FontScale);
			base_TLP_Side.Padding = UI.Scale(new Padding(5), UI.FontScale);
			base_P_Side.Padding = UI.Scale(new Padding(0, 5, 5, 5), UI.FontScale);

			this.TryBeginInvoke(() =>
			{
				P_All.Padding = new Padding(0, base_Text.Height + base_Text.Top, 0, 0);
				LabelBounds = new Point(P_All.Left, LabelBounds.Y);
			});
		}

		private void AddVersion(VersionChangeLog versionInfo, string text = null)
		{
			var M = versionInfo.Version.Major;
			var m = versionInfo.Version.Minor;
			var vers = ChangeLogs.Where(x => x.Version.Major == M && x.Version.Minor == m);

			var tab = new PanelTab(new PanelItem()
			{
				Text = text.IfNull(vers.Count() == 1 ? $"v {versionInfo.Version}" : $"v {M}.{m}.{vers.Min(x => x.Version.Build)} → {M}.{m}.{vers.Max(x => x.Version.Build)}"),
				Data = text != null ? null : versionInfo
			});

			tab.PanelItem.OnClick += Tile_Click;

			base_P_Tabs.Add(tab);

			if (text != null)
			{
				Tile_Click(tab.PanelItem, null);
			}
		}

		private void Tile_Click(object sender, MouseEventArgs e)
		{
			var inf = (VersionChangeLog)(sender as PanelItem).Data;

			P_VersionInfo.SuspendDrawing();
			P_VersionInfo.Controls.Clear();
			if (inf == null)
			{
				if (Current != null)
				{
					P_VersionInfo.Controls.Add(new ChangeLogVersion(Current));
				}
			}
			else
			{
				foreach (var item in ChangeLogs.Where(x => x.Version.Major == inf.Version.Major && x.Version.Minor == inf.Version.Minor).OrderBy(x => x.Version))
				{
					P_VersionInfo.Controls.Add(new ChangeLogVersion(item));
				}
			}

			base_P_Tabs.Items.Where(x => x.PanelItem != null).Foreach(x => x.PanelItem.Selected = x.PanelItem == sender);
			base_P_Tabs.Invalidate();

			P_VersionInfo.ResumeDrawing();
		}
	}
}