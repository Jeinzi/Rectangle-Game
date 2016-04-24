using System;
using System.Drawing;
using System.Windows;

namespace RectangleGame.Entity
{
	class Entity
	{
		/******** Variables ********/

		public RectangleF rectangle;
		public Brush brush;
		public Pen pen;
		protected Map.Map map;
		protected System.Drawing.Point target;
		protected int velocity;
		protected Vector direction;
		protected Random random;

		/******** Functions ********/

		/// <summary>
		/// Creates a new entity.
		/// </summary>
		/// <param name="rectangle">The rectangle representing the entity.</param>
		/// <param name="brush">The Brush to fill the entity.</param>
		/// <param name="map">The map the entity is moving on.</param>
		public Entity(RectangleF rectangle, Brush brush, Map.Map map)
		{
			// Setting given parameters.
			this.rectangle = rectangle;
			this.brush = brush;
			this.map = map;

			// Randomize using ticks and properties of rectangle.
			random = new Random((int)(DateTime.Now.Ticks + (long)(rectangle.X + rectangle.Y + rectangle.Width)));

			// Setting standardized parameters.
			pen = new Pen(Color.Black, 2);

			// Setting velocity.
			velocity = 3;
		}


		/// <summary>
		/// Updates position and target of the entity.
		/// </summary>
		public virtual void Update()
		{
			// Moving entity towards target point.
			System.Windows.Point position = new System.Windows.Point(rectangle.X, rectangle.Y);
			System.Windows.Point targetPosition = new System.Windows.Point(target.X, target.Y);
			if (System.Windows.Point.Subtract(position, targetPosition).Length > velocity)
			{
				Target = target;
				System.Windows.Point newPosition = new System.Windows.Point();
				newPosition = position + direction * velocity;
				rectangle = new RectangleF((float)newPosition.X, (float)newPosition.Y, rectangle.Width, rectangle.Height);
			}
		}


		/// <summary>
		/// Draws the entity using the given graphics object.
		/// </summary>
		/// <param name="g">The graphics object to draw with.</param>
		public virtual void Draw(Graphics g)
		{
			// Converting floating point rectangle to integer rectangle
			Rectangle drawnRectangle = new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);

			g.FillRectangle(brush, drawnRectangle);
			g.DrawRectangle(pen, drawnRectangle);
		}


		/// <summary>
		/// Tests collision with an other rectangle.
		/// </summary>
		/// <param name="otherRectangle">The rectangle that will be checked.</param>
		/// <returns>A boolean indicating if the entity is colliding with the given rectangle.</returns>
		public bool CollidesWith(Rectangle otherRectangle)
		{
			// Convert RectangleF to Rectangle
			Rectangle ownRectangle = new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);

			return (ownRectangle.IntersectsWith(otherRectangle));
		}

		/******** Getter & Setter ********/

		public Brush Brush
		{
			set
			{
				brush = value;
				pen = new Pen(brush, 1);
			}
			get
			{
				return (brush);
			}
		}

		public System.Drawing.Point Target
		{
			set
			{
				target = value;
				direction = new Vector(target.X - rectangle.X, target.Y - rectangle.Y);
				direction.Normalize();
			}
			get
			{
				return (target);
			}
		}
	}
}