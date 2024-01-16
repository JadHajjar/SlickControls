using System.Windows.Forms;

namespace SlickControls;

public class InputResult
{
	public DialogResult DialogResult;
	public string Input;

	public InputResult(DialogResult dialogResult, string input)
	{
		DialogResult = dialogResult;
		Input = input;
	}
}