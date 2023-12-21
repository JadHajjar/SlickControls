using Extensions;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class SlickDropdown : SlickTextBox
	{
		private object[] _items;

		private DropDownItems DropDownItems;

		[Category("Behavior"), DefaultValue(true)]
		public bool AutoComplete { get; set; } = true;

		public SlickDropdown()
		{
			InitializeComponent();

			_textBox.TextChanged += TB_TextChanged;
			_textBox.Leave += TB_Leave;
			_textBox.MouseWheel += TB_MouseWheel;
			_textBox.KeyPress += TB_KeyPress;
			_textBox.MouseDoubleClick += TB_MouseDoubleClick;
		}

		protected override void OnCreateControl()
		{
			if (FindForm() is SlickForm frm)
			{
				frm.OnWndProc += Frm_OnWndProc;
				Disposed += (s, e) => frm.OnWndProc -= Frm_OnWndProc;
			}

			base.OnCreateControl();
		}

		private bool Frm_OnWndProc(Message arg)
		{
			if (Visible
				&& DropDownItems != null
				&& arg.Msg == 0x21
				&& !new Rectangle(PointToScreen(Point.Empty), Size).Contains(Cursor.Position)
				&& !new Rectangle(DropDownItems.PointToScreen(Point.Empty), DropDownItems.Size).Contains(Cursor.Position))
			{
				DropDownItems.Dispose();
				DropDownItems = null;
			}

			return false;
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			if (Live && (Items?.Length ?? 0) == 0)
				Loading = true;
		}

		private void TB_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!ReadOnly && ClientRectangle.Contains(PointToClient(Cursor.Position)))
			{
				if (SelectedItem != null && Items != null)
				{
					if (e.Delta > 0)
						Text = Items[Math.Max(0, Items.ToList().IndexOf(SelectedItem) - 1)].If(x => Conversion == null, x => x.ToString(), x => Conversion(x));
					else if (e.Delta < 0)
						Text = Items[Math.Min(Items.Length - 1, Items.ToList().IndexOf(SelectedItem) + 1)].If(x => Conversion == null, x => x.ToString(), x => Conversion(x));
				}
				else if (Items?.Any() ?? false)
					Text = (e.Delta <= 0 ? Items.FirstOrDefault() : Items.LastOrDefault()).If(x => Conversion == null, x => x.ToString(), x => Conversion(x));
			}
		}

		[Browsable(true)]
		public new event EventHandler TextChanged;

		[Category("Data")]
		public Func<object, string> Conversion { get; set; }

		[Category("Data")]
		public object[] Items { get => _items; set { _items = value; Image = Properties.Resources.ArrowDown; } }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FontDropdown { get; set; }

		public object SelectedItem
		{
			get => Items?.FirstOrDefault(x => Text == (Conversion == null ? x.ToString() : Conversion(x)));
			set
			{
				if (value?.GetType() == typeof(string))
					value = Items?.FirstOrDefault(x => value.ToString() == (Conversion == null ? x.ToString() : Conversion(x)));

				Text = value == null ? "" : Conversion == null ? value.ToString() : Conversion(value);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = _textBox.Text = value; TextChanged?.Invoke(this, new EventArgs()); } }

		public override bool ValidInput
		{
			get
			{
				if (Required && Items != null && Items.Any() && Validation == ValidationType.None)
					return SelectedItem != null;

				return base.ValidInput;
			}
		}

		private void ActiveDropDown_Load(object sender, EventArgs e)
		{
			if (Live && (Items?.Length ?? 0) == 0)
				Loading = true;

			var frm = FindForm();
			if (frm != null)
			{
				LocationChanged += (s, ea) =>
				{
					if (DropDownItems != null)
						DropDownItems.Location = FindForm().PointToClient(PointToScreen(new Point(0, Height - 2)));
				};

				frm.LocationChanged += (s, ea) =>
				{
					if (DropDownItems != null)
						DropDownItems.Location = FindForm().PointToClient(PointToScreen(new Point(0, Height - 2)));
				};

				frm.Resize += (s, ea) =>
				{
					if (DropDownItems != null && DropDownItems.Visible)
					{
						if (frm.WindowState == FormWindowState.Minimized)
						{
							DropDownItems.Dispose();
							DropDownItems = null;
						}
						else
						{
							DropDownItems.Location = FindForm().PointToClient(PointToScreen(new Point(0, Height - 2)));
							DropDownItems.MaximumSize = new Size(Width, 9999);
							DropDownItems.MinimumSize = new Size(Width, 0);
						}
					}
				};
			}
		}

		private void Label1_Click(object sender, EventArgs e) => _textBox.Focus();

		private void PB_Arrow_Click(object sender, EventArgs e)
		{
			ShowDropdown();
		}

		public void ShowDropdown()
		{
			if (DropDownItems == null)
			{
				if (Items != null && !ReadOnly)
				{
					var itemsize = (int)FindForm().Font.Height + 2;
					DropDownItems = new DropDownItems(Conversion, FontDropdown)
					{
						Location = FindForm().PointToClient(PointToScreen(new Point(0, Height - 3))),
						MaximumSize = new Size(Width, 9999),
						MinimumSize = new Size(Width, 0)
					};
					new AnimationHandler(DropDownItems, new Size(Width, Math.Min(DropDownItems.Height, FindForm().Height - DropDownItems.Top - 15))).StartAnimation();
					//DropDownItems.Height = Math.Min(DropDownItems.Height, FindForm().Height - DropDownItems.Top - 15);
					DropDownItems.ItemSelected += (item) => { Text = Conversion == null ? item.ToString() : Conversion(item); DropDownItems = null; };
					DropDownItems.Disposed += (s, ea) => Image = Properties.Resources.ArrowDown;
					DropDownItems.Parent = FindForm();
					DropDownItems.BringToFront();
					DropDownItems.SetItems(Items);
					Image = Properties.Resources.ArrowUp;
					_textBox.Focus();
				}
				else
				{
					SystemSounds.Exclamation.Play();
				}
			}
			else
			{
				DropDownItems.Dispose();
				DropDownItems = null;
			}
		}

		private void TB_KeyPress(object sender, KeyPressEventArgs e) => OnKeyPress(e);

		private void TB_Leave(object sender, EventArgs e)
		{
			DropDownItems?.Dispose();
			DropDownItems = null;
		}

		private void TB_MouseDoubleClick(object sender, MouseEventArgs e) => PB_Arrow_Click(sender, e);

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (DropDownItems?.KeyPressed(ref msg, keyData) ?? false)
				return true;

			if (Keys.Back == keyData && _textBox.SelectionStart > 0 && _textBox.SelectionLength > 0)
			{
				_textBox.SelectionStart--;
				_textBox.SelectionLength++;
			}

			if (Keys.Down == keyData && DropDownItems == null)
				PB_Arrow_Click(null, null);

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void TB_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (!AutoComplete)
					return;

				if (_textBox.Text != "")
				{
					if (Items == null || Items.Any(x => (Conversion == null ? x.ToString() : Conversion(x)) == _textBox.Text))
						return;

					var items = Items.Convert(x => Conversion == null ? x.ToString() : Conversion(x));
					var txt = _textBox.Text.ToLower();

					var match = items.Where(x => x.ToLower() != txt && x.ToLower().StartsWith(txt)).FirstOrDefault();

					if (match != null)
					{
						var index = _textBox.Text.Length;
						var itemsize = (int)FindForm().Font.Height + 2;
						Text = match;
						if (DropDownItems != null)
						{
							DropDownItems.SetItems(items.Where(x => x.ToLower() != txt && x.ToLower().StartsWith(txt)));
						}
						else
						{
							DropDownItems = new DropDownItems(Conversion, FontDropdown)
							{
								Location = FindForm().PointToClient(PointToScreen(new Point(0, Height - 2))),
								MaximumSize = new Size(Width, 9999),
								MinimumSize = new Size(Width, 0)
							};
							new AnimationHandler(DropDownItems, new Size(Width, Math.Min(DropDownItems.Height, FindForm().Height - DropDownItems.Top - 15))).StartAnimation();
							//DropDownItems.Height = Math.Min(DropDownItems.Height, FindForm().Height - DropDownItems.Top - 15);
							DropDownItems.ItemSelected += (item) => { Text = Conversion == null ? item.ToString() : Conversion(item); DropDownItems = null; };
							DropDownItems.Disposed += (s, ea) => Image = Properties.Resources.ArrowDown;
							DropDownItems.Parent = FindForm();
							DropDownItems.BringToFront();
							DropDownItems.SetItems(items.Where(x => x.ToLower() != txt && x.ToLower().StartsWith(txt)));
							Image = Properties.Resources.ArrowUp;
							_textBox.Focus();
						}
						_textBox.Select(index, _textBox.Text.Length - index);
					}

					if (DropDownItems != null && items.Count(x => x.ToLower() != txt && x.ToLower().StartsWith(txt)) == 1)
					{
						DropDownItems.Dispose();
						DropDownItems = null;
					}
				}
				else
				{
					DropDownItems?.Dispose();
					DropDownItems = null;
				}
			}
			finally { Text = _textBox.Text; }
		}
	}
}