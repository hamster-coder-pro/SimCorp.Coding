using FluentValidation;

namespace SimCorp.Coding.Triangles;

internal class TriangleTypeProcessor : InputProcessorBase<TriangleArguments, TriangleTypeResult, TriangleTypeMatchResult>
{
    public TriangleTypeProcessor(
        IEnumerable<IValidator<TriangleArguments>> validators,
        IEnumerable<IResultMatcher<TriangleArguments, TriangleTypeMatchResult>> resultMatchers
    )
        : base(validators, resultMatchers)
    {
    }

    protected override bool BreakOnMatchFound { get; } = true;

    protected override TriangleTypeResult? AggregateMatch(TriangleTypeResult? resultValue, TriangleTypeMatchResult matchResult)
    {
        return new TriangleTypeResult()
        {
            Result = matchResult.Result
        };
    }
}