using NUnit.Framework;
using SimCorp.Coding.Triangles;
using FluentValidation;
using Moq;
using FluentAssertions;
using FluentValidation.Results;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(InputProcessorBase<,>))]
public class InputProcessorBaseTest
{
    private Mock<IValidator<IInputArguments>>                      _validatorMock      = default!;
    private Mock<IResultMatcher<IInputArguments, TestMatchResult>> _resultMatcherMock  = default!;
    private Mock<IResultMatcher<IInputArguments, TestMatchResult>> _resultMatcherMock2 = default!;

    [SetUp]
    public void SetUp()
    {
        _validatorMock = new Mock<IValidator<IInputArguments>>();
        _validatorMock.Setup(x => x.Validate(It.IsAny<IInputArguments>())).Returns(() => new ValidationResult());

        _resultMatcherMock = new Mock<IResultMatcher<IInputArguments, TestMatchResult>>();
        _resultMatcherMock.Setup(m => m.Priority)
                          .Returns(() => 0);
        _resultMatcherMock.Setup(m => m.IsDefault)
                          .Returns(() => false);

        _resultMatcherMock2 = new Mock<IResultMatcher<IInputArguments, TestMatchResult>>();
        _resultMatcherMock2.Setup(m => m.Priority)
                           .Returns(() => 1);
        _resultMatcherMock2.Setup(m => m.IsDefault)
                           .Returns(() => false);
    }

    [Test]
    [TestCase(true,  null)]
    [TestCase(false, "Validation error")]
    public void ValidateArguments_ShouldReturnExpectedResult(bool isValid, string? errorMessage)
    {
        // Arrange
        var validationResult = isValid
            ? new ValidationResult()
            : new ValidationResult([new ValidationFailure("", errorMessage ?? string.Empty)]);

        _validatorMock.Setup(v => v.Validate(It.IsAny<IInputArguments>()))
                      .Returns(validationResult);

        var processor = new TestInputProcessor(new[] { _validatorMock.Object });

        // Act
        var result = processor.Process(Mock.Of<IInputArguments>());

        // Assert
        result.IsSucceed.Should().Be(isValid);
        result.Error.Should().Be(errorMessage);
    }

    [Test]
    public void Process_ShouldReturnFailedResult_OnException()
    {
        // Arrange
        var processor = new TestInputProcessor(new[] { _validatorMock.Object });
        _validatorMock.Setup(v => v.Validate(It.IsAny<IInputArguments>()))
                      .Throws(new Exception("Test exception"));

        // Act
        var result = processor.Process(Mock.Of<IInputArguments>());

        // Assert
        result.IsSucceed.Should().BeFalse();
        result.Error.Should().Be("Test exception");
    }

    [Test]
    public void ProcessLogic_ShouldBeCalled_WhenValidationSucceeds()
    {
        // Arrange
        _validatorMock.Setup(v => v.Validate(It.IsAny<IInputArguments>()))
                      .Returns(new ValidationResult());

        var processor = new TestInputProcessor(new[] { _validatorMock.Object });

        // Act
        var result = processor.Process(Mock.Of<IInputArguments>());

        // Assert
        processor.ProcessLogicCalled.Should().BeTrue();
    }

    [Test]
    [TestCase(true,  true,  true,  true, true,  "M2", null)]
    [TestCase(true,  true,  false, true, true,  "M1", null)]
    [TestCase(true,  false, false, true, true,  "M1", null)]
    [TestCase(false, false, false, true, false, "",   "No match found")]
    [TestCase(true,  true,  true,  false, true,  "M1", null)]
    [TestCase(true,  true,  false, false, true,  "M2", null)]
    [TestCase(true,  false, false, false, true,  "M1", null)]
    [TestCase(false, false, false, false, false, "",   "No match found")]
    public void ProcessLogic_WithMatchers_ShouldReturnExpectedResult(
        bool    isMatched1,
        bool    isMatched2,
        bool    m1IsDefault,
        bool    breakOnMatch,
        bool    isSucceed,
        string  expectedValue,
        string? expectedError
    )
    {
        // Arrange
        _resultMatcherMock.Setup(m => m.Priority)
                          .Returns(() => 0);
        _resultMatcherMock.Setup(m => m.IsDefault)
                          .Returns(() => m1IsDefault);
        _resultMatcherMock.Setup(m => m.Match(It.IsAny<IInputArguments>()))
                          .Returns(() => Result.Succeed(new TestMatchResult { IsMatched = isMatched1, Value = "M1" }));

        _resultMatcherMock2.Setup(m => m.Priority)
                           .Returns(() => 1);
        _resultMatcherMock2.Setup(m => m.Match(It.IsAny<IInputArguments>()))
                           .Returns(() => Result.Succeed(new TestMatchResult { IsMatched = isMatched2, Value = "M2" }));

        var processor = new TestInputProcessorWithMatchers(
            [_validatorMock.Object],
            [_resultMatcherMock2.Object, _resultMatcherMock.Object],
            breakOnMatch
        );

        var expectedResult = new Result<IOutputResult>
        {
            IsSucceed = isSucceed,
            Error     = expectedError,
            Value = isSucceed
                ? new TestOutputResult { Value = expectedValue }
                : null
        };

        // Act
        var actualResult = processor.Process(Mock.Of<IInputArguments>());

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult, o => o.RespectingRuntimeTypes());
        //if (actualResult.Value != null)
        //{
        //    actualResult.Value.Should().BeEquivalentTo(expectedResult.Value);
        //}
    }

    private class TestInputProcessor : InputProcessorBase<IInputArguments, IOutputResult>
    {
        public bool ProcessLogicCalled { get; private set; }

        public TestInputProcessor(IEnumerable<IValidator<IInputArguments>> validators)
            : base(validators)
        {
        }

        protected override Result<IOutputResult> ProcessLogic(IInputArguments arguments)
        {
            ProcessLogicCalled = true;
            return Result.Succeed(Mock.Of<IOutputResult>());
        }
    }

    private class TestInputProcessorWithMatchers : InputProcessorBase<IInputArguments, TestOutputResult, TestMatchResult>
    {
        public TestInputProcessorWithMatchers(
            IEnumerable<IValidator<IInputArguments>>                      validators,
            IEnumerable<IResultMatcher<IInputArguments, TestMatchResult>> resultMatchers,
            bool                                                          breakOnMatch
        )
            : base(validators, resultMatchers)
        {
            BreakOnMatchFound = breakOnMatch;
        }

        protected override bool BreakOnMatchFound { get; }

        protected override TestOutputResult? AggregateMatch(TestOutputResult? resultValue, TestMatchResult matchResult)
        {
            return matchResult.IsMatched
                ? new TestOutputResult { Value = matchResult.Value }
                : resultValue;
        }
    }

    public class TestMatchResult : IMatchResult
    {
        public bool IsMatched { get; set; }

        public string Value { get; init; } = string.Empty;
    }

    public class TestOutputResult : IOutputResult
    {
        public string Value { get; init; } = string.Empty;
    }
}