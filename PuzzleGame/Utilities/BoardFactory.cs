using PuzzleGame.Controllers;
using PuzzleGame.Models;
using PuzzleGame.Views;

namespace PuzzleGame.Utilities
{
    public static class BoardFactory
    {
        public static BoardController CreateBoardController(MainForm view, int size, IShuffleStrategy shuffleStrategy)
        {
            var board = new Board(size, shuffleStrategy);
            return new BoardController(view, board);
        }
    }
}
