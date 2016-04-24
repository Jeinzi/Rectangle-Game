using System;
using System.Windows.Forms;

namespace RectangleGame
{
	static class Program
	{
		/// <summary>
		/// The form that is used by the application.
		/// </summary>
		public static Form1 mainForm;

		/// <summary>
		/// The main entry point of the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			mainForm = new Form1();
			Application.Run(mainForm);
		}
	}
}