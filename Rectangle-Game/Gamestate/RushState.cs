using System;
using System.Drawing;
using Layout;

namespace RectangleGame.Gamestate
{
	class RushState : LevelState
	{
		/******** Variables ********/

		private double difficulty;
		private Map.RushMap rushMap;
		private Layout.Bar healthBar;
		private Layout.Textbox difficultyBox;

		/******** Functions ********/

		public RushState(GameStateManager gameStateManager)
			: base(gameStateManager)
		{
			rushMap = new Map.RushMap(map.size.Width, map.size.Height);
			difficulty = 30;

			// Health bar.
			healthBar = new Layout.Bar("healthBar");
			healthBar.maxValue = rushMap.playerBase.health;
			healthBar.percentFull = 100;
			healthBar.caption = "";
			healthBar.status = "";
			healthBar.fill = Brushes.Red;
			healthBar.percentageY = 93;
			healthBar.anchor = Layout.Anchor.CenterX;
			healthBar.percentageSize = new SizeF(20, 5);
			healthBar.padding = 1;
			healthBar.borderLine.Width = 3;

			// Difficulty counter.
			difficultyBox = new Layout.Textbox("difficultyBox");
			difficultyBox.percentageY = 2;
			difficultyBox.anchor = Layout.Anchor.CenterX;

			layout.AddBox(difficultyBox);
			layout.AddBox(healthBar);
		}


		/// <summary>
		/// Executed every time the gamestate is activated.
		/// </summary>
		public override void Init()
		{
			base.Init();

			rushMap = new Map.RushMap(map.size.Width, map.size.Height);
			healthBar.maxValue = rushMap.playerBase.health;
			healthBar.percentFull = 100;
			
			difficulty = 30;
		}


		/// <summary>
		/// Updates all the content within the gamestate that is changing over time.
		/// </summary>
		public override void Update()
		{
			base.Update();

			// Do nothing if the game already has ended.
			if (victory) return;

			// Maybe, a new entity spawns.
			int x = hits;
			difficulty = 0.5 * (10*x*x + 0.2)/(x*x + 70*x + 0.1) + Math.Cos(1.2 * Math.Sqrt(1.5*x)) + 2 * Math.Exp(-0.05*x);
			if (random.Next(0, (int)(60 / difficulty)) == 0)
			{
				Entity.Entity entity = GetRandomEntity();
				// If the random entity does not intersect with the base, it is finally created.
				if(!entity.rectangle.IntersectsWith(rushMap.playerBase.rectangle))
				{
					Entity.RushEntity rushEntity = new Entity.RushEntity(entity.rectangle, entity.brush, rushMap);
					entities.Add(rushEntity);
				}
			}

			// Base is updated.
			rushMap.playerBase.Update();

			// If an entity collides with the base, health is decremented
			// Difficulty is displayed with one decimal place.
			difficultyBox.text = difficulty.ToString("F1");

			// If health is 0, the game stops.
			score = (int)stopwatch.ElapsedMilliseconds;
			healthBar.value = rushMap.playerBase.health;
			if(healthBar.value == 0 && !victory)
			{
				stopwatch.Stop();
				victory = true;
				centerBox.visible = true;
				caption.visible = true;
				inputBox.visible = true;
				SFXPlayer.Play(Sound.Death);
			}
		}


		/// <summary>
		/// Draws all the content to the given graphics object.
		/// </summary>
		/// <param name="g">The graphics object in question.</param>
		public override void Draw(Graphics g)
		{
			base.Draw(g);

			g.TranslateTransform(map.position.X, map.position.Y);
			rushMap.playerBase.Draw(g);
			g.ResetTransform();

			layout.Draw(g);
		}

		/// <summary>
		/// Saves the score along with the entered name in the database.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void NameEntered(object sender, Layout.StringEventArgs e)
		{
			base.NameEntered(sender, e);
			string text = e.text.Trim();
			Database.Query("insert into rushState (name, score) values ('" + text + "', " + score + ")");
		}
	}
}