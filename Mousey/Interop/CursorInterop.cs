using System.Runtime.InteropServices;

namespace Mousey.Interop;

public class CursorInterop
{
    [DllImport("User32.Dll")]
    private static extern int GetCursorPos(ref InteropPosition pPoint);

    [DllImport("User32.Dll")]
    private static extern int SetCursorPos(int pX, int pY);

    public static bool GetCursorPosition(ref InteropPosition rPoint) => (GetCursorPos(ref rPoint) == InteropBool.True);

    public static bool SetCursorPosition(InteropPosition cPoint) =>
        (SetCursorPos(cPoint.mX, cPoint.mY) == InteropBool.False);
}