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

		ViewFlipper flippy;

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

			var info = AndroidConnectivity.GetGameInfo (Int32.Parse(gameId));

			var moveliststring = "d3c5f6f5e6e3c3f3c4b4b5d2a3d6c6b3c2e7f7d7f4g4d1g3g6g5e2a5f2a4a6c7b6f1f8e8h6c1h4e1h3g2a2g8h1b2a1b1g1h2c8d8h8h7g7a7a8h5b7b8";
			if (info != null && info.Movelist != null)
				moveliststring = info.Movelist;
			var movelist = new MoveList(moveliststring);
			var movenumber = 0;
			OthelloBoard board = new OthelloBoard ();


			var btnFirst = FindViewById<TextView> (Resource.Id.btnMoveFirst);
			btnFirst.Click += (o, e) => {
				movenumber = 0;
				board.BuildBoardFromMoveList(movelist, movenumber);
				(game_grid.Adapter as ImageAdapter).SetBoard(board.Squares);
				game_grid.InvalidateViews();
			};

			var btnPrior = FindViewById<TextView> (Resource.Id.btnMovePrior);
			btnPrior.Click += (o, e) => {
				movenumber--;
				if (movenumber < 0 ) movenumber = 0;
				board.BuildBoardFromMoveList(movelist, movenumber);
				(game_grid.Adapter as ImageAdapter).SetBoard(board.Squares);
				game_grid.InvalidateViews();
			};

			var btnNext = FindViewById<TextView> (Resource.Id.btnMoveNext);
			btnNext.Click += (o, e) => {
				movenumber++;
				if (movenumber > movelist.List.Count() ) movenumber = movelist.List.Count();
				board.BuildBoardFromMoveList(movelist, movenumber);
				(game_grid.Adapter as ImageAdapter).SetBoard(board.Squares);
				game_grid.InvalidateViews();
			};

			var btnLast = FindViewById<TextView> (Resource.Id.btnMoveLast);
			btnLast.Click += (o, e) => {
				movenumber = movelist.List.Count();				
				board.BuildBoardFromMoveList(movelist, movenumber);
				(game_grid.Adapter as ImageAdapter).SetBoard(board.Squares);
				game_grid.InvalidateViews();
			};


		}

	}
}

