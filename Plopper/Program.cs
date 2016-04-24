using System;
using System.Windows.Forms;

namespace Plopper
{
	static class Program
	{
		/// <summary>
		/// The form that is used by the application
		/// </summary>
		public static Form1 mainForm;

		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
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