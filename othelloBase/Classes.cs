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
    }

    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }

		public override string ToString()
		{
			return  Name;
		}	
    }

    public class GameInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Movelist { get; set; }
    }
}
