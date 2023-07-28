using System.Runtime.InteropServices;

namespace Mousey.Interop;

[StructLayout(LayoutKind.Sequential)]
public struct InteropPosition
{
    public int mX;
    public int mY;

    public bool Equals(InteropPosition otherPosition)
    {
        return mX == otherPosition.mX
               && mY == otherPosition.mY;
    }
}