using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Layout;

namespace RectangleGame.Gamestate
{
	class LevelState : GameState
	{
		/******** Variables ********/

		protected bool firstBlood;
		protected bool victory;
		protected int shots;
		protected int hits;
		protected int score;
		protected int doubleHits;

		protected Stopwatch stopwatch;
		protected Random random;
		protected List<Entity.Entity> entities;
		protected Map.Map map;

		protected Box centerBox;
		protected Textbox caption;
		protected Inputbox inputBox;


		/******** Functions ********/

		public LevelState(GameStateManager gameStateManager)
		{
			// Randomize using ticks.
			random = new Random((int)DateTime.Now.Ticks);
			
			this.gameStateManager = gameStateManager;
			firstBlood = false;
			victory = false;
			stopwatch = new Stopwatch();
			entities = new List<Entity.Entity>();
			map = new Map.Map(Program.mainForm.ClientSize.Width / 4 * 3, Program.mainForm.ClientSize.Height / 4 * 3);
			map.position.X = Program.mainForm.ClientSize.Width / 2 - map.size.Width;
			map.position.Y = Program.mainForm.ClientSize.Height / 2 - map.size.Height;

			layout = new Layout.Layout();
			layout.backgroundBrush = Brushes.Transparent;

			Layout.Box statusBox = new Layout.Box("statusBox");
			statusBox.percentageWidth = 10;
			statusBox.percentageHeight = 20;
			statusBox.percentageLocation = new PointF(89, 12.5f);

			// Redefining textbox standards.
			Textbox.SaveDefaultStyle();
			Textbox.defaultAnchor = Anchor.Left;
			Textbox.defaultBorderline = Pens.Transparent;
			Textbox.defaultFill = Brushes.Transparent;

			// Labels.
			Textbox timerLabel = new Layout.Textbox("timerLabel");
			Textbox shotCounterLabel = new Layout.Textbox("shotCounterLabel");
			Textbox enemyCounterLabel = new Layout.Textbox("enemyCounterLabel");
			Textbox scoreCounterLabel = new Layout.Textbox("scoreCounterLabel");
			timerLabel.parent = statusBox;
			shotCounterLabel.parent = statusBox;
			enemyCounterLabel.parent = statusBox;
			scoreCounterLabel.parent = statusBox;
			timerLabel.percentageY = 5;
			shotCounterLabel.percentageY = 30;
			enemyCounterLabel.percentageY = 55;
			scoreCounterLabel.percentageY = 80;
			timerLabel.text = "Time:";
			shotCounterLabel.text = "Shots:";
			enemyCounterLabel.text = "Hits:";
			scoreCounterLabel.text = "Score:";

			// Infoboxes.
			Textbox.defaultAnchor = Anchor.Right;

			Textbox timer = new Textbox("timer");
			Textbox shotCounter = new Textbox("shotCounter");
			Textbox enemyCounter = new Textbox("enemyCounter");
			Textbox scoreCounter = new Textbox("scoreCounter");
			timer.parent = statusBox;
			shotCounter.parent = statusBox;
			enemyCounter.parent = statusBox;
			scoreCounter.parent = statusBox;
			timer.percentageY = timerLabel.percentageY;
			shotCounter.percentageY = shotCounterLabel.percentageY;
			enemyCounter.percentageY = enemyCounterLabel.percentageY;
			scoreCounter.percentageY = scoreCounterLabel.percentageY;

			Textbox.RestoreDefaultStyle();

			// Adding boxes to layout.
			layout.AddBox(statusBox);
			layout.AddBox(timer);
			layout.AddBox(timerLabel);
			layout.AddBox(shotCounter);
			layout.AddBox(shotCounterLabel);
			layout.AddBox(enemyCounter);
			layout.AddBox(enemyCounterLabel);
			layout.AddBox(scoreCounter);
			layout.AddBox(scoreCounterLabel);

			// Already setting up victory dialogue.
#warning You may not set the relative position after setting the anchor or every child box will not show up correctly
			centerBox = new Layout.Box("centerBox");
			centerBox.percentageSize = new SizeF(25, 15);
			centerBox.anchor = Layout.Anchor.Center;
			centerBox.borderLine.Width = 3;
			centerBox.borderLine.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
			centerBox.fill = Brushes.LightBlue;

			caption = new Layout.Textbox("caption");
			caption.parent = centerBox;
			caption.text = "Please enter your name:";
			caption.percentageY = 20;
			caption.anchor = Layout.Anchor.CenterX;

			inputBox = new Layout.Inputbox("inputBox");
			inputBox.parent = centerBox;
			inputBox.percentageY = 60;
			inputBox.anchor = Layout.Anchor.CenterX;
			inputBox.text = "";
			inputBox.maxLength = 15;
			inputBox.maxSize = true;
			inputBox.textAnchor = Layout.Anchor.Center;
			inputBox.OnDelimiterEntered += NameEntered;

			layout.AddBox(centerBox);
			layout.AddBox(caption);
			layout.AddBox(inputBox);
		}


