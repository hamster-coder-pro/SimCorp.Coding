using FluentAssertions;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class BAngleResultMatcherTest
{
    static BAngleResultMatcher CreateSut()
    {
        var sut = new BAngleResultMatcher();
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
        sut.IsDefault.Should().Be(false);
    }

    [Test]
    public void ResultsValid()
    {
        var arguments = new TriangleArguments
        {
            A = 3,
            B = 5,
            C = 4,
        };

        var expectedResult = Result.Succeed(new TriangleAngleMatchResult()
        {
            IsMatched = true,
            Angle = 90
        });

        var sut = CreateSut();
        var actualResult = sut.Match(arguments);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}