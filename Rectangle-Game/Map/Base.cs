using System.Drawing;
using Layout;

namespace RectangleGame.Map
{
	/// <summary>
	/// A rectangular basis consisting out of an inner and an outer part.
	/// </summary>
	class Base : Layout.ActiveElement
	{
		/******** Variables ********/

		/// <summary>
		/// The outer part of the base.
		/// </summary>
		private Layout.Box outerBase;
		/// <summary>
		/// The inner part of the basis.
		/// </summary>
		private Layout.Box innerBase;

		/// <summary>
		/// The layout containing the inner and outer part of the base.
		/// </summary>
		private Layout.Layout layout;
		/// <summary>
		/// The health of the base.
		/// </summary>
		public double health;
		

		/******** Getter & Setter ********/

		/// <summary>
		/// Gets and sets the x position of the base.
		/// </summary>
		public int x
		{
			get { return (outerBase.x); }
			set
			{
				outerBase.x = value;
			}
		}

		/// <summary>
		/// Gets and sets the y position of the base.
		/// </summary>
		public int y
		{
			get { return (outerBase.y); }
			set
			{
				outerBase.y = value;
			}
		}

		/// <summary>
		/// Gets and sets the width of the base.
		/// </summary>
		public int width
		{
			get { return (outerBase.width); }
			set
			{
				outerBase.width = value;
			}
		}

		/// <summary>
		/// Gets and sets the height position of the base.
		/// </summary>
		public int height
		{
			get { return (outerBase.height); }
			set
			{
				outerBase.height = value;
			}
		}

		/// <summary>
		/// Gets and sets the outer boundary of the base.
		/// </summary>
		public Rectangle rectangle
		{
			get { return (outerBase.area); }
			set
			{
				outerBase.area = value;
			}
		}

		/******** Functions ********/

		/// <summary>
		/// Creates a new base.
		/// </summary>
		/// <param name="rectangle">The rectangle representing the outer boundary of the base.</param>
		public Base(Rectangle rectangle)
		{
			health = 1000;
			layout = new Layout.Layout(Brushes.Transparent);

			// Adding outer part of base.
			outerBase = new Layout.Box("outerBase");
			outerBase.area = rectangle;
			outerBase.fill = new SolidBrush(Color.FromArgb(255, 50, 50, 50));
			outerBase.borderLine.Width = 3;
			layout.AddBox(outerBase);

			// Adding inner part of base.
			innerBase = new Layout.Box("innerBase");
			innerBase.parent = outerBase;
			innerBase.percentageSize = new Size(50, 50);
			innerBase.anchor = Layout.Anchor.Center;
			innerBase.fill = Brushes.Gray;
			outerBase.borderLine.Width = 3;
			layout.AddBox(innerBase);
		}

		/// <summary>
		/// Draws the base.
		/// </summary>
		/// <param name="g">The graphics object used to draw the base.</param>
		public override void Draw(Graphics g)
		{
			layout.Draw(g);
		}

		/// <summary>
		/// Updates the base - nothing happens here at the moment.
		/// </summary>
		public override void Update()
		{
			layout.Update();
		}

		/// <summary>
		/// Attacks the base which then calculates the caused damage.
		/// </summary>
		/// <param name="strength">The strength of the attack.</param>
		/// <returns>The caused damage.</returns>
		public double ApplyAttack(double strength)
		{
			double damage = strength;
			health -= damage;
			SFXPlayer.Play(Sound.Hit);
			return (damage);
		}
	}
}
