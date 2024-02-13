using Extensions;

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

		style = reader.ReadToEnd()
			.Replace("AccentColor", rgb(FormDesign.Design.AccentColor))
			.Replace("AccentBackColor", rgb(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor)))
			.Replace("ActiveColor", rgb(FormDesign.Design.ActiveColor))
			.Replace("BackColor", rgb(FormDesign.Design.BackColor))
			.Replace("ForeColor", rgb(FormDesign.Design.ForeColor))
			.Replace("FontFamily", UI.FontFamily)
			.Replace("H3FontSize", (UI.FontScale * 9.75).ToString("0.00"))
			.Replace("H2FontSize", (UI.FontScale * 11.5).ToString("0.00"))
			.Replace("H1FontSize", (UI.FontScale * 13.5).ToString("0.00"))
			.Replace("FontSize", (UI.FontScale * 8.25).ToString("0.00"));

		if (refresh)
		{
			UpdateDocument();
		}

		static string rgb(Color color) => $"rgb({color.R}, {color.G}, {color.B})";
	}

	private void UpdateDocument()
	{
		DocumentText = $"<html><head><style>{style}</style>{Head}</head><body>{Body}</body></html>";
	}
}
