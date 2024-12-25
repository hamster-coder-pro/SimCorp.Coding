using System.Globalization;
using System.Text;
using NUnit.Framework;
using Moq;
using FluentAssertions;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleLargestAngleResultOutputBuilder))]
public class TriangleLargestAngleResultOutputBuilderTest
{
    private Mock<IOutputDataStrategy>               _outputMock    = default!;
    private TriangleLargestAngleResultOutputBuilder _builder       = default!;
    private StringBuilder                           _stringBuilder = default!;

    [SetUp]
    public void SetUp()
    {
        Thread.CurrentThread.CurrentCulture   = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

        _stringBuilder = new StringBuilder();
        _outputMock    = new Mock<IOutputDataStrategy>();
        _outputMock.Setup(x => x.WriteLine(It.IsAny<string>())).Callback<string>(text => _stringBuilder.AppendLine(text));
        _outputMock.Setup(x => x.Write(It.IsAny<string>())).Callback<string>(text => _stringBuilder.Append(text));
        _builder = new TriangleLargestAngleResultOutputBuilder(_outputMock.Object);
    }

    private string BuildExpectedResult(IEnumerable<string> expectedResultLines)
    {
        var expectedResult = new StringBuilder();
        foreach (var line in expectedResultLines)
        {
            expectedResult.AppendLine(line);
        }

        return expectedResult.ToString();
    }

    [Test]
    [TestCase(0.0,      new[] { "Triangle largest angle is 0.00 degrees" })]
    [TestCase(45.1234,  new[] { "Triangle largest angle is 45.12 degrees" })]
    [TestCase(90.0,     new[] { "Triangle largest angle is 90.00 degrees" })]
    [TestCase(179.9999, new[] { "Triangle largest angle is 180.00 degrees" })]
    public void ProcessValue_ShouldWriteCorrectMessage(double angle, string[] expectedResultLines)
    {
        var expectedResult = BuildExpectedResult(expectedResultLines);

        // Arrange
        var result = new TriangleLargestAngleResult { Angle = angle };

        // Act
        _builder.Build(Result.Succeed(result));

        // Assert
        _stringBuilder.ToString().Should().Be(expectedResult.ToString());
    }

    [Test]
    [TestCase(true,  null,             new[] { "Failed. Value is NULL" })]
    [TestCase(false, "Error occurred", new[] { "Failed. Error occurred" })]
    [TestCase(false, "",               new[] { "Failed." })]
    public void Build_ShouldHandleFailedResultCorrectly(bool isSucceed, string? error, string[] expectedResultLines)
    {
        var expectedResult = BuildExpectedResult(expectedResultLines);

        // Arrange
        var result = new Result<TriangleLargestAngleResult>
        {
            IsSucceed = isSucceed,
            Error     = error,
            Value     = null
        };

        // Act
        _builder.Build(result);

        // Assert
        _stringBuilder.ToString().Should().Be(expectedResult);
    }

    [Test]
    public void Build_ShouldHandleNullValueCorrectly()
    {
        // Arrange
        var result = new Result<TriangleLargestAngleResult>
        {
            IsSucceed = true,
            Value     = null
        };

        // Act
        _builder.Build(result);

        // Assert
        _outputMock.Verify(m => m.WriteLine("Failed. Value is NULL"), Times.Once);
    }

    [Test]
    [TestCase(0.0,  "Triangle largest angle is 0.00 degrees")]
    [TestCase(90.0, "Triangle largest angle is 90.00 degrees")]
    public void Build_ShouldProcessValueWhenResultIsSucceed(double angle, string expectedMessage)
    {
        // Arrange
        var triangleResult = new TriangleLargestAngleResult { Angle = angle };
        var result = new Result<TriangleLargestAngleResult>
        {
            IsSucceed = true,
            Value     = triangleResult
        };

        // Act
        _builder.Build(result);

        // Assert
        _outputMock.Verify(m => m.WriteLine(expectedMessage), Times.Once);
    }
}