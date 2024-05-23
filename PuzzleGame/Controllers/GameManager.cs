using System.Diagnostics.CodeAnalysis;
using PuzzleGame.Models;
using PuzzleGame.Utilities;
using PuzzleGame.Views;

namespace PuzzleGame.Controllers;

public class GameManager
{
    private static readonly Lazy<GameManager> Instance = new(() => new GameManager());
    private StartGameDialog? _dialog;
    private MainForm? _mainForm;
    private Board? _board;
    public IStrategy? Strategy { get; private set; }

    public static GameManager GameInstance => Instance.Value;

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    
    public void Launch()
    {
        _dialog = new StartGameDialog();
        _mainForm = new MainForm();
        _dialog.FormClosing += StartGameDialog_FormClosing;
        _mainForm.FormClosing += MainForm_OnClosing;
        Application.Run(_dialog);
    }

    public void SetStrategy(bool isTimeGame, Board board, MainForm mainForm)
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
            _dialog?.Hide();
            
            var size = _dialog!.BoardSize;
            _board = new Board(size);
            var newController = new BoardController(_mainForm!, _board);
            //newController.CreateButtons();
            //_mainForm?.SetController(newController);
            SetStrategy(_dialog.IsTimeGame, _board, _mainForm!);
            Strategy?.Execute();
            
            _mainForm?.Show();
        }
        else
        {
            _dialog?.Dispose();
            _mainForm?.Dispose();
            Application.Exit();
        }
    }
    
    private void MainForm_OnClosing(object? sender, FormClosingEventArgs e)
    {
        if (_board?.Status == Status.Win)
        {
            e.Cancel = true;
            _mainForm?.Hide();
            _dialog!.DialogResult = DialogResult.Abort;
            _dialog?.Show();
        }
        else
        {
            _mainForm?.Dispose();
            _dialog?.Dispose();
            Application.Exit();
        }
            
    }
}