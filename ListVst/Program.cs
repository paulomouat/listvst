using System;
using System.Collections.Generic;
using System.Linq;
using Cocona;
using ListVst.OutputFormatting;
using ListVst.Processing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ListVst
{
    internal class Program
    {
        private IOutputFormatterRegistry OutputFormatterRegistry { get; }
        private IProcessorRegistry ProcessorRegistry { get; }
        private ILogger Logger { get; }
        
        public Program(IOutputFormatterRegistry outputFormatterRegistry, IProcessorRegistry processorRegistry, ILogger<Program> logger)
        {
            OutputFormatterRegistry = outputFormatterRegistry;
            ProcessorRegistry = processorRegistry;
            Logger = logger;
        }

        [Command("save")]
        public void Save(
            [Option("format",
                Description = "The format to use in the output file (e.g. txt, html)",
                StopParsingOptions = false,
                ValueName = "value")]string[] format,
            [Option("file",
                Description = "The output file with the saved list",
                StopParsingOptions = false,
                ValueName = "value")]string[] file,
            [Option("sourcePath",
                Description = "The path to the projects to be inspected (note: will automatically search subfolders)",
                StopParsingOptions = false,
                ValueName = "value")]string sourcePath)
        {
            Logger.LogInformation("List VSTs");

            Logger.LogInformation($"Source path is {sourcePath}");

            try
            {
                ValidateCommandLineParameters(format, file);

                var mappedFormatters = GetFormatters(format, file).ToList();
                if (!mappedFormatters.Any())
                {
                    var message = "There were no output formats nor output files specified.";
                    throw new ArgumentException(message);
                }

                Logger.LogInformation("Will process list into the following {OutputCount} outputs:", mappedFormatters.Count());
                foreach (var mappedFormatter in mappedFormatters)
                {
                    Logger.LogInformation("  - Format '{OutputFormat}' in file '{OutputFile}'", mappedFormatter.Format, mappedFormatter.File);
                }
                
                var all = ProcessorRegistry.Processors
                    .SelectMany(p => p.Process(sourcePath).Result)
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
            catch (ArgumentException ae)
            {
                Logger.LogCritical(ae, ae.Message);
            }        
        }

        private static void ValidateCommandLineParameters(string[]? formats, string[]? files)
        {
            if (formats is null || !formats.Any())
            {
                throw new ArgumentException("Invalid format. Need to specify at least one output format.");
            }

            if (files is null || !files.Any())
            {
                throw new ArgumentException("Invalid file. Need to specify at least one output file.");
            }            
            
            if (formats.Length != files.Length)
            {
                throw new ArgumentException("Mismatched number of formats and files. Need to specify an equal number of formats and files.");
            }
        }

        private IEnumerable<MappedFormatter> GetFormatters(string[]? format, string[]? file)
        {
            var formats = format!;
            var files = file!;
            
            return MapFormatters(formats, files);
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
                    throw new ArgumentException(message);
                }

                var file = pair.Second;

                var mapped = new MappedFormatter(format, file, formatter);
                return mapped;
            });
            
            return mappedFormatters;
        }
    }
}
