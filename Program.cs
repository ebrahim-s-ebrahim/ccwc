using System.CommandLine;
using System.CommandLine.Invocation;

namespace ccwc
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            var fileArgument = new Argument<FileInfo?>(
                name: "file",
                description: "The required file.");

            var countBytesOption = new Option<FileInfo?>(
                name: "--byte",
                description: "The number of bytes in a file.");


            var rootCommand = new RootCommand("Word counter command line tool");
            rootCommand.AddArgument(fileArgument);
            rootCommand.AddOption(countBytesOption);

            rootCommand.SetHandler((file) =>
            {
                if (file != null)
                {
                    var bytes = GetFileSizeInBytes(file);
                    Console.WriteLine($"Byte count: {bytes}");
                }
                else
                {
                    Console.WriteLine("File argument not provided.");
                }
            }, countBytesOption);

            

            return await rootCommand.InvokeAsync(args);
        }

        static long GetFileSizeInBytes(FileInfo file)
        {
            try
            {
                using (var fileStream = file.OpenRead())
                {
                    return fileStream.Length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -1;
            }
        }
    }
}