using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Webkit;
using othelloBase;
using System.Linq;
using System.Threading;
using Android.Net;

namespace test
{
	[Activity (Label = "LiveOthello", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public bool IsConnected {
			get {
				var connectivityManager = (ConnectivityManager)GetSystemService (ConnectivityService);
				var activeConnection = connectivityManager.ActiveNetworkInfo;
				return  (activeConnection != null) && activeConnection.IsConnected;
			}
		}

		IList<Tournament> tournaments = null;
		IList<Game> games;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			var tournaments = GetTournaments ();
			UpdateTournamentSpinner();

			var btnShowGame = FindViewById<Button> (Resource.Id.btnGame);
			btnShowGame.Click += (sender, e) => {
				ViewGame();
			};

			ThreadPool.QueueUserWorkItem (o => UpdateTournamentsFromSite ());
		}

		protected void UpdateTournamentSpinner ()
		{
			Spinner spinner = FindViewById<Spinner> (Resource.Id.tournamentspinner);
			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (tournamentspinner_ItemSelected);
			FillSpinner (spinner, tournaments);
		}

		protected void FillSpinner(Spinner spinner, IEnumerable<Tournament> items)
		{
			var adapter = new ArrayAdapter<Tournament>(this, Android.Resource.Layout.SimpleSpinnerItem, items.ToList());
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
		}

		protected void FillSpinner(Spinner spinner, IEnumerable<Game> items)
		{
			var adapter = new ArrayAdapter<Game>(this, Android.Resource.Layout.SimpleSpinnerItem, items.ToList());
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
		}

		protected void UpdateTournamentsFromSite ()
		{
			if (HasNewTournaments())
				RunOnUiThread (() => SaveTournamentsAndUpdateTournamentSpinner());
		}

		protected void SaveTournamentsAndUpdateTournamentSpinner ()
		{
			var localStorage = new LocalStorage ();
			localStorage.SaveTournamentsToStorage (this.ApplicationContext, tournaments);
			UpdateTournamentSpinner ();
		}


		protected IEnumerable<Tournament> GetTournaments()
		{
			if (tournaments == null) 
			{
				var localStorage = new LocalStorage ();
				if (!localStorage.LoadTournamentsFromStorage (this.ApplicationContext, out tournaments)) 
				{
					if (IsConnected) {
						tournaments = AndroidConnectivity.GetTournaments ();
						localStorage.SaveTournamentsToStorage (this.ApplicationContext, tournaments);
					}
				}
			}
			return tournaments;
		}


		protected bool HasNewTournaments()
		{
			if (!IsConnected)
				return false;
			var newtournaments = AndroidConnectivity.GetTournaments ();
			if (newtournaments.Count() != tournaments.Count())
			{
				tournaments = newtournaments;
				return true;
			} else 
			{
				return false;
			}
		}

		protected IEnumerable<Game> GetGamesFromTournament(Tournament tournament)
		{
			if (tournament.Games == null) 
			{
				var localStorage = new LocalStorage ();
				if (!localStorage.LoadGamesFromStorage (this.ApplicationContext, tournament)) 
				{
					if (IsConnected) 
					{
						tournament.Games = AndroidConnectivity.GetGamesFromTournament (tournament.Id);
						localStorage.SaveGamesToStorage (this.ApplicationContext, tournament);
					}
				}
			}
			return tournament.Games;
		}

		protected bool HasNewGames(Tournament tournament)
		{
			if (!IsConnected)
				return false;
			var newgames = AndroidConnectivity.GetGamesFromTournament (tournament.Id).ToList ();
			if (newgames.Count() != tournament.Games.Count())
			{
				tournament.Games = newgames;
				return true;
			} else 
			{
				return false;
			}
		}

		protected void tournamentspinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var tournament = tournaments.ToList()[e.Position];
			games = GetGamesFromTournament (tournament).ToList();
			UpdateGameSpinner (games);
			ThreadPool.QueueUserWorkItem (o => UpdateGamesFromSite (tournament));
		}

		protected void UpdateGamesFromSite (Tournament tournament)
		{
			if (HasNewGames(tournament))
				RunOnUiThread (() => SaveGamesAndUpdateGameSpinner(tournament));
		}

		protected void SaveGamesAndUpdateGameSpinner (Tournament tournament)
		{
			var localStorage = new LocalStorage ();
			localStorage.SaveGamesToStorage (this.ApplicationContext, tournament);
			UpdateGameSpinner (tournament.Games);
		}

		protected void UpdateGameSpinner (IEnumerable<Game> games)
		{
			Spinner gamesspinner = FindViewById<Spinner> (Resource.Id.gamesspinner);
			FillSpinner (gamesspinner, games);
		}

		protected void ViewGame()
		{
			Spinner gamesspinner = FindViewById<Spinner> (Resource.Id.gamesspinner);
			if (gamesspinner.Count > 0 && gamesspinner.SelectedItemPosition >= 0) {
				var game = games [gamesspinner.SelectedItemPosition];

				var second = new Intent (this, typeof(ViewGameActivity));
				second.PutExtra ("GameName", game.Name);
				second.PutExtra ("GameId", game.Id.ToString ());
				StartActivity (second);	
			} 

		}
	}
}


