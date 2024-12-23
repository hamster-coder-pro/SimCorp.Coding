using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleTypeOutputBuilder))]
public class TriangleTypeOutputBuilderTest
{
    [Test]
    [TestCase(true,  null,             TriangleTypeEnum.Unknown,     "Triangle type is unknown")]
    [TestCase(true,  null,             TriangleTypeEnum.Scalene,     "Triangle type is scalene")]
    [TestCase(true,  null,             TriangleTypeEnum.Equilateral, "Triangle type is equilateral")]
    [TestCase(true,  null,             TriangleTypeEnum.Isosceles,   "Triangle type is isosceles")]
    [TestCase(true,  null,             (TriangleTypeEnum)999,        "Triangle type is 999")]
    [TestCase(false, null,             TriangleTypeEnum.Unknown,     "Failed.")]
    [TestCase(false, "Error occurred", TriangleTypeEnum.Unknown,     "Failed.\nError occurred")]
    public void Build_ShouldOutputCorrectMessages(
        bool             isSucceed,
        string?          error,
        TriangleTypeEnum resultEnum,
        string           expectedOutput
    )
    {
        // Arrange
        var mockOutput = new Mock<IOutputDataStrategy>();
        var builder    = new TriangleTypeOutputBuilder(mockOutput.Object);
        var result = isSucceed
            ? Result.Succeed(new TriangleTypeResult { Result = resultEnum })
            : Result.Failed<TriangleTypeResult>(error);

        var outputMessages = new List<string>();
        mockOutput.Setup(o => o.WriteLine(It.IsAny<string>()))
                  .Callback<string>(msg => outputMessages.Add(msg));

        // Act
        builder.Build(result);

        // Assert
        var expectedMessages = expectedOutput.Split('\n');
        outputMessages.Should().BeEquivalentTo(expectedMessages);
    }
}