using Extensions;

using System;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls;

public partial class MessagePrompt : SlickForm
{
	private Func<string, bool> InputValidation;
	private readonly PromptButtons selectedButtons;
	private readonly PromptIcons selectedIcon;
	private readonly bool isInput;

	private MessagePrompt()
	{
		InitializeComponent();
	}

	private MessagePrompt(string title, string msg, string details, PromptButtons buttons, PromptIcons icons, bool input)
	{
		InitializeComponent();

		Text = title.IfEmpty("Prompt");
		L_Title.Text = title;
		L_Text.Text = msg;
		L_Details.Text = details;
		L_Title.Font = UI.Font(13.5F, FontStyle.Bold);
		L_Text.Font = UI.Font(9F);

		selectedButtons = buttons;
		selectedIcon = icons;
		isInput = input;

		if (string.IsNullOrEmpty(details))
		{
			B_Details.Parent = null;
			TLP_Main.SetColumn(FLP_Buttons, 0);
			TLP_Main.SetColumnSpan(FLP_Buttons, 2);
		}

		if (string.IsNullOrEmpty(title))
		{
			L_Text.Font = UI.Font(10.5F);
			L_Title.Parent = null;
		}

		SetIcon();
		SetButtons();

		PB_Icon.MouseDown += Form_MouseDown;
		L_Text.MouseDown += Form_MouseDown;
		TLP_ImgText.MouseDown += Form_MouseDown;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		PB_Icon.Margin = TB_Input.Margin = UI.Scale(new Padding(20));
		P_Spacer_1.Margin = UI.Scale(new Padding(10, 5, 10, 5));
		L_Title.Margin = UI.Scale(new Padding(5, 15, 5, 5));
		FLP_Buttons.Padding = UI.Scale(new Padding(0, 0, 5, 5));
		B_Details.Margin = UI.Scale(new Padding(5));

		foreach (Control item in FLP_Buttons.Controls)
		{
			item.Margin = UI.Scale(new Padding(5));
		}
	}

	public string OutputText { get; private set; } = string.Empty;

	public override FormState CurrentFormState
	{
		get => base.CurrentFormState;
		set { }
	}

	private void MessagePrompt_Load(object sender, EventArgs e)
	{
		PlaySound();

		if (TB_Input.Visible)
		{
			BeginInvoke(new Action(() =>
			{
				TB_Input.Focus();
				TB_Input.SelectAll();
			}));
		}
		else
		{
			BeginInvoke(new Action(() =>
			{
				switch (selectedButtons)
				{
					case PromptButtons.OK:
					case PromptButtons.OKCancel:
					case PromptButtons.OKIgnore:
						B_OK.Focus();
						break;

					case PromptButtons.AbortRetryIgnore:
						B_Abort.Focus();
						break;

					case PromptButtons.YesNoCancel:
					case PromptButtons.YesNo:
						B_Yes.Focus();
						break;

					case PromptButtons.RetryCancel:
						B_Retry.Focus();
						break;
				}
			}));
		}
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
	}

	protected override void OnCreateControl()
	{
		Opacity = 0;
		var w = (int)((L_Details.Text.Length != 0 || L_Title.Text.Length != 0 ? 340 : 280) * UI.FontScale);
		var h = 0;
		var screen = Screen.FromHandle(Handle);

		using (var g = Graphics.FromHwnd(IntPtr.Zero))
		{
			var heightDiff = Height - L_Text.Height;
			var widthDiff = Width - L_Text.Width;
			var heightLimit = false;

			while (true)
			{
				if (!heightLimit)
				{
					h = heightDiff + Math.Max(80, (int)(1.05F * g.Measure(L_Text.Text, L_Text.Font, w).Height));
				}

				if (h > screen.WorkingArea.Height * 95 / 100)
				{
					heightLimit = true;

					h = screen.WorkingArea.Height * 95 / 100;

					w += (int)(20 * UI.UIScale);
				}
				else if (w + widthDiff > screen.WorkingArea.Width * 95 / 100)
				{
					h = Math.Min(h, screen.WorkingArea.Height * 95 / 100);
					w = Math.Min(w, screen.WorkingArea.Width * 95 / 100);

					break;
				}
				else
				{
					break;
				}
			}

			w += widthDiff;
		}

		LastUiScale = UI.UIScale;
		Size = new Size(w, h);

		if (isInput)
		{
			TB_Input.Visible = true;

			if(!string.IsNullOrWhiteSpace(L_Text.Text))
			Height += TB_Input.Height + TB_Input.Margin.Vertical;
		}

		if (Owner != null)
		{
			Location = Owner.Bounds.Center(Size);
			TopMost = false;
		}
		else
		{
			Location = Screen.PrimaryScreen.Bounds.Center(Size);
		}

		base.OnCreateControl();
	}

