using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace Layout
{
	/// <summary>
	/// An inputbox saving the typed chars.
	/// </summary>
	public class Inputbox : Textbox
	{
		/******** Variables ********/
		/// <summary>
		/// Internally used random object.
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
		/// <summary>
		/// The time the blinking cursor is visible/invisible.
		/// </summary>
		private int cursorBlinkInterval;

		private Stopwatch stopwatch;
		private bool cursorVisible;
		public bool cursor;
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
		/// Disables Textbox.boxAdaptive.
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
			maxLength = 50;
			delimiter = Keys.Enter;
			cursorBlinkInterval = 500;

#warning Adapt to different Framerates with Update
			random = new Random((int)DateTime.Now.Ticks);
			stopwatch = new Stopwatch();
			stopwatch.Start();
			cursorPen = new Pen(Brushes.Black, 1);
			cursor = true;
		}

		/// <summary>
		/// Updates the Inputbox.
		/// </summary>
		public override void Update()
		{
			base.Update();
			if (cursor && stopwatch.Elapsed.Milliseconds >= cursorBlinkInterval)
			{
				stopwatch.Restart();
				cursorVisible = !cursorVisible;
			}
		}

		/// <summary>
		/// Renders the Inputbox to the given graphics object.
		/// </summary>
		/// <param name="g">The grahpics object to render the element to.</param>
		public override void Draw(System.Drawing.Graphics g)
		{
			if (!visible) return;
			// Fixing box to maximum size.
			if(maxSize)
			{
				string temp = new string('W', maxLength);
				AdaptToContent(g, temp);
			}

			// Disable adaptiveness of Inputbox, so the size calculated
			// by the following call of AdaptToContent will not be overwritten
			// in base class. This is done to get the same height as a filled Inputbox.
			bool wasAdaptive = false;
			if (text == "" && boxAdaptive == true)
			{
				boxAdaptive = false;
				wasAdaptive = true;
				AdaptToContent(g, ".");
				width = 2 * padding;
			}
			base.Draw(g);
			if(wasAdaptive)
			{
				boxAdaptive = true;
			}

			// Displaying cursor.
			if(cursorVisible)
			{
				Point cursorPosition = textPosition;
				if(text == "")
				{
					cursorPosition.X = absoluteX + padding;
					cursorPosition.Y = absoluteY + padding;
				}
				else
				{
					cursorPosition.X += GetTextSize(g).Width + 1;
				}

				Point cursorBottomPosition = cursorPosition;
				cursorBottomPosition.Y += height - 2 * padding;
				g.DrawLine(cursorPen, cursorPosition, cursorBottomPosition);
			}
		}

		/// <summary>
		/// Called if a key has been pressed.
		/// Passes this event to all elements within the layout.
		/// </summary>
		/// <param name="e">An object containing information about the event.</param>
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
				// Ignore '\r' and '\n' and Escape
				case Keys.Enter:
				case Keys.Escape:
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

		/// <summary>
		/// Called, if a character on the keyboard has been pressed.
		/// Passes this event to all elements within the layout.
		/// </summary>
		/// <param name="e">An object containing information about the event.</param>
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