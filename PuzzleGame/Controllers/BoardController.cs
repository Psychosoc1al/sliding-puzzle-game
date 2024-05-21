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
        Board.Status = Status.StartGame;
        TileColor = board.Size switch
        {
            3 => Color.SpringGreen,
            4 => Color.DarkOrange,
            _ => Color.Tomato
        };
    }

    public void MoveTile(int row, int col)
    {
        Board.Status = Status.Move;
        var command = new MoveTileCommand(Board, row, col);
        command.Execute();
        _commands.Push(command);
    }

    public void UndoMove()
    {
        Board.Status = Status.UndoMove;
        if (_commands.Count <= 0) return;
        var command = _commands.Pop();
        command.Undo();
    }
}
