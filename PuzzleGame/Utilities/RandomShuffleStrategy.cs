using PuzzleGame.Models;

namespace PuzzleGame.Utilities;

public class RandomShuffleStrategy : IShuffleStrategy
{
    private readonly Random _random = new();

    public void Shuffle(Tile[,] tiles, int size)
    {
        var swapCount = _random.Next((int)Math.Pow(size, 4), (int)Math.Pow(size, 5)) * 2;
        for (var i = 0; i < swapCount; i++)
        {
            var swap1 = GetRandomConsecutiveTiles(size);
            var swap2 = GetNextConsecutiveTile(swap1[0], swap1[1], size);
            Swap(tiles, swap1[0], swap1[1], swap2[0], swap2[1]);
        }
    }

    private int[] GetRandomConsecutiveTiles(int size)
    {
        var index = _random.Next(size * size - 2);
        var row = index / size;
        var col = index % size;
        return [row, col];
    }

    private static int[] GetNextConsecutiveTile(int row, int col, int size)
    {
        if (col < size - 1) return [row, col + 1];

        return row < size - 1 ? [row + 1, 0] : [0, 0];
    }

    private static void Swap(Tile[,] tiles, int i1, int j1, int i2, int j2)
    {
        (tiles[i1, j1], tiles[i2, j2]) = (tiles[i2, j2], tiles[i1, j1]);
    }
}
