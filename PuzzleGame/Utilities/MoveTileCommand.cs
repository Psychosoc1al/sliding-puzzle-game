using PuzzleGame.Models;

namespace PuzzleGame.Utilities;

public class MoveTileCommand(Board board, int row, int col) : ICommand
{
    private readonly int _row = row;
    private readonly int _col = col;
    private int _rowOld = row;
    private int _colOld = col;

    public void Execute()
    {
        (_rowOld, _colOld) = board.MoveTile(_row, _col);
    }

    public void Undo()
    {
        (_rowOld, _colOld) = board.MoveTile(_rowOld, _colOld);
    }
}
