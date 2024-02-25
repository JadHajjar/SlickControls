using Extensions;

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SlickControls.Controls.Advanced;
public class SlickWebBrowser : WebBrowser
{
	private string body;
	private string head;
	private string style;

	public string Body { get => body; set { body = value; UpdateDocument(); } }
	public string Head { get => head; set { head = value; UpdateDocument(); } }

	public SlickWebBrowser()
	{
		IsWebBrowserContextMenuEnabled = false;
		WebBrowserShortcutsEnabled = false;
		FormDesign.DesignChanged += FormDesign_DesignChanged;
		UI.UIChanged += UI_UIChanged;
	}

	protected override void Dispose(bool disposing)
	{
		FormDesign.DesignChanged -= FormDesign_DesignChanged;
		UI.UIChanged -= UI_UIChanged;

		base.Dispose(disposing);
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		UpdateStyle(false);
	}

	private void FormDesign_DesignChanged(FormDesign design)
	{
		UpdateStyle();
	}

	private void UI_UIChanged()
	{
		UpdateStyle();
	}

	public void UpdateStyle(bool refresh = true)
	{
		using var resourceStream = GetType().Assembly.GetManifestResourceStream("SlickControls.Controls.Advanced.WebBrowser.css");
		using var reader = new StreamReader(resourceStream, Encoding.UTF8);

		var headerColor = FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.ActiveColor, 25);
		style = reader.ReadToEnd()
			.Replace("AccentColor", RGB(FormDesign.Design.AccentColor))
			.Replace("AccentBackColor", RGB(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor)))
			.Replace("ActiveColor", RGB(FormDesign.Design.ActiveColor))
			.Replace("BackColor", RGB(FormDesign.Design.BackColor))
			.Replace("ForeColor", RGB(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor, 85)))
			.Replace("LinkColor", RGB(FormDesign.Design.ForeColor))
			.Replace("Header1Color", RGB(FormDesign.Design.ForeColor))
			.Replace("Header2Color", RGB(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor, 90)))
			.Replace("Header3Color", RGB(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.BackColor, 80)))
			.Replace("FontFamily", UI.FontFamily)
			.Replace("PadSize", (UI.FontScale * 3).ToString("0.00"))
			.Replace("H3FontSize", (UI.FontScale * 9).ToString("0.00"))
			.Replace("H2FontSize", (UI.FontScale * 10).ToString("0.00"))
			.Replace("H1FontSize", (UI.FontScale * 12).ToString("0.00"))
			.Replace("FontSize", (UI.FontScale * 8.25).ToString("0.00"));

		if (refresh)
		{
			UpdateDocument();
		}
	}

	public static string RGB(Color color) => $"rgb({color.R}, {color.G}, {color.B})";

	private void UpdateDocument()
	{
		DocumentText = $"<html><head><style>{style}</style>{Head}</head><body>{Body}</body></html>";
	}
}
