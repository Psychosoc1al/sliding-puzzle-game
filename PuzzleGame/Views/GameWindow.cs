using PuzzleGame.Models;

namespace PuzzleGame.Views;

public class GameWindow : Form
{
    private readonly Label _countTypeLabel;
    private readonly Label _countLabel;
    private Button[,] _buttons = new Button[0, 0];

    public event EventHandler CtrlZEvent = delegate { };
    public event EventHandler<MoveEventArgs> BtnClickEvent = delegate { };

    public GameWindow()
    {
        _countTypeLabel = new Label
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
            Text = "0",
            TextAlign = ContentAlignment.TopRight,
            Top = 0,
            Left = 220,
            Height = 100,
            Width = 150,
            Font = new Font("Comfortaa", 15, FontStyle.Bold)
        };

        KeyPreview = true;
        KeyDown += (_, e) =>
        {
            if (e is { Modifiers: Keys.Control, KeyCode: Keys.Z })
                CtrlZEvent.Invoke(this, EventArgs.Empty);
        };

        InitializeComponent();
    }

    public void CreateButtons(int boardSize, Tile[,] tiles)
    {
        const int offset = 50;
        var buttonSize = ClientSize.Width / boardSize;
        var buttonFont = new Font("Comfortaa", (int)(60.0 / boardSize), FontStyle.Bold);
        _buttons = new Button[boardSize, boardSize];

        for (var i = 0; i < boardSize; i++)
        for (var j = 0; j < boardSize; j++)
        {
            _buttons[i, j] = new Button
            {
                Width = buttonSize,
                Height = buttonSize,
                Left = j * buttonSize,
                Top = i * buttonSize + offset,
                Text = tiles[i, j].Number.ToString(),
                Font = buttonFont
            };

            var (row, col) = (i, j);
            _buttons[i, j].Click += (_, _) => BtnClickEvent(this, new MoveEventArgs(row, col));

            Controls.Add(_buttons[i, j]);
        }
    }

    public void UpdateView(int boardSize, Tile[,] tiles, Color tileColor)
    {
        Controls.Clear();

        for (var i = 0; i < boardSize; i++)
        for (var j = 0; j < boardSize; j++)
        {
            _buttons[i, j].Text = tiles[i, j].Number.ToString();
            _buttons[i, j].BackColor = Color.FromArgb(CountTileAlpha(boardSize, tiles, i, j), tileColor);
            _buttons[i, j].Enabled = true;

            if (tiles[i, j].IsEmpty)
            {
                _buttons[i, j].Text = "";
                _buttons[i, j].BackColor = Color.White;
                _buttons[i, j].Enabled = false;
            }

            Controls.Add(_buttons[i, j]);
        }

        Controls.Add(_countTypeLabel);
        Controls.Add(_countLabel);
    }

    private static int CountTileAlpha(int boardSize, Tile[,] tiles, int row, int col)
    {
        const int offset = 20;
        const double multiplier = 150;

        return (int)(tiles[row, col].Number * multiplier / (boardSize * boardSize) + offset);
    }

    public void SetCountType(string countType)
    {
        _countTypeLabel.Text = countType;
    }

    public void SetCount(string count)
    {
        _countLabel.Text = count;
    }

    private void InitializeComponent()
    {
        MinimumSize = new Size(500, 583);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Name = "GameWindow";
        Text = "Пятнашки";
        DoubleBuffered = true;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
    }
}

public class MoveEventArgs(int row, int col) : EventArgs
{
    public int Row { get; } = row;
    public int Col { get; } = col;
}
