using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls;

public partial class NotificationForm : Form
{
	private readonly Form _form;
	private long epoch;
	private long lastTick;
	private bool closing;
	private bool loaded;
	private readonly bool loading;
	private double percentage;
	private double loaderPercentage;

	private static DateTime lastSoundPlay;
	private Rectangle closeRect;
	private bool hovered;
	private static readonly Dictionary<Form, List<NotificationForm>> Notifications = [];
	private static readonly List<NotificationForm> Notifications_Screen = [];

	public Notification Notification { get; }

	private NotificationForm(Notification notification, Form form = null, NotificationSound sound = NotificationSound.Short, int? timeoutSeconds = null)
	{
		_form = form;
		Notification = notification;
		loaded = AnimationHandler.NoAnimations;
		percentage = loaded ? 1 : 0;
		loading = notification.Icon == PromptIcons.Loading;

		FormBorderStyle = FormBorderStyle.None;
		AutoScaleMode = AutoScaleMode.None;
		StartPosition = FormStartPosition.Manual;
		DoubleBuffered = true;
		ResizeRedraw = true;
		ShowIcon = false;
		ShowInTaskbar = false;
		Padding = UI.Scale(new Padding(5), UI.FontScale);
		TransparencyKey = BackColor = Color.FromArgb(64, 64, 0);
		Size = Size.Empty;

		if (notification.Action != null)
		{
			Cursor = Cursors.Hand;
		}

		if (form != null)
		{
			form.LocationChanged += Form_Move;
			form.Resize += Form_Move;

			if (form is SlickForm slickForm)
			{
				slickForm.CurrentFormState = FormState.ForcedFocused;
			}
		}

		if (timeoutSeconds is not null and > 0)
		{
			CreateTimeoutTimer(timeoutSeconds.Value);
		}

		PlaySound(sound);

		FormDesign.DesignChanged += DesignChanged;
		UI.UIChanged += UI_UIChanged;

		epoch = DateTime.Now.Ticks;
	}

	private static void PlaySound(NotificationSound sound)
	{
		if (Notification.PlaySounds && (DateTime.Now - lastSoundPlay).TotalSeconds > 5)
		{
			switch (sound)
			{
				case NotificationSound.Short:
					using (var snd = new System.Media.SoundPlayer(Properties.Resources.Notif_Quick))
					{
						snd.Play();
					}

					break;

				case NotificationSound.Long:
					using (var snd = new System.Media.SoundPlayer(Properties.Resources.Notif_Long))
					{
						snd.Play();
					}

					break;
			}

			lastSoundPlay = DateTime.Now;
		}
	}

	private void CreateTimeoutTimer(int timeoutSeconds)
	{
		var timer = new System.Timers.Timer(timeoutSeconds * 1000D) { Enabled = true, AutoReset = false };

		timer.Elapsed += (s, e) =>
		{
			if (!IsDisposed)
			{
				this.TryInvoke(Close);
			}

			(s as System.Timers.Timer).Dispose();
		};
	}

	private void UI_UIChanged()
	{
		Size = UI.Scale(Notification.Size, UI.FontScale);
	}

	internal static void Clear()
	{
		foreach (var item in Notifications.Values.SelectMany(x => x))
		{
			item.TryInvoke(item.Dispose);
		}

		foreach (var item in Notifications_Screen)
		{
			item.TryInvoke(item.Dispose);
		}
	}

	protected override bool ShowWithoutActivation => true;

	protected override CreateParams CreateParams
	{
		get
		{
			var baseParams = base.CreateParams;

			const int WS_EX_NOACTIVATE = 0x08000000;
			const int WS_EX_TOOLWINDOW = 0x00000080;
			baseParams.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;

			return baseParams;
		}
	}

	private void DesignChanged(FormDesign design)
	{
		Invalidate();
	}

	private void Form_Move(object sender, EventArgs e)
	{
		SetLocation();
	}

	private void SetLocation()
	{
		if (_form != null)
		{
			var y = _form.Bottom - Padding.Vertical - Notifications[_form].Take(Notifications[_form].IndexOf(this) + 1).Sum(f => Padding.Vertical + f.Height);
			var x = _form.Right - Padding.Horizontal * 2 - Width;

			Location = new Point(x, y);
		}
		else
		{
			var y = Screen.PrimaryScreen.WorkingArea.Height - Padding.Vertical - ((Padding.Vertical + Height) * (Notifications_Screen.IndexOf(this) + 1));
			var x = Screen.PrimaryScreen.WorkingArea.Width - Padding.Vertical * 2 - Width;

			Location = new Point(x, y);
		}
	}

