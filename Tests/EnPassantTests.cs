using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessLogic;

namespace Tests
{
    [TestClass]
    public class EnPassantTests
    {
        [TestMethod]
        public void EnPassantAvailableAfterDoubleStep()
        {
            Board board = new Board();
            
            board[3,4] = new Pawn(Player.White);
            board[1,5] = new Pawn(Player.Black);

            GameState state = new GameState(Player.Black, board);
           
            Move doubleMove = new DoublePawnMove(new Position(1,5), new Position(3,5));
            state.MakeMove(doubleMove);

           
            var moves = state.LegalMovesForPiece(new Position(3,4));
            Assert.IsTrue(moves.Any(m => m.Type == MoveType.EnPassant));
        }

        [TestMethod]
        public void EnPassantExecutesAndRemovesCapturedPawn()
        {
            Board board = new Board();
            board[3,4] = new Pawn(Player.White);
            board[1,5] = new Pawn(Player.Black);

            GameState state = new GameState(Player.Black, board);
            Move doubleMove = new DoublePawnMove(new Position(1,5), new Position(3,5));
            state.MakeMove(doubleMove);

            Move ep = new EnPassantMove(new Position(3,4), new Position(2,5));
            state.MakeMove(ep);

           
            Assert.IsTrue(board.IsEmpty(new Position(3,5)));
           
            Assert.IsFalse(board.IsEmpty(new Position(2,5)));
            Assert.AreEqual(Player.White, board[new Position(2,5)].Color);
        }
    }
}
