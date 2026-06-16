using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
        // Square that can be captured en-passant. Null if none.
        public Position EnPassantTarget { get; set; }

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return pieces[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }
        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }
        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(Player.Black);
                this[6, c] = new Pawn(Player.White);
            }
        }
        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);
                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }
        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }


        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }
        public Board Copy()
        {
            Board copy = new Board();
            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }
            // copy en-passant target as well
            if (this.EnPassantTarget != null)
            {
                copy.EnPassantTarget = new Position(this.EnPassantTarget.Row, this.EnPassantTarget.Column);
            }
            return copy;

        }

        // Produce a compact string key that represents the board piece placement and moved flags.
        // This includes piece type and color and whether the piece has moved (to capture castling-related differences).
        public string GetBoardKey()
        {
            var sb = new System.Text.StringBuilder(8 * 8 * 2 + 10);
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);
                    Piece piece = this[pos];
                    if (piece == null)
                    {
                        sb.Append("..");
                    }
                    else
                    {
                        char ch = piece.Type switch
                        {
                            PieceType.Pawn => 'p',
                            PieceType.Knight => 'n',
                            PieceType.Bishop => 'b',
                            PieceType.Rook => 'r',
                            PieceType.Queen => 'q',
                            PieceType.King => 'k',
                            _ => '?'
                        };
                        if (piece.Color == Player.White) ch = char.ToUpperInvariant(ch);
                        sb.Append(ch);
                        sb.Append(piece.HasMoved ? '1' : '0');
                    }
                }
            }
            return sb.ToString();
        }

        // Determine if a square is attacked by any piece of the given player.
        public bool IsSquareAttacked(Position square, Player byPlayer)
        {
            // Pawn attacks
            int dir = (byPlayer == Player.White) ? -1 : 1; // white pawns attack north (-1)
            Position p1 = new Position(square.Row - dir, square.Column - 1);
            Position p2 = new Position(square.Row - dir, square.Column + 1);
            if (IsInside(p1) && !IsEmpty(p1) && this[p1].Type == PieceType.Pawn && this[p1].Color == byPlayer) return true;
            if (IsInside(p2) && !IsEmpty(p2) && this[p2].Type == PieceType.Pawn && this[p2].Color == byPlayer) return true;

            // Knight attacks
            int[] kdr = new int[] { -2, -1, 1, 2 };
            int[] kdc = new int[] { -2, -1, 1, 2 };
            foreach (int dr in kdr)
            {
                foreach (int dc in kdc)
                {
                    if (Math.Abs(dr) == Math.Abs(dc)) continue;
                    Position kp = new Position(square.Row + dr, square.Column + dc);
                    if (IsInside(kp) && !IsEmpty(kp) && this[kp].Type == PieceType.Knight && this[kp].Color == byPlayer) return true;
                }
            }

            // King adjacent
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    Position kp = new Position(square.Row + dr, square.Column + dc);
                    if (IsInside(kp) && !IsEmpty(kp) && this[kp].Type == PieceType.King && this[kp].Color == byPlayer) return true;
                }
            }

            // Sliding pieces: rook/queen (orthogonal), bishop/queen (diagonal)
            Direction[] orth = new Direction[] { Direction.North, Direction.South, Direction.East, Direction.West };
            Direction[] diag = new Direction[] { Direction.NorthEast, Direction.NorthWest, Direction.SouthEast, Direction.SouthWest };

            foreach (Direction d in orth)
            {
                for (Position p = square + d; IsInside(p); p += d)
                {
                    if (IsEmpty(p)) continue;
                    if (this[p].Color != byPlayer) break;
                    if (this[p].Type == PieceType.Rook || this[p].Type == PieceType.Queen) return true;
                    break;
                }
            }
            foreach (Direction d in diag)
            {
                for (Position p = square + d; IsInside(p); p += d)
                {
                    if (IsEmpty(p)) continue;
                    if (this[p].Color != byPlayer) break;
                    if (this[p].Type == PieceType.Bishop || this[p].Type == PieceType.Queen) return true;
                    break;
                }
            }

            return false;
        }
    }
}
