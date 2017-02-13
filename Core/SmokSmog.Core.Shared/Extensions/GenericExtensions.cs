using System;

namespace SmokSmog.Extensions
{
    public static class GenericExtensions
    {
        public static bool Between<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;
        }
    }
}