﻿namespace RectangleGame.Gamestate
{
	class TimeState : LevelState
	{
		/******** Functions ********/

		public TimeState(GameStateManager gameStateManager)
			: base(gameStateManager) { }


		/// <summary>
		/// Executed every time the gamestate is activated.
		/// </summary>
		public override void Init()
		{
			base.Init();
			for(int i = 0; i < 25; i++)
			{
				Entity.Entity entity = GetRandomEntity();
				Entity.TimeEntity timeEntity = new Entity.TimeEntity(entity.rectangle, entity.brush, map);
				entities.Add(timeEntity);
			}

			centerBox.visible = false;
			caption.visible = false;
			inputBox.visible = false;
		}


		/// <summary>
		/// Updates all the content within the gamestate that is changing over time.
		/// </summary>
		public override void Update()
		{
			base.Update();

			// If there's no entity left, stop game and insert into database
			if (entities.Count == 0 && shots > 0 && !victory)
			{
				SFXPlayer.Play(Sound.Death);
				centerBox.visible = true;
				caption.visible = true;
				inputBox.visible = true;
				
				victory = true;
				stopwatch.Stop();
				score = (int)(10000000 * ((float)hits / (float)shots) / (float)stopwatch.ElapsedMilliseconds);
			}
			else
			{
				score = (int)(10000000 * ((float)hits / (float)shots) / (float)stopwatch.ElapsedMilliseconds);
				((Layout.Textbox)layout.GetBox("scoreCounter")).text = score.ToString();
			}
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
			Database.Query("insert into timeState (name, score) values ('" + text + "', " + score + ")");
		}
	}
}