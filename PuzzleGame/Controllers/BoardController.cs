using PuzzleGame.Models;
using PuzzleGame.Utilities;
using PuzzleGame.Views;

namespace PuzzleGame.Controllers;

public class BoardController
{
    public Board Board { get; }
    public Color TileColor { get; }
    private readonly Stack<ICommand> _commands = new();

    public BoardController(MainForm view, Board board)
    {
        Board = board;
        TileColor = board.Size switch
        {
            3 => Color.SpringGreen,
            4 => Color.DarkOrange,
            _ => Color.Tomato
        };
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
