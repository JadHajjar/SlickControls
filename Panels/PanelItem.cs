using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("OnClick")]
	public class PanelItem : Component
	{
		public string Text { get; set; }
		[DefaultValue(null)]
		public Bitmap Icon { get; set; }
		public bool Selected { get; set; }
		public string Group { get; set; }
		public bool ForceReopen { get; set; }
		public bool Highlighted { get; set; }

		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		//public bool Loading { get => Control?.Loading ?? false; set => Control.Loading = value; }

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public event MouseEventHandler OnClick;

		public void MouseClick(MouseEventArgs e) => OnClick?.Invoke(this, e);

		public override string ToString() => $"[{Group}] {Text}";

		public static PanelItem Empty = new PanelItem();
	}
}