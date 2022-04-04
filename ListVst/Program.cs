using System;
using System.Collections.Generic;
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
                ValueName = "value")]string[]? format,
            [Option("file",
                Description = "The output file with the saved list",
                StopParsingOptions = false,
                ValueName = "value")]string[]? file,
            [Option("sourcePath",
                Description = "The path to the projects to be inspected (note: will automatically search subfolders)",
                StopParsingOptions = false,
                ValueName = "value")]string? sourcePath)
        {
            Logger.LogInformation("List VSTs");
            
            Logger.LogInformation($"Source path is {sourcePath}");

            var mappedFormatters = GetFormattersFromCommandLine(format, file);

            if (!mappedFormatters.Any())
            {
                mappedFormatters = GetFormattersFromConfiguration();
            }
            
            if (!mappedFormatters.Any())
            {
                var message = "There were no output formats nor output files specified as parameters or through configuration.";
                Logger.LogCritical(message);
                throw new ArgumentException(message);
            }
            
            Logger.LogInformation("Will process list into the following {OutputCount} outputs:", mappedFormatters.Count());
            foreach (var mappedFormatter in mappedFormatters)
            {
                Logger.LogInformation("  - Format '{OutputFormat}' in file '{OutputFile}':", mappedFormatter.Format, mappedFormatter.File);
            }
            
            var all = Configuration.Processors
                .SelectMany(p => p.Process(Configuration.SourcePath!).Result)
                .ToList();

            foreach (var mappedFormatter in mappedFormatters)
            {
                var formatterOptions = new FileOutputFormatterOptions
                {
                    Path = mappedFormatter.File
                };

                mappedFormatter.Formatter.Write(all, formatterOptions);
            }
        }

        private bool UseCommandLineParameters(string[]? formats, string[]? files)
        {
            var result = false;
            
            if (formats is not null && formats.Any())
            {
                result = files is not null && files.Any();
            }

            return result;
        }

        private bool ValidateCommandLineParameters(IReadOnlyCollection<string> formats, IReadOnlyCollection<string> files)
        {
            var result = false;
            
            if (formats.Any())
            {
                result = formats.Count == files.Count;
            }

            return result;
        }

        private bool UseConfiguredParameters(IEnumerable<OutputDetails>? configured)
        {
            var result = configured is not null && configured.Any();
            return result;
        }

        private IEnumerable<MappedFormatter> GetFormattersFromCommandLine(string[]? format, string[]? file)
        {
            if (UseCommandLineParameters(format, file))
            {
                var formats = format!;
                var files = file!;
                
                if (!ValidateCommandLineParameters(formats, files))
                {
                    var message = $"The number of formats and the number of files need to be the same.";
                    Logger.LogCritical(message);
                    throw new ArgumentException(message);
                }

                return MapFormatters(formats, files);
            }

            return Array.Empty<MappedFormatter>();
        }
        
        private IEnumerable<MappedFormatter> GetFormattersFromConfiguration()
        {
            if (UseConfiguredParameters(Configuration.Outputs))
            {
                var configured = Configuration.Outputs;
                return MapFormatters(configured);
            }

            return Array.Empty<MappedFormatter>();
        }
        
        private IEnumerable<MappedFormatter> MapFormatters(IEnumerable<string> formats, IEnumerable<string> files)
        {
            var mappedFormatters = formats.Zip(files).Select(pair =>
            {
                var format = pair.First;
                var formatter = OutputFormatterRegistry[format];
                if (formatter is null)
                {
                    var message = $"No formatter registered for format '{format}'.";
                    Logger.LogCritical(message);
                    throw new ArgumentException(message);
                }

                var file = pair.Second;

                var mapped = new MappedFormatter(format, file, formatter);
                return mapped;
            });
            
            return mappedFormatters;
        }
        
        private IEnumerable<MappedFormatter> MapFormatters(IEnumerable<OutputDetails> configured)
        {
            var mappedFormatters = configured.Select(od =>
            {
                var format = od.Format!;
                var formatter = OutputFormatterRegistry[format];
                if (formatter is null)
                {
                    var message = $"No formatter registered for format '{format}'.";
                    Logger.LogCritical(message);
                    throw new ArgumentException(message);
                }

                var file = od.Path!;

                var mapped = new MappedFormatter(format, file, formatter);
                return mapped;
            });

            return mappedFormatters;
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

        private record struct MappedFormatter(string Format, string File, IOutputFormatter Formatter);
    }
}
