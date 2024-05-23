using PuzzleGame.Models;
using PuzzleGame.Utilities;
using PuzzleGame.Views;

namespace PuzzleGame.Controllers;

public class BoardController : IObserver, IWinObserver
{
    private readonly Board _board;
    private readonly MainForm _view;
    private Color TileColor { get; }
    private readonly Stack<ICommand> _commands;

    public BoardController(MainForm view, Board board)
    {
        _board = board;
        _view = view;
        _board.Status = Status.StartGame;
        _commands = new Stack<ICommand>();
        TileColor = board.Size switch
        {
            3 => Color.SpringGreen,
            4 => Color.DarkOrange,
            _ => Color.Tomato
        };
        _view.CtrlZEvent += UndoMove;
        _board.RegisterObserver(this);
        _board.RegisterWinObserver(this);
        CreateButtons();
    }

    private void CreateButtons()
    {
        _view.CreateButtons(_board.Size, _board.Tiles);
        _view.BtnClickEvent += MoveTile;
        _view.UpdateView(_board.Size, _board.Tiles, TileColor);
    }


    private void MoveTile(object? sender, CustomEventArgs e)
    {
        _board.Status = Status.Move;
        var command = new MoveTileCommand(_board, e.Row, e.Col);
        command.Execute();
        _commands.Push(command);
    }

    private void UndoMove(object? sender, EventArgs e)
    {
        _board.Status = Status.UndoMove;
        if (_commands.Count <= 0) return;
        var command = _commands.Pop();
        command.Undo();
    }

    public void Update()
    {
        _view.UpdateView(_board.Size, _board.Tiles, TileColor);
    }

    public void OnWin()
    {
        MessageBox.Show(
            "Поздравляем! Вы выиграли!\nХотите начать заново?",
            "Победа!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
        _view.Close();
    }
}
