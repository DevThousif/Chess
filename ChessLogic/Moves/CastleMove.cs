using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class CastleMove : Move
    {
        public override MoveType Type { get; }
        public override Position FromPos { get; }
        public override Position ToPos { get; }
        private readonly Position rookFrom;
        private readonly Position rookTo;

        public CastleMove(MoveType type, Position kingFrom, Position kingTo, Position rookFrom, Position rookTo)
        {
            Type = type;
            FromPos = kingFrom;
            ToPos = kingTo;
            this.rookFrom = rookFrom;
            this.rookTo = rookTo;
        }

        public override void Execute(Board board)
        {
            Piece king = board[FromPos];
            Piece rook = board[rookFrom];

            // move king
            board[ToPos] = king;
            board[FromPos] = null;
            king.HasMoved = true;

            // move rook
            board[rookTo] = rook;
            board[rookFrom] = null;
            if (rook != null)
            {
                rook.HasMoved = true;
            }

            // clear en-passant target
            board.EnPassantTarget = null;
        }
    }
}
