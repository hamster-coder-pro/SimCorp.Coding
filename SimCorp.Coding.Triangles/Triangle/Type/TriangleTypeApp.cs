namespace SimCorp.Coding.Triangles;

internal class TriangleTypeApp : AppBase<TriangleArguments, TriangleTypeResult>
{
    public TriangleTypeApp(
        IInputProvider<TriangleArguments>                            inputProvider,
        IInputProcessor<TriangleArguments, TriangleTypeResult> inputProcessor,
        IOutputBuilder<TriangleTypeResult>                     outputProvider
    )
        : base(inputProvider, inputProcessor, outputProvider)
    {
    }
}