namespace SimCorp.Coding.Triangles;

internal abstract class AppBase<TInput, TOutput>: IApplication
    where TInput : IInputArguments
    where TOutput : IOutputResult
{
    protected IInputProvider<TInput>           InputProvider  { get; }
    protected IInputProcessor<TInput, TOutput> InputProcessor { get; }
    protected IOutputBuilder<TOutput>          OutputProvider { get; }

    protected AppBase(
        IInputProvider<TInput>           inputProvider,
        IInputProcessor<TInput, TOutput> inputProcessor,
        IOutputBuilder<TOutput>          outputProvider
    )
    {
        InputProvider  = inputProvider;
        InputProcessor = inputProcessor;
        OutputProvider = outputProvider;
    }

    public abstract string Name { get; }

    public virtual void Run()
    {
        var arguments = InputProvider.Provide();
        var result = arguments.IsSucceed == false
            ? Result.Failed<TOutput>(arguments.Error)
            : arguments.Value == null
                ? Result.Failed<TOutput>("arguments.Value is NULL")
                : InputProcessor.Process(arguments.Value);
        OutputProvider.Build(result);
    }
}