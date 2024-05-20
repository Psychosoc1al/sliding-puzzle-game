using PuzzleGame.Controllers;
using PuzzleGame.Utilities;

namespace PuzzleGame.Views;

public class MainForm : Form, IObserver, IWinObserver
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

    private void UpdateView()
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
                    Text = _controller.Board.Tiles[i, j].Number.ToString(),
                    BackColor = Color.FromArgb(CountTileAlpha(i, j), _controller.TileColor),
                    ForeColor = Color.FromArgb(255, 67, 67, 67),
                    Font = new Font("Comfortaa", (int)(60.0 / _controller.Board.Size), FontStyle.Bold)
                };
                if (_controller.Board.Tiles[i, j].IsEmpty)
                {
                    tileButton.Text = "";
                    tileButton.BackColor = Color.White;
                    tileButton.Enabled = false;
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

    private int CountTileAlpha(int row, int col)
    {
        if (_controller == null) return 0;
        const int offset = 20;
        const double multiplier = 150;
        var size = _controller.Board.Size;

        return (int)(_controller.Board.Tiles[row, col].Number * multiplier / (size * size) + offset);
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
        var newController = BoardFactory.CreateBoardController(this, size);
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
