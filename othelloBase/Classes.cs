using System.Collections.Generic;

namespace othelloBase
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public IEnumerable<Game> Games { get; set;}

		public Tournament()
		{
			Games = null;
		}

		public override string ToString()
		{
			return  Name;
		}	

		public static Tournament ParseFromString(string strTournament)
		{
			try
			{
				var id = int.Parse(strTournament.Split(']')[0].Replace("[", ""));
				var name = strTournament.Split(']')[1];
				return new Tournament(){Id = id, Name = name};
			}
			catch
			{
				return null;
			}
		}

		public string ToStorageString()
		{
			return string.Format ("[{0}]{1};", Id, Name);
		}
    }

    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }

		public override string ToString()
		{
			return  Name;
		}	

		public static Game ParseFromString(string strGame)
		{
			try
			{
				var id = int.Parse(strGame.Split(']')[0].Replace("[", ""));
				var name = strGame.Split(']')[1];
				return new Game(){Id = id, Name = name};
			}
			catch
			{
				return null;
			}
		}

		public string ToStorageString()
		{
			return string.Format ("[{0}]{1};", Id, Name);
		}
    }

    public class GameInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Movelist { get; set; }
    }
}
