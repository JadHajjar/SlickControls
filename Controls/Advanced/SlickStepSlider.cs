using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls.Controls.Advanced;
public class SlickStepSlider : SlickControl
{
	private bool mouseDown;
	private object lastValidItem;
	private Rectangle validArea;

	[Category("Behavior")]
	public event EventHandler OnSelectedItemChanged;

	[Category("Appearance"), DisplayName("Progressive Design"), DefaultValue(false)]
	public bool Progressive { get; set; }

	[Category("Appearance"), DisplayName("Use Overhead Text"), DefaultValue(false)]
	public bool OverheadText { get; set; }

	[Category("Data")]
	public object[] Items { get; set; } = [];

	[Category("Appearance"), DefaultValue(null)]
	public Color[] Colors { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
	public object SelectedItem { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
	public Func<object, string> TextConversion { get; set; } = x => x.ToString();

	[Category("Appearance"), DisplayName("Left Image"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
	public DynamicIcon LeftImage { get; set; }

	[Category("Appearance"), DisplayName("Right Image"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
	public DynamicIcon RightImage { get; set; }

	public SlickStepSlider()
	{
		Cursor = Cursors.Hand;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);

		mouseDown = true;
		lastValidItem = SelectedItem;

		OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);

		mouseDown = false;

		if (ClientRectangle.Pad(UI.Scale(-64)).Contains(e.Location))
		{
			var index = (int)Math.Round((e.Location.X - validArea.X) * (Items.Length - 1d) / validArea.Width).Between(0, Items.Length - 1);

			SelectedItem = Items[index];

			OnSelectedItemChanged?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			SelectedItem = lastValidItem;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (mouseDown)
		{
			if (ClientRectangle.Pad(UI.Scale(-64)).Contains(e.Location))
			{
				var index = (int)Math.Round((e.Location.X - validArea.X) * (Items.Length - 1d) / validArea.Width).Between(0, Items.Length - 1);

				SelectedItem = Items[index];
			}
			else
			{
				SelectedItem = lastValidItem;
			}
		}
	}

	protected override void UIChanged()
	{
		if (Padding == default)
		{
			Padding = UI.Scale(new Padding(3));
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		validArea = ClientRectangle.Pad(Padding);
		var currentIndex = SelectedItem is null ? 0 : Items.IndexOf(SelectedItem);

		if (LeftImage is not null)
		{
			using var img = LeftImage.Get(Height * 3 / 4).Color(GetColor(0));

			e.Graphics.DrawImage(img, validArea.Align(img.Size, ContentAlignment.MiddleLeft));

			validArea = validArea.Pad(Padding.Left + img.Width, 0, 0, 0);
		}

		if (RightImage is not null)
		{
			using var img = RightImage.Get(Height * 3 / 4).Color(currentIndex == Items.Length - 1 ? GetColor(currentIndex) : FormDesign.Design.IconColor);

			e.Graphics.DrawImage(img, validArea.Align(img.Size, ContentAlignment.MiddleRight));

			validArea = validArea.Pad(0, 0, Padding.Right + img.Width, 0);
		}

		validArea = validArea.Pad(UI.Scale(8), 0, UI.Scale(8), 0);

		var lineY = OverheadText ? (Padding.Bottom - UI.Scale(5)) : (Height / 2);

		using var backBrush = new SolidBrush(FormDesign.Design.AccentColor);
		using var activeBrush = new LinearGradientBrush(validArea.Pad(UI.Scale(-8)), FormDesign.Design.ActiveColor, FormDesign.Design.ActiveColor, 0f);
		using var backPen = new Pen(FormDesign.Design.AccentColor, UI.Scale(6.5f)) { StartCap = LineCap.Round, EndCap = LineCap.Round };
		e.Graphics.DrawLine(backPen, validArea.X, lineY, validArea.Right, lineY);

		activeBrush.InterpolationColors = new ColorBlend
		{
			Positions = Enumerable.Range(0, Items.Length).Select(x => x / (Items.Length - 1f)).ToArray(),
			Colors = Enumerable.Range(0, Items.Length).Select(GetColor).ToArray()
		};

		if (!OverheadText && Progressive)
		{
			using var activePen = new Pen(activeBrush, UI.Scale(6.5f)) { StartCap = LineCap.Round, EndCap = LineCap.Round };
			e.Graphics.DrawLine(activePen, validArea.X, lineY, validArea.X + (validArea.Width * currentIndex / (Items.Length - 1)), lineY);
		}

		for (var i = 0; i < Items.Length; i++)
		{
			if (OverheadText)
			{
				e.Graphics.FillEllipse(backBrush, new Rectangle(validArea.X + (validArea.Width * i / (Items.Length - 1)), lineY, 0, 0).Pad(UI.Scale(-8)));

				if (Progressive && currentIndex >= i)
				{
					e.Graphics.FillEllipse(activeBrush, new Rectangle(validArea.X + (validArea.Width * i / (Items.Length - 1)), lineY, 0, 0).Pad(UI.Scale(-6)));
				}
			}
			else
			{
				using var font = UI.Font(6.25f, FontStyle.Bold);
				var text = TextConversion(Items[i]);
				var mainRect = new Rectangle(validArea.X + (validArea.Width * i / (Items.Length - 1)), lineY, 0, 0).Pad(UI.Scale(-5)).Align(e.Graphics.Measure(text, font).ToSize() + UI.Scale(new Size(4, 2)), i == 0 ? ContentAlignment.MiddleLeft : i == Items.Length - 1 ? ContentAlignment.MiddleRight : ContentAlignment.MiddleCenter);

				if (Progressive && currentIndex >= i)
				{
					using var brush = new SolidBrush(GetColor(i));
					e.Graphics.FillRoundedRectangle(brush, mainRect, UI.Scale(3));
				}
				else
				{
					e.Graphics.FillRoundedRectangle(backBrush, mainRect, UI.Scale(3));
				}

				using var textBrush = new SolidBrush(Progressive && currentIndex >= i ? GetColor(i).GetTextColor() : FormDesign.Design.ForeColor);
				using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
				e.Graphics.DrawString(text, font, textBrush, mainRect, format);
			}
		}

		if (OverheadText && Progressive)
		{
			using var activePen = new Pen(activeBrush, UI.Scale(3.5f)) { StartCap = LineCap.Round, EndCap = LineCap.Round };
			e.Graphics.DrawLine(activePen, validArea.X, lineY, validArea.X + (validArea.Width * currentIndex / (Items.Length - 1)), lineY);
		}

		if (SelectedItem is not null)
		{
			using var font = UI.Font(7.5f, FontStyle.Bold);
			var text = TextConversion(SelectedItem);
			var mainRect = new Rectangle(validArea.X + (validArea.Width * currentIndex / (Items.Length - 1)), lineY, 0, 0).Pad(UI.Scale(-5)).Align(e.Graphics.Measure(text, font).ToSize() + UI.Scale(new Size(6, 4)), currentIndex == 0 ? ContentAlignment.MiddleLeft : currentIndex == Items.Length - 1 ? ContentAlignment.MiddleRight : ContentAlignment.MiddleCenter);

			e.Graphics.FillRoundedRectangleWithShadow(mainRect, UI.Scale(3), UI.Scale(4), GetColor(currentIndex), Color.FromArgb(15, GetColor(currentIndex)));

			using var textBrush = new SolidBrush(GetColor(currentIndex).GetTextColor());
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(text, font, textBrush, mainRect, format);
		}
	}

	private Color GetColor(int index)
	{
		if (Colors != null && Colors.Length > index)
		{
			return Colors[index];
		}

		return FormDesign.Design.ActiveColor;
	}
}
