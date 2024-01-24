using Extensions;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

public class Notification
{
	public static bool PlaySounds { get; set; } = true;

	public string Title { get; }
	public string Description { get; set; }
	public PromptIcons Icon { get; }
	public ExtensionClass.action Action { get; }
	public Action<Control, Graphics> OnPaint { get; set; }
	public Size Size { get; }
	public NotificationSound Sound { get; }

	private Notification(string title, string description, PromptIcons icon, ExtensionClass.action action, NotificationSound sound, Size? size)
	{
		Title = title;
		Description = description;
		Icon = icon;
		Action = action;
		Sound = sound;
		Size = size ?? new Size(350, 75);
	}

	public NotificationForm Show(Form form, int? timeoutSeconds = null)
	{
		return NotificationForm.Push(this, form, Sound, timeoutSeconds);
	}

	public static Notification Create(string title, string description, PromptIcons icon, ExtensionClass.action action, NotificationSound sound = NotificationSound.Short, Size? size = null)
	{
		return new Notification(title, description, icon, action, sound, size);
	}

	public static Notification Create(Action<Control, Graphics> onpaint, ExtensionClass.action action, NotificationSound sound = NotificationSound.Short, Size? size = null)
	{
		return new Notification(string.Empty, string.Empty, PromptIcons.Input, action, sound, size) { OnPaint = onpaint };
	}

	public static void Clear()
	{
		NotificationForm.Clear();
	}
}