using System.Diagnostics.CodeAnalysis;
using PuzzleGame.Models;
using PuzzleGame.Utilities;
using PuzzleGame.Views;

namespace PuzzleGame.Controllers;

public class GameManager
{
    private static readonly Lazy<GameManager> Instance = new(() => new GameManager());
    private StartGameDialog? _dialog;
    public IStrategy? Strategy { get; private set; }

    public static GameManager GameInstance => Instance.Value;

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    
    public void Launch()
    {
        _dialog = new StartGameDialog();
        StartGame();
    }

    public void SetStrategy(bool isTimeGame, Board board, MainForm mainForm)
    {
        Strategy = isTimeGame
            ? new TimeCountStrategy(board, mainForm)
            : new StepsCountStrategy(board, mainForm);
    }
    
    private void StartGame()
    {
        if (_dialog == null) return;
        _dialog.DialogResult = DialogResult.Abort;
        if (_dialog.ShowDialog() != DialogResult.OK) return;
        
        // _dialog.Hide();

        var size = _dialog.BoardSize;
        var mainForm = new MainForm();
        var board = new Board(size);
        mainForm.FormClosed += (_, _) => OnClose(board);
        var newController = new BoardController(mainForm, board);
        mainForm.SetController(newController);
        SetStrategy(_dialog.IsTimeGame, board, mainForm);
        Strategy?.Execute();
        
        Application.Run(mainForm);
        StartGame();
    }
    
    private void OnClose(Board board)
    {
        if (board.Status != Status.Win) Application.Exit();
    }
}