	#region Statics

	public static DialogResult Show(
		  Exception exception
		, PromptButtons buttons = PromptButtons.OK
		, PromptIcons icon = PromptIcons.Error
		, SlickForm form = null)
	{
		return Show(exception, string.Empty, string.Empty, buttons, icon, form);
	}

	public static DialogResult Show(
		  Exception exception
		, string message
		, PromptButtons buttons = PromptButtons.OK
		, PromptIcons icon = PromptIcons.Error
		, SlickForm form = null)
	{
		return Show(exception, message, string.Empty, buttons, icon, form);
	}

	public static DialogResult Show(
		  string message
		, PromptButtons buttons = PromptButtons.OK
		, PromptIcons icon = PromptIcons.None
		, SlickForm form = null)
	{
		return Show(null, message, string.Empty, buttons, icon, form);
	}

	public static DialogResult Show(
		  string message
		, string title
		, PromptButtons buttons = PromptButtons.OK
		, PromptIcons icon = PromptIcons.None
		, SlickForm form = null)
	{
		return Show(null, message, title, buttons, icon, form);
	}

	public static DialogResult Show(
		  Exception exception
		, string message
		, string title
		, PromptButtons buttons = PromptButtons.OK
		, PromptIcons icon = PromptIcons.Error
		, SlickForm form = null)
	{
		var @out = DialogResult.OK;

		WinExtensionClass.TryInvoke(form, () =>
		{
			GetError(exception, message, out var newMessage, out var details);

			var prompt = new MessagePrompt(title, newMessage, details, buttons, icon, false);

			if (form != null)
			{
				form.CurrentFormState = FormState.ForcedFocused;
				form.ShowUp();
			}
			else
			{
				prompt.StartPosition = FormStartPosition.CenterScreen;
			}

			try
			{
				@out = prompt.ShowDialog(form);
			}
			finally
			{
				form?.OnNextIdle(() =>
					{
						form.ShowUp();
						form.CurrentFormState = FormState.NormalFocused;
					});
			}
		});

		return @out;
	}

	public static InputResult ShowInput(
		  string message
		, string title = "Input Prompt"
		, string defaultValue = ""
		, PromptButtons buttons = PromptButtons.OKCancel
		, PromptIcons icon = PromptIcons.Input
		, Func<string, bool> inputValidation = null
		, SlickForm form = null)
	{
		InputResult @out = null;

		WinExtensionClass.TryInvoke(form, () =>
		{
			var prompt = new MessagePrompt(title, message, string.Empty, buttons, icon, true);
			prompt.TB_Input.Text = defaultValue;
			prompt.InputValidation = inputValidation;

			if (form != null)
			{
				form.CurrentFormState = FormState.ForcedFocused;
				form.ShowUp();
			}
			else
			{
				prompt.StartPosition = FormStartPosition.CenterScreen;
			}

			try
			{
				@out = new InputResult(prompt.ShowDialog(form), prompt.OutputText);
			}
			finally
			{
				form?.OnNextIdle(() =>
					{
						form.ShowUp();
						form.CurrentFormState = FormState.NormalFocused;
					});
			}
		});

		return @out;
	}

	public static void GetError(Exception exception, string text, out string message, out string details)
	{
		if (exception == null)
		{
			message = text;
			details = string.Empty;
			return;
		}

		if (string.IsNullOrEmpty(text))
		{
			message = string.Empty;
		}
		else
		{
			message = text + ":";
		}

		details = message;

		if (exception is SEHException)
		{
			message += "\r\n\r\n" + LocaleHelper.GetGlobalText("Something outside of this app may have malfunctioned. You need to close it entirely to fix this issue.");
			details += "\r\n\r\n" + LocaleHelper.GetGlobalText("Something outside of this app may have malfunctioned. You need to close it entirely to fix this issue.");
			return;
		}

		while (true)
		{
			message += $"\r\n\r\n{exception.Message}\r\n\r\n";
			details += $"\r\n\r\n{exception.Message}\r\n\r\n";
			details += $"{exception.GetType().Name.FormatWords().Replace("Exception", "Error")} ";
			details += exception.StackTrace?
								.Split('\n')
								.Select(y => y
									.RegexReplace(" in .+:line", " on line")?
									.RegexRemove(" in .+")?
									.RegexReplace(@"\.<>.+?<(.+?)>.+?\(", x => $".{x.Groups[1].Value}(")?
									.RegexReplace(@"\.<(.+?)>[\w_`]+([\.\(])", x => $".{x.Groups[1].Value}{x.Groups[2].Value}")?
									.Trim())
								.ListStrings("\r\n");

			if (exception.InnerException != null)
			{
				exception = exception.InnerException;
			}
			else
			{
				break;
			}
		}
	}

