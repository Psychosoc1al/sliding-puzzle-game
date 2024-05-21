using PuzzleGame.Controllers;
using PuzzleGame.Models;
using PuzzleGame.Views;

namespace PuzzleGame.Utilities;

public class GameManager
{
    private static readonly Lazy<GameManager> Instance = new(() => new GameManager());
    private Strategy? _strategy;

    public static GameManager GameInstance => Instance.Value;


    public void StartGame()
    {
        using var dialog = new StartGameDialog();
        if (dialog.ShowDialog() != DialogResult.OK) return;

        SetStrategy(dialog.IsTimeGame ? new TimeCountStrategy() : new StepsCountStrategy());
        var size = dialog.BoardSize;
        var mainForm = new MainForm();
        var board = new Board(size);
        var counter = new Counter(board);
        var controller = new BoardController(mainForm, board, counter);
        mainForm.SetController(controller);

        Application.Run(mainForm);
    }

    private void SetStrategy(Strategy strategy)
    {
        _strategy = strategy;
    }
}
