using System.Drawing;

namespace Layout
{
	/// <summary>
	/// A simple box with a specified fill and border.
	/// A parent element can be assigned and it can be positioned and sized in relation to it.
	/// </summary>
	public class Box : ActiveElement
	{
		/******** Variables ********/
		private SizeF _relativeSize;
		private Rectangle _area;
		private Anchor _anchor;
		private Box _parent;

		/// <summary>
		/// An identifier to later retrieve the element when assigned to a Layout.
		/// </summary>
		public string identifier { get; private set; }

		/// <summary>
		/// The brush used to fill the box.
		/// </summary>
		public Brush fill;
		
		/// <summary>
		/// The pen to draw the boundary of the box.
		/// </summary>
		public Pen borderLine;

		/// <summary>
		/// Indicating if the draw method should be skipped.
		/// </summary>
		public bool visible;



		
		/******** Functions ********/

		/// <summary>
		/// Full constructor.
		/// If no identifier is used, you may not retrieve the element after assigning it to a layout.
		/// </summary>
		/// <param name="area">The rectangle corresponding to the box.</param>
		/// <param name="fill">The brush used to fill the box.</param>
		/// <param name="borderLine">The pen to draw the boundary of the box.</param>
		/// <param name="identifier">An optional identifier to later retrieve the element.</param>
		public Box(Rectangle area, Brush fill, Pen borderLine, string identifier = "")
		{
			this.parent = null;
			this._area = area;
			this.fill = fill;
			this.borderLine = borderLine;
			this.identifier = identifier;
			this.visible = true;
		}

		/// <summary>
		/// Extended constructor. Will use the standard fill and borderLine set in the Layout class.
		/// If no identifier is used, you may not retrieve the element after assigning it to a layout.
		/// </summary>
		/// <param name="area">The rectangle corresponding to the box.</param>
		/// <param name="identifier">An optional identifier to later retrieve the element.</param>
		public Box(Rectangle area, string identifier = "")
			: this(area, (Brush)Layout.defaultFill.Clone(), (Pen)Layout.defaultBorderLine.Clone(), identifier) { }

		/// <summary>
		/// Standard constructor. Will use an empty rectangle and the standard fill and borderLine set in the Layout class.
		/// If no identifier is used, you may not retrieve the element after assigning it to a layout.
		/// </summary>
		/// <param name="identifier">An optional identifier to later retrieve the element.</param>
		public Box(string identifier = "")
			: this(new Rectangle(), identifier) { }


		/// <summary>
		/// Adapt position and size to the changing parent element.
		/// </summary>
		public override void Update()
		{
			base.Update();
		}


		/// <summary>
		/// Draws the element with the given Graphics object.
		/// </summary>
		/// <param name="g">The graphics object to be rendered to.</param>
		public override void Draw(Graphics g)
		{
			if (visible)
			{
				g.FillRectangle(fill, area);
				g.DrawRectangle(borderLine, area);
			}
		}




		/******** Getter & Setter ********/

		/// <summary>
		/// A parent element. You may position and size the element in relation to any parent box.
		/// Default is null (no parent, respectively the entire formular).
		/// </summary>
		public Box parent
		{
			get
			{
				if(_parent == null)
				{
					Box tempBox =  new Box("form");
#warning Link to Program, but should be independent of software the library is used in
					tempBox.size = RectangleGame.Program.mainForm.ClientSize;
					return (tempBox);
				}
				else
				{
					return (_parent);
				}
			}
			set
			{
				_parent = value;

				if (_parent != null)
				{
					_area.X = _parent.absoluteX;
					_area.Y = _parent.absoluteY;

					// Adapt relative positioning to new parent.
					if (anchor != Anchor.None)
					{
						anchor = anchor;
					}
					else
					{
						percentageLocation = percentageLocation;
					}
				}
				else
				{
					_area.X = 0;
					_area.Y = 0;
				}
			}
		}

		// Setting position and size of the rectangle representing the box
		// Just a wrapper around the getters and setter for x, y, width and height
		public Rectangle area
		{
			get { return (_area); }
			set
			{
				x = value.X;
				y = value.Y;
				width = value.Width;
				height = value.Height;
			}
		}


		/// <summary>
		/// The x position in relation to the parent, in pixels.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public int x
		{
			get { return (_area.X - parent.absoluteX); }
			set
			{
				_area.X = value + parent.absoluteX;
				// Remove horizontal part of the anchor.
				anchor ^= (anchor & Anchor.CenterX);
			}
		}

		/// <summary>
		/// The y position in relation to the set parent, in pixels.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public int y
		{
			get { return (_area.Y - parent.absoluteY); }
			set
			{
				_area.Y = value + parent.absoluteY;
				// Remove vertical part of the anchor.
				anchor ^= (anchor & Anchor.CenterY);
			}
		}

		/// <summary>
		/// The location in relation to the set parent, in pixels.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public Point location
		{
			get
			{
				Point tempPoint = new Point(x, y);
				return (tempPoint);
			}
			set
			{
				x = value.X;
				y = value.Y;
			}
		}

		/// <summary>
		/// The x position in relation to the set parent, in percent of the parent's width.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public float percentageX
		{
			get
			{
				float tempPercentageX;
				tempPercentageX = (float)x / (float)parent.width * 100;
				return (tempPercentageX);
			}
			set
			{
				int tempX;
				tempX = (int)(value * parent.width / 100);
				x = tempX;
				// Remove horizontal part of the anchor.
				anchor ^= (anchor & Anchor.CenterX);
			}
		}

