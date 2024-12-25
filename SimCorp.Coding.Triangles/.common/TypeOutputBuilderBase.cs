namespace SimCorp.Coding.Triangles;

/// <summary>
/// Serves as a base class for building output messages for specific result types.
/// </summary>
/// <typeparam name="T">
/// The type of the result that implements <see cref="IOutputResult"/>.
/// </typeparam>
/// <remarks>
/// This abstract class provides a framework for processing and outputting result data.
/// Derived classes must implement the <see cref="ProcessValue(T)"/> method to handle
/// specific logic for processing the result value.
/// </remarks>
internal abstract class TypeOutputBuilderBase<T> : IOutputBuilder<T>
    where T : IOutputResult
{
    protected IOutputDataStrategy Output { get; }

    protected TypeOutputBuilderBase(IOutputDataStrategy output)
    {
        Output = output;
    }

    /// <summary>
    /// Builds the output based on the provided result.
    /// </summary>
    /// <param name="result">
    /// The result object containing the data to process and output. 
    /// If <see cref="Result{T}.IsSucceed"/> is <c>false</c>, an error message is written.
    /// If <see cref="Result{T}.Value"/> is <c>null</c>, a null value message is written.
    /// Otherwise, the result value is processed.
    /// </param>
    public virtual void Build(Result<T> result)
    {
        if (result.IsSucceed == false)
        {
            Output.WriteLine(string.IsNullOrEmpty(result.Error) == false ? $"Failed. {result.Error}" : "Failed.");
            return;
        }

        if (result.Value == null)
        {
            Output.WriteLine("Failed. Value is NULL");
            return;
        }

        ProcessValue(result.Value);
    }

    protected abstract void ProcessValue(T value);
}