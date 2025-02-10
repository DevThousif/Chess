using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        public NormalMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }
        public override void Execute(Board board)
        {
            Piece piece = board[FromPos];  // Get the piece at the 'FromPos'
            board[ToPos] = piece;  // Move the piece to the new position
            board[FromPos] = null;  // Clear the old position
            piece.HasMoved = true;  // Mark the piece as having moved
        }

    }
}
