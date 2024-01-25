using Extensions;

namespace SlickControls;

public class LocaleSlickUI : LocaleHelper
{
	private static readonly LocaleSlickUI _instance = new();

	public static void Load() { _ = _instance; }

	protected LocaleSlickUI() : base($"SlickControls.Properties.SlickUI.json") { }

	/// <summary>
	/// {0} ago
	/// </summary>
	public static Translation Ago => _instance.GetText("{0} ago");

	/// <summary>
	/// <para>{0} day</para>
	/// <para>Plural: {0} days</para>
	/// </summary>
	public static Translation Day => _instance.GetText("{0} day");

	/// <summary>
	/// <para>{0} day ago</para>
	/// <para>Plural: {0} days ago</para>
	/// </summary>
	public static Translation DayAgo => _instance.GetText("{0} day ago");

	/// <summary>
	/// <para>{0} hour</para>
	/// <para>Plural: {0} hours</para>
	/// </summary>
	public static Translation Hour => _instance.GetText("{0} hour");

	/// <summary>
	/// <para>{0} minute</para>
	/// <para>Plural: {0} minutes</para>
	/// </summary>
	public static Translation Minute => _instance.GetText("{0} minute");

	/// <summary>
	/// <para>{0} month</para>
	/// <para>Plural: {0} months</para>
	/// </summary>
	public static Translation Month => _instance.GetText("{0} month");

	/// <summary>
	/// <para>{0} month ago</para>
	/// <para>Plural: {0} months ago</para>
	/// </summary>
	public static Translation MonthAgo => _instance.GetText("{0} month ago");

	/// <summary>
	/// <para>{0} month ago on {1}</para>
	/// <para>Plural: {0} months ago on {1}</para>
	/// </summary>
	public static Translation MonthAgoOn => _instance.GetText("{0} month ago on {1}");

	/// <summary>
	/// <para>{0} second</para>
	/// <para>Plural: {0} seconds</para>
	/// </summary>
	public static Translation Second => _instance.GetText("{0} second");

	/// <summary>
	/// <para>{0} year</para>
	/// <para>Plural: {0} years</para>
	/// </summary>
	public static Translation Year => _instance.GetText("{0} year");

	/// <summary>
	/// {0}d
	/// </summary>
	public static Translation D => _instance.GetText("{0}d");

	/// <summary>
	/// {0}h
	/// </summary>
	public static Translation H => _instance.GetText("{0}h");

	/// <summary>
	/// {0}m
	/// </summary>
	public static Translation M => _instance.GetText("{0}m");

	/// <summary>
	/// {0}s
	/// </summary>
	public static Translation S => _instance.GetText("{0}s");

	/// <summary>
	/// Abort
	/// </summary>
	public static Translation Abort => _instance.GetText("Abort");

	/// <summary>
	/// Accent Color
	/// </summary>
	public static Translation AccentBackground => _instance.GetText("Accent Background");

	/// <summary>
	/// Actions
	/// </summary>
	public static Translation Actions => _instance.GetText("Actions");

	/// <summary>
	/// Active
	/// </summary>
	public static Translation Active => _instance.GetText("Active");

	/// <summary>
	/// Active Text
	/// </summary>
	public static Translation ActiveText => _instance.GetText("Active Text");

	/// <summary>
	/// After
	/// </summary>
	public static Translation After => _instance.GetText("After");

	/// <summary>
	/// All
	/// </summary>
	public static Translation All => _instance.GetText("All");

	/// <summary>
	/// All Versions
	/// </summary>
	public static Translation AllVersions => _instance.GetText("All Versions");

	/// <summary>
	/// Any Date
	/// </summary>
	public static Translation AnyDate => _instance.GetText("Any Date");

	/// <summary>
	/// Apply
	/// </summary>
	public static Translation Apply => _instance.GetText("Apply");

	/// <summary>
	/// Auto Night-Mode
	/// </summary>
	public static Translation AutoNightMode => _instance.GetText("Auto Night-Mode");

	/// <summary>
	/// Auto-Hide Menu
	/// </summary>
	public static Translation AutoHideMenu => _instance.GetText("Auto-Hide Menu");

	/// <summary>
	/// Automatically switches to a Light mode during the day and Dark mode during the night
	/// </summary>
	public static Translation AutomaticallySwitchesToALightModeDuringTheDayAndDarkModeDuringTheNight => _instance.GetText("Automatically switches to a Light mode during the day and Dark mode during the night");

	/// <summary>
	/// Back
	/// </summary>
	public static Translation Back => _instance.GetText("Back");

	/// <summary>
	/// Background
	/// </summary>
	public static Translation Background => _instance.GetText("Background");

	/// <summary>
	/// Before
	/// </summary>
	public static Translation Before => _instance.GetText("Before");

