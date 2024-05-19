using PuzzleGame.Views;

namespace PuzzleGame.Utilities;

public abstract class GameManager
{
    private GameManager()
    {
    }

    public static void StartGame()
    {
        using var dialog = new StartGameDialog();
        if (dialog.ShowDialog() != DialogResult.OK) return;
        var size = dialog.BoardSize;
        var mainForm = new MainForm();
        var controller = BoardFactory.CreateBoardController(mainForm, size);
        mainForm.SetController(controller);
        Application.Run(mainForm);
    }
}