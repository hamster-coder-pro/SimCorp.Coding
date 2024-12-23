namespace SimCorp.Coding.Triangles;

internal abstract class StreamInputDataStrategyBase : IInputDataStrategy, IDisposable
{
    private StreamReader? StreamReader { get; set; }

    protected Lazy<StreamReader> StreamReaderLazy { get; }

    protected StreamInputDataStrategyBase()
    {
        StreamReaderLazy = new Lazy<StreamReader>(() =>
        {
            StreamReader = CreateReader();
            return StreamReader;
        });
    }

    protected abstract StreamReader CreateReader();

    public string? ReadLine()
    {
        var reader = StreamReaderLazy.Value;
        return reader.ReadLine();
    }

    public virtual void Dispose()
    {
        StreamReader?.Dispose();
    }
}