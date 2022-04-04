using System.Linq;
using Cocona;
using ListVst.OutputFormatters;
using Microsoft.Extensions.Logging;

namespace ListVst
{
    class Program
    {
        private ILogger Logger { get; }
        private Configuration Configuration { get; }
        
        public Program(Configuration configuration, ILogger<Program> logger)
        {
            Logger = logger;
            Configuration = configuration;
        }

        [Command("save")]
        public void Save(
            [Option("format",
                Description = "The format to use in the output file (e.g. txt, html)",
                StopParsingOptions = false,
                ValueName = "value")]string? format,
            [Option("file",
                Description = "The output file with the saved list",
                StopParsingOptions = false,
                ValueName = "value")]string? file,
            [Option("sourcePath",
                Description = "The path to the projects to be inspected (note: will automatically search subfolders)",
                StopParsingOptions = false,
                ValueName = "value")]string? sourcePath)
        {
            // TODO: Implement support for multiple outputs
            format = ApplyPrecedence(Configuration.Outputs.FirstOrDefault()?.Format, format);
            file = ApplyPrecedence(Configuration.Outputs.FirstOrDefault()?.Path, file);
            sourcePath = ApplyPrecedence(Configuration.SourcePath, sourcePath);
            
            Logger.LogInformation("List VSTs");
            Logger.LogInformation($"Source path is {sourcePath}");
            Logger.LogInformation($"Output is in format {format} into file {file}");

            // TODO: Instantiate through output formatter registry
            var outputFormatter = new TxtFile(file!);
            
            var all = Configuration.Processors
                .SelectMany(p => p.Process(Configuration.SourcePath!).Result)
                .ToList();

            outputFormatter.Write(all);
        }

        private static string? ApplyPrecedence(string? configurationValue, string? overrideValue)
        {
            var result = configurationValue;

            if (!string.IsNullOrWhiteSpace(overrideValue))
            {
                result = overrideValue;
            }

            return result;
        }
    }
}
