using Extensions;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SlickControls;

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

		base.EnterTriggersClick = false;
	}

	protected override void OnImageChanged(EventArgs e)
	{
		base.OnImageChanged(e);

		if (Live)
		{
			UIChanged();
		}
	}

	[DefaultValue(false), Category("Behavior"), DisplayName("Enter Triggers Click")]
	public new bool EnterTriggersClick { get => base.EnterTriggersClick; set => base.EnterTriggersClick = value; }

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
	public string LabelText
	{
		get => labelText; set
		{
			labelText = value;
			Invalidate();
		}
	}

	[Category("Appearance"), DefaultValue("")]
	public string Placeholder
	{
		get => placeholder; set
		{
			placeholder = value;
			Invalidate();
		}
	}

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
		set
		{
			showLabel = value;
			Invalidate();
		}
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
	public override string Text
	{
		get => base.Text; set
		{
			_textBox.Text = base.Text = value;
			if (DefaultValue != null)
			{
				DefaultValue = value;
			}
		}
	}

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
				return Validation switch
				{
					ValidationType.Number => _textBox.Text.All(char.IsDigit),
					ValidationType.Decimal => decimal.TryParse(_textBox.Text, out var d),
					ValidationType.Regex => string.IsNullOrWhiteSpace(ValidationRegex) || Regex.IsMatch(_textBox.Text, ValidationRegex),
					ValidationType.Custom => ValidationCustom == null || ValidationCustom(_textBox.Text),
					_ => true,
				};
			}

			return !Required;
		}
	}

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Func<string, bool> ValidationCustom { get; set; }

	public override DynamicIcon ImageName
	{
		get => base.ImageName; set
		{
			base.ImageName = value;
			UIChanged();
		}
	}

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

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);

		UIChanged();
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

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

	protected override void UIChanged()
	{
		var pad = UI.Scale(4);
		var minimumSizeSet = MinimumSize.Height > 0 || Dock is DockStyle.Right or DockStyle.Left or DockStyle.Fill || MultiLine;

		using var font = UI.Font(6.75F, FontStyle.Bold);

		_textBox.Font = UI.Font(8.25F * (float)UI.WindowsScale);

		if (minimumSizeSet && (!showLabel || string.IsNullOrWhiteSpace(LabelText)))
		{
			Padding = new Padding(pad, (Height - _textBox.Height) / 2, pad, (Height - _textBox.Height) / 2);
		}
		else
		{
			Padding = new Padding(pad, showLabel && !string.IsNullOrWhiteSpace(LabelText) ? (int)((FontMeasuring.Measure(" ", font).Height * 0.65) + pad) : pad, pad, pad);
		}

		if (!minimumSizeSet)
		{
			var height = _textBox.Font.Height + Padding.Vertical + (pad / 2);

			if (Height != height)
			{
				Height = height;
			}
		}

		using var img = ImageName?.Get(Height * 5 / 7) ?? Image;

		if (img != null)
		{
			Padding = new Padding(Padding.Left, Padding.Top, Padding.Right + img.Width + pad, Padding.Bottom);
		}

		Invalidate();
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		base.OnBackColorChanged(e);

		DesignChanged(FormDesign.Design);
	}

	protected override void DesignChanged(FormDesign design)
	{
		BackColor = Color.Empty;
		_textBox.BackColor = BackColor.Tint(Lum: Enabled ? (design.IsDarkTheme ? 5 : -5) : (design.IsDarkTheme ? 2 : -2));
		_textBox.ForeColor = Enabled ? _textBox.BackColor.GetTextColor() : _textBox.BackColor.GetTextColor().MergeColor(BackColor);
		Invalidate();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Enter && MultiLine)
		{
			SendKeys.Send("+{ENTER}");
		}

		return base.ProcessCmdKey(ref msg, keyData);
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
		else if (e.KeyData == (Keys.Control | Keys.Back))
		{
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
		else if (e.KeyData == (Keys.Control | Keys.Back))
		{
			var text = Text;

			for (var i = _textBox.SelectionStart - 2; i > 0; i--)
			{
				if (text[i] == ' ')
				{
					Text = text.Remove(i + 1, _textBox.SelectionStart - 1 - i);
					_textBox.Select(i, 0);
					e.Handled = true;
					return;
				}
			}

			e.Handled = true;
			Text = string.Empty;
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
					{
						Text = _textBox.Text;
						TextChanged?.Invoke(this, e);
					}
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
					{
						Text = _textBox.Text;
						TextChanged?.Invoke(this, e);
					}

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
				{
					Text = _textBox.Text;
					TextChanged?.Invoke(this, e);
				}

				break;
			}
		}
		else if (_textBox.Text.Contains('\u007f'))
		{
			var index = _textBox.SelectionStart;
			var length = _textBox.SelectionLength;
			_textBox.Text = _textBox.Text.Remove("\u007f");
			_textBox.Select(index, length);
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

	public void ResetError()
	{
		error = false;
		warning = false;
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

	protected override void OnEnabledChanged(EventArgs e)
	{
		base.OnEnabledChanged(e);

		DesignChanged(FormDesign.Design);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		try
		{
			e.Graphics.SetUp(BackColor);

			var pad = UI.Scale(4);
			var barColor =
				error ? FormDesign.Design.RedColor :
				warning ? FormDesign.Design.YellowColor :
				_textBox.Focused ? FormDesign.Design.ActiveColor :
				FormDesign.Design.AccentColor;

			using var brush = new SolidBrush(barColor);
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1, 1, 2, 1), pad);

			using var brush1 = new SolidBrush(_textBox.BackColor);
			e.Graphics.FillRoundedRectangle(brush1, ClientRectangle.Pad(0, 0, 1, 1 + UI.Scale(2)), pad);

			using var img = ImageName?.Get(Height * 5 / 7) ?? Image;
			var imgWidth = img?.Width ?? IconManager.GetNormalScale();
			var iconRect = new Rectangle(Width - imgWidth - (pad * 2), 0, imgWidth + (pad * 2), Height - 2);

			if (ShowLabel && !string.IsNullOrEmpty(LabelText))
			{
				var text = LocaleHelper.GetGlobalText(LabelText);
				var rect = new Rectangle(pad, pad / 2, Width - Padding.Right - pad, Height - pad);
				using var font = UI.Font(6.75F, FontStyle.Bold).FitToWidth(text, rect, e.Graphics);
				using var brush2 = new SolidBrush(Color.FromArgb(200, _textBox.ForeColor));
				e.Graphics.DrawString(text, font, brush2, rect);
			}

			if (string.IsNullOrWhiteSpace(_textBox.Text) && !string.IsNullOrWhiteSpace(Placeholder))
			{
				var text = LocaleHelper.GetGlobalText(Placeholder);
				var rect = new Rectangle(_textBox.Left + _textBox.Width, _textBox.Top, Width - Padding.Right - _textBox.Left - _textBox.Width, _textBox.Height + (ShowLabel ? 0 : 0));
				using var brush2 = new SolidBrush(Color.FromArgb(135, _textBox.ForeColor));
				using var font = UI.Font(7.5F, FontStyle.Italic).FitToWidth(text, rect, e.Graphics);
				using var format = new StringFormat { LineAlignment = ShowLabel && !string.IsNullOrEmpty(LabelText) ? StringAlignment.Far : MultiLine && Height / UI.FontScale > 32 ? StringAlignment.Near : StringAlignment.Center };
				e.Graphics.DrawString(text, font, brush2, rect, format);
			}

			if (Loading)
			{
				e.Graphics.DrawLoader(LoaderPercentage, iconRect.CenterR(imgWidth, imgWidth));
			}
			else if (img != null)
			{
				var active = IconClicked != null && iconRect.Contains(CursorLocation);

				if (active)
				{
					using var brush2 = new SolidBrush(Color.FromArgb(20, _textBox.ForeColor));
					e.Graphics.FillRoundedRectangle(brush2, iconRect, 4);
				}

				e.Graphics.DrawImage(img.Color(active ? FormDesign.Design.ActiveColor : _textBox.ForeColor.MergeColor(_textBox.BackColor, 85)), iconRect.CenterR(img.Size));
			}
		}
		catch { }
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		using var img = Image;
		var iconRect = new Rectangle(Width - ((img?.Width ?? 0) * 11 / 8), 0, (img?.Width ?? 0) * 11 / 8, Height - 2);

		Cursor = HoverState.HasFlag(HoverState.Hovered) && IconClicked != null && iconRect.Contains(e.Location) ? Cursors.Hand : Cursors.IBeam;
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