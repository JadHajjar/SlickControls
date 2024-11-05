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

	public void SetDefaultProgressiveColorPoints()
	{
		var colors = new Color[Items.Length];

		for (var i = 0; i < Items.Length; i++)
		{
			colors[i] = FormDesign.Design.YellowColor
				.MergeColor(FormDesign.Design.RedColor, (int)(100 * ((float)i / (Items.Length - 1)).Between(0, 1)))
				.MergeColor(FormDesign.Design.GreenColor, (int)(100 * ((float)(Items.Length - i - 1) / (Items.Length - 1)).Between(0, 1)))
				.Tint(Sat: 25, Lum: 5);
		}

		Colors = colors;
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

		if ((Items?.Length ?? 0) == 0)
		{
			return;
		}

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

		var lineY = OverheadText ? (Height - Padding.Bottom - UI.Scale(8)) : (Height / 2);

		using var backBrush = new SolidBrush(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor));
		using var activeBrush = new LinearGradientBrush(validArea.Pad(UI.Scale(-8)), FormDesign.Design.ActiveColor, FormDesign.Design.ActiveColor, 0f);
		using var backPen = new Pen(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 75), UI.Scale(OverheadText ? 3f : 6.5f)) { StartCap = LineCap.Round, EndCap = LineCap.Round };
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

		var cursor = PointToClient(Cursor.Position);

		for (var i = 0; i < Items.Length; i++)
		{
			if (OverheadText)
			{
				e.Graphics.FillEllipse(backBrush, new Rectangle(validArea.X + (validArea.Width * i / (Items.Length - 1)), lineY, 0, 0).Pad(UI.Scale(-6)));

				if (Progressive && currentIndex >= i)
				{
					e.Graphics.FillEllipse(activeBrush, new Rectangle(validArea.X + (validArea.Width * i / (Items.Length - 1)), lineY, 0, 0).Pad(UI.Scale(i == currentIndex ? -8 : -6)));
				}
			}

			using var font = UI.Font(6.25f, FontStyle.Bold);
			var text = TextConversion(Items[i]);
			var mainRect = getMainRect(i, font, text);

			if (Progressive && currentIndex >= i)
			{
				using var brush = new SolidBrush(GetColor(i));
				e.Graphics.FillRoundedRectangle(brush, mainRect, UI.Scale(3));
			}
			else if (mainRect.Contains(cursor))
			{
				using var brush = new SolidBrush(GetColor(i).MergeColor(backBrush.Color, 75));
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

		if (OverheadText && Progressive)
		{
			using var activePen = new Pen(activeBrush, UI.Scale(3.5f)) { StartCap = LineCap.Round, EndCap = LineCap.Round };
			e.Graphics.DrawLine(activePen, validArea.X, lineY, validArea.X + (validArea.Width * currentIndex / (Items.Length - 1)), lineY);
		}

		if (SelectedItem is not null)
		{
			using var font = UI.Font(OverheadText ? 8.25f : 7.5f, FontStyle.Bold);
			var text = TextConversion(SelectedItem);
			var mainRect = getMainRect(currentIndex, font, text);

			if (!OverheadText && Progressive)
			{
				using var gradient = new LinearGradientBrush(new Rectangle(0, 0, mainRect.X, Height), Color.FromArgb(125, BackColor), Color.FromArgb(25, BackColor), 0f);

				e.Graphics.FillRectangle(gradient, new Rectangle(1, 1, mainRect.X - 2, Height - 2));
			}
			else if (!Progressive)
			{
				using var shadow = new SolidBrush(Color.FromArgb(65, BackColor));

				e.Graphics.FillRectangle(shadow, ClientRectangle);
			}

			e.Graphics.FillRoundedRectangleWithShadow(mainRect, UI.Scale(4), UI.Scale(4), GetColor(currentIndex), Color.FromArgb(15, GetColor(currentIndex)));

			using var textBrush = new SolidBrush(GetColor(currentIndex).GetTextColor());
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(text, font, textBrush, mainRect, format);
		}

		Rectangle getMainRect(int i, Font font, string text)
		{
			ContentAlignment alignment;

			if (OverheadText)
			{
				alignment = i == 0 ? ContentAlignment.BottomLeft : i == Items.Length - 1 ? ContentAlignment.BottomRight : ContentAlignment.BottomCenter;
			}
			else
			{
				alignment = i == 0 ? ContentAlignment.MiddleLeft : i == Items.Length - 1 ? ContentAlignment.MiddleRight : ContentAlignment.MiddleCenter;
			}

			return new Rectangle(validArea.X + (validArea.Width * i / (Items.Length - 1)), OverheadText ? 0 : lineY, 0, OverheadText ? lineY - UI.Scale(16) - Padding.Bottom : 0)
				.Pad(UI.Scale(-5))
				.Align(e.Graphics.Measure(text, font).ToSize() + UI.Scale(new Size(6, 3)), alignment);
		}
	}

	private Color GetColor(int index)
	{
		var color = Colors != null && Colors.Length > index ? Colors[index] : FormDesign.Design.ActiveColor;
		
		if (index != (SelectedItem is null ? 0 : Items.IndexOf(SelectedItem)))
			return color.MergeColor(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor), 60);

		return color;
	}
}
