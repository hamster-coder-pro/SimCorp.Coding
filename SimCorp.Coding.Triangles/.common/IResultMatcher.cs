namespace SimCorp.Coding.Triangles;

public interface IResultMatcher<in TArguments, TMatchResult>
    where TArguments: IInputArguments
    where TMatchResult: IMatchResult
{
    int Priority { get; }

    bool IsDefault { get; }

    Result<TMatchResult> Match(TArguments arguments);
}