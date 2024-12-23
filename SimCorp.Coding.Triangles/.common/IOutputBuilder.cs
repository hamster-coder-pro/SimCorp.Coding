namespace SimCorp.Coding.Triangles;

public interface IOutputBuilder<TResult>
    where TResult: IOutputResult
{
    void Build(Result<TResult> result);
}