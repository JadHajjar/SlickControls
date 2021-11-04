using Extensions;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("TextChanged")]
	public partial class SlickTextBox : SlickControl, IValidationControl, ISupportsReset
	{
		[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public new event EventHandler TextChanged;

		[Category("Action")]
		public event EventHandler IconClicked;

		public Func<string, bool> ValidationCustom { get; set; }

		private string labelText = "Textbox";

		private string placeholder;

		private bool showLabel = true;
		private bool error;

		public SlickTextBox()
		{
			InitializeComponent();

			label.Click += (s, e) => TB.Focus();
			TB.TextChanged += TB_TextChanged;
			TB.Enter += TB_Enter;
			TB.Leave += TB_Leave;
			TB.PreviewKeyDown += TB_PreviewKeyDown;
			TB.KeyDown += TB_KeyDown;
			TB.KeyUp += (s, e) => OnKeyUp(e);
			TB.KeyPress += (s, e) => OnKeyPress(e);
		}

		[Category("Appearance")]
		public Image Image
		{
			get => PB.Image;
			set => PB.TryInvoke(() =>
				   {
					   PB.Loading = false;
					   PB.Image = value.Color(FormDesign.Design.IconColor);
					   PB.Visible = value != null || PB.Loading;
					   panel1.Padding = new Padding(0, 0, PB.Visible ? 20 : 0, 0);
				   });
		}

		[Category("Behavior")]
		public bool Password { get => TB.UseSystemPasswordChar; set => TB.UseSystemPasswordChar = value; }

		[Category("Behavior")]
		public bool ReadOnly { get => TB.ReadOnly; set => TB.ReadOnly = value; }

		[Category("Behavior")]
		public bool Required { get; set; }

		[Category("Behavior"), DisplayName("Select All On Focus")]
		public bool SelectAllOnFocus { get; set; }

		[Category("Behavior")]
		public int MaxLength { get => TB.MaxLength; set => TB.MaxLength = value; }

		[Category("Appearance"), DisplayName("Label Text"), DefaultValue("Textbox")]
		public string LabelText { get => labelText; set => label.Text = labelText = value; }

		[Category("Appearance")]
		public string Placeholder { get => placeholder; set => placeholder = L_Placerholder.Text = value; }

		[Browsable(false)]
		public string SelectedText { get => TB.SelectedText; set => TB.SelectedText = value; }

		[Browsable(false)]
		public int SelectionLength { get => TB.SelectionLength; set => TB.SelectionLength = value; }

		[Browsable(false)]
		public int SelectionStart { get => TB.SelectionStart; set => TB.SelectionStart = value; }

		public new bool Focused => TB.Focused;

		[Category("Behavior"), DisplayName("Show Text"), DefaultValue(true)]
		public bool ShowLabel
		{
			get => showLabel;
			set => label.Visible = showLabel = value;
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { TB.Text = base.Text = value; if (DefaultValue != null) DefaultValue = value; } }

		[Category("Appearance")]
		public HorizontalAlignment TextAlign { get => TB.TextAlign; set => TB.TextAlign = value; }

		[Category("Behavior")]
		public virtual ValidationType Validation { get; set; } = ValidationType.None;

		[Category("Behavior"), DefaultValue(null)]
		public string DefaultValue { get; set; } = null;

		[Category("Behavior"), DisplayName("Regex Validation")]
		public string ValidationRegex { get; set; } = "";

		public virtual bool ValidInput
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(TB.Text) || Validation == ValidationType.Custom)
				{
					switch (Validation)
					{
						case ValidationType.Number:
							return TB.Text.All(char.IsDigit);

						case ValidationType.Decimal:
							return decimal.TryParse(TB.Text, out var d);

						case ValidationType.Regex:
							return string.IsNullOrWhiteSpace(ValidationRegex) || Regex.IsMatch(TB.Text, ValidationRegex);

						case ValidationType.Custom:
							return ValidationCustom == null || ValidationCustom(TB.Text);

						default:
							return true;
					}
				}

				return !Required;
			}
		}

		public new void Select() => TB.Select();

		public void Select(int start, int length) => TB.Select(start.Between(0, TB.Text.Length - 1), length == -1 ? TB.Text.Length - start : length);

		public void SelectAll() => TB.SelectAll();

		public void ResetValue() => Text = DefaultValue;

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			PB.Visible = Image != null || PB.Loading;
		}

		protected override void UIChanged()
		{
			TB.Font = UI.Font(8.25F);
			label.Font = UI.Font(7.25F, FontStyle.Bold | FontStyle.Italic);
			L_Placerholder.Font = UI.Font(8.25F, FontStyle.Italic);
			using (var g = CreateGraphics())
			{
				var h = TB.Font.Height
					+ (ShowLabel ? label.Font.Height : 0)
					+ 5;
				MinimumSize = new Size(0, h);
				MaximumSize = new Size(9999, h);
				Height = h;
			}

			L_Placerholder.Top = TB.Height - L_Placerholder.Height + TB.Top - 2;
			PB.Location = new Point(panel1.Width - 19, panel1.Height - 19);
		}

		protected override void DesignChanged(FormDesign design)
		{
			label.ForeColor = design.LabelColor;
			L_Placerholder.ForeColor = design.InfoColor;
			TB.ForeColor = design.ForeColor;
			PB.Color(FormDesign.Design.IconColor);
			P_Bar.TryInvoke(() => P_Bar.BackColor = TB.Focused ? design.ActiveColor : design.AccentColor);
		}

		private void SlickTextBox_BackColorChanged(object sender, EventArgs e)
			=> TB.BackColor = BackColor;

		private void TB_Enter(object sender, EventArgs e) { if (!error) P_Bar.BackColor = FormDesign.Design.ActiveColor; }

		private void TB_Leave(object sender, EventArgs e) { if (!error) P_Bar.BackColor = FormDesign.Design.AccentColor; }

		private void TB_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if ((e.KeyData == Keys.Up || e.KeyData == Keys.Down) 
				&& (Validation == ValidationType.Decimal || Validation == ValidationType.Number) 
				&& ValidInput)
			{
				var numb = (e.KeyData == Keys.Up ? 1 : -1) * (Validation == ValidationType.Decimal ? 0.1 : 1);

				var ind = new[] { Text.Length - SelectionStart, SelectionLength };
				Text = (Text.SmartParseD() + numb).ToString();
				Select(Text.Length - ind[0], ind[1]);
				e.IsInputKey = true;
			}
			else
				OnPreviewKeyDown(e);
		}

		private void TB_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyData == Keys.Up || e.KeyData == Keys.Down)
				&& (Validation == ValidationType.Decimal || Validation == ValidationType.Number)
				&& ValidInput)
			{
				e.Handled = true;
			}
			else
				OnKeyDown(e);
		}

		private void TB_TextChanged(object sender, EventArgs e)
		{
			error = false;
			if (string.IsNullOrWhiteSpace(TB.Text))
			{
				switch (Validation)
				{
					case ValidationType.Number:
						if (TB.Text.All(char.IsDigit))
						{ Text = TB.Text; TextChanged?.Invoke(this, e); }
						else
						{
							TB.Text = TB.Text.Where(char.IsDigit);
						}

						break;

					case ValidationType.Decimal:
						if (!ValidInput)
						{
							TB.Text = Regex.Match(TB.Text, @"[,\d]+\.?(\d+)?").Value;
						}
						else
						{ Text = TB.Text; TextChanged?.Invoke(this, e); }
						break;

					case ValidationType.Custom:
						Text = TB.Text;
						TextChanged?.Invoke(this, e);
						break;

					case ValidationType.Regex:
						Text = TB.Text;
						if (Regex.IsMatch(TB.Text, ValidationRegex))
							TextChanged?.Invoke(this, e);
						break;

					default:
					{ Text = TB.Text; TextChanged?.Invoke(this, e); }
					break;
				}
			}
			else
			{ Text = TB.Text; TextChanged?.Invoke(this, e); }

			L_Placerholder.Visible = string.IsNullOrWhiteSpace(TB.Text);
		}

		private void L_Placerholder_Click(object sender, EventArgs e) => TB.Focus();

		private void PB_Click(object sender, EventArgs e) => IconClicked?.Invoke(this, e);

		private void SlickTextBox_Load(object sender, EventArgs e)
		{
			if (TB.Focused)
				P_Bar.BackColor = FormDesign.Design.ActiveColor;
		}

		public void SetError(bool warning = false)
		{
			error = true;
			P_Bar.BackColor = warning ? FormDesign.Design.YellowColor : FormDesign.Design.RedColor;
		}
	}
}