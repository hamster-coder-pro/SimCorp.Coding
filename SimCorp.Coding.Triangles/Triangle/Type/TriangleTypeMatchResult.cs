namespace SimCorp.Coding.Triangles;

public class TriangleTypeMatchResult : IMatchResult
{
    public TriangleTypeEnum Result { get; init; }

    public bool IsMatched { get; init; }
}