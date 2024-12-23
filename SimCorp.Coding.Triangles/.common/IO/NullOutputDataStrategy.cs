namespace SimCorp.Coding.Triangles;

internal sealed class NullOutputDataStrategy : IOutputDataStrategy
{
    public void Write(string message)
    {
    }

    public void WriteLine(string message)
    {
    }
}