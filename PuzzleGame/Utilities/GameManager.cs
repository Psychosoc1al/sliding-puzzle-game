using System.Diagnostics.CodeAnalysis;
using PuzzleGame.Controllers;
using PuzzleGame.Models;
using PuzzleGame.Views;

namespace PuzzleGame.Utilities;

public class GameManager
{
    private static readonly Lazy<GameManager> Instance = new(() => new GameManager());
    public IStrategy? Strategy { get; private set; }

    public static GameManager GameInstance => Instance.Value;

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void StartGame()
    {
        using var dialog = new StartGameDialog();
        if (dialog.ShowDialog() != DialogResult.OK) return;

        var size = dialog.BoardSize;
        var mainForm = new MainForm();
        var board = new Board(size);
        var controller = new BoardController(mainForm, board);
        mainForm.SetController(controller);

        SetStrategy(dialog.IsTimeGame, board, mainForm);
        Strategy?.Execute();

        Application.Run(mainForm);
    }

    public void SetStrategy(bool isTimeGame, Board board, MainForm mainForm)
    {
        Strategy = isTimeGame
            ? new TimeCountStrategy(board, mainForm)
            : new StepsCountStrategy(board, mainForm);
    }
}
