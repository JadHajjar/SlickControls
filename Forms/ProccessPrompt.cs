using Extensions;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class ProccessPrompt : SlickForm
	{
		public event Action<FormClosingEventArgs> ActionCanceled;

		public bool CancelClicked { get; private set; } = false;

		private ProccessPrompt(string msg, bool showCancel)
		{
			InitializeComponent();

			Text = "Working..";
			L_Text.Font = UI.Font(9.75F);
			L_Text.Text = msg;

			PB_Icon.MouseDown += Form_MouseDown;
			L_Text.MouseDown += Form_MouseDown;
			TLP_ImgText.MouseDown += Form_MouseDown;

			B_Cancel.Visible = P_Spacer_1.Visible = showCancel;

			if (!showCancel)
				TLP_Main.RowStyles[2].Height = TLP_Main.RowStyles[1].Height = 0;
		}

		public override FormState CurrentFormState
		{
			get => FormState.ForcedFocused;
			set => base.CurrentFormState = FormState.ForcedFocused;
		}

		protected override void OnCreateControl()
		{
			Opacity = 0;
			Width = (int)(350 * UI.UIScale);
			Height = Math.Max(80, (int)Graphics.FromHwnd(IntPtr.Zero).Measure(L_Text.Text, L_Text.Font, Width - 135).Height + Padding.Vertical)
				+ 135;

			if (B_Cancel.Visible)
				TLP_Main.RowStyles[2].Height = (float)(42D * UI.UIScale);

			if (Owner != null)
			{
				Location = Owner.Bounds.Center(Size);
				TopMost = false;
			}
			else
			{
				Owner.Location = SystemInformation.VirtualScreen.Size.Center(Owner.Size);
			}

			base.OnCreateControl();
		}

		public static ProccessPrompt Create(string message, bool showCancel = false)
			=> new ProccessPrompt(message, showCancel);

		public new void Close() => this.TryInvoke(base.Close);

		public void SetText(string text) => this.TryInvoke(() => L_Text.Text = text);

		public void Show(SlickForm form = null)
		{
			if (form != null)
			{
				form.CurrentFormState = FormState.ForcedFocused;
			}
			else
			{
				StartPosition = FormStartPosition.CenterScreen;
			}

			try
			{
				ShowDialog();
			}
			finally
			{
				if (form != null)
					form.CurrentFormState = FormState.NormalFocused;
			}
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);
			base_P_Content.BackColor = design.BackColor;

			P_Spacer_1.BackColor = design.AccentColor;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape && B_Cancel.Visible)
			{
				B_Cancel_Click(this, new EventArgs());

				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void B_Cancel_Click(object sender, EventArgs e)
		{
			var ea = new FormClosingEventArgs(CloseReason.UserClosing, false);
			ActionCanceled?.Invoke(ea);
			if (!ea.Cancel)
			{
				Close();
				CancelClicked = true;
			}
		}
	}
}