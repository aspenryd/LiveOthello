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


namespace LiveOthelloAndroid
{
	[Activity (Label = "LiveOthello Settings", ScreenOrientation = ScreenOrientation.Portrait)]		
	public class SettingsActivity : Activity
	{
		LocalStorage localstorage;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Settings);

			localstorage = new LocalStorage (this.ApplicationContext);

			var cbSounds = FindViewById<CheckBox> (Resource.Id.cbNotificationSound);
			cbSounds.Checked = localstorage.NotificationSound;
			cbSounds.CheckedChange += (o, e) => {
				localstorage.NotificationSound = cbSounds.Checked;
			};

			var cbVibrate = FindViewById<CheckBox> (Resource.Id.cbNotificationVibration);
			cbVibrate.Checked = localstorage.NotificationVibration;
			cbVibrate.CheckedChange += (o, e) => {
				localstorage.NotificationVibration = cbVibrate.Checked;
			};

//			var cbLights = FindViewById<CheckBox> (Resource.Id.cbNotificationLights);
//			cbLights.Checked = localstorage.NotificationLight;
//			cbLights.CheckedChange += (o, e) => {
//				localstorage.NotificationLight = cbLights.Checked;
//			};

			var rgTournaments = FindViewById<RadioGroup> (Resource.Id.rgTournamentNotifications);
			rgTournaments.Check(Tnt2RgId(localstorage.TournamentNotificationType));
			rgTournaments.CheckedChange += (o, e) => {
				localstorage.TournamentNotificationType = TrgId2Nt(rgTournaments.CheckedRadioButtonId);
			};

			var rgGames = FindViewById<RadioGroup> (Resource.Id.rgGamesNotifications);
			rgGames.Check(Gnt2rgId(localstorage.GamesNotificationType));
			rgGames.CheckedChange += (o, e) => {
				localstorage.GamesNotificationType = GrgId2Nt(rgGames.CheckedRadioButtonId);
			};
		}

		int Tnt2RgId (NotificationType notificationType)
		{
			switch (notificationType)
			{
			case NotificationType.None:
				return Resource.Id.rbTournamentNone; 
			case NotificationType.One:
				return Resource.Id.rbTournamentOne; 
			case NotificationType.Every:
				return Resource.Id.rbTournamentEvery; 
			default :
				return Resource.Id.rbTournamentEvery;
			}
		}

		NotificationType TrgId2Nt (int notificationType)
		{
			switch (notificationType)
			{
			case Resource.Id.rbTournamentNone: 
				return NotificationType.None;
			case Resource.Id.rbTournamentOne: 
				return NotificationType.One;
			case Resource.Id.rbTournamentEvery: 
				return NotificationType.Every;
			default :
				return NotificationType.Every;
			}
		}

		int Gnt2rgId (NotificationType notificationType)
		{
			switch (notificationType)
			{
				case NotificationType.None:
				return Resource.Id.rbGamesNone; 
				case NotificationType.One:
				return Resource.Id.rbGamesOne; 
				case NotificationType.Every:
				return Resource.Id.rbGamesEvery; 
				default :
				return Resource.Id.rbGamesEvery;
			}
		}

		NotificationType GrgId2Nt (int notificationType)
		{
			switch (notificationType)
			{
			case Resource.Id.rbGamesNone: 
				return NotificationType.None;
			case Resource.Id.rbGamesOne: 
				return NotificationType.One;
			case Resource.Id.rbGamesEvery: 
				return NotificationType.Every;
			default :
				return NotificationType.Every;
			}
		}
	
	}

}

