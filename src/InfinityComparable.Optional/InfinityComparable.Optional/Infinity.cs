using LanguageExt;
using System.ComponentModel;
using System.Diagnostics;

namespace InfinityComparable
{
    public readonly struct Infinity<T> : IEquatable<Infinity<T>>, IComparable<Infinity<T>>, IComparable
        where T : struct, IComparable<T>, IComparable
    {
        private readonly bool positive;

        public bool IsInfinite() => Value.IsNone;

        public Option<T> Value { get; }

        public static readonly Infinity<T> NegativeInfinity = new(Option<T>.None, false);
        public static readonly Infinity<T> PositiveInfinity = new(Option<T>.None, true);

        public Infinity() : this(Option<T>.None, true)
        {
        }

        public Infinity(bool positive) : this(Option<T>.None, positive)
        {
        }

        public Infinity(Option<T> value, bool positive)
        {
            Value = value;
            this.positive = positive;
        }

        public bool Equals(Infinity<T> other)
        {
            return Value.Match(
                value => other.Value.IsSome && other.Value.Equals(value),
                other.Value.IsNone && positive.Equals(other.positive)
                );
        }

        public override bool Equals(object? other)
        {
            return other is Infinity<T> otherInfinity && Equals(otherInfinity)
                || Value.Match(
                    value => value.Equals(other),
                    other is double otherDouble && positive.Equals(otherDouble == double.PositiveInfinity)
                        || other is float otherFloat && positive.Equals(otherFloat == float.PositiveInfinity)
                );
        }

        public override int GetHashCode() => Value.Match(
            value => value.GetHashCode(),
            positive ? 1 : -1
            );

        public int CompareTo(Infinity<T> other)
        {
            return Value.Match(
                value => other.Value.Match(
                    otherValue => value.CompareTo(otherValue),
                    other.positive ? -1 : 1),
                other.Value.IsSome
                    ? positive ? 1 : -1
                    : positive.CompareTo(other.positive)
                );
        }

        public int CompareTo(object? other)
        {
            return other is Infinity<T> otherInfinity
                ? CompareTo(otherInfinity)
                : Value.Match(
                    value => value.CompareTo(other),
                    other is double otherDouble
                    ? positive.CompareTo(otherDouble == double.PositiveInfinity)
                    : other is float otherFloat
                        ? positive.CompareTo(otherFloat == float.PositiveInfinity)
                        : positive ? 1 : -1
                );
        }

        public override string ToString() => Value.Match(
            value => string.Empty + value.ToString(),
            positive ? "Infinity" : "-Infinity");

        public static implicit operator Infinity<T>(ValueTuple<T?, bool> value) => new(value.Item1.ToOption(), value.Item2);
        public static implicit operator Infinity<T>(T? value) => new(value.ToOption(), true);
        public static implicit operator T?(Infinity<T> value) => value.Value.ToNullable();

        public static implicit operator Infinity<T>(ValueTuple<Option<T>, bool> value) => new(value.Item1, value.Item2);
        public static implicit operator Infinity<T>(Option<T> value) => new(value, true);
        public static implicit operator Option<T>(Infinity<T> value) => value.Value;

        public static Infinity<T> operator -(Infinity<T> value) => new(value.Value, false);
        public static Infinity<T> operator +(Infinity<T> value) => new(value.Value, true);
        public static Infinity<T> operator !(Infinity<T> value) => new(value.Value, !value.positive);

        public static bool operator ==(Infinity<T> left, Infinity<T> right) => left.Equals(right);
        public static bool operator !=(Infinity<T> left, Infinity<T> right) => !left.Equals(right);
        public static bool operator >(Infinity<T> left, Infinity<T> right) => left.CompareTo(right) == 1;
        public static bool operator <(Infinity<T> left, Infinity<T> right) => left.CompareTo(right) == -1;
        public static bool operator >=(Infinity<T> left, Infinity<T> right) => left == right || left > right;
        public static bool operator <=(Infinity<T> left, Infinity<T> right) => left == right || left < right;

        public void Deconstruct(out Option<T> value, out bool positive)
        {
            value = Value;
            positive = this.positive;
        }
    }

    public static class Infinity
    {
        public static Infinity<T> Inf<T>(T? value = null, bool positive = true) where T : struct, IComparable<T>, IComparable
            => new(value.ToOption(), positive);

        public static Infinity<T> Inf<T>(bool positive) where T : struct, IComparable<T>, IComparable
            => new(positive);

        public static Infinity<T> Inf<T>() where T : struct, IComparable<T>, IComparable
            => new();

        public static Infinity<T> ToInfinity<T>(this T? value, bool positive) where T : struct, IComparable<T>, IComparable
            => new(value.ToOption(), positive);

        public static Infinity<T> ToPositiveInfinity<T>(this T? value) where T : struct, IComparable<T>, IComparable
            => new(value.ToOption(), true);

        public static Infinity<T> ToNegativeInfinity<T>(this T? value) where T : struct, IComparable<T>, IComparable
            => new(value.ToOption(), false);
    }
}