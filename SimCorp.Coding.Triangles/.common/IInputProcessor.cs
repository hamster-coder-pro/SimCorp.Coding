namespace SimCorp.Coding.Triangles;

public interface IInputProcessor<in TArguments, TResult>
    where TArguments: IInputArguments
    where TResult: IOutputResult
{
    Result<TResult> Process(TArguments arguments);
}