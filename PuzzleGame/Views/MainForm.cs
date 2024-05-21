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
        KeyPreview = true;
        KeyDown += new KeyEventHandler(CtrlZ);
    }

    public void SetController(BoardController? controller)
    {
        _controller = controller;
        _controller?.Board.RegisterObserver(this);
        _controller?.Board.RegisterWinObserver(this);
        
        UpdateView();
    }

    private void OnKeyPress(object? sender, KeyPressEventArgs e)
    {
        MessageBox.Show(e.KeyChar.ToString());
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
                    Text = _controller.Board.Tiles[i, j].Number.ToString(),
                    Font = new Font("Microsoft Sans Serif", 20)
                };
                if (_controller.Board.Tiles[i, j].IsEmpty)
                {
                    tileButton.Text = "";
                    tileButton.BackColor = BackColor;
                }
                else
                {
                    var row = i;
                    var col = j;
                    tileButton.Click += (_, _) => _controller.MoveTile(row, col);
                    tileButton.BackColor = Color.White;
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
        if (_controller == null) return;
        int count = _controller.Counter.Count;
        MessageBox.Show($"Congratulations! You've solved the puzzle!\nMoves: {count}\nRestarting the game.", "You Win!",
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
        var board = new Board(size, shuffleStrategy);
        var counter = new Counter(board);
        var newController = new BoardController(this, board, counter);
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
    private void CtrlZ(object? sender, KeyEventArgs e)
    {
        if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            _controller.UndoMove();
    }

}
