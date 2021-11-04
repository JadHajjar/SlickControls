using Extensions;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlickControls
{
	public class VersionInfo
	{
		public VersionInfo(Version v) => Version = v;

		public List<VersionDescription> Descriptions { get; set; } = new List<VersionDescription>();
		public Version Version { get; private set; }

		public static List<VersionInfo> GenerateInfo(IEnumerable<string> changelog)
		{
			Version ver = null;
			var info = new List<string>();
			var vers = new List<VersionInfo>();

			foreach (var item in changelog)
			{
				if (Regex.IsMatch(item, "V \\d+?(\\.\\d+?)+", RegexOptions.IgnoreCase))
				{
					if (ver != null)
						vers.Add(getVerInf(ver, info));

					ver = new Version(item.Substring(2));
					info = new List<string>();
				}
				else
				{
					info.Add(item);
				}
			}

			if (ver != null)
				vers.Add(getVerInf(ver, info));

			vers.Reverse();

			return vers;
		}

		private static VersionInfo getVerInf(Version ver, List<string> info)
		{
			var versionInf = new VersionInfo(ver);
			var title = string.Empty;
			var desc = new List<string>();

			for (var i = 0; i < info.Count; i++)
			{
				var line = info[i];

				if (!string.IsNullOrWhiteSpace(line) && line[0] != ' ')
				{
					if (title != string.Empty)
						versionInf.Descriptions.Add(new VersionDescription(title, desc.Trim(string.IsNullOrWhiteSpace)));

					title = line;
					desc = new List<string>();
				}
				else
				{
					desc.Add(line);
				}
			}

			if (title != string.Empty)
				versionInf.Descriptions.Add(new VersionDescription(title, desc.Trim(string.IsNullOrWhiteSpace)));

			return versionInf;
		}
	}

	public class VersionDescription
	{
		public VersionDescription(string title, IEnumerable<string> info)
		{
			Title = title;
			Info = info;
		}

		public IEnumerable<string> Info { get; set; }
		public string Title { get; set; }
	}
}