using System;

namespace BaseUtil.Base
{
    public static class FnVal
    {
        public static Func<string, int> AsInt = (string text) =>
        {
            return SafeGet(0, () => int.Parse(text));
        };

        public static T SafeGet<T>(T defaultValue, Func<T> getFn)
        {
            try
            {
                return getFn();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static Func<string, Func<string, bool>> StartsWith = (string part) =>
        {
            return (string whole) =>
            {
                if (whole == null) return false;
                if (part == null) return false;
                return whole.StartsWith(part);
            };
        };

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

        public static int Clamp(int current, int min, int max)
        {
            return AtMost(max, AtLeast(min, current));
        }

        public static bool RandomBool(Random newRandom)
        {
            return newRandom.NextDouble() >= 0.5;
        }

        public static int GetNextIndex(int currentIndex, int size)
        {
            return (currentIndex + 1) % size;
        }

        public static int GetPreviousIndex(int currentIndex, int size)
        {
            return (currentIndex + size - 1) % size;
        }
    }
}