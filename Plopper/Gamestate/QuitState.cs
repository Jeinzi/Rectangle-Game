using System.Windows.Forms;

namespace Plopper.Gamestate
{
	/// <summary>
	/// Quits the application in initialisation.
	/// </summary>
	class QuitState : GameState
	{
		public QuitState(GameStateManager gameStateManager)
		{
			this.gameStateManager = gameStateManager;
		}

		public override void Init() { Application.Exit(); }
	}
}