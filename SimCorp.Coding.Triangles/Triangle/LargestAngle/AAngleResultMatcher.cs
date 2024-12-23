namespace SimCorp.Coding.Triangles;

internal sealed class AAngleResultMatcher : ResultMatcherBase<TriangleArguments, TriangleAngleMatchResult>
{
    protected override Result<TriangleAngleMatchResult> MatchLogic(TriangleArguments arguments)
    {
        return Result.Succeed(
            new TriangleAngleMatchResult()
            {
                IsMatched = true,
                Angle     = Math.Acos((arguments.B * arguments.B + arguments.C * arguments.C - arguments.A * arguments.A) / (2 * arguments.B * arguments.C)) * (180 / Math.PI)
            }
        );
    }
}