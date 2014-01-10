using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace othelloBase.Test
{
    [TestClass]
    public class OthelloBoardTests
    {
        [TestMethod]        
        public void OthelloBoard_GetNeighbour_North_WorksOk()
        {
            Assert.AreEqual(new Move("A1"), OthelloBoard.GetNeighbour(Direction.North, new Move("A2")));
            Assert.AreEqual(new Move("D2"), OthelloBoard.GetNeighbour(Direction.North, new Move("D3")));
            Assert.AreEqual(new Move("H7"), OthelloBoard.GetNeighbour(Direction.North, new Move("H8")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.North, new Move("A1")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbour_NorthEast_WorksOk()
        {
            Assert.AreEqual(new Move("C1"), OthelloBoard.GetNeighbour(Direction.NorthEast, new Move("B2")));
            Assert.AreEqual(new Move("E2"), OthelloBoard.GetNeighbour(Direction.NorthEast, new Move("D3")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.NorthEast, new Move("H2")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.NorthEast, new Move("G1")));
        }
        

        [TestMethod]
        public void OthelloBoard_GetNeighbour_East_WorksOk()
        {
            Assert.AreEqual(new Move("B2"), OthelloBoard.GetNeighbour(Direction.East, new Move("A2")));
            Assert.AreEqual(new Move("E3"), OthelloBoard.GetNeighbour(Direction.East, new Move("D3")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.East, new Move("H1")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbour_SouthEast_WorksOk()
        {
            Assert.AreEqual(new Move("C3"), OthelloBoard.GetNeighbour(Direction.SouthEast, new Move("B2")));
            Assert.AreEqual(new Move("E4"), OthelloBoard.GetNeighbour(Direction.SouthEast, new Move("D3")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.SouthEast, new Move("H2")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.SouthEast, new Move("G8")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbour_South_WorksOk()
        {
            Assert.AreEqual(new Move("A3"), OthelloBoard.GetNeighbour(Direction.South, new Move("A2")));
            Assert.AreEqual(new Move("D4"), OthelloBoard.GetNeighbour(Direction.South, new Move("D3")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.South, new Move("H8")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbour_SouthWest_WorksOk()
        {
            Assert.AreEqual(new Move("A3"), OthelloBoard.GetNeighbour(Direction.SouthWest, new Move("B2")));
            Assert.AreEqual(new Move("C4"), OthelloBoard.GetNeighbour(Direction.SouthWest, new Move("D3")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.SouthWest, new Move("A2")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.SouthWest, new Move("G8")));
        }


        [TestMethod]
        public void OthelloBoard_GetNeighbour_West_WorksOk()
        {
            Assert.AreEqual(new Move("A2"), OthelloBoard.GetNeighbour(Direction.West, new Move("B2")));
            Assert.AreEqual(new Move("C3"), OthelloBoard.GetNeighbour(Direction.West, new Move("D3")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.West, new Move("A8")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbour_NorthWest_WorksOk()
        {
            Assert.AreEqual(new Move("A1"), OthelloBoard.GetNeighbour(Direction.NorthWest, new Move("B2")));
            Assert.AreEqual(new Move("C2"), OthelloBoard.GetNeighbour(Direction.NorthWest, new Move("D3")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.NorthWest, new Move("A2")));
            Assert.IsNull(OthelloBoard.GetNeighbour(Direction.NorthWest, new Move("G1")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_North_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.North, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 3);
            Assert.IsTrue(neigbourList.Contains(new Move("C1")));
            Assert.IsTrue(neigbourList.Contains(new Move("C2")));
            Assert.IsTrue(neigbourList.Contains(new Move("C3")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_NorthEast_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.NorthEast, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 3);
            Assert.IsTrue(neigbourList.Contains(new Move("F1")));
            Assert.IsTrue(neigbourList.Contains(new Move("E2")));
            Assert.IsTrue(neigbourList.Contains(new Move("D3")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_East_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.East, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 5);
            Assert.IsTrue(neigbourList.Contains(new Move("D4")));
            Assert.IsTrue(neigbourList.Contains(new Move("E4")));
            Assert.IsTrue(neigbourList.Contains(new Move("F4")));
            Assert.IsTrue(neigbourList.Contains(new Move("G4")));
            Assert.IsTrue(neigbourList.Contains(new Move("H4")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_SouthEast_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.SouthEast, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 4);
            Assert.IsTrue(neigbourList.Contains(new Move("D5")));
            Assert.IsTrue(neigbourList.Contains(new Move("E6")));
            Assert.IsTrue(neigbourList.Contains(new Move("F7")));
            Assert.IsTrue(neigbourList.Contains(new Move("G8")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_South_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.South, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 4);            
            Assert.IsTrue(neigbourList.Contains(new Move("C5")));
            Assert.IsTrue(neigbourList.Contains(new Move("C6")));
            Assert.IsTrue(neigbourList.Contains(new Move("C7")));
            Assert.IsTrue(neigbourList.Contains(new Move("C8")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_SouthWest_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.SouthWest, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 2);
            Assert.IsTrue(neigbourList.Contains(new Move("B5")));
            Assert.IsTrue(neigbourList.Contains(new Move("A6")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_West_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.West, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 2);
            Assert.IsTrue(neigbourList.Contains(new Move("B4")));
            Assert.IsTrue(neigbourList.Contains(new Move("A4")));
        }

        [TestMethod]
        public void OthelloBoard_GetNeighbourList_NorthWest_WorksOk()
        {
            var neigbourList = OthelloBoard.GetNeigbourList(Direction.NorthWest, new Move("C4"));
            Assert.IsTrue(neigbourList.Count == 2);
            Assert.IsTrue(neigbourList.Contains(new Move("B3")));
            Assert.IsTrue(neigbourList.Contains(new Move("A2")));
        }

        [TestMethod]
        public void OthelloBoard_GetFlippedNeighbourList_start_WorksOk()
        {
            var board = new OthelloBoard();
            var neigbourList = board.GetNeighboursThatShouldBeFlipped(new Move("C4"), SquareType.Black);
            Assert.IsTrue(neigbourList.Count == 1);
            Assert.IsTrue(neigbourList.Contains(new Move("D4")));            
        }

        [TestMethod]
        public void OthelloBoard_GetFlippedNeighbourList_AllDirections_WorksOk()
        {
            var board = new OthelloBoard();
            
            var move = new Move("D4");
            
            for (int i = 0; i <= 63; i++)
            {
                board.Squares[i] = new Move(i).IsEdgePiece ? SquareType.White : SquareType.Black;
            }
            board.Squares[move.Position] = SquareType.Empty;

            var neigbourList = board.GetNeighboursThatShouldBeFlipped(move, SquareType.White);
            Assert.AreEqual(19, neigbourList.Count);
        }


        [TestMethod]
        public void OthelloBoard_MakeMove_WorksOk()
        {
            var board = new OthelloBoard();
            var move = new Move("C4");
            Assert.AreEqual(2, board.NumberOfBlackDiscs);
            Assert.AreEqual(2, board.NumberOfWhiteDiscs);
            board.MakeMove(move);
            Assert.AreEqual(4, board.NumberOfBlackDiscs);
            Assert.AreEqual(1, board.NumberOfWhiteDiscs);
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void OthelloBoard_MakeMove_OccupiedSquare()
        {
            var board = new OthelloBoard();
            var move = new Move("D4");
            board.MakeMove(move);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void OthelloBoard_MakeMove_NoDiscsFlipped()
        {
            var board = new OthelloBoard();
            var move = new Move("D4");
            board.MakeMove(move);
        }

        [TestMethod]        
        public void OthelloBoard_PossibleMoves_WorksOk()
        {
            var board = new OthelloBoard();
            Assert.AreEqual(4, board.PossibleMoves(SquareType.Black));
            Assert.AreEqual(4, board.PossibleMoves(SquareType.White));
            board.MakeMove(new Move("C4"));
            Assert.AreEqual(3, board.PossibleMoves(SquareType.Black));
            Assert.AreEqual(3, board.PossibleMoves(SquareType.White));
            board.MakeMove(new Move("E3"));
            Assert.AreEqual(5, board.PossibleMoves(SquareType.Black));
            Assert.AreEqual(4, board.PossibleMoves(SquareType.White));
        }

        [TestMethod]
        public void OthelloBoard_PossibleMoves_EarlyWipe_WorksOk()
        {
            var board = new OthelloBoard("f5d6c5f4e7f6g5e6");
            Assert.AreEqual(7, board.PossibleMoves(SquareType.Black));
            board.MakeMove(new Move("e3"));
            Assert.AreEqual(0, board.PossibleMoves(SquareType.White));
            Assert.AreEqual(0, board.PossibleMoves(SquareType.Black));
            Assert.AreEqual(true, board.GameFinished);
        }

        [TestMethod]
        public void OthelloBoard_PossibleMoves_MultiplePass_WorksOk()
        {
            var board = new OthelloBoard("e6f6f5d6e7f7d7f8e8c7g8g5g6f4d8g4h4h5h6g7f3h3h8g3e3e2h7");
            Assert.AreEqual(SquareType.White, board.NextColor);
            Assert.AreEqual(2, board.PossibleMoves(SquareType.White));            
            board.MakeMove(new Move("d3"));
            Assert.AreEqual(SquareType.Black, board.NextColor);
            Assert.AreEqual(15, board.PossibleMoves(SquareType.Black));
            Assert.AreEqual(0, board.PossibleMoves(SquareType.White));
            board.MakeMove(new Move("c8"));
            Assert.AreEqual(SquareType.Black, board.NextColor);
            Assert.AreEqual(14, board.PossibleMoves(SquareType.Black));
            Assert.AreEqual(0, board.PossibleMoves(SquareType.White));
            board.MakeMove(new Move("b8"));
            Assert.AreEqual(SquareType.Black, board.NextColor);
            Assert.AreEqual(11, board.PossibleMoves(SquareType.Black));
            Assert.AreEqual(0, board.PossibleMoves(SquareType.White));            
        }

        [TestMethod]
        public void OthelloBoard_NumberOfDiscs_WorksOk()
        {
            var board = new OthelloBoard();
            Assert.AreEqual(2, board.NumberOfWhiteDiscs);
            Assert.AreEqual(2, board.NumberOfBlackDiscs);
            board.BuildBoardFromMoveList("e6f6f5d6e7f7d7f8e8c7g8g5g6f4d8g4h4h5h6g7f3h3h8g3e3e2h7");
            Assert.AreEqual(10, board.NumberOfWhiteDiscs);
            Assert.AreEqual(21, board.NumberOfBlackDiscs);
        }

        [TestMethod]
        public void OthelloBoard_NumberOfDiscs_EarlyWipe_WorksOk()
        {
            var board = new OthelloBoard("f5d6c5f4e7f6g5e6e3");
            Assert.AreEqual(0, board.NumberOfWhiteDiscs);
            Assert.AreEqual(13, board.NumberOfBlackDiscs);
            Assert.AreEqual(true, board.GameFinished);
        }

        [TestMethod]
        public void OthelloBoard_Score_EarlyBlackWipe_WorksOk()
        {
            var board = new OthelloBoard("f5d6c5f4e7f6g5e6");
            Assert.AreEqual(false, board.GameFinished);
            Assert.AreEqual("", board.FinalScore);
            board.MakeMove("e3");
            Assert.AreEqual(true, board.GameFinished);
            Assert.AreEqual("64 - 0", board.FinalScore);
        }

        [TestMethod]
        public void OthelloBoard_Score_EarlyWhiteWipe_WorksOk()
        {
            var board = new OthelloBoard("f5f6e6d6c4c3c5c6d3e3f3f4g5f2d7d8e7f7c7h5e8f8c8b8e2e1d2");
            board.MakeMove("d1");
            Assert.AreEqual(true, board.GameFinished);
            Assert.AreEqual("0 - 64", board.FinalScore);
        }


        [TestMethod]
        public void OthelloBoard_Score_TwoEmptyDrawGame_WorksOk()
        {
            var board = new OthelloBoard();
            board.SetBoard(GetDrawBoardWithTwoEmpties());
            Assert.AreEqual(31, board.NumberOfBlackDiscs);
            Assert.AreEqual(31, board.NumberOfWhiteDiscs);
            Assert.AreEqual(true, board.GameFinished);
            Assert.AreEqual("32 - 32", board.FinalScore);
        }

        [TestMethod]
        public void OthelloBoard_CharToSquareType_WorksOk()
        {
            Assert.AreEqual(SquareType.Black, OthelloBoard.CharToSquareType('x'));
            Assert.AreEqual(SquareType.Black, OthelloBoard.CharToSquareType('X'));
            Assert.AreEqual(SquareType.White, OthelloBoard.CharToSquareType('o'));
            Assert.AreEqual(SquareType.White, OthelloBoard.CharToSquareType('O'));
            Assert.AreEqual(SquareType.Empty, OthelloBoard.CharToSquareType('-'));
            Assert.AreEqual(SquareType.Empty, OthelloBoard.CharToSquareType(' '));
        }


        [TestMethod]
        public void OthelloBoard_BuildBoardFromMoveList_WorksOk()
        {
            var movelist = new MoveList("d3c5f6f5e6e3c3f3c4b4b5d2a3d6c6b3c2e7f7d7f4g4d1g3g6g5e2a5f2a4a6c7b6f1f8e8h6c1h4e1h3g2a2g8h1b2a1b1g1h2c8d8h8h7g7a7a8h5b7b8");
            var board = new OthelloBoard();

            for (int i = 0; i < movelist.List.Count; i++)
            {
                board.BuildBoardFromMoveList(movelist, i);
            }
        }

        [TestMethod]
        //[Ignore]
        public void OthelloBoard_BuildBoardFromMoveList_PerformanceCheck()
        {
            var movelist = new MoveList("d3c5f6f5e6e3c3f3c4b4b5d2a3d6c6b3c2e7f7d7f4g4d1g3g6g5e2a5f2a4a6c7b6f1f8e8h6c1h4e1h3g2a2g8h1b2a1b1g1h2c8d8h8h7g7a7a8h5b7b8");
            var board = new OthelloBoard();
            var start = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                board.BuildBoardFromMoveList(movelist, i);
            }
            var stop = DateTime.Now;
            Assert.IsTrue((stop-start).Milliseconds < 150, string.Format("Time exceeded 150ms, it took {0}ms", (stop - start).Milliseconds));
        }



        [TestMethod]
        public void OthelloBoard_MoveList_WorksOk()
        {
            var movelist = new MoveList("d3c5f6f5e6e3");
            var board = new OthelloBoard();
            Assert.AreEqual(0, board.MoveList.List.Count);
            board.BuildBoardFromMoveList(movelist);
            Assert.AreEqual(movelist.List.Count, board.MoveList.List.Count);
            board.MakeMove("c3");
            Assert.AreEqual(movelist.List.Count+1, board.MoveList.List.Count);
            board.ResetBoard();
            Assert.AreEqual(0, board.MoveList.List.Count);
        }

        private string GetDrawBoardWithTwoEmpties()
        {
            return
                "XXXXXXX-" +
                "XXXOOOOX" +
                "XXXOOOOX" +
                "XOOXOOOX" +
                "XOOOOOOX" +
                "XOOOOOOX" +
                "XOOOOOOX" +
                "-XXXXXXX";
        }
    }
} 


