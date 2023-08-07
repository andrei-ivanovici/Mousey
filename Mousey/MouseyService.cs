using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mousey.Interop;

namespace Mousey;

public class MouseyService
{
    private CancellationTokenSource _tcs;
    public bool IsRunning { get; private set; } = false;

    public event Action<TimeSpan> OnCountdown;


    private async Task Countdown(TimeSpan interval, CancellationToken token)
    {
        var countdown = interval;
        OnCountdown?.Invoke(countdown);
        while (countdown > TimeSpan.Zero)
        {
            var initialPosition = GetCurrentPosition();
            await Task.Delay(1000, token);
            Debug.WriteLine($"Token {token.IsCancellationRequested}");
            Debug.WriteLine($"Tick");
            var newPosition = GetCurrentPosition();
            countdown = !newPosition.Equals(initialPosition)
                ? interval
                : countdown.Subtract(TimeSpan.FromSeconds(1));
            OnCountdown?.Invoke(countdown);
            token.ThrowIfCancellationRequested();
        }
    }

    private async Task TwitchMouse()
    {
        var currentPosition = GetCurrentPosition();
        var nextPosition = currentPosition with
        {
            mX = currentPosition.mX + 10
        };
        CursorInterop.SetCursorPosition(nextPosition);
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        CursorInterop.SetCursorPosition(currentPosition);
    }

    public void Start(TimeSpan interval)
    {
        _tcs = new CancellationTokenSource();
        if (interval.TotalMilliseconds <= 0)
        {
            throw new InvalidOperationException("interval is invalid");
        }

        IsRunning = true;
        Task.Run(async () =>
        {
            try
            {
                while (!_tcs.IsCancellationRequested)
                {
                    await Countdown(interval, _tcs.Token);
                    await TwitchMouse();
                }
            }
            catch (OperationCanceledException ex)
            {
                OnCountdown?.Invoke(interval);
                Debug.WriteLine($"Op Canceled {ex.Message}");
                IsRunning = false;
            }
        });
    }

    private InteropPosition GetCurrentPosition()
    {
        var position = new InteropPosition();
        CursorInterop.GetCursorPosition(ref position);
        return position;
    }

    public void Stop()
    {
        _tcs.Cancel();
        IsRunning = false;
    }
}