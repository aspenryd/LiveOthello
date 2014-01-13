using System.Collections.Generic;
using System.ComponentModel;

namespace othelloBase
{
    [DefaultProperty("List")]
    public class MoveList
    {
        private IList<Move> list = new List<Move>();

        public MoveList()
        {
            list = new List<Move>();
        }

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
			var moves = new List<Move>();
			if (string.IsNullOrWhiteSpace (listOfMoves))
				return moves;

            ValidateListOfMoves(listOfMoves);
            for (int i = 0; i < listOfMoves.Length; i+=2)
            {
                var move = new Move(listOfMoves.Substring(i, 2));
                if (moves.Contains(move)) 
                    throw new ValidationException(string.Format("Move list already contains the move {0}", move));
                moves.Add(move);
            }
            return moves;
        }

        public void UpdateList(string listOfMoves)
        {
            list = ConvertToMoveList(listOfMoves);
        }

        public IList<Move> List {
            get { return list; }
            set { list = value;}
        }

		public string ToString()
		{
			var movestring = "";
			foreach (var move in list) {
				movestring += move.AsString;
			}
			return movestring;
		}
    }
}