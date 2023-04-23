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
	public partial class SlickTextBox : SlickImageControl, IValidationControl, ISupportsReset
	{
		[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public new event EventHandler TextChanged;

		[Category("Action")]
		public event EventHandler IconClicked;

		private string labelText = "Textbox";
		private string placeholder = "";
		private bool showLabel = true;
		private bool error;
		private bool warning;
		protected readonly TextBox _textBox;

		public SlickTextBox()
		{
			_textBox = new TextBox
			{
				BorderStyle = BorderStyle.None,
				Dock = DockStyle.Bottom,
				MaximumSize = new Size(2, 1000)
			};

			_textBox.TextChanged += TB_TextChanged;
			_textBox.Enter += TB_Enter;
			_textBox.Leave += TB_Leave;
			_textBox.PreviewKeyDown += TB_PreviewKeyDown;
			_textBox.KeyDown += TB_KeyDown;
			_textBox.KeyUp += (s, e) => OnKeyUp(e);
			_textBox.KeyPress += (s, e) => OnKeyPress(e);

			Controls.Add(_textBox);
		}

		protected override void OnImageChanged(EventArgs e)
		{
			base.OnImageChanged(e);

			if (Live)
			{
				UIChanged();
			}
		}

		[Category("Behavior"), DefaultValue(false)]
		public bool Password { get => _textBox.UseSystemPasswordChar; set => _textBox.UseSystemPasswordChar = value; }

		[Category("Behavior"), DefaultValue(false)]
		public bool ReadOnly { get => _textBox.ReadOnly; set => _textBox.ReadOnly = value; }

		[Category("Behavior"), DefaultValue(false)]
		public bool Required { get; set; }

		[Category("Behavior"), DisplayName("Select All On Focus"), DefaultValue(false)]
		public bool SelectAllOnFocus { get; set; }

		[Category("Behavior"), DefaultValue(32767)]
		public int MaxLength { get => _textBox.MaxLength; set => _textBox.MaxLength = value; }

		[Category("Appearance"), DisplayName("Label Text"), DefaultValue("Textbox")]
		public string LabelText { get => labelText; set { labelText = value; Invalidate(); } }

		[Category("Appearance"), DefaultValue("")]
		public string Placeholder { get => placeholder; set { placeholder = value; Invalidate(); } }

		[Browsable(false)]
		public string SelectedText { get => _textBox.SelectedText; set => _textBox.SelectedText = value; }

		[Browsable(false)]
		public int SelectionLength { get => _textBox.SelectionLength; set => _textBox.SelectionLength = value; }

		[Browsable(false)]
		public int SelectionStart { get => _textBox.SelectionStart; set => _textBox.SelectionStart = value; }

		public new bool Focused => _textBox.Focused;

		[Category("Behavior"), DisplayName("Show Text"), DefaultValue(true)]
		public bool ShowLabel
		{
			get => showLabel;
			set { showLabel = value; Invalidate(); }
		}

		[Category("Behavior"), DefaultValue(false)]
		public bool MultiLine
		{
			get => _textBox.Multiline;
			set
			{
				_textBox.Multiline = value;

				if (value)
				{
					_textBox.Dock = DockStyle.Fill;
				}
				else
				{
					_textBox.Dock = DockStyle.Bottom;
					UIChanged();
				}

				if (string.IsNullOrWhiteSpace(_textBox.Text) && !string.IsNullOrWhiteSpace(Placeholder))
				{
					_textBox.MaximumSize = new Size(MultiLine ? 8 : 2, 1000);
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { _textBox.Text = base.Text = value; if (DefaultValue != null) { DefaultValue = value; } } }

		[Category("Appearance"), DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign { get => _textBox.TextAlign; set => _textBox.TextAlign = value; }

		[Category("Behavior"), DefaultValue(ValidationType.None)]
		public virtual ValidationType Validation { get; set; }

		[Category("Behavior"), DefaultValue(null)]
		public string DefaultValue { get; set; }

		[Category("Behavior"), DisplayName("Regex Validation"), DefaultValue("")]
		public string ValidationRegex { get; set; } = "";

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool ValidInput
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(_textBox.Text) || Validation == ValidationType.Custom)
				{
					switch (Validation)
					{
						case ValidationType.Number:
							return _textBox.Text.All(char.IsDigit);

						case ValidationType.Decimal:
							return decimal.TryParse(_textBox.Text, out var d);

						case ValidationType.Regex:
							return string.IsNullOrWhiteSpace(ValidationRegex) || Regex.IsMatch(_textBox.Text, ValidationRegex);

						case ValidationType.Custom:
							return ValidationCustom == null || ValidationCustom(_textBox.Text);

						default:
							return true;
					}
				}

				return !Required;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Func<string, bool> ValidationCustom { get; set; }

		public new void Select()
		{
			_textBox.Select();
		}

		public void Select(int start, int length)
		{
			_textBox.Select(start.Between(0, _textBox.Text.Length - 1), length == -1 ? _textBox.Text.Length - start : length);
		}

		public void SelectAll()
		{
			_textBox.SelectAll();
		}

		public void ResetValue()
		{
			Text = DefaultValue;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
		}

		protected override void UIChanged()
		{
			using (var g = Graphics.FromHwnd(IntPtr.Zero))
			{
				using (var img = Image)
				{
					Padding = new Padding(4, showLabel ? (int)(g.Measure(nameof(SlickTextBox), UI.Font(6.75F, FontStyle.Bold)).Height + 4) : 4, img != null ? (img.Width + 8) : 4, 4);
				}

				_textBox.Font = UI.Font(8.25F * (float)UI.WindowsScale);
				var height = _textBox.Font.Height + Padding.Vertical;

				MaximumSize = Size.Empty;
				if (Live)
				{
					MinimumSize = new Size(Padding.Horizontal, height);
				}
				else
				{
					MinimumSize = Size.Empty;
				}
				Height = height;
			}
		}

		protected override void DesignChanged(FormDesign design)
		{
			_textBox.ForeColor = design.ForeColor;
			_textBox.BackColor = design.AccentBackColor;
			BackColor = Color.Empty;
		}

		private void TB_Enter(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void TB_Leave(object sender, EventArgs e)
		{
			Invalidate();
		}

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
			{
				OnPreviewKeyDown(e);
			}
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
			{
				OnKeyDown(e);
			}
		}

		private void TB_TextChanged(object sender, EventArgs e)
		{
			error = false;

			if (string.IsNullOrWhiteSpace(_textBox.Text))
			{
				switch (Validation)
				{
					case ValidationType.Number:
						if (_textBox.Text.All(char.IsDigit))
						{ Text = _textBox.Text; TextChanged?.Invoke(this, e); }
						else
						{
							_textBox.Text = _textBox.Text.Where(char.IsDigit);
						}

						break;

					case ValidationType.Decimal:
						if (!ValidInput)
						{
							_textBox.Text = Regex.Match(_textBox.Text, @"[,\d]+\.?(\d+)?").Value;
						}
						else
						{ Text = _textBox.Text; TextChanged?.Invoke(this, e); }
						break;

					case ValidationType.Custom:
						Text = _textBox.Text;
						TextChanged?.Invoke(this, e);
						break;

					case ValidationType.Regex:
						Text = _textBox.Text;
						if (Regex.IsMatch(_textBox.Text, ValidationRegex))
						{
							TextChanged?.Invoke(this, e);
						}

						break;

					default:
					{ Text = _textBox.Text; TextChanged?.Invoke(this, e); }
					break;
				}
			}
			else
			{
				Text = _textBox.Text;
				TextChanged?.Invoke(this, e);
			}

			if (string.IsNullOrWhiteSpace(_textBox.Text) && !string.IsNullOrWhiteSpace(Placeholder))
			{
				_textBox.MaximumSize = new Size(MultiLine ? 8 : 2, 1000);
			}
			else
			{
				_textBox.MaximumSize = Size.Empty;
				if (_textBox.Width < 3)
				{
					var cursor = _textBox.SelectionStart;
					var len = _textBox.SelectionLength;

					_textBox.Size = Size;
					_textBox.Select(0, 0);
					_textBox.Select(cursor, len);
				}
				else
				{
					_textBox.Size = Size;
				}
			}
		}

		public void SetError(bool warning = false)
		{
			error = true;
			this.warning = warning;
			Invalidate();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_textBox?.Dispose();
			}

			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				e.Graphics.Clear(BackColor);

				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

				var barColor =
					error ? FormDesign.Design.RedColor :
					warning ? FormDesign.Design.YellowColor :
					_textBox.Focused ? FormDesign.Design.ActiveColor :
					FormDesign.Design.AccentColor;

				e.Graphics.FillRoundedRectangle(new SolidBrush(barColor), ClientRectangle.Pad(1, 1, 2, 1), 4);

				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 3), 4);

				if (ShowLabel && !string.IsNullOrEmpty(LabelText))
				{
					var font = UI.Font(6.75F, FontStyle.Bold);
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(LabelText), font, new SolidBrush(FormDesign.Design.LabelColor), new Rectangle(2, 2, Width - Padding.Right, (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(LabelText), font).Height), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
				}

				using (var img = Image)
				{
					var iconRect = new Rectangle(Width - ((img?.Width ?? 0) * 11 / 8), 0, (img?.Width ?? 0) * 11 / 8, Height - 2);

					if (string.IsNullOrWhiteSpace(_textBox.Text) && !string.IsNullOrWhiteSpace(Placeholder))
					{
						var font = UI.Font(7.5F, FontStyle.Italic);
						var height = (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Placeholder), font).Height;
						e.Graphics.DrawString(LocaleHelper.GetGlobalText(Placeholder), font, new SolidBrush(FormDesign.Design.InfoColor), new Rectangle(_textBox.Left + _textBox.Width, Height - height - 2 - Padding.Bottom, Width - Padding.Right - _textBox.Left - _textBox.Width, height).Pad(0, 0, iconRect.Width, 0), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
					}

					if (Loading)
					{
						e.Graphics.DrawLoader(LoaderPercentage, UI.FontScale < 1.25 ? iconRect.CenterR(16, 16) : iconRect.CenterR(24, 24));
					}
					else if (img != null)
					{
						var active = IconClicked != null && iconRect.Contains(CursorLocation);

						if (active)
						{
							e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(20, FormDesign.Design.ForeColor)), iconRect, 4);
						}

						e.Graphics.DrawImage(img.Color(active ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), iconRect.CenterR(img.Size));
					}
				}
			}
			catch { }
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			using (var img = Image)
			{
				var iconRect = new Rectangle(Width - ((img?.Width ?? 0) * 11 / 8), 0, (img?.Width ?? 0) * 11 / 8, Height - 2);

				Cursor = HoverState.HasFlag(HoverState.Hovered) && IconClicked != null && iconRect.Contains(e.Location) ? Cursors.Hand : Cursors.IBeam;
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			using (var img = Image)
			{
				var iconRect = new Rectangle(Width - ((img?.Width ?? 0) * 11 / 8), 0, (img?.Width ?? 0) * 11 / 8, Height - 2);

				if (IconClicked != null && (e.Button == MouseButtons.None || (e.Button == MouseButtons.Left && iconRect.Contains(e.Location))))
				{
					IconClicked(this, e);
				}
				else
				{
					_textBox.Focus();
				}
			}

			base.OnMouseClick(e);
		}
	}
}