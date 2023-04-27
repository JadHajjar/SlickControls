using Extensions;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace SlickControls
{
	public partial class PanelContent : SlickControl
	{
		private BasePanelForm form;
		private Thread LoadThread;
		private Padding? defaultPadding;

		public event EventHandler Shown;

		public PanelContent() : this(false)
		{ }

		public PanelContent(bool load)
		{
			InitializeComponent();
			TabStop = false;
			DoubleBuffered = true;
			ResizeRedraw = false;
			EnterTriggersClick = false;
			Font = UI.Font(8.25F);

			timer.Tick += Timer_Tick;
			
			VisibleChanged += (s, e) => timer.Enabled = Visible && loading;

			if (!DesignMode)
				DataLoaded = !load;
		}

		[Category("Design")]
		public SlickButton AcceptButton { get; set; }

		[Category("Design")]
		public SlickButton CancelButton { get; set; }

		[Browsable(false)]
		public bool DataLoaded { get; private set; }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public BasePanelForm Form { get => form ?? (form = FindForm() as BasePanelForm); set => form = value; }

		[Browsable(false)]
		public bool DataLoading { get; private set; }

		[Category("Behavior"), DefaultValue(true)]
		public bool ApplyMouseDown { get; set; } = true;

		[Category("Behavior"), DefaultValue(true)]
		public bool HideWindowIcons { get; set; }

		[Category("Design")]
		protected Control FirstFocusedControl { get; set; }

		[Category("Design"), DisplayName("Label Bounds")]
		public Point LabelBounds { get => base_Text.Location; set => base_Text.Location = value; }

		public static PanelContent GetParentPanel(Control ctrl)
		{
			while (ctrl != null && !(ctrl is PanelContent))
				ctrl = ctrl.Parent;

			return (PanelContent)ctrl;
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set => base_Text.Text = base.Text = value; }

		[Browsable(false)]
		internal bool PanelWasSetUp { get; set; } = false;

		[Browsable(false)]
		public PanelItem PanelItem { get; internal set; }

		public virtual bool CanExit(bool toBeDisposed) => true;

		public virtual bool KeyPressed(ref Message msg, Keys keyData)
			=> false;

		public virtual bool KeyPressed(char keyChar)
			=> false;

		public virtual Color GetTopBarColor()
			=> FormDesign.Design.BackColor;

		protected void AbortLoad()
		{
			if (DataLoading)
			{
				if (!IsDisposed)
					StopLoader();

				this.TryInvoke(OnLoadFail);
				LoadThread?.Interrupt();
				LoadThread?.Abort();
				DataLoading = false;
			}
		}

		protected override void UIChanged()
		{
			Font = UI.Font(8.25F);
			Padding = UI.Scale(defaultPadding ?? new Padding(5, 30, 5, 5), UI.FontScale);
			SetBackIcon();
			base_Text.Font = UI.Font(11.25F);
			base_Text.Size = base_Text.GetPreferredSize(Size.Empty);
			base_Text.Location = new Point((int)Math.Ceiling(3F * (float)UI.FontScale) + ((int)(5 * UI.UIScale) - Padding.Left), (int)Math.Ceiling(3F * (float)UI.FontScale));
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			BackColor = design.BackColor;
			ForeColor = design.ForeColor;
		}

		protected virtual bool LoadData()
		{
			return true;
		}

#if NET47
		protected virtual Task<bool> LoadDataAsync()
		{
			return Task.FromResult(true);
		}
#endif

		protected DialogResult ShowPrompt(Exception exception, string message, string title, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None)
			=> MessagePrompt.Show(exception, message, title, buttons, icon, Form);

		protected DialogResult ShowPrompt(Exception exception, string message, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None)
			=> MessagePrompt.Show(exception, message, buttons, icon, Form);

		protected DialogResult ShowPrompt(string message, string title, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None)
			=> MessagePrompt.Show(message, title, buttons, icon, Form);

		protected DialogResult ShowPrompt(string message, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None)
			=> MessagePrompt.Show(message, buttons, icon, Form);

		protected InputResult ShowInputPrompt(string message, string title, string defaultValue = "", PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None, Func<string, bool> inputValidation = null)
			=> MessagePrompt.ShowInput(message, title, defaultValue, buttons, icon, inputValidation, Form);

		protected InputResult ShowInputPrompt(string message, string defaultValue = "", PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None, Func<string, bool> inputValidation = null)
			=> MessagePrompt.ShowInput(message, "", defaultValue, buttons, icon, inputValidation, Form);

		protected override void OnCreateControl()
		{
			base_Text.BringToFront();

			SetBackIcon();

			base.OnCreateControl();
		}

		private void SetBackIcon()
		{
			if (Form?.PanelHistory?.Any(x => x != this) ?? false)
			{
				base_Text.Image = UI.FontScale > 2 ? Properties.Resources.I_ArrowLeft_32 : UI.FontScale >= 1.5 ? Properties.Resources.I_ArrowLeft_24 : Properties.Resources.I_ArrowLeft_16;
				base_Text.Enabled = true;
				SlickTip.SetTo(base_Text, string.Format(LocaleHelper.GetGlobalText("Go back to {0}"), Form.PanelHistory.Last().Text));
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			if (defaultPadding == null)
				defaultPadding = Padding;

			base.OnHandleCreated(e);

			if (!IsDisposed && !DesignMode)
			{
				if (Form == null)
					Form = (BasePanelForm)FindForm();

				if (Form != null && !DataLoaded)
					StartDataLoad();
			}
		}

		protected virtual void OnDataLoad()
		{ }

		protected virtual void OnLoadFail()
		{ }

		public virtual bool OnWndProc(Message m)
			=> false;

		protected void StartDataLoad()
		{
			if (!DesignMode && !DataLoading)
			{
				DataLoaded = false;
				DataLoading = true;
				StartLoader();
#if NET47
				LoadThread = new Thread(async () =>
#else
				LoadThread = new Thread(() =>
#endif
				{
					try
					{
#if NET47
						if (LoadData() && await LoadDataAsync())
#else
						if (LoadData())
#endif
						{
							DataLoaded = true;
							this.TryInvoke(OnDataLoad);
						}
						else
						{
							this.TryInvoke(OnLoadFail);
						}
					}
					catch { }

					if (!IsDisposed)
						StopLoader();

					DataLoading = false;
					LoadThread = null;
				})
				{ IsBackground = true };

				LoadThread.Start();
			}
		}

		private void PanelContent_Load(object sender, EventArgs e)
		{
			if (FirstFocusedControl != null)
				BeginInvoke(new Action(() => FirstFocusedControl.Focus()));
		}

		internal void OnShown() => Shown?.Invoke(this, new EventArgs());

		private void base_Text_Click(object sender, EventArgs e) => Form?.PushBack();

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
				Invalidate(new Rectangle(0, 0, Width, (int)Math.Ceiling(3F * (float)UI.FontScale)));
			});
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Parent != null)
				{
					perc += (1 + (Math.Abs(50 - perc) / 25)) / 1.75;
					if (perc >= 200)
						perc = 0;

					Invalidate(new Rectangle(0, 0, Width, (int)Math.Ceiling(3F * (float)UI.FontScale)));
				}
			}
			catch { }
		}

		private void PanelContent_Paint(object sender, PaintEventArgs e)
		{
			var w = Width - 115;
			e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, w, 2);

			if (loading)
			{
				var width = (int)((w / 2 * 50 / Math.Abs(25 - perc)).Between(50, w / 2) - (Math.Pow(Math.Abs(25 - perc), 2) * w / 12500 / (perc - 75).Between(20, 35) / 20).Between(0, w / 4));
				var x = (int)(perc * (w + 0) / 100) - width;

				if (x >= w)
					perc = -35;

				var rect = new RectangleF(x, 0, Math.Min(width, w-x), 3F * (float)UI.FontScale);
				e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect);
				e.Graphics.FillRectangle(new LinearGradientBrush(new PointF(0, 0), new PointF(150, 0), BackColor, Color.Empty), new RectangleF(0, 0, 150, rect.Height));
				e.Graphics.FillRectangle(new LinearGradientBrush(new PointF(w - 150, 0), new PointF(w, 0), Color.Empty, BackColor), new RectangleF(w - 150, 0, 151, rect.Height));
			}
		}

		public virtual void GlobalMouseMove(Point p)
		{

		}

		#endregion Loader
	}
}