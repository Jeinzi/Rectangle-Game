using System.Drawing;

namespace Layout
{
	public class Textbox : Box
	{
		/******** Variables ********/

		public Anchor textAnchor;
		public bool boxAdaptive;
		public string text;
		public Font font;
		public Brush textBrush;
		protected Point textPosition;
		private int _padding;
		private static Textbox savedStyle;

		// Default values.
		public static bool defaultBoxAdaptive = true;
		public static int defaultPadding = 5;
		public static Font defaultFont = new Font("ARIAL", 12);
		public static Brush defaultTextBrush = Brushes.Black;
		public static Brush defaultFill = (Brush)Layout.defaultFill.Clone();
		public static Pen defaultBorderline = (Pen)Layout.defaultBorderLine.Clone();
		public static Anchor defaultAnchor = Anchor.None;
		public static Anchor defaultTextAnchor = Anchor.None;


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

		/// <summary>
		/// Constructs a Textbox object with the given parameters.
		/// </summary>
		/// <param name="text">The text to show within the textbox.</param>
		/// <param name="area">The area the textbox should cover.</param>
		/// <param name="fill">The brush used to fill the inner of the textbox.</param>
		/// <param name="borderLine">The pen used to outline the textbox.</param>
		/// <param name="identifier">Identifier string for element, defaults to empty string.</param>
		public Textbox(string text, Rectangle area, Brush fill, Pen borderLine, string identifier = "")
			: base(area, fill, borderLine, identifier)
		{
			this.text = text;
			textPosition = area.Location;
			boxAdaptive = defaultBoxAdaptive;
			padding = defaultPadding;
			anchor = defaultAnchor;
			textAnchor = defaultTextAnchor;
			textBrush = defaultTextBrush;
			font = defaultFont;
		}

		/// <summary>
		/// Constructor using standard layout values for box.
		/// </summary>
		/// <param name="text">The text to show within the textbox.</param>
		/// <param name="area">The area the textbox should cover.</param>
		/// <param name="identifier">Identifier string for element, defaults to empty string.</param>
		public Textbox(string text, Rectangle area, string identifier = "")
			: this(text, area, defaultFill, defaultBorderline, identifier)
		{ }

		/// <summary>
		/// Standard constructor.
		/// </summary>
		/// <param name="identifier">Identifier string for element, defaults to empty string.</param>
		public Textbox(string identifier = "")
			: this("", new Rectangle(), identifier)
		{ }

		/// <summary>
		/// Updates the textbox.
		/// </summary>
		public override void Update()
		{
			base.Update();
		}

		/// <summary>
		/// Draws the box to the given graphics object.
		/// </summary>
		/// <param name="g">The graphics object to render to.</param>
		public override void Draw(Graphics g)
		{
			if (!visible) return;

			// Need to calculate the position of the text here,
			// because the graphics object is needed.
			if (boxAdaptive) AdaptToContent(g);
			anchor = anchor;

			base.Draw(g);

#warning Deal with it!
			StringFormat format = StringFormat.GenericTypographic;
			format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

			textPosition = area.Location;
			textPosition.X += padding;
			textPosition.Y += padding;
			SetTextAnchor(g, textAnchor);
			g.DrawString(text, font, textBrush, textPosition, format);
		}

		/// <summary>
		/// Adapts the size of the box to its content.
		/// </summary>
		/// <param name="g">The graphics object used to determine the size of the box's content.</param>
		/// <param name="content">Content to which the box should adapt. Defaults to null, which is interpreted as the currently displayed text.</param>
		protected void AdaptToContent(Graphics g, string content = null)
		{
			if (content == null) content = text;
			Size textSize = GetTextSize(g, content);
			width = textSize.Width + 2 * padding;
			height = textSize.Height + 2 * padding;

			textPosition = area.Location;
			textPosition.X += padding;
			textPosition.Y += padding;
		}

		/// <summary>
		/// You may center the text similar to the box itself.
		/// You may combine the anchors using the | operator.
		/// Combining the top, left, right and bottom anchor will result in centering the element.
		/// </summary>
		/// <param name="g">The graphics object used to determine the textsize.</param>
		/// <param name="anchor">An anchor object determining the relative position of the text wihin the box.</param>
#warning Ambiguous to Textbox.textAnchor
		private void SetTextAnchor(Graphics g, Anchor anchor)
		{
			Size textSize = GetTextSize(g);

			// Aligned right.
			if ((anchor & Anchor.Right) == Anchor.Right)
			{
				textPosition.X = absoluteX + width - textSize.Width - padding;
			}
			// Aligned Down.
			if ((anchor & Anchor.Bottom) == Anchor.Bottom)
			{
				textPosition.Y = absoluteY + height - textSize.Height - padding;
			}
			// Aligned left.
			if ((anchor & Anchor.Left) == Anchor.Left)
			{
				textPosition.X = absoluteX + padding;
			}
			// Aligned up.
			if ((anchor & Anchor.Top) == Anchor.Top)
			{
				textPosition.Y = absoluteY + padding;
			}
			// Box centered on x axis.
			if ((anchor & Anchor.CenterX) == Anchor.CenterX)
			{
				textPosition.X = (int)((float)absoluteX + ((float)width / 2.0) - ((float)textSize.Width / 2.0));
			}
			// Box centered on y axis.
			if ((anchor & Anchor.CenterY) == Anchor.CenterY)
			{
				textPosition.Y = absoluteY + (height / 2) - (textSize.Height / 2);
			}

			this.textAnchor = anchor;
		}


		/// <summary>
		/// Returns the size of the currently saved text.
		/// </summary>
		/// <param name="g">The graphics object used to determine the size of the given string.</param>
		/// <param name="text">The string of interest. Defaults to null, which will get the size of the currently displayed text.</param>
		/// <returns>The size of the specified text.</returns>
		protected Size GetTextSize(Graphics g, string text = null)
		{
			if (text == null) text = this.text;
#warning Deal with it!
			StringFormat format = StringFormat.GenericTypographic;
			format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
#warning Not independent of program
			Size size = g.MeasureString(text, font, RectangleGame.Program.mainForm.ClientSize.Width, format).ToSize();
			return (size);
		}

		/// <summary>
		/// Saves the current default textbox style to restore it later on.
		/// </summary>
		public static void SaveDefaultStyle()
		{
			savedStyle = new Textbox();
			savedStyle.boxAdaptive = defaultBoxAdaptive;
			savedStyle.padding = defaultPadding;
			savedStyle.font = defaultFont;
			savedStyle.textBrush = defaultTextBrush;
			savedStyle.fill = defaultFill;
			savedStyle.borderLine = defaultBorderline;
			savedStyle.anchor = defaultAnchor;
			savedStyle.textAnchor = defaultTextAnchor;
		}

		/// <summary>
		/// Restores the currently saved default textbox style, if there is one saved.
		/// </summary>
		/// <returns>Boolean variable indicating if the default style has been restored successfully.</returns>
		public static bool RestoreDefaultStyle()
		{
			if (savedStyle == null) return (false);

			defaultBoxAdaptive = savedStyle.boxAdaptive;
			defaultPadding = savedStyle.padding;
			defaultFont = savedStyle.font;
			defaultTextBrush = savedStyle.textBrush;
			defaultFill = savedStyle.fill;
			defaultBorderline = savedStyle.borderLine;
			defaultAnchor = savedStyle.anchor;
			defaultTextAnchor = savedStyle.textAnchor;
			return (true);
		}
	}
}