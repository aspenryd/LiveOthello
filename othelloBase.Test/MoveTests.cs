using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace othelloBase.Test
{
    [TestClass]
    public class MoveTest
    {
        [TestMethod]
        public void Move_ValidateMoveString_CorrectValues()
        {
            Assert.AreEqual(0, Move.ValidateMoveString("A1"));
            Assert.AreEqual(7, Move.ValidateMoveString("H1"));
            Assert.AreEqual(8, Move.ValidateMoveString("A2"));
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
            Assert.AreEqual("H1", Move.PositionAsString(7));
            Assert.AreEqual("A2", Move.PositionAsString(8));
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
        public void Move_CorrectRow()
        {
            Assert.AreEqual(2, new Move("B2").Row);
            Assert.AreEqual(3, new Move("C3").Row);
            Assert.AreEqual(1, new Move("H1").Row);
            Assert.AreEqual(8, new Move("A8").Row);
        }

        [TestMethod]
        public void Move_CorrectColumn()
        {
            Assert.AreEqual(2, new Move("B2").Column);
            Assert.AreEqual(3, new Move("C3").Column);
            Assert.AreEqual(8, new Move("H1").Column);
            Assert.AreEqual(1, new Move("A8").Column);
        }

 
        [TestMethod]
        public void Move_Equals_Correct()
        {
            Assert.IsTrue(new Move(0).Equals(new Move("A1")));            
        }

 
    }
} 


