using PuzzleGame.Models;
using PuzzleGame.Views;
using Timer = System.Windows.Forms.Timer;

namespace PuzzleGame.Utilities;

public class TimeCountStrategy(Board boardObservable, MainForm mainForm) : IWinObserver, IStrategy, IObserver
{
    private readonly Timer _timer = new();
    private int _elapsedTimeDecimals;

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _elapsedTimeDecimals++;
        mainForm.SetCount($"{_elapsedTimeDecimals / 10.0:F1} c");
    }

    public void Execute()
    {
        boardObservable.RegisterWinObserver(this);
        mainForm.SetCount("0.0 c");
        mainForm.SetCountType("Время:");
        
        boardObservable.RegisterObserver(this);
        boardObservable.RegisterWinObserver(this);

        _timer.Interval = 100;
        _timer.Tick += Timer_Tick;
    }

    public void Update()
    {
        if (!_timer.Enabled)
        {
            _timer.Start();
            _elapsedTimeDecimals = 0;
        }
    }

    public void OnWin()
    {
        _timer.Stop();
    }
}
