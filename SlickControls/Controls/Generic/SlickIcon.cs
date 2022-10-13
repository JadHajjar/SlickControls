using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickIcon : SlickPictureBox
	{
		private bool enableGraphics = true;

		private int minimumIconSize = 18;

		private bool selected = false;

		public SlickIcon()
		{
			DoubleBuffered = true;
			Cursor = Cursors.Hand;
			FormDesign.DesignChanged += (d) => UpdateState();
			UpdateState(true);
			SizeMode = PictureBoxSizeMode.Zoom;
		}

		public delegate void action();

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Func<Color> ActiveColor { get; set; }

		private HoverState hoverState = HoverState.Normal;

		[Category("Appearance")]
		public new Image Image
		{
			get => base.Image;
			set
			{
				base.Image = value;
				Visible = value != null;
				UpdateState(true);
			}
		}

		[Category("Design")]
		public int MinimumIconSize
		{
			get => minimumIconSize;
			set
			{
				minimumIconSize = value;
				Size = new Size(value, value);
			}
		}

		[Category("Behavior")]
		public new bool Enabled
		{
			get => enableGraphics;
			set
			{
				enableGraphics = value;
				Cursor = value ? Cursors.Hand : Cursors.Default;
				UpdateState(true);
			}
		}

		[Category("Appearance"), DisplayName("Color Style"), DefaultValue(ColorStyle.Active)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Active;

		private bool IsPicture
		{
			get
			{
				try { return Image != null && Image.RawFormat.Guid != System.Drawing.Imaging.ImageFormat.Gif.Guid; }
				catch { return false; }
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override HoverState HoverState { get => hoverState; internal set { hoverState = value; UpdateState(); } }

		private void UpdateState(bool forced = false)
		{
			if (DesignMode || (!forced && (!Enabled || selected)))
				return;

			if (hoverState.HasFlag(HoverState.Hovered))
			{
				if (IsPicture)
				{
					if (ActiveColor == null)
						base.Image = Image.Color(ColorStyle.GetColor());
					else
						base.Image = Image.Color(ActiveColor());
				}
			}
			else if (IsPicture)
				base.Image = Image.Color(FormDesign.Design.IconColor);
		}

		public void Hold()
		{
			HoverState &= HoverState.Pressed;
			selected = true;
		}

		public void Release()
		{
			selected = false;
		}

		public void Disable()
			=> Enabled = false;

		public void Enable()
			=> Enabled = true;
	}
}