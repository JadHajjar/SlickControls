using Extensions;

using SlickControls.Controls.Other;

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SlickControls
{
	public class IOControl
	{
		private bool thumbnailLoaded;

		public FileInfo FileObject { get; }
		public DirectoryInfo FolderObject { get; }
		public string Path { get; }
		public IO.IController Controller { get; }
		public string Name { get; }
		public string Text { get; internal set; }
		public Image Icon { get; internal set; }

		public IOControl(object x, IO.IController controller, out bool valid, Bitmap icon = null)
		{
			valid = false;

			if (x is FileInfo file)
			{
				if ((file.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0)
				{
					if (file.Extension.ToLower() == ".lnk")
					{
						var path = file.FullName.GetShortcutPath();

						if (Directory.Exists(path) && File.GetAttributes(path).HasFlag(FileAttributes.Directory))
						{
							x = new DirectoryInfo(path);
						}
						else
						{
							FileObject = new FileInfo(path);
						}
					}
					else
					{
						FileObject = file;
					}
				}
			}

			else if (x is DirectoryInfo folder)
			{
				if (folder.Parent == null || !folder.Attributes.HasFlag(FileAttributes.Hidden))
				{
					FolderObject = folder;
				}
			}

			if (FileObject == null && FolderObject == null)
			{
				return;
			}

			Path = FileObject?.FullName ?? FolderObject.FullName;
			Controller = controller;
			Name = FolderObject?.Name ?? FileObject.Name;
			Text = FolderObject?.Name ?? FileObject.FileName();

			if (FileObject != null)
			{
				Icon = icon ?? FileObject.GetFileTypeIcon() ?? Properties.Resources.Big_File;

				if (icon == null)
				{
					Controller.Factory.Add(() =>
					{
						Directory.CreateDirectory(System.IO.Path.Combine(ISave.DocsFolder, "Thumbs", "Library"));
						var filepath = System.IO.Path.Combine(ISave.DocsFolder, "Thumbs", "Library", $"{FileObject.Name}.{FileObject.Length}.jpg");

						try
						{
							if (File.Exists(filepath))
							{
								thumbnailLoaded = true;
								Icon = Image.FromFile(filepath);
							}
							else
							{
								using (var bitmap = FileObject.GetThumbnail(out thumbnailLoaded))
								{
									if (bitmap != null)
									{
										if (thumbnailLoaded)
										{
											thumbnailLoaded = true;
											var w = Math.Min((int)(200 * UI.UIScale), bitmap.Width);
											Icon = new Bitmap(bitmap, w, w * bitmap.Height / bitmap.Width);

											if (FileObject.IsVideo())
											{
												using (var bmp = new Bitmap(bitmap, w, w * bitmap.Height / bitmap.Width))
												{
													bmp.Save(filepath);
												}
											}
										}
										else
										{
											Icon = (Bitmap)bitmap.Clone();
										}
									}
								}
							}
						}
						catch { thumbnailLoaded = false; }
					});
				}
			}
			else
			{
				Icon = Properties.Resources.Big_Folder;

				Controller.Factory.Add(() =>
				{
					Icon = IO.GetThumbnail(FolderObject.FullName) ?? Icon;
				});
			}

			valid = true;
		}

		internal void OnPaintGrid(ItemPaintEventArgs<IOControl, IoListControl.Rectangles> e, bool selected)
		{
			var d = FormDesign.Design;

			if (e.HoverState.HasFlag(HoverState.Hovered))
			{
				e.Graphics.FillRoundedRectangle(SlickControl.Gradient(e.ClipRectangle, e.HoverState.HasFlag(HoverState.Pressed) ? d.ActiveColor : Color.FromArgb(125, d.AccentColor)), e.ClipRectangle.Pad(1), (int)(6 * UI.FontScale));
			}

			if (selected)
			{
				e.Graphics.FillRoundedRectangle(SlickControl.Gradient(e.ClipRectangle, Color.FromArgb(25, d.ActiveColor)), e.ClipRectangle.Pad(1), (int)(6 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(125, d.ActiveColor), 2F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, e.ClipRectangle.Pad(1), (int)(6 * UI.FontScale));
			}

			var imgRect = e.ClipRectangle.Pad((int)(6 * UI.FontScale));
			var textRect = e.ClipRectangle.Pad((int)(6 * UI.FontScale));

			imgRect.Height = imgRect.Height * 4 / 10;

			if (Icon != null)
			{
				try
				{
					var size = Icon.Size.GetProportionalDownscaledSize(imgRect.Height);

					if (thumbnailLoaded)
					{
						e.Graphics.DrawBorderedImage(Icon, imgRect.CenterR(size));
					}
					else
					{
						e.Graphics.DrawImage(Icon, imgRect.CenterR(size));
					}

					textRect.Y += imgRect.Height;
					textRect.Height -= imgRect.Height;
				}
				catch { }
			}

			using (var font = UI.Font(7.5F))
			{
				using (var brush = new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? d.ActiveForeColor : d.ForeColor))
				{
					e.Graphics.DrawString(Text, font, brush, textRect.AlignToFontSize(font, graphics: e.Graphics)
					, new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center,
						Trimming = StringTrimming.EllipsisCharacter
					});
				}
			}
		}

		internal void OnPaintList(ItemPaintEventArgs<IOControl, IoListControl.Rectangles> e, bool selected)
		{
			var d = FormDesign.Design;

			if (e.HoverState.HasFlag(HoverState.Hovered))
			{
				e.Graphics.FillRoundedRectangle(SlickControl.Gradient(e.ClipRectangle, e.HoverState.HasFlag(HoverState.Pressed) ? d.ActiveColor : Color.FromArgb(125, d.AccentColor)), e.ClipRectangle.Pad(1), (int)(3 * UI.FontScale));
			}

			if (selected)
			{
				e.Graphics.FillRoundedRectangle(SlickControl.Gradient(e.ClipRectangle, Color.FromArgb(25, d.ActiveColor)), e.ClipRectangle.Pad(1), (int)(6 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(new Pen(Color.FromArgb(125, d.ActiveColor), 2F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, e.ClipRectangle.Pad(1), (int)(3 * UI.FontScale));
			}

			var imgRect = e.ClipRectangle.Pad((int)(3 * UI.FontScale));
			var textRect = e.ClipRectangle.Pad((int)(3 * UI.FontScale));

			imgRect.Width = imgRect.Height;

			if (Icon != null)
			{
				try
				{
					var size = Icon.Size.GetProportionalDownscaledSize(imgRect.Height);

					if (thumbnailLoaded)
					{
						e.Graphics.DrawBorderedImage(Icon, imgRect.CenterR(size));
					}
					else
					{
						e.Graphics.DrawImage(Icon, imgRect.CenterR(size));
					}

					textRect = textRect.Pad(imgRect.Right, 0, 0, 0);
				}
				catch { }
			}

			using (var font = UI.Font(8.25F))
			{
				using (var brush = new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? d.ActiveForeColor : d.ForeColor))
				{
					textRect = textRect.AlignToFontSize(font, graphics: e.Graphics);
					var rect1 = textRect.Pad(0, 0, textRect.Width / 2, 0).Pad(0,0, (int)(8 * UI.FontScale),0);
					var rect2 = textRect.Pad(textRect.Width / 2, 0, textRect.Width / 4, 0).Pad(0, 0, (int)(8 * UI.FontScale), 0);
					var rect3 = textRect.Pad(textRect.Width * 3 / 4, 0, 0, 0);

					e.Graphics.DrawString(Text, font, brush, rect1
					, new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Trimming = StringTrimming.EllipsisCharacter
					});

					if (e.Item.FileObject != null)
					{
						e.Graphics.DrawString(e.Item.FileObject.LastWriteTime.ToString("g"), font, brush, rect2
						, new StringFormat
						{
							LineAlignment = StringAlignment.Center,
							Trimming = StringTrimming.EllipsisCharacter
						});

						e.Graphics.DrawString(e.Item.FileObject.Length.SizeString(), font, brush, rect3
						, new StringFormat
						{
							LineAlignment = StringAlignment.Center,
							Trimming = StringTrimming.EllipsisCharacter
						});
					}
				}
			}
		}
	}
}