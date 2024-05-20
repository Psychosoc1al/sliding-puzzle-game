using PuzzleGame.Controllers;
using PuzzleGame.Models;

namespace PuzzleGame.Utilities
{
    public static class BoardFactory
    {
        public static BoardController CreateBoardController(IBoardView view, int size, IShuffleStrategy shuffleStrategy)
        {
            var board = new Board(size, shuffleStrategy);
            var counter = new Counter(board);
            return new BoardController(view, board, counter);
        }
    }
}