	/// <summary>
	/// Berry Blues
	/// </summary>
	public static Translation BerryBlues => _instance.GetText("Berry Blues");

	/// <summary>
	/// Between
	/// </summary>
	public static Translation Between => _instance.GetText("Between");

	/// <summary>
	/// Button
	/// </summary>
	public static Translation Button => _instance.GetText("Button");

	/// <summary>
	/// Button Text
	/// </summary>
	public static Translation ButtonText => _instance.GetText("Button Text");

	/// <summary>
	/// Cancel
	/// </summary>
	public static Translation Cancel => _instance.GetText("Cancel");

	/// <summary>
	/// Chic
	/// </summary>
	public static Translation Chic => _instance.GetText("Chic");

	/// <summary>
	/// Clear
	/// </summary>
	public static Translation Clear => _instance.GetText("Clear");

	/// <summary>
	/// Click on any color to change it, or middle-click to reset it.
	/// </summary>
	public static Translation ClickOnAnyColorToChangeItOrMiddleclickToResetIt => _instance.GetText("Click on any color to change it, or middle-click to reset it.");

	/// <summary>
	/// Close
	/// </summary>
	public static Translation Close => _instance.GetText("Close");

	/// <summary>
	/// Common Folders
	/// </summary>
	public static Translation CommonFolders => _instance.GetText("Common Folders");

	/// <summary>
	/// Confirmation
	/// </summary>
	public static Translation Confirmation => _instance.GetText("Confirmation");

	/// <summary>
	/// Content
	/// </summary>
	public static Translation Content => _instance.GetText("Content");

	/// <summary>
	/// Continue
	/// </summary>
	public static Translation Continue => _instance.GetText("Continue");

	/// <summary>
	/// Converts your theme into Light or Dark mode based on your computer's theme
	/// </summary>
	public static Translation ConvertsYourThemeIntoLightOrDarkModeBasedOnYourComputersTheme => _instance.GetText("Converts your theme into Light or Dark mode based on your computer's theme");

	/// <summary>
	/// Copy
	/// </summary>
	public static Translation Copy => _instance.GetText("Copy");

	/// <summary>
	/// Current Version
	/// </summary>
	public static Translation CurrentVersion => _instance.GetText("Current Version");

	/// <summary>
	/// Custom
	/// </summary>
	public static Translation Custom => _instance.GetText("Custom");

	/// <summary>
	/// Custom Theme
	/// </summary>
	public static Translation CustomTheme => _instance.GetText("Custom Theme");

	/// <summary>
	/// Dark
	/// </summary>
	public static Translation Dark => _instance.GetText("Dark");

	/// <summary>
	/// Date Created
	/// </summary>
	public static Translation DateCreated => _instance.GetText("Date Created");

	/// <summary>
	/// Date Modified
	/// </summary>
	public static Translation DateModified => _instance.GetText("Date Modified");

	/// <summary>
	/// Disable Animations
	/// </summary>
	public static Translation DisableAnimations => _instance.GetText("Disable Animations");

	/// <summary>
	/// Do you want to apply your changes before leaving?
	/// </summary>
	public static Translation DoYouWantToApplyYourChangesBeforeLeaving => _instance.GetText("Do you want to apply your changes before leaving?");

	/// <summary>
	/// Download
	/// </summary>
	public static Translation Download => _instance.GetText("Download");

	/// <summary>
	/// Drives
	/// </summary>
	public static Translation Drives => _instance.GetText("Drives");

	/// <summary>
	/// Edit Color
	/// </summary>
	public static Translation EditColor => _instance.GetText("Edit Color");

	/// <summary>
	/// Enable
	/// </summary>
	public static Translation Enable => _instance.GetText("Enable");

	/// <summary>
	/// Enable Animations
	/// </summary>
	public static Translation EnableAnimations => _instance.GetText("Enable Animations");

	/// <summary>
	/// End of Month
	/// </summary>
	public static Translation EndOfMonth => _instance.GetText("End of Month");

	/// <summary>
	/// End of Year
	/// </summary>
	public static Translation EndOfYear => _instance.GetText("End of Year");

	/// <summary>
	/// Filters
	/// </summary>
	public static Translation Filters => _instance.GetText("Filters");

	/// <summary>
	/// Font Family
	/// </summary>
	public static Translation FontFamily => _instance.GetText("Font Family");

	/// <summary>
	/// Go back to {0}
	/// </summary>
	public static Translation GoBackTo => _instance.GetText("Go back to {0}");

	/// <summary>
	/// Golden Forest
	/// </summary>
	public static Translation GoldenForest => _instance.GetText("Golden Forest");

	/// <summary>
	/// Green
	/// </summary>
	public static Translation Green => _instance.GetText("Green");

