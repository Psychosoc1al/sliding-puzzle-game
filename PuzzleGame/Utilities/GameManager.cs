using PuzzleGame.Controllers;
using PuzzleGame.Models;
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
            var board = new Board(size, shuffleStrategy);
            var counter = new Counter(board);
            var controller = new BoardController(mainForm, board, counter);
            mainForm.SetController(controller);
            Application.Run(mainForm);
        }
    }
}
