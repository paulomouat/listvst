using Microsoft.Extensions.Logging;

namespace ListVst.StudioOne
{
    public class Processor : IProcessor
    {
        private ILogger Logger { get; }

        public Processor(ILogger<Processor> logger)
        {
            Logger = logger;
        }
        
        public async Task<IEnumerable<(string Path, string Vst)>> Process(string sourcePath)
        {
            var results = new List<(string Path, string Vst)>();

            var fl = new FileList(sourcePath);
            var files = fl.GetFiles("song").Where(f => !f.Contains("(Autosaved)") &&
                !f.Contains("(Before Autosave)") && !f.Contains("/History/"));
            
            foreach (var file in files)
            {
                Logger.LogInformation("Processing Studio One project {File}", file);
                var pf = new ProjectFile(file);
                await pf.Read();
                var c = pf.Contents;

                if (string.IsNullOrEmpty(c))
                {
                    continue;
                }

                var p = new Parser();
                var vsts = p.Parse(c);
                var list = vsts.Select(vst => (file, vst)).ToList();
                results.AddRange(list);
            }

            return results;
        }
    }
}
