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
	[Activity (Label = "LiveOthello NativeGame", ScreenOrientation = ScreenOrientation.Portrait)]		
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

		string black_string;
		string white_string;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.ViewGameNative);
			black_string = Resources.GetString (Resource.String.black);
			white_string = Resources.GetString (Resource.String.white);

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
			string view_chat = Resources.GetString (Resource.String.view_chat);
			string view_game = Resources.GetString (Resource.String.view_game);

			btnChange.Click += (o, e) => {
				flippy.ShowNext();
				if (btnChange.Text == view_chat)
					btnChange.Text = view_game;
				else
					btnChange.Text = view_chat;
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

			game_positioninfo.Text = string.Format ("({0}) {1} {2} {3}", movenumber, black_string, GetScore(board), white_string);
		}

		string GetScore (OthelloBoard board)
		{
			if (board.GameFinished)
				return board.FinalScore;
			return string.Format ("{0} - {1}", board.NumberOfBlackDiscs, board.NumberOfWhiteDiscs);
		}

		private void CreateTimerForUpdates ()
		{
			_timer = new System.Timers.Timer();
			_timer.Interval = 1000; //Trigger event every seconds
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

			if (ShouldLookForUpdate())
			{
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
		}

		bool ShouldLookForUpdate ()
		{
			//1,2,4,7,10,15,20...
			var spann = DateTime.Now - lastUpdate;
			var sec = Math.Floor(spann.TotalSeconds);
			if (sec <= 2)
				return true; //Update every second
			if (sec < 10)
				return sec == 4 ||	sec == 7 ; //Update every 3 seconds
			return sec % 5 == 0; //Update every 5 seconds
		}

		void UpdateStatusText(){

			if (board.GameFinished) {
				status_text.Text = Resources.GetString (Resource.String.game_finished);
				_timer.Enabled = false;
			} else if (board.NumberOfMoves == 0) {
				status_text.Text = Resources.GetString (Resource.String.game_waiting_start);
			} else
			{
				var next_color = board.NextColor == SquareType.Black ? black_string : white_string;
				var live_status_string = Resources.GetString (Resource.String.live_status);
				var new_move = !haveSeenLastMove? Resources.GetString (Resource.String.new_move) + " " : "";
				status_text.Text = string.Format (live_status_string, new_move, next_color, TimeSpanToString((DateTime.Now - lastUpdate)));
			}
		}

		public string TimeSpanToString (TimeSpan timeSpan)
		{
			var text = "";
			if (timeSpan.Minutes > 0)
				text = timeSpan.Minutes + "m ";
			text += timeSpan.Seconds + "s";
			return text;
		}
	}
}

