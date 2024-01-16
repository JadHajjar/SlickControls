using Extensions;

using System;
using System.Drawing;

namespace MechanikaDesign.WinForms.UI.ColorPicker;

public class ColorChangedEventArgs : EventArgs
{
	private HslColor selectedHslColor;

	public ColorChangedEventArgs(Color selectedColor)
	{
		SelectedColor = selectedColor;
		selectedHslColor = HslColor.FromColor(selectedColor);
	}

	public ColorChangedEventArgs(HslColor selectedHslColor)
	{
		SelectedColor = selectedHslColor.RgbValue;
		this.selectedHslColor = selectedHslColor;
	}

	public Color SelectedColor { get; }

	public HslColor SelectedHslColor => selectedHslColor;
}