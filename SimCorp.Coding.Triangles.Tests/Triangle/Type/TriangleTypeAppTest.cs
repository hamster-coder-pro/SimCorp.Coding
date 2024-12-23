using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleTypeApp))]
public class TriangleTypeAppTest
{
    private Mock<IInputProvider<TriangleArguments>>                      _mockInputProvider  = default!;
    private Mock<IInputProcessor<TriangleArguments, TriangleTypeResult>> _mockInputProcessor = default!;
    private Mock<IOutputBuilder<TriangleTypeResult>>                     _mockOutputBuilder  = default!;
    private TriangleTypeApp                                              _triangleTypeApp    = default!;

    [SetUp]
    public void SetUp()
    {
        _mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        _mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleTypeResult>>();
        _mockOutputBuilder  = new Mock<IOutputBuilder<TriangleTypeResult>>();

        _triangleTypeApp = new TriangleTypeApp(
            _mockInputProvider.Object,
            _mockInputProcessor.Object,
            _mockOutputBuilder.Object
        );
    }

    [Test]
    public void Constructor_ShouldInitializeDependencies()
    {
        _triangleTypeApp.Should().NotBeNull();
    }

    [Test]
    public void Run_ShouldProcessValidInputAndOutputResult()
    {
        // Arrange
        var arguments = new TriangleArguments { A = 3, B = 4, C = 5 };
        var result    = Result.Succeed(new TriangleTypeResult { Result = TriangleTypeEnum.Scalene });

        _mockInputProvider.Setup(p => p.Provide()).Returns(Result.Succeed(arguments));
        _mockInputProcessor.Setup(p => p.Process(arguments)).Returns(result);

        // Act
        _triangleTypeApp.Run();

        // Assert
        _mockInputProvider.Verify(p => p.Provide(), Times.Once);
        _mockInputProcessor.Verify(p => p.Process(arguments), Times.Once);
        _mockOutputBuilder.Verify(b => b.Build(result), Times.Once);
    }

    [Test]
    public void Run_ShouldHandleFailedInputProvider()
    {
        // Arrange
        var errorMessage = "Input error";
        _mockInputProvider.Setup(p => p.Provide()).Returns(Result.Failed<TriangleArguments>(errorMessage));

        // Act
        _triangleTypeApp.Run();

        // Assert
        _mockInputProvider.Verify(p => p.Provide(), Times.Once);
        _mockInputProcessor.Verify(p => p.Process(It.IsAny<TriangleArguments>()), Times.Never);
        _mockOutputBuilder.Verify(b => b.Build(It.IsAny<Result<TriangleTypeResult>>()), Times.Once);
    }

    [Test]
    public void Run_ShouldHandleFailedInputProcessor()
    {
        // Arrange
        var arguments    = new TriangleArguments { A = 3, B = 4, C = 5 };
        var errorMessage = "Processing error";

        _mockInputProvider.Setup(p => p.Provide()).Returns(Result.Succeed(arguments));
        _mockInputProcessor.Setup(p => p.Process(arguments)).Returns(Result.Failed<TriangleTypeResult>(errorMessage));

        // Act
        _triangleTypeApp.Run();

        // Assert
        _mockInputProvider.Verify(p => p.Provide(), Times.Once);
        _mockInputProcessor.Verify(p => p.Process(arguments), Times.Once);
        _mockOutputBuilder.Verify(b => b.Build(It.IsAny<Result<TriangleTypeResult>>()), Times.Once);
    }

    [TestCase(3,  4,  5,  TriangleTypeEnum.Scalene)]
    [TestCase(2,  2,  2,  TriangleTypeEnum.Equilateral)]
    [TestCase(2,  2,  3,  TriangleTypeEnum.Isosceles)]
    public void Run_ShouldHandleVariousTriangleTypes(
        double           a,
        double           b,
        double           c,
        TriangleTypeEnum expectedType
    )
    {
        // Arrange
        var arguments = new TriangleArguments { A = a, B = b, C = c };
        var result    = Result.Succeed(new TriangleTypeResult { Result = expectedType });

        _mockInputProvider.Setup(p => p.Provide()).Returns(Result.Succeed(arguments));
        _mockInputProcessor.Setup(p => p.Process(arguments)).Returns(result);

        // Act
        _triangleTypeApp.Run();

        // Assert
        _mockInputProvider.Verify(p => p.Provide(), Times.Once);
        _mockInputProcessor.Verify(p => p.Process(arguments), Times.Once);
        _mockOutputBuilder.Verify(b => b.Build(result), Times.Once);
    }
}