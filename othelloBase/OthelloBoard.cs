using System;
using System.Collections.Generic;
using System.Linq;

namespace othelloBase
{

    #region Class Othelloboard

    public class OthelloBoard
    {
        private Enumerations[] squares;

        public Enumerations[] Squares
        {
            get { return squares; }
            set { squares = value; }
        }

        private Enumerations nextColor;
        

        public Enumerations NextColor
        {
            get { return nextColor; }
            set { nextColor = value; }
        }

        public int NumberOfBlackDiscs { get { return DiscsOfColor(Enumerations.Black); } }
        public int NumberOfWhiteDiscs { get { return DiscsOfColor(Enumerations.White); } }
        public int NumberOfEmptyDiscs { get { return DiscsOfColor(Enumerations.Empty); } }

        public bool GameFinished
        {
            get
            {
                if (NextColor == Enumerations.Empty) return true;
                if (PossibleMoves(NextColor) > 0) return false;
                if (PossibleMoves(ChangeColor(NextColor)) > 0) return false;
                NextColor = Enumerations.Empty;
                return true;
            }
        }

        public string FinalScore
        {
            get
            {
                if (!GameFinished) return "";
                if (NumberOfEmptyDiscs == 0) return string.Format("{0} - {1}", NumberOfBlackDiscs, NumberOfWhiteDiscs);
                if (NumberOfBlackDiscs == NumberOfWhiteDiscs)
                {
                    int halfOfEmpties = NumberOfEmptyDiscs / 2;
                    return string.Format("{0} - {1}", NumberOfBlackDiscs + halfOfEmpties, NumberOfWhiteDiscs + halfOfEmpties);
                }
                if (NumberOfBlackDiscs < NumberOfWhiteDiscs)
                    return string.Format("{0} - {1}", NumberOfBlackDiscs, NumberOfWhiteDiscs + NumberOfEmptyDiscs);
                return string.Format("{0} - {1}", NumberOfBlackDiscs + NumberOfEmptyDiscs, NumberOfWhiteDiscs);
            }
        }

        


        private int DiscsOfColor(Enumerations color)
        {
            return Squares.ToList().Where(s => s == color).Count();
        }

        public static Enumerations[] NewStartingBoard()
        {
            var board = new Enumerations[64];
            for (var i = 0; i < board.Length; i++)
            {
                board[i] = Enumerations.Empty;
            }
            board[27] = Enumerations.White;
            board[28] = Enumerations.Black;
            board[35] = Enumerations.Black;
            board[36] = Enumerations.White;            
            return board;
        }

        public OthelloBoard()
        {
            squares = NewStartingBoard();
            nextColor = Enumerations.Black;
        }

        public OthelloBoard(string movelist)
        {
            squares = NewStartingBoard();
            nextColor = Enumerations.Black;
            BuildBoardFromMoveList(movelist);
        }

        public static Enumerations ChangeColor(Enumerations color)
        {
            return color == Enumerations.Black ? Enumerations.White : Enumerations.Black;
        }

        public OthelloBoard BuildBoardFromMoveList(string movelist, int movenumber = -1)
        {
            var moves = new MoveList(movelist);
            if (movenumber == -1 || movenumber >= moves.List.Count)
                movenumber = moves.List.Count;
            if (movenumber < 0)
                movenumber = 0;
            for (int i = 0; i < movenumber; i++)
            {                
                MakeMove(moves.List[i]);
            }
            return this;
        }

        public IList<Move> MakeMove(string move)
        {
            return MakeMove(new Move(move));
        }

        public IList<Move> MakeMove(Move move)
        {
            if (Squares[move.Position] != Enumerations.Empty)
                throw new ValidationException(string.Format("Unvalid move, square {0} are not empty", move.AsString));
            var flippedDiscs = GetNeighboursThatShouldBeFlipped(move);
            if (flippedDiscs.Count == 0)
                throw new ValidationException(string.Format("Unvalid move, square {0} does not flip any discs", move.AsString));

            var currentColor = NextColor;

            foreach (var flippedDisc in flippedDiscs)
            {
                squares[flippedDisc.Position] = currentColor;
            }
            squares[move.Position] = currentColor;
            SetNextMoveColor(currentColor);
            return flippedDiscs;

        }

