using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

/* TOCHTER:
 * VorVariablen: Alle Monster
 *
 * Konstruktor:
 *	this.gsm = gsm;
 *	this.player = player;
 *
 *	Variablen:
 *		DrawInventory = false, startX, startY, resetCoord = true
 *	MainTileMap = new TileMap(".txt", ".png", Tilegröße)
 *	Monster myMonster = new Monster(MainTileMap, X, Y)
 * Init()
 *	Wenn ueberschreiben, dann mit super.Init(); PositionSet Regeln mit resetCoord
 * Update()
 *	Monster.Update(); super.Update();
 * DrawMobs()
 *	Monster.Draw();
 * Draw()
 *	Wenn üeberschreiben, dann super.Draw() zuerst, sonst wird übermalt
 * KeyPressed();
 *	Wenn überschreiben, dann mit super(), das Meiste ist schon drin.
 * KeyReleased();
 *	Wenn überschreiben, dann mit super(), das Meiste ist schon drin.
 */

namespace Plopper.Gamestate
{
	public abstract class GameState : ActiveElement
	{
	    protected GameStateManager gameStateManager;
		protected Layout.Layout layout;

		public GameState()
		{
			layout = new Layout.Layout();
		}

		public virtual void Init() { }



		// Calling methods of base class and calling the corrisponding layout functions

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