		/// <summary>
		/// Executed every time the gamestate is activated.
		/// </summary>
		public override void Init()
		{
			base.Init();
			centerBox.visible = false;
			caption.visible = false;
			inputBox.visible = false;

			// Resetting variables.
			map = new Map.Map(Program.mainForm.ClientSize.Width / 4 * 3, Program.mainForm.ClientSize.Height / 4 * 3);
			lock (entities)
			{
				entities.Clear();
			}
			firstBlood = false;
			victory = false;
			score = 0;
			shots = 0;
			hits = 0;
			doubleHits = 0;
			stopwatch.Reset();
		}


		/// <summary>
		/// Updates all the content within the gamestate that is changing over time.
		/// </summary>
		public override void Update()
		{
			base.Update();
			((Layout.Textbox)layout.GetBox("timer")).text = stopwatch.Elapsed.ToString(@"m\:ss\:f");
			((Layout.Textbox)layout.GetBox("shotCounter")).text = shots.ToString();
			((Layout.Textbox)layout.GetBox("enemyCounter")).text = hits.ToString();
			((Layout.Textbox)layout.GetBox("scoreCounter")).text = score.ToString();

			// Updating objects
			if (victory) return;
			map.Update();
			lock (entities)
			{
				foreach (Entity.Entity entity in entities)
				{
					entity.Update();
				}
			}
		}


		/// <summary>
		/// Draws all the content to the given graphics object.
		/// </summary>
		/// <param name="g">The graphics object in question.</param>
		public override void Draw(Graphics g)
		{
			// Drawing all the objects.
			layout.Clear(g, Layout.Layout.defaultBackgroundBrush);
			g.TranslateTransform(map.position.X, map.position.Y);
			map.Draw(g);
			lock (entities)
			{
				foreach (Entity.Entity entity in entities)
				{
					entity.Draw(g);
				}
			}
			g.ResetTransform();
			base.Draw(g);
		}


		/// <summary>
		/// Executed when a key is pressed.
		/// </summary>
		/// <param name="e"></param>
		public override void KeyPressed(KeyEventArgs e)
		{
			base.KeyPressed(e);
			switch (e.KeyCode)
			{
				case Keys.Escape:
				//case Keys.Back:
					gameStateManager.SetState(GameStateManager.State.Menu);
					break;
			}
		}


		/// <summary>
		/// Executed when a mouse button is pressed.
		/// </summary>
		/// <param name="e"></param>
		public override void MousePressed(MouseEventArgs e)
		{
			// Do nothing if the game already has stopped.
			if (victory) return;

			base.MousePressed(e);
			List<Entity.Entity> toBeRemoved = new List<Entity.Entity>();
			shots++;

			// Convert display coordinates into coordinates on map.
			Point loctationOnMap = map.ToMapCoordinates(e.Location);
			Rectangle collisionBox = new Rectangle(loctationOnMap, new Size(1, 1));

			// Gathering entities that were hit by the cursor.
			foreach (Entity.Entity entity in entities)
			{
				if (entity.CollidesWith(collisionBox))
				{
					toBeRemoved.Add(entity);
				}
			}
			if(toBeRemoved.Count > 1)
			{
				doubleHits = toBeRemoved.Count - 1;
			}

			// Removing the entities.
			lock (entities)
			{
				foreach (Entity.Entity entity in toBeRemoved)
				{
					entities.Remove(entity);
					EntityDies();
				}
			}
		}

		/// <summary>
		/// Should be called whenever an entity dies.
		/// Counts hits, plays sounds and stops time.
		/// </summary>
		protected virtual void EntityDies()
		{
			hits++;
			int plopID = random.Next(0, 6);
			SFXPlayer.Play(Sound.Plop1 + plopID);

			// If this has been the first entity to be killed in this act of cruel violence,
			// the stopwatch is started to measure the time the user needs to get the remainig ones.
			if (!firstBlood)
			{
				stopwatch.Reset();
				stopwatch.Start();
				firstBlood = true;
			}
		}


		/// <summary>
		/// Returns an entity of random size and color at a random position.
		/// </summary>
		/// <param name="number">The number of entities to be generated.</param>
		protected Entity.Entity GetRandomEntity()
		{
			// Get random color
			int r = random.Next(0, 256);
			int g = random.Next(0, 256);
			int b = random.Next(0, 256);
			Brush entityBrush = new SolidBrush(Color.FromArgb(r, g, b));

			// Get random size
			int tempSize = random.Next(10, 30);
			Size entitySize = new Size(tempSize, tempSize);
			// Get random position
			int tempX = random.Next(0, map.size.Width - entitySize.Width);
			int tempY = random.Next(0, map.size.Height - entitySize.Height);
			Point entityPosition = new Point(tempX, tempY);
			// Generate entity
			Rectangle entityRectangle = new Rectangle(entityPosition, entitySize);
			return (new Entity.Entity(entityRectangle, entityBrush, map));
		}

		/// <summary>
		/// Switches to the menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void NameEntered(object sender, Layout.StringEventArgs e)
		{
			gameStateManager.SetState(GameStateManager.State.Menu);
		}
	}
}