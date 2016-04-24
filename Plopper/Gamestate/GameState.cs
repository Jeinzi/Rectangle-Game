using System.Drawing;
using System.Windows.Forms;

namespace Plopper.Gamestate
{
	public abstract class GameState : ActiveElement
	{
		/**** Variables ****/

	    protected GameStateManager gameStateManager;
		protected Layout.Layout layout;

		/**** Functions ****/

		public GameState()
		{
			layout = new Layout.Layout();
		}

		public virtual void Init() { }

		/**** Calling methods of base class and the corrisponding layout functions ****/

		public override void Draw(Graphics g)
		{
			base.Draw(g);
			layout.Draw(g);
		}

		public override void Update()
		{
			base.Update();
			layout.Update();
		}

		public override void KeyPressed(KeyEventArgs e)
		{
			base.KeyPressed(e);
			layout.KeyPressed(e);
		}

		public override void KeyReleased(KeyEventArgs e)
		{
			base.KeyReleased(e);
			layout.KeyReleased(e);
		}

		public override void CharPressed(KeyPressEventArgs e)
		{
			base.CharPressed(e);
			layout.CharPressed(e);
		}

		public override void MouseMoved(MouseEventArgs e)
		{
			base.MouseMoved(e);
			layout.MouseMoved(e);
		}

		public override void MousePressed(MouseEventArgs e)
		{
			base.MousePressed(e);
			layout.MousePressed(e);
		}

		public override void MouseReleased(MouseEventArgs e)
		{
			base.MouseReleased(e);
			layout.MouseReleased(e);
		}
	}
}