	/// <summary>
	/// Hue
	/// </summary>
	public static Translation Hue => _instance.GetText("Hue");

	/// <summary>
	/// Ice
	/// </summary>
	public static Translation Ice => _instance.GetText("Ice");

	/// <summary>
	/// Icon
	/// </summary>
	public static Translation Icon => _instance.GetText("Icon");

	/// <summary>
	/// Ignore
	/// </summary>
	public static Translation Ignore => _instance.GetText("Ignore");

	/// <summary>
	/// in {0}
	/// </summary>
	public static Translation In => _instance.GetText("in {0}");

	/// <summary>
	/// <para>in {0} day</para>
	/// <para>Plural: in {0} days</para>
	/// </summary>
	public static Translation InDay => _instance.GetText("in {0} day");

	/// <summary>
	/// <para>in {0} month on {1}</para>
	/// <para>Plural: in {0} months on {1}</para>
	/// </summary>
	public static Translation InMonthOn => _instance.GetText("in {0} month on {1}");

	/// <summary>
	/// In here, you can customize the scale and colors of the App to your liking.
	/// </summary>
	public static Translation InHereYouCanCustomizeTheScaleAndColorsOfTheAppToYourLiking => _instance.GetText("In here, you can customize the scale and colors of the App to your liking.");

	/// <summary>
	/// Info Text
	/// </summary>
	public static Translation InfoText => _instance.GetText("Info Text");

	/// <summary>
	/// Input
	/// </summary>
	public static Translation Input => _instance.GetText("Input");

	/// <summary>
	/// Label
	/// </summary>
	public static Translation Label => _instance.GetText("Label");

	/// <summary>
	/// Last {0} at {1}
	/// </summary>
	public static Translation LastAt => _instance.GetText("Last {0} at {1}");

	/// <summary>
	/// Loading..
	/// </summary>
	public static Translation Loading => _instance.GetText("Loading");

	/// <summary>
	/// Luminance
	/// </summary>
	public static Translation Luminance => _instance.GetText("Luminance");

	/// <summary>
	/// Maximize
	/// </summary>
	public static Translation Maximize => _instance.GetText("Maximize");

	/// <summary>
	/// Menu Background
	/// </summary>
	public static Translation MenuBackground => _instance.GetText("Menu Background");

	/// <summary>
	/// Menu Text
	/// </summary>
	public static Translation MenuText => _instance.GetText("Menu Text");

	/// <summary>
	/// Midnight
	/// </summary>
	public static Translation Midnight => _instance.GetText("Midnight");

	/// <summary>
	/// Minimize
	/// </summary>
	public static Translation Minimize => _instance.GetText("Minimize");

	/// <summary>
	/// Modern
	/// </summary>
	public static Translation Modern => _instance.GetText("Modern");

	/// <summary>
	/// More Info
	/// </summary>
	public static Translation MoreInfo => _instance.GetText("More Info");

	/// <summary>
	/// Name
	/// </summary>
	public static Translation Name => _instance.GetText("Name");

	/// <summary>
	/// Next {0} at {1}
	/// </summary>
	public static Translation NextAt => _instance.GetText("Next {0} at {1}");

	/// <summary>
	/// No
	/// </summary>
	public static Translation No => _instance.GetText("No");

	/// <summary>
	/// Ok
	/// </summary>
	public static Translation Ok => _instance.GetText("Ok");

	/// <summary>
	/// Open File
	/// </summary>
	public static Translation OpenFile => _instance.GetText("Open File");

	/// <summary>
	/// Options
	/// </summary>
	public static Translation Options => _instance.GetText("Options");

	/// <summary>
	/// Other
	/// </summary>
	public static Translation Other => _instance.GetText("Other");

	/// <summary>
	/// Pinned Folders
	/// </summary>
	public static Translation PinnedFolders => _instance.GetText("Pinned Folders");

	/// <summary>
	/// Profiles
	/// </summary>
	public static Translation Profiles => _instance.GetText("Profiles");

	/// <summary>
	/// Randomize
	/// </summary>
	public static Translation Randomize => _instance.GetText("Randomize");

	/// <summary>
	/// Red
	/// </summary>
	public static Translation Red => _instance.GetText("Red");

	/// <summary>
	/// Remove
	/// </summary>
	public static Translation Remove => _instance.GetText("Remove");

	/// <summary>
	/// Reset
	/// </summary>
	public static Translation Reset => _instance.GetText("Reset");

	/// <summary>
	/// Reset Color
	/// </summary>
	public static Translation ResetColor => _instance.GetText("Reset Color");

	/// <summary>
	/// Restore
	/// </summary>
	public static Translation Restore => _instance.GetText("Restore");

	/// <summary>
	/// Retry
	/// </summary>
	public static Translation Retry => _instance.GetText("Retry");

