using PuzzleGame.Models;

namespace PuzzleGame.Utilities;

public class MoveTileCommand: ICommand
{
    private Board _board;
    private int _row;
    private int _col;
    private int _row_old;
    private int _col_old;
    public MoveTileCommand(Board board, int row, int col)
    {
        _board = board;
        _row = row;
        _col = col;
        _row_old = row;
        _col_old = col;
}
    public void Execute()
    {
        (_row_old, _col_old) = _board.MoveTile(_row, _col);
    }

    public void Undo()
    {
        (_row_old, _col_old) = _board.MoveTile(_row_old, _col_old);
    }
}
