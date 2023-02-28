using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class WorkingPrompt : SlickControl
	{
		private bool showCancel;

		public event Action<FormClosingEventArgs> ActionCanceled;

		public bool CancelClicked { get; private set; } = false;
		public bool ShowCancel { get => showCancel; set => B_Cancel.Enabled = showCancel = value; }

		public WorkingPrompt(string msg, bool showCancel)
		{
			InitializeComponent();

			Text = "Working..";
			L_Text.Font = UI.Font(9.75F);
			L_Text.Text = msg;

			B_Cancel.Visible = ShowCancel = showCancel;

			if (!showCancel)
				TLP_Main.RowStyles[2].Height = TLP_Main.RowStyles[1].Height = 0;
		}

		protected override void OnCreateControl()
		{
			Width = (int)(310 * UI.UIScale);
			Height = Math.Max(80, (int)CreateGraphics().MeasureString(L_Text.Text, L_Text.Font, Width - 135).Height + Padding.Vertical)
				+ 100;

			if (B_Cancel.Visible)
				TLP_Main.RowStyles[2].Height = (float)(42D * UI.UIScale);

			if (Parent != null)
			{
				Location = Parent.Bounds.Center(Size);
			}

			base.OnCreateControl();
		}

		public void SetText(string text) => this.TryInvoke(() => L_Text.Text = text);

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			BackColor = design.ActiveColor;
			TLP_Main.BackColor = design.BackColor;

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
				Dispose();
				CancelClicked = true;
			}
		}
	}
}
