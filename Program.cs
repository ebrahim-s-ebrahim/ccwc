using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.IO.Pipes;
using System.Text.RegularExpressions;

namespace ccwc
{
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

            return await rootCommand.InvokeAsync(args);

        }


        static void CountHandler(FileInfo file, bool countBytes, bool countWords, bool countLines)
        {
            if (file == null)
            {
                Console.WriteLine("File not provided");
                return;
            }

            if (countBytes)
            {
                var bytes = GetBytes(file);
                Console.WriteLine($"Bytes: {bytes}");
            }

            if (countWords)
            {
                var words = GetWords(file);
                Console.WriteLine($"Words: {words}");
            }

            if (countLines)
            {
                var lines = GetLines(file);
                Console.WriteLine($"Lines: {lines}");
            }
        }


        private static long GetLines(FileInfo file)
        {
            var myfilestream = file.OpenRead();

            string fileContents;
            using (StreamReader reader = new StreamReader(myfilestream))
            {
                fileContents = reader.ReadToEnd();
            }
            var lines = Regex.Matches(fileContents, "\n").Count + 1;
            return lines;
        }

        private static long GetWords(FileInfo file)
        {
            var myfilestream = file.OpenRead();

            string fileContents;
            using (StreamReader reader = new StreamReader(myfilestream))
            {
                fileContents = reader.ReadToEnd();
            }
            var words = Regex.Matches(fileContents, @"\b\w+\b").Count;
            return words;
        }

        private static long GetBytes(FileInfo file)
        {
            long bytes;
            using (var fileStream = file.OpenRead())
            {
                bytes = fileStream.Length;
            }

            return bytes;
        }
    }

}