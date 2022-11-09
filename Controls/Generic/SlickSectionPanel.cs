using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class SlickSectionPanel : FlowLayoutPanel
	{
		private bool _active;
		private string info;
		private Image icon;

		public SlickSectionPanel()
		{
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Padding = new Padding(43, 54, 0, 0);
			DoubleBuffered = ResizeRedraw = true;

			FormDesign.DesignChanged += DesignChanged;
			UI.UIChanged += UIChanged;
			AutoSizeChanged += SectionPanel_AutoSizeChanged;
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public new string Text { get => base.Text; set { base.Text = value; Invalidate(); } }

		[Category("Appearance")]
		public string Info { get => info; set { info = value; this.TryInvoke(Invalidate); } }

		[Category("Appearance")]
		public Image Icon { get => icon; set { icon = value; this.TryInvoke(Invalidate); } }

		[Category("Appearance")]
		public bool Active { get => _active; set { _active = value; this.TryInvoke(Invalidate); } }

		[Category("Appearance")]
		public string[] Flavor { get; set; }

		protected void DesignChanged(FormDesign design) => this.TryInvoke(Invalidate);

		[Category("Behavior")]
		public bool AutoHide { get; set; }

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (!DesignMode)
				UIChanged();

			if (Flavor?.Any() ?? false)
				Info = Flavor.Random();

			if (Parent != null)
			{
				MaximumSize = new Size(Parent?.Width ?? Width, int.MaxValue);
				Parent.Resize += (s, ee) => MaximumSize = new Size(Parent?.Width ?? Width, int.MaxValue);
			}

			if (AutoHide && !DesignMode)
				BeginInvoke(new Action(() =>
				{
					Visible = Controls.Count > 0;
					ControlRemoved += (_, __) => Visible = Controls.Count > 0;
					ControlAdded += (_, __) => Visible = true;
				}));
		}

		private void SectionPanel_AutoSizeChanged(object sender, EventArgs e)
		{
			AutoSize = AutoSize;

			Dock = AutoSize ? DockStyle.Top : DockStyle.Fill;
		}

		public void Add(params Control[] controls) => Controls.AddRange(controls);

		public void Add(IEnumerable<Control> controls) => Controls.AddRange(controls.ToArray());

		public void Remove(params Control[] controls)
		{
			foreach (var item in controls)
				Controls.Remove(item);
		}

		public void Remove(IEnumerable<Control> controls)
		{
			foreach (var item in controls)
				Controls.Remove(item);
		}

		protected void UIChanged()
		{
			Padding = new Padding(43, UI.Font(9.75F, FontStyle.Bold).Height + 24 + 8 + 4, 0, 0);
			MinimumSize = new Size(150, Padding.Top + 1);
			MaximumSize = new Size(Parent?.Width ?? Width, int.MaxValue);
			Invalidate();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				FormDesign.DesignChanged -= DesignChanged;
				UI.UIChanged -= UIChanged;
			}

			try { base.Dispose(disposing); } catch { }
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var bnds = e.Graphics.MeasureString(Text, UI.Font(9.75F, FontStyle.Bold));
			e.Graphics.DrawString(Text, UI.Font(9.75F, FontStyle.Bold), new SolidBrush(Active ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor), 50, 24);

			if (icon != null)
				e.Graphics.DrawImage(new Bitmap(icon, 22, 22).Color(Active ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), 15, 24 + (int)((bnds.Height - 22) / 2));

			var flavFont = UI.Font(8.25F, FontStyle.Italic);

			e.Graphics.DrawString(info, flavFont, new SolidBrush(FormDesign.Design.InfoColor), 58 + (int)bnds.Width, 24 + UI.Font(9.75F, FontStyle.Bold).Height - flavFont.Height);

			var w = 58 + (int)(bnds.Width + e.Graphics.MeasureString(info, flavFont).Width);
			e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor, 1), 50, (int)bnds.Height + 24 + 8, Math.Max(Controls.Count == 0 ? Width - 32 : Math.Min(Math.Max(w, Controls.Max(x => x.Left + x.Width)), Width - 32), 200), (int)bnds.Height + 24 + 8);

			w = Math.Max(MinimumSize.Width, w + 30);
			MinimumSize = new Size(Math.Min(w, Parent?.Width ?? w), Padding.Top + 1);
		}
	}
}