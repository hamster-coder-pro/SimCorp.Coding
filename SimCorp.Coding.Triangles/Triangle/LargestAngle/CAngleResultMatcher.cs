namespace SimCorp.Coding.Triangles;

internal sealed class CAngleResultMatcher : ResultMatcherBase<TriangleArguments, TriangleAngleMatchResult>
{
    protected override Result<TriangleAngleMatchResult> MatchLogic(TriangleArguments arguments)
    {
        return Result.Succeed(
            new TriangleAngleMatchResult()
            {
                IsMatched = true,
                Angle     = Math.Acos((arguments.A * arguments.A + arguments.B * arguments.B - arguments.C * arguments.C) / (2 * arguments.A * arguments.B)) * (180 / Math.PI)
            }
        );
    }
}