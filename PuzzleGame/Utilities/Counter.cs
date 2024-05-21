using PuzzleGame.Models;

namespace PuzzleGame.Utilities;

public class Counter(IObservable obs) : IObserver
{
    public int Count { get; private set; }

    public void Update()
    {
        var statusEnum = ((Board)obs).Status;
        switch (statusEnum)
        {
            case StatusEnum.StartGame:
                Count = 0;
                break;
            case StatusEnum.Move:
                Count++;
                break;
            case StatusEnum.UndoMove:
                Count--;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
