using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

// Represents the layout of the formular with different objects
// such as labels, textboxes...
namespace Layout
{
	public class Layout : ActiveElement
	{
		/******** Variables ********/
		
		// Standard values for layout of the entire application
		private static Brush _defaultFill = Brushes.White;
		private static Pen _defaultBorderLine = new Pen(Brushes.Black, 2);
		private static Brush _defaultBackgroundBrush = Brushes.Teal;
		public static Brush defaultFill { get { return (_defaultFill); } }
		public static Pen defaultBorderLine { get { return (_defaultBorderLine); } }
		public static Brush defaultBackgroundBrush { get { return (_defaultBackgroundBrush); } }

		private List<Box> boxes;
		private Rectangle background;
		public Brush backgroundBrush;

		/******** Functions ********/
		
		public Layout()
		{
			boxes = new List<Box>();
#warning Not independent of program
			background = new Rectangle(new Point(), RectangleGame.Program.mainForm.ClientSize);
			this.backgroundBrush = defaultBackgroundBrush;
		}

		public Layout(Brush backgroundBrush) : this()
		{
			this.backgroundBrush = backgroundBrush;
		}

		
		/// <summary>
		/// Updates every element within the layout.
		/// </summary>
		public override void Update()
		{
			base.Update();
			foreach (Box box in boxes)
			{
				box.Update();
			}
		}


		/// <summary>
		/// Draws the background and every element within the layout.
		/// </summary>
		/// <param name="g"></param>
		public override void Draw(Graphics g)
		{
			base.Draw(g);
			g.FillRectangle(backgroundBrush, background);
			foreach(Box box in boxes)
			{
				box.Draw(g);
			}
		}

		/// <summary>
		/// Called if a key has been pressed.
		/// Passes this event to all elements within the layout.
		/// </summary>
		/// <param name="e">An object containing information about the event.</param>
		public override void KeyPressed(KeyEventArgs e)
		{
			base.KeyPressed(e);
			foreach (Box box in boxes)
			{
				box.KeyPressed(e);
			}
		}

		/// <summary>
		/// Called, if a character on the keyboard has been pressed.
		/// Passes this event to all elements within the layout.
		/// </summary>
		/// <param name="e">An object containing information about the event.</param>
		public override void CharPressed(KeyPressEventArgs e)
		{
			base.CharPressed(e);
			foreach (Box box in boxes)
			{
				box.CharPressed(e);
			}
		}


		/// <summary>
		/// Adds a box to the layout.
		/// </summary>
		/// <param name="box">The box to add.</param>
		public void AddBox(Box box)
		{
			boxes.Add(box);
		}


		/// <summary>
		/// Returns a box based on a given identifier.
		/// </summary>
		/// <param name="identifier"></param>
		/// <returns>Null, if there is no matching box.</returns>
		public Box GetBox(string identifier)
		{
			if (identifier == "") return (null);
			foreach(Box box in boxes)
			{
				if (box.identifier == identifier) return (box);
			}

			return (null);
		}

		/// <summary>
		/// Clears the entire background.
		/// </summary>
		/// <param name="g">The graphics object used for rendering.</param>
		/// <param name="brush">A color to fill the background with.</param>
		public void Clear(Graphics g, Brush brush)
		{
			g.FillRectangle(brush, background);
		}

	}

	/// <summary>
	/// Contains predefined relative positions.
	/// </summary>
	[Flags]
	public enum Anchor
	{
		None = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8,
		CenterX = 3,
		CenterY = 12,
		Center = 15
	}
}