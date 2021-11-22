using Extensions;

using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class MessagePrompt : SlickForm
	{
		private Func<string, bool> InputValidation;
		private readonly PromptButtons selectedButtons;
		private readonly PromptIcons selectedIcon;
		private readonly bool isInput;

		private MessagePrompt() => InitializeComponent();

		private MessagePrompt(string title, string msg, PromptButtons buttons, PromptIcons icons, bool input)
		{
			InitializeComponent();

			Text = title;
			L_Text.Text = msg;
			L_Text.Font = UI.Font(9.75F);

			selectedButtons = buttons;
			selectedIcon = icons;
			isInput = input;

			SetIcon();
			SetButtons();

			PB_Icon.MouseDown += Form_MouseDown;
			L_Text.MouseDown += Form_MouseDown;
			TLP_ImgText.MouseDown += Form_MouseDown;
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
				BeginInvoke(new Action(() => { TB_Input.Focus(); TB_Input.SelectAll(); }));
			else
				BeginInvoke(new Action(() =>
				{
					switch (selectedButtons)
					{
						case PromptButtons.OK:
						case PromptButtons.OKCancel:
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

		protected override void OnCreateControl()
		{
			Opacity = 0;
			Width = (int)(400 * UI.UIScale);
			Height = Math.Max(80, (int)CreateGraphics().MeasureString(L_Text.Text, L_Text.Font, Width - 135).Height + Padding.Vertical)
				+ 135;

			if (isInput)
			{
				TB_Input.Visible = true;
				Height += (int)(50D * UI.UIScale);
			}
			else
			{
				TLP_ImgText.RowStyles[1].Height = 0;
			}

			TLP_Main.RowStyles[2].Height = (float)(42D * UI.UIScale);

			if (Owner != null)
			{
				Location = Owner.Bounds.Center(Size);
				TopMost = false;
			}
			else
			{
				Location = SystemInformation.VirtualScreen.Size.Center(Size);
			}

			base.OnCreateControl();
		}

		#region Statics

		public static DialogResult Show(
			  string message
			, PromptButtons buttons = PromptButtons.OK
			, PromptIcons icon = PromptIcons.None
			, SlickForm form = null)
			=> Show(message, "Prompt", buttons, icon, form);

		public static DialogResult Show(
			  string message
			, string title = "Prompt"
			, PromptButtons buttons = PromptButtons.OK
			, PromptIcons icon = PromptIcons.None
			, SlickForm form = null)
		{
			var @out = DialogResult.OK;

			ExtensionClass.TryInvoke(form, () =>
			{
				var prompt = new MessagePrompt(title, message, buttons, icon, false);

				if (form != null)
				{
					form.CurrentFormState = FormState.ForcedFocused;
					form.ShowUp();
				}
				else
					prompt.StartPosition = FormStartPosition.CenterScreen;

				try
				{
					@out = prompt.ShowDialog(form);
				}
				finally
				{
					if (form != null)
					{
						new Action(() =>
						form.TryInvoke(() =>
						{
							form.ShowUp();
							form.CurrentFormState = FormState.NormalFocused;
						})).RunInBackground(50);
					}
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

			ExtensionClass.TryInvoke(form, () =>
			{
				var prompt = new MessagePrompt(title, message, buttons, icon, true);
				prompt.TB_Input.Text = defaultValue;
				prompt.InputValidation = inputValidation;

				if (form != null)
				{
					form.CurrentFormState = FormState.ForcedFocused;
					form.ShowUp();
				}
				else
					prompt.StartPosition = FormStartPosition.CenterScreen;

				try
				{
					@out = new InputResult(prompt.ShowDialog(form), prompt.OutputText);
				}
				finally
				{
					if (form != null)
					{
						new Action(() =>
						form.TryInvoke(() =>
						{
							form.ShowUp();
							form.CurrentFormState = FormState.NormalFocused;
						})).RunInBackground(50);
					}
				}
			});

			return @out;
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
						B_OK_Click(this, new EventArgs());
						break;

					case PromptButtons.OKCancel:
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
					PB_Icon.Image = Properties.Resources.Icon_Hand.Color(design.IconColor);
					base.CurrentFormState = FormState.Active;
					break;

				case PromptIcons.Info:
					PB_Icon.Image = Properties.Resources.Icon_Info.Color(design.ActiveColor);
					base.CurrentFormState = FormState.Active;
					break;

				case PromptIcons.Input:
					PB_Icon.Image = Properties.Resources.Icon_Change.Color(design.IconColor);
					base.CurrentFormState = FormState.Active;
					break;

				case PromptIcons.Question:
					PB_Icon.Image = Properties.Resources.Icon_Question.Color(design.IconColor);
					base.CurrentFormState = FormState.Active;
					break;

				case PromptIcons.Ok:
					PB_Icon.Image = Properties.Resources.Icon_OK.Color(design.GreenColor);
					base.CurrentFormState = FormState.Running;
					break;

				case PromptIcons.Loading:
					PB_Icon.Loading = true;
					base.CurrentFormState = FormState.Working;
					break;

				case PromptIcons.Warning:
					PB_Icon.Image = Properties.Resources.Icon_Warning.Color(design.YellowColor);
					base.CurrentFormState = FormState.Working;
					break;

				case PromptIcons.Error:
					PB_Icon.Image = Properties.Resources.Icon_No.Color(design.RedColor);
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
			switch (selectedButtons)
			{
				case PromptButtons.OK:
					DialogResult = DialogResult.OK;
					break;

				case PromptButtons.OKCancel:
					DialogResult = DialogResult.Cancel;
					break;

				case PromptButtons.AbortRetryIgnore:
					DialogResult = DialogResult.Ignore;
					break;

				case PromptButtons.YesNoCancel:
					DialogResult = DialogResult.Cancel;
					break;

				case PromptButtons.YesNo:
					DialogResult = DialogResult.No;
					break;

				case PromptButtons.RetryCancel:
					DialogResult = DialogResult.Cancel;
					break;

				default:
					DialogResult = DialogResult.None;
					break;
			}
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
	}
}