using PuzzleGame.Controllers;

namespace PuzzleGame.Utilities;

public static class BoardFactory
{
    public static BoardController? CreateBoardController(IBoardView view, int size)
    {
        return new BoardController(view, size);
    }
}
