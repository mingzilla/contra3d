using System;

namespace BaseUtil.Base
{
    public static class FnVal
    {
        public static Func<string, int> AsInt = (string text) =>
        {
            try
            {
                return int.Parse(text);
            }
            catch (Exception)
            {
                return 0;
            }
        };

        public static Func<string, Func<string, bool>> StartsWith = (string part) =>
        {
            return (string whole) =>
            {
                if (whole == null) return false;
                if (part == null) return false;
                return whole.StartsWith(part);
            };
        };

        public static int GetNextCircularIndex(int currentIndex, int size)
        {
            if (size == 0) throw new Exception("GetNextCircularIndex doesn't allow empty array");
            if (currentIndex + 1 == size) return 0;
            return currentIndex + 1;
        }

        public static int AtLeast(int minValue, int value)
        {
            if (value < minValue) return minValue;
            return value;
        }

        public static int AtMost(int maxValue, int value)
        {
            if (value > maxValue) return maxValue;
            return value;
        }

        public static float AtLeastF(float minValue, float value)
        {
            if (value < minValue) return minValue;
            return value;
        }

        public static float AtMostF(float maxValue, float value)
        {
            if (value > maxValue) return maxValue;
            return value;
        }
    }
}