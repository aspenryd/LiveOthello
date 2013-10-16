using System;
using othelloBase;
using System.Collections.Generic;
using System.Linq;

namespace test
{
	public static class AndroidConnectivity
	{
		public static IList<Tournament> GetTournaments ()
		{
			return new LiveOthelloService ().GetTournaments ().ToList ();
		}

		public static IEnumerable<Game> GetGamesFromTournament (int id)
		{
			return new LiveOthelloService ().GetGamesFromTournament (id);
		}
	}
}

