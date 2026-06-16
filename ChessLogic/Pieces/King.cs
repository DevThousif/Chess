using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class King : Piece
    {

        public override PieceType Type => PieceType.King;
        public override Player Color { get; }
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.NorthWest,
        };

        public King(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction dir in dirs)
            {
                Position to = from + dir;
                if (!Board.IsInside(to))
                {
                    continue;
                }

                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }

            }

        }
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }

            // castling
            // only if king hasn't moved and not currently in check
            if (!HasMoved && !board.IsInCheck(Color))
            {
                int row = from.Row;
                // kingside: rook at column 7, spaces 5 and 6 empty
                Position rookPosKS = new Position(row, 7);
                if (Board.IsInside(rookPosKS) && !board.IsEmpty(rookPosKS))
                {
                    Piece rook = board[rookPosKS];
                    if (rook.Type == PieceType.Rook && rook.Color == Color && !rook.HasMoved)
                    {
                        Position between1 = new Position(row, 5);
                        Position between2 = new Position(row, 6);
                        if (board.IsEmpty(between1) && board.IsEmpty(between2))
                        {
                            // squares the king passes through must not be attacked
                            if (!board.IsSquareAttacked(between1, Color.Opponent()) && !board.IsSquareAttacked(between2, Color.Opponent()))
                            {
                                yield return new CastleMove(MoveType.CastleKS, from, between2, rookPosKS, new Position(row, 5));
                            }
                        }
                    }
                }
                // queenside: rook at column 0, spaces 1,2,3 empty
                Position rookPosQS = new Position(row, 0);
                if (Board.IsInside(rookPosQS) && !board.IsEmpty(rookPosQS))
                {
                    Piece rook = board[rookPosQS];
                    if (rook.Type == PieceType.Rook && rook.Color == Color && !rook.HasMoved)
                    {
                        Position b1 = new Position(row, 1);
                        Position b2 = new Position(row, 2);
                        Position b3 = new Position(row, 3);
                        if (board.IsEmpty(b1) && board.IsEmpty(b2) && board.IsEmpty(b3))
                        {
                            // squares the king passes through must not be attacked
                            if (!board.IsSquareAttacked(b3, Color.Opponent()) && !board.IsSquareAttacked(b2, Color.Opponent()))
                            {
                                yield return new CastleMove(MoveType.CastleQS, from, b2, rookPosQS, new Position(row, 3));
                            }
                        }
                    }
                }
            }
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });

        }



    }

}