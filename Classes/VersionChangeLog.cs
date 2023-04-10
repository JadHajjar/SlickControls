using Newtonsoft.Json;

using System;

namespace SlickControls
{
	public class VersionChangeLog
	{
		[JsonIgnore] public Version Version => new Version(VersionString);

		[JsonProperty("Version")] public string VersionString { get; set; }
		public DateTime? Date { get; set; }
		public string Tagline { get; set; }
		public VersionChangeLogGroup[] ChangeGroups { get; set; }
	}
}