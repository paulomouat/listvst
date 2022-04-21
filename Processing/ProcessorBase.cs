using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing;

public abstract class ProcessorBase : IProcessor
{
    public abstract string ProjectType { get; }
    public abstract string FileExtension { get; }
    public abstract Func<string, bool> FileFilter { get; }
        
    private ILogger Logger { get; }
    private string? SourcePath { get; set; }

    protected ProcessorBase(ILogger logger)
    {
        Logger = logger;
    }
        
    public virtual async Task<IEnumerable<PluginRawData>> Process(string sourcePath)
    {
        SourcePath = sourcePath;
            
        var results = new ConcurrentBag<PluginRawData>();

        var fl = new FileList(sourcePath);
        var files = fl.GetFiles(FileExtension, FileFilter);

        await Parallel.ForEachAsync(files, async (file, token) =>
        {
            var batch = await ProcessFile(file);
            foreach (var pair in batch)
            {
                results.Add(pair);
            }
        });

        return results;
    }

    protected abstract IProjectFile GetProjectFile(string file);
    protected abstract IParser GetParser(ILogger logger);
        
    protected virtual async Task<IEnumerable<PluginRawData>> ProcessFile(string file)
    {
        Logger.LogInformation("Processing " + ProjectType + " project {File}", file);
            
        var pf = GetProjectFile(file);
        await pf.Read();
        var c = pf.Contents;

        if (string.IsNullOrEmpty(c))
        {
            return Array.Empty<PluginRawData>();
        }
            
        var p = GetParser(Logger);
        var pluginNames = await p.Parse(c);

        var rawPath = file;
        if (rawPath.StartsWith(SourcePath!))
        {
            rawPath = rawPath[SourcePath!.Length..];
        }
        
        var list = pluginNames.Select(pi => new PluginRawData((pi.Manufacturer + " " + pi.Name).Trim(), rawPath)).ToList();

        return list;
    }
}