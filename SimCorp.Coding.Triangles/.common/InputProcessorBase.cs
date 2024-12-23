using FluentValidation;
using FluentValidation.Results;

namespace SimCorp.Coding.Triangles;

internal abstract class InputProcessorBase<TArguments, TResult> : IInputProcessor<TArguments, TResult>
    where TArguments : IInputArguments
    where TResult : IOutputResult
{
    private IReadOnlyList<IValidator<TArguments>> Validators { get; }

    protected InputProcessorBase(
        IEnumerable<IValidator<TArguments>> validators
    )
    {
        Validators = validators.ToArray();
    }

    protected virtual Result<TResult> ValidateArguments(TArguments arguments)
    {
        var validationResult = new ValidationResult();
        foreach (var validator in Validators)
        {
            validationResult = validator.Validate(arguments);
            if (validationResult.IsValid == false)
            {
                break;
            }
        }

        if (validationResult.IsValid == false)
        {
            return Result.Failed<TResult>(validationResult.ToString());
        }

        return Result.Succeed(default(TResult)!);
    }

    public Result<TResult> Process(TArguments arguments)
    {
        try
        {
            var result = ValidateArguments(arguments);
            if (result.IsSucceed == false)
            {
                return result;
            }

            return ProcessLogic(arguments);
        }
        catch (Exception exception)
        {
            return Result.Failed<TResult>(exception);
        }
    }

    protected abstract Result<TResult> ProcessLogic(TArguments arguments);
}

internal abstract class InputProcessorBase<TArguments, TResult, TMatchResult> : InputProcessorBase<TArguments, TResult>
    where TArguments : IInputArguments
    where TResult : IOutputResult
    where TMatchResult : IMatchResult
{
    protected IReadOnlyList<IResultMatcher<TArguments, TMatchResult>> ResultMatcherList { get; }

    protected InputProcessorBase(
        IEnumerable<IValidator<TArguments>>                   validators,
        IEnumerable<IResultMatcher<TArguments, TMatchResult>> resultMatchers
    )
        : base(validators)
    {
        ResultMatcherList = resultMatchers.ToArray();
    }

    protected abstract bool BreakOnMatchFound { get; }

    protected virtual IEnumerable<IResultMatcher<TArguments, TMatchResult>> EnumerateResultMatchers()
    {
        foreach (var matcher in ResultMatcherList.Where(x => x.IsDefault == false).OrderBy(x => x.Priority).Where(x => x.IsDefault == false))
        {
            yield return matcher;
        }
        
        var defaultMatcher = ResultMatcherList.FirstOrDefault(x => x.IsDefault);
        if (defaultMatcher != null)
        {
            yield return defaultMatcher;
        }
    }
    
    protected override Result<TResult> ProcessLogic(TArguments arguments)
    {
        TResult? resultValue = default;

        foreach (var resultMatcher in EnumerateResultMatchers())
        {
            var matchResult = resultMatcher.Match(arguments);
            if (matchResult.IsSucceed == false)
            {
                return Result.Failed<TResult>(matchResult.Error);
            }

            if (matchResult.Value == null)
            {
                return Result.Failed<TResult>($"{nameof(matchResult)}.{nameof(matchResult.Value)} is NULL");
            }
            
            if (matchResult.Value.IsMatched)
            {
                resultValue = AggregateMatch(resultValue, matchResult.Value);
                if (BreakOnMatchFound)
                    break;
            }
        }

        if (resultValue != null)
        {
            return Result.Succeed(resultValue);
        }

        return Result.Failed<TResult>("No match found");
    }

    protected abstract TResult? AggregateMatch(TResult? resultValue, TMatchResult matchResult);
}