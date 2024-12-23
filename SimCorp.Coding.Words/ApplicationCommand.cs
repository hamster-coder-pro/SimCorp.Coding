using System.CommandLine;
using System.CommandLine.Parsing;

namespace SimCorp.Coding.Words;

internal class ApplicationCommand : Command
{
    public ApplicationCommand()
        : base("app-command", "Words occurence application")
    {
        FilesOption = new Option<IEnumerable<FileInfo>>("--files")
        {
            Arity = ArgumentArity.ZeroOrMore
        };
        AddOption(FilesOption);

        BufferSizeOption = new Option<int>(
            name: "--buffer-size",
            description: "Buffer size",
            parseArgument: ParseSizeInt32
        );
        BufferSizeOption.SetDefaultValue(128 * 1024 * 1024);
        AddOption(BufferSizeOption);

        TaskCountOption = new Option<int>(
            name: "--task-count",
            description: "Count of parallel task which generates data (heavy process)"
        );
        TaskCountOption.SetDefaultValue(Environment.ProcessorCount);
        AddOption(TaskCountOption);

        PrintResultsOption = new Option<bool>(
            name: "--print-results",
            description: "Print results (not recommended for large files)"
        );
        PrintResultsOption.SetDefaultValue(false);
        AddOption(PrintResultsOption);

        GenerateFilesOption = new Option<int>(
            name: "--generate-files",
            description: "Count of files to generate"
        );
        TaskCountOption.SetDefaultValue(0);
        AddOption(GenerateFilesOption);

        WordsPerFile = new Option<int>(
            name: "--words-per-file",
            description: "Count words in file"
        );
        TaskCountOption.SetDefaultValue(1000000);
        AddOption(WordsPerFile);
    }

    int ParseSizeInt32(ArgumentResult result)
    {
        var input = result.Tokens[0].Value; // Get the provided value
        try
        {
            return (int)SizeParser.ParseSizeString(input);
        }
        catch (Exception exception)
        {
            result.ErrorMessage = $"Invalid size value: {input} ({exception.Message})";
            return default;
        }
    }

    private Option<IEnumerable<FileInfo>> FilesOption { get; }

    private Option<int> BufferSizeOption { get; }

    private Option<int> TaskCountOption { get; }

    private Option<bool> PrintResultsOption { get; }

    private Option<int> GenerateFilesOption { get; }

    private Option<int> WordsPerFile { get; }

    public void SetHandler(ApplicationCommandHandlerDelegate handler)
    {
        this.SetHandler(
            context => handler(
                context.ParseResult.GetValueForOption(FilesOption)!.ToArray(),
                context.ParseResult.GetValueForOption(BufferSizeOption),
                context.ParseResult.GetValueForOption(TaskCountOption),
                context.ParseResult.GetValueForOption(PrintResultsOption),
                context.ParseResult.GetValueForOption(GenerateFilesOption),
                context.ParseResult.GetValueForOption(WordsPerFile),
                context.GetCancellationToken()
            )
        );
    }
}