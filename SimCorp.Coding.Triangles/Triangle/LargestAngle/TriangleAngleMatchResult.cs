namespace SimCorp.Coding.Triangles;

internal sealed class TriangleAngleMatchResult : IMatchResult
{
    public bool IsMatched { get; init; }

    public double Angle { get; init; }
}