using Extensions;

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public class IOControl : SlickPictureBox
	{
		private bool thumbnailLoaded;

		public FileInfo FileObject { get; }
		public DirectoryInfo FolderObject { get; }
		public string Path { get; }
		public IO.IController Controller { get; }

		public bool Selected { get; internal set; }

		public IOControl(object x, IO.IController controller)
		{
			if (x is FileInfo file)
			{
				if (!file.Attributes.HasFlag(FileAttributes.Hidden) && !file.Attributes.HasFlag(FileAttributes.System))
				{
					if (file.Extension.ToLower() == ".lnk")
					{
						var path = file.FullName.GetShortcutPath();

						if (Directory.Exists(path) && File.GetAttributes(path).HasFlag(FileAttributes.Directory))
							x = new DirectoryInfo(path);
						else
							FileObject = new FileInfo(path);
					}
					else
						FileObject = file;
				}
			}

			if (x is DirectoryInfo folder)
			{
				if (folder.Parent == null || !folder.Attributes.HasFlag(FileAttributes.Hidden))
				{
					FolderObject = folder;
				}
			}

			if (FileObject == null && FolderObject == null)
			{
				Dispose();
				return;
			}

			Path = FileObject?.FullName ?? FolderObject.FullName;
			Size = UI.Scale(new Size(115, 115), UI.UIScale);
			Margin = new Padding(7);
			Cursor = Cursors.Hand;
			Font = UI.Font(7.75F);
			Controller = controller;
			Text = (FolderObject?.Name ?? FileObject.FileName()).FormatWords();
			SlickTip.SetTo(this, Text);

			MouseClick += IOControl_MouseClick;
			MouseDoubleClick += IOControl_MouseClick;
			Disposed += (s, e) => Image?.Dispose();

			if (FileObject != null)
			{
				Directory.CreateDirectory(System.IO.Path.Combine(ISave.DocsFolder, "Thumbs", "Library"));
				var filepath = System.IO.Path.Combine(ISave.DocsFolder, "Thumbs", "Library", $"{FileObject.Name}.{FileObject.Length}.jpg");

				Image = FileObject.GetFileTypeIcon() ?? Properties.Resources.Big_File;

				Controller.Factory.Run(() =>
				{
					try
					{
						if (File.Exists(filepath))
						{
							thumbnailLoaded = true;
							Image = Image.FromFile(filepath);
						}
						else using (var bitmap = FileObject.GetThumbnail(out thumbnailLoaded))
							{
								if (bitmap != null)
								{
									if (thumbnailLoaded)
									{
										thumbnailLoaded = true;
										var w = Math.Min((int)(200 * UI.UIScale), bitmap.Width);
										Image = new Bitmap(bitmap, w, w * bitmap.Height / bitmap.Width);

										if (FileObject.IsVideo())
											using (var bmp = new Bitmap(bitmap, w, w * bitmap.Height / bitmap.Width))
												bmp.Save(filepath);
									}
									else
										Image = (Bitmap)bitmap.Clone();
								}
							}
					}
					catch { thumbnailLoaded = false; }
				});
			}
			else
			{
				Image = Properties.Resources.Big_Folder;

				Controller.Factory.Run(() =>
				{
					Image = IO.GetThumbnail(FolderObject.FullName) ?? Image;
				});
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var d = FormDesign.Design;

			e.Graphics.Clear(BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			if (HoverState.HasFlag(HoverState.Hovered))
				e.Graphics.FillRoundedRectangle(SlickControl.Gradient(new Rectangle(Point.Empty, Size), HoverState.HasFlag(HoverState.Pressed) ? d.ActiveColor : d.AccentBackColor), new Rectangle(0, 0, Width, Height), 7);

			if (Selected)
			{
				e.Graphics.FillRoundedRectangle(SlickControl.Gradient(new Rectangle(Point.Empty, Size), Color.FromArgb(25, d.ActiveColor)), new Rectangle(0, 0, Width, Height), 7);
				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(125, d.ActiveColor), 2F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, new Rectangle(0, 0, Width - 2, Height - 2), 7);
			}

			var imgRect = new Rectangle(7, 7, Width - 14, Height / 2);

			if (Image != null)
			{
				try
				{
					var size = Image.Width < imgRect.Width && Image.Height < imgRect.Height ? Image.Size :
						(Image.Width == Image.Height || (double)Image.Width / Image.Height < (double)imgRect.Width / imgRect.Height)
							? new Size(imgRect.Height * Image.Width / Image.Height, imgRect.Height)
							: new Size(imgRect.Width, imgRect.Width * Image.Height / Image.Width);

					if (thumbnailLoaded)
						e.Graphics.DrawBorderedImage(Image, new Rectangle(imgRect.Center(size), size));
					else
						e.Graphics.DrawImage(Image, new Rectangle(imgRect.Center(size), size));
				}
				catch { imgRect = new Rectangle(0, 2, 0, 0); }
			}
			else
				imgRect = new Rectangle(0, 2, 0, 0);

			e.Graphics.DrawString(Text, Font, SlickControl.Gradient(new Rectangle(Point.Empty, Size), HoverState.HasFlag(HoverState.Pressed) ? d.ActiveForeColor : d.ForeColor)
				, new Rectangle(7, imgRect.Y + imgRect.Height + 5, Width - 14, (int)Font.GetHeight().ClosestMultipleTo(Height - (imgRect.Y + imgRect.Height + 7)))
				, new StringFormat
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center,
					Trimming = StringTrimming.EllipsisCharacter
				});
		}

		private void IOControl_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (e.Clicks == 1)
				{
					Parent.Controls.OfType<IOControl>().Foreach(x => { if (x.Selected) { x.Selected = false; x.Invalidate(); } });
					Selected = true;
					Invalidate();
				}
				else if (e.Clicks == 2)
				{
					if (FileObject != null)
						Controller.fileOpened(FileObject);
					else if (FolderObject != null)
						Controller.folderOpened(FolderObject);
				}

				Focus();
			}
			else if (e.Button == MouseButtons.Right)
			{
				SlickToolStrip.Show(FindForm() as SlickForm, (Controller.RightClickContext?.Invoke(this) ?? new[]
				{
					new SlickStripItem(FileObject != null ? "Select File" : "Open Folder", () =>
					{
						if (FileObject != null)
							Controller.fileOpened(FileObject);
						else if (FolderObject != null)
							Controller.folderOpened(FolderObject);
					}, FileObject != null ? Properties.Resources.Tiny_Play : Properties.Resources.Tiny_Search)
				})
				.Concat(new[]
				{
					SlickStripItem.Empty,

					new SlickStripItem("View in Explorer", () =>
					{
						if (FileObject != null)
							new Action(() => System.Diagnostics.Process.Start("explorer.exe", $"/select, \"{FileObject.FullName}\"")).RunInBackground();
						else
							new Action(() => System.Diagnostics.Process.Start(FolderObject.FullName)).RunInBackground();
					}, Properties.Resources.Tiny_Folder),

					new SlickStripItem("Delete", () =>
					{
						if (MessagePrompt.Show($"Are you sure you want to delete '{Text}'", "Confirm Action", PromptButtons.OKCancel, PromptIcons.Warning, FindForm() as SlickForm) == DialogResult.OK)
						{
							new Action(() => FileOperationAPIWrapper.MoveToRecycleBin(FileObject?.FullName ?? FolderObject.FullName)).RunInBackground();
							Dispose();
						}
					}, Properties.Resources.Tiny_Trash)
				}).ToArray());
			}
		}
	}
}