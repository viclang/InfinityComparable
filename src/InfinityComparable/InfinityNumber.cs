using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace InfinityComparable
{
#if NET7_0
    public readonly struct InfinityNumber<T> : IInfinityNumber<T>, INumber<InfinityNumber<T>>
        where T : struct, INumber<T>
    {
        internal readonly bool positive;
        [MemberNotNullWhen(false, nameof(Finite))]
        public bool IsInfinity { get; }
        public T? Finite => throw new NotImplementedException();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly T value;

        public static InfinityNumber<T> NegativeInfinity => new(default, true, false);

        public static InfinityNumber<T> PositiveInfinity => new(default, true, true);

        public static InfinityNumber<T> One => new(T.One, false, true);

        public static int Radix => T.Radix;

        public static InfinityNumber<T> Zero => new(T.Zero, false, true);

        public static InfinityNumber<T> AdditiveIdentity => new(T.AdditiveIdentity, false, true);

        public static InfinityNumber<T> MultiplicativeIdentity => new(T.MultiplicativeIdentity, false, true);

        public InfinityNumber() : this(default, true, true)
        {
        }

        public InfinityNumber(bool positive) : this(default, true, positive)
        {
        }

        public InfinityNumber(T? value, bool positive) : this(value.GetValueOrDefault(), !value.HasValue, positive)
        {
        }

        internal InfinityNumber(T value, bool isInfinity, bool positive)
        {
            this.value = value;
            IsInfinity = isInfinity;
            this.positive = positive;
        }

        public static bool IsFinite(InfinityNumber<T> value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNegativeInfinity(InfinityNumber<T> value) => value.IsInfinity && !value.positive;

        public static bool IsPositiveInfinity(InfinityNumber<T> value) => value.IsInfinity && !value.positive;

        public T GetValueOrDefault()
        {
            throw new NotImplementedException();
        }

        public T GetValueOrDefault(T defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool Equals(InfinityNumber<T> other) => IsInfinity
            ? other.IsInfinity && positive == other.positive
            : !other.IsInfinity && value.Equals(other.value);

        public override bool Equals(object? obj)
        {
            return obj is Infinity<T> otherInfinity && Equals(otherInfinity)
                || !IsInfinity && value.Equals(obj)
                || obj is double otherDouble && positive.Equals(otherDouble == double.PositiveInfinity)
                || obj is float otherFloat && positive.Equals(otherFloat == float.PositiveInfinity);
        }

        public override int GetHashCode() => IsInfinity
            ? positive ? 1 : -1
            : Finite.GetHashCode();

        public int CompareTo(object? obj)
        {
            if (obj is Infinity<T> otherInfinity)
            {
                return CompareTo(otherInfinity);
            }
            if (IsInfinity)
            {
                if (obj is double otherDouble)
                {
                    return positive.CompareTo(double.IsPositiveInfinity(otherDouble));
                }
                if (obj is float otherFloat)
                {
                    return positive.CompareTo(float.IsPositiveInfinity(otherFloat));
                }
                return positive ? 1 : -1;
            }
            return value.CompareTo(obj);
        }

        public int CompareTo(InfinityNumber<T> other)
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

        #region Parser
        public static InfinityNumber<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static InfinityNumber<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out InfinityNumber<T> result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out InfinityNumber<T> result)
        {
            throw new NotImplementedException();
        }

        public static InfinityNumber<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out InfinityNumber<T> result)
        {
            throw new NotImplementedException();
        }

        static InfinityNumber<T> IParsable<InfinityNumber<T>>.Parse(string s, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out InfinityNumber<T> result)
        {
            throw new NotImplementedException();
        }
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }
        #endregion



        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }

        public static implicit operator InfinityNumber<T>(ValueTuple<T?, bool> value) => new(value.Item1, value.Item2);

        public static implicit operator InfinityNumber<T>(T? value) => new(value, true);

        public static implicit operator T?(InfinityNumber<T> value) => value.Finite;

        public static bool operator ==(InfinityNumber<T> left, InfinityNumber<T> right) => left.Equals(right);

        public static bool operator !=(InfinityNumber<T> left, InfinityNumber<T> right) => !(left == right);

        public static bool operator <(InfinityNumber<T> left, InfinityNumber<T> right) => left.CompareTo(right) < 0;

        public static bool operator <=(InfinityNumber<T> left, InfinityNumber<T> right) => left.CompareTo(right) <= 0;

        public static bool operator >(InfinityNumber<T> left, InfinityNumber<T> right) => left.CompareTo(right) > 0;

        public static bool operator >=(InfinityNumber<T> left, InfinityNumber<T> right) => left.CompareTo(right) >= 0;


        public static InfinityNumber<T> operator ++(InfinityNumber<T> value)
            => value + One;

        public static InfinityNumber<T> operator --(InfinityNumber<T> value)
            => value - One;

        public static InfinityNumber<T> operator %(InfinityNumber<T> left, InfinityNumber<T> right)
            => new(left.value % right.value, false);

        public static InfinityNumber<T> operator +(InfinityNumber<T> left, InfinityNumber<T> right)
            => (left.IsInfinity, right.IsInfinity) switch
            {
                (false, false) => new(left.value + right.value, false),
                (true, false) => left,
                (false, true) => right,
                (true, true) when left.positive == right.positive => left,
                (true, true) => throw new NotSupportedException()
            };

        public static InfinityNumber<T> operator -(InfinityNumber<T> left, InfinityNumber<T> right)
            => (left.IsInfinity, right.IsInfinity) switch
            {
                (false, false) => new(left.value - right.value, false),
                (true, false) => left,
                (false, true) => right,
                (true, true) => throw new NotSupportedException()
            };

        public static InfinityNumber<T> operator *(InfinityNumber<T> left, InfinityNumber<T> right)
            => (left.IsInfinity, right.IsInfinity) switch
            {
                (false, false) => new(left.value * right.value, false),
                (true, false) when right.value != Zero => left,
                (false, true) when left.value != Zero => right,
                (true, true) when left.positive == right.positive => PositiveInfinity,
                (true, true) when left.positive != right.positive => NegativeInfinity,
                (_, _) => throw new NotSupportedException()
            };

        public static InfinityNumber<T> operator /(InfinityNumber<T> left, InfinityNumber<T> right)
            => (left.IsInfinity, right.IsInfinity) switch
            {
                (false, false) => new(left.value / right.value, false),
                (true, false) => PositiveInfinity,
                (false, true) => Zero,
                (true, true) => throw new NotSupportedException()
            };

        public static InfinityNumber<T> operator -(InfinityNumber<T> value) => new(value.value, value.IsInfinity, false);

        public static InfinityNumber<T> operator +(InfinityNumber<T> value) => new(value.value, value.IsInfinity, true);

        #region implements other INumber<T> methods
        public static InfinityNumber<T> Abs(InfinityNumber<T> value) => value.IsInfinity ? value : new InfinityNumber<T>(T.Abs(value.value), value.IsInfinity, value.positive);

        public static bool IsCanonical(InfinityNumber<T> value) => !value.IsInfinity && T.IsCanonical(value.value);

        public static bool IsComplexNumber(InfinityNumber<T> value) => !value.IsInfinity && T.IsComplexNumber(value.value);

        public static bool IsEvenInteger(InfinityNumber<T> value) => !value.IsInfinity && T.IsComplexNumber(value.value);

        public static bool IsImaginaryNumber(InfinityNumber<T> value) => !value.IsInfinity && T.IsImaginaryNumber(value.value);

        static bool INumberBase<InfinityNumber<T>>.IsInfinity(InfinityNumber<T> value) => value.IsInfinity;

        public static bool IsInteger(InfinityNumber<T> value) => !value.IsInfinity && T.IsInteger(value.value);

        public static bool IsNaN(InfinityNumber<T> value) => false;

        public static bool IsNegative(InfinityNumber<T> value) => !value.IsInfinity && T.IsNegative(value.value);

        public static bool IsNormal(InfinityNumber<T> value) => !value.IsInfinity && T.IsNormal(value.value);

        public static bool IsOddInteger(InfinityNumber<T> value) => !value.IsInfinity && T.IsOddInteger(value.value);

        public static bool IsPositive(InfinityNumber<T> value) => !value.IsInfinity && T.IsPositive(value.value);

        public static bool IsRealNumber(InfinityNumber<T> value) => !value.IsInfinity && T.IsRealNumber(value.value);

        public static bool IsSubnormal(InfinityNumber<T> value) => !value.IsInfinity && T.IsSubnormal(value.value);

        public static bool IsZero(InfinityNumber<T> value) => !value.IsInfinity && T.IsZero(value.value);

        public static InfinityNumber<T> MaxMagnitude(InfinityNumber<T> x, InfinityNumber<T> y) => MaxMagnitudeNumber(x, y);

        public static InfinityNumber<T> MaxMagnitudeNumber(InfinityNumber<T> x, InfinityNumber<T> y) => x >= y ? x : y;

        public static InfinityNumber<T> MinMagnitude(InfinityNumber<T> x, InfinityNumber<T> y) => MinMagnitudeNumber(x, y);

        public static InfinityNumber<T> MinMagnitudeNumber(InfinityNumber<T> x, InfinityNumber<T> y) => x <= y ? x : y;

        static bool INumberBase<InfinityNumber<T>>.TryConvertFromChecked<TOther>(TOther value, out InfinityNumber<T> result)
        {
            return T.TryConvertFromChecked<TOther>(value, out result);
        }

        static bool INumberBase<InfinityNumber<T>>.TryConvertFromSaturating<TOther>(TOther value, out InfinityNumber<T> result)
        {
            return !value.IsInfinity && T.TryConvertFromSaturating(value.value, result);
        }

        static bool INumberBase<InfinityNumber<T>>.TryConvertFromTruncating<TOther>(TOther value, out InfinityNumber<T> result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<InfinityNumber<T>>.TryConvertToChecked<TOther>(InfinityNumber<T> value, out TOther result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<InfinityNumber<T>>.TryConvertToSaturating<TOther>(InfinityNumber<T> value, out TOther result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<InfinityNumber<T>>.TryConvertToTruncating<TOther>(InfinityNumber<T> value, out TOther result)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
#endif
}
