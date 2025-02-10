using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }
        private readonly Direction forward;

        // Constructor to initialize the pawn based on color
        public Pawn(Player color)
        {
            Color = color;
            forward = (color == Player.White) ? Direction.North : Direction.South;
        }

        // Copy method for creating a new instance of Pawn
        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        // Helper function to check if a move is possible
        private static bool CanMoveTo(Position pos, Board board)
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        // Helper function to check if a pawn can capture
        private bool CanCaptureAt(Position pos, Board board)
        {
            return Board.IsInside(pos) && !board.IsEmpty(pos) && board[pos].Color != Color;
        }

        private static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Knight);
            yield return new PawnPromotion(from, to, PieceType.Bishop);
            yield return new PawnPromotion(from, to, PieceType.Rook);
            yield return new PawnPromotion(from, to, PieceType.Queen);


        }




        // Function for generating forward moves (normal pawn move)
        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePos = from + forward;
            if (CanMoveTo(oneMovePos, board))
            {
                if (oneMovePos.Row == 0 || oneMovePos.Row == 7)
                {
                    foreach(Move promMove in PromotionMoves(from, oneMovePos))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, oneMovePos);
                }
                yield return new NormalMove(from, oneMovePos);

                Position twoMovesPos = oneMovePos + forward;

                // Allow double move on first move
                if (!HasMoved && CanMoveTo(twoMovesPos, board))
                {
                    yield return new NormalMove(from, twoMovesPos);
                }
            }
        }

        // Function for generating diagonal capture moves
        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + forward + dir;
                if (CanCaptureAt(to, board))
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promMove in PromotionMoves(from, to))
                        {
                            yield return promMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }

        // Method to get all possible moves for the pawn
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }


        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
