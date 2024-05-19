using PuzzleGame.Controllers;

namespace PuzzleGame.Utilities;

public interface IBoardView : IObserver
{
    void SetController(BoardController? controller);
    void UpdateView();
}