        private void SetNextMoveColor(Enumerations currentColor)
        {
            var oppositeColor = ChangeColor(nextColor);
            if (PossibleMoves(oppositeColor) > 0)
                nextColor = oppositeColor;
            else if (PossibleMoves(currentColor) > 0)
                nextColor = currentColor;
            else
                nextColor = Enumerations.Empty;
        }

        public int PossibleMoves(Enumerations color = Enumerations.Empty)
        {
            if (NextColor == Enumerations.Empty) return 0;
            if (color == Enumerations.Empty) color = NextColor;
            var possibleMoves = 0;
            for (int i = 0; i < 64; i++)
            {
                if (Squares[i] == Enumerations.Empty && GetNeighboursThatShouldBeFlipped(new Move(i), color).Count > 0)
                    possibleMoves++;
            }
            return possibleMoves;
        }

        


        public static IList<Move> GetNeigbourList(Direction north, Move move)
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

        public static Move GetNeighbour(Direction direction, Move move)
        {
            switch (direction)
            {
                case Direction.North:
                    if (move.Row <= 1) return null;
                    return new Move(move.Position - 8);
                case Direction.NorthWest:
                    if ((move.Row <= 1) || (move.Column <= 1)) return null;
                    return new Move(move.Position - 9);
                case Direction.West:
                    if (move.Column <= 1) return null;
                    return new Move(move.Position - 1);
                case Direction.SouthWest:
                    if ((move.Row >= 8) || (move.Column <= 1)) return null;
                    return new Move(move.Position + 7);
                case Direction.South:
                    if (move.Row >= 8) return null;
                    return new Move(move.Position + 8);
                case Direction.SouthEast:
                    if ((move.Row >= 8) || (move.Column >= 8)) return null;
                    return new Move(move.Position + 9);
                case Direction.East:
                    if (move.Column >= 8) return null;
                    return new Move(move.Position + 1);
                case Direction.NorthEast:
                    if ((move.Row <= 1) || (move.Column >= 8)) return null;
                    return new Move(move.Position - 7);
                default:
                    return null;
            }
        }

        public IList<Move> GetNeighboursThatShouldBeFlipped(Move move, Enumerations movecolor = Enumerations.Empty)
        {
            var flippedNeighbours = new List<Move>();
            var color = movecolor == Enumerations.Empty ? nextColor : movecolor;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var neighbours = GetNeigbourList(direction, move);
                var oppositeColorNeighbours = new List<Move>();
                foreach (var neighbour in neighbours)
                {
                    if (Squares[neighbour.Position] == ChangeColor(color))
                    {
                        oppositeColorNeighbours.Add(neighbour);
                    }
                    else if (Squares[neighbour.Position] == color && oppositeColorNeighbours.Count > 0)
                    {
                        flippedNeighbours.AddRange(oppositeColorNeighbours);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return flippedNeighbours;
        }

        public void SetBoard(string positions)
        {
            if (positions.Length != 64) throw new ValidationException("Positions must contain 64 chars");
            if (positions.Length != 64) throw new ValidationException("Positions may only consists of the letter x, o and - or space (not case sensitive)");
            var i = 0;
            foreach (var position in positions)
            {
                squares[i] = CharToSquareType(position);
                i++;
            }
        }

        public static Enumerations CharToSquareType(char position)
        {
            position = char.ToLower(position);
            if (position == 'x') return Enumerations.Black;
            if (position == 'o') return Enumerations.White;
            if (position == '-' || position == ' ') return Enumerations.Empty;
            throw new ValidationException(string.Format("Unvalid char for position {0}, only x, o and - or space are valid", position));
        }
    }

    


    #endregion



}


