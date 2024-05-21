using PuzzleGame.Utilities;

namespace PuzzleGame.Models;

public class Board : IObservable
{
    private readonly List<IObserver> _observers;
    private readonly List<IWinObserver> _winObservers;
    public StatusEnum Status { get; set; }

    public int Size { get; }

    public Tile[,] Tiles { get; }

    public Board(int size)
    {
        Size = size;
        Tiles = new Tile[size, size];
        _observers = new List<IObserver>();
        _winObservers = new List<IWinObserver>();
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        var number = 1;
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                Tiles[i, j] = new Tile(number % (Size * Size));
                number++;
            }
        }

        Shuffler.Shuffle(Tiles, Size);
    }


    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers) observer.Update();
    }


    public (int, int) MoveTile(int row, int col)
    {
        if (Tiles[row, col].IsEmpty) return (0,0);

        var (emptyRow, emptyCol) = FindEmptyTile();
        if (Math.Abs(emptyRow - row) + Math.Abs(emptyCol - col) != 1) return (0,0);

        Shuffler.Swap(Tiles, row, col, emptyRow, emptyCol);
        NotifyObservers();

        if (CheckWin()) NotifyWinObservers();
        return (emptyRow, emptyCol);
    }


    private (int, int) FindEmptyTile()
    {
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
                if (Tiles[i, j].IsEmpty)
                    return (i, j);
        }

        return (-1, -1);
    }

    private bool CheckWin()
    {
        var expectedNumber = 1;
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                if (i == Size - 1 && j == Size - 1) break;
                if (Tiles[i, j].Number != expectedNumber) return false;
                expectedNumber++;
            }
        }

        return true;
    }

    public void RegisterWinObserver(IWinObserver winObserver)
    {
        _winObservers.Clear();
        _winObservers.Add(winObserver);
    }

    public void RemoveWinObserver(IWinObserver winObserver)
    {
        _winObservers.Remove(winObserver);
    }

    private void NotifyWinObservers()
    {
        foreach (var winObserver in _winObservers) winObserver.OnWin();
    }
}
