using System.Drawing;
using System.Windows.Forms;

namespace RectangleGame.Gamestate
{
	class OptionState : GameState
	{
		/******** Variables ********/




		/******** Functions ********/

		// Constructor
		public OptionState(GameStateManager gameStateManager)
		{
			this.gameStateManager = gameStateManager;

			Layout.Inputbox box = new Layout.Inputbox("box1");
			box.anchor = Layout.Anchor.Center;
			//box.cursor = false;
#warning Why does maxSize influence box height?
#warning Fix size with empty textbox
			//box.maxSize = true;
			box.textAnchor = Layout.Anchor.Top | Layout.Anchor.CenterX;
			box.OnDelimiterEntered += onEnter;
			box.padding = 20;
			//box.allowMultiline = false;

			layout.AddBox(box);
		}

		public void onEnter(object sender, Layout.StringEventArgs e)
		{
			MessageBox.Show(e.text);
		}

		public override void Draw(Graphics g)
		{
			base.Draw(g);
		}

		
		// Executed, when a key is beeing pressed
		public override void KeyPressed(KeyEventArgs e)
		{
			base.KeyPressed(e);
			switch (e.KeyCode)
			{
				// Switching back to the main menu
				case Keys.Escape:
				//case Keys.Back:
					gameStateManager.SetState(GameStateManager.State.Menu);
					break;
			}
		}
	}
}