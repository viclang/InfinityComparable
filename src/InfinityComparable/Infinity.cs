using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace InfinityComparable
{
    public readonly struct Infinity<T> : IInfinity<T>, IEquatable<Infinity<T>>, IComparable<Infinity<T>>, IComparable
        where T : struct, IEquatable<T>, IComparable<T>, IComparable
    {
        internal readonly bool positive;

        [MemberNotNullWhen(false, nameof(Finite))]
        public bool IsInfinity { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly T value;

        public bool IsPositiveInfinity() => IsInfinity && positive;

        public bool IsNegativeInfinity() => IsInfinity && !positive;

        public T? Finite => IsInfinity ? null : value;

        public static Infinity<T> NegativeInfinity => new Infinity<T>(default, true, false);

        public static Infinity<T> PositiveInfinity => new Infinity<T>(default, true, true);

        public T GetValueOrDefault() => value;

        public T GetValueOrDefault(T defaultValue) => IsInfinity ? defaultValue : value;

        public Infinity() : this(default, true, true)
        {
        }

        public Infinity(bool positive) : this(default, true, positive)
        {
        }

        public Infinity(T? value, bool positive) : this(value.GetValueOrDefault(), !value.HasValue, positive)
        {
        }

        internal Infinity(T value, bool isInfinite, bool positive)
        {
            this.value = value;
            IsInfinity = isInfinite;
            this.positive = positive;
        }

        public bool Equals(Infinity<T> other) => IsInfinity
            ? other.IsInfinity && positive == other.positive
            : !other.IsInfinity && value.Equals(other.value);

        public override bool Equals(object? other)
        {
            return other is Infinity<T> otherInfinity && Equals(otherInfinity)
                || !IsInfinity && value.Equals(other)
                || other is double otherDouble && positive.Equals(otherDouble == double.PositiveInfinity)
                || other is float otherFloat && positive.Equals(otherFloat == float.PositiveInfinity);
        }

        public override int GetHashCode() => IsInfinity
            ? positive ? 1 : -1
            : Finite.GetHashCode();

        public int CompareTo(Infinity<T> other)
        {
            if (IsInfinity)
            {
                return other.IsInfinity
                    ? positive.CompareTo(other.positive)
                    : positive ? 1 : -1;
            }
            return other.IsInfinity
                ? other.positive ? -1 : 1
                : value.CompareTo(other.value);
        }

        public static implicit operator Infinity<T>(ValueTuple<T?, bool> value) => new(value.Item1, value.Item2);
        public static implicit operator Infinity<T>(T? value) => new(value, true);
        public static implicit operator T?(Infinity<T> value) => value.Finite;

        public static Infinity<T> operator -(Infinity<T> value) => new(value.value, value.IsInfinity, false);
        public static Infinity<T> operator +(Infinity<T> value) => new(value.value, value.IsInfinity, true);
        public static Infinity<T> operator !(Infinity<T> value) => new(value.value, value.IsInfinity, !value.positive);

        public static bool operator ==(Infinity<T> left, Infinity<T> right) => left.Equals(right);
        public static bool operator !=(Infinity<T> left, Infinity<T> right) => !left.Equals(right);
        public static bool operator >(Infinity<T> left, Infinity<T> right) => left.CompareTo(right) == 1;
        public static bool operator <(Infinity<T> left, Infinity<T> right) => left.CompareTo(right) == -1;
        public static bool operator >=(Infinity<T> left, Infinity<T> right) => left == right || left > right;
        public static bool operator <=(Infinity<T> left, Infinity<T> right) => left == right || left < right;

        public int CompareTo(object? other)
        {
            if (other is Infinity<T> otherInfinity)
            {
                return CompareTo(otherInfinity);
            }
            if (IsInfinity)
            {
                if (other is double otherDouble)
                {
                    return positive.CompareTo(otherDouble == double.PositiveInfinity);
                }
                if (other is float otherFloat)
                {
                    return positive.CompareTo(otherFloat == double.PositiveInfinity);
                }
                return positive ? 1 : -1;
            }
            return value.CompareTo(other);
        }

        public override string? ToString() => ToString(x => x.ToString(), Infinity.InfinityToString);

        public string? ToString(Func<T, string?> finiteToString, Func<bool, string?> infinityToString)
            => IsInfinity ? infinityToString(positive) : finiteToString(value);

        public void Deconstruct(out T? value, out bool positive)
        {
            value = Finite;
            positive = this.positive;
        }
    }
}