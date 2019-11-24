using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locksmith.Data
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x; Y = y;
        }
        public int X;
        public int Y;
    }

    public static class Utility
    {
        public static Point GetPoint(byte Position)
        {
            return new Point(GetXPosition(Position), GetYPosition(Position));
        }

        public static int GetXPosition(byte Position)
        {
            return Position & 0x0F;
        }
        public static int GetYPosition(byte Position)
        {
            return (Position & 0xF0) >> 4;
        }
        public static bool IsItemHidden(byte itemIndex)
        {
            return !((itemIndex & 0x40) >> 6 == 0);
        }
        public static bool IsItemInBlock(byte itemIndex)
        {
            return !((itemIndex & 0x80) >> 7 == 0);
        }
    }
}
