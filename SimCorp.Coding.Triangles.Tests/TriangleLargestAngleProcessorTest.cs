using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class TriangleLargestAngleProcessorTest
{
    static TriangleLargestAngleProcessor CreateSut()
    {
        var validators = Array.Empty<IValidator<TriangleArguments>>();
        var mList      = new IResultMatcher<TriangleArguments, TriangleAngleMatchResult>[] { new AAngleResultMatcher(), new BAngleResultMatcher(), new CAngleResultMatcher() };
        var sut        = new TriangleLargestAngleProcessor(validators, mList);
        return sut;
    }

    [Test]
    public void IntegrationTest()
    {
        // assign
        var sut = CreateSut();
        
        var arguments = new TriangleArguments
        {
            A = 3,
            B = 4,
            C = 5,
        };

        var expectedResult = Result.Succeed(new TriangleLargestAngleResult()
        {
            Angle = 90
        });

        // act
        var actualResult = sut.Process(arguments);
        
        // assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}