using PuzzleGame.Controllers;
using PuzzleGame.Models;
using PuzzleGame.Utilities;

namespace PuzzleGame.Views;

public class MainForm : Form
{
    private readonly Label _countTypeLabel;
    private readonly Label _countLabel;
    private Font? _tileFont;
    private Button[,]? _buttons;

    public event EventHandler CtrlZEvent;
    public event EventHandler<CustomEventArgs> BtnClickEvent;

    public MainForm()
    {
        InitializeComponent();
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
            Left = 270,
            Height = 100,
            Font = new Font("Comfortaa", 15, FontStyle.Bold)
        };

        KeyPreview = true;
        KeyDown += (s, e) =>
        {
            if (e is { Modifiers: Keys.Control, KeyCode: Keys.Z })
                CtrlZEvent?.Invoke(this, EventArgs.Empty);
        };


    }

    public void CreateButtons(int board_size, Tile[,] tiles)
    {
        const int offset = 50;
        var tileSize = ClientSize.Width / board_size;
        _tileFont ??= new Font("Comfortaa", (int)(60.0 / board_size)!, FontStyle.Bold);
        var size = board_size;
        _buttons = new Button[size, size];
        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
                _buttons[i, j] = new Button
                {
                    Width = tileSize,
                    Height = tileSize,
                    Left = j * tileSize,
                    Top = i * tileSize + offset,
                    Text = tiles[i, j].Number.ToString(),
                    Font = _tileFont
                };

                var row = i;
                var col = j;
                _buttons[i, j].Click += (s, e) => OnButtonClick(row, col);
                
                Controls.Add(_buttons[i, j]);
            }

    }

    public void UpdateView(int board_size, Tile[,] tiles, Color tile_color)
    {
        Controls.Clear();
        for (var i = 0; i < board_size; i++)
            for (var j = 0; j < board_size; j++)
            {
                _buttons[i, j].Text = tiles[i, j].Number.ToString();
                _buttons[i, j].BackColor = Color.FromArgb(CountTileAlpha(board_size, tiles, i, j), tile_color);
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

    private void OnButtonClick(int row, int col)
    {
        EventHandler<CustomEventArgs> handler = BtnClickEvent;
        if (handler != null)
        {
            handler(this, new CustomEventArgs(row, col));
        }
    }

    private int CountTileAlpha(int board_size, Tile[,] tiles, int row, int col)
    {
        const int offset = 20;
        const double multiplier = 150;
        var size = board_size;

        return (int)(tiles[row, col].Number * multiplier / (size * size) + offset);
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

public class CustomEventArgs : EventArgs
{
    public CustomEventArgs(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int Row { get; }
    public int Col { get; }
}