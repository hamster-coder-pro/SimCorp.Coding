using System.Globalization;

namespace SimCorp.Coding.Triangles;

/// <summary>
/// Provides input handling logic for obtaining the lengths of the sides of a triangle.
/// </summary>
/// <remarks>
/// This class is responsible for interacting with input and output strategies to gather
/// the necessary data to construct a <see cref="TriangleArguments"/> instance.
/// It extends the <see cref="InputProviderBase{TArguments}"/> class, specifically
/// tailored for triangle-related input.
/// </remarks>
internal sealed class TriangleArgumentsInputProvider : InputProviderBase<TriangleArguments>
{
    private IInputDataStrategy InputDataStrategy { get; }

    private IOutputDataStrategy OutputDataStrategy { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TriangleArgumentsInputProvider"/> class.
    /// </summary>
    /// <param name="inputDataStrategy">
    /// The strategy used to handle input data for obtaining the lengths of the triangle's sides.
    /// </param>
    /// <param name="outputDataStrategy">
    /// The strategy used to handle output data for providing prompts and feedback during input.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="inputDataStrategy"/> or <paramref name="outputDataStrategy"/> is <c>null</c>.
    /// </exception>
    public TriangleArgumentsInputProvider(IInputDataStrategy inputDataStrategy, IOutputDataStrategy outputDataStrategy)
    {
        InputDataStrategy  = inputDataStrategy;
        OutputDataStrategy = outputDataStrategy;
    }

    /// <inheritdoc />
    protected override Result<TriangleArguments> ProvideLogic()
    {
        // Prompt the user to input the length of the first side of the triangle
        OutputDataStrategy.WriteLine("Enter the length of the first side:");
        // Parse the input string into a double value using invariant culture
        var a = double.Parse(InputDataStrategy.ReadLine() ?? "", CultureInfo.InvariantCulture);
        // Prompt the user to input the length of the second side of the triangle
        OutputDataStrategy.WriteLine("Enter the length of the second side:");
        // Parse the input string into a double value using invariant culture
        var b = double.Parse(InputDataStrategy.ReadLine() ?? "", CultureInfo.InvariantCulture);
        // Prompt the user to input the length of the third side of the triangle
        OutputDataStrategy.WriteLine("Enter the length of the third side:");
        // Parse the input string into a double value using invariant culture
        var c = double.Parse(InputDataStrategy.ReadLine() ?? "", CultureInfo.InvariantCulture);
        // Return a successful result containing the triangle arguments
        return Result.Succeed(
            new TriangleArguments
            {
                A = a, // Assign the first side length
                B = b, // Assign the second side length
                C = c, // Assign the third side length
            }
        );
    }
}