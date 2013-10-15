using System;
using Android.Preferences;
using Android.Content;
using othelloBase;
using System.Collections.Generic;
using System.Linq;

namespace test
{
	public class LocalStorage
	{

		const string MyPrefKey = "LiveOthelloPrefKey";
	
		public bool LoadTournamentsFromStorage(Context context, out IList<Tournament> tournaments)
		{
			tournaments = new List<Tournament> ();
			try
			{
				ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
				var strTournaments = prefs.GetString("Tournaments", "");

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
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			ISharedPreferencesEditor editor = prefs.Edit();
			var strTournaments = "";
			foreach (var tournament in tournaments) 
			{
				strTournaments += tournament.ToStorageString();
			}
			editor.PutString("Tournaments", strTournaments);
			editor.Commit();
		}

		public bool LoadGamesFromStorage(Context context, Tournament tournament)
		{
			try{
				if (tournament.Games != null && tournament.Games.Any ())
					return true;
				ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
				var strGames = prefs.GetString("GamesFromTournament"+tournament.Id.ToString(), "");
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

		public void SaveGamesToStorage(Context context, Tournament tournament)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context); 
			ISharedPreferencesEditor editor = prefs.Edit();
			var strTournaments = "";
			foreach (var game in tournament.Games) 
			{
				strTournaments += game.ToStorageString();
			}
			editor.PutString("GamesFromTournament"+tournament.Id.ToString(), strTournaments);
			editor.Commit();
		}
	}
}

