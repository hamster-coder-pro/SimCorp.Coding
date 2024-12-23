using FluentValidation;

namespace SimCorp.Coding.Triangles;

/// <summary>
/// Example of processor without multipart match split of the logic.
/// </summary>
internal sealed class TriangleAreaProcessor : InputProcessorBase<TriangleArguments, TriangleAreaResult>
{
    public TriangleAreaProcessor(IEnumerable<IValidator<TriangleArguments>> validators)
        : base(validators)
    {
    }

    protected override Result<TriangleAreaResult> ProcessLogic(TriangleArguments arguments)
    {
        return Result.Succeed(
            new TriangleAreaResult
            {
                Area = CalculateArea(arguments.A, arguments.B, arguments.C)
            }
        );
    }

    /// <summary>
    /// Calculates the area of a triangle using Heron's formula.
    /// </summary>
    /// <param name="a">Length of side A.</param>
    /// <param name="b">Length of side B.</param>
    /// <param name="c">Length of side C.</param>
    /// <returns>The area of the triangle.</returns>
    private static double CalculateArea(double a, double b, double c)
    {
        // Calculate the semi-perimeter
        var s = (a + b + c) / 2;
        // Calculate the area using Heron's formula
        return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
    }
}