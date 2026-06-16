using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class GameState
    {
        public Board Board {  get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;
        // history of board keys (including side to move) to detect repetition
        private readonly List<string> positionHistory = new List<string>();

        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
            // record initial position
            positionHistory.Add(PositionKey());
        }
        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if(Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }
            Piece piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
            return moveCandidates.Where(move  => move.IsLegal(Board));

        }
        public void MakeMove(Move move)
        {
            // clear en-passant by default; special moves will set it if needed
            Board.EnPassantTarget = null;
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
            positionHistory.Add(PositionKey());
            CheckForGameOver();

        }

        private string PositionKey()
        {
            // include side to move so that same placement with different side is different
            return (Board.GetBoardKey() + (CurrentPlayer == Player.White ? " w" : " b"));
        }

        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });
            return moveCandidates.Where(move => move.IsLegal(Board));
        }
        private void CheckForGameOver()
        {
            // check threefold repetition
            string current = PositionKey();
            int occurrences = positionHistory.Count(k => k == current);
            if (occurrences >= 3)
            {
                Result = Result.Draw(EndReason.ThreefoldRepetition);
                return;
            }
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if (Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(CurrentPlayer.Opponent());

                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
        }
        public bool IsGameOver()
        {
            return Result != null; 
        }
    }
}
