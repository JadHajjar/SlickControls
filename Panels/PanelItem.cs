using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("OnClick")]
	public class PanelItem : Component
	{
		[Category("Appearance"), DisplayName("Text"), DefaultValue(null)]
		public string Text { get; set; }
		[Category("Appearance"), DisplayName("Image"), DefaultValue(null)]
		public Bitmap Icon { get; set; }

		[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
		public DynamicIcon IconName { get; set; }
		[Browsable(false)]
		public bool Selected { get; set; }
		[Category("Design"), DisplayName("Group"), DefaultValue(null)]
		public string Group { get; set; }
		[Category("Behavior"), DisplayName("Force Re-open"), DefaultValue(false)]
		public bool ForceReopen { get; set; }
		[Browsable(false)]
		public bool Highlighted { get; set; }
		[Browsable(false)]
		public object Data { get; set; }
		[Category("Appearance"), DisplayName("Hidden"), DefaultValue(false)]
		public bool Hidden { get; set; }
		[Browsable(false)]
		internal string ShowKey { get; set; }

		[Category("Design"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public PanelItem[] SubItems { get; set; } = new PanelItem[0];
		public bool Loading { get; set; }

		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
		public event MouseEventHandler OnClick;

		public void MouseClick(MouseEventArgs e) => OnClick?.Invoke(this, e);

		public override string ToString() => $"[{Group}] {Text}";

		public static PanelItem Empty = new PanelItem();
	}
}