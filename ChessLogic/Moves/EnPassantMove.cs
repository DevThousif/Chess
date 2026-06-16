using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class EnPassantMove : Move
    {
        public override MoveType Type => MoveType.EnPassant;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        public EnPassantMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }

        public override void Execute(Board board)
        {
            Piece pawn = board[FromPos];
            // Move our pawn to the target square
            board[ToPos] = pawn;
            board[FromPos] = null;
            pawn.HasMoved = true;

            // Remove the captured pawn which is located on the same column as ToPos,
            // but on the FromPos.Row (i.e., the pawn that moved two squares)
            int capturedRow = FromPos.Row;
            Position capturedPos = new Position(capturedRow, ToPos.Column);
            board[capturedPos] = null;

            // Clear en-passant target after capture
            board.EnPassantTarget = null;
        }
    }
}
