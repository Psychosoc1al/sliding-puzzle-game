using PuzzleGame.Utilities;

namespace PuzzleGame.Models;

public class Board : IObservable
{
    private readonly List<IObserver> _observers;
    private readonly Random _random = new();
    private readonly List<IWinObserver> _winObservers;

    public Board(int size)
    {
        Size = size;
        Tiles = new Tile[size, size];
        _observers = new List<IObserver>();
        _winObservers = new List<IWinObserver>();
        InitializeBoard();
    }

    public int Size { get; }

    public Tile[,] Tiles { get; }


    public void RegisterObserver(IObserver observer)
    {
        _observers.Clear();
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

        Shuffle();
    }

    private void Shuffle()
    {
        var swapCount = _random.Next(10, 51) * 2;
        for (var i = 0; i < swapCount; i++)
        {
            var swap1 = GetRandomConsecutiveTiles();
            var swap2 = GetNextConsecutiveTile(swap1[0], swap1[1]);
            Swap(swap1[0], swap1[1], swap2[0], swap2[1]);
        }
    }

    private int[] GetRandomConsecutiveTiles()
    {
        var index = _random.Next(Size * Size - 2);
        var row = index / Size;
        var col = index % Size;
        return [row, col];
    }

    private int[] GetNextConsecutiveTile(int row, int col)
    {
        if (col < Size - 1) return [row, col + 1];

        return row < Size - 1 ? [row + 1, 0] : [row, col];
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
