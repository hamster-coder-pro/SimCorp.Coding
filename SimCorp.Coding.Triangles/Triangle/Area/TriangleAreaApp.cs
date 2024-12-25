namespace SimCorp.Coding.Triangles;

internal class TriangleAreaApp : AppBase<TriangleArguments, TriangleAreaResult>
{
    public TriangleAreaApp(
        IInputProvider<TriangleArguments>                      inputProvider,
        IInputProcessor<TriangleArguments, TriangleAreaResult> inputProcessor,
        IOutputBuilder<TriangleAreaResult>                     outputProvider
    )
        : base(inputProvider, inputProcessor, outputProvider)
    {
    }

    public override string Name { get; } = "triangle-area";
}