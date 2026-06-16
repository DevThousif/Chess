using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class DoublePawnMove : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        public DoublePawnMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }
        public override void Execute(Board board)
        {
            Piece piece = board[FromPos];
            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;
            // set en-passant target to the square the pawn passed over
            int passedRow = (FromPos.Row + ToPos.Row) / 2;
            board.EnPassantTarget = new Position(passedRow, FromPos.Column);
        }
    }
}
