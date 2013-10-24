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
using Android.Support.V4.App;
using Java.Lang;
using Android.Media;


namespace test
{
	[Activity (Label = "LiveOthello", Icon = "@drawable/logo", MainLauncher = true)]
	public class MainActivity : Activity
	{
		#region fields
		private static readonly int NewGameNotificationId = 2000;
		private static readonly int NewTournamentNotificationId = 1000;
		private const int menuItemInfo = 0;
		private const int menuItemSettings = 1;
		private const int menuItemUpdate = 2;
		private DateTime LastUpdateCheck;
		private int minTimeBetweenUpdates = 60;  //seconds

		System.Timers.Timer _timer;
		IList<Tournament> tournaments = null;
		IList<Game> games;

		#endregion


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

//			var btnSettings = FindViewById<Button> (Resource.Id.btnSettings);
//			btnSettings.Click += (sender, e) => {
//				AddTournament();
//				AddGame();
//			};

			ThreadPool.QueueUserWorkItem (o => UpdateTournamentsFromSite ());

			//CreateTimerForUpdates ();
		}

//		int tournamentNumber = 0;
//
//		void AddTournament ()
//		{
//			tournamentNumber++;
//			NotifyNewTournament(new Tournament() {Id = tournamentNumber, Name = string.Format("Fake Tournament {0}", tournamentNumber)});
//		}
//
//		int gameNumber = 0;
//
//		void AddGame ()
//		{
//			gameNumber++;
//			NotifyNewGame (new Game () { Id = gameNumber, Name = string.Format ("Fake Game {0}", gameNumber) });
//		}


