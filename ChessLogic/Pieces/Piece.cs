using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    // Abstract class to represent a chess piece. 
    // This class is intended to be inherited by specific types of chess pieces (e.g., King, Queen, Knight, etc.).
    public abstract class Piece
    {
        // Property that returns the type of the piece (e.g., King, Queen, etc.).
        // This property must be implemented in derived classes to return the specific piece type.
        public abstract PieceType Type { get; }

        // Property that returns the color of the piece (either White or Black).
        // This property must be implemented in derived classes to return the specific color of the piece.
        public abstract Player Color { get; }

        // A flag to track whether the piece has moved during the game.
        // Default value is false, indicating the piece has not moved yet.
        public bool HasMoved { get; set; } = false;

        // Abstract method to create and return a copy of the piece.
        // This allows the creation of a new piece that is identical to the original one.
        // This method must be implemented in derived classes to perform the actual copying logic.
        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        protected IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }
                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;

                }
                yield break;
            }
        }
        protected IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
        }
        public virtual bool CanCaptureOpponentKing(Position from, Board board)
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
