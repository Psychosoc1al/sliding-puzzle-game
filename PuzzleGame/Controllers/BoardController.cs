using PuzzleGame.Models;
using PuzzleGame.Utilities;

namespace PuzzleGame.Controllers;

public class BoardController
{
    public BoardController(IBoardView view, int size)
    {
        Board = new Board(size);
        Board.RegisterObserver(view);
        view.SetController(this);
    }

    public Board Board { get; }

    public void MoveTile(int row, int col)
    {
        Board.MoveTile(row, col);
    }
}
