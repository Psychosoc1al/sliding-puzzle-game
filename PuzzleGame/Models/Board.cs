using PuzzleGame.Utilities;

namespace PuzzleGame.Models;

public class Board : IObservable
{
    private readonly List<IObserver> _observers;
    private readonly List<IWinObserver> _winObservers;
    private readonly IShuffleStrategy _shuffleStrategy;
    public StatusEnum Status { get; set; }

    public int Size { get; }

    public Tile[,] Tiles { get; }

    public Board(int size, IShuffleStrategy shuffleStrategy)
    {
        Size = size;
        Tiles = new Tile[size, size];
        _observers = new List<IObserver>();
        _winObservers = new List<IWinObserver>();
        _shuffleStrategy = shuffleStrategy;
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

        _shuffleStrategy.Shuffle(Tiles, Size);
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

    private void Swap(int i1, int j1, int i2, int j2)
    {
        (Tiles[i1, j1], Tiles[i2, j2]) = (Tiles[i2, j2], Tiles[i1, j1]);
    }


    public void MoveTile(int row, int col)
    {
        if (Tiles[row, col].IsEmpty) return;
        var (emptyRow, emptyCol) = FindEmptyTile();
        if (Math.Abs(emptyRow - row) + Math.Abs(emptyCol - col) != 1) return;
        Swap(row, col, emptyRow, emptyCol);
        NotifyObservers();
        if (CheckWin()) NotifyWinObservers();
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
