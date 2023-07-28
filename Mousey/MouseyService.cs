using System;
using System.Threading;
using System.Threading.Tasks;
using Mousey.Interop;

namespace Mousey;

public class MouseyService
{
    private CancellationTokenSource _tcs;
    public bool IsRunning { get; private set; } = false;


    public void Start(TimeSpan inteval)
    {
        ;
        _tcs = new CancellationTokenSource();
        if (inteval.TotalMilliseconds <= 0)
        {
            throw new InvalidOperationException("interval is invalid");
        }

        IsRunning = true;
        Task.Run(async () =>
        {
            while (!_tcs.IsCancellationRequested)
            {
                var initialPosition = GetCurrentPosition();
                await Task.Delay(inteval, _tcs.Token);
                var currentPosition = GetCurrentPosition();
                if (!initialPosition.Equals(currentPosition))
                {
                    continue;
                }

                var nextPosition = currentPosition with
                {
                    mX = currentPosition.mX + 10
                };
                CursorInterop.SetCursorPosition(nextPosition);
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                CursorInterop.SetCursorPosition(currentPosition);
            }

            IsRunning = false;
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
    }
}