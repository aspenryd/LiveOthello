using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace othelloBase
{
    public class LiveOthelloService
    {
        public IEnumerable<Tournament> GetTournaments()
        {
            using (WebClient wc = new WebClient())
            {
                var tournaments = new List<Tournament>();

				GetTournamentsForYear (wc, tournaments, DateTime.Now.Year);
				GetTournamentsForYear (wc, tournaments, DateTime.Now.Year-1);


                return tournaments;
            }
        }

		void GetTournamentsForYear (WebClient wc, IList<Tournament> tournaments, int year)
		{

			string aRequestHTML = wc.DownloadString("http://www.liveothello.com/mobile?year="+year);

			Regex r = new Regex(@"TourID[^>]*>(.*?)</a>");
			MatchCollection mcl = r.Matches(aRequestHTML);
			List<string> a = new List<string>();
			foreach (Match ml in mcl)
			{
				int id;
				string name;
				if (ParseIdAndNameFromTournamentstring(ml.ToString(), out id, out name))
					tournaments.Add(new Tournament(){Id = id, Name = name});
			}
		}

		public IEnumerable<Tournament> GetMockTournaments()
		{
			var tournaments = new List<Tournament>();
			tournaments.Add(new Tournament(){Id = 1, Name = "Swedish Champs"});
			tournaments.Add(new Tournament(){Id = 2, Name = "Danish Champs"});
			return tournaments;
		}
		public IEnumerable<Game> GetMockGamesFromTournament(int tournamentid)
		{
			var games = new List<Game>();
			games.Add(new Game(){Id = 2552, Name = "Game1"});
			games.Add(new Game(){Id = 2549, Name = "Game2"});
			return games;
		}

        public bool ParseIdAndNameFromTournamentstring(string text, out int id, out string name)
        {
            //TourID=181">French Champ. 2013</a>
            try
            {
                var strings = text.Remove(0, 7).Split('"');
                id = int.Parse(strings[0]);
                name = convertHtmlSignsInString(strings[1].Remove(0, 1).Replace("</a>", ""));
            }
            catch (Exception)
            {
                id = 0;
                name = "";
                return false;
            }
            return true;
        }


        public IEnumerable<Game> GetGamesFromTournament(int tournamentid)
        {
            using (WebClient wc = new WebClient())
            {
                var games = new List<Game>();
                string aRequestHTML = wc.DownloadString("http://www.liveothello.com/mobile/mobile_tour.php?TourID=" + tournamentid.ToString());


                Regex r = new Regex(@"GameID[^>]*>(.*?)</a> ");
                MatchCollection mcl = r.Matches(aRequestHTML);
                List<string> a = new List<string>();
                foreach (Match ml in mcl)
                {
                    int id;
                    string name;
                    if (ParseIdAndNameFromGamestring(ml.ToString(), out id, out name))
                        games.Add(new Game() { Id = id, Name = name });
                }


                return games;
            }
        }

        public bool ParseIdAndNameFromGamestring(string text, out int id, out string name)
        {
            //GameID=2552">Odsell 31-33 Berner (F2)</a> 
            try
            {
                var strings = text.Remove(0, 7).Split('"');
                id = int.Parse(strings[0]);
                name = convertHtmlSignsInString(strings[1].Remove(0, 1).Replace("</a> ", ""));
            }
			catch (Exception)
            {
                id = 0;
                name = "";
                return false;
            }
            return true;
        }

        public static string convertHtmlSignsInString(string text)
        {
            Regex r = new Regex(@"&#[0-9]{2,3};");
            MatchCollection mcl = r.Matches(text);
            List<string> a = new List<string>();
            foreach (Match ml in mcl)
            {
                text = text.Replace(ml.ToString(), GetCharFromHTMLCode(ml.ToString()));
            }

            return text;
        }

        public static string GetCharFromHTMLCode(string code)
        {
            if (code == "&#39;") return "'";
            code = code.Trim('&', '#', ';');
            var intcode = Int32.Parse(code);
            var c = Convert.ToChar(intcode);
            return c.ToString();
        }

        public GameInfo GetGameInfoFromId(int gameid)
        {
            using (WebClient wc = new WebClient())
            {
                var aRequestHTML = wc.DownloadString("http://www.liveothello.com/mobile/new.php?GameID=" + gameid.ToString());

                Regex r = new Regex(@"kifu=[^>]*&size");
                MatchCollection mcl = r.Matches(aRequestHTML);
                List<string> a = new List<string>();
                foreach (Match ml in mcl)
                {
                    string moves;
                    if (ParseMoveStringFromGamestring(ml.ToString(), out moves))
                        return new GameInfo() { Id = gameid, Movelist = moves };
                }
                return null;
            }
        }

        public bool ParseMoveStringFromGamestring(string text, out string movestring)
        {
            //kifu=c4e3f4c5d6f3e...g2h1a7&size
            try
            {
                movestring = text.Remove(0, 5).Replace("&size", "");
            }
			catch (Exception)
            {
                movestring = "";
                return false;
            }
            return true;
        }
    }
}
