using System.Drawing;
using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public class DsRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public DsRect()
        {
            left = 0;
            top = 0;
            right = 0;
            bottom = 0;
        }

        public DsRect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public DsRect(Rectangle rectangle)
        {
            left = rectangle.Left;
            top = rectangle.Top;
            right = rectangle.Right;
            bottom = rectangle.Bottom;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1} - {2}, {3}]", this.left, this.top, this.right, this.bottom);
        }

        public override int GetHashCode()
        {
            return left.GetHashCode() |
                   top.GetHashCode() |
                   right.GetHashCode() |
                   bottom.GetHashCode();
        }

        public static implicit operator Rectangle(DsRect r)
        {
            return r.ToRectangle();
        }

        public static implicit operator DsRect(Rectangle r)
        {
            return new DsRect(r);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(left, top, (right - left), (bottom - top));
        }

        public static DsRect FromRectangle(Rectangle r)
        {
            return new DsRect(r);
        }
    }
}