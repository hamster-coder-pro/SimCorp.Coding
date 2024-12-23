namespace SimCorp.Coding.Triangles;

internal class TriangleIsScaleneResultMatcher : ResultMatcherBase<TriangleArguments, TriangleTypeMatchResult>
{
    public override bool IsDefault { get; } = true;

    protected override Result<TriangleTypeMatchResult> MatchLogic(TriangleArguments arguments)
    {
        return Result.Succeed(
            new TriangleTypeMatchResult()
            {
                IsMatched = true,
                Result    = TriangleTypeEnum.Scalene
            }
        );
    }
}