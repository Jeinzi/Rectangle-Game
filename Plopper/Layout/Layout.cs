using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

// Represents the layout of the formular with different objects
// such as labels, textboxes...
namespace Plopper.Layout
{
	public class Layout : ActiveElement
	{
		/******** Variables ********/

		// Standard values for layout of the entire application
		private static Brush _standardFill = Brushes.White;
		private static Pen _standardBorderLine = new Pen(Brushes.Black, 2);
		private static Brush _standardBackgroundBrush = Brushes.Teal;
		public static Brush standardFill { get { return (_standardFill); } }
		public static Pen standardBorderLine { get { return (_standardBorderLine); } }
		public static Brush standardBackgroundBrush { get { return (_standardBackgroundBrush); } }

		private List<Box> boxes;
		private Rectangle background;
		public Brush backgroundBrush;

		/******** Functions ********/
		
		// Constructor
		public Layout()
		{
			boxes = new List<Box>();
			background = new Rectangle(new Point(), Program.mainForm.ClientSize);
			this.backgroundBrush = standardBackgroundBrush;
		}

		public Layout(Brush backgroundBrush) : this()
		{
			this.backgroundBrush = backgroundBrush;
		}

		
		// Updates every element
		public override void Update()
		{
			base.Update();
			foreach (Box box in boxes)
			{
				box.Update();
			}
		}


		// Draws the background and every element above
		public override void Draw(Graphics g)
		{
			base.Draw(g);
			g.FillRectangle(backgroundBrush, background);
			foreach(Box box in boxes)
			{
				box.Draw(g);
			}
		}

		public override void KeyPressed(KeyEventArgs e)
		{
			base.KeyPressed(e);
			foreach (Box box in boxes)
			{
				box.KeyPressed(e);
			}
		}

		public override void CharPressed(KeyPressEventArgs e)
		{
			base.CharPressed(e);
			foreach (Box box in boxes)
			{
				box.CharPressed(e);
			}
		}


		// Adds a box to the layout
		public void AddBox(Box box)
		{
			boxes.Add(box);
		}


		// Returns a box based on a given identifier
		// Returns null, if there is no matching box
		public Box GetBox(string identifier)
		{
			if (identifier == "") return (null);
			foreach(Box box in boxes)
			{
				if (box.identifier == identifier) return (box);
			}

			return (null);
		}

		public void Clear(Graphics g, Brush brush)
		{
			g.FillRectangle(brush, background);
		}

	}

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