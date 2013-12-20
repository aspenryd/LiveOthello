using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace othelloBase.Test
{
    [TestClass]
    public class OthelloGameTests
    {
        [TestMethod]
        public void Move_ValidateMoveString_CorrectValues()
        {
            Assert.AreEqual(0, Move.ValidateMoveString("A1"));
            Assert.AreEqual(7, Move.ValidateMoveString("A8"));
            Assert.AreEqual(8, Move.ValidateMoveString("B1"));
            Assert.AreEqual(63, Move.ValidateMoveString("H8"));
            Assert.AreEqual(63, Move.ValidateMoveString("h8"));
        }

        [TestMethod]
        [ExpectedException (typeof(ValidationException))]
        public void Move_ValidateMoveString_InCorrectValues()
        {
            Move.ValidateMoveString("I1");
            Move.ValidateMoveString("A0");
            Move.ValidateMoveString("A9");
            Move.ValidateMoveString("AA");
            Move.ValidateMoveString("1A");
        }

        [TestMethod]
        public void Move_PositionAsString_CorrectValues()
        {
            Assert.AreEqual("A1", Move.PositionAsString(0));
            Assert.AreEqual("A8", Move.PositionAsString(7));
            Assert.AreEqual("B1", Move.PositionAsString(8));
            Assert.AreEqual("H8", Move.PositionAsString(63));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Move_PositionAsString_InCorrectValues()
        {
            Move.PositionAsString(-1);
            Move.PositionAsString(64);
        }

        [TestMethod]
        public void Move_Creating()
        {
            var move1 = new Move("B2");
            var move2 = new Move(9);
            var move3 = new Move {Position = 9};
            Assert.AreEqual(move1.Position, move2.Position);
            Assert.AreEqual(move1.AsString, move2.AsString);
            Assert.AreEqual(move1.Position, move3.Position);
            Assert.AreEqual(move1.AsString, move3.AsString);
        }

        [TestMethod]
        public void Move_ChangingValue()
        {
            var move1 = new Move("B2");
            Assert.AreEqual(9, move1.Position);
            move1.AsString = "C3";
            Assert.AreEqual(18, move1.Position);
            move1.Position = 63;
            Assert.AreEqual("H8", move1.AsString);            
        }

        
        [TestMethod]
        public void MoveList_ConvertToMoveList_1Move_CorrectValues()
        {
            var list = MoveList.ConvertToMoveList("A1");
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(0, list.First().Position);
            Assert.AreEqual("A1", list.First().AsString);
        }

        [TestMethod]
        public void MoveList_ConvertToMoveList_3moves_CorrectValues()
        {
            var list = MoveList.ConvertToMoveList("A1B2C3");
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("A1", list[0].AsString);
            Assert.AreEqual("B2", list[1].AsString);
            Assert.AreEqual("C3", list[2].AsString);
        }

        [TestMethod]
        public void MoveList_ConvertToMoveList_4moves_CorrectValues()
        {
            var list = MoveList.ConvertToMoveList("H1H8A1A8");
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual("H1", list[0].AsString);
            Assert.AreEqual("H8", list[1].AsString);
            Assert.AreEqual("A1", list[2].AsString);
            Assert.AreEqual("A8", list[3].AsString);
        }

        [TestMethod]
        public void Move_Equals_Correct()
        {
            Assert.IsTrue(new Move(0).Equals(new Move("A1")));            
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void MoveList_ConvertToMoveList_DuplicateMove_ThrowsException()
        {
            //Assert.AreEqual();
            MoveList.ConvertToMoveList("H1H8H1");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void MoveList_ConvertToMoveList_TooShortMoveList_ThrowsException()
        {
            MoveList.ConvertToMoveList("");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void MoveList_ConvertToMoveList_TooLongMoveList_ThrowsException()
        {
            MoveList.ConvertToMoveList("A1A2A3A4A5A6A7A8B1B2B3B4B5B6B7B8C1C2C3C4C5C6C7C8D1D2D3D4D5D6D7D8E1E2E3E4E5E6E7E8F1F2F3F4F5F6F7F8G1G2G3G4G5G6G7G8H1H2H3H4H5H6H7H8");
        }

        [TestMethod]        
        public void MoveList_Create_WorksOk()
        {
            var movelist = new MoveList("A1A2A3A4");
            Assert.AreEqual(4, movelist.List.Count);
            Assert.AreEqual("A1", movelist.List[0].AsString);
            Assert.AreEqual("A2", movelist.List[1].AsString);
            Assert.AreEqual("A3", movelist.List[2].AsString);
            Assert.AreEqual("A4", movelist.List[3].AsString);
        }
    }
}
