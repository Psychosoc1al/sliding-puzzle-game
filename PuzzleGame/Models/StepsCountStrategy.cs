using PuzzleGame.Utilities;
using PuzzleGame.Views;

namespace PuzzleGame.Models;

public class StepsCountStrategy(Board boardObservable, GameWindow gameWindow) : IObserver, IStrategy
{
    private int Count { get; set; }

    public void Update()
    {
        var statusEnum = boardObservable.Status;
        switch (statusEnum)
        {
            case Status.StartGame:
                Count = 0;
                break;
            case Status.Move:
                Count++;
                break;
            case Status.UndoMove:
                Count--;
                break;
            case Status.Win:
            default:
                break;
        }

        gameWindow.SetCount(Count.ToString());
    }

    public void Execute()
    {
        boardObservable.RegisterObserver(this);
        gameWindow.SetCount("0");
        gameWindow.SetCountType("Шаги:");
    }
}
