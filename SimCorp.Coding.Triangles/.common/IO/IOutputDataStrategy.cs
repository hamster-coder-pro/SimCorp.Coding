namespace SimCorp.Coding.Triangles;

public interface IOutputDataStrategy
{
    void Write(string message);

    void WriteLine(string message);
}