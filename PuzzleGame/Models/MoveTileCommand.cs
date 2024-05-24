using PuzzleGame.Utilities;

namespace PuzzleGame.Models;

public class MoveTileCommand(Board board, int row, int col) : ICommand
{
    private readonly int _row = row;
    private readonly int _col = col;
    private int _rowOld = row;
    private int _colOld = col;

    public bool Execute()
    {
        (_rowOld, _colOld) = board.MoveTile(_row, _col);

        return (_rowOld, _colOld) != (-1, -1);
    }

    public void Undo()
    {
        (_rowOld, _colOld) = board.MoveTile(_rowOld, _colOld);
    }
}
