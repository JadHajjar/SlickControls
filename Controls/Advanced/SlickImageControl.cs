using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickImageControl : SlickControl
	{
		private Image image;

		public event AsyncCompletedEventHandler LoadCompleted;
		public event EventHandler ImageChanged;

		[Category("Appearance"), DefaultValue(null)]
		public virtual Image Image { get => image; set { image = value; Loading = false; OnImageChanged(EventArgs.Empty); } }

		protected Point CursorLocation { get; set; }

		public void OnImageLoaded(AsyncCompletedEventArgs e = null)
			=> this.TryInvoke(() => OnLoadCompleted(e ?? new AsyncCompletedEventArgs(null, false, null)));

		protected virtual void OnLoadCompleted(AsyncCompletedEventArgs e)
		{
			Loading = false;
			LoadCompleted?.Invoke(this, e);
		}

		protected virtual void OnImageChanged(EventArgs e)
		{
			ImageChanged?.Invoke(this, e);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
				image?.Dispose();
		}

		public void LoadImage(string url) => new Action(() =>
		{
			if (!ConnectionHandler.WhenConnected(() =>
			{
				var firstTry = true;
			tryAgain: try
				{
					using (var webClient = new WebClient())
					{
						var imageData = webClient.DownloadData(url);

						using (var ms = new MemoryStream(imageData))
						{
							Image = Image.FromStream(ms);
							OnImageLoaded();
						}
					}
				}
				catch (Exception ex)
				{
					if (firstTry)
					{
						firstTry = false;
						goto tryAgain;
					}
					else fail(ex);
				}
			}))
				fail(new Exception());
		}).RunInBackground();

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			CursorLocation = PointToClient(Cursor.Position);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Apps)
			{
				OnMouseClick(new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void fail(Exception ex)
		{
			Loading = false;
			OnImageLoaded(new AsyncCompletedEventArgs(ex, true, null));
		}
	}
}