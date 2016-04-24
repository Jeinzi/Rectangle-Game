using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Plopper.Layout
{
	public class Textbox : Box
	{
		/******** Variables ********/
		public Anchor textAnchor;
		private int _padding;
		public bool boxAdaptive;
		public string text = "";
		public Font font = new Font("ARIAL", 12);
		public Brush textBrush = Brushes.Black;
		protected Point textPosition;


		/******** Getter & Setter ********/

		// Padding must not become negative
		public int padding
		{
			get { return (_padding); }
			set
			{
				if (value < 0) _padding = 0;
				else _padding = value;
			}
		}

		/******** Functions ********/

		// Constructor
		public Textbox(string text, Rectangle area, Brush fill, Pen borderLine, string identifier = "")
			: base(area, fill, borderLine, identifier)
		{
			this.text = text;
			boxAdaptive = true;
			textPosition = area.Location;
			padding = 5;
			textAnchor = Anchor.Left | Anchor.CenterY;
		}

		// Constructor using standard layout values for box
		public Textbox(string text, Rectangle area, string identifier = "")
			: this(text, area, (Brush)Layout.standardFill.Clone(), (Pen)Layout.standardBorderLine.Clone(), identifier) { }

		// Standard constructor
		public Textbox(string identifier = "")
			: this("", new Rectangle(), identifier) { }


		// Updating the textbox
		public override void Update()
		{
			base.Update();
		}


		// Drawing the box to the screen
		public override void Draw(Graphics g)
		{
			if (!visible) return;

			// Need to calculate the position of the text here,
			// because the graphics object is needed
			if (boxAdaptive) AdaptToContent(g);
			anchor = anchor;
			if (boxAdaptive) AdaptToContent(g);

			base.Draw(g);
		
#warning Deal with it!
			StringFormat format = StringFormat.GenericTypographic;
			format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

			SetTextAnchor(g, textAnchor);
			g.DrawString(text, font, textBrush, textPosition, format);
		}

		/// <summary>
		/// Adapts the size of the box to it's content.
		/// </summary>
		/// <param name="g"></param>
		protected void AdaptToContent(Graphics g, string content = null)
		{
			if (content == null) content = text;
			textPosition = area.Location;
			textPosition.X += padding;
			textPosition.Y += padding;

			Size textSize = GetTextSize(g, content);
			width = textSize.Width + 2 * padding;
			height = textSize.Height + 2 * padding;
			if(text == "")
			{

			}
		}

		/// <summary>
		/// You may center the the text similar to the box itself.
		/// You may combine the anchors using the | operator.
		/// Combining the top, left, right and bottom anchor will result in centering the element.
		/// </summary>
		private void SetTextAnchor(Graphics g, Anchor anchor)
		{
			Size textSize = GetTextSize(g);

			// Aligned right
			if ((anchor & Anchor.Right) == Anchor.Right)
			{
				textPosition.X = absoluteX + width - textSize.Width - padding;
			}
			// Aligned Down
			if ((anchor & Anchor.Bottom) == Anchor.Bottom)
			{
				textPosition.Y = absoluteY + height - textSize.Height - padding;
			}
			// Aligned left
			if ((anchor & Anchor.Left) == Anchor.Left)
			{
				textPosition.X = absoluteX + padding;
			}
			// Aligned up
			if ((anchor & Anchor.Top) == Anchor.Top)
			{
				textPosition.Y = absoluteY + padding;
			}
			// Box centered on x axis
			if ((anchor & Anchor.CenterX) == Anchor.CenterX)
			{
				textPosition.X = (int)((float)absoluteX + ((float)width / 2.0) - ((float)textSize.Width / 2.0));
			}
			// Box centered on y axis
			if ((anchor & Anchor.CenterY) == Anchor.CenterY)
			{
				textPosition.Y = absoluteY + (height / 2) - (textSize.Height / 2);
			}

			this.textAnchor = anchor;
		}


		// Returns the size of the currently saved text
		protected Size GetTextSize(Graphics g, string text = null)
		{
			if (text == null) text = this.text;
#warning Deal with it!
			StringFormat format = StringFormat.GenericTypographic;
			format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

			Size size = g.MeasureString(text, font, Program.mainForm.ClientSize.Width, format).ToSize();
			return (size);
		}
	}
}