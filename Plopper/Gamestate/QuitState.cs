using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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