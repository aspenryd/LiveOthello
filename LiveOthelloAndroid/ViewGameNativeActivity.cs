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

		int movenumber;

		MoveList movelist;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.ViewGameNative);

			var gameId = Intent.GetStringExtra("GameId") ?? "2552";

			game_grid = FindViewById<GridView> (Resource.Id.gridview);
			game_grid.Adapter = new ImageAdapter (this);

			chat_view = FindViewById<WebView> (Resource.Id.webView2);
			chat_view.Settings.JavaScriptEnabled = true;
			chat_view.LoadUrl ("http://chat.liveothello.com/server/getChat.php?GameID="+gameId);

			chat_view.SetWebViewClient (new MyWebViewClient ());


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
			var intGameId = Int32.Parse(gameId);

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
				UpdateGame();
			};

			var btnLast = FindViewById<TextView> (Resource.Id.btnMoveLast);
			btnLast.Click += (o, e) => {
				movenumber = movelist.List.Count();				
				UpdateGame();
			};

			UpdateGame ();
		}

		void UpdateGameInfo (int gameId, MoveList movelist)
		{
			var info = AndroidConnectivity.GetGameInfo (gameId);

			if (info != null && info.Movelist != null) 
			{
				movelist.UpdateList(info.Movelist);
				localStorage.SaveGameInfoToStorage (info);
			}
		}

		void UpdateGame ()
		{
			board.BuildBoardFromMoveList(movelist, movenumber);
			(game_grid.Adapter as ImageAdapter).SetBoard(board.Squares);
			game_grid.InvalidateViews();
		}
	}
}
