namespace SlickControls
{
	partial class SlickButton
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden), System.ComponentModel.Browsable(false)]
		public new System.Windows.Forms.AutoScaleMode AutoScaleMode { get;  set; }
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden), System.ComponentModel.Browsable(false)]
		public new System.Windows.Forms.AutoSizeMode AutoSizeMode { get;  set; }


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Size = new System.Drawing.Size(100, 30);
			this.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.SpaceTriggersClick = true;
		}

		#endregion
	}
}
