namespace PuzzleGame.Models;

public class Tile(int number)
{
    public int Number { get; } = number;
    public bool IsEmpty => Number == 0;
}
