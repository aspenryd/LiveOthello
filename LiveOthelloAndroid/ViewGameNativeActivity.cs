using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using System.Threading;
using Android.Content.PM;
using othelloBase;


namespace LiveOthelloAndroid
{
	[Activity (Label = "LiveOthello Settings", ScreenOrientation = ScreenOrientation.Portrait)]		
	public class ViewGameNativeActivity : Activity
	{
		GridView game_grid;
		WebView chat_view;
		LocalStorage localStorage;

		ViewFlipper flippy;
		OthelloBoard board;

		TextView game_positioninfo;

		TextView status_text;

		int intGameId;

		int movenumber;

		DateTime lastUpdate;

		bool haveSeenLastMove;

		System.Timers.Timer _timer;

		MoveList movelist;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.ViewGameNative);

			var gameId = Intent.GetStringExtra("GameId") ?? "2552";
			var gameName = Intent.GetStringExtra("GameName") ?? "";

			var disp = WindowManager.DefaultDisplay;
			int colwidth = (int)Math.Floor((disp.Width < disp.Height ? disp.Width : disp.Height) * 0.9 / 8);
			int gridwidth = colwidth * 8 + 8;// +8 is for the padding

			game_grid = FindViewById<GridView> (Resource.Id.gridview);
			game_grid.LayoutParameters.Width = gridwidth;
			game_grid.LayoutParameters.Height = gridwidth;
			game_grid.Adapter = new ImageAdapter (this, colwidth);

			chat_view = FindViewById<WebView> (Resource.Id.webView2);
			chat_view.Settings.JavaScriptEnabled = true;
			chat_view.LoadUrl ("http://chat.liveothello.com/server/getChat.php?GameID="+gameId);

			chat_view.SetWebViewClient (new MyWebViewClient ());

			var game_name = FindViewById<TextView> (Resource.Id.gameName);
			game_name.Text = gameName;

			status_text = FindViewById<TextView> (Resource.Id.statusText);

			haveSeenLastMove = true;
			game_positioninfo = FindViewById<TextView> (Resource.Id.positionInfo);
			lastUpdate = DateTime.Now;


			var btnMenu = FindViewById<TextView> (Resource.Id.btnMenu);
			btnMenu.Click += (o, e) => {
				this.Finish();
			};

			flippy = FindViewById<ViewFlipper> (Resource.Id.viewFlipper1);
			var btnChange = FindViewById<TextView> (Resource.Id.btnViewChange);
			btnChange.Click += (o, e) => {
				flippy.ShowNext();
				if (btnChange.Text == "View Chat")
					btnChange.Text = "View Game";
				else
					btnChange.Text = "View Chat";
			};
			localStorage = new LocalStorage (this.ApplicationContext);
			board = new OthelloBoard ();
			intGameId = Int32.Parse(gameId);

			GameInfo gameinfo = new GameInfo () { Id = intGameId };
			localStorage.LoadGameInfoFromStorage (gameinfo);
			movelist = new MoveList(gameinfo.Movelist);
			board.BuildBoardFromMoveList (movelist);

			if (!board.GameFinished) 
			{
				UpdateGameInfo (intGameId, movelist);
			}

			movenumber = movelist.List.Count;

			var btnFirst = FindViewById<TextView> (Resource.Id.btnMoveFirst);
			btnFirst.Click += (o, e) => {
				movenumber = 0;
				UpdateGame();
			};

			var btnPrior = FindViewById<TextView> (Resource.Id.btnMovePrior);
			btnPrior.Click += (o, e) => {
				movenumber--;
				if (movenumber < 0 ) movenumber = 0;
				UpdateGame();
			};

			var btnNext = FindViewById<TextView> (Resource.Id.btnMoveNext);
			btnNext.Click += (o, e) => {
				movenumber++;
				if (movenumber > movelist.List.Count() ) movenumber = movelist.List.Count();
				haveSeenLastMove = movenumber == movelist.List.Count();
				UpdateGame();
			};

			var btnLast = FindViewById<TextView> (Resource.Id.btnMoveLast);
			btnLast.Click += (o, e) => {
				haveSeenLastMove = true;
				movenumber = movelist.List.Count();				
				UpdateGame();
			};

			UpdateGame ();
			CreateTimerForUpdates ();
			UpdateStatusText ();
		}

		bool UpdateGameInfo (int gameId, MoveList movelist)
		{
			var info = AndroidConnectivity.GetGameInfo (gameId);

			if (info != null && info.Movelist != null && info.Movelist.ToUpper() != movelist.ToString()) 
			{
				movelist.UpdateList(info.Movelist);
				localStorage.SaveGameInfoToStorage (info);
				lastUpdate = DateTime.Now;
				return true;
			} else 
				return false;
		}

		void UpdateGame ()
		{
			board.BuildBoardFromMoveList(movelist, movenumber);
			(game_grid.Adapter as ImageAdapter).SetBoard(board.Squares);
			game_grid.InvalidateViews();
			game_positioninfo.Text = string.Format ("Move: {0}   Black: {1}   White: {2}", movenumber, board.NumberOfBlackDiscs, board.NumberOfWhiteDiscs);
		}

		private void CreateTimerForUpdates ()
		{
			_timer = new System.Timers.Timer();
			_timer.Interval = 5000; //Trigger event every five seconds
			_timer.Elapsed += OnTimedEvent;
			_timer.Enabled = true;
		}

		bool IsAtLastMove ()
		{
			return movenumber == movelist.List.Count;
		}

		void OnTimedEvent (object sender, System.Timers.ElapsedEventArgs e)
		{
			if (board.GameFinished) {
				RunOnUiThread (() => UpdateStatusText());
				return;
			}

			var isAtLastMove = IsAtLastMove();
			var haveNewMove = UpdateGameInfo (intGameId, movelist);

			if (isAtLastMove && haveNewMove) {
				movenumber = movelist.List.Count ();				
				RunOnUiThread (() => UpdateGame ());
			}
			if (haveNewMove)
				haveSeenLastMove = isAtLastMove;
			RunOnUiThread (() => UpdateStatusText());
		}

		void UpdateStatusText(){
			if (board.GameFinished) {
				status_text.Text = "Game is finished";
				_timer.Enabled = false;
			} else {
				status_text.Text = string.Format ("{0}Last update was made {1} seconds ago", !haveSeenLastMove? "NEW MOVE! " : "",(DateTime.Now - lastUpdate).Seconds);
			}
		}
	}
}

