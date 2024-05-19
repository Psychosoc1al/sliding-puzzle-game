using PuzzleGame.Models;

namespace PuzzleGame.Utilities;

public class MoveTileCommand(Board board, int row, int col) : ICommand
{
    public void Execute()
    {
        board.MoveTile(row, col);
    }

    public void Undo()
    {
        // Implement undo logic if needed
    }
}
