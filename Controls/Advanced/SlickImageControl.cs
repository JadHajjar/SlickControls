﻿using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickImageControl : SlickControl
	{
		private Image image;
		private DynamicIcon _imageName;

		public event AsyncCompletedEventHandler LoadCompleted;
		public event EventHandler ImageChanged;

		[Category("Appearance"), DefaultValue(null)]
		public virtual Image Image
		{
			get
			{
				if (Live)
				{
					if (image != null)
						lock (image)
							return new Bitmap(image);

					if (LargeImage)
						return ImageName?.Large;

					if (SmallImage)
						return ImageName?.Small;

					return ImageName;
				}

				return image == null ? null : new Bitmap(image);
			}
			set
			{
				image = value;
				Loading = false;
				OnImageChanged(EventArgs.Empty);
			}
		}

		[Category("Appearance"), DisplayName("Image Name"), DefaultValue(null), TypeConverter(typeof(IconManager.IconConverter))]
		public virtual DynamicIcon ImageName { get => _imageName; set { _imageName = value; Invalidate(); } }

		[Category("Appearance"), DisplayName("Large Image"), DefaultValue(false)]
		public bool LargeImage { get; set; }

		[Category("Appearance"), DisplayName("Small Image"), DefaultValue(false)]
		public bool SmallImage { get; set; }

		protected Point CursorLocation { get; set; }

		public void OnImageLoaded(AsyncCompletedEventArgs e = null)
		{
			this.TryInvoke(() => OnLoadCompleted(e ?? new AsyncCompletedEventArgs(null, false, null)));
		}

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
			{
				image?.Dispose();
			}
		}

		public void LoadImage(string url)
		{
			LoadImage(url, LoadImageFromUrl);
		}
#if NET47
		public void LoadImage(string url, Func<string, Task<Bitmap>> method)
		{
			if (string.IsNullOrEmpty(url) || method == null)
			{
				fail(new Exception());
				return;
			}

			Loading = true;

			new BackgroundAction("Loading Image", async () =>
			{
				try
				{
					if (IsDisposed)
					{
						return;
					}

					Image = await method(url);
					ImageChanged?.Invoke(this, EventArgs.Empty);
				}
				catch (Exception ex)
				{
					fail(ex);

					if (!ConnectionHandler.IsConnected)
					{
						if (!ConnectionHandler.WhenConnected(() => new BackgroundAction("Loading Image", async () =>
						{
							try
							{
								if (IsDisposed)
								{
									return;
								}

								Image = await method(url);
								ImageChanged?.Invoke(this, EventArgs.Empty);
							}
							catch (Exception ex2)
							{
								fail(ex2);
							}
						}).Run()))
						{
							fail(new Exception());
						}
					}
				}
			}).Run();
		}
#endif
		public void LoadImage(string url, Func<string, Image> method)
		{
			if (string.IsNullOrEmpty(url) || method == null)
			{
				fail(new Exception());
				return;
			}

			Loading = true;

			if (!ConnectionHandler.WhenConnected(() => new BackgroundAction("Loading Image", () =>
			{
				try
				{
					if (IsDisposed)
					{
						return;
					}

					Image = method(url);
					ImageChanged?.Invoke(this, EventArgs.Empty);
				}
				catch (Exception ex)
				{
					fail(ex);
				}
			}).Run()))
			{
				fail(new Exception());
			}
		}

		private Image LoadImageFromUrl(string url)
		{
			var firstTry = true;
			tryAgain:
			try
			{
				using (var webClient = new WebClient())
				{
					var imageData = webClient.DownloadData(url);

					using (var ms = new MemoryStream(imageData))
					{
						return Image.FromStream(ms);
					}
				}
			}
			catch
			{
				if (firstTry)
				{
					firstTry = false;
					goto tryAgain;
				}
				else
				{
					throw;
				}
			}
		}

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