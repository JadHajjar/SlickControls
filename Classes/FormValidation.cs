using System.Windows.Forms;

namespace SlickControls;

public static class FormValidation
{
	public static bool CheckValidation(this Control ctrl)
	{
		var b = true;

		if (ctrl is IValidationControl tb)
		{
			if (tb.Required && tb.Visible)
			{
				b = tb.ValidInput;
			}

			if (!b)
			{
				tb.SetError();
			}
		}

		foreach (Control item in ctrl.Controls)
		{
			b &= CheckValidation(item);
		}

		return b;
	}

	public static void ClearForm(this Control ctrl)
	{
		if (ctrl is ISupportsReset sr)
		{
			sr.ResetValue();
		}

		foreach (Control item in ctrl.Controls)
		{
			ClearForm(item);
		}
	}

	public static void OnFormChanged(this Control ctrl, Extensions.ExtensionClass.action action)
	{
		if (ctrl is SlickTextBox tb)
		{
			tb.TextChanged += (s, e) => action();
		}
		else if (ctrl is SlickCheckbox cb)
		{
			cb.CheckChanged += (s, e) => action();
		}
		else if (ctrl is SlickRadioButton rb)
		{
			rb.CheckChanged += (s, e) => action();
		}
		else
		{
			foreach (Control item in ctrl.Controls)
			{
				OnFormChanged(item, action);
			}
		}
	}
}