		#region public methods
		public bool IsConnected {
			get {
				var connectivityManager = (ConnectivityManager)GetSystemService (ConnectivityService);
				var activeConnection = connectivityManager.ActiveNetworkInfo;
				return  (activeConnection != null) && activeConnection.IsConnected;
			}
		}


		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			//var menuitemInfo = menu.Add(0,menuItemInfo,0,"Info");
			//menuitemInfo.SetIcon(Resource.Drawable.menu_info);
			var menuitemSettings = menu.Add(0,menuItemSettings,1,"Settings");
			menuitemSettings.SetIcon(Resource.Drawable.menu_settings);
			menu.Add(0,menuItemUpdate,2,"Check for new games").SetIcon(Resource.Drawable.menu_update);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case menuItemInfo: 
				{
					ViewInfo ();
					return true;
				}
			case menuItemSettings: 
				{
					ViewSettings ();
					return true;
				}
			case menuItemUpdate: 
				{
					UpdateTournamentListAndTournamentsThatHaveGames ();
					return true;
				}
				default:
				return base.OnOptionsItemSelected(item);
			}
		}
		#endregion

		#region Properties

		Tournament CurrentTournament {
			get
			{ 
				var spinner = FindViewById<Spinner> (Resource.Id.tournamentspinner);
				return tournaments.ToList()[spinner.SelectedItemPosition];
			}

		}

		LocalStorage localStorage {
			get
			{ 
				return new LocalStorage (this.ApplicationContext);
			}
		}

		#endregion


		#region protected methods
		protected void UpdateTournamentsFromSite ()
		{
			if (HasNewTournaments())
				RunOnUiThread (() => SaveTournamentsAndUpdateTournamentSpinner());
		}

		protected bool HasNewTournaments()
		{
			if (!IsConnected)
				return false;
			var newtournaments = AndroidConnectivity.GetTournaments ();
			foreach (var tournament in newtournaments) 
			{
				if (!tournaments.Any (t=>t.Id == tournament.Id)) 
				{
					NotifyNewTournament (tournament);
					tournaments = newtournaments;
					return true;
				}
			}
			return false;
		}


		protected void SaveTournamentsAndUpdateTournamentSpinner ()
		{

			localStorage.SaveTournamentsToStorage (this.ApplicationContext, tournaments);
			UpdateTournamentSpinner ();
		}


		protected IEnumerable<Tournament> GetTournaments()
		{
			if (tournaments == null) 
			{
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

		protected IEnumerable<Game> GetGamesFromTournament(Tournament tournament)
		{
			if (tournament.Games == null) 
			{
				if (!localStorage.LoadGamesFromStorage (tournament)) 
				{
					if (IsConnected) 
					{
						tournament.Games = AndroidConnectivity.GetGamesFromTournament (tournament.Id);
						localStorage.SaveGamesToStorage (tournament);
					}
				}
			}
			return tournament.Games;
		}

		protected void UpdateGamesFromSite (Tournament tournament, bool updateSpinner)
		{
			if (HasNewGames(tournament))
				RunOnUiThread (() => SaveGamesAndUpdateGameSpinner(tournament, updateSpinner));
		}

		protected bool HasNewGames(Tournament tournament)
		{
			if (!IsConnected)
				return false;
			var newgames = AndroidConnectivity.GetGamesFromTournament (tournament.Id).ToList ();
			foreach (var game in newgames) 
			{
				if (!tournament.Games.Any (g=>g.Id == game.Id)) 
				{
					NotifyNewGame (game);
					tournament.Games = newgames;
					return true;
				}
			}
			return false;

		}

		protected void SaveGamesAndUpdateGameSpinner (Tournament tournament, bool updateSpinner)
		{
			localStorage.SaveGamesToStorage (tournament);
			if (updateSpinner)
				UpdateGameSpinner (tournament.Games);
		}

		#endregion

		#region events

		private void tournamentspinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var tournament = tournaments.ToList()[e.Position];
			games = GetGamesFromTournament (tournament).ToList();
			UpdateGameSpinner (games);
			ThreadPool.QueueUserWorkItem (o => UpdateGamesFromSite (tournament, true));
		}

		void OnTimedEvent (object sender, System.Timers.ElapsedEventArgs e)
		{
			if ((DateTime.Now - LastUpdateCheck).Seconds > minTimeBetweenUpdates) 
			{
				ThreadPool.QueueUserWorkItem (o => UpdateTournamentListAndTournamentsThatHaveGames ());
				LastUpdateCheck = DateTime.Now;
			}
		}


		#endregion 

		#region UI Methods
	

		protected void ViewGame()
		{
			Spinner gamesspinner = FindViewById<Spinner> (Resource.Id.gamesspinner);
			if (gamesspinner.Count > 0 && gamesspinner.SelectedItemPosition >= 0) {
				var game = games [gamesspinner.SelectedItemPosition];

				NotifyNewGame (game);

				var second = new Intent (this, typeof(ViewGameActivity));
				second.PutExtra ("GameName", game.Name);
				second.PutExtra ("GameId", game.Id.ToString ());
				StartActivity (second);	
			} 
		}

		void ViewInfo ()
		{
			//throw new NotImplementedException ();
		}

		void ViewSettings ()
		{
			var settings = new Intent (this, typeof(SettingsActivity));
			StartActivity (settings);
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

		protected void UpdateGameSpinner (IEnumerable<Game> newgames)
		{
			Spinner gamesspinner = FindViewById<Spinner> (Resource.Id.gamesspinner);
			FillSpinner (gamesspinner, newgames);
			games = newgames.ToList();
			var gamebutton = FindViewById<Button> (Resource.Id.btnGame);
			gamebutton.Enabled = newgames.Count() > 0;
		}

		protected void FillSpinner(Spinner spinner, IEnumerable<Game> items)
		{
			var adapter = new ArrayAdapter<Game>(this, Android.Resource.Layout.SimpleSpinnerItem, items.ToList());
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
		}

		#endregion

		#region private methods
		private void CreateTimerForUpdates ()
		{
			_timer = new System.Timers.Timer();
			_timer.Interval = 1000; //Trigger event every second
			_timer.Elapsed += OnTimedEvent;
			_timer.Enabled = true;
		}

		private void UpdateTournamentListAndTournamentsThatHaveGames ()
		{
			UpdateTournamentsFromSite ();
			foreach (var tournament in tournaments.Where(t=>t.Games != null && t.Games.Any())) 
			{
				UpdateGamesFromSite (tournament, CurrentTournament.Id == tournament.Id);
			}
		}

		#endregion

		#region Notifications

		protected void NotifyNewGame(Game game)
		{
			int id = NewGameNotificationId;
			switch (localStorage.GamesNotificationType) 
			{
				case NotificationType.None:
					return;
				case NotificationType.One:
					id = NewGameNotificationId;
					break;
				case NotificationType.Every:
					id = NewGameNotificationId + game.Id;
					break;
			}
			CreateNotification ("New LiveOthello game", game.Name, Resource.Drawable.game_small, id, game);
		}

		protected void NotifyNewTournament(Tournament tournament)
		{
			int id = NewTournamentNotificationId;
			switch (localStorage.TournamentNotificationType) 
			{
				case NotificationType.None:
				return;
				case NotificationType.One:
				id = NewTournamentNotificationId;
				break;
				case NotificationType.Every:
				id = NewTournamentNotificationId + tournament.Id;
				break;
			}
			CreateNotification ("New LiveOthello tournament", tournament.Name, Resource.Drawable.tournament_small, id);
		}

		protected void CreateNotification(string title, string text, int icon, int notificationId, Game game = null)
		{
			Intent resultIntent = new Intent(this, typeof(MainActivity));

			if (game != null) {
				resultIntent = new Intent(this, typeof(ViewGameActivity));
				resultIntent.PutExtra ("GameName", game.Name);
				resultIntent.PutExtra ("GameId", game.Id.ToString ());
			}

			var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
			stackBuilder.AddParentStack(Class.FromType(typeof(MainActivity)));
			stackBuilder.AddNextIntent(resultIntent);

			PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

			var builder = new Android.Support.V4.App.NotificationCompat.Builder(this)
					.SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
					.SetContentIntent(resultPendingIntent) // start up this activity when the user clicks the intent.
					.SetContentTitle(title)
					.SetSmallIcon(icon)
					.SetContentText(text);

			if (localStorage.NotificationSound) {
				Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri (RingtoneType.Notification);
				builder.SetSound (alarmSound);
			}

			if (localStorage.NotificationVibration) {
				long[] pattern = {500,500};
				builder.SetVibrate(pattern);
			}

			if (localStorage.NotificationLight) {
				int red = 0x0000ff;
				builder.SetLights(red, 500, 500);
			}

			// Obtain a reference to the NotificationManager
			var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
			notificationManager.Notify(notificationId, builder.Build());
		}

		#endregion
	}
}




