using Extensions;

namespace SlickControls;
[SaveName("Settings.tf", "SlickUI")]
public class SlickUISettings : ISaveObject
{
	public SaveHandler Handler { get; set; }
	public bool TutorialShown { get; set; }
	public bool AutoHideMenu { get; set; }
	public bool SmallMenu { get; set; }
}
