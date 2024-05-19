namespace PuzzleGame.Models
{
    public class Tile
    {
        public int Number { get; set; }
        public bool IsEmpty => Number == 0;

        public Tile(int number)
        {
            Number = number;
        }
    }
}
