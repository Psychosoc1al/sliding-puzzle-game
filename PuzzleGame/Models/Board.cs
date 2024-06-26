using PuzzleGame.Utilities;

namespace PuzzleGame.Models;

public class Board : IObservable
{
    private readonly List<IObserver> _observers;
    private readonly List<IWinObserver> _winObservers;
    public int Size { get; }
    public Status Status { get; set; }
    public Tile[,] Tiles { get; }
    private (int, int) EmptyTile { get; set; }

    public Board(int size)
    {
        Size = size;
        EmptyTile = (size - 1, size - 1);
        Tiles = new Tile[size, size];
        _observers = new List<IObserver>();
        _winObservers = new List<IWinObserver>();

        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (var i = 0; i < Size; i++)
        for (var j = 0; j < Size; j++)

            Tiles[i, j] = new Tile((i * Size + j + 1) % (Size * Size));


        Shuffler.Shuffle(Tiles, Size);
    }

    public (int, int) MoveTile(int row, int col)
    {
        var (emptyRow, emptyCol) = EmptyTile;
        if (Math.Abs(emptyRow - row) + Math.Abs(emptyCol - col) != 1) return (-1, -1);

        Shuffler.Swap(Tiles, row, col, emptyRow, emptyCol);
        NotifyObservers();
        EmptyTile = (row, col);

        if (!CheckWin()) return (emptyRow, emptyCol);
        Status = Status.Win;
        NotifyWinObservers();

        return (emptyRow, emptyCol);
    }

    private bool CheckWin()
    {
        var expectedNumber = 1;
        for (var i = 0; i < Size; i++)
        for (var j = 0; j < Size; j++)
        {
            if (i == Size - 1 && j == Size - 1) break;
            if (Tiles[i, j].Number != expectedNumber) return false;
            expectedNumber++;
        }

        return true;
    }

    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers) observer.Update();
    }

    public void RegisterWinObserver(IWinObserver winObserver)
    {
        _winObservers.Insert(0, winObserver);
    }

    private void NotifyWinObservers()
    {
        foreach (var winObserver in _winObservers) winObserver.OnWin();

        _winObservers.Clear();
        _observers.Clear();
    }
}
