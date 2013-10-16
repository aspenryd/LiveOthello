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

namespace test
{
	[Activity (Label = "LiveOthelloGame", ScreenOrientation = ScreenOrientation.Portrait)]		
	public class ViewGameActivity : Activity
	{
		WebView web_view;
		WebView chat_view;

		ViewFlipper flippy;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.ViewGame);

			var gameId = Intent.GetStringExtra("GameId") ?? "2552";

			web_view = FindViewById<WebView> (Resource.Id.webView1);
			web_view.Settings.JavaScriptEnabled = true;
			web_view.LoadUrl ("http://www.liveothello.com/mobile/mobile_game.php?GameID="+gameId);

			web_view.SetWebViewClient (new MyWebViewClient ());

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

		
		}


	}

	public class MyWebViewClient : WebViewClient
	{
		public override bool ShouldOverrideUrlLoading (WebView view, string url)
		{
			view.LoadUrl (url);
			return true;
		}
	}
}

