using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls;

[DefaultEvent("OnClick")]
public class PanelItem : Component
{
	private bool _loading;

	[Category("Appearance"), DisplayName("Text"), DefaultValue(null)]
	public string Text { get; set; }
	[Category("Appearance"), DisplayName("Image"), DefaultValue(null)]
	public Bitmap Icon { get; set; }

	[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
	public DynamicIcon IconName { get; set; }
	[Browsable(false), DefaultValue(false)]
	public bool Selected { get; set; }
	[Category("Design"), DisplayName("Group"), DefaultValue(null)]
	public string Group { get; set; }
	[Category("Behavior"), DisplayName("Force Re-open"), DefaultValue(false)]
	public bool ForceReopen { get; set; }
	[Browsable(false), DefaultValue(false)]
	public bool Highlighted { get; set; }
	[Browsable(false), DefaultValue(null)]
	public object Data { get; set; }
	[Category("Appearance"), DisplayName("Hidden"), DefaultValue(false)]
	public bool Hidden { get; set; }
	[Browsable(false), DefaultValue(false)]
	internal string ShowKey { get; set; }

	[Category("Design"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public PanelItem[] SubItems { get; set; } = new PanelItem[0];
	[Browsable(false), DefaultValue(false)]
	public bool Loading
	{
		get => _loading; set
		{
			_loading = value;
			LoadingStateChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
	public event MouseEventHandler OnClick;

	[Browsable(false)]
	internal event EventHandler LoadingStateChanged;

	public void MouseClick(MouseEventArgs e)
	{
		OnClick?.Invoke(this, e);
	}

	public override string ToString()
	{
		return $"[{Group}] {Text}";
	}

	public static PanelItem Empty = new();
}