using NUnit.Framework;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleAreaProcessor))]
public class TriangleAreaProcessorTest
{
    /// <summary>
    /// Integration tests  the <see cref="TriangleAreaProcessor.Process"/> method to ensure it calculates the area of a triangle
    /// correctly based on the provided side lengths.
    /// </summary>
    /// <param name="a">The length of side A of the triangle.</param>
    /// <param name="b">The length of side B of the triangle.</param>
    /// <param name="c">The length of side C of the triangle.</param>
    /// <param name="isSucceed">
    /// A boolean indicating whether the area calculation is expected to succeed.
    /// If <c>true</c>, the triangle is valid, and the area is calculated.
    /// If <c>false</c>, the triangle is invalid, and the calculation fails.
    /// </param>
    /// <param name="expectedArea">
    /// The expected area of the triangle if the calculation succeeds.
    /// If the calculation fails, this value is ignored.
    /// </param>
    [Test]
    [TestCase(3,  4,  5,  true,  6)]                  // Valid triangle
    [TestCase(5,  12, 13, true,  30)]                 // Valid triangle
    [TestCase(6,  8,  10, true,  24)]                 // Valid triangle
    [TestCase(1,  1,  1,  true,  0.4330127018922193)] // Equilateral triangle
    [TestCase(0,  4,  5,  false, 0)]                  // Invalid triangle (side length zero)
    [TestCase(-3, 4,  5,  false, 0)]                  // Invalid triangle (negative side length)
    [TestCase(1,  2,  3,  false, 0)]                  // Degenerate triangle
    public void CalculateArea_ShouldReturnExpectedArea(
        double  a,
        double  b,
        double  c,
        bool isSucceed,
        double expectedArea
    )
    {
        // Arrange
        var doubleHelper = new DoubleHelper();
        var validator    = new TriangleArgumentsValidator(doubleHelper);
        var processor    = new TriangleAreaProcessor([validator]);
        var arguments    = new TriangleArguments { A = a, B = b, C = c };

        // Act
        var result = processor.Process(arguments);

        // Assert
        if (isSucceed)
        {
            result.IsSucceed.Should().BeTrue();
            result.Value!.Area.Should().BeApproximately(expectedArea, double.Epsilon);
        }
        else
        {
            result.IsSucceed.Should().BeFalse();
            result.Value.Should().Be(null);
        }
    }

    [Test]
    public void Process_ShouldReturnFailedResult_WhenValidationFails()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TriangleArguments>>();
        mockValidator
           .Setup(v => v.Validate(It.IsAny<TriangleArguments>()))
           .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("A", "Invalid value") }));
        var processor = new TriangleAreaProcessor(new[] { mockValidator.Object });
        var arguments = new TriangleArguments { A = 3, B = 4, C = 5 };

        // Act
        var result = processor.Process(arguments);

        // Assert
        result.IsSucceed.Should().BeFalse();
        result.Error.Should().Contain("Invalid value");
    }

    [Test]
    public void Process_ShouldReturnFailedResult_WhenExceptionOccurs()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<TriangleArguments>>();
        mockValidator
           .Setup(v => v.Validate(It.IsAny<TriangleArguments>()))
           .Throws(new Exception("Unexpected error"));
        var processor = new TriangleAreaProcessor(new[] { mockValidator.Object });
        var arguments = new TriangleArguments { A = 3, B = 4, C = 5 };

        // Act
        var result = processor.Process(arguments);

        // Assert
        result.IsSucceed.Should().BeFalse();
        result.Error.Should().Contain("Unexpected error");
    }
}