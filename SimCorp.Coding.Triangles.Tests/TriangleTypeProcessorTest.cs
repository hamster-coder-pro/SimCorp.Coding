using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class TriangleTypeProcessorTest
{
    [Test]
    public void ResultMatcherPriorityIsValid()
    {
        // assign
        var executionOrder = new List<string>();

        var m1Mock = new Mock<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>>();
        m1Mock.Setup(x => x.Priority).Returns(0);
        var m1MatchResult = Result.Succeed(new TriangleTypeMatchResult
        {
            Result    = TriangleTypeEnum.Unknown,
            IsMatched = false
        });

        m1Mock.Setup(x => x.Match(It.IsAny<TriangleArguments>())).Returns(m1MatchResult).Callback(() => executionOrder.Add("m1"));

        var m2Mock = new Mock<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>>();
        m2Mock.Setup(x => x.Priority).Returns(1);
        var m2MatchResult = Result.Succeed(new TriangleTypeMatchResult
        {
            Result    = TriangleTypeEnum.Unknown,
            IsMatched = false
        });
        m2Mock.Setup(x => x.Match(It.IsAny<TriangleArguments>())).Returns(m2MatchResult).Callback(() => executionOrder.Add("m2"));

        var validatorMock = new Mock<IValidator<TriangleArguments>>();
        validatorMock.Setup(x => x.Validate(It.IsAny<TriangleArguments>())).Returns(() => new ValidationResult());

        var sut = new TriangleTypeProcessor([validatorMock.Object], [m2Mock.Object, m1Mock.Object]);

        // act
        var arguments = new TriangleArguments()
        {
            A = 1,
            B = 2,
            C = 2.5
        };

        var result = sut.Process(arguments);

        // assert
        m1Mock.Verify(x => x.Match(It.IsAny<TriangleArguments>()), Times.Once);
        m2Mock.Verify(x => x.Match(It.IsAny<TriangleArguments>()), Times.Once);
        executionOrder.Should().Equal("m1", "m2");
    }

    [TestCase(1, 1, 1,   true,  TriangleTypeEnum.Equilateral, null)]
    [TestCase(4, 4, 3,   true,  TriangleTypeEnum.Isosceles,   null)]
    [TestCase(3, 4, 5,   true,  TriangleTypeEnum.Scalene,     null)]
    [TestCase(1, 1, 2.5, false, null,                         "Not a valid triangle")]
    [TestCase(0, 1, 1,   false, null, "Side A length should be positive")]
    [TestCase(1, 0, 1,   false, null, "Side B length should be positive")]
    [TestCase(1, 1, 0,   false, null, "Side C length should be positive")]
    [TestCase(1, 1, 2,   false, null,                         "Not a valid triangle")]
    public void IntegrationTest(double a, double b, double c, bool isSucceed, TriangleTypeEnum? triangleType, string? error)
    {
        // assign
        var doubleHelper = new DoubleHelper();
        IEnumerable<IValidator<TriangleArguments>> validators =
        [
            new TriangleArgumentsValidator(doubleHelper)
        ];
        IEnumerable<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>> matchers =
        [
            new TriangleIsScaleneResultMatcher(),
            new TriangleIsIsoscelesResultMatcher(doubleHelper),
            new TriangleIsEquilateralResultMatcher(doubleHelper)
        ];
        var sut = new TriangleTypeProcessor(validators, matchers);
        var arguments = new TriangleArguments
        {
            A = a,
            B = b,
            C = c
        };
        var expectedResult = new Result<TriangleTypeResult>
        {
            IsSucceed = isSucceed,
            Value     = triangleType == null ? null : new TriangleTypeResult { Result = triangleType.Value },
            Error     = error
        };
        // act
        var actualResult = sut.Process(arguments);

        // assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void ValidationErrorsProcessingValid()
    {
        // assign
        var doubleHelper = new DoubleHelper();

        var executionOrder = new List<string>();

        var m1Mock = new Mock<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>>();
        m1Mock.Setup(x => x.Priority).Returns(0);
        var m1MatchResult = Result.Succeed(new TriangleTypeMatchResult
        {
            Result    = TriangleTypeEnum.Unknown,
            IsMatched = false
        });

        m1Mock.Setup(x => x.Match(It.IsAny<TriangleArguments>())).Returns(m1MatchResult).Callback(() => executionOrder.Add("m1"));

        var validatorMock = new Mock<IValidator<TriangleArguments>>();
        validatorMock.Setup(x => x.Validate(It.IsAny<TriangleArguments>())).Returns(() => new ValidationResult([new ValidationFailure("A", "Test error")]));

        var sut = new TriangleTypeProcessor([validatorMock.Object], [m1Mock.Object]);

        // act
        var arguments = new TriangleArguments()
        {
            A = 1,
            B = 2,
            C = 2.5
        };

        var expectedResult = Result.Failed<TriangleTypeProcessor>("Test error");
        var actualResult   = sut.Process(arguments);

        actualResult.Should().BeEquivalentTo(expectedResult);

        // assert
        m1Mock.Verify(x => x.Match(It.IsAny<TriangleArguments>()), Times.Never);
    }

    [Test]
    public void MatchIsSucceed_False_Handled()
    {
        // assign
        var m1Mock = new Mock<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>>();
        m1Mock.Setup(x => x.Priority).Returns(0);
        var m1MatchResult = Result.Failed<TriangleTypeMatchResult>("TriangleMatchResult Failed");
        m1Mock.Setup(x => x.Match(It.IsAny<TriangleArguments>())).Returns(m1MatchResult);

        var validatorMock = new Mock<IValidator<TriangleArguments>>();
        validatorMock.Setup(x => x.Validate(It.IsAny<TriangleArguments>())).Returns(() => new ValidationResult());

        var sut = new TriangleTypeProcessor([validatorMock.Object], [m1Mock.Object]);

        // act
        var arguments = new TriangleArguments()
        {
            A = 1,
            B = 2,
            C = 2.5
        };

        var expectedResult = Result.Failed<TriangleTypeProcessor>("TriangleMatchResult Failed");
        var actualResult   = sut.Process(arguments);

        actualResult.Should().BeEquivalentTo(expectedResult);

        // assert
        m1Mock.Verify(x => x.Match(It.IsAny<TriangleArguments>()), Times.Once);
    }

    [Test]
    public void MatchValue_Null_Handled()
    {
        // assign
        var m1Mock = new Mock<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>>();
        m1Mock.Setup(x => x.Priority).Returns(0);
        var m1MatchResult = new Result<TriangleTypeMatchResult>
        {
            IsSucceed = true,
            Error     = null,
            Value     = null
        };
        m1Mock.Setup(x => x.Match(It.IsAny<TriangleArguments>())).Returns(m1MatchResult);

        var validatorMock = new Mock<IValidator<TriangleArguments>>();
        validatorMock.Setup(x => x.Validate(It.IsAny<TriangleArguments>())).Returns(() => new ValidationResult());

        var sut = new TriangleTypeProcessor([validatorMock.Object], [m1Mock.Object]);

        // act
        var arguments = new TriangleArguments()
        {
            A = 1,
            B = 2,
            C = 2.5
        };

        var expectedResult = Result.Failed<TriangleTypeProcessor>("matchResult.Value is NULL");
        var actualResult   = sut.Process(arguments);

        actualResult.Should().BeEquivalentTo(expectedResult);

        // assert
        m1Mock.Verify(x => x.Match(It.IsAny<TriangleArguments>()), Times.Once);
    }
}