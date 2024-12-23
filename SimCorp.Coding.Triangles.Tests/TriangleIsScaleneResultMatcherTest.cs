using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class TriangleIsScaleneResultMatcherTest
{
    static TriangleIsScaleneResultMatcher CreateSut()
    {
        var sut                = new TriangleIsScaleneResultMatcher();
        return sut;
    }

    [Test]
    public void PriorityIsValid()
    {
        var sut = CreateSut();
        sut.Priority.Should().Be(0);
    }

    [Test]
    public void IsDefaultValid()
    {
        var sut = CreateSut();
        sut.IsDefault.Should().Be(true);
    }

    [Test]
    public void ResultsValid(        )
    {
        var arguments = new TriangleArguments
        {
            A = 1,
            B = 2,
            C = 3,
        };
        
        var expectedResult = Result.Succeed(new TriangleTypeMatchResult()
        {
            IsMatched = true,
            Result = TriangleTypeEnum.Scalene
        });

        var        sut   = CreateSut();
        var actualResult = sut.Match(arguments);

        actualResult.Should().BeEquivalentTo(expectedResult);
        
        // output.Verify(x => x.WriteLine("Triangle is scalene"), Times.Once);
    }
}