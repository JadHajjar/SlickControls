using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Documents;

using Img = SlickControls.Properties.Resources;

namespace SlickControls
{
	public static class IO
	{
		public static Bitmap GetThumbnail(this FileInfo file, float? timeInSeconds = null)
			=> GetThumbnail(file, out _, timeInSeconds);

		public static Bitmap GetThumbnail(this FileInfo file, out bool isThumbnail, float? timeInSeconds = null)
		{
			try
			{
				//if (VideoExtensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				//	using (var thumbJpegStream = new MemoryStream())
				//	{
				//		try
				//		{
				//			new FFMpegConverter().GetVideoThumbnail(
				//				file.FullName,
				//				thumbJpegStream,
				//				timeInSeconds ?? (float)(new FFProbe().GetMediaInfo(file.FullName).Duration.TotalSeconds / 4));

				//			if (thumbJpegStream.Length != 0)
				//			{
				//				isThumbnail = true;
				//				return new Bitmap(thumbJpegStream);
				//			}
				//		}
				//		catch { }

				//		isThumbnail = false;
				//		return null;
				//	}

				if (ImageExtensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				{
					try
					{
						isThumbnail = true;
						return (Bitmap)Image.FromFile(file.FullName);
					}
					catch { }

					isThumbnail = false;
					return null;
				}

				var icon = GetFileTypeIcon(file);
				isThumbnail = false;

				if (icon != null)
					return icon;

				return GetThumbnail(file.FullName);
			}
			catch { }

			isThumbnail = false;
			return null;
		}

		public static Bitmap GetFileTypeIcon(this FileInfo file)
		{
			if (string.IsNullOrWhiteSpace(file.Extension))
				return Img.Big_File;

			if (new[] { ".log" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Log;

			if (new[] { ".crp" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_CRP;

			if (new[] { ".htm", ".html" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_HTML;

			if (new[] { ".xml" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_XML;

			if (new[] { ".json" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_JSON;

			if (new[] { ".pdf" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Pdf;

			if (new[] { ".txt" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Txt;

			if (new[] { ".zip", ".7z", ".rar" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Zip;

			if (new[] { ".doc", ".docx" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Doc;

			if (new[] { ".xls", ".xlsx", ".csv" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Xls;

			if (new[] { ".ppt", ".pptx" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_PPT;

			if (new[] { ".dll" }.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Dll;

			if (VideoExtensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Vid;

			if (ImageExtensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Img;

			if (MusicExtensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase)))
				return Img.File_Music;

			return null;
		}

		public static Bitmap GetThumbnail(string path)
		{
			try
			{
				var shinfo = new SHFILEINFO();

				SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICONLOCATION | SHGFI_ICON | SHGFI_LARGEICON);

				if (File.Exists(shinfo.szDisplayName) && shinfo.szDisplayName.EndsWith(".ico", StringComparison.InvariantCultureIgnoreCase))
					return new Icon(shinfo.szDisplayName, 128, 128)?.ToBitmap();
				else if (!Directory.Exists(path))
					return Icon.FromHandle(shinfo.hIcon)?.ToBitmap();

				DestroyIcon(shinfo.hIcon);
			}
			catch { }

			return null;
		}

		public static bool IsVideo(this FileInfo file)
			=> VideoExtensions.Any(y => string.Equals(y, file.Extension, StringComparison.CurrentCultureIgnoreCase));

		public static readonly string[] VideoExtensions =
		{
			".mkv", ".webm", ".flv", "vob", ".ogv",
			".ts", ".drc", ".mng", ".avi", ".mov",
			".wmv", ".amv", ".mp4", ".m4v", ".amv",
			".mpg", ".mpeg", ".3gp", ".f4v"
		};

		public static readonly string[] ImageExtensions =
		{
			".ANI", ".ANIM", ".APNG", ".ART", ".BMP", ".BPG", ".BSAVE", ".CAL", ".CIN", ".CPC", ".CPT",
			".DDS", ".DPX", ".ECW", ".EXR", ".FITS", ".FLIC", ".FLIF", ".FPX", ".GIF", ".HDRi", ".HEVC",
			".ICER", ".ICNS", ".ICO", ".ICS", ".ILBM", ".JBIG", ".JBIG2", ".JNG", ".JPEG", ".JPG", ".KRA",
			".MNG", ".MIFF", ".NRRD", ".PAM", ".PBM", ".PCX", ".PGF", ".PICtor", ".PNG", ".PSP", ".QTVR",
			".RAS", ".RGBE", ".SGI", ".TGA", ".TIFF", ".WBMP", ".WebP", ".XBM", ".XCF", ".XPM", ".XWD"
		};

		public static readonly string[] MusicExtensions =
		{
			".3gp", ".aa", ".aac", ".aax", ".act",
			".m4a", ".m4b", ".m4p", ".mp3", ".mpc",
			".opus", ".tta", ".wav", ".wma"
		};

		public interface IController
		{
			void folderOpened(DirectoryInfo directory);

			void fileOpened(FileInfo file);

			List<ExtensionClass.action> Factory { get; }

			Func<IOControl, SlickStripItem[]> RightClickContext { get; }
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		public const uint SHGFI_ICONLOCATION = 0x1000;
		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0; // 'Large icon

		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool DestroyIcon(IntPtr handle);
	}
}