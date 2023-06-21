using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using static System.Environment;

namespace SlickControls
{
	public partial class SlickPathTextBox : SlickTextBox, IValidationControl
	{
		public SlickPathTextBox() : base()
		{
			InitializeComponent();
			Validation = ValidationType.None;
			_textBox.PreviewKeyDown += TextBoxPreviewKeyDown;
			_textBox.KeyPress += HandleKeyInput;
			_textBox.TextChanged += AutoCompletePath;
			IconClicked += SlickPathTextBox_IconClicked;
		}

		[Category("Behavior")]
		public bool Folder { get; set; } = true;

		[Category("Behavior"), DefaultValue(null)]
		public string[] FileExtensions { get => ioSelectionDialog.ValidExtensions; set => ioSelectionDialog.ValidExtensions = value; }

		[Category("Behavior"), DefaultValue(null)]
		public string StartingFolder { get => ioSelectionDialog.StartingFolder; set => ioSelectionDialog.StartingFolder = value; }

		private void SlickPathTextBox_IconClicked(object sender, EventArgs e)
		{
			if (Folder)
			{
				if (ModifierKeys.HasFlag(Keys.Control))
				{
					ioSelectionDialog.ValidExtensions = new[] { ".lnk" };

					if (ioSelectionDialog.PromptFile(FindForm()) == DialogResult.OK)
						Text = Directory.GetParent(ioSelectionDialog.SelectedPath.GetShortcutPath()).FullName;
				}
				else if (ioSelectionDialog.PromptFolder(FindForm()) == DialogResult.OK)
				{
					Text = ioSelectionDialog.SelectedPath;
				}
			}
			else
			{
				ioSelectionDialog.ValidExtensions = FileExtensions;

				if (ioSelectionDialog.PromptFile(FindForm()) == DialogResult.OK)
					Text = ioSelectionDialog.SelectedPath;
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (Live)
			{
				ImageName = "I_FolderSearch";

				using (var img = Image)
				{
					Padding = new Padding(Padding.Left, Padding.Top, img != null ? (img.Width + 8) : 4, 4);
				}
			}
		}

		public override bool ValidInput
		{
			get
			{
				if (DesignMode)
					return true;

				if (Validation == ValidationType.Custom && ValidationCustom != null)
					return ValidationCustom(Text);

				if (string.IsNullOrWhiteSpace(Text))
					return false;

				if (!Folder && File.Exists(Text) && FileExtensions.Any(e => Text.EndsWith(e, StringComparison.CurrentCultureIgnoreCase)))
					return true;

				if (Folder && Directory.Exists(Text))
					return true;

				var path = GetUNCPath(Text);

				if (path != Text)
				{
					this.TryInvoke(() => Text = path);
					return ValidInput;
				}

				if (!Folder)
					return false;

				//if (DialogResult.Yes == MessagePrompt.Show($"The folder: '{Text}' does not exist.\n\nWould you like to create it?", "Folder not found", PromptButtons.YesNo, PromptIcons.Warning, FindForm() is SlickForm frm ? frm : null))
				{
					try
					{ Directory.CreateDirectory(Text); }
					catch
					{
						MessagePrompt.Show($"The folder: '{Text}' could not be created.", "Folder not found", PromptButtons.OK, PromptIcons.Error, FindForm() is SlickForm _frm ? _frm : null);
						return false;
					}
				}

				return Directory.Exists(Text);
			}
		}

		[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int WNetGetConnection(
		[MarshalAs(UnmanagedType.LPWStr)] string localName,
		[MarshalAs(UnmanagedType.LPWStr)] StringBuilder remoteName, ref int length);

		public static string GetUNCPath(string originalPath)
		{
			var sb = new StringBuilder(512);
			var size = sb.Capacity;

			if (originalPath != null && originalPath.Length > 2 && originalPath[1] == ':')
			{
				var c = originalPath[0];
				if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
				{
					var error = WNetGetConnection(originalPath.Substring(0, 2), sb, ref size);
					if (error == 0 || (!string.IsNullOrWhiteSpace(sb.ToString()) && Directory.Exists(sb.ToString())))
					{
						var path = Path.GetFullPath(originalPath).Substring(Path.GetPathRoot(originalPath).Length);
						return Path.Combine(sb.ToString().TrimEnd(), path);
					}
				}
			}

			return originalPath;
		}

		#region TextBox Handling

		private string lastAddedChars;
		private char lastKeyPress;
		private int chosenIndex;
		private readonly DisableIdentifier autoCompleteDisableIdentifier = new DisableIdentifier();

		private void AutoCompletePath(object sender, EventArgs e)
		{
			if (!_textBox.Focused || CrossIO.CurrentPlatform != Platform.Windows)
			{
				return;
			}

			var len = Text.Length;
			var hasBrackets = Regex.IsMatch(Text, @"[/\\]");

			foreach (var kvp in shortcuts)
			{
				if (hasBrackets)
				{
					if (Text.StartsWith(kvp.Key))
					{
						var endsBrackets = Regex.IsMatch(Text, @"[/\\]$");
						Text = (endsBrackets ? Text.TrimEnd('/', '\\') : Text).RegexReplace($"^{kvp.Key}", kvp.Value);
						Select(Text.Length + len - kvp.Value.Length - kvp.Key.Length + (Text.Length - kvp.Value.Length), -1);

						if (endsBrackets)
						{
							var start = SelectionStart;
							Text += "\\";
							Select(start + 1, -1);
						}
						return;
					}
				}
				else if (Text.StartsWith(kvp.Key.Substring(0, 3)))
				{
					Text = kvp.Value;
					Select(Text.Length - kvp.Key.Length + len, -1);
					return;
				}
			}

			if (len <= 3) return;

			try
			{
				var parentPath = Directory.GetParent(Text)?.FullName;
				var oldText = Text;

				if (!Directory.Exists(parentPath) && Directory.Exists(GetUNCPath(parentPath)))
				{
					parentPath = GetUNCPath(parentPath);
					var index = SelectionStart + GetUNCPath(Text).Length - Text.Length;

					autoCompleteDisableIdentifier.Disable();
					Text = GetUNCPath(Text);
					Select(index, Text.Length - index);
					lastAddedChars = SelectedText;
					chosenIndex = 0;
					autoCompleteDisableIdentifier.Enable();
				}

				if (parentPath == null && (Text.EndsWith("\\") || Text.EndsWith("/")))
					parentPath = Path.GetFullPath(Text);

				if (autoCompleteDisableIdentifier.Enabled && parentPath != null
					&& (((Text.EndsWith("\\") || Text.EndsWith("/")) && lastKeyPress != '\b')
					|| (!Directory.Exists(Text) && Directory.Exists(parentPath))))
				{
					var searchedDirectoryName = Regex.Match(Text, @"[/\\]([^/\\]+)$").Groups[1].Value;
					var selectedDirectory = string.Empty;

					if (Folder)
					{
						selectedDirectory = Directory.GetDirectories(parentPath, "*", SearchOption.TopDirectoryOnly)
						.Where(x => Path.GetFileName(x)[0] != '$' && (searchedDirectoryName == string.Empty
							|| Path.GetFileName(x).StartsWith(searchedDirectoryName, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
					}
					else
					{
						var directories = Directory.GetDirectories(parentPath, "*", SearchOption.TopDirectoryOnly)
							 .Where(x => Path.GetFileName(x)[0] != '$' && (searchedDirectoryName == string.Empty
								  || Path.GetFileName(x).StartsWith(searchedDirectoryName, StringComparison.OrdinalIgnoreCase)));
						selectedDirectory = Directory.GetFiles(parentPath, "*", SearchOption.TopDirectoryOnly)
							 .Where(x => FileExtensions.Any(f => x.EndsWith(f, StringComparison.CurrentCultureIgnoreCase))
								  && (searchedDirectoryName == string.Empty || Path.GetFileName(x).StartsWith(searchedDirectoryName, StringComparison.OrdinalIgnoreCase)))
										.Concat(directories).FirstOrDefault();
					}

					if (selectedDirectory != null)
					{
						var index = SelectionStart;

						autoCompleteDisableIdentifier.Disable();
						Text = selectedDirectory;
						Select(index, Text.Length - index);
						lastAddedChars = SelectedText;
						chosenIndex = 0;
						autoCompleteDisableIdentifier.Enable();

						if (oldText == Text)
							return;
					}
				}
			}
			catch { }
		}

		private void HandleKeyInput(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\b' && _textBox.SelectionStart > 0 && _textBox.SelectionLength > 0 && _textBox.SelectedText == lastAddedChars)
			{
				_textBox.SelectionStart--;
				_textBox.SelectionLength++;
			}
			else if (e.KeyChar == '\b' && _textBox.Text.Length > 3 && _textBox.Text[_textBox.Text.Length - 2].AnyOf('\\', '/'))
			{
				Select(_textBox.Text.Length - 2, 2);
			}

			if ((e.KeyChar == '\\' || e.KeyChar == '/') && Directory.Exists(_textBox.Text))
				_textBox.Select(_textBox.Text.Length, 0);

			if (e.KeyChar == '\t' && _textBox.SelectionLength != _textBox.TextLength && Directory.Exists(_textBox.Text) && Directory.GetParent(_textBox.Text) != null)
			{
				var index = _textBox.SelectionStart;
				var searchedDirectoryName = Regex.Match(_textBox.Text.Substring(0, index), @"[/\\]([^/\\]+)$").Groups[1].Value;
				var directories =
					Directory.GetDirectories(Directory.GetParent(_textBox.Text).FullName, "*", SearchOption.TopDirectoryOnly)
					.Where(x => Path.GetFileName(x)[0] != '$' && (searchedDirectoryName == string.Empty
						|| Path.GetFileName(x).StartsWith(searchedDirectoryName, StringComparison.OrdinalIgnoreCase)));

				if (!Folder)
				{
					directories = Directory.GetFiles(Directory.GetParent(_textBox.Text).FullName, "*", SearchOption.TopDirectoryOnly)
						.Where(x => FileExtensions.Any(f => x.EndsWith(f, StringComparison.CurrentCultureIgnoreCase))
							&& (searchedDirectoryName == string.Empty || Path.GetFileName(x).StartsWith(searchedDirectoryName, StringComparison.OrdinalIgnoreCase)))
								.Concat(directories);
				}

				if (directories.Any())
				{
					chosenIndex++;
					if (chosenIndex >= directories.Count())
						chosenIndex = 0;

					autoCompleteDisableIdentifier.Disable();
					_textBox.Text = directories.ElementAt(chosenIndex);
					_textBox.Select(index, _textBox.Text.Length - index);
					lastAddedChars = _textBox.SelectedText;
					autoCompleteDisableIdentifier.Enable();
					e.Handled = true;
				}
			}

			lastKeyPress = e.KeyChar;
		}

		private void TextBoxPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (_textBox.SelectedText != string.Empty)
			{
				if (e.KeyCode == Keys.Enter)
					_textBox.SelectionStart = _textBox.Text.Length;
				else if (_textBox.Text.EndsWith(_textBox.SelectedText))
					e.IsInputKey = true;
			}
		}

		#endregion TextBox Handling

		#region Shortcuts

		private static readonly Dictionary<string, string> shortcuts = new Dictionary<string, string>
		{
			{ "desktop", GetFolderPath(SpecialFolder.Desktop) },
			{ "documents", GetFolderPath(SpecialFolder.MyDocuments) },
			{ "downloads", Path.Combine(GetFolderPath(SpecialFolder.UserProfile), "Downloads") },
		};

		#endregion
	}
}