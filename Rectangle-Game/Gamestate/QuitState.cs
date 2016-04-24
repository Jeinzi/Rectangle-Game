using System.Windows.Forms;

namespace RectangleGame.Gamestate
{
	/// <summary>
	/// Quits the application on initialisation.
	/// </summary>
	class QuitState : GameState
	{
		public QuitState(GameStateManager gameStateManager)
		{
			this.gameStateManager = gameStateManager;
		}

		/// <summary>
		/// Closes the application.
		/// </summary>
		public override void Init() { Application.Exit(); }
	}
}