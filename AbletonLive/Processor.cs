namespace ListVst.AbletonLive
{
    public class Processor : IProcessor
    {
        public async Task<IEnumerable<(string Path, string Vst)>> Process(string sourcePath)
        {
            var results = new List<(string Path, string Vst)>();

            var fl = new FileList(sourcePath);
            var files = fl.GetFiles("als").Where(f => !f.Contains("Backup"));
            foreach (var file in files)
            {
                Console.WriteLine($"Processing Ableton Live project {file}");
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
