using System;
using System.Drawing;

namespace RectangleGame.Entity
{
	class RushEntity : Entity
	{
		/******** Variables ********/

		Map.RushMap rushMap;
		
		/******** Functions ********/
	
		/// <summary>
		/// Creates a new entity that is specialized for RushMode.
		/// </summary>
		/// <param name="rectangle">The rectangle representing the entity.</param>
		/// <param name="brush">The Brush to fill the entity.</param>
		/// <param name="map">The map the entity is moving on.</param>
		public RushEntity(RectangleF rectangle, Brush brush, Map.RushMap map)
			: base(rectangle, brush, map)
		{
			rushMap = ((Map.RushMap)base.map);
			ChangeTarget();
		}


		/// <summary>
		/// Sets the entity's target to the location of the base.
		/// </summary>
		public override void Update()
		{
			RectangleF oldRectangle = rectangle;
			base.Update();

			int xDistance = (int)Math.Min(	Math.Abs(rectangle.X - (rushMap.playerBase.x + rushMap.playerBase.width / 2)),
											Math.Abs(rectangle.X + rectangle.Width - (rushMap.playerBase.x + rushMap.playerBase.width / 2)));
			int yDistance = (int)Math.Min(	Math.Abs(rectangle.Y - (rushMap.playerBase.y + rushMap.playerBase.height / 2)),
											Math.Abs(rectangle.Y + rectangle.Height - (rushMap.playerBase.y + rushMap.playerBase.height / 2)));

			if (xDistance <= rushMap.playerBase.width / 2
				&& yDistance  <= rushMap.playerBase.height / 2)
			{
				rectangle = oldRectangle;
				if(random.Next(0, 10) == 0)	rushMap.playerBase.ApplyAttack(rectangle.Width);
			}

			if (random.Next(0, 43) == 42)
			{
				ChangeTarget();
			}
		}

		/// <summary>
		/// Sets the target of the entity to a new point within the base.
		/// </summary>
		private void ChangeTarget()
		{
			target.X = rushMap.playerBase.x + rushMap.playerBase.width / 2;
			target.Y = rushMap.playerBase.y + rushMap.playerBase.height / 2;
			target.X += random.Next((int)(-rushMap.playerBase.width / 2), (int)(rushMap.playerBase.width / 2 - rectangle.Width));
			target.Y += random.Next((int)(-rushMap.playerBase.height / 2), (int)(rushMap.playerBase.height / 2 - rectangle.Height));
		}
	}
}
