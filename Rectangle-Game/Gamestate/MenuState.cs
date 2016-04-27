using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Layout;

namespace RectangleGame.Gamestate
{
	public class MenuState : GameState
	{
		/******** Variables ********/

		private int currentChoice;
		private List<Option> options;

		/******** Functions ********/

		public MenuState(GameStateManager gameStateManager)
		{
			this.gameStateManager = gameStateManager;

			// Creating Layout.
			Textbox caption = new Textbox("caption");
			caption.text = "Rectangles";
			caption.font = new Font("ARIAL", 90);
			caption.borderLine = new Pen(Brushes.Black, 3);
			caption.y = 100;
			caption.anchor = Layout.Anchor.CenterX;

			Box optionBox = new Box("optionBox");
			optionBox.width = 200;
			optionBox.height = 200;
			optionBox.anchor = Layout.Anchor.Center;

			layout.AddBox(caption);
			layout.AddBox(optionBox);

			// Creating options within the main menu.
			options = new List<Option>();
			options.Add(new Option("Fight against the time!", GameStateManager.State.Time));
			options.Add(new Option("Rush Mode", GameStateManager.State.Rush));
			options.Add(new Option("View ya' scores", GameStateManager.State.Highscore));
			options.Add(new Option("General Info", GameStateManager.State.Info));
			options.Add(new Option("Options", GameStateManager.State.Options));
			options.Add(new Option("Stop playing", GameStateManager.State.Quit));
			currentChoice = 0;
		}


		/// <summary>
		/// Draws all the content to the given graphics object.
		/// </summary>
		/// <param name="g">The graphics object in question.</param>
		public override void Draw(Graphics g)
		{
			base.Draw(g);

			// Drawing options to the screen
			int xPos = layout.GetBox("optionBox").area.X + 10;
			int yPos = layout.GetBox("optionBox").area.Y + 10;
			Font optionFont = new Font("ARIAL", 12);
			Brush optionColor = Brushes.Black;

			for(var i = 0; i < options.Count; i++)
			{
				// Changing text color to draw the choosen element
				if(i == currentChoice) optionColor = Brushes.Blue;
				else optionColor = Brushes.Black;

				// Finally drawing
				g.DrawString(options[i].GetTitle(), optionFont, optionColor, xPos, yPos);
				yPos += 20;
			}
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
				// Quitting application
				//case Keys.Back:
				case Keys.Escape:
					Application.Exit();
					break;
				// One element up
				case Keys.W:
				case Keys.Up:
					currentChoice--;
					if (currentChoice < 0) currentChoice = options.Count - 1;
					SFXPlayer.Play(Sound.Menu_Toggle);
					break;
				// One element down
				case Keys.S:
				case Keys.Down:
					currentChoice++;
					if (currentChoice >= options.Count) currentChoice = 0;
					SFXPlayer.Play(Sound.Menu_Toggle);
					break;

				// Confirm choice
				case Keys.Enter:
					//case Keys.Space:
					SFXPlayer.Play(Sound.Menu_Confirm);
					gameStateManager.SetState(options[currentChoice].GetState());
					break;
			}
		}
	}


	/// <summary>
	/// Class representing the different options and the matching gamestates within the main menu.
	/// </summary>
	class Option
	{
		private string title;
		private GameStateManager.State executedState;

		public Option(string title, GameStateManager.State executedState)
		{
			this.title = title;
			this.executedState = executedState;
		}

		public string GetTitle() { return (title); }
		public GameStateManager.State GetState() { return (executedState); }
	}
}