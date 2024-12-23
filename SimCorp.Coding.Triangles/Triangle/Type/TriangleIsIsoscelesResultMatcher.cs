namespace SimCorp.Coding.Triangles;

internal class TriangleIsIsoscelesResultMatcher : ResultMatcherBase<TriangleArguments, TriangleTypeMatchResult>
{
    private IDoubleHelper       DoubleHelper       { get; }

    public override int  Priority  { get; } = 1;
    
    protected override Result<TriangleTypeMatchResult> MatchLogic(TriangleArguments arguments)
    {
        var result = new TriangleTypeMatchResult
        {
            Result    = TriangleTypeEnum.Unknown,
            IsMatched = false
        };

        if (DoubleHelper.Equals(arguments.A, arguments.B) || DoubleHelper.Equals(arguments.B, arguments.C) || DoubleHelper.Equals(arguments.A, arguments.C))
        {
            result = new TriangleTypeMatchResult
            {
                Result    = TriangleTypeEnum.Isosceles,
                IsMatched = true
            };
        }

        return Result.Succeed(result);
    }

    public TriangleIsIsoscelesResultMatcher(IDoubleHelper doubleHelper)
    {
        DoubleHelper       = doubleHelper;
    }
}