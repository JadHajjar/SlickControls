using System.Collections.Generic;
using System.Drawing;

namespace SlickControls;

public class SlickStripItem
{
	public delegate void action();

	public string Text { get; set; }
	public DynamicIcon Image { get; set; }
	public action Action { get; set; }
	public bool Disabled { get; set; }
	public bool CloseOnClick { get; set; }
	public bool Visible { get; }
	public List<SlickStripItem> SubItems { get; set; }

	public bool IsEmpty => string.IsNullOrWhiteSpace(Text) && Image == null;

	internal bool IsOpened;
	internal bool IsFocused;
	internal Rectangle Rectangle;
	internal SlickStripItem Parent;

	public SlickStripItem(bool visible = true)
	{
		Text = string.Empty;
		Visible = visible;
	}

	public SlickStripItem(string text, action action, bool disabled = false, bool closeOnClick = true, bool visible = true)
	{
		Text = text;
		Action = action;
		Disabled = disabled;
		CloseOnClick = closeOnClick;
		Visible = visible;
	}

	public SlickStripItem(string text, DynamicIcon icon, bool disabled = false, bool closeOnClick = true, bool visible = true)
	{
		Text = text;
		Image = icon;
		Disabled = disabled;
		CloseOnClick = closeOnClick;
		Visible = visible;
	}

	public SlickStripItem(string text, DynamicIcon icon, action action, bool disabled = false, bool closeOnClick = true, bool visible = true)
	{
		Text = text;
		Image = icon;
		Action = action;
		Disabled = disabled;
		CloseOnClick = closeOnClick;
		Visible = visible;
	}

	public static SlickStripItem Empty => new();
}