using NUnit.Framework;
using Moq;
using FluentAssertions;

namespace SimCorp.Coding.Triangles.Tests;

[TestFixture]
[TestOf(typeof(TriangleAreaApp))]
public class TriangleAreaAppTest
{
    [Test]
    public void HasValidName()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleAreaResult>>();
        var mockOutputProvider = new Mock<IOutputBuilder<TriangleAreaResult>>();

        // Act
        var sut = new TriangleAreaApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputProvider.Object);
        
        // assert
        sut.Name.Should().Be("triangle-area");
    }

    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleAreaResult>>();
        var mockOutputProvider = new Mock<IOutputBuilder<TriangleAreaResult>>();

        // Act
        var app = new TriangleAreaApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputProvider.Object);

        // Assert
        app.Should().NotBeNull();
    }

    [Test]
    public void Run_ShouldProcessAndBuildOutput_WhenInputIsValid()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleAreaResult>>();
        var mockOutputProvider = new Mock<IOutputBuilder<TriangleAreaResult>>();

        var validArguments = new TriangleArguments { A     = 3, B = 4, C = 5 };
        var validResult    = new TriangleAreaResult { Area = 6 };

        mockInputProvider
           .Setup(p => p.Provide())
           .Returns(Result.Succeed(validArguments));

        mockInputProcessor
           .Setup(p => p.Process(validArguments))
           .Returns(Result.Succeed(validResult));

        var app = new TriangleAreaApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputProvider.Object);

        // Act
        app.Run();

        // Assert
        mockOutputProvider.Verify(p => p.Build(It.Is<Result<TriangleAreaResult>>(r => r.IsSucceed && r.Value!.Area == 6)), Times.Once);
    }

    [Test]
    public void Run_ShouldBuildFailedOutput_WhenInputProviderFails()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleAreaResult>>();
        var mockOutputProvider = new Mock<IOutputBuilder<TriangleAreaResult>>();

        mockInputProvider
           .Setup(p => p.Provide())
           .Returns(Result.Failed<TriangleArguments>("Input error"));

        var app = new TriangleAreaApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputProvider.Object);

        // Act
        app.Run();

        // Assert
        mockOutputProvider.Verify(p => p.Build(It.Is<Result<TriangleAreaResult>>(r => !r.IsSucceed && r.Error == "Input error")), Times.Once);
    }

    [Test]
    public void Run_ShouldBuildFailedOutput_WhenArgumentsValueIsNull()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleAreaResult>>();
        var mockOutputProvider = new Mock<IOutputBuilder<TriangleAreaResult>>();

        mockInputProvider
           .Setup(p => p.Provide())
           .Returns(Result.Succeed<TriangleArguments>(null!));

        var app = new TriangleAreaApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputProvider.Object);

        // Act
        app.Run();

        // Assert
        mockOutputProvider.Verify(p => p.Build(It.Is<Result<TriangleAreaResult>>(r => !r.IsSucceed && r.Error == "arguments.Value is NULL")), Times.Once);
    }

    [Test]
    public void Run_ShouldBuildFailedOutput_WhenProcessorFails()
    {
        // Arrange
        var mockInputProvider  = new Mock<IInputProvider<TriangleArguments>>();
        var mockInputProcessor = new Mock<IInputProcessor<TriangleArguments, TriangleAreaResult>>();
        var mockOutputProvider = new Mock<IOutputBuilder<TriangleAreaResult>>();

        var validArguments = new TriangleArguments { A = 3, B = 4, C = 5 };

        mockInputProvider
           .Setup(p => p.Provide())
           .Returns(Result.Succeed(validArguments));

        mockInputProcessor
           .Setup(p => p.Process(validArguments))
           .Returns(Result.Failed<TriangleAreaResult>("Processing error"));

        var app = new TriangleAreaApp(mockInputProvider.Object, mockInputProcessor.Object, mockOutputProvider.Object);

        // Act
        app.Run();

        // Assert
        mockOutputProvider.Verify(p => p.Build(It.Is<Result<TriangleAreaResult>>(r => !r.IsSucceed && r.Error == "Processing error")), Times.Once);
    }
}