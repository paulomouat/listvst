using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cocona;
using ListVst.OutputFormatting;
using ListVst.Processing;
using Microsoft.Extensions.Logging;

namespace ListVst;

internal class Program
{
    private IOutputFormatterRegistry OutputFormatterRegistry { get; }
    private IProcessorRegistry ProcessorRegistry { get; }
    private IPluginAliasesRegistry PluginAliasesRegistry { get; }
    private IPluginManufacturersRegistry PluginManufacturersRegistry { get; }
    private ILogger Logger { get; }
        
    public Program(IOutputFormatterRegistry outputFormatterRegistry, IProcessorRegistry processorRegistry,
        IPluginAliasesRegistry pluginAliasesRegistry, IPluginManufacturersRegistry pluginManufacturersRegistry,
        ILogger<Program> logger)
    {
        OutputFormatterRegistry = outputFormatterRegistry;
        ProcessorRegistry = processorRegistry;
        PluginAliasesRegistry = pluginAliasesRegistry;
        PluginManufacturersRegistry = pluginManufacturersRegistry;
        Logger = logger;
    }

    [Command("save")]
    public async Task Save(
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
                
            var rawData = ProcessorRegistry.Processors
                .SelectMany(p => p.Process(sourcePath).Result)
                .ToList();

            var withResolvedAliases = rawData.Select(rd =>
            {
                var current = rd.PluginFullName;
                var proposed = PluginAliasesRegistry[current];
                if (!string.IsNullOrWhiteSpace(proposed) && current != proposed)
                {
                    var adjusted = rd with { PluginFullName = proposed };
                    return adjusted;
                };

                return rd;
            });
            
            var data = withResolvedAliases.Select(rd =>
            {
                var projectDescriptor = new ProjectDescriptor(rd.ProjectPath);

                var fullName = rd.PluginFullName;
                var name = fullName;
                var manufacturer = PluginManufacturersRegistry.GetManufacturer(fullName);
                if (!string.IsNullOrEmpty(manufacturer))
                { 
                    name = name[manufacturer.Length..].Trim();
                }

                var pluginDescriptor = new PluginDescriptor(name, manufacturer, fullName);

                var pluginData = new PluginData(pluginDescriptor, projectDescriptor);
                return pluginData;
            })
                .Distinct()
                .ToList();

            foreach (var mappedFormatter in mappedFormatters)
            {
                var formatterOptions = new FileOutputFormatterOptions
                {
                    Path = mappedFormatter.File
                };

                await mappedFormatter.Formatter.Write(data, formatterOptions);
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