using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing.AbletonLive
{
    public class Processor : IProcessor
    {
        private ILogger Logger { get; }
        private string? SourcePath { get; set; }

        public Processor(ILogger<Processor> logger)
        {
            Logger = logger;
        }
        
        public async Task<IEnumerable<PluginDescriptor>> Process(string sourcePath)
        {
            SourcePath = sourcePath;
            
            var results = new ConcurrentBag<PluginDescriptor>();

            var fl = new FileList(sourcePath);
            var files = fl.GetFiles("als").Where(f => !f.Contains("Backup"));

            await Parallel.ForEachAsync(files, async (file, token) =>
            {
                var batch = await ProcessFile(file);
                foreach (var pd in batch)
                {
                    results.Add(pd);
                }
            });

            return results;
        }

        private async Task<IEnumerable<PluginDescriptor>> ProcessFile(string file)
        {
            Logger.LogInformation("Processing Ableton Live project {File}", file);
            
            var pf = new ProjectFile(file);
            await pf.Read();
            var c = pf.Contents;

            if (string.IsNullOrEmpty(c))
            {
                return Array.Empty<PluginDescriptor>();
            }

            var p = new Parser(Logger);
            var plugins = await p.Parse(c);

            var rawPath = file;
            if (rawPath.StartsWith(SourcePath!))
            {
                rawPath = rawPath[SourcePath!.Length..];
            }
            
            var projectDescriptor = ProjectDescriptor.Parse(rawPath);
            var list = plugins.Select(plugin => new PluginDescriptor(
                projectDescriptor, plugin)).ToList();
            
            return list;
        }
    }
}
