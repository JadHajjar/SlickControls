using Extensions;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace SlickControls
{
	internal class PanelTab
	{
		public static PanelTab Separator()
		{
			return new PanelTab { IsSeparator = true };
		}

		public static PanelTab GroupName(string text)
		{
			return new PanelTab { IsGroupHeader = true, GroupText = text };
		}

		public bool IsSeparator { get; private set; }
		public string GroupText { get; private set; }
		public bool IsGroupHeader { get; private set; }
		public PanelItem PanelItem { get; }

		private PanelTab() { }

		public PanelTab(PanelItem panelItem)
		{
			PanelItem = panelItem;
		}

		public void Paint(ItemPaintEventArgs<PanelTab> e, bool small)
		{
			if (IsSeparator)
			{
				using (var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawLine(pen, small ? 0 : (int)(10 * UI.FontScale), e.ClipRectangle.Y + (e.ClipRectangle.Height / 2), e.ClipRectangle.Width - (small ? 0 : (2 * (int)(10 * UI.FontScale))), e.ClipRectangle.Y + (e.ClipRectangle.Height / 2));
				}

				return;
			}

			if (IsGroupHeader)
			{
				using (var brush = new SolidBrush(FormDesign.Design.LabelColor))
				{
					var h = e.Graphics.Measure(LocaleHelper.GetGlobalText(GroupText).ToString().ToUpper(), UI.Font(8.25F, FontStyle.Bold)).Height;
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(GroupText).ToString().ToUpper(), UI.Font(8.25F, FontStyle.Bold), brush, new Rectangle(0, e.ClipRectangle.Y + ((e.ClipRectangle.Height - (int)h) / 2), e.ClipRectangle.Width, (int)h));
				}

				return;
			}

			var bar = (int)(4 * UI.FontScale);
			var back = Color.Empty;
			var fore = FormDesign.Design.MenuForeColor;

			if (PanelItem.Highlighted)
			{
				back = Color.FromArgb(150, FormDesign.Design.ActiveColor);
				fore = FormDesign.Design.ActiveForeColor;
			}
			else if (e.HoverState.HasFlag(HoverState.Pressed))
			{
				back = Color.FromArgb(230, FormDesign.Design.ActiveColor);
				fore = FormDesign.Design.ActiveForeColor;
			}
			else if (e.HoverState.HasFlag(HoverState.Hovered))
			{
				back = Color.FromArgb(40, FormDesign.Design.MenuForeColor);

				if (PanelItem.Selected)
				{
					fore = FormDesign.Design.ActiveColor;
				}
			}
			else if (PanelItem.Selected)
			{
				if (small)
				{
					back = small ? FormDesign.Design.MenuColor.MergeColor(FormDesign.Design.ActiveColor, 75) : FormDesign.Design.MenuColor;
				}

				fore = FormDesign.Design.ActiveColor;
			}

			e.Graphics.FillRoundedRectangle(SlickControl.Gradient(e.ClipRectangle, back, 1), e.ClipRectangle.Pad(0, 1, 0, 1), bar);

			if (PanelItem.Selected && !e.HoverState.HasFlag(HoverState.Pressed) && !small)
			{
				var brush = new LinearGradientBrush(e.ClipRectangle.Pad(e.ClipRectangle.Width / 4, 0, 0, 0), Color.Empty, Color.FromArgb(50, FormDesign.Design.ActiveColor), LinearGradientMode.Horizontal);

				e.Graphics.FillRoundedRectangle(brush, e.ClipRectangle.Pad((e.ClipRectangle.Width / 4) + 1, 1, bar, 1), bar);

				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), new Rectangle(e.ClipRectangle.Right - (3 * bar / 2) - 1, e.ClipRectangle.Y + 1, bar * 3 / 2, e.ClipRectangle.Height - 2), bar * 3 / 4);
			}

			if (PanelItem.Highlighted)
			{
				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(100, FormDesign.Design.ActiveForeColor), 2F) { DashStyle = DashStyle.Dash }, e.ClipRectangle.Pad(0, 1, 0, 1), bar);
			}

			var iconWidth = 0;


			if (!string.IsNullOrEmpty(PanelItem.ShowKey))
			{
				var roundRect = e.ClipRectangle.AlignToFontSize(UI.Font(8.25F, FontStyle.Bold), ContentAlignment.MiddleLeft);

				roundRect = roundRect.Pad(small ? ((e.ClipRectangle.Width - roundRect.Height) / 2) : (int)(7 * UI.FontScale), 0, 0, 0);
				roundRect.Width = iconWidth = roundRect.Height;

				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), roundRect, bar);
				e.Graphics.DrawString(PanelItem.ShowKey, UI.Font(8.25F, FontStyle.Bold), new SolidBrush(FormDesign.Design.ActiveForeColor), roundRect.Pad(0, 1, -1, -1), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
			else if (PanelItem.Icon != null)
			{
				using (var image = new Bitmap(PanelItem.Icon))
				{
					iconWidth = image.Width;

					e.Graphics.DrawImage(image.Color(fore), e.ClipRectangle.Pad(small ? (e.ClipRectangle.Width - image.Width) / 2 : (int)(7 * UI.FontScale), 0, 0, 0).Align(image.Size, ContentAlignment.MiddleLeft));
				}
			}
			else
			{
				using (var image = (Bitmap)PanelItem.IconName)
				{
					if (image != null)
					{
						iconWidth = image.Width;
						e.Graphics.DrawImage(image.Color(fore), e.ClipRectangle.Pad(small ? (e.ClipRectangle.Width - image.Width) / 2 : (int)(7 * UI.FontScale), 0, 0, 0).Align(image.Size, ContentAlignment.MiddleLeft));
					}
				}
			}


			if (!small)
			{
				using (var brush = SlickControl.Gradient(e.ClipRectangle, fore))
				{
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(PanelItem.Text), UI.Font(8.25F), brush, (int)(10 * UI.FontScale) + iconWidth, e.ClipRectangle.Y + ((e.ClipRectangle.Height - e.Graphics.Measure(LocaleHelper.GetGlobalText(PanelItem.Text), UI.Font(8.25F)).Height) / 2));
				}
			}
		}
	}
}
