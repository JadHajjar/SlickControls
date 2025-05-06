using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls;

public partial class PanelContent : SlickControl
{
	private BasePanelForm form;
	private Thread LoadThread;
	private bool softDispose;
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

		if (!DesignMode)
		{
			DataLoaded = !load;
		}
	}

	[Category("Design")]
	public SlickButton AcceptButton { get; set; }

	[Category("Design")]
	public SlickButton CancelButton { get; set; }

	[Browsable(false)]
	public bool DataLoaded { get; private set; }

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BasePanelForm Form { get => form ??= FindForm() as BasePanelForm; set => form = value; }

	[Browsable(false)]
	public bool DataLoading { get; private set; }

	[Category("Behavior"), DefaultValue(true)]
	public bool ApplyMouseDown { get; set; } = true;

	[Category("Behavior"), DefaultValue(true)]
	public bool HideWindowIcons { get; set; }

	[Category("Design")]
	protected Control FirstFocusedControl { get; set; }

	[Category("Design"), DisplayName("Label Bounds"), DefaultValue(typeof(Point), "0, 0")]
	public Point CustomTitleBounds { get; set; }

	public static PanelContent GetParentPanel(Control ctrl)
	{
		while (ctrl is not null and not PanelContent)
		{
			ctrl = ctrl.Parent;
		}

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

	public virtual bool CanExit(bool toBeDisposed)
	{
		return true;
	}

	public virtual bool KeyPressed(ref Message msg, Keys keyData)
	{
		return false;
	}

	public virtual bool KeyPressed(char keyChar)
	{
		return false;
	}

	public virtual Color GetTopBarColor()
	{
		return FormDesign.Design.BackColor;
	}

	protected void AbortLoad()
	{
		if (DataLoading)
		{
			if (!IsDisposed)
			{
				StopLoader();
			}

			this.TryInvoke(OnLoadFail);
			LoadThread?.Interrupt();
			LoadThread?.Abort();
			DataLoading = false;
		}
	}

	protected override void UIChanged()
	{
		Font = UI.Font(8.25F);
		Padding = UI.Scale(defaultPadding ?? new Padding(5, 30, 5, 5));
		SetBackIcon();
		base_Text.Padding = UI.Scale(new Padding(3));
		base_Text.Font = UI.Font(10F, FontStyle.Bold);
		base_Text.PerformAutoScale();
		base_Text.Location = new Point(CustomTitleBounds.X == 0 ? UI.Scale(5) : CustomTitleBounds.X, CustomTitleBounds.Y == 0 ? (UI.Scale(30) - base_Text.Height) / 2 : CustomTitleBounds.Y);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		BackColor = GetTopBarColor();
		ForeColor = BackColor.GetTextColor();
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

	protected DialogResult ShowPrompt(Exception exception, string message, string title, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.Error)
	{
		return MessagePrompt.Show(exception, message, title, buttons, icon, Form);
	}

	protected DialogResult ShowPrompt(Exception exception, string message, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.Error)
	{
		return MessagePrompt.Show(exception, message, buttons, icon, Form);
	}

	protected DialogResult ShowPrompt(string message, string title, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None)
	{
		return MessagePrompt.Show(message, title, buttons, icon, Form);
	}

	protected DialogResult ShowPrompt(string message, PromptButtons buttons = PromptButtons.OK, PromptIcons icon = PromptIcons.None)
	{
		return MessagePrompt.Show(message, buttons, icon, Form);
	}

	protected InputResult ShowInputPrompt(string message, string title, string defaultValue = "", PromptButtons buttons = PromptButtons.OKCancel, PromptIcons icon = PromptIcons.None, Func<string, bool> inputValidation = null)
	{
		return MessagePrompt.ShowInput(message, title, defaultValue, buttons, icon, inputValidation, Form);
	}

	protected InputResult ShowInputPrompt(string message, string defaultValue = "", PromptButtons buttons = PromptButtons.OKCancel, PromptIcons icon = PromptIcons.None, Func<string, bool> inputValidation = null)
	{
		return MessagePrompt.ShowInput(message, "", defaultValue, buttons, icon, inputValidation, Form);
	}

	protected override void OnCreateControl()
	{
		base_Text.BringToFront();

		SetBackIcon();

		base.OnCreateControl();
	}

	internal void SoftDispose()
	{
		softDispose = true;
		Dispose();
		softDispose = false;
	}

	private void SetBackIcon()
	{
		if (Form?.PanelHistory?.Any(x => x != this) ?? false)
		{
			base_Text.ImageName = "ArrowLeft";
			base_Text.Enabled = true;
			base_Text.PerformAutoScale();
			base_Text.Location = new Point(CustomTitleBounds.X == 0 ? UI.Scale(5) : CustomTitleBounds.X, CustomTitleBounds.Y == 0 ? (UI.Scale(30) - base_Text.Height) / 2 : CustomTitleBounds.Y);
			SlickTip.SetTo(base_Text, string.Format(LocaleHelper.GetGlobalText("Go back to {0}"), Form.PanelHistory.Last().Text));
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (defaultPadding == null)
		{
			defaultPadding = Padding;
		}

		base.OnHandleCreated(e);

		if (!IsDisposed && !DesignMode)
		{
			Form ??= (BasePanelForm)FindForm();

			if (Form != null && !DataLoaded)
			{
				StartDataLoad();
			}
		}
	}

	protected virtual void OnDataLoad()
	{ }

	protected virtual void OnLoadFail()
	{ }

	public virtual bool OnWndProc(Message m)
	{
		return false;
	}

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
				catch
				{
					this.TryInvoke(OnLoadFail);
				}

				if (!IsDisposed)
				{
					StopLoader();
				}

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
		{
			BeginInvoke(new Action(() => FirstFocusedControl.Focus()));
		}
	}

	protected internal virtual void OnShown()
	{
		Shown?.Invoke(this, new EventArgs());
	}

	private void base_Text_Click(object sender, EventArgs e)
	{
		Form?.PushBack();
	}

	protected void PushBack()
	{
		if (Form?.CurrentPanel == this)
		{
			Form.PushBack();
		}
	}

	#region Loader

	public void StartLoader()
	{
		Loading = true;
	}

	public void StopLoader()
	{
		Loading = false;
	}

	protected override void InvalidateForLoading()
	{
		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		if (!Loading)
		{
			return;
		}

		var maxwWidth = Width - UI.Scale(115);
		var width = maxwWidth / 2;
		var x = (int)((LoaderPercentage - 100) * maxwWidth / 100) + width;
		var rect = Rectangle.Intersect(new Rectangle(x, 0, width, (int)Math.Ceiling(3F * (float)UI.FontScale)), new Rectangle(0, 0, maxwWidth, (int)Math.Ceiling(3F * (float)UI.FontScale)));

		if (rect.Width <= 5)
		{
			return;
		}

		using var brush = new LinearGradientBrush(rect.Pad(-1), default, default, 0f);

		if (rect.Width > UI.Scale(200))
		{
			brush.InterpolationColors = new ColorBlend(5)
			{
				Colors = [default, Color.FromArgb(100, FormDesign.Design.ActiveColor), FormDesign.Design.ActiveColor, Color.FromArgb(100, FormDesign.Design.ActiveColor), default],
				Positions = [0f, 0.25f, 0.5f, 0.75f, 1f]
			};
		}
		else
		{
			brush.InterpolationColors = new ColorBlend(3)
			{
				Colors = [default, Color.FromArgb(Math.Min((int)(rect.Width / UI.FontScale / 2), 100), FormDesign.Design.ActiveColor), default],
				Positions = [0f, 0.5f, 1f]
			};
		}

		e.Graphics.FillRectangle(brush, rect);
	}

	protected internal virtual void GlobalMouseMove(Point p)
	{

	}

	protected internal virtual object GetTransitoryData()
	{
		return null;
	}

	protected internal virtual void UseTransitoryData(object data)
	{

	}

	#endregion Loader
}