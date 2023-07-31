using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public partial class NotificationForm : Form
	{
		private static readonly Dictionary<Form, List<NotificationForm>> Notifications = new Dictionary<Form, List<NotificationForm>>();

		public Notification Notification { get; }
		private readonly Form Form;

		private NotificationForm(Notification notification, Form form = null, NotificationSound sound = NotificationSound.Short, int? timeoutSeconds = null)
		{
			InitializeComponent();

			Notification = notification;
			Form = form;
			Padding = UI.Scale(new Padding(4), UI.FontScale);

			if (notification.Icon == PromptIcons.Loading)
			{
				PictureBox.Loading = true;
			}

			if (notification.Action != null)
			{
				Cursor = Cursors.Hand;
			}

			Disposed += (s, e) =>
			{
				Notifications[Form ?? Empty].Remove(this);
				FormDesign.DesignChanged -= DesignChanged;

				if (form != null)
				{
					form.LocationChanged -= Form_Move;
					form.Resize -= Form_Move;
				}

				foreach (var item in Notifications[Form ?? Empty])
				{
					item.SetLocation();
				}
			};

			if (form != null)
			{
				form.LocationChanged += Form_Move;
				form.Resize += Form_Move;

				if (form is SlickForm slickForm)
				{
					slickForm.CurrentFormState = FormState.ForcedFocused;
				}
			}

			if (timeoutSeconds != null && timeoutSeconds > 0)
			{
				new System.Timers.Timer((double)timeoutSeconds * 1000) { Enabled = true, AutoReset = false }
					.Elapsed += (s, e) =>
					{
						if (!IsDisposed)
						{
							this.TryInvoke(Close);
						} (s as System.Timers.Timer).Dispose();
					};
			}

			FormDesign.DesignChanged += DesignChanged;

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

		internal static void Clear()
		{
			foreach (var item in Notifications.ConvertEnumerable(x => x.Value).ToArray())
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
			PictureBox.Invalidate();
		}

		private void Form_Move(object sender, EventArgs e)
		{
			SetLocation();
		}

		private void SetLocation()
		{
			if (Form != null)
			{
				var y = Form.Bottom - 10 - (Notifications[Form].Take((Notifications[Form].IndexOf(this) + 1)).Sum(f => (10 + f.Height)));
				var x = Form.Right - 20 - Width;

				Location = new Point(x, y);
			}
			else
			{
				var y = Screen.PrimaryScreen.WorkingArea.Height - 10 - ((10 + Height) * (Notifications[Empty].IndexOf(this) + 1));
				var x = Screen.PrimaryScreen.WorkingArea.Width - 20 - Width;

				Location = new Point(x, y);
			}
		}

		public void SetText(string text)
		{
			this.TryInvoke(() => { Notification.Description = text; PictureBox.Invalidate(); });
		}

		private static readonly Form Empty = new Form();
		private static DateTime lastSoundPlay;

		public static NotificationForm Push(Notification notification, Form form, NotificationSound sound = NotificationSound.Short, int? timeoutSeconds = null)
		{
			NotificationForm frm = null;
			form?.TryInvoke(() =>
			{
				if (form != null && (!form.Visible || form.WindowState == FormWindowState.Minimized))
				{
					form = null;
				}

				frm = new NotificationForm(notification, form, sound, timeoutSeconds) { Size = new Size(0, UI.Scale(notification.Size, UI.FontScale).Height) };
				frm.PictureBox.Size = UI.Scale(notification.Size, UI.FontScale);

				var animationHandler = new AnimationHandler(frm, frm.PictureBox.Size, AnimationOption.IgnoreHeight);
				animationHandler.OnAnimationTick += (s, e, p) => frm.SetLocation();

				if (Notifications.ContainsKey(form ?? Empty))
				{
					Notifications[form ?? Empty].Add(frm);
				}
				else
				{
					Notifications.TryAdd(form ?? Empty, new List<NotificationForm>() { frm });
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
				animationHandler.StartAnimation();
			});

			return frm;
		}

		private void NotificationForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SetUp(FormDesign.Design.AccentColor);

			GetColorIcons(out var icon, out var color);

			using (var brush = new SolidBrush(FormDesign.Design.BackColor))
			{
				e.Graphics.FillRectangle(brush, PictureBox.ClientRectangle.Pad((int)UI.FontScale));
			}

			using (var brush = new SolidBrush(color))
			{
				e.Graphics.FillRectangle(brush, 0, 0, (int)(3 * UI.FontScale), PictureBox.Height);
			}

			if (Notification.OnPaint != null)
			{
				Notification.OnPaint(PictureBox, e.Graphics);
			}
			else
			{
				using (var titleFont = UI.Font(9.75F))
				using (var font = UI.Font(8.25F))
				{
					var imgRect = Rectangle.Empty;

					if (PictureBox.Loading)
					{
						imgRect = PictureBox.ClientRectangle.Pad(Padding).Align(UI.Scale(new Size(32, 32), UI.FontScale), ContentAlignment.MiddleLeft);

						PictureBox.DrawLoader(e.Graphics, imgRect);
					}
					else if (icon != null)
					{
						imgRect = PictureBox.ClientRectangle.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleLeft);
						imgRect.X += imgRect.Width / 3;

						using (icon)
						{
							e.Graphics.DrawImage(icon, imgRect);
						}
					}

					using (var brush = new SolidBrush(FormDesign.Design.ForeColor))
					{
						e.Graphics.DrawString(
						Notification.Title,
						titleFont,
						brush,
						new Rectangle(imgRect.Right + Padding.Left + imgRect.Width / 3, Padding.Top, PictureBox.Width - Padding.Horizontal - imgRect.Right, PictureBox.Height - Padding.Vertical),
						new StringFormat() { LineAlignment = string.IsNullOrWhiteSpace(Notification.Description) ? StringAlignment.Center : StringAlignment.Near });
					}

					using (var brush = new SolidBrush(FormDesign.Design.InfoColor))
					{
						e.Graphics.DrawString(
						Notification.Description,
						font,
						brush,
						new Rectangle(imgRect.Right + Padding.Left + imgRect.Width / 3, (int)e.Graphics.Measure(Notification.Title, titleFont).Height + Padding.Vertical, PictureBox.Width - Padding.Horizontal - imgRect.Right, PictureBox.Height - Padding.Vertical),
						new StringFormat() { Trimming = StringTrimming.EllipsisCharacter });
					}
				}
			}

			using (var closeIcon = IconManager.GetIcon("I_Close"))
			{
				var hovered = PictureBox.ClientRectangle.Align(closeIcon.Size + Padding.Size, ContentAlignment.TopRight).Contains(PointToClient(MousePosition));

				e.Graphics.DrawImage(closeIcon.Color(hovered ? FormDesign.Design.RedColor : FormDesign.Design.IconColor)
					, PictureBox.ClientRectangle.Pad(Padding).Align(closeIcon.Size, ContentAlignment.TopRight));
			}
		}

		private void GetColorIcons(out Bitmap icon, out Color color)
		{
			var design = FormDesign.Design;
			switch (Notification.Icon)
			{
				case PromptIcons.Hand:
					icon = IconManager.GetIcon("I_Hand", IconManager.GetLargeScale() * 3 / 2).Color(design.LabelColor);
					color = design.ActiveColor;
					break;

				case PromptIcons.Info:
					icon = IconManager.GetIcon("I_Info", IconManager.GetLargeScale() * 3 / 2).Color(design.LabelColor);
					color = design.ActiveColor;
					break;

				case PromptIcons.Input:
					icon = IconManager.GetIcon("I_Edit", IconManager.GetLargeScale() * 3 / 2).Color(design.LabelColor);
					color = design.ActiveColor;
					break;

				case PromptIcons.Question:
					icon = IconManager.GetIcon("I_Question", IconManager.GetLargeScale() * 3 / 2).Color(design.LabelColor);
					color = design.ActiveColor;
					break;

				case PromptIcons.Warning:
					icon = IconManager.GetIcon("I_Warning", IconManager.GetLargeScale() * 3 / 2).Color(design.YellowColor);
					color = design.YellowColor;
					break;

				case PromptIcons.Error:
					icon = IconManager.GetIcon("I_Error", IconManager.GetLargeScale() * 3 / 2).Color(design.RedColor);
					color = design.RedColor;
					break;

				case PromptIcons.Ok:
					icon = IconManager.GetIcon("I_Ok", IconManager.GetLargeScale() * 3 / 2).Color(design.GreenColor);
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

		private void NotificationForm_MouseMove(object sender, MouseEventArgs e)
		{
			PictureBox.Invalidate();

			if (Notification.Action == null)
			{
				PictureBox.Cursor = new Rectangle(Width - 20, 4, 16, 16).Contains(PointToClient(MousePosition))
					? Cursors.Hand
					: Cursors.Default;
			}
		}

		private void NotificationForm_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (!new Rectangle(Width - 20, 4, 16, 16).Contains(PointToClient(MousePosition)))
				{
					if (Notification.Action != null)
					{
						Notification.Action.Invoke();
					}
					Close();
				}
				else
				{
					Close();
				}
			}

			if (Form != null)
			{
				Form.ShowUp();
			}
		}

		public new void Close()
		{
			if (Notifications.Sum(x => x.Value.Count) > 1)
			{
				Dispose();
			}
			else
			{
				var aH = new AnimationHandler(this, Size.Empty, AnimationOption.IgnoreHeight);
				aH.OnAnimationTick += (s, e, p) => SetLocation();
				aH.StartAnimation(Dispose);
			}
		}

		private void NotificationForm_Load(object sender, EventArgs e)
		{
			if (Form is SlickForm slickForm)
			{
				Form.BeginInvoke(new Action(() => { slickForm.Focus(); slickForm.CurrentFormState = FormState.NormalFocused; }));
			}
		}

		private void NotificationForm_Activated(object sender, EventArgs e)
		{
			if (Form is SlickForm slickForm)
			{
				slickForm.CurrentFormState = FormState.ForcedFocused;
			}
		}
	}
}