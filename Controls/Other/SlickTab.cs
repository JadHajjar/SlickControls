using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

[DefaultEvent("TabSelected")]
public partial class SlickTab : SlickControl, IAnimatable
{
	[Category("Action")]
	public event EventHandler TabSelected;

	[Category("Action")]
	public event EventHandler TabDeselected;

	private bool hovered;
	private bool selected;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text
	{
		get => base.Text; set
		{
			base.Text = value;
			Invalidate();
		}
	}

	[Category("Behavior"), DisplayName("Linked Control")]
	public Control LinkedControl { get; set; }

	[Category("Appearance"), DefaultValue(null)]
	public Image Icon { get; set; }

	[Category("Appearance"), DisplayName("Icon Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
	public DynamicIcon IconName { get; set; }

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int AnimatedValue { get; set; } = 15;

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int TargetAnimationValue => (Selected ? 100 : 0) + (Selected || Hovered ? 100 : 0);

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Hovered
	{
		get => hovered;
		set
		{
			hovered = value;

			if (!IsHandleCreated || DesignMode)
			{
				AnimatedValue = TargetAnimationValue;
			}
			else if (AnimatedValue != TargetAnimationValue)
			{
				AnimationHandler.Animate(this, 1.25);
			}

			Invalidate();
		}
	}

	[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Selected
	{
		get => selected;
		set
		{
			if (value && Parent != null)
			{
				foreach (var item in Parent.Controls.ThatAre<SlickTab>())
				{
					item.Selected = false;
				}
			}

			var wasSelected = selected;
			selected = value;

			if (!IsHandleCreated || DesignMode)
			{
				AnimatedValue = TargetAnimationValue;
			}
			else if (AnimatedValue != TargetAnimationValue)
			{
				AnimationHandler.Animate(this, 1.35);
			}

			Invalidate();

			if (value)
			{
				TabSelected?.Invoke(this, new EventArgs());
			}
			else if (wasSelected)
			{
				TabDeselected?.Invoke(this, new EventArgs());
			}
		}
	}

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color Tint { get; set; }

	public SlickTab()
	{
		InitializeComponent();
		DoubleBuffered = ResizeRedraw = true;
		TabStop = false;
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(5));
		Margin = UI.Scale(new Padding(0, 0, 5, 0));

		if (this is not SlickTabControl.Tab)
		{
			Size = UI.Scale(new Size(DefaultSize.Width, 48));
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var client = ClientRectangle.Pad(Margin);
		var active = Tint == Color.Empty ? FormDesign.Design.ActiveColor : Tint;
		var activeColor = active.MergeColor(ForeColor.MergeColor(BackColor, 70), Math.Max(0, AnimatedValue - 110));
		var backColor = Color.FromArgb(Math.Min(125, AnimatedValue) / 2, active);

		using var activeBrush = new SolidBrush(activeColor);

		if (backColor.A > 0)
		{
			using var backBrush = new SolidBrush(backColor);
			e.Graphics.FillRoundedRectangle(backBrush, client, Padding.Left);
		}

		var text = LocaleHelper.GetGlobalText(Text).One.ToUpper();
		using var font = UI.Font(7F, FontStyle.Bold).FitToWidth(text, client.Pad(Padding), e.Graphics);
		var textHeight = (int)e.Graphics.Measure(text, font).Height;
		using var img = (Icon != null ? new Bitmap(Icon) : IconName)?.Color(activeColor);

		if (img == null)
		{
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			e.Graphics.DrawString(text, font, activeBrush, client, format);
		}
		else if (Width > Height * 4 / 3)
		{
			var rect = client.CenterR(client.Width, textHeight + img.Height + UI.Scale(3));

			e.Graphics.DrawImage(img, rect.Align(img.Size, ContentAlignment.TopCenter));

			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
			e.Graphics.DrawString(text, font, activeBrush, rect, format);
		}
		else
		{
			e.Graphics.DrawImage(img, client.CenterR(img.Size));
		}

		base.OnPaint(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);

		Hovered = true;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);

		Hovered = false;
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left && !Selected)
		{
			Selected = true;
		}
	}
}