using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace InfinityComparable
{
    public readonly struct Infinity<T> : IEquatable<Infinity<T>>, IComparable<Infinity<T>>, IComparable
        where T : notnull, IComparable<T>, IComparable
    {
        internal readonly bool IsPositive;
        public bool HasValue { get; }
        public bool IsInfinite => !HasValue;

        internal readonly T Value;
        //public T? Finite => IsSome ? value : null;
        public T ValueOrDefault() => Value;
        public T ValueOr(T other) => HasValue ? Value : other;

        public static Infinity<T> Some(T value) =>
            value is null
            ? throw new ArgumentNullException()
            : new Infinity<T>(value, true, true);

        public static readonly Infinity<T> NegativeInfinity = new(false);
        public static readonly Infinity<T> PositiveInfinity = new(true);

        internal Infinity(bool positive) : this(default!, false, positive)
        {
        }

        internal Infinity(T value, bool hasValue, bool positive)
        {
            Value = value;
            HasValue = hasValue;
            this.IsPositive = positive;
        }

        public TResult Match<TResult>(Func<T, TResult> value, Func<TResult> infinity) where TResult : notnull
            => HasValue ? value(Value) : infinity();

        public TResult Match<TResult>(Func<T, TResult> value, TResult infinity) where TResult : notnull
            => HasValue ? value(Value) : infinity;

        public TResult Match<TResult>(Func<T, TResult> finite, Func<bool, TResult> infinity) where TResult : notnull
            => HasValue ? finite(Value) : infinity(IsPositive);

        public TResult Match<TResult>(Func<T, TResult> value, TResult positiveInfinity, TResult negativeInfinity) where TResult : notnull
            => HasValue ? value(Value) : IsPositive ? positiveInfinity : negativeInfinity;

        public Infinity<TResult> Map<TResult>(Func<T, TResult> value) where TResult : struct, IComparable<TResult>, IComparable
            => HasValue ? new(value(Value), false, true) : new();

        public Infinity<TResult> Map<TResult>(Func<T, TResult> value, Func<bool, TResult> infinity) where TResult : struct, IComparable<TResult>, IComparable
            => HasValue ? new(value(Value), true, true) : new(infinity(IsPositive), true, true);

        public Infinity<TResult> Map<TResult>(Func<T, TResult> value, TResult infinity) where TResult : struct, IComparable<TResult>, IComparable
            => HasValue ? new(value(Value), true, true) : new(infinity, true, true);

        public Infinity<TResult> Map<TResult>(Func<T, TResult> value, TResult positiveValue, TResult negativeValue) where TResult : struct, IComparable<TResult>, IComparable
            => HasValue ? Infinity<TResult>.Some(value(Value)) : IsPositive ? Infinity<TResult>.Some(positiveValue) : Infinity<TResult>.Some(negativeValue);
        
        public Infinity<TResult> Bind<TResult>(Func<T, Infinity<TResult>> f) where TResult : struct, IComparable<TResult>, IComparable
            => IsPositive
               ? f(Value)
               : new();

        public Infinity<TResult> BiBind<TResult>(Func<T, Infinity<TResult>> value, Func<bool, Infinity<TResult>> infinity) where TResult : struct, IComparable<TResult>, IComparable
            => HasValue
                ? value(Value)
                : infinity(IsPositive);

        public bool Equals(Infinity<T> other) => HasValue
            ? other.HasValue && Value.Equals(other.Value)
            : !other.HasValue && IsPositive.Equals(other.IsPositive);

        public override bool Equals(object? other)
        {
            return other is Infinity<T> otherInfinity && Equals(otherInfinity)
                || HasValue && Value.Equals(other)
                || other is double otherDouble && IsPositive.Equals(otherDouble == double.PositiveInfinity)
                || other is float otherFloat && IsPositive.Equals(otherFloat == float.PositiveInfinity);
        }

        public override int GetHashCode() => HasValue
            ? Value.GetHashCode()
            : IsPositive ? 1 : -1;

        public int CompareTo(Infinity<T> other)
        {
            if(HasValue)
            {
                return other.HasValue
                    ? Value.CompareTo(other.Value)
                    : other.IsPositive ? -1 : 1;
            }
            return other.HasValue
                ? IsPositive ? 1 : -1
                : IsPositive.CompareTo(other.IsPositive);
        }

        public int CompareTo(object? other)
        {
            if (other is Infinity<T> otherInfinity)
            {
                return CompareTo(otherInfinity);
            }
            if (HasValue)
            {
                return Value.CompareTo(other);
            }
            if (other is double otherDouble)
            {
                return IsPositive.CompareTo(otherDouble == double.PositiveInfinity);
            }
            if (other is float otherFloat)
            {
                return IsPositive.CompareTo(otherFloat == float.PositiveInfinity);
            }
            return IsPositive ? 1 : -1;
        }

        public override string ToString() => HasValue
            ? string.Empty + Value.ToString()
            : IsPositive ? "Infinity" : "-Infinity";

        public static implicit operator Infinity<T>(ValueTuple<T?, bool> value) => value new(value.Item1, value.Item2);
        public static implicit operator Infinity<T>(T? value) => new(value, true);
        public static implicit operator T?(Infinity<T> value) => value.IsInfinite ? null : value.Value;

        public static Infinity<T> operator -(Infinity<T> value) => new(value.Value, value.HasValue, false);
        public static Infinity<T> operator +(Infinity<T> value) => new(value.Value, value.HasValue, true);
        public static Infinity<T> operator !(Infinity<T> value) => new(value.Value, value.HasValue, !value.IsPositive);

        public static bool operator ==(Infinity<T> left, Infinity<T> right) => left.Equals(right);
        public static bool operator !=(Infinity<T> left, Infinity<T> right) => !left.Equals(right);
        public static bool operator >(Infinity<T> left, Infinity<T> right) => left.CompareTo(right) == 1;
        public static bool operator <(Infinity<T> left, Infinity<T> right) => left.CompareTo(right) == -1;
        public static bool operator >=(Infinity<T> left, Infinity<T> right) => left == right || left > right;
        public static bool operator <=(Infinity<T> left, Infinity<T> right) => left == right || left < right;

        public void Deconstruct(out T? value, out bool positive)
        {
            value = Value;
            positive = IsPositive;
        }
    }

    public static class Infinity
    {
        public 
        public static Infinity<T> Inf<T>(T? value = null, bool positive = true) where T : struct, IComparable<T>, IComparable
            => new(value, positive);

        public static Infinity<T> Inf<T>(bool positive) where T : struct, IComparable<T>, IComparable
            => new(positive);

        public static Infinity<T> Inf<T>() where T : struct, IComparable<T>, IComparable
            => new();

        public static Infinity<T> ToInfinity<T>(this T? value, bool positive) where T : struct, IComparable<T>, IComparable
            => new(value, positive);

        public static Infinity<T> ToPositiveInfinity<T>(this T? value) where T : struct, IComparable<T>, IComparable
            => new(value, true);

        public static Infinity<T> ToNegativeInfinity<T>(this T? value) where T : struct, IComparable<T>, IComparable
            => new(value, false);
    }
}