using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace SlickControls
{
	public partial class BaseForm : SlickForm
	{
		#region Private Fields

		private Image formIcon;
		private bool showControls = false;

		public new string Text { get => base_L_Title.Text; set => base.Text = base_L_Title.Text = value; }

		#endregion Private Fields

		#region Public Constructors

		public BaseForm() : this(false)
		{ }

		public BaseForm(bool initialized)
		{
			InitializeComponent();

			if (!initialized)
				FormDesign.DesignChanged += DesignChanged;

			timer.Tick += Timer_Tick;

			VisibleChanged += (s, e) => timer.Enabled = Visible && loading;

			base_L_Title.MouseDown += Form_MouseDown;
			base_P_Controls.MouseDown += Form_MouseDown;
			base_P_Top_Spacer.MouseDown += Form_MouseDown;
		}

		#endregion Public Constructors

		#region Properties

		[Category("Appearance")]
		public override Image FormIcon { get => formIcon; set => base_PB_Icon.Image = formIcon = value.Color(FormDesign.Design.IconColor); }

		[Category("Appearance")]
		public override Rectangle IconBounds { get => base_PB_Icon.Bounds; set => base_PB_Icon.Bounds = value; }

		[Category("Behavior"), EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Bindable(true)]
		public bool ShowControls
		{
			get => base_P_Controls.Visible = showControls;
			set
			{
				base_P_Controls.Visible = showControls = value;
				if (value && base_P_Controls.Height == 0)
					base_P_Controls.Height = 45;
			}
		}

		protected new bool MaximizeBox { get => base.MaximizeBox; set => base_B_Max.Visible = base.MaximizeBox = value; }

		protected new bool MinimizeBox { get => base.MinimizeBox; set => base_B_Min.Visible = base.MinimizeBox = value; }

		#endregion Properties

		#region General Methods

		protected override void UIChanged()
		{
			base.UIChanged();

			base_L_Title.Padding = UI.Scale(new Padding(5, 0, 0, 0), UI.FontScale);
			base_L_Title.Font = UI.Font(7.5F, FontStyle.Bold);
			base_P_Top.Height = (int)(24 * UI.UIScale);
			base_B_Close.Size = base_B_Max.Size = base_B_Min.Size = new Size((int)(24 * UI.UIScale) - 6, (int)(24 * UI.UIScale));
			base_P_Top_Spacer.Height = (int)Math.Ceiling(3F * (float)UI.FontScale);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			ShowControls = ShowControls;

			if (base_PB_Icon.Image == null)
				base_PB_Icon.Hide();
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			base_P_Top.BackColor = base_P_Controls.BackColor = base_P_Top_Spacer.BackColor = design.AccentBackColor;
			base_L_Title.ForeColor = base_P_Controls.ForeColor = design.ForeColor;
			base_P_Content.BackColor = design.BackColor;
			base_PB_Icon.Color(design.ForeColor);
		}

		private void BaseForm_Load(object sender, EventArgs e)
		{
			recurse(base_P_Controls);

			void recurse(Control ctrl)
			{
				if (ctrl is Panel || ctrl is UserControl)
				{
					ctrl.MouseDown += Form_MouseDown;

					foreach (var item in ctrl.Controls.ThatAre<Panel>())
						recurse(item);

					foreach (var item in ctrl.Controls.ThatAre<Label>())
						item.MouseDown += Form_MouseDown;
				}
			}
		}

		private void L_Title_TextChanged(object sender, EventArgs e)
		{
			if (base_L_Title.Text != Text)
				Text = base_L_Title.Text;
		}

		#endregion General Methods

		#region Loader

		private readonly Timer timer = new Timer { Interval = 14 };
		private double perc = -20;
		private bool loading;

		public void StartLoader()
		{
			this.TryInvoke(() => timer.Enabled = loading = true);
		}

		public void StopLoader()
		{
			this.TryInvoke(() =>
			{
				timer.Enabled = loading = false;
				base_P_Top_Spacer.Invalidate();
			});
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			try
			{
				perc += (1 + (Math.Abs(50 - perc) / 25)) / 1.75;
				if (perc >= 200)
					perc = 0;

				base_P_Top_Spacer.Invalidate();
			}
			catch { }
		}

		private void P_Top_Spacer_Paint(object sender, PaintEventArgs e)
		{
			var w = base_P_Top_Spacer.Width - 115;
			e.Graphics.FillRectangle(new SolidBrush(base_P_Top.BackColor), 0, 0, w, 2);

			if (loading)
			{
				var width = (int)((w / 2 * 50 / Math.Abs(25 - perc)).Between(50, w / 2) - (Math.Pow(Math.Abs(25 - perc), 2) * w / 12500 / (perc - 75).Between(20, 35) / 20).Between(0, w / 4));
				var x = (int)(perc * (w + 0) / 100) - width;

				if (x >= w)
					perc = -35;

				var rect = new RectangleF(x, 0, Math.Min(width, w - x), 3F * (float)UI.FontScale);
				e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect);
				e.Graphics.FillRectangle(new LinearGradientBrush(new PointF(0, 0), new PointF(150, 0), base_P_Top.BackColor, Color.Empty), new RectangleF(0, 0, 150, rect.Height));
				e.Graphics.FillRectangle(new LinearGradientBrush(new PointF(w - 150, 0), new PointF(w, 0), Color.Empty, base_P_Top.BackColor), new RectangleF(w - 150, 0, 151, rect.Height));
			}
		}

		#endregion Loader
	}
}