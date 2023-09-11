using System.CommandLine;
using System.Text.RegularExpressions;

namespace mywc;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        var fileArgument = new Argument<FileInfo?>(
            name: "file",
            description: "The required file.");

        var countBytesOption = new Option<bool>(
            name: "--byte",
            description: "The number of bytes in a file.");

        var countWordsOption = new Option<bool>(
            name: "--word",
            description: "The number of words in a file.");

        var countLinesOption = new Option<bool>(
            name: "--line",
            description: "The number of lines in a file.");

        var rootCommand = new RootCommand("Word counter command line tool");
        rootCommand.AddArgument(fileArgument);
        rootCommand.AddOption(countBytesOption);
        rootCommand.AddOption(countWordsOption);
        rootCommand.AddOption(countLinesOption);

        rootCommand.SetHandler(CountHandler, fileArgument, countBytesOption, countWordsOption, countLinesOption);

        if (args.Length > 0)
        {
            await rootCommand.InvokeAsync(args);
        }
        else
        {
            // Enter the interactive loop for user input.
            bool exitRequested = false;
            while (!exitRequested)
            {
                var commandInput = PromptForCommand();

                if (commandInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    exitRequested = true;
                }
                else
                {
                    var inputArgs = commandInput.Split(' ');

                    if (inputArgs.Length < 2 || string.IsNullOrEmpty(inputArgs[1]))
                    {
                        Console.WriteLine("Please enter the file name:");
                        var fileName = Console.ReadLine();
                        inputArgs = new[] { inputArgs[0], fileName };
                    }

                    await rootCommand.InvokeAsync(inputArgs);
                }
            }
        }

        return 0;
    }


    static void CountHandler(FileInfo file, bool countBytes, bool countWords, bool countLines)
    {
        if (file == null)
        {
            Console.WriteLine("File not provided");
            return;
        }

        string fileContents;
        using (var myfilestream = file.OpenRead())
        using (var reader = new StreamReader(myfilestream))
        {
            fileContents = reader.ReadToEnd();
        }

        if (countBytes)
        {
            var bytes = fileContents.Length;
            Console.WriteLine($"Bytes: {bytes}");
        }

        if (countWords)
        {
            var words = Regex.Matches(fileContents, @"\b\w+\b").Count;
            Console.WriteLine($"Words: {words}");
        }

        if (countLines)
        {
            var lines = Regex.Matches(fileContents, "\n").Count + 1;
            Console.WriteLine($"Lines: {lines}");
        }
    }

    static string PromptForCommand()
    {
        Console.WriteLine("Enter the command and options (or type 'exit' to quit):");
        var commandInput = Console.ReadLine();
        return commandInput;
    }
}

