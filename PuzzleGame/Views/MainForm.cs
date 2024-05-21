using PuzzleGame.Controllers;
using PuzzleGame.Models;
using PuzzleGame.Utilities;

namespace PuzzleGame.Views;

public class MainForm : Form, IObserver, IWinObserver
{
    private BoardController? _controller;
    private readonly Label _countTitleLabel;
    private readonly Label _countLabel;
    private Font? _tileFont;

    public MainForm()
    {
        InitializeComponent();
        _countTitleLabel = new Label
        {
            Name = "textField",
            Text = "Счёт:",
            Top = 0,
            Left = 70,
            Height = 100,
            Width = 150,
            Font = new Font("Comfortaa", 15, FontStyle.Bold),
        };

        _countLabel = new Label
        {
            Name = "textField",
            Text = "12",
            TextAlign = ContentAlignment.TopRight,
            Top = 0,
            Left = 270,
            Height = 100,
            Font = new Font("Comfortaa", 15, FontStyle.Bold),
        };

        KeyPreview = true;
        KeyDown += CtrlZ;
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
        GC.Collect();
        if (_controller == null) return;
        _tileFont ??= new Font("Comfortaa", (int)(60.0 / _controller.Board.Size)!, FontStyle.Bold);

        const int offset = 50;
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
                    Top = i * tileSize + offset,
                    Text = _controller.Board.Tiles[i, j].Number.ToString(),
                    BackColor = Color.FromArgb(CountTileAlpha(i, j), _controller.TileColor),
                    ForeColor = Color.FromArgb(255, 67, 67, 67),
                    Font = _tileFont
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

        Controls.Add(_countTitleLabel);
        Controls.Add(_countLabel);
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
        if (_controller == null) return;
        var count = _controller.Counter.Count;
        MessageBox.Show($"Поздравляем! Вы выиграли!\nВаш счёт: {count}\nХотите начать заново?", "Победа!",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        RestartGame();
    }

    private void RestartGame()
    {
        Hide();
        using var dialog = new StartGameDialog();
        if (dialog.ShowDialog() != DialogResult.OK) return;

        var size = dialog.BoardSize;
        var board = new Board(size);
        var counter = new Counter(board);
        var newController = new BoardController(this, board, counter);
        SetController(newController);
        Show();
    }

    private void CtrlZ(object? sender, KeyEventArgs e)
    {
        if (e is { Modifiers: Keys.Control, KeyCode: Keys.Z })
            _controller?.UndoMove();
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        MinimumSize = new Size(500, 583);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Name = "MainForm";
        Text = "Пятнашки";
        DoubleBuffered = true;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        ResumeLayout(false);
    }
}
