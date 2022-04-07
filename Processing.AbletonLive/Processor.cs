using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace ListVst.Processing.AbletonLive
{
    public class Processor : IProcessor
    {
        private ILogger Logger { get; }

        public Processor(ILogger<Processor> logger)
        {
            Logger = logger;
        }
        
        public async Task<IEnumerable<PluginDescriptor>> Process(string sourcePath)
        {
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
                
            var p = new Parser();
            var vsts = p.Parse(c);
            var list = vsts.Select(vst => new PluginDescriptor(file, vst)).ToList();
            return list;
        }
    }
}
