using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml.Schema;

namespace othelloBase
{

    #region Exceptions

    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {
        }
    }

    #endregion

    public enum Direction
    {
        North = 0,
        NorthEast = 1,
        East = 2,
        SouthEast = 3,
        South = 4,
        SouthWest = 5,
        West = 6,
        NorthWest = 7
    }


    #region Class Move

    public class Move
    {
        private int position;

        public Move()
        {
        }

        public Move(int pos)
        {
            position = pos;
        }

        public Move(string pos)
        {
            AsString = pos;
        }

        public int Position
        {
            get { return position; }
            set
            {
                if (value < 0 || value > 63)
                    throw new ValidationException("Position value have to be between 0 and 63");
                position = value;
            }
        }

        public string AsString
        {
            get { return PositionAsString(position); }
            set { position = ValidateMoveString(value); }
        }

        public decimal Row {
            get { return decimal.Floor(position / 8); }
        }

        public decimal Column
        {
            get { return position % 8; }
        }

        public static int ValidateMoveString(string value)
        {
            if (value.Length != 2)
                throw new ValidationException("Move string has to be 2 chars long");
            if (value.ToUpper()[0] < 'A' || value.ToUpper()[0] > 'H')
                throw new ValidationException("First char have to be a letter between A and H");
            if (value[1] < '1' || value[1] > '8')
                throw new ValidationException(
                    string.Format("Second char have to be a digit between 1 and 8, but was {0}", value[1]));
            int lettervalue = (int) value.ToUpper()[0] - 65; //A == 0
            int digitvalue = int.Parse(value[1].ToString()) - 1; //1 == 0
            return lettervalue*8 + digitvalue; //A1 == 0
        }

        public static string PositionAsString(int i)
        {
            if (i < 0 || i > 63) throw new ValidationException("Position value must be between 0 and 63");
            var letter = (char) (decimal.Floor(i/8) + 65);
            var digit = (i%8 + 1).ToString();
            return letter + digit;
        }

        public override bool Equals(object x)
        {
            if (x is Move)
                return (x as Move).Position == position;
            return false;
        }

        public override int GetHashCode()
        {
            return position;
        }


    }

    [DefaultProperty("List")]
    public class MoveList
    {
        private IList<Move> list = new List<Move>();
        public MoveList(string listOfMoves)
        {
            
            list = ConvertToMoveList(listOfMoves);
        }

        public static void ValidateListOfMoves(string listOfMoves)
        {
            if (listOfMoves.Length < 2 || listOfMoves.Length > 120)
                throw new ValidationException(string.Format("List of moves must be between 2 and 120 chars long, but was {0} chars", listOfMoves.Length));
        }

        public static IList<Move> ConvertToMoveList(string listOfMoves)
        {
            ValidateListOfMoves(listOfMoves);
            var moves = new List<Move>();
            for (int i = 0; i < listOfMoves.Length; i+=2)
            {
                var move = new Move(listOfMoves.Substring(i, 2));
                if (moves.Contains(move)) 
                    throw new ValidationException(string.Format("Move list already contains the move {0}", move));
                moves.Add(move);
            }
            return moves;
        }

        public IList<Move> List {
            get { return list; }
            set { list = value;}
        }

        public IList<Move> GetNeigbourList(Direction north, Move move)
        {
            var neighbours = new List<Move>();
            var neighbour = GetNeighbour(north, move);
            while (neighbour != null)
            {
                neighbours.Add(neighbour);
                neighbour = GetNeighbour(north, neighbour);
            }
            return neighbours;
        }

        private Move GetNeighbour(Direction direction, Move move)
        {
            switch (direction)
            {
                case Direction.North:
                    if (move.Row < 0) return null;
                    return new Move(move.Position - 8);
                case Direction.East:
                    if (move.Position < 8) return null;
                    return new Move(move.Position - 8);
            }
        }
    }

    #endregion

    #region Class OthelloGame

    public class OthelloGame
    {
        private SquareType[] board = NewStartingBoard();

        public SquareType[] Board
        {
            get { return board; }
            set { board = value; }
        }

        public OthelloGame(string movelist = null)
        {
            if (movelist != null)
                board = BuildBoardFromMoveList(movelist);
        }

        public SquareType[] BuildBoardFromMoveList(string movelist, int movenumber = -1)
        {
            var board = NewStartingBoard();
            var moves = new MoveList(movelist);
            if (movenumber == -1 || movenumber >= moves.List.Count) 
                movenumber = moves.List.Count - 1;
            if (movenumber < 0)
                movenumber = 0;
            for (int i = 0; i < movenumber; i++)
            {
                var color = SquareType.Black;
                MakeMove(board, moves.List[movenumber], ref color);
            }


            return board;
        }

        private void MakeMove(SquareType[] board, Move move, ref SquareType color)
        {
            if (board[move.Position] != SquareType.Empty) 
                throw new ValidationException(string.Format("Felaktigt drag, ruta {0} är redan upptagen", move.AsString));
            board[move.Position] = color;
            FlipDiscs(board, move);
        }

        private void FlipDiscs(SquareType[] board, Move move)
        {
            var flippedDiscs = 0;
            var color = board[move.Position];
            var oppositeColor = ChangeColor(color);
            var neighbours = move.GetNeigbourList(Direction.North);
            
            
        }

        public static SquareType[] NewStartingBoard()
        {
            var board = new SquareType[64];
            for (var i = 0; i < board.Length; i++)
            {
                board[i] = SquareType.Empty;
            }
            board[27] = SquareType.White;
            board[28] = SquareType.Black;
            board[35] = SquareType.Black;
            board[36] = SquareType.White;
            return board;
        }

        public static SquareType ChangeColor(SquareType color)
        {
            return color == SquareType.Black ? SquareType.White : SquareType.Black;
        }

    }

    #endregion
}

