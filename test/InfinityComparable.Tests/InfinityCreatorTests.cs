using LanguageExt;

namespace InifinityComparable.Tests
{
    public class InfinityCreatorTests
    {
        private static int? intNull = null;
        private static Infinity<int> positiveInfinity = new Infinity<int>(Option<int>.None, true);
        private static Infinity<int> negativeInfinity = new Infinity<int>(Option<int>.None, false);

        public static TheoryData<Infinity<int>, Infinity<int>> AllCreatorsShouldBeEqual =
            new TheoryData<Infinity<int>, Infinity<int>>
            {
                { Inf<int>(), positiveInfinity },
                { +Inf<int>(), positiveInfinity },
                { Inf<int>(true), positiveInfinity },
                { intNull.ToPositiveInfinity(), positiveInfinity },
                { intNull.ToInfinity(true), positiveInfinity },
                { Inf<int>(false), negativeInfinity },
                { -Inf<int>(), negativeInfinity },
                { intNull.ToInfinity(false), negativeInfinity },
                { intNull.ToNegativeInfinity(), negativeInfinity },
                { new Infinity<int>(), positiveInfinity },
                { new Infinity<int>(false), negativeInfinity }
            };

        [Theory]
        [MemberData(nameof(AllCreatorsShouldBeEqual))]
        public void InfinityCreators_ShouldBeEqualToExpectedResult(Infinity<int> result, Infinity<int> expectedResult)
        {
            result.Should().Be(expectedResult);
        }
    }
}
