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
    private Button[,]? _buttons;

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
            Font = new Font("Comfortaa", 15, FontStyle.Bold)
        };

        _countLabel = new Label
        {
            Name = "textField",
            Text = "12",
            TextAlign = ContentAlignment.TopRight,
            Top = 0,
            Left = 270,
            Height = 100,
            Font = new Font("Comfortaa", 15, FontStyle.Bold)
        };


        KeyPreview = true;
        KeyDown += CtrlZ;
    }

    public void SetController(BoardController? controller)
    {
        _controller = controller;
        _controller?.Board.RegisterObserver(this);
        _controller?.Board.RegisterWinObserver(this);


        const int offset = 50;
        var tileSize = ClientSize.Width / _controller!.Board.Size;
        _tileFont ??= new Font("Comfortaa", (int)(60.0 / _controller.Board.Size)!, FontStyle.Bold);
        var size = _controller!.Board.Size;
        _buttons = new Button[size, size];
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                _buttons[i, j] = new Button
                {
                    Width = tileSize,
                    Height = tileSize,
                    Left = j * tileSize,
                    Top = i * tileSize + offset,
                    Text = _controller.Board.Tiles[i, j].Number.ToString(),
                    Font = _tileFont
                };

                var row = i;
                var col = j;
                _buttons[i, j].Click += (_, _) => _controller.MoveTile(row, col);

                Controls.Add(_buttons[i, j]);
            }
        }

        UpdateView();
    }

    private void UpdateView()
    {
        Controls.Clear();
        if (_controller == null) return;
        for (var i = 0; i < _controller.Board.Size; i++)
        {
            for (var j = 0; j < _controller.Board.Size; j++)
            {
                _buttons[i, j].Text = _controller.Board.Tiles[i, j].Number.ToString();
                _buttons[i, j].BackColor = Color.FromArgb(CountTileAlpha(i, j), _controller.TileColor);
                _buttons[i, j].Enabled = true;

                if (_controller.Board.Tiles[i, j].IsEmpty)
                {
                    _buttons[i, j].Text = "";
                    _buttons[i, j].BackColor = Color.White;
                    _buttons[i, j].Enabled = false;
                }

                Controls.Add(_buttons[i, j]);
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
