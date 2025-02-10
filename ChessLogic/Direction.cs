namespace ChessLogic
{
    // This class represents a direction on a chessboard or grid.
    // Each direction is represented by a row and column delta.
    public class Direction
    {
        // Predefined directions (static fields):
        // North moves 1 step up (row -1, column 0)
        public readonly static Direction North = new Direction(-1, 0);
        // South moves 1 step down (row 1, column 0)
        public readonly static Direction South = new Direction(1, 0);
        // East moves 1 step right (row 0, column 1)
        public readonly static Direction East = new Direction(0, 1);
        // West moves 1 step left (row 0, column -1)
        public readonly static Direction West = new Direction(0, -1);
        // Diagonal directions:
        // NorthEast is the combination of North and East (row -1, column 1)
        public readonly static Direction NorthEast = North + East;
        // NorthWest is the combination of North and West (row -1, column -1)
        public readonly static Direction NorthWest = North + West;
        // SouthEast is the combination of South and East (row 1, column 1)
        public readonly static Direction SouthEast = South + East;
        // SouthWest is the combination of South and West (row 1, column -1)
        public readonly static Direction SouthWest = South + West;

        // Properties to hold row and column deltas for the direction
        public int RowDelta { get; }
        public int ColumnDelta { get; }

        // Constructor to initialize a direction with row and column deltas
        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        // Operator overloading for + (addition) to combine two directions.
        // It creates a new Direction by adding the row and column deltas of both directions.
        public static Direction operator +(Direction dir1, Direction dir2)
        {
            return new Direction(dir1.RowDelta + dir2.RowDelta, dir1.ColumnDelta + dir2.ColumnDelta);
        }

        // Operator overloading for * (multiplication) to scale a direction by a scalar (integer).
        // It creates a new Direction by multiplying the row and column deltas by the scalar.
        public static Direction operator *(int scalar, Direction dir)
        {
            return new Direction(scalar * dir.RowDelta, scalar * dir.ColumnDelta);
        }
    }
}
