using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Plopper.Map
{
	/// <summary>
	/// A map containing an additional base.
	/// </summary>
	class RushMap : Map
	{
		/******** Variables ********/

		/// <summary>
		/// The base the player's got to defend.
		/// </summary>
		public Base playerBase;
		/// <summary>
		/// An internally used random object.
		/// </summary>
		private Random random;

		/******** Functions ********/
		
		/// <summary>
		/// Creates a new map with a base at a random position.
		/// </summary>
		/// <param name="width">The width of the map.</param>
		/// <param name="height">The height of the map.</param>
		public RushMap(int width, int height)
			: base(width, height)
		{
			random = new Random((int)DateTime.Now.Ticks);
			playerBase = new Base(new Rectangle(0, 0, 100, 100));
			playerBase.x = random.Next(size.Width / 10, size.Width / 10 * 9 - playerBase.width);
			playerBase.y = random.Next(size.Height / 10, size.Height / 10 * 9 - playerBase.height);
		}
	}
}