namespace SimCorp.Coding.Triangles;

/// <summary>
/// Provides utility methods for comparing and evaluating double-precision floating-point numbers.
/// </summary>
/// <remarks>
/// This interface is designed to handle operations involving double values with a specified precision,
/// defined by the <see cref="Epsilon"/> property. It includes methods for equality comparison and ordering.
/// </remarks>
public interface IDoubleHelper: IEqualityComparer<double>, IComparer<double>
{
    double Epsilon { get; }
}