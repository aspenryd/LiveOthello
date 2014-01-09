using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace othelloBase.Test
{
    [TestClass]
    public class LiveOthelloTests
    {
        [TestMethod]
        public void GetTournaments_RetrievesTournaments()
        {
            var start = DateTime.Now;
            var tournaments = new LiveOthelloService().GetTournaments();
            var stop = DateTime.Now;
            Assert.IsNotNull(tournaments);
            Assert.IsTrue(tournaments.Any());
            Assert.AreEqual(35, tournaments.Count());
            Assert.IsTrue((stop-start).Milliseconds <= 300, string.Format("Time exceeded 300ms, it took {0}ms",(stop-start).Milliseconds));
        }

        [TestMethod]
        public void ParseIdAndNameFromTournamentstring_RetrievesTournaments()
        {
            var text = @"TourID=181"">French Champ. 2013</a>";
            int id;
            string name;
            var couldParse = new LiveOthelloService().ParseIdAndNameFromTournamentstring(text, out id, out name);
            Assert.IsTrue(couldParse);
            Assert.AreEqual(181, id);
            Assert.AreEqual("French Champ. 2013", name);
        }

        [TestMethod]
        public void GetGamesFromTournament_RetrievesTournaments()
        {
            var start = DateTime.Now;
            var games = new LiveOthelloService().GetGamesFromTournament(180);
            var stop = DateTime.Now;
            Assert.IsNotNull(games);
            Assert.IsTrue(games.Any());
            Assert.AreEqual(13, games.Count());
            Assert.IsTrue((stop - start).Milliseconds <= 200, string.Format("Time exceeded 200ms, it took {0}ms", (stop - start).Milliseconds));
        }

        [TestMethod]
        public void ParseIdAndNameFromGamestring_RetrievesIdAndName()
        {
            var text = @"GameID=2552"">Odsell 31-33 Berner (F2)</a> ";
            int id;
            string name;
            var couldParse = new LiveOthelloService().ParseIdAndNameFromGamestring(text, out id, out name);
            Assert.IsTrue(couldParse);
            Assert.AreEqual(2552, id);
            Assert.AreEqual("Odsell 31-33 Berner (F2)", name);
        }

        [TestMethod]
        public void GetGameInfoFromId_RetrievesGameInfo()
        {
            var start = DateTime.Now;
            var game = new LiveOthelloService().GetGameInfoFromId(2552);
            var stop = DateTime.Now;
            Assert.IsNotNull(game);
            Assert.AreEqual(2552, game.Id);
            Assert.AreEqual("c4e3f4c5d6f3e6b4c3c6b3f6b6d3b5g4d7f5g5h6c7g6e7a3h5a4h3f7e8a5g3a6f2c8e2d8b8f8g8h4h7d2b7d1e1a8c1h8g7h2c2g1f1b1b2a1a2g2h1a7", game.Movelist);
            Assert.IsTrue((stop - start).Milliseconds <= 150, string.Format("Time exceeded 150ms, it took {0}ms", (stop - start).Milliseconds));
        }

        [TestMethod]
        public void ParseMoveStringFromGamestring_RetrievesMovelist()
        {
            var text = @"kifu=c4e3f4c5d6f3e6b4c3c6b3f6b6d3b5g4d7f5g5h6c7g6e7a3h5a4h3f7e8a5g3a6f2c8e2d8b8f8g8h4h7d2b7d1e1a8c1h8g7h2c2g1f1b1b2a1a2g2h1a7&size";
            string moves;
            var couldParse = new LiveOthelloService().ParseMoveStringFromGamestring(text, out moves);
            Assert.IsTrue(couldParse);            
            Assert.AreEqual("c4e3f4c5d6f3e6b4c3c6b3f6b6d3b5g4d7f5g5h6c7g6e7a3h5a4h3f7e8a5g3a6f2c8e2d8b8f8g8h4h7d2b7d1e1a8c1h8g7h2c2g1f1b1b2a1a2g2h1a7", moves);
        }
    }
}
