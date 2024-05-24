// ReSharper disable UnusedMemberInSuper.Global

namespace PuzzleGame.Utilities;

public interface IObservable
{
    void RegisterObserver(IObserver observer);
    void NotifyObservers();
}
