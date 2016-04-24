using System.Drawing;
using System.Windows.Forms;

namespace RectangleGame.Gamestate
{
	class InfoState : GameState
	{
		/******** Variables ********/




		/******** Functions ********/

		public InfoState(GameStateManager gameStateManager)
		{
			this.gameStateManager = gameStateManager;
			Size formularSize = Program.mainForm.ClientSize;

			layout = new Layout.Layout();

			Layout.Textbox infoBox = new Layout.Textbox("infoBox");
			infoBox.anchor = Layout.Anchor.Center;
			infoBox.boxAdaptive = true;
			string infoString =
			"This is just a simple game to test the structure of such a project in C# \n" +
			"Written by Julian Heinzel \n" +
			"© 2015";
			infoBox.text = infoString;

			layout.AddBox(infoBox);
		}


		/// <summary>
		/// Executed when a key is pressed.
		/// </summary>
		/// <param name="e"></param>
		public override void KeyPressed(KeyEventArgs e)
		{
			base.KeyPressed(e);
			switch(e.KeyCode)
			{
				case Keys.Escape:
				case Keys.Back:
					gameStateManager.SetState(GameStateManager.State.Menu);
					break;
			}
		}
	}
}