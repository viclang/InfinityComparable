using LanguageExt;

namespace InifinityComparable.Tests
{
    public class InfinityComparisonTests
    {
        private static readonly Infinity<int> positiveInfinity = new Infinity<int>(Option<int>.None, true);
        private static readonly Infinity<int> negativeInfinity = new Infinity<int>(Option<int>.None, false);
        private static readonly Infinity<int> maxFinite = new Infinity<int>(int.MaxValue, true);
        private static readonly Infinity<int> minFinite = new Infinity<int>(int.MinValue, true);
        private static readonly Infinity<int> negativeMinFinite = new Infinity<int>(int.MinValue, false);
        private static readonly Infinity<int> negativeMaxFinite = new Infinity<int>(int.MaxValue, false);

        // Arrange
        public static TheoryData<Infinity<int>, Infinity<int>, int> AllCompareToWithExpectedResult =
            new TheoryData<Infinity<int>, Infinity<int>, int>
            {
                { positiveInfinity, positiveInfinity, 0 },
                { negativeInfinity, negativeInfinity, 0 },
                { positiveInfinity, negativeInfinity, 1 },
                { negativeInfinity, positiveInfinity, -1 },
                { minFinite, negativeMinFinite, 0 },
                { maxFinite, negativeMaxFinite, 0 },
                { positiveInfinity, maxFinite, 1 },
                { negativeInfinity, minFinite, -1 },
                { minFinite, negativeInfinity, 1 },
                { maxFinite, positiveInfinity, -1 },
            };

        public static TheoryData<Infinity<int>, object?, int> AllCompareToObjectWithExpectedResult =
            new TheoryData<Infinity<int>, object?, int>
            {
                { positiveInfinity, float.PositiveInfinity, 0 },
                { positiveInfinity, double.PositiveInfinity, 0 },
                { negativeInfinity, float.NegativeInfinity, 0 },
                { negativeInfinity, double.NegativeInfinity, 0 },
                { positiveInfinity, float.NegativeInfinity, 1 },
                { positiveInfinity, double.NegativeInfinity, 1 },
                { negativeInfinity, float.PositiveInfinity, -1 },
                { negativeInfinity, double.PositiveInfinity, -1 }
            };

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        public void AllCompareTo_ShouldGiveExpectedResult(Infinity<int> a, Infinity<int> b, int expectedResult)
        {
            // Act
            var result = a.CompareTo(b);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        [MemberData(nameof(AllCompareToObjectWithExpectedResult))]
        public void AllCompareToObject_ShouldGiveExpectedResult(Infinity<int> a, object? b, int expectedResult)
        {
            // Act
            var result = a.CompareTo(b);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        public void AllEquals_ShouldGiveExpectedResult(Infinity<int> a, Infinity<int> b, int compareResult)
        {
            // Act
            var result = a.Equals(b);

            // Assert
            result.Should().Be(compareResult == 0);
        }

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        [MemberData(nameof(AllCompareToObjectWithExpectedResult))]
        public void AllEqualsObject_ShouldGiveExpectedResult(Infinity<int> a, object? b, int compareResult)
        {

            string? testing = "";
            testing.ToOption();
            // Act
            var result = a.Equals(b);

            // Assert
            result.Should().Be(compareResult == 0);
        }

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        public void GreaterThan_ShouldGiveExpectedResult(Infinity<int> a, Infinity<int> b, int compareResult)
        {
            // Act
            var result = a > b;

            // Assert
            result.Should().Be(compareResult == 1);
        }

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        public void SmallerThan_ShouldGiveExpectedResult(Infinity<int> a, Infinity<int> b, int compareResult)
        {
            // Act
            var result = a < b;

            // Assert
            result.Should().Be(compareResult == -1);
        }

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        public void GreaterThanOrEquals_ShouldGiveExpectedResult(Infinity<int> a, Infinity<int> b, int compareResult)
        {
            // Act
            var result = a >= b;

            // Assert
            result.Should().Be(compareResult >= 0);
        }

        [Theory]
        [MemberData(nameof(AllCompareToWithExpectedResult))]
        public void SmallerThanOrEquals_ShouldGiveExpectedResult(Infinity<int> a, Infinity<int> b, int compareResult)
        {
            // Act
            var result = a <= b;

            // Assert
            result.Should().Be(compareResult <= 0);
        }
    }
}