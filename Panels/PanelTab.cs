using Extensions;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using static SlickControls.PanelItemControl;

namespace SlickControls
{
	public class PanelTab
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
		public bool IsSubItem { get; private set; }
		public PanelItem PanelItem { get; }
		public PanelItem ParentItem { get; }

		private PanelTab() { }

		public PanelTab(PanelItem panelItem)
		{
			PanelItem = panelItem;
		}

		public PanelTab(PanelItem panelItem, PanelItem parentItem)
		{
			PanelItem = panelItem;
			ParentItem = parentItem;
			IsSubItem = true;
		}

		public void Paint(ItemPaintEventArgs<PanelTab, Rectangles> e, PanelItemControl panelItemControl, bool small)
		{
			var clientRectangle = e.ClipRectangle;

			if (IsSeparator)
			{
				using (var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawLine(pen, small ? 0 : (int)(10 * UI.FontScale), clientRectangle.Y + (clientRectangle.Height / 2), clientRectangle.Width - (small ? 0 : (2 * (int)(10 * UI.FontScale))), clientRectangle.Y + (clientRectangle.Height / 2));
				}

				e.DrawableItem.CachedHeight = panelItemControl.ItemHeight;

				return;
			}

			if (IsGroupHeader)
			{
				using (var brush = new SolidBrush(FormDesign.Design.LabelColor))
					using (var font = UI.Font(8.25F, FontStyle.Bold))
				{
					var h = e.Graphics.Measure(LocaleHelper.GetGlobalText(GroupText).ToString().ToUpper(), font).Height;
					e.Graphics.DrawString(LocaleHelper.GetGlobalText(GroupText).ToString().ToUpper(), font, brush, new Rectangle(0, clientRectangle.Y + ((clientRectangle.Height - (int)h) / 2), clientRectangle.Width, (int)h));
				}

				e.DrawableItem.CachedHeight = panelItemControl.ItemHeight;

				return;
			}

			var bar = (int)(4 * UI.FontScale);
			var back = Color.Empty;
			var fore = FormDesign.Design.MenuForeColor;

			if (IsSubItem)
			{
				clientRectangle = clientRectangle.Pad(small ? bar : IconManager.GetNormalScale(), 0, 0, 0);

				using (var pen = new Pen(Color.FromArgb(200, FormDesign.Design.AccentColor), (float)(1.5 * UI.FontScale)))
				{
					e.Graphics.DrawLine(pen, (clientRectangle.X / 2) + bar, clientRectangle.Y / 2, (clientRectangle.X / 2) + bar, clientRectangle.Bottom);
				}
			}

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

			e.Graphics.FillRoundedRectangle(SlickControl.Gradient(clientRectangle, back, 1), clientRectangle.Pad(0, 1, 0, 1), bar);

			if (PanelItem.Selected && !e.HoverState.HasFlag(HoverState.Pressed) && !small)
			{
				using (var brush = new LinearGradientBrush(clientRectangle.Pad(clientRectangle.Width / 4, 0, 0, 0), Color.Empty, Color.FromArgb(50, FormDesign.Design.ActiveColor), LinearGradientMode.Horizontal))
				{
					e.Graphics.FillRoundedRectangle(brush, clientRectangle.Pad((clientRectangle.Width / 4) + 1, 1, bar, 1), bar);
				}

				using (var brush = new SolidBrush(FormDesign.Design.ActiveColor))
				e.Graphics.FillRoundedRectangle(brush, new Rectangle(clientRectangle.Right - (3 * bar / 2) - 1, clientRectangle.Y + 1, bar * 3 / 2, clientRectangle.Height - 2), bar * 3 / 4);
			}

			if (PanelItem.Highlighted)
			{
				using (var pen= new Pen(Color.FromArgb(100, FormDesign.Design.ActiveForeColor), 2F) { DashStyle = DashStyle.Dash })
				e.Graphics.DrawRoundedRectangle(pen, clientRectangle.Pad(0, 1, 0, 1), bar);
			}

			var iconWidth = 0;

			if (!string.IsNullOrEmpty(PanelItem.ShowKey))
			{
				using (var font = UI.Font(8.25F, FontStyle.Bold))
				{
					var roundRect = clientRectangle.Align(new Size(clientRectangle.Width, IconManager.GetNormalScale()), ContentAlignment.MiddleLeft);

					roundRect = roundRect.Pad(small ? ((clientRectangle.Width - roundRect.Height) / 2) : (int)(7 * UI.FontScale), 0, 0, 0);
					roundRect.Width = iconWidth = roundRect.Height;

					using (var brush= new SolidBrush(FormDesign.Design.ActiveColor))
					e.Graphics.FillRoundedRectangle(brush, roundRect, bar);
					using (var brush= new SolidBrush(FormDesign.Design.ActiveForeColor))
					using (var stringFormat= new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
					e.Graphics.DrawString(PanelItem.ShowKey, font, brush, roundRect.Pad(0, 1, -1, -1), stringFormat);
				}
			}
			else if (PanelItem.Icon != null)
			{
				using (var image = new Bitmap(PanelItem.Icon))
				{
					iconWidth = image.Width;

					var imageRect = clientRectangle.Pad(small ? (clientRectangle.Width - image.Width) / 2 : (int)(7 * UI.FontScale), 0, 0, 0).Align(image.Size, ContentAlignment.MiddleLeft);

					if (PanelItem.Loading)
					{
						panelItemControl.DrawLoader(e.Graphics, imageRect);
					}
					else
					{
						e.Graphics.DrawImage(image.Color(fore), imageRect);
					}
				}
			}
			else
			{
				using (var image = PanelItem.IconName?.Get((int)(18 * UI.FontScale)))
				{
					if (image != null)
					{
						iconWidth = image.Width;

						var imageRect = clientRectangle.Pad(small ? (clientRectangle.Width - image.Width) / 2 : (int)(7 * UI.FontScale), 0, 0, 0).Align(image.Size, ContentAlignment.MiddleLeft);

						if (PanelItem.Loading)
						{
							panelItemControl.DrawLoader(e.Graphics, imageRect);
						}
						else
						{
							e.Graphics.DrawImage(image.Color(fore), imageRect);
						}
					}
				}
			}

			if (!small)
			{
				using (var brush = SlickControl.Gradient(clientRectangle, fore))
				{
					var textRect = new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, (int)(24 * UI.FontScale));
					var text = LocaleHelper.GetGlobalText(PanelItem.Text);

					textRect = textRect.Pad((int)(10 * UI.FontScale) + iconWidth, 0, bar, 0);

					using (var font = UI.Font(8.25F))
					{
						var textSize = e.Graphics.Measure(text, font, textRect.Width);

						textRect.Height = Math.Max(textRect.Height, (int)textSize.Height + (bar * 2));

						e.Graphics.DrawString(text, font, brush, textRect.Align(new Size(textRect.Width, (int)textSize.Height + 1), ContentAlignment.MiddleLeft));

						e.DrawableItem.CachedHeight = textRect.Height;
					}
				}
			}
		}
	}
}
