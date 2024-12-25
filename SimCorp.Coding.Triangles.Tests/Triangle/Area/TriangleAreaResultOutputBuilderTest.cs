using System.Globalization;
using System.Text;
using NUnit.Framework;
using Moq;
using FluentAssertions;
using SimCorp.Coding.Triangles;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleAreaResultOutputBuilder))]
public class TriangleAreaResultOutputBuilderTest
{
    private Mock<IOutputDataStrategy>       _outputMock = default!;
    private TriangleAreaResultOutputBuilder _builder = default!;
    private StringBuilder                   _stringBuilder = default!;

    [SetUp]
    public void SetUp()
    {
        Thread.CurrentThread.CurrentCulture   = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

        _stringBuilder = new StringBuilder();
        _outputMock    = new Mock<IOutputDataStrategy>();
        _outputMock.Setup(x => x.WriteLine(It.IsAny<string>())).Callback<string>(text => _stringBuilder.AppendLine(text));
        _outputMock.Setup(x => x.Write(It.IsAny<string>())).Callback<string>(text => _stringBuilder.Append(text));
        _builder                              = new TriangleAreaResultOutputBuilder(_outputMock.Object);
    }

    [Test]
    [TestCase(0.0,      "Triangle area is: 0.00")]
    [TestCase(123.456,  "Triangle area is: 123.46")]
    [TestCase(-123.456, "Triangle area is: -123.46")]
    public void ProcessValue_ShouldWriteCorrectOutput(double area, string expectedOutput)
    {
        expectedOutput = expectedOutput + Environment.NewLine;
        
        // Arrange
        var result = new TriangleAreaResult { Area = area };
        
        // Act
        _builder.Build(Result.Succeed(result));

        // Assert
        _stringBuilder.ToString().Should().Be(expectedOutput);
    }

    [Test]
    [TestCase("Some error occurred", "Failed. Some error occurred")]
    [TestCase(null,                  "Failed.")]
    public void Build_ShouldHandleFailedResult(string? error, string expectedOutput)
    {
        // Arrange
        var result = Result.Failed<TriangleAreaResult>(error);

        // Act
        _builder.Build(result);

        // Assert
        _outputMock.Verify(o => o.WriteLine(expectedOutput), Times.Once);
    }

    [Test]
    public void Build_ShouldHandleException()
    {
        // Arrange
        var exception = new Exception("Exception occurred");
        var result    = Result.Failed<TriangleAreaResult>(exception);

        // Act
        _builder.Build(result);

        // Assert
        _outputMock.Verify(o => o.WriteLine("Failed. Exception occurred"), Times.Once);
    }
}