	public void SetText(string text)
	{
		Notification.Description = text;
		Invalidate();
	}


	public static NotificationForm Push(Notification notification, Form form, NotificationSound sound = NotificationSound.Short, int? timeoutSeconds = null)
	{
		NotificationForm frm = null;

		form?.TryInvoke(() =>
		{
			if (form != null && (!form.Visible || form.WindowState == FormWindowState.Minimized))
			{
				form = null;
			}

			frm = new NotificationForm(notification, form, sound, timeoutSeconds)
			{
				Size = UI.Scale(notification.Size, UI.FontScale)
			};

			if (form is null)
			{
				Notifications_Screen.Add(frm);
			}
			else if (Notifications.ContainsKey(form))
			{
				Notifications[form].Add(frm);
			}
			else
			{
				Notifications.TryAdd(form, [frm]);
			}

			frm.SetLocation();
			form?.ShowUp();

			if (form != null)
			{
				frm.Show(form);
			}
			else
			{
				frm.Show();
			}

			frm.BringToFront();
		});

		return frm;
	}

#if NET47
	protected override async void OnPaintBackground(PaintEventArgs e)
#else
	protected override void OnPaintBackground(PaintEventArgs e)
#endif
	{
		base.OnPaintBackground(e);

		if (!loaded || loading)
		{
			var oldTick = lastTick;

			lastTick = (DateTime.Now.Ticks - epoch) / TimeSpan.TicksPerMillisecond;

			if (loading && IsHandleCreated)
			{
				var val = lastTick / 600D % Math.PI;

				loaderPercentage = 100 - (100 * Math.Cos(val));
			}

			if (!loaded)
			{
				percentage = (1 - Math.Cos(lastTick / 150D)) / 2d;

				if (lastTick > Math.PI * 150)
				{
					percentage = 1;
					loaded = true;

					if (closing)
					{
						Dispose();
					}

					if (!loading)
					{
						return;
					}
				}
			}

#if NET47
			await Task.Delay(Math.Max(2, 20 - (int)(lastTick - oldTick)));
#endif

			Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (IsDisposed)
		{
			return;
		}

		e.Graphics.SetUp(BackColor);

		var mousePos = PointToClient(Cursor.Position);
		var clip = new Rectangle(closing ? (int)(Width * percentage) : (Width - (int)(Width * percentage)), 0, Width, Height);
		e.Graphics.SetClip(clip);

		e.Graphics.Clear(FormDesign.Design.AccentColor);

		GetColorIcons(out var icon, out var color);

		using (var brush = new SolidBrush(FormDesign.Design.BackColor))
		{
			e.Graphics.FillRectangle(brush, clip.Pad((int)UI.FontScale));
		}

		using (var brush = new SolidBrush(color))
		{
			e.Graphics.FillRectangle(brush, clip.X, 0, (int)(3 * UI.FontScale), Height);
		}

		if (Notification.OnPaint != null)
		{
			Notification.OnPaint(this, e.Graphics);
		}
		else
		{
			using var titleFont = UI.Font(9.75F);
			var imgRect = clip.Pad(Padding).Align(UI.Scale(new Size(26, 26), UI.FontScale), ContentAlignment.MiddleLeft);

			imgRect.X += imgRect.Width / 2;

			if (loading)
			{
				e.Graphics.DrawLoader(loaderPercentage, imgRect);
			}
			else if (icon != null)
			{
				using (icon)
				{
					e.Graphics.DrawImage(icon, imgRect.CenterR(icon.Size));
				}
			}

			using (var brush = new SolidBrush(FormDesign.Design.ForeColor))
			{
				var titleRect = clip.Pad(Padding).Pad(imgRect.Width + (2 * imgRect.Width / 2), 0, 0, 0);
				using var format = new StringFormat { LineAlignment = string.IsNullOrWhiteSpace(Notification.Description) ? StringAlignment.Center : StringAlignment.Near };

				e.Graphics.DrawString(
					Notification.Title,
					titleFont,
					brush,
					titleRect,
					format);
			}

			using (var brush = new SolidBrush(FormDesign.Design.InfoColor))
			{
				var textRect = clip.Pad(Padding).Pad(imgRect.Width + (2 * imgRect.Width / 2), string.IsNullOrWhiteSpace(Notification.Title) ? 0 : ((int)e.Graphics.Measure(Notification.Title, titleFont).Height + Padding.Vertical), 0, 0);
				using var font = UI.Font(8.25F).FitToHeight(Notification.Description, textRect, e.Graphics);
				using var format = new StringFormat { LineAlignment = string.IsNullOrWhiteSpace(Notification.Title) ? StringAlignment.Center : StringAlignment.Near };

				e.Graphics.DrawString(
					Notification.Description,
					font,
					brush,
					textRect);
			}
		}

		using var closeIcon = IconManager.GetIcon("Close");
		closeRect = clip.Align(closeIcon.Size + Padding.Size, ContentAlignment.TopRight);
		var hovered = this.hovered && closeRect.Contains(PointToClient(MousePosition));

		e.Graphics.DrawImage(closeIcon.Color(hovered ? FormDesign.Design.RedColor : FormDesign.Design.IconColor), closeRect.CenterR(closeIcon.Size));
	}

	private void GetColorIcons(out Bitmap icon, out Color color)
	{
		var design = FormDesign.Design;
		switch (Notification.Icon)
		{
			case PromptIcons.Hand:
				icon = IconManager.GetIcon("Hand", Height / 2).Color(design.LabelColor);
				color = design.ActiveColor;
				break;

			case PromptIcons.Info:
				icon = IconManager.GetIcon("Info", Height / 2).Color(design.LabelColor);
				color = design.ActiveColor;
				break;

			case PromptIcons.Input:
				icon = IconManager.GetIcon("Edit", Height / 2).Color(design.LabelColor);
				color = design.ActiveColor;
				break;

			case PromptIcons.Question:
				icon = IconManager.GetIcon("Question", Height / 2).Color(design.LabelColor);
				color = design.ActiveColor;
				break;

			case PromptIcons.Warning:
				icon = IconManager.GetIcon("Warning", Height / 2).Color(design.YellowColor);
				color = design.YellowColor;
				break;

			case PromptIcons.Error:
				icon = IconManager.GetIcon("Error", Height / 2).Color(design.RedColor);
				color = design.RedColor;
				break;

			case PromptIcons.Ok:
				icon = IconManager.GetIcon("Ok", Height / 2).Color(design.GreenColor);
				color = design.GreenColor;
				break;

			case PromptIcons.Loading:
				icon = null;
				color = design.ActiveColor;
				break;

			default:
				icon = null;
				color = design.ActiveColor;
				break;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		Invalidate();

		if (Notification.Action == null)
		{
			var iconSize = Padding.Horizontal + IconManager.GetNormalScale();

			Cursor = closeRect.Contains(PointToClient(MousePosition))
				? Cursors.Hand
				: Cursors.Default;
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);

		hovered = true;
		Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);

		hovered = false;
		Invalidate();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			var iconSize = Padding.Horizontal + IconManager.GetNormalScale();

			if (!closeRect.Contains(PointToClient(MousePosition)))
			{
				Notification.Action?.Invoke();
			}

			Close();
		}

		_form?.ShowUp();
	}

	public new void Close()
	{
		if (Notifications.Sum(x => x.Value.Count) > 1 || AnimationHandler.NoAnimations)
		{
			Dispose();
		}
		else
		{
			epoch = DateTime.Now.Ticks;
			closing = true;
			loaded = false;
			percentage = 0;
			Invalidate();
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (_form is SlickForm slickForm)
		{
			_form.BeginInvoke(new Action(() =>
			{
				slickForm.Focus();
				slickForm.CurrentFormState = FormState.NormalFocused;
			}));
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		base.OnActivated(e);

		if (_form is SlickForm slickForm)
		{
			slickForm.CurrentFormState = FormState.ForcedFocused;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_form != null)
			{
				_form.LocationChanged -= Form_Move;
				_form.Resize -= Form_Move;

				Notifications[_form].Remove(this);

				foreach (var item in Notifications[_form])
				{
					item.SetLocation();
				}
			}
			else
			{
				Notifications_Screen.Remove(this);

				foreach (var item in Notifications_Screen)
				{
					item.SetLocation();
				}
			}

			base.Dispose(disposing);
		}
	}
}
