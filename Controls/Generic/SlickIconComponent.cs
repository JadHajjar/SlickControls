using Extensions;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlickControls
{
	[DefaultEvent("Click")]
	public partial class SlickIconComponent : Component
	{
		#region Public Events

		[Category("Action")]
		public event MouseEventHandler MouseHoverChanged;

		[Category("Action")]
		public event MouseEventHandler Click;

		#endregion Public Events

		#region Private Fields

		private Rectangle _bounds = new Rectangle(0, 0, 16, 16);
		private Image _icon;
		private Control _parent;
		private bool _visible = true;

		#endregion Private Fields

		#region Public Properties

		[Category("Layout")]
		public Rectangle Bounds { get => _bounds; set { _bounds = value; Parent?.Invalidate(value); } }

		[Category("Appearance"), DefaultValue(ColorStyle.Icon)]
		public ColorStyle ColorStyle { get; set; } = ColorStyle.Icon;

		[Category("Appearance"), DefaultValue(ColorStyle.Active)]
		public ColorStyle HoverStyle { get; set; } = ColorStyle.Active;

		[Category("Appearance")]
		public Image Icon { get => _icon; set { _icon = value; Parent?.Invalidate(Bounds); } }

		[Category("Layout")]
		public Point Location { get => Bounds.Location; set => Bounds = new Rectangle(value, Size); }

		[Category("Behavior"), Browsable(false)]
		public bool MouseHovered { get; private set; }

		[Category("Behavior")]
		public Control Parent
		{
			get => _parent;
			set
			{
				if (_parent != null && !DesignMode)
				{
					_parent.Paint -= _parent_Paint;
					_parent.MouseMove -= _parent_MouseMove;
					_parent.MouseClick -= _parent_MouseClick;
				}

				_parent = value;

				if (value != null && !DesignMode)
				{
					_parent.Paint += _parent_Paint;
					_parent.MouseMove += _parent_MouseMove;
					_parent.MouseClick += _parent_MouseClick;
				}
			}
		}

		[DefaultValue(AnchorStyles.Top | AnchorStyles.Left)]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public AnchorStyles Anchor { get; set; }

		private void _parent_MouseClick(object sender, MouseEventArgs e)
		{
			if (Visible && VisibleBounds.Contains(e.Location))
				Click?.Invoke(this, e);
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool Enabled { get; set; } = true;

		[Category("Layout")]
		public Size Size { get => Bounds.Size; set => Bounds = new Rectangle(Location, value); }

		[Category("Behavior"), DefaultValue(true)]
		public bool Visible { get => _visible; set { _visible = value; if (!value) MouseHovered = false; } }

		#endregion Public Properties

		#region Public Constructors

		public SlickIconComponent() => InitializeComponent();

		public SlickIconComponent(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		#endregion Public Constructors

		#region Private Methods

		private void _parent_MouseMove(object sender, MouseEventArgs e)
		{
			if (Visible)
			{
				MouseHovered = VisibleBounds.Contains(e.Location);
				MouseHoverChanged?.Invoke(this, e);
				Parent?.Invalidate(VisibleBounds);
			}
		}

		public Rectangle VisibleBounds
		{
			get
			{
				if (Anchor == (AnchorStyles.Right | AnchorStyles.Top))
					return new Rectangle(new Point(Parent.Width - Location.X - Size.Width, Location.Y), Size);
				else if (Anchor == (AnchorStyles.Right | AnchorStyles.Bottom))
					return new Rectangle(new Point(Parent.Width - Location.X - Size.Width, Parent.Height - Location.Y - Size.Height), Size);
				else if (Anchor == (AnchorStyles.Left | AnchorStyles.Bottom))
					return new Rectangle(new Point(Location.X, Parent.Height - Location.Y - Size.Height), Size);

				return Bounds;
			}
		}

		private void _parent_Paint(object sender, PaintEventArgs e)
		{
			Draw(e);
		}

		public void Draw(PaintEventArgs e)
		{
			if (Visible)
				e.Graphics.DrawImage(new Bitmap(Icon).Color(((MouseHovered && Enabled) ? HoverStyle : ColorStyle).GetColor()), VisibleBounds);
		}

		#endregion Private Methods
	}
}