using Extensions;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace SlickControls;

public class PanelTab
{
	public static PanelTab Separator(System.Collections.Generic.IEnumerable<PanelItem> items = null)
	{
		return new PanelTab { IsSeparator = true, GroupItems = items?.ToArray() ?? [] };
	}

	public static PanelTab GroupName(string text, System.Collections.Generic.IEnumerable<PanelItem> items = null)
	{
		return new PanelTab { IsGroupHeader = true, GroupText = text, GroupItems = items?.ToArray() ?? [] };
	}

	public bool IsSeparator { get; private set; }
	public string GroupText { get; private set; }
	public bool IsGroupHeader { get; private set; }
	public bool IsSubItem { get; private set; }
	public PanelItem PanelItem { get; }
	public PanelItem ParentItem { get; }
	public PanelItem[] GroupItems { get; private set; }

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

	public void Paint(ItemPaintEventArgs<PanelTab> e, PanelItemControl panelItemControl, bool small)
	{
		var clientRectangle = e.ClipRectangle;

		if (e.ClipRectangle.Size == default)
		{
			return;
		}

		if (IsSeparator)
		{
			using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale));

			e.Graphics.DrawLine(pen, small ? 0 : UI.Scale(10), clientRectangle.Y + (clientRectangle.Height / 2), clientRectangle.Width - (small ? 0 : (2 * UI.Scale(10))), clientRectangle.Y + (clientRectangle.Height / 2));

			return;
		}

		if (IsGroupHeader)
		{
			using var brush = new SolidBrush(FormDesign.Design.LabelColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center };
			using var font = UI.Font(8.25F, FontStyle.Bold).FitTo(LocaleHelper.GetGlobalText(GroupText).ToString().ToUpper(), clientRectangle, e.Graphics);

			e.Graphics.DrawString(LocaleHelper.GetGlobalText(GroupText).ToString().ToUpper(), font, brush, clientRectangle, format);

			return;
		}

		var bar = UI.Scale(4);
		var back = Color.Empty;
		var fore = e.BackColor == FormDesign.Design.MenuColor ? FormDesign.Design.MenuForeColor : FormDesign.Design.ForeColor;

		if (IsSubItem)
		{
			clientRectangle = clientRectangle.Pad(small ? bar : IconManager.GetNormalScale(), 0, 0, 0);

			using var pen = new Pen(Color.FromArgb(200, FormDesign.Design.AccentColor), (float)(1.5 * UI.FontScale));
			e.Graphics.DrawLine(pen, (clientRectangle.X / 2) + bar, clientRectangle.Y / 2, (clientRectangle.X / 2) + bar, clientRectangle.Bottom);
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
			{
				e.Graphics.FillRoundedRectangle(brush, new Rectangle(clientRectangle.Right - (3 * bar / 2) - 1, clientRectangle.Y + 1, bar * 3 / 2, clientRectangle.Height - 2), bar * 3 / 4);
			}
		}

		if (PanelItem.Highlighted)
		{
			using var pen = new Pen(Color.FromArgb(100, FormDesign.Design.ActiveForeColor), 2F) { DashStyle = DashStyle.Dash };
			e.Graphics.DrawRoundedRectangle(pen, clientRectangle.Pad(0, 1, 0, 1), bar);
		}

		var iconWidth = 0;

		if (!string.IsNullOrEmpty(PanelItem.ShowKey))
		{
			using var font = UI.Font(8.25F, FontStyle.Bold);
			var roundRect = clientRectangle.Align(new Size(clientRectangle.Width, IconManager.GetNormalScale()), ContentAlignment.MiddleLeft);

			roundRect = roundRect.Pad(small ? ((clientRectangle.Width - roundRect.Height) / 2) : UI.Scale(7), 0, 0, 0);
			roundRect.Width = iconWidth = roundRect.Height;

			using (var brush = new SolidBrush(FormDesign.Design.ActiveColor))
			{
				e.Graphics.FillRoundedRectangle(brush, roundRect, bar);
			}

			using (var brush = new SolidBrush(FormDesign.Design.ActiveForeColor))
			using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
			{
				e.Graphics.DrawString(PanelItem.ShowKey, font, brush, roundRect.Pad(0, 1, -1, -1), stringFormat);
			}
		}
		else if (PanelItem.Icon != null)
		{
			using var image = new Bitmap(PanelItem.Icon);
			iconWidth = image.Width;

			var imageRect = clientRectangle.Pad(small ? (clientRectangle.Width - image.Width) / 2 : UI.Scale(7), 0, 0, 0).Align(image.Size, ContentAlignment.MiddleLeft);

			if (PanelItem.Loading)
			{
				panelItemControl.DrawLoader(e.Graphics, imageRect);
			}
			else
			{
				e.Graphics.DrawImage(image.Color(fore), imageRect);
			}
		}
		else
		{
			using var image = PanelItem.IconName?.Get(UI.Scale(18));
			if (image != null)
			{
				iconWidth = image.Width;

				var imageRect = clientRectangle.Pad(small ? (clientRectangle.Width - image.Width) / 2 : UI.Scale(7), 0, 0, 0).Align(image.Size, ContentAlignment.MiddleLeft);

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

		if (!small)
		{
			using var brush = SlickControl.Gradient(clientRectangle, fore);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center };
			var textRect = new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
			var text = LocaleHelper.GetGlobalText(PanelItem.Text);

			textRect = textRect.Pad(UI.Scale(10) + iconWidth, bar, bar, bar);

			using var font = UI.Font(8.25F).FitTo(text, textRect, e.Graphics);

			e.Graphics.DrawString(text, font, brush, textRect, format);
		}
	}
}
