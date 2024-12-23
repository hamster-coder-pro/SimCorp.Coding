namespace SimCorp.Coding.Words;

internal delegate Task ApplicationCommandHandlerDelegate(
    IReadOnlyList<FileInfo> files,
    int                     bufferSize,
    int                     taskCount,
    bool                    printResults,
    int                     generateFilesCount,
    int                     wordsPerFile,
    CancellationToken       cancellationToken
);