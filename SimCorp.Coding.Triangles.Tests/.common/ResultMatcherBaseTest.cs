using NUnit.Framework;
using Moq;
using FluentAssertions;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(ResultMatcherBase<,>))]
public class ResultMatcherBaseTest
{
    private Mock<IInputArguments> _mockArguments   = default!;
    private Mock<IMatchResult>    _mockMatchResult = default!;

    [SetUp]
    public void SetUp()
    {
        _mockArguments   = new Mock<IInputArguments>();
        _mockMatchResult = new Mock<IMatchResult>();
    }

    [Test]
    public void Priority_ShouldReturnDefaultValue()
    {
        var matcher = new TestResultMatcher();
        matcher.Priority.Should().Be(0);
    }

    [Test]
    public void IsDefault_ShouldReturnDefaultValue()
    {
        var matcher = new TestResultMatcher();
        matcher.IsDefault.Should().BeFalse();
    }

    [Test]
    public void Match_ShouldReturnSuccessResult_WhenMatchLogicSucceeds()
    {
        var matcher = new TestResultMatcher();
        var result  = matcher.Match(_mockArguments.Object);

        result.Should().NotBeNull();
        result.IsSucceed.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<MockMatchResult>();
    }

    [Test]
    public void Match_ShouldReturnFailedResult_WhenMatchLogicThrowsException()
    {
        var matcher = new TestResultMatcher(throwException: true);
        var result  = matcher.Match(_mockArguments.Object);

        result.Should().NotBeNull();
        result.IsSucceed.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void MatchLogic_ShouldHandleValidArguments()
    {
        var matcher = new TestResultMatcher();
        var result  = matcher.Match(_mockArguments.Object);

        result.Should().NotBeNull();
        result.IsSucceed.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<MockMatchResult>();
    }

    #region Nested classes

    private class TestResultMatcher : ResultMatcherBase<IInputArguments, MockMatchResult>
    {
        private readonly bool _throwException;

        public TestResultMatcher(bool throwException = false)
        {
            _throwException = throwException;
        }

        protected override Result<MockMatchResult> MatchLogic(IInputArguments arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (_throwException)
                throw new InvalidOperationException("Test exception");

            return Result.Succeed(new MockMatchResult { IsMatched = true });
        }
    }

    private class MockMatchResult : IMatchResult
    {
        public bool IsMatched { get; set; }
    }

    #endregion Nested classes
}