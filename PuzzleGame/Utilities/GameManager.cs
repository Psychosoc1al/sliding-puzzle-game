using PuzzleGame.Views;

namespace PuzzleGame.Utilities
{
    public class GameManager
    {
        private static readonly Lazy<GameManager> Instance = new(() => new GameManager());

        private GameManager()
        {
        }

        public static GameManager GameInstance => Instance.Value;


        public void StartGame()
        {
            using var dialog = new StartGameDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            var size = dialog.BoardSize;
            var mainForm = new MainForm();
            var shuffleStrategy = new RandomShuffleStrategy();
            var controller = BoardFactory.CreateBoardController(mainForm, size, shuffleStrategy);
            mainForm.SetController(controller);
            Application.Run(mainForm);
        }
    }
}
