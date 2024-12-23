using NUnit.Framework;
using Moq;
using FluentAssertions;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleArgumentsValidator))]
public class TriangleArgumentsValidatorTest
{
    private Mock<IDoubleHelper>        _doubleHelperMock = default!;
    private TriangleArgumentsValidator _validator        = default!;

    [SetUp]
    public void SetUp()
    {
        _doubleHelperMock = new Mock<IDoubleHelper>();
        _validator        = new TriangleArgumentsValidator(_doubleHelperMock.Object);
    }

    [Test]
    [TestCase(1,               1,               1,               true)]  // Valid triangle
    [TestCase(0,               1,               1,               false)] // Side A is zero
    [TestCase(1,               0,               1,               false)] // Side B is zero
    [TestCase(1,               1,               0,               false)] // Side C is zero
    [TestCase(-1,              1,               1,               false)] // Side A is negative
    [TestCase(1,               -1,              1,               false)] // Side B is negative
    [TestCase(1,               1,               -1,              false)] // Side C is negative
    [TestCase(1,               2,               3,               false)] // Not a valid triangle
    [TestCase(3,               1,               2,               false)] // Not a valid triangle
    [TestCase(2,               3,               1,               false)] // Not a valid triangle
    [TestCase(double.MaxValue, double.MaxValue, double.MaxValue, true)]  // Large valid triangle
    [TestCase(double.Epsilon,  double.Epsilon,  double.Epsilon,  true)]  // Small valid triangle
    public void Validate_ShouldReturnExpectedResult(
        double a,
        double b,
        double c,
        bool   isValid
    )
    {
        // Arrange
        _doubleHelperMock.Setup(d => d.Compare(It.IsAny<double>(), 0)).Returns<double, double>((x,                  _) => x > 0 ? 1 : -1);
        _doubleHelperMock.Setup(d => d.Compare(It.IsAny<double>(), It.IsAny<double>())).Returns<double, double>((x, y) => x > y ? 1 : x < y ? -1 : 0);

        var arguments = new TriangleArguments { A = a, B = b, C = c };

        // Act
        var result = _validator.Validate(arguments);

        // Assert
        result.IsValid.Should().Be(isValid);
    }

    [Test]
    public void Validate_ShouldReturnErrorMessagesForInvalidTriangle()
    {
        // Arrange
        _doubleHelperMock.Setup(d => d.Compare(It.IsAny<double>(), 0)).Returns<double, double>((x,                  _) => x > 0 ? 1 : -1);
        _doubleHelperMock.Setup(d => d.Compare(It.IsAny<double>(), It.IsAny<double>())).Returns<double, double>((x, y) => x > y ? 1 : x < y ? -1 : 0);

        var arguments = new TriangleArguments { A = 1, B = 2, C = 3 };

        // Act
        var result = _validator.Validate(arguments);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Not a valid triangle");
    }

    [Test]
    public void Validate_ShouldReturnErrorMessagesForNegativeSides()
    {
        // Arrange
        _doubleHelperMock.Setup(d => d.Compare(It.IsAny<double>(), 0)).Returns<double, double>((x, _) => x > 0 ? 1 : -1);

        var arguments = new TriangleArguments { A = -1, B = 1, C = 1 };

        // Act
        var result = _validator.Validate(arguments);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Side A length should be positive");
    }
}