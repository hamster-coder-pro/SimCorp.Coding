namespace SimCorp.Coding.Triangles;

internal sealed class ConsoleInputOutputDataStrategy : IInputDataStrategy, IOutputDataStrategy
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void Write(string message)
    {
        Console.Write(message);
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}