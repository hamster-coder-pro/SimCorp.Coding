using System.Collections.Concurrent;
using System.CommandLine;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimCorp.Coding.Words
{
    /// <summary>
    /// There are no extension points in this project for fasted developments as well as no tests implemented...
    /// <remarks>You may find all kings of tests and extensions points in SimCorp.Coding.Triangles</remarks>
    /// Just using System.CommandLine to parse console arguments and process the request.
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var rootCommand = new ApplicationCommand();
            rootCommand.SetHandler(ApplicationCommandExecuteAsync);

            Console.WriteLine("STARTED");
            Console.WriteLine("=======");

            Stopwatch sw = Stopwatch.StartNew();
            await rootCommand.InvokeAsync(args);
            sw.Stop();

            Console.WriteLine("=======");
            Console.WriteLine("DONE");
            Console.WriteLine($"Elapsed time: {sw.Elapsed}");
        }


        private static ConcurrentDictionary<string, int> Words { get; } = new ();

        private static async Task ApplicationCommandExecuteAsync(
            IReadOnlyList<FileInfo> files,
            int                     bufferSize,
            int                     taskCount,
            bool                    printResults,
            int                     generateFilesCount,
            int                     wordsPerFile,
            CancellationToken       cancellationToken
        )
        {
            try
            {
                // in case autogenerate requested
                if (generateFilesCount > 0)
                {
                    // generate files
                    files = Enumerable.Range(0, generateFilesCount).Select(_ => Path.GetTempFileName()).Select(fileName => new FileInfo(fileName)).ToArray();
                    await GenerateFilesAsync(files, wordsPerFile, bufferSize, taskCount, cancellationToken);
                }
                else
                {
                    // otherwise use provided files
                    foreach (var fileInfo in files)
                    {
                        if (fileInfo.Exists == false)
                        {
                            Console.WriteLine($"File \"{fileInfo.FullName}\" not exists.");
                            return;
                        }
                    }
                }

                // calculate logic
                await CalculateAsync(files, bufferSize, taskCount, cancellationToken);
                
                // print results if requested
                if (printResults)
                {
                    foreach (var word in Words)
                    {
                        Console.WriteLine($"{word.Value}: {word.Key}");
                    }
                }
            }
            finally
            {
                if (generateFilesCount > 0)
                {
                    RemoveTempFiles(files);
                }
            }
        }

        /// <summary>
        /// Processes a collection of files to calculate word occurrences asynchronously.
        /// </summary>
        /// <param name="files">The collection of files to process.</param>
        /// <param name="bufferSize">The size of the buffer used for reading files.</param>
        /// <param name="taskCount">The maximum number of concurrent tasks to process files.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private static async Task CalculateAsync(IReadOnlyList<FileInfo> files, int bufferSize, int taskCount, CancellationToken cancellationToken)
        {
            Console.WriteLine("Calculating occurrences...");
            Stopwatch sw = Stopwatch.StartNew();
            var       semaphore     = new SemaphoreSlim(taskCount);
            var tasks = files.Select(async fileInfo =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    await ProcessFile(fileInfo, bufferSize, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList();
            await Task.WhenAll(tasks);
            sw.Stop();
            Console.WriteLine($"Calculated occurrences: {sw.Elapsed}");
        }

        
        /// <summary>
        /// Deletes temporary files from the specified collection of files.
        /// </summary>
        /// <param name="files">The collection of files to be deleted if they exist.</param>
        private static void RemoveTempFiles(IReadOnlyList<FileInfo> files)
        {
            foreach (var fileInfo in files)
            {
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
        }

        /// <summary>
        /// Generates a collection of files asynchronously, each containing a specified number of random words.
        /// </summary>
        /// <param name="files">The collection of files to generate.</param>
        /// <param name="wordsPerFile">The number of words to generate in each file.</param>
        /// <param name="bufferSize">The size of the buffer used for writing files.</param>
        /// <param name="taskCount">The maximum number of concurrent tasks to generate files.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous file generation operation.</returns>
        private static async Task GenerateFilesAsync(
            IReadOnlyList<FileInfo> files,
            int                     wordsPerFile,
            int                     bufferSize,
            int                     taskCount,
            CancellationToken       cancellationToken
        )
        {
            Console.WriteLine("Generating files...");
            Stopwatch sw = Stopwatch.StartNew();

            var semaphore = new SemaphoreSlim(taskCount);
            var tasks = files.Select(async fileInfo =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    await GenerateFileAsync(fileInfo, bufferSize, wordsPerFile, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList();
            await Task.WhenAll(tasks);
            sw.Stop();
            Console.WriteLine($"Files generated: {sw.Elapsed}");
        }

        /// <summary>
        /// Generates a single file asynchronously, containing a specified number of random words.
        /// </summary>
        /// <param name="fileInfo">The file to generate.</param>
        /// <param name="bufferSize">The size of the buffer used for writing the file.</param>
        /// <param name="wordsCount">The number of words to generate in the file.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous file generation operation.</returns>
        private static async Task GenerateFileAsync(
            FileInfo          fileInfo,
            int               bufferSize,
            int               wordsCount,
            CancellationToken cancellationToken
        )
        {
            var faker     = new Bogus.Faker();
            var random    = new Random();
            var multiline = faker.Random.Bool();

            await using var writer = new StreamWriter(fileInfo.FullName, false, Encoding.UTF8, bufferSize);

            for (var i = 0; i < wordsCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var word = faker.Random.Word();

                if (multiline)
                {
                    if (random.Next(1, 5) == 3)
                    {
                        await writer.WriteLineAsync(word);
                        continue;
                    }
                }

                var whitespace = new string(' ', random.Next(1, 5)); // Randomize whitespace (1 to 4 spaces)
                await writer.WriteAsync(word + whitespace);
            }

            await writer.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="bufferSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task ProcessFile(FileInfo fileInfo, int bufferSize, CancellationToken cancellationToken)
        {
            var count = 0;
            await foreach (var word in ReadWords(fileInfo.FullName, bufferSize, cancellationToken))
            {
                count++;
                Words.AddOrUpdate(word, 1, (_, current) => current+1);
            }
            Console.WriteLine(count);
        }


        /// <summary>
        /// Reads words asynchronously from a file, yielding each word as it is read.
        /// </summary>
        /// <param name="fileName">The full path of the file to read words from.</param>
        /// <param name="bufferSize">The size of the buffer used for reading the file.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An asynchronous stream of words read from the file.</returns>
        private static async IAsyncEnumerable<string> ReadWords(string fileName, int bufferSize, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var       buffer      = new char[bufferSize];
            using var reader      = new StreamReader(fileName);
            var       wordBuilder = new StringBuilder();

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var charsRead = await reader.ReadAsync(buffer, 0, bufferSize);

                for (var i = 0; i < charsRead; i++)
                {
                    var currentChar = buffer[i];

                    if (char.IsWhiteSpace(currentChar))
                    {
                        if (wordBuilder.Length > 0)
                        {
                            yield return wordBuilder.ToString();
                            wordBuilder.Clear();
                        }
                    }
                    else
                    {
                        wordBuilder.Append(currentChar);
                    }
                }
            }

            // Yield the last word if any
            if (wordBuilder.Length > 0)
            {
                yield return wordBuilder.ToString();
            }
        }
    }
}