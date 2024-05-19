using PuzzleGame.Models;

namespace PuzzleGame.Utilities;

public interface IShuffleStrategy
{
    void Shuffle(Tile[,] tiles, int size);
}