	/// <summary>
	/// Saturation
	/// </summary>
	public static Translation Saturation => _instance.GetText("Saturation");

	/// <summary>
	/// Search
	/// </summary>
	public static Translation Search => _instance.GetText("Search");

	/// <summary>
	/// Select a file
	/// </summary>
	public static Translation SelectAFile => _instance.GetText("Select a file");

	/// <summary>
	/// Select a folder
	/// </summary>
	public static Translation SelectAFolder => _instance.GetText("Select a folder");

	/// <summary>
	/// Settings
	/// </summary>
	public static Translation Settings => _instance.GetText("Settings");

	/// <summary>
	/// Size
	/// </summary>
	public static Translation Size => _instance.GetText("Size");

	/// <summary>
	/// Smaller Menu
	/// </summary>
	public static Translation SmallerMenu => _instance.GetText("Smaller Menu");

	/// <summary>
	/// Sorting
	/// </summary>
	public static Translation Sorting => _instance.GetText("Sorting");

	/// <summary>
	/// Start of Month
	/// </summary>
	public static Translation StartOfMonth => _instance.GetText("Start of Month");

	/// <summary>
	/// Start of Year
	/// </summary>
	public static Translation StartOfYear => _instance.GetText("Start of Year");

	/// <summary>
	/// Strawberries
	/// </summary>
	public static Translation Strawberries => _instance.GetText("Strawberries");

	/// <summary>
	/// Switch To
	/// </summary>
	public static Translation SwitchTo => _instance.GetText("Switch To");

	/// <summary>
	/// Switch to Grid-View
	/// </summary>
	public static Translation SwitchToGridView => _instance.GetText("Switch to Grid-View");

	/// <summary>
	/// Switch to List-View
	/// </summary>
	public static Translation SwitchToListView => _instance.GetText("Switch to List-View");

	/// <summary>
	/// Tags
	/// </summary>
	public static Translation Tags => _instance.GetText("Tags");

	/// <summary>
	/// Task Completed
	/// </summary>
	public static Translation TaskCompleted => _instance.GetText("TaskCompleted");

	/// <summary>
	/// Text
	/// </summary>
	public static Translation Text => _instance.GetText("Text");

	/// <summary>
	/// Theme Changer
	/// </summary>
	public static Translation ThemeChanger => _instance.GetText("Theme Changer");

	/// <summary>
	/// This PC
	/// </summary>
	public static Translation ThisPC => _instance.GetText("This PC");

	/// <summary>
	/// Today
	/// </summary>
	public static Translation Today => _instance.GetText("Today");

	/// <summary>
	/// Today at {0}
	/// </summary>
	public static Translation TodayAt => _instance.GetText("Today at {0}");

	/// <summary>
	/// Tomorrow
	/// </summary>
	public static Translation Tomorrow => _instance.GetText("Tomorrow");

	/// <summary>
	/// Tomorrow at {0}
	/// </summary>
	public static Translation TomorrowAt => _instance.GetText("Tomorrow at {0}");

	/// <summary>
	/// Total
	/// </summary>
	public static Translation Total => _instance.GetText("Total");

	/// <summary>
	/// UI Scale
	/// </summary>
	public static Translation UIScale => _instance.GetText("UI Scale");

	/// <summary>
	/// An unexpected error occurred
	/// </summary>
	public static Translation UnexpectedError => _instance.GetText("UnexpectedError");

	/// <summary>
	/// Use your System's Theme
	/// </summary>
	public static Translation UseTheSystemsTheme => _instance.GetText("Use the System's Theme");

	/// <summary>
	/// Use Windows-style top bar buttons
	/// </summary>
	public static Translation UseWindowsstyleTopBarButtons => _instance.GetText("Use Windows-style top bar buttons");

	/// <summary>
	/// User Interface
	/// </summary>
	public static Translation UserInterface => _instance.GetText("User Interface");

	/// <summary>
	/// Utilities
	/// </summary>
	public static Translation Utilities => _instance.GetText("Utilities");

	/// <summary>
	/// Welcome to Theme Changer!
	/// </summary>
	public static Translation WelcomeToThemeChanger => _instance.GetText("Welcome to Theme Changer!");

	/// <summary>
	/// Yellow
	/// </summary>
	public static Translation Yellow => _instance.GetText("Yellow");

	/// <summary>
	/// Yes
	/// </summary>
	public static Translation Yes => _instance.GetText("Yes");

	/// <summary>
	/// Yesterday
	/// </summary>
	public static Translation Yesterday => _instance.GetText("Yesterday");

	/// <summary>
	/// Yesterday at {0}
	/// </summary>
	public static Translation YesterdayAt => _instance.GetText("Yesterday at {0}");
}
