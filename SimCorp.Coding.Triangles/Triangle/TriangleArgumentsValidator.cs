using FluentValidation;

namespace SimCorp.Coding.Triangles;

/// <summary>
/// Provides validation logic for <see cref="TriangleArguments"/> to ensure the values represent a valid triangle.
/// </summary>
/// <remarks>
/// This validator enforces the following rules:
/// <list type="bullet">
/// <item><description>Each side of the triangle must have a positive length.</description></item>
/// <item><description>The triangle inequality must be satisfied (the sum of the lengths of any two sides must be greater than the length of the remaining side).</description></item>
/// </list>
/// </remarks>
internal sealed class TriangleArgumentsValidator : AbstractValidator<TriangleArguments>
{
    public TriangleArgumentsValidator(IDoubleHelper doubleHelper)
    {
        // Set the cascade mode to stop further validations on the first failure
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        // Rule to ensure side A is positive
        RuleFor(x => x.A)
           .Must(a => doubleHelper.Compare(a, 0) == 1) // Use doubleHelper to compare A with 0
           .WithMessage("Side A length should be positive"); // Error message if validation fails
        // Rule to ensure side B is positive
        RuleFor(x => x.B)
           .Must(b => doubleHelper.Compare(b, 0) == 1) // Use doubleHelper to compare B with 0
           .WithMessage("Side B length should be positive"); // Error message if validation fails
        // Rule to ensure side C is positive
        RuleFor(x => x.C)
           .Must(c => doubleHelper.Compare(c, 0) == 1) // Use doubleHelper to compare C with 0
           .WithMessage("Side C length should be positive"); // Error message if validation fails
        // Rule to ensure the triangle inequality holds
        RuleFor(x => x)
           .Must(x => doubleHelper.Compare(x.A + x.B, x.C) == 1 && // A + B > C
                      doubleHelper.Compare(x.A + x.C, x.B) == 1 && // A + C > B
                      doubleHelper.Compare(x.B + x.C, x.A) == 1)   // B + C > A
           .WithMessage("Not a valid triangle"); // Error message if validation fails
    }
}