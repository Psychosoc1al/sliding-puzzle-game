using PuzzleGame.Presenters;

namespace PuzzleGame;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        GameManager.GameInstance.Launch();
    }
}
