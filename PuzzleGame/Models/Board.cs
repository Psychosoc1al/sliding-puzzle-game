using PuzzleGame.Utilities;

namespace PuzzleGame.Models;

public class Board : IObservable
{
    private readonly List<IObserver> _observers;
    private readonly List<IWinObserver> _winObservers;
    public Status Status { get; set; }

    public int Size { get; }
    private (int, int) EmptyTile { get; set; }
    public Tile[,] Tiles { get; }

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
        var number = 1;
        for (var i = 0; i < Size; i++)
        for (var j = 0; j < Size; j++)
        {
            Tiles[i, j] = new Tile(number % (Size * Size));
            number++;
        }

        Shuffler.Shuffle(Tiles, Size);
    }


    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }
    
    public void ClearObservers()
    {
        _observers.Clear();
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers) observer.Update();
    }

    public (int, int) MoveTile(int row, int col)
    {
        if (Tiles[row, col].IsEmpty) return (0, 0);

        var (emptyRow, emptyCol) = EmptyTile;
        if (Math.Abs(emptyRow - row) + Math.Abs(emptyCol - col) != 1) return (0, 0);

        Shuffler.Swap(Tiles, row, col, emptyRow, emptyCol);
        NotifyObservers();
        EmptyTile = (row, col);

        if (CheckWin())
        {
            Status = Status.Win;
            NotifyWinObservers();
        }
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

    public void RegisterWinObserver(IWinObserver winObserver)
    {
        _winObservers.Insert(0, winObserver);
    }

    private void NotifyWinObservers()
    {
        foreach (var winObserver in _winObservers) winObserver.OnWin();
        _winObservers.Clear();
    }
}
