using System.Media;

namespace Layout
{
	/// <summary>
	/// Class used to play short sound effects.
	/// No object can be instantiated, only the static Play-method can be called for a certain sound.
	/// </summary>
	public abstract class SFXPlayer
	{
		/// <summary>
		/// Plays a RectangleGame.Sound.
		/// </summary>
		/// <param name="sound">A RectangleGame.Sound object.</param>
		public static void Play(Sound sound)
		{
			SoundPlayer soundPlayer = new SoundPlayer();
#warning Properties is not independent of the program the libary is used in
			switch(sound)
			{
				case Sound.Menu_Toggle:
					soundPlayer.Stream = RectangleGame.Properties.Resources.toggle;
					break;
				case Sound.Menu_Confirm:
					soundPlayer.Stream = RectangleGame.Properties.Resources.select;
					break;
				case Sound.Plop1:
					soundPlayer.Stream = RectangleGame.Properties.Resources.plop1;
					break;
				case Sound.Plop2:
					soundPlayer.Stream = RectangleGame.Properties.Resources.plop2;
					break;
				case Sound.Plop3:
					soundPlayer.Stream = RectangleGame.Properties.Resources.plop3;
					break;
				case Sound.Plop4:
					soundPlayer.Stream = RectangleGame.Properties.Resources.plop4;
					break;
				case Sound.Plop5:
					soundPlayer.Stream = RectangleGame.Properties.Resources.plop5;
					break;
				case Sound.Plop6:
					soundPlayer.Stream = RectangleGame.Properties.Resources.plop6;
					break;
				case Sound.Click1:
					soundPlayer.Stream = RectangleGame.Properties.Resources.click1;
					break;
				case Sound.Click2:
					soundPlayer.Stream = RectangleGame.Properties.Resources.click2;
					break;
				case Sound.Click3:
					soundPlayer.Stream = RectangleGame.Properties.Resources.click3;
					break;
				case Sound.Click4:
					soundPlayer.Stream = RectangleGame.Properties.Resources.click4;
					break;
				case Sound.Death:
					soundPlayer.Stream = RectangleGame.Properties.Resources.death;
					break;
				case Sound.Hit:
					soundPlayer.Stream = RectangleGame.Properties.Resources.hit;
					break;
				default:
					return;
			}

			soundPlayer.Play();
		}
	}

	/// <summary>
	/// Enumerator encapsulating the available sounds into a distict datatype.
	/// </summary>
	public enum Sound
	{
		Menu_Toggle,
		Menu_Confirm,
		Plop1,
		Plop2,
		Plop3,
		Plop4,
		Plop5,
		Plop6,
		Click1,
		Click2,
		Click3,
		Click4,
		Death,
		Hit
	}
}
