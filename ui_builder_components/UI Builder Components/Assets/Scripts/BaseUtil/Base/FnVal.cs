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

        public static int RandomIntBetween(Random newRandom, int low, int high)
        {
            return newRandom.Next(low, high);
        }

        public static float RandomFloatBetween(Random newRandom, float low, float high)
        {
            return (float) RandomDoubleBetween(newRandom, low, high);
        }

        public static float RandomFloatBetween(float low, float high)
        {
            return RandomFloatBetween(new Random(), low, high);
        }

        public static double RandomDoubleBetween(Random newRandom, float low, float high)
        {
            return newRandom.NextDouble() * (high - low) + low;
        }

        public static int GetNextIndex(int currentIndex, int size)
        {
            return (currentIndex + 1) % size;
        }

        public static int GetPreviousIndex(int currentIndex, int size)
        {
            return (currentIndex + size - 1) % size;
        }

        public static bool IsBetweenF(float low, float high, bool isInclusive, float num)
        {
            if (isInclusive) return low <= num && num <= high;
            return low < num && num < high;
        }

        /// <summary>
        /// Converts value to -1, 0, 1 because axis accepts these 3 as rounded values
        /// </summary>
        /// <param name="value">actual value</param>
        /// <param name="dividingValue">e.g. 0.3f, at what point to round up or down</param>
        /// <returns></returns>
        public static float RoundAxisValue(float value, float dividingValue)
        {
            if (value > dividingValue) return 1f;
            if (value < -(dividingValue)) return -1f;
            return 0f;
        }
    }
}