using NUnit.Framework;
using Moq;
using FluentAssertions;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleLargestAngleApp))]
public class TriangleLargestAngleAppTest
{
    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleLargestAngleResult>>();
        var mockOutputBuilder  = new Mock<IOutputBuilder<TriangleLargestAngleResult>>();

        // Act
        var app = new TriangleLargestAngleApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputBuilder.Object);

        // Assert
        app.Should().NotBeNull();
    }

    [Test]
    public void Run_ShouldCallInputProviderProvide()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleLargestAngleResult>>();
        var mockOutputBuilder  = new Mock<IOutputBuilder<TriangleLargestAngleResult>>();

        var app = new TriangleLargestAngleApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputBuilder.Object);

        mockInputProvider.Setup(p => p.Provide()).Returns(Result.Failed<TriangleArguments>("Error"));

        // Act
        app.Run();

        // Assert
        mockInputProvider.Verify(p => p.Provide(), Times.Once);
    }

    [Test]
    public void Run_ShouldHandleFailedInputProviderResult()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleLargestAngleResult>>();
        var mockOutputBuilder  = new Mock<IOutputBuilder<TriangleLargestAngleResult>>();

        var app = new TriangleLargestAngleApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputBuilder.Object);

        mockInputProvider.Setup(p => p.Provide()).Returns(Result.Failed<TriangleArguments>("Error"));

        // Act
        app.Run();

        // Assert
        mockOutputBuilder.Verify(b => b.Build(It.Is<Result<TriangleLargestAngleResult>>(r => !r.IsSucceed && r.Error == "Error")), Times.Once);
    }

    [Test]
    public void Run_ShouldHandleNullArgumentsValue()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleLargestAngleResult>>();
        var mockOutputBuilder  = new Mock<IOutputBuilder<TriangleLargestAngleResult>>();

        var app = new TriangleLargestAngleApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputBuilder.Object);

        mockInputProvider.Setup(p => p.Provide()).Returns(Result.Succeed<TriangleArguments>(null));

        // Act
        app.Run();

        // Assert
        mockOutputBuilder.Verify(b => b.Build(It.Is<Result<TriangleLargestAngleResult>>(r => !r.IsSucceed && r.Error == "arguments.Value is NULL")), Times.Once);
    }

    [Test]
    public void Run_ShouldProcessValidArguments()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleLargestAngleResult>>();
        var mockOutputBuilder  = new Mock<IOutputBuilder<TriangleLargestAngleResult>>();

        var app = new TriangleLargestAngleApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputBuilder.Object);

        var arguments = new TriangleArguments { A = 3, B = 4, C = 5 };
        var result    = Result.Succeed(new TriangleLargestAngleResult { Angle = 90 });

        mockInputProvider.Setup(p => p.Provide()).Returns(Result.Succeed(arguments));
        mockInputProcessor.Setup(p => p.Process(arguments)).Returns(result);

        // Act
        app.Run();

        // Assert
        mockInputProcessor.Verify(p => p.Process(arguments), Times.Once);
        mockOutputBuilder.Verify(b => b.Build(result), Times.Once);
    }

    [Test]
    public void Run_ShouldHandleProcessorFailure()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleLargestAngleResult>>();
        var mockOutputBuilder  = new Mock<IOutputBuilder<TriangleLargestAngleResult>>();

        var app = new TriangleLargestAngleApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputBuilder.Object);

        var arguments    = new TriangleArguments { A = 3, B = 4, C = 5 };
        var failedResult = Result.Failed<TriangleLargestAngleResult>("Processing error");

        mockInputProvider.Setup(p => p.Provide()).Returns(Result.Succeed(arguments));
        mockInputProcessor.Setup(p => p.Process(arguments)).Returns(failedResult);

        // Act
        app.Run();

        // Assert
        mockOutputBuilder.Verify(b => b.Build(failedResult), Times.Once);
    }
}