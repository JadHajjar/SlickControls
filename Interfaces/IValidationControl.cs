namespace SlickControls;

public interface IValidationControl
{
	bool ValidInput { get; }
	bool Required { get; set; }
	bool Visible { get; set; }

	void SetError(bool warning = false);
}