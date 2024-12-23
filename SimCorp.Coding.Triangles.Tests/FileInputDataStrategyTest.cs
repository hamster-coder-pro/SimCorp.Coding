using FluentAssertions;
using NUnit.Framework;

namespace SimCorp.Coding.Triangles.Tests;

public class FileInputDataStrategyTest
{
    [Test]
    public void Valid()
    {
        // assign

        var                   tempFileName    = Path.GetTempFileName();
        IReadOnlyList<string> expectedResults = ["one", "two"];
        var                   sut             = new FileInputDataStrategy(tempFileName);

        try
        {
            File.WriteAllLines(tempFileName, expectedResults);

            foreach (var expectedResult in expectedResults)
            {
                // act
                var line = sut.ReadLine();
                // assert
                line.Should().Be(expectedResult);
            }
        }
        finally
        {
            // cleanup
            sut.Dispose();
            File.Delete(tempFileName);
        }
    }
}

public class NullInputDataStrategyTest
{
    [Test]
    public void Valid()
    {
        // assign
        var                   sut             = new NullInputDataStrategy();
        sut.ReadLine().Should().BeNull();
    }
}

public class NullOutputDataStrategyTest
{
    [Test]
    public void Valid()
    {
        // assign
        var sut = new NullOutputDataStrategy();
        sut.Write("test");
        sut.WriteLine("test");
    }
}

public class ConsoleInputOutputDataStrategyTest
{
    [Test]
    public void ValidRead()

    {
        // assign
        var expected  = "Valid";
        var mockInput = new StringReader(expected);
        Console.SetIn(mockInput);
        
        var sut = new ConsoleInputOutputDataStrategy();
        var actual = sut.ReadLine();

        actual.Should().Be(expected);
    }

    [Test]
    public void ValidWrite()

    {
        // assign
        var mockOutput = new StringWriter();
        Console.SetOut(mockOutput);

        var sut    = new ConsoleInputOutputDataStrategy();
        sut.Write("one");
        sut.Write("two");

        mockOutput.ToString().Should().Be("onetwo");
    }


    [Test]
    public void ValidWriteLine()

    {
        // assign
        var mockOutput = new StringWriter();
        Console.SetOut(mockOutput);

        var sut = new ConsoleInputOutputDataStrategy();
        sut.WriteLine("one");
        sut.WriteLine("two");
        var envnl = Environment.NewLine;
        mockOutput.ToString().Should().Be($"one{envnl}two{envnl}");
    }
}