using NUnit.Framework;
using Moq;
using FluentAssertions;
using SimCorp.Coding.Triangles;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TypeOutputBuilderBase<>))]
public class TypeOutputBuilderBaseTest
{
    private Mock<IOutputDataStrategy> _mockOutputDataStrategy;

    [SetUp]
    public void SetUp()
    {
        _mockOutputDataStrategy = new Mock<IOutputDataStrategy>();
    }

    [Test]
    [TestCase(true,  null,            "Processed Value")]
    [TestCase(true,  null,            null)]
    [TestCase(false, "Error Message", null)]
    [TestCase(false, null,            null)]
    public void Build_ShouldHandleAllCasesCorrectly(bool isSucceed, string? error, string? value)
    {
        // Arrange
        var result = new Result<TestOutputResult>
        {
            IsSucceed = isSucceed,
            Error     = error,
            Value     = value == null ? null : new TestOutputResult() { Value = value }
        };
        //var result = isSucceed
        //    ? Result.Succeed(new TestOutputResult() { Value = value })
        //    : Result.Failed<TestOutputResult>(error);

        var builder = new TestTypeOutputBuilder(_mockOutputDataStrategy.Object);

        // Act
        builder.Build(result);

        // Assert
        if (!isSucceed)
        {
            _mockOutputDataStrategy.Verify(o => o.WriteLine("Failed."), Times.Once);
            if (!string.IsNullOrEmpty(error))
            {
                _mockOutputDataStrategy.Verify(o => o.WriteLine(error), Times.Once);
            }
        }
        else if (value == null)
        {
            _mockOutputDataStrategy.Verify(o => o.WriteLine("Failed."),              Times.Once);
            _mockOutputDataStrategy.Verify(o => o.WriteLine("result.Value is NULL"), Times.Once);
        }
        else
        {
            _mockOutputDataStrategy.Verify(o => o.WriteLine($"Processed: {value}"), Times.Once);
        }
    }

    private class TestTypeOutputBuilder : TypeOutputBuilderBase<TestOutputResult>
    {
        public TestTypeOutputBuilder(IOutputDataStrategy output)
            : base(output)
        {
        }

        protected override void ProcessValue(TestOutputResult value)
        {
            Output.WriteLine($"Processed: {value.Value}");
        }
    }

    private class TestOutputResult : IOutputResult
    {
        public string Value { get; init; } = string.Empty;
    }
}