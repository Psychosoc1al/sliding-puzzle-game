using PuzzleGame.Models;
using PuzzleGame.Utilities;

namespace PuzzleGame.Controllers;

public class BoardController
{
    public Board Board { get; }
    private readonly Stack<ICommand> _commands = new();

    public BoardController(IBoardView view, Board board)
    {
        Board = board;
        Board.RegisterObserver(view);
        view.SetController(this);
    }

    public void MoveTile(int row, int col)
    {
        var command = new MoveTileCommand(Board, row, col);
        command.Execute();
        _commands.Push(command);
    }

    public void UndoMove()
    {
        if (_commands.Count <= 0) return;
        var command = _commands.Pop();
        command.Undo();
    }
}
