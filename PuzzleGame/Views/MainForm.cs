using PuzzleGame.Controllers;
using PuzzleGame.Models;
using PuzzleGame.Utilities;

namespace PuzzleGame.Views;

public class MainForm : Form, IBoardView, IWinObserver
{
    private BoardController? _controller;

    public MainForm()
    {
        InitializeComponent();
    }

    public void SetController(BoardController? controller)
    {
        _controller = controller;
        _controller?.Board.RegisterObserver(this);
        _controller?.Board.RegisterWinObserver(this);
        UpdateView();
    }

    public void UpdateView()
    {
        Controls.Clear();
        if (_controller == null) return;
        var tileSize = ClientSize.Width / _controller.Board.Size;
        for (var i = 0; i < _controller.Board.Size; i++)
        {
            for (var j = 0; j < _controller.Board.Size; j++)
            {
                var tileButton = new Button
                {
                    Width = tileSize,
                    Height = tileSize,
                    Left = j * tileSize,
                    Top = i * tileSize,
                    Text = _controller.Board.Tiles[i, j].Number.ToString()
                };
                if (_controller.Board.Tiles[i, j].IsEmpty)
                {
                    tileButton.Text = "";
                }
                else
                {
                    var row = i;
                    var col = j;
                    tileButton.Click += (_, _) => _controller.MoveTile(row, col);
                }

                Controls.Add(tileButton);
            }
        }
    }

    public new void Update()
    {
        UpdateView();
    }

    public void OnWin()
    {
        MessageBox.Show("Congratulations! You've solved the puzzle! Restarting the game.", "You Win!",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        RestartGame();
    }

    private void RestartGame()
    {
        Hide();
        using var dialog = new StartGameDialog();
        if (dialog.ShowDialog() != DialogResult.OK) return;
        var size = dialog.BoardSize;
        var shuffleStrategy = new RandomShuffleStrategy();
        var newController = BoardFactory.CreateBoardController(this, size, shuffleStrategy);
        SetController(newController);
        Show();
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        ClientSize = new Size(500, 500);
        Name = "MainForm";
        Text = "Sliding Puzzle Game";
        DoubleBuffered = true;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        ResumeLayout(false);
    }
}
