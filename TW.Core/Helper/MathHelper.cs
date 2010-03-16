using System;

namespace TW.Core.Helper
{
    public class MathHelper
    {
        public static double Log2(double value)
        {
            return Math.Log10(value)/Math.Log10(2);
        }

        public static int Clip(int min, int max, double value)
        {
            value = Math.Ceiling(value);
            value = Math.Max(min, value);
            value = Math.Min(max, value);

            return (int) value;
        }

        public static int Clip(int min, int max, int value)
        {
            value = Math.Max(min, value);
            value = Math.Min(max, value);

            return value;
        }

        public static int Clip(int max, double value)
        {
            value = Math.Ceiling(value);
            value = Math.Max(0, value);
            value = Math.Min(max, value);

            return (int) value;
        }

        public static int Clip(int max, int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(max, value);

            return value;
        }

        public static int RshiftRound(int value, int amount)
        {
            return ((value + (1 << (amount - 1))) >> amount);
        }
    }
}