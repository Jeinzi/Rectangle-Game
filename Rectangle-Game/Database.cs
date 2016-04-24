using System;
using System.Data.SQLite;
using System.IO;

namespace RectangleGame
{
	/// <summary>
	/// A wrapper around SQLite simplyfiing its use and managing the database of the game.
	/// </summary>
	public abstract class Database
	{
		/******** Variables ********/
		private static bool connected = false;
		private static SQLiteConnection databaseConnection;
		private static string saveDirectory;
		private static string databasePath;


		/******** Functions ********/

		/// <summary>
		/// Creates a new SQLite Database within the given directory, in case there is none yet.
		/// </summary>
		private static void Create()
		{
			// Create SQLite database file
			Directory.CreateDirectory(saveDirectory);
			SQLiteConnection.CreateFile(databasePath);

			// Setting up database connection
			Connect();

			// Creating tables
			Query("create table timeState (name varchar(50), score int)");
			Query("create table rushState (name varchar(50), score int)");

			// Closing the connection to the database
			Disconnect();
		}


		/// <summary>
		/// Connects to the database.
		/// If there is no database, it will be created first, the connection will then be established subsequently.
		/// </summary>
		/// <param name="saveDirectory">The path the database is saved in. When set to null, the last database will be used.</param>
		public static void Connect(string saveDirectory = null)
		{
			// Is there even a file in the right place?
			if (saveDirectory != null)
			{
				Database.saveDirectory = saveDirectory;
				databasePath = saveDirectory + "database.sqlite";
			}

			// Connecting or creating to database
			if(File.Exists(databasePath))
			{
				databaseConnection = new SQLiteConnection("Data Source=" + databasePath + ";Version=3;");
				databaseConnection.Open();
				connected = true;
			}
			else
			{
				Create();
				Connect();
			}
		}

		
		/// <summary>
		/// Closes the connection to the database and disposes all used objects.
		/// </summary>
		public static void Disconnect()
		{
			if (!connected)
			{
				return;
			}
			else
			{
				// Closing connection and manually dispoing all related objects,
				// because it didn't work automatically... for reasons
				databaseConnection.Close();
				databaseConnection.Dispose();

				// Forcing garbage collection to free database
				GC.Collect();
				// Developer gave database a garbage collection... database.. is... freeeeee!
				connected = false;
			}
		}


		/// <summary>
		/// Deletes the database and creates it new from scratch.
		/// </summary>
		public static void Clear()
		{
			if (!connected) return;

			Disconnect();
			File.Delete(databasePath);
			Create();
			Connect();
		}


		/// <summary>
		/// Sends a query to the database. No value will be returned.
		/// </summary>
		/// <param name="query">A query that hasn't got any type of return value.</param>
		public static void Query(string query)
		{
			SQLiteCommand command = new SQLiteCommand(query, databaseConnection);
			command.ExecuteNonQuery();
			command.Dispose();
		}

		/// <summary>
		/// Sends a query to the database and returns a SQLiteReader object.
		/// </summary>
		/// <param name="query">A query producing a return value.</param>
		public static SQLiteDataReader GetReader(string query)
		{
			SQLiteCommand command = new SQLiteCommand(query, databaseConnection);
			return(command.ExecuteReader());
		}
	}
}