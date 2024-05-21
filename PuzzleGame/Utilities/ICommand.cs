// ReSharper disable UnusedMemberInSuper.Global

namespace PuzzleGame.Utilities;

public interface ICommand
{
    void Execute();
    void Undo();
}
