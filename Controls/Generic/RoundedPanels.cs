using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	public class RoundedPanel : DBPanel
	{
		[Category("Appearance"), DefaultValue(false)]
		public bool AddOutline { get; set; }
		[Category("Appearance"), DefaultValue(false)]
		public bool TopLeft { get; set; }
		[Category("Appearance"), DefaultValue(false)]
		public bool TopRight { get; set; }
		[Category("Appearance"), DefaultValue(false)]
		public bool BotRight { get; set; }
		[Category("Appearance"), DefaultValue(false)]
		public bool BotLeft { get; set; }

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			using (var brush = new SolidBrush(BackColor == Parent?.BackColor && !AddOutline ? FormDesign.Design.ButtonColor : BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(1, 1, 2, 2) : ClientRectangle.Pad(0, 0, 1, 1), Padding.Left, !TopLeft, !TopRight, !BotRight, !BotLeft);
			}

			if (AddOutline)
			{
				using (var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(0, 0, 1, 1).Pad((int)pen.Width), Padding.Left, !TopLeft,!TopRight,!BotRight,!BotLeft);
				}
			}
		}
	}

	public class RoundedTableLayoutPanel : DBTableLayoutPanel
	{
		[Category("Appearance"), DefaultValue(false)]
		public bool AddOutline { get; set; }

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			using (var brush = new SolidBrush(BackColor == Parent?.BackColor && !AddOutline ? FormDesign.Design.ButtonColor : BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(1, 1, 2, 2) : ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
			}

			if (AddOutline)
			{
				using (var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(0, 0, 1, 1).Pad((int)pen.Width), Padding.Left);
				}
			}
		}
	}

	public class RoundedFlowLayoutPanel : DBFlowLayoutPanel
	{
		[Category("Appearance"), DefaultValue(false)]
		public bool AddOutline { get; set; }

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			using (var brush = new SolidBrush(BackColor == Parent?.BackColor && !AddOutline ? FormDesign.Design.ButtonColor : BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(1, 1, 2, 2) : ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
			}

			if (AddOutline)
			{
				using (var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(0, 0, 1, 1).Pad((int)pen.Width), Padding.Left);
				}
			}
		}
	}

	public class RoundedGroupPanel : DBPanel
	{
		private Image image;

		[Category("Appearance"), DefaultValue(null)]
		public virtual Image Image { get => image; set { image = value; UIChanged(); } }

		[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
		public DynamicIcon ImageName { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool AddOutline { get; set; }

		[Category("Appearance"), DefaultValue(null)]
		public string Info { get; set; }

		[Category("Appearance"), DefaultValue(ColorStyle.Text)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Text;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		public RoundedGroupPanel()
		{
			UI.UIChanged += UIChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UI.UIChanged -= UIChanged;
			}

			base.Dispose(disposing);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			UIChanged();
		}

		private void UIChanged()
		{
			try
			{
				var padding = UI.Scale(new Padding(5), UI.FontScale);

				using (var g = Graphics.FromHwnd(IntPtr.Zero))
				{
					var iconWidth = 0;
					if (Image != null)
					{
						iconWidth = Image.Width;
					}
					else if (ImageName != null)
					{
						iconWidth = ImageName.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16)?.Width ?? 0;
					}

					var titleHeight = Math.Max(iconWidth, (int)g.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth <= 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);

					if (titleHeight > 0)
					{
						padding.Top += padding.Top + titleHeight;
					}
				}

				Padding = padding;
				Invalidate();
			}
			catch { }
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			using (var brush = new SolidBrush(BackColor == Parent?.BackColor && !AddOutline ? FormDesign.Design.ButtonColor : BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(1, 1, 2, 2) : ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
			}

			if (AddOutline)
			{
				using (var pen = new Pen(ColorStyle == ColorStyle.Text ? FormDesign.Design.AccentColor : ColorStyle.GetColor(), (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(0, 0, 1, 1).Pad((int)pen.Width), Padding.Left);
				}
			}

			try
			{
				using (var icon = Image == null ? ImageName?.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16) : new Bitmap(Image))
				{
					var textColor = ColorStyle == ColorStyle.Text ? FormDesign.Design.LabelColor : ColorStyle.GetColor().MergeColor(FormDesign.Design.IconColor, 70);
					var iconWidth = icon?.Width ?? 0;
					var titleHeight = Math.Max(iconWidth, (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);
					var iconRectangle = new Rectangle(Padding.Left * 2, (Padding.Bottom * 2) + ((titleHeight - iconWidth) / 2), iconWidth, iconWidth);

					if (icon != null)
					{
						e.Graphics.DrawImage(icon.Color(textColor), iconRectangle);
					}

					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), new SolidBrush(textColor), new Rectangle(iconWidth + (Padding.Left * 3), Padding.Bottom * 2, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });

					if (!string.IsNullOrWhiteSpace(Info))
					{
						var bnds = e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold));

						e.Graphics.DrawString(LocaleHelper.GetGlobalText(Info), UI.Font(iconWidth == 16 ? 7.5F : 8.25F), new SolidBrush(Color.FromArgb(200, textColor)), new Rectangle(iconWidth + (Padding.Left * 4) + (int)bnds.Width, Padding.Bottom * 2, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });
					}
				}
			}
			catch { }
		}
	}

	public class RoundedGroupFlowLayoutPanel : DBFlowLayoutPanel
	{
		private Image image;

		[Category("Appearance"), DefaultValue(null)]
		public virtual Image Image { get => image; set { image = value; UIChanged(); } }

		[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
		public DynamicIcon ImageName { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool AddOutline { get; set; }

		[Category("Appearance"), DefaultValue(null)]
		public string Info { get; set; }

		[Category("Appearance"), DefaultValue(ColorStyle.Text)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Text;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		public RoundedGroupFlowLayoutPanel()
		{
			UI.UIChanged += UIChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UI.UIChanged -= UIChanged;
			}

			base.Dispose(disposing);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			UIChanged();
		}

		private void UIChanged()
		{
			try
			{
				var padding = UI.Scale(new Padding(5), UI.FontScale);

				using (var g = Graphics.FromHwnd(IntPtr.Zero))
				{
					var iconWidth = 0;
					if (Image != null)
					{
						iconWidth = Image.Width;
					}
					else if (ImageName != null)
					{
						iconWidth = ImageName.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16)?.Width ?? 0;
					}

					var titleHeight = Math.Max(iconWidth, (int)g.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth <= 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);

					if (titleHeight > 0)
					{
						padding.Top += padding.Top + titleHeight;
					}
				}

				Padding = padding;
				Invalidate();
			}
			catch { }
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			using (var brush = new SolidBrush(BackColor == Parent?.BackColor && !AddOutline ? FormDesign.Design.ButtonColor : BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(1, 1, 2, 2) : ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
			}

			if (AddOutline)
			{
				using (var pen = new Pen(ColorStyle == ColorStyle.Text ? FormDesign.Design.AccentColor : ColorStyle.GetColor(), (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(0, 0, 1, 1).Pad((int)pen.Width), Padding.Left);
				}
			}

			try
			{
				using (var icon = Image == null ? ImageName?.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16) : new Bitmap(Image))
				{
					var textColor = ColorStyle == ColorStyle.Text ? FormDesign.Design.LabelColor : ColorStyle.GetColor().MergeColor(FormDesign.Design.IconColor, 70);
					var iconWidth = icon?.Width ?? 0;
					var titleHeight = Math.Max(iconWidth, (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);
					var iconRectangle = new Rectangle(Padding.Left * 2, (Padding.Bottom * 2) + ((titleHeight - iconWidth) / 2), iconWidth, iconWidth);

					if (icon != null)
					{
						e.Graphics.DrawImage(icon.Color(textColor), iconRectangle);
					}

					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), new SolidBrush(textColor), new Rectangle(iconWidth + (Padding.Left * 3), Padding.Bottom * 2, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });

					if (!string.IsNullOrWhiteSpace(Info))
					{
						var bnds = e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold));

						e.Graphics.DrawString(LocaleHelper.GetGlobalText(Info), UI.Font(iconWidth == 16 ? 7.5F : 8.25F), new SolidBrush(Color.FromArgb(200, textColor)), new Rectangle(iconWidth + (Padding.Left * 4) + (int)bnds.Width, Padding.Bottom * 2, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });
					}
				}
			}
			catch { }
		}
	}

	public class RoundedGroupTableLayoutPanel : DBTableLayoutPanel
	{
		private Image image;

		[Category("Appearance"), DefaultValue(null)]
		public virtual Image Image { get => image; set { image = value; UIChanged(); } }

		[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
		public DynamicIcon ImageName { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool AddOutline { get; set; }

		[Category("Appearance"), DefaultValue(null)]
		public string Info { get; set; }

		[Category("Behavior"), DefaultValue(false)]
		public bool UseFirstRowForPadding { get; set; }

		[Category("Appearance"), DefaultValue(ColorStyle.Text)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Text;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		public RoundedGroupTableLayoutPanel()
		{
			UI.UIChanged += UIChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UI.UIChanged -= UIChanged;
			}

			base.Dispose(disposing);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			UIChanged();
		}

		private void UIChanged()
		{
			try
			{
				var padding = UI.Scale(new Padding(5), UI.FontScale);

				using (var g = Graphics.FromHwnd(IntPtr.Zero))
				{
					var iconWidth = 0;
					if (Image != null)
					{
						iconWidth = Image.Width;
					}
					else if (ImageName != null)
					{
						iconWidth = ImageName.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16)?.Width ?? 0;
					}

					var titleHeight = Math.Max(iconWidth, (int)g.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth <= 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);

					if (titleHeight > 0)
					{
						if (UseFirstRowForPadding)
							RowStyles[0].Height = padding.Top + titleHeight;
						else
							padding.Top += padding.Top + titleHeight;
					}
				}

				Padding = padding;
				Invalidate();
			}
			catch { }
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Parent?.BackColor ?? BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			using (var brush = new SolidBrush(BackColor == Parent?.BackColor && !AddOutline ? FormDesign.Design.ButtonColor : BackColor))
			{
				e.Graphics.FillRoundedRectangle(brush, AddOutline ? ClientRectangle.Pad(1, 1, 2, 2) : ClientRectangle.Pad(0, 0, 1, 1), Padding.Left);
			}

			if (AddOutline)
			{
				using (var pen = new Pen(ColorStyle == ColorStyle.Text ? FormDesign.Design.AccentColor : ColorStyle.GetColor(), (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(0, 0, 1, 1).Pad((int)pen.Width), Padding.Left);
				}
			}

			try
			{
				using (var icon = Image == null ? ImageName?.Get(UI.FontScale >= 2.5 ? 48 : UI.FontScale >= 1.25 ? 24 : 16) : new Bitmap(Image))
				{
					var textColor = ColorStyle == ColorStyle.Text ? FormDesign.Design.LabelColor : ColorStyle.GetColor().MergeColor(FormDesign.Design.IconColor, 70);
					var iconWidth = icon?.Width ?? 0;
					var titleHeight = Math.Max(iconWidth, (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), Width - Padding.Horizontal).Height);
					var iconRectangle = new Rectangle(Padding.Left * 2, (Padding.Bottom * 2) + ((titleHeight - iconWidth) / 2), iconWidth, iconWidth);

					if (icon != null)
					{
						e.Graphics.DrawImage(icon.Color(textColor), iconRectangle);
					}

					e.Graphics.DrawString(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold), new SolidBrush(textColor), new Rectangle(iconWidth + (Padding.Left * 3), Padding.Bottom * 2, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });

					if (!string.IsNullOrWhiteSpace(Info))
					{
						var bnds = e.Graphics.Measure(LocaleHelper.GetGlobalText(Text), UI.Font(iconWidth == 16 ? 8.25F : 9.75F, FontStyle.Bold));

						e.Graphics.DrawString(LocaleHelper.GetGlobalText(Info), UI.Font(iconWidth == 16 ? 7.5F : 8.25F), new SolidBrush(Color.FromArgb(200, textColor)), new Rectangle(iconWidth + (Padding.Left * 4) + (int)bnds.Width, Padding.Bottom * 2, Width - Padding.Horizontal, titleHeight), new StringFormat { LineAlignment = StringAlignment.Center });
					}
				}
			}
			catch { }
		}
	}
}