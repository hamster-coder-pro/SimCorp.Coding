using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace SimCorp.Coding.Triangles.Tests
{
    public class DoubleHelperTest
    {
        [Test]
        public void ValidEpsilon()
        {
            var sut = new DoubleHelper();
            sut.Epsilon.Should().Be(0.001d);
        }

        [Test]
        public void ValidHashCode()
        {
            var sut = new DoubleHelper();
            sut.GetHashCode(123).Should().Be(sut.GetHashCode(123));
        }

        [TestCase(0D,       0D,                true)]
        [TestCase(0D,       0.00099999999999D, true)]
        [TestCase(0D,       0.001D,            false)]
        [TestCase(-0.0004D, 0.0005D,           true)]
        [TestCase(-0.0005D, 0.0005D,           false)]
        public void ValidEquals(double a, double b, bool expectedResult)
        {
            var sut          = new DoubleHelper();
            var actualResult = sut.Equals(a, b);

            actualResult.Should().Be(expectedResult);
        }

        [TestCase(0D,       0D,                0)]
        [TestCase(0D,       0.00099999999999D, 0)]
        [TestCase(0D,       0.001D,            -1)]
        [TestCase(0.001D,   0D,                1)]
        [TestCase(-0.0004D, 0.0005D,           0)]
        [TestCase(-0.0005D, 0.0005D,           -1)]
        [TestCase(0.0005D,  -0.0005D,          1)]
        public void ValidCompare(double a, double b, int expectedResult)
        {
            var sut          = new DoubleHelper();
            var actualResult = sut.Compare(a, b);

            actualResult.Should().Be(expectedResult);
        }
    }
}