namespace SimCorp.Coding.Triangles;

/// <summary>
/// Represents an application for calculating the largest angle of a triangle.
/// </summary>
/// <remarks>
/// This class is a specialized implementation of <see cref="AppBase{TInput, TOutput}"/> 
/// designed to process triangle-related input and compute the largest angle as the result.
/// </remarks>
internal class TriangleLargestAngleApp : AppBase<TriangleArguments, TriangleLargestAngleResult>
{
    public TriangleLargestAngleApp(
        IInputProvider<TriangleArguments>                              inputProvider,
        IInputProcessor<TriangleArguments, TriangleLargestAngleResult> inputProcessor,
        IOutputBuilder<TriangleLargestAngleResult>                     outputProvider
    )
        : base(inputProvider, inputProcessor, outputProvider)
    {
    }
}