using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace RectangleGame
{
	/// <summary>
	/// Class controlling form properties.
	/// </summary>
	public class FormControl
	{
		/**** Variables ****/

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

		/**** Functions ****/

		/// <summary>
		/// Constructs the controller for a given form.
		/// </summary>
		/// <param name="TargetForm">The form to be controled.</param>
		public FormControl(Form TargetForm)
		{
			this.TargetForm = TargetForm;
		}

		// Including and defining methods necessary to interact with the form.
		[DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
		private static extern int GetSystemMetrics(int smIndex);
		[DllImport("user32.dll")]
		private static extern void SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int width, int height, uint flags);


		/// <summary>
		/// Returns the width of the main screen in pixels.
		/// </summary>
		/// <returns>Screenwidth in pixels.</returns>
		public static int GetScreenWidth()
		{
			return GetSystemMetrics(SM_SCREENX);
		}

		/// <summary>
		/// Returns the height of the main screen in pixels.
		/// </summary>
		/// <returns>Screenheight in pixels.</returns>
		public static int GetScreenHeight()
		{
			return GetSystemMetrics(SM_SCREENY);
		}

		/// <summary>
		/// Saves the current configuration of the given form.
		/// </summary>
		private void SaveFormState()
		{
			BorderStyle = TargetForm.FormBorderStyle;
			WindowState = TargetForm.WindowState;
			Bounds = TargetForm.Bounds;
			TopMost = TargetForm.TopMost;
		}

		/// <summary>
		/// Configures the given form like the previously saved one.
		/// </summary>
		public void RestoreForm()
		{
			if (Maximized == true)
			{
				TargetForm.FormBorderStyle = BorderStyle;
				TargetForm.WindowState = WindowState;
				TargetForm.Bounds = Bounds;
				TargetForm.TopMost = TopMost;
				Maximized = false;
			}
		}

		/// <summary>
		/// Activates fullscreen mode for the given form.
		/// </summary>
		public void MaximizeForm()
		{
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
 * Additional info regarding SystemMetrics can be found at:
 * http://pinvoke.net/default.aspx/Enums.SystemMetric
 * 
*/