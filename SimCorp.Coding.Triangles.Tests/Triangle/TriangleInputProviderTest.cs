using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class TriangleInputProviderTest
{
    [Test]
    public void Check()
    {
        // assign
        var inputStrategyMock = new Mock<IInputDataStrategy>();
        var outputStrategyMock = new Mock<IOutputDataStrategy>();
        var sut = new TriangleInputProvider(inputStrategyMock.Object, outputStrategyMock.Object);

        inputStrategyMock.SetupSequence(x => x.ReadLine())
                         .Returns("1")
                         .Returns("2")
                         .Returns("2.5");

        var expectedResult = Result.Succeed(
            new TriangleArguments
            {
                A = 1,
                B = 2,
                C = 2.5
            }
        );

        //IReadOnlyList<double> expectedResult = [1, 2, 2.5];

        // act
        var actualResult = sut.Provide();

        // assert
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the first side:"), Times.Once);
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the second side:"), Times.Once);
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the third side:"), Times.Once);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void ExceptionHanded()
    {
        // assign
        var inputStrategyMock  = new Mock<IInputDataStrategy>();
        var outputStrategyMock = new Mock<IOutputDataStrategy>();
        var sut                = new TriangleInputProvider(inputStrategyMock.Object, outputStrategyMock.Object);
        
        outputStrategyMock.Setup(x => x.WriteLine(It.IsAny<string>())).Throws(() => new Exception("WriteLine Exception"));
        outputStrategyMock.Setup(x => x.Write(It.IsAny<string>())).Throws(() => new Exception("Write Exception"));

        inputStrategyMock.SetupSequence(x => x.ReadLine())
                         .Returns("1")
                         .Returns("2")
                         .Returns("2.5");

        var expectedResult = Result.Failed<TriangleArguments>(
            "WriteLine Exception"
        );
        
        // act
        var actualResult = sut.Provide();

        // assert
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the first side:"),  Times.Once);
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the second side:"), Times.Never);
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the third side:"),  Times.Never);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void ExceptionHanded2()
    {
        // assign
        var inputStrategyMock  = new Mock<IInputDataStrategy>();
        var outputStrategyMock = new Mock<IOutputDataStrategy>();
        var sut                = new TriangleInputProvider(inputStrategyMock.Object, outputStrategyMock.Object);

        inputStrategyMock.Setup(x => x.ReadLine()).Throws(() => new Exception("ReadLine Exception"));

        var expectedResult = Result.Failed<TriangleArguments>(
            "ReadLine Exception"
        );

        // act
        var actualResult = sut.Provide();

        // assert
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the first side:"),  Times.Once);
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the second side:"), Times.Never);
        outputStrategyMock.Verify(x => x.WriteLine("Enter the length of the third side:"),  Times.Never);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}