namespace SimCorp.Coding.Triangles;

internal sealed class NullInputDataStrategy : IInputDataStrategy
{
    public string? ReadLine()
    {
        return default;
    }
}