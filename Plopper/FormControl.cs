using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace Plopper
{
	public class FormControl
	{
		//Klasse zum Kontrollieren von Formulareigenschaften

		//Variablen und Konstanten werden deklariert/definert
		private const int SW_HIDE = 0;
		private const int SW_SHOW = 1;
		private const int SM_SCREENX = 0;
		private const int SM_SCREENY = 1;
		private const int SWP_SHOWWINDOW = 64;

		private Form TargetForm;
		private FormWindowState WindowState;
		private FormBorderStyle BorderStyle;
		private bool TopMost;
		private bool Maximized;
		private Rectangle Bounds;


		public FormControl(Form TargetForm)
		{
			this.TargetForm = TargetForm;
		}

		//Benötigte Methoden zur Formularkontrolle werden eingebunden und definiert
		[DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
		private static extern int GetSystemMetrics(int smIndex);
		[DllImport("user32.dll")]
		private static extern void SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int width, int height, uint flags);


		//Eigene Methoden werden definiert
		public static int GetScreenWidth()
		{
			//Gibt die Breite des Hauptbildschirmes in Pixeln zurück
			return GetSystemMetrics(SM_SCREENX);
		}


		public static int GetScreenHeight()
		{
			//Gibt die Höhe des Hauptbildschirmes in Pixeln zurück
			return GetSystemMetrics(SM_SCREENY);
		}


		private void SaveFormState()
		{
			//Speichert die aktuelle Konfiguration des angegebenen Formulars
			BorderStyle = TargetForm.FormBorderStyle;
			WindowState = TargetForm.WindowState;
			Bounds = TargetForm.Bounds;
			TopMost = TargetForm.TopMost;
		}

		public void RestoreForm()
		{
			//Konfiguriert das angegebene Formular wie das zuletzt gespeicherte
			if (Maximized == true)
			{
				TargetForm.FormBorderStyle = BorderStyle;
				TargetForm.WindowState = WindowState;
				TargetForm.Bounds = Bounds;
				TargetForm.TopMost = TopMost;
				Maximized = false;
			}
		}

		public void MaximizeForm()
		{
			//Zeigt das angegebene Formular im Vollbildmodus an
			if (Maximized == false)
			{
				Maximized = true;
				SaveFormState();
				TargetForm.WindowState = FormWindowState.Maximized;
				TargetForm.FormBorderStyle = FormBorderStyle.None;
				TargetForm.TopMost = true;
				SetWindowPos(TargetForm.Handle, IntPtr.Zero, 0, 0, GetScreenWidth(), GetScreenHeight(), SWP_SHOWWINDOW);
			}
		}
	}
}

/*
 * Weiterführende Informationen zu SystemMetrics:
 * http://pinvoke.net/default.aspx/Enums.SystemMetric
 * 
*/