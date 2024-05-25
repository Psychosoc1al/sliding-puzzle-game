using PuzzleGame.Models;
using PuzzleGame.Utilities;
using PuzzleGame.Views;

namespace PuzzleGame.Presenters;

public class GameManager
{
    private static readonly Lazy<GameManager> Instance = new(() => new GameManager());
    private readonly StartGameDialog _dialog = new();
    private readonly GameWindow _gameWindow = new();
    private Board _board = null!;
    private IStrategy Strategy { get; set; } = null!;

    public static GameManager GameInstance => Instance.Value;

    public void Launch()
    {
        _dialog.FormClosing += StartGameDialog_FormClosing;
        _gameWindow.FormClosing += GameWindowOnClosing;

        Application.Run(_dialog);
    }

    private void SetStrategy(bool isTimeGame, Board board, GameWindow gameWindow)
    {
        Strategy = isTimeGame
            ? new TimeCountStrategy(board, gameWindow)
            : new StepsCountStrategy(board, gameWindow);
    }

    private void StartGameDialog_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (((StartGameDialog)sender!).DialogResult == DialogResult.OK)
        {
            e.Cancel = true;
            _dialog.Hide();

            var size = _dialog.BoardSize;
            _board = new Board(size);
            _ = new BoardPresenter(_gameWindow, _board);

            SetStrategy(_dialog.IsTimeGame, _board, _gameWindow);
            Strategy.Execute();

            _dialog.DialogResult = DialogResult.Abort;

            _gameWindow.Show();
            return;
        }

        _dialog.Dispose();
        _gameWindow.Dispose();
        Application.Exit();
    }

    private void GameWindowOnClosing(object? sender, FormClosingEventArgs e)
    {
        if (_board.Status == Status.Win)
        {
            e.Cancel = true;
            _gameWindow.Hide();
            _dialog.DialogResult = DialogResult.Abort;
            _dialog.Show();

            return;
        }

        _dialog.Dispose();
        _gameWindow.Dispose();
        Application.Exit();
    }
}
