using System;

namespace SlickControls;

[Flags]
public enum HoverState
{
	Normal = 1,
	Hovered = 2,
	Pressed = 4,
	Focused = 8,
};