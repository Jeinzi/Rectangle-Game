using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Plopper.Map
{
	/// <summary>
	/// An empty map without any additional objects.
	/// </summary>
	class Map
	{
		/******** Variables ********/

		/// <summary>
		/// The position of the map. It will not automatically be drawn at this point!
		/// </summary>
		public Point position;
		/// <summary>
		/// The size of the map. 
		/// </summary>
		public Size size;
		/// <summary>
		/// The pen used to draw the outer part of the map.
		/// </summary>
		public Pen pen;
		/// <summary>
		/// The brush used to draw the inner of the map.
		/// </summary>
		public Brush brush;


		/******** Functions ********/

		/// <summary>
		/// Creates a new map with the specified size.
		/// </summary>
		/// <param name="width">The width of the map.</param>
		/// <param name="height">The height of the map.</param>
		public Map(int width, int height)
		{
			size = new Size(width, height);
			
			// Center map, if it's smaller than the screen
			position = new Point(0, 0);
			if(width < Program.mainForm.ClientSize.Width)
			{
				position.X = Program.mainForm.ClientSize.Width / 2 - size.Width / 2;
			}
			if (height < Program.mainForm.ClientSize.Height)
			{
				position.Y = Program.mainForm.ClientSize.Height / 2 - size.Height / 2;
			}

			pen = new Pen(Brushes.Black, 2);
			brush = Brushes.White;
		}


		/// <summary>
		/// Draw the map.
		/// </summary>
		/// <param name="g">The graphics object that is used for drawing.</param>
		public virtual void Draw(Graphics g)
		{
			Rectangle tempRectangle = new Rectangle(new Point(), size);
			g.FillRectangle(brush, tempRectangle);
			g.DrawRectangle(pen, tempRectangle);
		}

		/// <summary>
		/// Nothing in here at the moment.
		/// </summary>
		public virtual void Update() { }


		/// <summary>
		/// Convert display coordinates to map coordinates.
		/// </summary>
		/// <param name="displayLocation">Location in display coordinates.</param>
		/// <returns>Location in map coordinates.</returns>
		public Point ToMapCoordinates(Point displayLocation)
		{
			Point mapLocation = displayLocation;
			mapLocation.Y -= position.Y;
			mapLocation.X -= position.X;
			return (mapLocation);
		}

		/// <summary>
		/// Convert map coordinates to display coordinates.
		/// </summary>
		/// <param name="mapLocation">Location in map coordinates.</param>
		/// <returns>Location in display coordinates.</returns>
		public Point ToDisplayCoordinates(Point mapLocation)
		{
			Point displayLocation = mapLocation;
			displayLocation.X += position.X;
			displayLocation.Y += position.Y;
			return (displayLocation);
		}
	}
}
