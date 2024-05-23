using System.Diagnostics.CodeAnalysis;
using PuzzleGame.Models;
using PuzzleGame.Utilities;
using PuzzleGame.Views;

namespace PuzzleGame.Controllers;

public class GameManager
{
    private static readonly Lazy<GameManager> Instance = new(() => new GameManager());
    private readonly StartGameDialog _dialog = new();
    private readonly MainForm _mainForm = new();
    private Board _board = null!;
    private IStrategy Strategy { get; set; } = null!;

    public static GameManager GameInstance => Instance.Value;

    public void Launch()
    {
        _dialog.FormClosing += StartGameDialog_FormClosing;
        _mainForm.FormClosing += MainForm_OnClosing;

        Application.Run(_dialog);
    }

    private void SetStrategy(bool isTimeGame, Board board, MainForm mainForm)
    {
        Strategy = isTimeGame
            ? new TimeCountStrategy(board, mainForm)
            : new StepsCountStrategy(board, mainForm);
    }

    private void StartGameDialog_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (((StartGameDialog)sender!).DialogResult == DialogResult.OK)
        {
            e.Cancel = true;
            _dialog.Hide();

            var size = _dialog.BoardSize;
            _board = new Board(size);
            _ = new BoardController(_mainForm, _board);

            SetStrategy(_dialog.IsTimeGame, _board, _mainForm);
            Strategy.Execute();
            
            _dialog.DialogResult = DialogResult.Abort;
            
            _mainForm.Show();
        }
        else
        {
            _dialog.Dispose();
            _mainForm.Dispose();
            Application.Exit();
        }
    }

    private void MainForm_OnClosing(object? sender, FormClosingEventArgs e)
    {
        if (_board.Status == Status.Win)
        {
            e.Cancel = true;
            _mainForm.Hide();
            _dialog.DialogResult = DialogResult.Abort;
            _dialog.Show();

            return;
        }

        _mainForm.Dispose();
        _dialog.Dispose();
        Application.Exit();
    }
}
