using FluentValidation;

namespace SimCorp.Coding.Triangles;

internal sealed class TriangleLargestAngleProcessor : InputProcessorBase<TriangleArguments, TriangleLargestAngleResult, TriangleAngleMatchResult>
{
    public TriangleLargestAngleProcessor(
        IEnumerable<IValidator<TriangleArguments>>                               validators,
        IEnumerable<IResultMatcher<TriangleArguments, TriangleAngleMatchResult>> resultMatcher
    )
        : base(validators, resultMatcher)
    {
    }

    protected override bool BreakOnMatchFound { get; } = false;

    protected override TriangleLargestAngleResult? AggregateMatch(TriangleLargestAngleResult? resultValue, TriangleAngleMatchResult matchResult)
    {
        // this is example of processing all the matchers without break execution after match found.
        // slightly stupid logic (not require splitting) but I did this for demo needs.

        return new TriangleLargestAngleResult()
        {
            Angle = Math.Max(resultValue?.Angle ?? 0, matchResult.Angle)
        };
    }
}