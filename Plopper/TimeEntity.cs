using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Plopper
{
	class TimeEntity : Entity
	{
		/// <summary>
		/// Creates a new Entity that is specialized for TimeMode.
		/// </summary>
		/// <param name="rectangle">The rectangle representing the entity.</param>
		/// <param name="brush">The Brush to fill the entity.</param>
		/// <param name="map">The map the entity is moving on.</param>
		public TimeEntity(RectangleF rectangle, Brush brush, Map.Map map)
			: base(rectangle, brush, map)
		{
			SetTarget(random.Next(0, map.size.Width - (int)rectangle.Width), random.Next(0, map.size.Height - (int)rectangle.Height));
		}


		/// <summary>
		/// Randomly updates the entity's target.
		/// </summary>
		public override void Update()
		{
			base.Update();
			
			// Maybe, the entity is changing its target.
			if(random.Next(0, 250) == 42)
			{
				SetTarget(	random.Next(0, map.size.Width - (int)rectangle.Width),
							random.Next(0, map.size.Height - (int)rectangle.Height));
			}
		}
	}
}
