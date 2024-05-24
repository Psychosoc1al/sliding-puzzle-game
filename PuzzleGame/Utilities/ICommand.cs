// ReSharper disable UnusedMemberInSuper.Global

namespace PuzzleGame.Utilities;

public interface ICommand
{
    bool Execute();
    void Undo();
}
