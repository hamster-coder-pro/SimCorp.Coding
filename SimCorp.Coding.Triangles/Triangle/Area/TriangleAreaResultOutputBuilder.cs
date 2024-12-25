namespace SimCorp.Coding.Triangles;

internal sealed class TriangleAreaResultOutputBuilder : TypeOutputBuilderBase<TriangleAreaResult>
{
    public TriangleAreaResultOutputBuilder(IOutputDataStrategy output)
        : base(output)
    {
    }

    protected override void ProcessValue(TriangleAreaResult value)
    {
        var str = $"Triangle area is: {value.Area:N2}";
        Output.WriteLine($"Triangle area is: {value.Area:N2}");
    }
}