using System.Text;

namespace SimCorp.Coding.Triangles;

/// <summary>
/// Just an example of file read input
/// </summary>
internal sealed class FileInputDataStrategy : StreamInputDataStrategyBase
{
    private string  FileName   { get; }
    
    private Stream? FileStream { get; set; }

    public FileInputDataStrategy(string fileName)
    {
        FileName = fileName;
    }

    protected override StreamReader CreateReader()
    {
        FileStream = File.OpenRead(FileName);
        return new StreamReader(FileStream, Encoding.UTF8);
    }

    public override void Dispose()
    {
        FileStream?.Dispose();
        base.Dispose();
    }
}