using System;
using Android.Widget;
using Android.Content;
using Android.Views;
using othelloBase;

namespace LiveOthelloAndroid
{
	public class ImageAdapter : BaseAdapter
	{
		Context context;

		int colwidth = 40;

		public ImageAdapter (Context c)
		{
			context = c;
		}

		public ImageAdapter (Context c, int width)
		{
			context = c;
			colwidth = width;
		}

		public override int Count {
			get { return thumbIds.Length; }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public void SetBoard(SquareType[] boardposition)
		{
			for (var i = 0; i < boardposition.Length; i++) 
			{
				thumbIds [i] = SquareTypeToInt (boardposition [i]);
			}
		}

		private int SquareTypeToInt(SquareType type)
		{
			switch (type)
			{
				case SquareType.Empty: return Resource.Drawable.empty_disc;
				case SquareType.Black: return Resource.Drawable.black_disc;
				case SquareType.White: return Resource.Drawable.white_disc;
				default: return Resource.Drawable.empty_disc;
			}
		}

		// create a new ImageView for each item referenced by the Adapter
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ImageView imageView;

			if (convertView == null) {  // if it's not recycled, initialize some attributes
				imageView = new ImageView (context);
				imageView.LayoutParameters = new GridView.LayoutParams (colwidth, colwidth);
				imageView.SetScaleType (ImageView.ScaleType.CenterCrop);
				imageView.SetPadding (1, 1, 0, 0);
			} else {
				imageView = (ImageView)convertView;
			}

			imageView.SetImageResource (thumbIds[position]);
			return imageView;
		}

		// references to our images

		int[] thumbIds = {
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc
			, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, 
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc
			, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, 
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc
			, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, 
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.white_disc
			, Resource.Drawable.black_disc,Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc,
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.black_disc
			, Resource.Drawable.white_disc,Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc,
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc
			, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, 
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc
			, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, 
			Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc
			, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, Resource.Drawable.empty_disc, 
		};

	}
}

