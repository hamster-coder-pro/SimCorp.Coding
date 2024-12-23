namespace SimCorp.Coding.Triangles;

/// <summary>
/// Serves as a base class for input providers that supply input arguments of type <typeparamref name="TArguments"/>.
/// </summary>
/// <typeparam name="TArguments">
/// The type of input arguments that this provider handles. Must implement <see cref="IInputArguments"/>.
/// </typeparam>
/// <remarks>
/// This class provides a template method pattern for input provision, where the actual logic for obtaining input
/// is implemented in the <see cref="ProvideLogic"/> method by derived classes.
/// </remarks>
public abstract class InputProviderBase<TArguments> : IInputProvider<TArguments>
    where TArguments : IInputArguments
{
    /// <inheritdoc />
    public Result<TArguments> Provide()
    {
        try
        {
            return ProvideLogic();
        }
        catch (Exception exception)
        {
            return Result.Failed<TArguments>(exception);
        }
    }

    /// <summary>
    /// Implements the logic for providing input arguments of type <typeparamref name="TArguments"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Result{TArguments}"/> instance containing the input arguments if successful, 
    /// or an error message if the operation fails.
    /// </returns>
    /// <remarks>
    /// Derived classes must override this method to supply the specific logic for obtaining input arguments.
    /// This method is invoked by the <see cref="Provide"/> method, which handles exceptions and wraps the result.
    /// </remarks>
    protected abstract Result<TArguments> ProvideLogic();
}