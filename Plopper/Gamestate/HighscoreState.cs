using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Plopper.Gamestate
{
	/// <summary>
	/// A gamestate displaying the highscores.
	/// </summary>
	class HighscoreState : GameState
	{
		/******** Variables ********/
		private List<string> timeRows;
		private List<string> rushRows;


		/******** Functions ********/

		public HighscoreState(GameStateManager gameStateManager)
		{
			this.gameStateManager = gameStateManager;
			timeRows = new List<string>();
			rushRows = new List<string>();

			// Creating layout
			layout = new Layout.Layout();

			// Vertical division
			Layout.Box upperBox = new Layout.Box("upperBox");
			upperBox.percentageSize = new SizeF(100, 20);
			upperBox.visible = false;

			Layout.Box lowerBox = new Layout.Box("lowerBox");
			lowerBox.percentageSize = new SizeF(100, 80);
			lowerBox.location = new Point(0, (int)((float)lowerBox.parent.height / 10 * 2));
			lowerBox.visible = false;

			// Horizontal division
			Layout.Box leftBox = new Layout.Box("leftBox");
			leftBox.parent = lowerBox;
			leftBox.percentageSize = new SizeF((float)33.3, 100);
			leftBox.anchor = Layout.Anchor.Left | Layout.Anchor.Top;
			leftBox.visible = false;

			Layout.Box centerBox = new Layout.Box("centerBox");
			centerBox.parent = lowerBox; 
			centerBox.percentageSize = new SizeF((float)33.3, 100);
			centerBox.anchor = Layout.Anchor.CenterX;
			centerBox.visible = false;

			Layout.Box rightBox = new Layout.Box("rightBox");
			rightBox.parent = lowerBox;
			rightBox.percentageSize = new SizeF((float)33.3, 100);
			rightBox.anchor = Layout.Anchor.Right | Layout.Anchor.Top;
			rightBox.visible = false;

			// Lists
			Layout.Textbox caption = new Layout.Textbox("caption");
			caption.borderLine = new Pen(Brushes.Black, 3);
			caption.font = new Font("ARIAL", 60);
			caption.text = "Highscore";
			caption.parent = upperBox;
			caption.anchor = Layout.Anchor.Center;

			Layout.Box leftList = new Layout.Box("leftList");
			leftList.parent = leftBox;
			leftList.y = 100;
			leftList.width = 300;
			leftList.height = 400;
			leftList.anchor = Layout.Anchor.CenterX;

			Layout.Box centerList = new Layout.Box("centerList");
			centerList.parent = centerBox;
			centerList.y = 100;
			centerList.width = 300;
			centerList.height = 400;
			centerList.anchor = Layout.Anchor.CenterX;

			// Captions
			Layout.Textbox leftCaption = new Layout.Textbox("leftCaption");
			leftCaption.parent = leftBox;
			leftCaption.text = "Time mode:";
			leftCaption.x = leftList.x;

			Layout.Textbox centerCaption = new Layout.Textbox("centerCaption");
			centerCaption.parent = centerBox;
			centerCaption.text = "Rush mode:";
			centerCaption.x = centerList.x;

			// Adding all to layout
			layout.AddBox(upperBox);
			layout.AddBox(lowerBox);

			layout.AddBox(leftBox);
			layout.AddBox(centerBox);
			layout.AddBox(rightBox);

			layout.AddBox(leftList);
			layout.AddBox(centerList);

			layout.AddBox(caption);
			layout.AddBox(leftCaption);
			layout.AddBox(centerCaption);
		}


		// Executed every time the gamestate is activated
		public override void Init()
		{
			base.Init();
			timeRows.Clear();
			rushRows.Clear();

			// Reading data to check correct execution
			int i = 0;
			string query = "select * from timeState order by score desc";
			SQLiteDataReader reader = Database.GetReader(query);
			while (reader.Read() && i++ < 15)
			{
				string row = reader["name"] + ":\t" + reader["score"];
				timeRows.Add(row);
			}

			i = 0;
			query = "select * from rushState order by score desc";
			reader = Database.GetReader(query);
			while (reader.Read() && i++ < 15)
			{
				string row = reader["name"] + ":\t" + reader["score"];
				rushRows.Add(row);
			}

			reader.Dispose();
		}


		// Drawing all the content to the given graphics object
		public override void Draw(Graphics g)
		{
			base.Draw(g);

			// Drawing highscore to the screen
			int yPos = layout.GetBox("leftList").area.Y + 10;
			int xPos = layout.GetBox("leftList").area.X + 10;
			Font highscoreFont = new Font("ARIAL", 12);
			foreach(string row in timeRows)
			{
				g.DrawString(row, highscoreFont, Brushes.Black, xPos, yPos);
				yPos += 20;
			}

			yPos = layout.GetBox("centerList").area.Y + 10;
			xPos = layout.GetBox("centerList").area.X + 10;
			foreach (string row in rushRows)
			{
				g.DrawString(row, highscoreFont, Brushes.Black, xPos, yPos);
				yPos += 20;
			}
		}


		// Executed, when a key is beeing pressed
		public override void KeyPressed(KeyEventArgs e)
		{
			base.KeyPressed(e);
			switch (e.KeyCode)
			{
				case Keys.Escape:
				case Keys.Back:
					gameStateManager.SetState(GameStateManager.State.Menu);
					break;
			}
		}
	}
}