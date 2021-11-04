using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SlickControls
{
	public delegate void HoverStateChanged(HoverState newState);

	public delegate void SimpleAction();

	public delegate void SetColor(Color color);

	public delegate void StateChangingEventHandler(object sender, StateChangingEventArgs eventArgs);

	public delegate void FileOpenedEventHandler(object sender, FileOpenedEventArgs e);

	public class StateChangingEventArgs
	{
		public bool Cancel { get; set; }
		public FormWindowState WindowState { get; }

		public StateChangingEventArgs(FormWindowState state) => WindowState = state;
	}

	public class FileOpenedEventArgs
	{
		public FileInfo File { get; set; }

		public FileOpenedEventArgs(FileInfo file) => File = file;
	}
}