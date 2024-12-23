using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class TriangleIsIsoscelesResultMatcherTest
{
    static TriangleIsIsoscelesResultMatcher CreateSut()
    {
        var doubleHelper = new DoubleHelper();
        var sut          = new TriangleIsIsoscelesResultMatcher(doubleHelper);
        return sut;
    }

    [Test]
    public void PriorityIsValid()
    {
        var sut = CreateSut();
        sut.Priority.Should().Be(1);
    }

    [Test]
    public void IsDefaultValid()
    {
        var sut = CreateSut();
        sut.IsDefault.Should().Be(false);
    }

    [TestCase(1,      1,      2,      true)]
    [TestCase(1,      2,      1,      true)]
    [TestCase(2,      1,      1,      true)]
    [TestCase(0.0005, 0.0004, 0.0013, true)]
    [TestCase(2,      1,      3,      false)]
    public void ResultsValid(
        double a,
        double b,
        double c,
        bool   isMatched
    )
    {
        var arguments = new TriangleArguments
        {
            A = a,
            B = b,
            C = c,
        };

        var expectedResult = Result.Succeed(
            new TriangleTypeMatchResult()
            {
                IsMatched = isMatched,
                Result    = isMatched ? TriangleTypeEnum.Isosceles : TriangleTypeEnum.Unknown
            }
        );

        var     sut          = CreateSut();
        var                   actualResult = sut.Match(arguments);

        actualResult.Should().BeEquivalentTo(expectedResult);
        
        //output.Verify(x => x.WriteLine("Triangle is isosceles"), expectedResult ? Times.Once : Times.Never);
    }
}