using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace RectangleGame.Gamestate
{
	public class GameStateManager
	{
		/******** Variables ********/

		/// <summary>
		/// Enumerator encapsulating the different gamestates.
		/// </summary>
		public enum State
		{
			Menu,
			Highscore,
			Info,
			Options,
			Quit,
			Time,
			Rush
		}

		private State lastState;
		private State currentState;
		private List<GameState> gameStates;
		
		
		/******** Constructor ********/
		
		public GameStateManager()
		{
			// Initializing variables.
			currentState	= State.Menu;
			lastState		= State.Menu;
			gameStates		= new List<GameState>();
			
			// Adding gamestates.
			gameStates.Add(new MenuState(this));
			gameStates.Add(new HighscoreState(this));
			gameStates.Add(new InfoState(this));
			gameStates.Add(new OptionState(this));
			gameStates.Add(new QuitState(this));
			gameStates.Add(new TimeState(this));
			gameStates.Add(new RushState(this));

			SetState(currentState);
		}


		/******** Functions ********/

		// Passing update and draw on to the currently active gamestate.
		public void Update() { gameStates[(int)currentState].Update(); }
		public void Draw(Graphics g) { gameStates[(int)currentState].Draw(g); }
		

		/******** Getter & Setter ********/
		
		public State GetState() { return (currentState); }
		
		public void SetState(State state)
		{
			lastState		= currentState;
			currentState	= state;
			gameStates[(int)currentState].Init();
		}

		/******** Keyboard ********/
		
		// User input is passed to the currelty active gamestate.
		public void KeyPressed(KeyEventArgs e) { gameStates[(int)currentState].KeyPressed(e); }
		public void KeyReleased(KeyEventArgs e) { gameStates[(int)currentState].KeyReleased(e); }
		public void CharPressed(KeyPressEventArgs e) { gameStates[(int)currentState].CharPressed(e); }
		public void MousePressed(MouseEventArgs e) { gameStates[(int)currentState].MousePressed(e); }
		public void MouseReleased(MouseEventArgs e) { gameStates[(int)currentState].MouseReleased(e); }
		public void MouseMoved(MouseEventArgs e) { gameStates[(int)currentState].MouseMoved(e); }
	}
}