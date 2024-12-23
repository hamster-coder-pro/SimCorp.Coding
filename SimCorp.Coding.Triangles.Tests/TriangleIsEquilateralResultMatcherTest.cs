using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class TriangleIsEquilateralResultMatcherTest
{
    static TriangleIsEquilateralResultMatcher CreateSut()
    {
        var doubleHelper = new DoubleHelper();
        var sut          = new TriangleIsEquilateralResultMatcher(doubleHelper);
        return sut;
    }

    [Test]
    public void PriorityValid()
    {
        var sut = CreateSut();
        sut.Priority.Should().Be(0);
    }

    [Test]
    public void IsDefaultValid()
    {
        var sut = CreateSut();
        sut.IsDefault.Should().Be(false);
    }

    [TestCase(1,      1,      1,      true)]
    [TestCase(1,      2,      1,      false)]
    [TestCase(0.0005, 0.0004, 0.0013, true)]
    public void ResultsValid(
        double a,
        double b,
        double c,
        bool   isMatched
    )
    {
        var arguments = new TriangleArguments()
        {
            A = a,
            B = b,
            C = c
        };

        var expectedResult = Result.Succeed(new TriangleTypeMatchResult
        {
            Result    = isMatched ? TriangleTypeEnum.Equilateral : TriangleTypeEnum.Unknown,
            IsMatched = isMatched
        });

        var sut          = CreateSut();
        var actualResult = sut.Match(arguments);

        actualResult.Should().BeEquivalentTo(expectedResult);

        //actualResult.IsMatched.Should().Be(expectedResult);
        //actualResult.Error.Should().BeNull();
        //actualResult.IsSucceed.Should().BeTrue();
        //safeActualResult.Result.Should().Be(TriangleTypeEnum.Equilateral);


        //actualResult.Should().Be(expectedResult);
        //output.Verify(x => x.WriteLine("Triangle is equilateral"), expectedResult ? Times.Once : Times.Never);
    }
}