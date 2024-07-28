using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing;

public abstract class ProcessorBase(ILogger logger) : IProcessor
{
    protected abstract string ProjectType { get; }
    protected abstract string FileExtension { get; }
    protected abstract Func<string, bool> FileFilter { get; }
        
    private ILogger Logger { get; } = logger;
    private string? SourcePath { get; set; }

    public virtual async Task<IEnumerable<ParsedRecord>> Process(string sourcePath)
    {
        SourcePath = sourcePath;
            
        var results = new ConcurrentBag<ParsedRecord>();

        var fl = new FileList(sourcePath);
        var files = fl.GetFiles(FileExtension, FileFilter);

        await Parallel.ForEachAsync(files, async (file, _) =>
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
        
    protected virtual async Task<IEnumerable<ParsedRecord>> ProcessFile(string file)
    {
        Logger.LogInformation("Processing " + ProjectType + " project {File}", file);
            
        var pf = GetProjectFile(file);
        await pf.Read();
        var c = pf.Contents;

        if (string.IsNullOrEmpty(c))
        {
            return Array.Empty<ParsedRecord>();
        }
            
        var p = GetParser(Logger);
        var pluginNames = await p.Parse(c);

        var rawPath = file;
        if (rawPath.StartsWith(SourcePath!))
        {
            rawPath = rawPath[SourcePath!.Length..];
        }
        
        var list = pluginNames.Select(pi => new ParsedRecord(pi, rawPath)).ToList();

        return list;
    }
}