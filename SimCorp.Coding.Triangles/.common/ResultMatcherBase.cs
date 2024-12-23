namespace SimCorp.Coding.Triangles;

public abstract class ResultMatcherBase<TArguments, TMatchResult> : IResultMatcher<TArguments, TMatchResult>
    where TArguments : IInputArguments
    where TMatchResult : IMatchResult
{
    public virtual int Priority { get; } = 0;

    public virtual bool IsDefault { get; } = false;

    public Result<TMatchResult> Match(TArguments arguments)
    {
        try
        {
            return MatchLogic(arguments);
        }
        catch (Exception exception)
        {
            return Result.Failed<TMatchResult>(exception);
        }
    }

    protected abstract Result<TMatchResult> MatchLogic(TArguments arguments);
}