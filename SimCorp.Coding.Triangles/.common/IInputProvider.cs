namespace SimCorp.Coding.Triangles;

/// <summary>
/// Defines a contract for providing input arguments of type <typeparamref name="TArguments"/>.
/// </summary>
/// <typeparam name="TArguments">
/// The type of input arguments that this provider supplies. Must implement <see cref="IInputArguments"/>.
/// </typeparam>
public interface IInputProvider<TArguments>
    where TArguments: IInputArguments
{
    /// <summary>
    /// Provides input arguments of type <typeparamref name="TArguments"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Result{TArguments}"/> instance containing the input arguments if successful, 
    /// or an error message if the operation fails.
    /// </returns>
    Result<TArguments> Provide();
}