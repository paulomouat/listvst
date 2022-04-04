using System.Linq;
using Cocona;
using ListVst.OutputFormatters;
using Microsoft.Extensions.Logging;

namespace ListVst
{
    class Program
    {
        private IOutputFormatterRegistry OutputFormatterRegistry { get; }
        private Configuration Configuration { get; }
        private ILogger Logger { get; }
        
        public Program(IOutputFormatterRegistry outputFormatterRegistry, Configuration configuration, ILogger<Program> logger)
        {
            OutputFormatterRegistry = outputFormatterRegistry;
            Configuration = configuration;
            Logger = logger;
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

            var outputFormatter = OutputFormatterRegistry[format!];

            var formatterOptions = new FileOutputFormatterOptions
            {
                Path = file
            };
            
            var all = Configuration.Processors
                .SelectMany(p => p.Process(Configuration.SourcePath!).Result)
                .ToList();

            outputFormatter!.Write(all, formatterOptions);
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
