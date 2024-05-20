using PuzzleGame.Controllers;
using PuzzleGame.Models;

namespace PuzzleGame.Utilities;

public class Counter : IObserver
{
    public int Count { get; set; }
    private IObservable _obs;

    public Counter(IObservable obs)
    {
        _obs = obs;
        Count = 0;
    }
    public void Update()
    {
        StatusEnum statusEnum = ((Board)_obs).Status;
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
        }
    }
}