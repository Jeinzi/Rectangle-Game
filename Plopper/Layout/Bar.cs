using System.Drawing;
using System.Windows.Forms;

namespace Plopper.Layout
{
	public class Bar : Box
	{
		/******** Variables ********/
		private int _padding;
		private double _percentFull;
		private double _maxValue;
		private double _value;
		public string status;
		public string caption;
		public Font statusFont;
		public Font captionFont;
		public Brush textBrush;


		/******** Getter & Setter ********/

		// Make sure padding doesn't get negative
		public int padding
		{
			get { return (_padding); }
			set
			{
				if (value < 0) _padding = 0;
				else _padding = value;
			}
		}

		// Make sure percentFull stays between 0 and 100
		public double percentFull
		{
			get { return (_percentFull); }
			set
			{
				if (value < 0) _percentFull = 0;
				else if (value > 100) _percentFull = 100;
				else _percentFull = value;
				this._value = _maxValue * _percentFull / 100;
			}
		}

		// Calculate the percentage the bar is filled when the max value is updated
		public double maxValue
		{
			get { return (_maxValue); }
			set
			{
				if (value <= 0) _maxValue = 1;
				else _maxValue = value;
				percentFull = this.value / maxValue * 100;
			}
		}

		// Calculate the percentage the bar is filled when the value is updated
		public double value
		{
			get { return (_value); }
			set
			{
				if (value < 0) _value = 0;
				else if (value > maxValue) _value = maxValue;
				else _value = value;

				percentFull = this.value / maxValue * 100;
			}
		}

		/******** Functions ********/

		// Constructor
		public Bar(Rectangle area, string identifier = "")
			: base(area, identifier)
		{
			padding = 1;
			percentFull = 0;
			maxValue = 100;
			value = 0;
			caption = "Loading";
			statusFont = new Font("ARIAL", 10);
			captionFont = new Font("ARIAL", 20);
			textBrush = Brushes.Black;
			fill = Brushes.Green;
		}

		// Standard constructor
		public Bar(string identifier = "")
			: this(new Rectangle(0, 0, 400, 40), identifier) { }


		// Drawing the current progressbar
		public override void Draw(Graphics g)
		{
			// Getting text sizes
			PaintEventArgs painting = new PaintEventArgs(g, new Rectangle());
			Point statusPosition = new Point();
			Point captionPosition = new Point();
			Size statusSize = painting.Graphics.MeasureString(status, statusFont).ToSize();
			Size captionSize = painting.Graphics.MeasureString(caption, captionFont).ToSize();

			// Calculating text positions
			statusPosition.X = area.X + area.Width / 2 - statusSize.Width / 2;
			statusPosition.Y = area.Y + area.Height + 5;
			captionPosition.X = area.X + area.Width / 2 - captionSize.Width / 2;
			captionPosition.Y = area.Y - 35;

			// Drawing inner progress bar
			Rectangle fillRectangle = area;
			fillRectangle.X += 1 + ((int)borderLine.Width - 1) / 2 + padding;
			fillRectangle.Y += 1 + ((int)borderLine.Width - 1) / 2 + padding;
			fillRectangle.Width -= 2 * padding + (int)borderLine.Width;
			fillRectangle.Height -= 2 * padding + (int)borderLine.Width;
			fillRectangle.Width = (int)((float)fillRectangle.Width / 100 * percentFull);
			g.FillRectangle(fill, fillRectangle);

			// Drawing outer progress bar
			g.DrawRectangle(borderLine, area);
			g.DrawString(caption, captionFont, textBrush, captionPosition);
			g.DrawString(status, statusFont, textBrush, statusPosition);
		}
	}
}
