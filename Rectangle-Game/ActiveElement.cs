using System.Drawing;
using System.Windows.Forms;

namespace Layout
{
	///	<summary>
	///	A class that serves as a parent for everything that recognizes user input.
	/// </summary>
	public abstract class ActiveElement
	{
		public virtual void Update() { }
		public virtual void Draw(Graphics g) { }
		public virtual void KeyPressed(KeyEventArgs e) { }
		public virtual void KeyReleased(KeyEventArgs e) { }
		public virtual void CharPressed(KeyPressEventArgs e) { }
		public virtual void MousePressed(MouseEventArgs e) { }
		public virtual void MouseReleased(MouseEventArgs e) { }
		public virtual void MouseMoved(MouseEventArgs e) { }
	}
}
