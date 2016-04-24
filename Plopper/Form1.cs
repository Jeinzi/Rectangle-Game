using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;


/*
 * ToDo:
 * 
 * Add OnLevelEnds event and function
 * Make adaptive
 * Add exceptions
 * Add music
 * Add List : Box
 * ~Remove multi line settings from Inputbox, maybe create new class
 * !Let boxes positioned relativly to the formular constantly adapt to screen size (make adaptive)
 * ~?Add standard username to database
 * #Add in-game stats (current shots, hits, time, points) to time mode
 * Maybe add buttons?
 * Make formular resizeable
 * Upload to github
 * Remove unnecessary using directives
 * 
 */


namespace Plopper
{
	partial class Form1 : Form
	{
		/******** Variables ********/
		private bool showDebugInfo;
		private int fps;
		private int requestedFps;
		private int time;
		private int cumulatedTime;
		private int averageTime;
		private int ticks;
		private string saveDirectory;
		private string screenshotDirectory;

		private FormControl formControl;
		private Gamestate.GameStateManager gameStateManager;
		private Stopwatch fpsStopwatch;
		private Bitmap backBuffer;
		private Graphics backGraphics;
		private Graphics screenGraphics;
		private Thread mainThread;


		/******** Functions ********/
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// Setting size and position of formular
			this.Location = new Point(0, 0);
			this.Size = new Size(1000, 500);
			this.WindowState = FormWindowState.Maximized;

			formControl = new FormControl(this);
			formControl.MaximizeForm();

			// Initializing variables
			saveDirectory = Environment.GetEnvironmentVariable("USERPROFILE")
									 + @"\AppData\Roaming\Jeinzi\Plopper\";
			screenshotDirectory = saveDirectory + @"screenshots\";
			showDebugInfo = false;
			requestedFps = 30;
			gameStateManager = new Gamestate.GameStateManager();
			fpsStopwatch = new Stopwatch();
			backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			backGraphics = Graphics.FromImage(backBuffer);
			screenGraphics = this.CreateGraphics();

			// Create directory structure and database
			CreateDirectories();
			Database.Connect(saveDirectory);

			// Starting threads
			mainThread = new Thread(GameLoop);
			mainThread.IsBackground = true;
			mainThread.Start();
		}


		/// <summary>
		/// An infinite loop updating and rendering the game.
		/// </summary>
		private void GameLoop()
		{
			while (true)
			{
				fpsStopwatch.Reset();
				fpsStopwatch.Start();

				// Updating und drawing game
				gameStateManager.Update();
				Render(backGraphics);
				Display();

				// FPS regulation
				fpsStopwatch.Stop();

				time = (int)fpsStopwatch.ElapsedMilliseconds;
				cumulatedTime += time;
				ticks++;
				averageTime = cumulatedTime / ticks;

				int requestedTimePerFrame = (int)(1.0 / (double)requestedFps * 1000);
				int sleepTime = (int)(requestedTimePerFrame - fpsStopwatch.ElapsedMilliseconds);
				if (sleepTime < 0)
				{
					sleepTime = 0;
				}
				Thread.Sleep(sleepTime);
				fps = (int)(1000 / (sleepTime + fpsStopwatch.ElapsedMilliseconds));
			}
		}


		/// <summary>
		/// Renders the current scenery to the given graphics object.
		/// </summary>
		private void Render(Graphics g)
		{
			lock (backBuffer)
			{
				gameStateManager.Draw(g);

				if (showDebugInfo)
				{
					g.DrawString("FPS: " + fps + " / " + requestedFps, new Font("ARIAL", 12), Brushes.Black, 10, 10);
				}
			}
		}

		/// <summary>
		/// Shows the current backbuffer on the sceen.
		/// </summary>
		private void Display()
		{
			try
			{
				lock (backBuffer)
				{
					screenGraphics.DrawImage(backBuffer, 0, 0);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Saves the current backbuffer to the screenshot save directory.
		/// </summary>
		private void SaveScreenshot()
		{
			int i = 0;
			string savePath = "";

			// Search for an unused file name
			do
			{
				savePath = screenshotDirectory + "Screenshot_" + i++ + ".jpg";
			}
			while (System.IO.File.Exists(savePath));
			// Save file
			lock (backBuffer)
			{
				backBuffer.Save(savePath);
			}
		}

		/// <summary>
		/// Creates all the needed directories.
		/// </summary>
		private void CreateDirectories()
		{
			System.IO.Directory.CreateDirectory(screenshotDirectory);
		}


		/******** Keyboard input ********/
		// All the user input is directly passed to the gamestate manager,
		// which then passes it to the currently active gamestate

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			// Toggle debug info, can always be activated
			if (e.KeyCode == Keys.F1) { showDebugInfo = !showDebugInfo; }
			// Toggle fullscreen
			if (e.KeyCode == Keys.F2) { formControl.MaximizeForm(); }
			// Screenshot
			if (e.KeyCode == Keys.F3) { SaveScreenshot(); }

			gameStateManager.KeyPressed(e);
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e) { gameStateManager.KeyReleased(e); }
		private void Form1_KeyPress(object sender, KeyPressEventArgs e) { gameStateManager.CharPressed(e); }
		private void Form1_MouseDown(object sender, MouseEventArgs e) { gameStateManager.MousePressed(e); }
		private void Form1_MouseUp(object sender, MouseEventArgs e) { gameStateManager.MouseReleased(e); }
		private void Form1_MouseMove(object sender, MouseEventArgs e) { gameStateManager.MouseMoved(e); }
	}
}