using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace Plopper.Layout
{
	/// <summary>
	/// An inputbox saving the typed chars.
	/// </summary>
	public class Inputbox : Textbox
	{
		/******** Variables ********/
		/// <summary>
		/// Internaly used random object.
		/// </summary>
		private Random random;
		/// <summary>
		/// Indicating if the next char typed by the user should be added.
		/// </summary>
		private bool handled;
		/// <summary>
		/// The maximum length of the entered string.
		/// </summary>
		private int _maxLength;
		/// <summary>
		/// Indicating if the input box should have a fixed size to fit the maximum string length.
		/// </summary>
		private bool _maxSize;
		/// <summary>
		/// The delimiter causing the call of OnDelimiterEntered.
		/// </summary>
		public Keys delimiter;

		private Stopwatch stopwatch;
		public bool cursor;
		private bool cursorVisible;
		public Pen cursorPen;

		/// <summary>
		/// Represents a method dealing with an event that contains a string as event data.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void StringEventHandler(object sender, StringEventArgs e);
		/// <summary>
		/// Called when the delimiter is entered.
		/// </summary>
		public event StringEventHandler OnDelimiterEntered;



		/******** Getter & Setter ********/

		/// <summary>
		/// Gets or sets the maximum length of the entered string.
		/// Can't be set to a value less than 0.
		/// </summary>
		public int maxLength
		{
			get { return (_maxLength); }
			set
			{
				if (value >= 0) _maxLength = value;
				else _maxLength = 0;

				try
				{
					text = text.Remove(maxLength);
				}
				catch (ArgumentOutOfRangeException)
				{
					// Do nothing if the maximum length is bigger than the current length
				}
			}
		}

		/// <summary>
		/// Indicating if the input box should have a fixed size to fit the maximum string length.
		/// Disables Layout.Textbox.boxAdaptive.
		/// </summary>
		public bool maxSize
		{
			get { return (_maxSize); }
			set
			{
				if (value)
				{
					boxAdaptive = false;
					_maxSize = true;
				}
				else
				{
					_maxSize = false;
				}
			}
		}



		/******** Functions ********/

		public Inputbox(string identifier = "")
			: base(identifier)
		{
			this.maxLength = 50;
			this.delimiter = Keys.Enter;

#warning Adapt to different Framerates with Update
			random = new Random((int)DateTime.Now.Ticks);
			stopwatch = new Stopwatch();
			stopwatch.Start();
			cursorPen = new Pen(Brushes.Black, 1);
			cursor = true;
		}

		public override void Update()
		{
			base.Update();
			if (cursor && stopwatch.Elapsed.Milliseconds >= 500)
			{
				stopwatch.Restart();
				cursorVisible = !cursorVisible;
			}
		}

		public override void Draw(System.Drawing.Graphics g)
		{
			if (!visible) return;
			// Fixing box to maximum size
			if(maxSize)
			{
				string temp = new string('W', maxLength);
				AdaptToContent(g, temp);
			}
			base.Draw(g);
			// Displaying cursor
			if(cursorVisible)
			{
				Point cursorPosition = textPosition;
				if(text == "")
				{
					cursorPosition.Y = absoluteY + padding;
				}
				
				cursorPosition.X += GetTextSize(g).Width + 1;
				Point cursorBottomPosition = cursorPosition;
				cursorBottomPosition.Y += height - 2 * padding;
				g.DrawLine(cursorPen, cursorPosition, cursorBottomPosition);
			}
		}

		public override void CharPressed(KeyPressEventArgs e)
		{
			if (!visible) return;

			base.CharPressed(e);
			if (handled && text.Length < maxLength)
			{
				SFXPlayer.Play(Sound.Click1 + random.Next(0, 4));
				text += e.KeyChar;
			}
		}

		public override void KeyPressed(KeyEventArgs e)
		{
			if (!visible) return;
			base.KeyPressed(e);

			handled = true;
			switch (e.KeyCode)
			{
				// When the user is pressing backspace, the last character is deleted.
				// If the users is simultaneoulsy pressing control, the entire string is deleted.
				case Keys.Back:
					handled = false;
					if (text.Length > 0)
					{
						text = text.Remove(text.Length - 1);
						SFXPlayer.Play(Sound.Click1 + random.Next(0, 4));
					}
					if (e.Control) text = "";
					break;
				// Ignore '\r' and '\n'
				case Keys.Enter:
					handled = false;
					break;
			}

			// If the delimiter key is pressed, the OnDelimiterEntered Event is beeing invoked
			if (e.KeyCode == delimiter)
			{
				SFXPlayer.Play(Sound.Menu_Confirm);
				if (OnDelimiterEntered != null) OnDelimiterEntered.Invoke(this, new StringEventArgs(text));
			}
		}
	}


	/// <summary>
	/// An extension to the EventArgs class containing a string.
	/// </summary>
	public class StringEventArgs : EventArgs
	{
		public string text { get; private set; }

		public StringEventArgs(string text)
		{
			this.text = text;
		}
	}
}