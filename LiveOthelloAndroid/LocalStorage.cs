using System;
using Android.Preferences;
using Android.Content;
using othelloBase;
using System.Collections.Generic;
using System.Linq;

namespace test
{
	public enum NotificationType
	{
		None = 0,
		One = 1,
		Every = 2
	}

	public class LocalStorage
	{
		const string parameterNotificationSound = "NotificationSound";
		const string parameterNotificationVibration = "NotificationVibration";
		const string parameterNotificationLight = "NotificationLight";
		const string parameterTournamentNotificationType = "NotificationTournamentType";
		const string parameterGamesNotificationType = "NotificationGamesType";

		private bool notificationSound;
		private bool notificationVibration;
		private bool notificationLight;

		NotificationType tournamentNotificationType;
		NotificationType gamesNotificationType;

		Context context;

		public LocalStorage(Context _context)
		{
			context = _context;
			notificationSound = GetPrefs (parameterNotificationSound, true);
			notificationVibration = GetPrefs (parameterNotificationVibration, true);
			notificationLight = GetPrefs (parameterNotificationLight, true);
			tournamentNotificationType = (NotificationType)GetPrefs (parameterTournamentNotificationType, (int)NotificationType.Every);
			gamesNotificationType = (NotificationType)GetPrefs (parameterGamesNotificationType, (int)NotificationType.Every);
		}

		public bool NotificationSound { 
			get {return notificationSound;}
			set 
			{
				notificationSound = value;
				SetPrefs (parameterNotificationSound, notificationSound);
			}
		}

		public bool NotificationVibration { 
			get {return notificationVibration;}
			set 
			{
				notificationVibration = value;
				SetPrefs (parameterNotificationVibration, notificationVibration);
			}
		}

		public bool NotificationLight { 
			get {return notificationLight;}
			set 
			{
				notificationLight = value;
				SetPrefs (parameterNotificationLight, notificationLight);
			}
		}

		public NotificationType TournamentNotificationType {
			get {return tournamentNotificationType;}
			set 
			{
				tournamentNotificationType = value;
				SetPrefs (parameterTournamentNotificationType, (int)tournamentNotificationType);
			}
		}

		public NotificationType GamesNotificationType {
			get {return gamesNotificationType;}
			set 
			{
				gamesNotificationType = value;
				SetPrefs (parameterGamesNotificationType, (int)gamesNotificationType);
			}
		}

		public bool LoadTournamentsFromStorage(Context context, out IList<Tournament> tournaments)
		{
			tournaments = new List<Tournament> ();
			try
			{
				var strTournaments = GetPrefs("Tournaments", "");
				foreach (var strTournament in strTournaments.Split(';')) 
				{
					var tournament = Tournament.ParseFromString (strTournament);
					if(tournament != null && !tournaments.Any(t=>t.Id == tournament.Id))
						tournaments.Add(tournament);
				}
				return tournaments.Any();
			}
			catch{
				return false;
			}
		}

		public void SaveTournamentsToStorage(Context context, IList<Tournament> tournaments)
		{
			var strTournaments = "";
			foreach (var tournament in tournaments) 
			{
				strTournaments += tournament.ToStorageString();
			}
			SetPrefs("Tournaments", strTournaments);
		}

		public bool LoadGamesFromStorage(Tournament tournament)
		{
			try{
				if (tournament.Games != null && tournament.Games.Any ())
					return true;
				var strGames = GetPrefs("GamesFromTournament"+tournament.Id.ToString(), "");
				var games = new List<Game> ();
				foreach (var strGame in strGames.Split(';')) 
				{
					var game = Game.ParseFromString (strGame);
					if(game != null && !games.Any(g=>g.Id == game.Id))
						games.Add(game);
				}
				tournament.Games = games;
				return games.Any();
			}
			catch{
				return false;
			}
		}

		public void SaveGamesToStorage(Tournament tournament)
		{
			var strTournaments = "";
			foreach (var game in tournament.Games) 
			{
				strTournaments += game.ToStorageString();
			}
			SetPrefs ("GamesFromTournament" + tournament.Id.ToString (), strTournaments);
		}



		private void SetPrefs (string parameter, string value)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString(parameter, value);
			editor.Commit();
		}

		private string GetPrefs (string parameter, string _default)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			return prefs.GetString(parameter, _default);
		}

		private void SetPrefs (string parameter, bool value)
		{
			SetPrefs (parameter, value.ToString ());
		}

		private bool GetPrefs (string parameter, bool _default)
		{
			try 
			{
				return Boolean.Parse(GetPrefs(parameter, _default.ToString()));
			}
			catch
			{
				return _default;
			}
		}

		private void SetPrefs (string parameter, int value)
		{
			SetPrefs (parameter, value.ToString ());
		}

		private int GetPrefs (string parameter, int _default)
		{
			try 
			{
				return Int32.Parse(GetPrefs(parameter, _default.ToString()));
			}
			catch
			{
				return _default;
			}
		}
	}
}

