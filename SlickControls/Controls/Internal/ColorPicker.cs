﻿using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	internal partial class ColorPicker : SlickControl
	{
		private Color color;

		public ColorPicker()
		{
			InitializeComponent();

			PB_Color.Paint += Picker_Paint;
			PB_Color.Click += Picker_Click;
			tableLayoutPanel1.Click += Picker_Click;
			label1.Click += Picker_Click;
		}

		protected override void UIChanged()
		{
			label1.Font = UI.Font(9F);
			Size = UI.Scale(new Size(188, 37), UI.UIScale);
		}

		public event Action<object, bool> ColorChanged;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color Color
		{
			get => DesignMode ? Color.White : (FormDesign.IsCustomEligible() && !string.IsNullOrWhiteSpace(ColorName) ? color : ResetColor());
			set
			{
				if (color != value)
				{
					color = value;
					ColorChanged?.Invoke(this, false);
					if (color == value)
						PB_Color?.Invalidate();
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Bindable(true)]
		public override string Text
		{
			get => label1.Text;
			set => label1.Text = value;
		}

		[Category("Behavior"), DisplayName("Color Name")]
		public string ColorName { get; set; }

		protected override void DesignChanged(FormDesign design)
		{
			panel1.BackColor = design.AccentColor;
			tableLayoutPanel1.ForeColor = design.ForeColor;
			if (!string.IsNullOrWhiteSpace(ColorName))
				Color = DefaultColor();
		}

		private void Picker_Click(object sender, EventArgs e)
		{
			if ((e as MouseEventArgs).Button == MouseButtons.Left)
			{
				var colorDialog = new SlickColorPicker(Color);
				colorDialog.ColorChanged += (s, ea) =>
						this.TryInvoke(() =>
						{
							ColorSetter(colorDialog.Color);
							FormDesign.Switch(FormDesign.Custom, false, true);
						});

				if (colorDialog.ShowDialog() != DialogResult.OK)
					return;

				color = colorDialog.Color;
				ColorSetter(color);

				ColorChanged?.Invoke(this, true);
				PB_Color.Refresh();
			}
			else if ((e as MouseEventArgs).Button == MouseButtons.Right)
			{
				Color = ResetColor();
				ColorSetter(Color);

				ColorChanged?.Invoke(this, true);
				PB_Color.Refresh();
			}

			FormDesign.Switch(FormDesign.Custom, true, true);
		}

		private Color ResetColor()
		{
			var propertyInfo = typeof(FormDesign).GetProperty(ColorName);

			if (FindForm() is Theme_Changer frm)
				return (Color)propertyInfo.GetValue(FormDesign.List[frm.UD_BaseTheme.Text], null);

			var p = Parent;
			while (p != null && !(p is PanelContent))
				p = p.Parent;

			if (p == null)
				return Color.Empty;

			return (Color)propertyInfo.GetValue(FormDesign.List[(p as PC_ThemeChanger).UD_BaseTheme.Text], null);
		}

		public Color DefaultColor()
		{
			var propertyInfo = typeof(FormDesign).GetProperty(ColorName);
			return (Color)propertyInfo.GetValue(FormDesign.Custom, null);
		}

		public void ColorSetter(Color color)
		{
			if (!string.IsNullOrWhiteSpace(ColorName))
			{
				if (!FormDesign.IsCustomEligible())
				{
					if (FindForm() is Theme_Changer frm)
						FormDesign.SetCustomBaseDesign(FormDesign.List[frm.UD_BaseTheme.Text]);

					var p = Parent;
					while (p != null && !(p is PanelContent))
						p = p.Parent;

					if (p != null)
						FormDesign.SetCustomBaseDesign(FormDesign.List[(p as PC_ThemeChanger).UD_BaseTheme.Text]);
				}

				var propertyInfo = typeof(FormDesign).GetProperty(ColorName);
				propertyInfo.SetValue(FormDesign.Custom, color, null);
			}
		}

		private void Picker_Paint(object sender, PaintEventArgs e)
		{
			var size = (sender as Control).Size;
			var color = (sender as Control).BackColor;
			if (color == null)
				return;

			e.Graphics.Clear(FormDesign.Design.BackColor);
			e.Graphics.FillRectangle(new SolidBrush(Color), new Rectangle(1, 1, size.Width - 3, size.Height - 3));
			e.Graphics.DrawRectangle(new Pen(Color.FromArgb(175, ExtensionClass.ColorFromHSL(Color.GetHue(), Color.GetSaturation(), (1D - Color.GetBrightness()).Between(.2, .8))), 1), new Rectangle(0, 0, size.Width - 3, size.Height - 3));
		}
	}
}