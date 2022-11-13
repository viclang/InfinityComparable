namespace InfinityComparable
{
    public static partial class Infinity
    {
        public static void Match<T, TResult>(this Infinity<T> value, Action<T> finite, Action<bool> infinite)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
        {
            if (value.IsInfinite)
            {
                infinite(value.positive);
            }
            else
            {
                finite(value.value);
            }
        }

        public static TResult Match<T, TResult>(this Infinity<T> value, Func<T, TResult> finite, Func<bool, TResult> infinite)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
            => value.IsInfinite
                ? infinite(value.positive)
                : finite(value.value);


        public static Task MatchAsync<T, TResult>(this Infinity<T> value, Func<T, Task> finite, Func<bool, Task> infinite)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
            => value.IsInfinite
                ? infinite(value.positive)
                : finite(value.value);

        public static Task<TResult> MatchAsync<T, TResult>(this Infinity<T> value, Func<T, Task<TResult>> finite, Func<bool, Task<TResult>> infinite)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
            => value.IsInfinite
                ? infinite(value.positive)
                : finite(value.value);
    }
}
