using Extensions;

using System.Drawing;

namespace SlickControls;

public interface ISlickForm
{
	bool CloseForm { get; set; }
	Image FormIcon { get; set; }
	Rectangle IconBounds { get; set; }
	FormState CurrentFormState { get; set; }
}