namespace ChessLogic
{
    // Class to represent a position on the chessboard.
    // A position is defined by a row and column, with methods to interact with it.
    public class Position
    {
        // Properties to get the row and column of the position.
        public int Row { get; }
        public int Column { get; }

        // Constructor to initialize a position with a specific row and column.
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        // Method to determine the color of the square at this position.
        // The chessboard alternates between black and white squares.
        // A square is white if the sum of its row and column is even, black if odd.
        public Player SquareColor()
        {
            if ((Row + Column) % 2 == 0)
            {
                return Player.White;  // White square if the sum is even
            }
            return Player.Black;     // Black square if the sum is odd
        }

        // Override of the Equals method to compare two Position objects.
        // It checks if the row and column of the two positions are the same.
        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }


        // Using the in built option of vs code community
        // Override of the GetHashCode method to generate a unique hash code for the Position object.
        // This ensures that Position objects can be used correctly in collections like HashSet or Dictionary.
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);  // Combines row and column to generate the hash code.
        }

        // Overloaded == operator to compare two Position objects.
        // It returns true if the positions are equal (same row and column), false otherwise.
        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        // Overloaded != operator to check if two Position objects are not equal.
        // It returns true if the positions are not equal (different row or column).
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);  // Calls the == operator and negates the result.
        }

        // Overloaded + operator to add a Direction to a Position.
        // This results in a new Position that is offset by the row and column deltas of the Direction.
        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }
    }
}
