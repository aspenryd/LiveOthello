namespace othelloBase
{
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
            get { return decimal.Floor(position / 8) + 1; }
        }

        public decimal Column
        {
            get { return position % 8 + 1; }            
        }

        public bool IsEdgePiece {
            get { return Column == 1 || Column == 8 || Row == 1 || Row == 8; }
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
            var lettervalue = (int) value.ToUpper()[0] - 65; //A == 0
            var digitvalue = int.Parse(value[1].ToString()) - 1; //1 == 0
            return lettervalue + digitvalue *8; //A1 == 0
        }

        public static string PositionAsString(int i)
        {
            if (i < 0 || i > 63) throw new ValidationException("Position value must be between 0 and 63");
            var letter = (char) (i%8 + 65);
            var digit = (decimal.Floor(i / 8) + 1).ToString();
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
}