using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleAppFramework;
using ListVst.OutputFormatting;
using ListVst.Processing;
using Microsoft.Extensions.Logging;

namespace ListVst;

internal class Program(
    IOutputFormatterRegistry outputFormatterRegistry,
    IProcessorRegistry processorRegistry,
    IPluginRegistry pluginRegistry,
    ILogger<Program> logger)
{
    private IOutputFormatterRegistry OutputFormatterRegistry { get; } = outputFormatterRegistry;
    private IProcessorRegistry ProcessorRegistry { get; } = processorRegistry;
    private IPluginRegistry PluginRegistry { get; } = pluginRegistry;
    private ILogger Logger { get; } = logger;
    
    /// <summary>
    /// List the VSTs in use in Ableton and Studio One projects.
    /// </summary>
    /// <param name="format">The format to use in the output file (e.g. txt, html).</param>
    /// <param name="file">The output file with the saved list.</param>
    /// <param name="sourcePath">The path to the projects to be inspected (note: will automatically search subfolders).</param>
    [Command("save")]
    public async Task Save(string[] format, string[] file, string sourcePath)
    {
        Logger.LogInformation("List VSTs");

        Logger.LogInformation("Source path is {SourcePath}", sourcePath);

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
                
            var parsedRecords = ProcessorRegistry.Processors
                .SelectMany(p => p.Process(sourcePath).Result)
                .ToList();

            var withResolvedAliases = parsedRecords.ResolveAliases(PluginRegistry);

            var pluginRecords = withResolvedAliases.ToPluginRecord().Distinct().ToList();

            foreach (var mappedFormatter in mappedFormatters)
            {
                var formatterOptions = new FileOutputFormatterOptions
                {
                    Path = mappedFormatter.File
                };

                await mappedFormatter.Formatter.Write(pluginRecords, formatterOptions);
            }
        }
        catch (ArgumentException ae)
        {
            Logger.LogCritical(ae, "Error while listing the VSTs.");
        }
            
        Logger.LogInformation("List VSTs done.");
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