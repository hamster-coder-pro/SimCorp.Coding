namespace SimCorp.Coding.Triangles;

internal sealed class TriangleLargestAngleResultOutputBuilder : TypeOutputBuilderBase<TriangleLargestAngleResult>
{
    public TriangleLargestAngleResultOutputBuilder(IOutputDataStrategy output)
        : base(output)
    {
    }

    protected override void ProcessValue(TriangleLargestAngleResult value)
    {
        Output.WriteLine($"Triangle largest angle is {value.Angle:N2} degrees");
    }
}