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

namespace test
{
	[Activity (Label = "LiveOthello", MainLauncher = true)]
	public class MainActivity : Activity
	{

		IList<Tournament> tournaments = null;
		IList<Game> games;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			Spinner spinner = FindViewById<Spinner> (Resource.Id.tournamentspinner);
			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (tournamentspinner_ItemSelected);

			var tournaments = GetTournaments ();
			FillSpinner (spinner, tournaments);

			var showSecond = FindViewById<Button> (Resource.Id.btnGame);
			showSecond.Click += (sender, e) => {
				ViewGame();
			};
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

		protected IEnumerable<Tournament> GetTournaments()
		{
			if (tournaments == null)
				tournaments = new LiveOthelloService ().GetTournaments ().ToList();
			return tournaments;
		}

		protected IEnumerable<Game> GetGamesFromTournament(Tournament tournament)
		{
			if (tournament.Games == null)
				tournament.Games = new LiveOthelloService ().GetGamesFromTournament (tournament.Id);
			return tournament.Games;
		}

		protected void tournamentspinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var tournament = tournaments.ToList()[e.Position];
			games = GetGamesFromTournament (tournament).ToList();
			Spinner gamesspinner = FindViewById<Spinner> (Resource.Id.gamesspinner);
			FillSpinner (gamesspinner, games);
		}

		protected void ViewGame()
		{
			Spinner gamesspinner = FindViewById<Spinner> (Resource.Id.gamesspinner);
			var game = games [gamesspinner.SelectedItemPosition];

			var second = new Intent(this, typeof(ViewGameActivity));
			second.PutExtra("GameName", game.Name);
			second.PutExtra("GameId", game.Id.ToString());
			StartActivity(second);	
		}
	}
}


