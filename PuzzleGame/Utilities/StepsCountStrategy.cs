using PuzzleGame.Models;
using PuzzleGame.Views;

namespace PuzzleGame.Utilities;

public class StepsCountStrategy(Board boardObservable, MainForm mainForm) : IObserver, IStrategy
{
    private int Count { get; set; }

    public void Update()
    {
        var statusEnum = boardObservable.Status;
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

        mainForm.SetCount(Count.ToString());
    }

    public void Execute()
    {
        boardObservable.RegisterObserver(this);
        mainForm.SetCount("0");
        mainForm.SetCountType("Шаги:");
    }
}
