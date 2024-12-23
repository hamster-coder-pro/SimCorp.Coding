namespace SimCorp.Coding.Triangles;

internal class TriangleIsEquilateralResultMatcher : ResultMatcherBase<TriangleArguments, TriangleTypeMatchResult>
{
    private IDoubleHelper DoubleHelper { get; }

    protected override Result<TriangleTypeMatchResult> MatchLogic(TriangleArguments arguments)
    {
        var result = new TriangleTypeMatchResult
        {
            Result    = TriangleTypeEnum.Unknown,
            IsMatched = false
        };

        if (DoubleHelper.Equals(arguments.A, arguments.B) && DoubleHelper.Equals(arguments.B, arguments.C))
        {
            result = new TriangleTypeMatchResult
            {
                Result    = TriangleTypeEnum.Equilateral,
                IsMatched = true
            };
        }

        return Result.Succeed(result);
    }

    public TriangleIsEquilateralResultMatcher(IDoubleHelper doubleHelper)
    {
        DoubleHelper = doubleHelper;
    }
}