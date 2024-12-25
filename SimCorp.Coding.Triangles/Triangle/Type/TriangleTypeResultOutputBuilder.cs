namespace SimCorp.Coding.Triangles;

/// <summary>
/// Responsible for building output messages for triangle type results.
/// </summary>
/// <remarks>
/// This class processes instances of <see cref="TriangleTypeResult"/> and generates
/// appropriate output messages based on the triangle type. It extends the functionality
/// of <see cref="TypeOutputBuilderBase{T}"/> to handle triangle-specific logic.
/// </remarks>
internal sealed class TriangleTypeResultOutputBuilder : TypeOutputBuilderBase<TriangleTypeResult>
{
    public TriangleTypeResultOutputBuilder(IOutputDataStrategy output)
        : base(output)
    {
    }

    protected override void ProcessValue(TriangleTypeResult value)
    {
        switch (value.Result)
        {
            case TriangleTypeEnum.Unknown:
                Output.WriteLine("Triangle type is unknown");
                break;
            case TriangleTypeEnum.Scalene:
                Output.WriteLine("Triangle type is scalene");
                break;
            case TriangleTypeEnum.Equilateral:
                Output.WriteLine("Triangle type is equilateral");
                break;
            case TriangleTypeEnum.Isosceles:
                Output.WriteLine("Triangle type is isosceles");
                break;
            default:
                Output.WriteLine($"Triangle type is {value.Result:G}");
                break;
        }
    }
}