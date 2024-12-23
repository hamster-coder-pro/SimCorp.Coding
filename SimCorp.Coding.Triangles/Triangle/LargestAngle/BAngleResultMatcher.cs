namespace SimCorp.Coding.Triangles;

internal sealed class BAngleResultMatcher : ResultMatcherBase<TriangleArguments, TriangleAngleMatchResult>
{
    protected override Result<TriangleAngleMatchResult> MatchLogic(TriangleArguments arguments)
    {
        return Result.Succeed(
            new TriangleAngleMatchResult()
            {
                IsMatched = true,
                Angle     = Math.Acos((arguments.A * arguments.A + arguments.C * arguments.C - arguments.B * arguments.B) / (2 * arguments.A * arguments.C)) * (180 / Math.PI)
            }
        );
    }
}