	#endregion Statics

	#region General Methods

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Enter)
		{
			switch (selectedButtons)
			{
				case PromptButtons.OK:
				case PromptButtons.OKCancel:
				case PromptButtons.OKIgnore:
					B_OK_Click(this, new EventArgs());
					break;

				case PromptButtons.AbortRetryIgnore:
					B_Abort_Click(this, new EventArgs());
					break;

				case PromptButtons.YesNoCancel:
					B_Yes_Click(this, new EventArgs());
					break;

				case PromptButtons.YesNo:
					B_Yes_Click(this, new EventArgs());
					break;

				case PromptButtons.RetryCancel:
					B_Retry_Click(this, new EventArgs());
					break;

				default:
					break;
			}

			return true;
		}

		if (keyData == Keys.Escape)
		{
			switch (selectedButtons)
			{
				case PromptButtons.OK:
				case PromptButtons.OKIgnore:
					B_OK_Click(this, new EventArgs());
					break;

				case PromptButtons.OKCancel:
					B_Cancel_Click(this, new EventArgs());
					break;

				case PromptButtons.AbortRetryIgnore:
					B_Ignore_Click(this, new EventArgs());
					break;

				case PromptButtons.YesNoCancel:
					B_Cancel_Click(this, new EventArgs());
					break;

				case PromptButtons.YesNo:
					B_No_Click(this, new EventArgs());
					break;

				case PromptButtons.RetryCancel:
					B_Cancel_Click(this, new EventArgs());
					break;

				default:
					break;
			}

			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);
		base_P_Content.BackColor = design.BackColor;
		L_Details.BackColor = design.BackColor;
		L_Details.ForeColor = design.ForeColor;
		P_Spacer_1.BackColor = design.AccentColor;
	}

	private void PlaySound()
	{
		switch (selectedIcon)
		{
			case PromptIcons.Hand:
				SystemSounds.Hand.Play();
				break;

			case PromptIcons.Info:
			case PromptIcons.Ok:
				SystemSounds.Beep.Play();
				break;

			case PromptIcons.Input:
				SystemSounds.Question.Play();
				break;

			case PromptIcons.Question:
				SystemSounds.Question.Play();
				break;

			case PromptIcons.Warning:
				SystemSounds.Asterisk.Play();
				break;

			case PromptIcons.Error:
				SystemSounds.Exclamation.Play();
				break;

			default:
				break;
		}
	}

	private void SetButtons()
	{
		switch (selectedButtons)
		{
			case PromptButtons.OK:
				B_OK.Visible = true;
				break;

			case PromptButtons.OKCancel:
				B_OK.Visible = B_Cancel.Visible = true;
				B_Cancel.ColorStyle = ColorStyle.Yellow;
				B_OK.ButtonType = ButtonType.Active;
				break;

			case PromptButtons.OKIgnore:
				B_OK.Visible = B_Ignore.Visible = true;
				B_Ignore.ColorStyle = ColorStyle.Yellow;
				B_OK.ButtonType = ButtonType.Active;
				break;

			case PromptButtons.AbortRetryIgnore:
				B_Abort.Visible = B_Retry.Visible = B_Ignore.Visible = true;
				B_Abort.ColorStyle = ColorStyle.Red;
				B_Retry.ColorStyle = ColorStyle.Yellow;
				B_Ignore.ColorStyle = ColorStyle.Green;
				break;

			case PromptButtons.YesNoCancel:
				B_Yes.Visible = B_No.Visible = B_Cancel.Visible = true;
				B_No.ColorStyle = ColorStyle.Red;
				B_Cancel.ColorStyle = ColorStyle.Yellow;
				B_Yes.ColorStyle = ColorStyle.Green;
				break;

			case PromptButtons.YesNo:
				B_Yes.Visible = B_No.Visible = true;
				B_No.ColorStyle = ColorStyle.Red;
				B_Yes.ColorStyle = ColorStyle.Green;
				break;

			case PromptButtons.RetryCancel:
				B_Retry.Visible = B_Cancel.Visible = true;
				B_Cancel.ColorStyle = ColorStyle.Red;
				B_Retry.ColorStyle = ColorStyle.Green;
				break;

			case PromptButtons.None:
				break;

			default:
				B_OK.Visible = true;
				break;
		}
	}

	private void SetIcon()
	{
		var design = FormDesign.Design;
		switch (selectedIcon)
		{
			case PromptIcons.Hand:
				PB_Icon.Image = IconManager.GetIcon("Hand", IconManager.GetLargeScale() * 3 / 2).Color(design.IconColor);
				base.CurrentFormState = FormState.Active;
				break;

			case PromptIcons.Info:
				PB_Icon.Image = IconManager.GetIcon("Info", IconManager.GetLargeScale() * 3 / 2).Color(design.ActiveColor);
				base.CurrentFormState = FormState.Active;
				break;

			case PromptIcons.Input:
				PB_Icon.Image = IconManager.GetIcon("Edit", IconManager.GetLargeScale() * 3 / 2).Color(design.IconColor);
				base.CurrentFormState = FormState.Active;
				break;

			case PromptIcons.Question:
				PB_Icon.Image = IconManager.GetIcon("Question", IconManager.GetLargeScale() * 3 / 2).Color(design.IconColor);
				base.CurrentFormState = FormState.Active;
				break;

			case PromptIcons.Ok:
				PB_Icon.Image = IconManager.GetIcon("Ok", IconManager.GetLargeScale() * 3 / 2).Color(design.GreenColor);
				base.CurrentFormState = FormState.Running;
				break;

			case PromptIcons.Loading:
				PB_Icon.Loading = true;
				base.CurrentFormState = FormState.Working;
				break;

			case PromptIcons.Warning:
				PB_Icon.Image = IconManager.GetIcon("Warning", IconManager.GetLargeScale() * 3 / 2).Color(design.YellowColor);
				base.CurrentFormState = FormState.Working;
				break;

			case PromptIcons.Error:
				PB_Icon.Image = IconManager.GetIcon("Error", IconManager.GetLargeScale() * 3 / 2).Color(design.RedColor);
				base.CurrentFormState = FormState.Busy;
				break;

			default:
				TLP_ImgText.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0);
				break;
		}
	}

	#endregion General Methods

	#region Click Events

	private void B_Abort_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Abort;
		Close();
	}

	private void B_Cancel_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
		Close();
	}

	private void B_Close_Click(object sender, EventArgs e)
	{
		DialogResult = selectedButtons switch
		{
			PromptButtons.OK => DialogResult.OK,
			PromptButtons.OKCancel => DialogResult.Cancel,
			PromptButtons.AbortRetryIgnore => DialogResult.Ignore,
			PromptButtons.YesNoCancel => DialogResult.Cancel,
			PromptButtons.YesNo => DialogResult.No,
			PromptButtons.RetryCancel => DialogResult.Cancel,
			_ => DialogResult.None,
		};
		Close();
	}

	private void B_Ignore_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Ignore;
		Close();
	}

	private void B_No_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.No;
		Close();
	}

	private void B_OK_Click(object sender, EventArgs e)
	{
		if (InputValidation == null || InputValidation(TB_Input.Text))
		{
			OutputText = TB_Input.Text;
			DialogResult = DialogResult.OK;
			Close();
		}
		else
		{
			SystemSounds.Exclamation.Play();
		}
	}

	private void B_Retry_Click(object sender, EventArgs e)
	{
		if (InputValidation == null || InputValidation(TB_Input.Text))
		{
			OutputText = TB_Input.Text;
			DialogResult = DialogResult.Retry;
			Close();
		}
		else
		{
			SystemSounds.Exclamation.Play();
		}
	}

	private void B_Yes_Click(object sender, EventArgs e)
	{
		if (InputValidation == null || InputValidation(TB_Input.Text))
		{
			OutputText = TB_Input.Text;
			DialogResult = DialogResult.Yes;
			Close();
		}
		else
		{
			SystemSounds.Exclamation.Play();
		}
	}

	#endregion Click Events

	private void B_Details_Click(object sender, EventArgs e)
	{
		var val = !TLP_Details.Visible;
		TLP_Details.Visible = val;
		TLP_Main.RowStyles[0] = val ? new RowStyle() : new RowStyle(SizeType.Percent, 100);
		TLP_Main.RowStyles[3].Height = val ? 100 : 0;
		B_Details.Text = val ? "Less Info" : "More Info";
		B_Details.ImageName = val ? "CircleArrowUp" : "CircleArrowDown";
		Height += (int)((val ? 200 : -200) * UI.FontScale);
	}

	private void TB_Input_KeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		if (e.KeyData == Keys.Enter)
		{
			var m = new Message();

			ProcessCmdKey(ref m, Keys.Enter);
		}
	}
}