		/// <summary>
		/// The y position in relation to the set parent, in percent of the parent's height.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public float percentageY
		{
			get
			{
				float tempPercentageY;
				tempPercentageY = (float)y / (float)parent.height * 100;
				return (tempPercentageY);
			}
			set
			{
				int tempY;
				tempY = (int)(value * parent.height / 100);
				y = tempY;
				// Remove vertical part of the anchor.
				anchor ^= (anchor & Anchor.CenterY);
			}
		}

		/// <summary>
		/// The position in relation to the set parent, in percent of the parent's size.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public PointF percentageLocation
		{
			get
			{
				PointF tempPercentageLocation = new PointF(percentageX, percentageY);
				return (tempPercentageLocation);
			}
			set
			{
				percentageX = value.X;
				percentageY = value.Y;
			}
		}

		/// <summary>
		/// The absolute x position in relation to the formular, in pixels.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public int absoluteX
		{
			get { return (_area.X); }
			set
			{
				_area.X = value;
				// Remove horizontal part of the anchor.
				anchor ^= (anchor & Anchor.CenterX);
			}
		}

		/// <summary>
		/// The absolute x position in relation to the formular, in pixels.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public int absoluteY
		{
			get { return (_area.Y); }
			set
			{
				_area.Y = value;
				// Remove vertical part of the anchor.
				anchor ^= (anchor & Anchor.CenterY);
			}
		}

		/// <summary>
		/// The absolute location in relation to the formular, in pixels.
		/// Setting the location this way will remove any anchor that was previously set.
		/// </summary>
		public Point absoluteLocation
		{
			get
			{
				Point tempPoint = new Point(absoluteX, absoluteY);
				return (tempPoint);
			}
			set
			{
				x = value.X;
				y = value.Y;
			}
		}


		/// <summary>
		/// The width of the element, in pixels.
		/// </summary>
		public int width
		{
			get { return (_area.Width); }
			set
			{
				if (value > 0) _area.Width = value;
				else _area.Width = 0;
			}
		}

		/// <summary>
		/// The height of the element, in pixels.
		/// </summary>
		public int height
		{
			get { return (_area.Height); }
			set
			{
				if (value > 0) _area.Height = value;
				else _area.Height = 0;
			}
		}

		/// <summary>
		/// The size of the element. Implicitly uses the width and height properties.
		/// </summary>
		public Size size
		{
			get
			{
				return (new Size(width, height));
			}
			set
			{
				width = value.Width;
				height = value.Height;
			}
		}

		/// <summary>
		/// The percentage width of the of the element in relation to its parent.
		/// -1 indicating that the value won't be used.
		/// </summary>
		public float percentageWidth
		{
			get { return (_relativeSize.Width); }
			set
			{
				if (value < 0)
				{
					_relativeSize.Width = -1;
				}
				else
				{
					// Setting width
					_area.Width = (int)(value / 100 * parent.width);
					_relativeSize.Width = value;
				}
			}
		}

		/// <summary>
		/// The percentage height of the of the element in relation to its parent.
		/// -1 indicating that the value won't be used.
		/// </summary>
		public float percentageHeight
		{
			get { return (_relativeSize.Height); }
			set
			{
				if (value < 0)
				{
					_relativeSize.Height = -1;
				}
				else
				{
					// Setting height
					_area.Height = (int)(value / 100 * parent.height);
					_relativeSize.Height = value;
				}
			}
		}

		/// <summary>
		/// The percentage size of the of the element in relation to its parent.
		/// Implicitly uses the relativeWidth and relativeHeight properties.
		/// -1 for either width or height indicating that the value won't be used.
		/// </summary>
		public SizeF percentageSize
		{
			get { return (_relativeSize); }
			set
			{
				percentageWidth = value.Width;
				percentageHeight = value.Height;
			}
		}

		/// <summary>
		/// You may center the element or set its position to the top, left, right or bottom using an anchor.
		/// You may combine them using the | operator.
		/// Combining the top, left, right and bottom anchor will result in centering the element.
		/// </summary>
		public Anchor anchor
		{
			get { return(_anchor); }
			set
			{
				// Aligned right
				if ((value & Anchor.Right) == Anchor.Right)
				{
					_area.X = parent.x + parent.width - width;
				}
				// Aligned Down
				if ((value & Anchor.Bottom) == Anchor.Bottom)
				{
					_area.Y = parent.y + parent.height - height;
				}
				// Aligned left
				if ((value & Anchor.Left) == Anchor.Left)
				{
					_area.X = parent.x;
				}
				// Aligned up
				if ((value & Anchor.Top) == Anchor.Top)
				{
					_area.Y = parent.y;
				}
				// Box centered on x axis
				if ((value & Anchor.CenterX) == Anchor.CenterX)
				{
					_area.X = parent.x + (parent.width / 2) - (width / 2);
				}
				// Box centered on y axis
				if ((value & Anchor.CenterY) == Anchor.CenterY)
				{
					_area.Y = parent.y + (parent.height / 2) - (height / 2);
				}

				_anchor = value;
			}
		}
	}
}