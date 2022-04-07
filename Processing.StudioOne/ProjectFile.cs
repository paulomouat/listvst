using System.IO.Compression;

namespace ListVst.Processing.StudioOne
{
    public class ProjectFile : IProjectFile
    {
        public string Name { get; }
        public string Path { get; }
        public string? Contents { get; private set; }

        public ProjectFile(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public async Task Read()
        {
            await using var ms = new MemoryStream();
            await using (var fs = File.OpenRead(Path))
            {
                using (var z = new ZipArchive(fs, ZipArchiveMode.Read))
                {
                    var entry = z.Entries.SingleOrDefault(e => e.Name == "audiomixer.xml");
                    if (entry != null)
                    {
                        await using var es = entry.Open();
                        await es.CopyToAsync(ms);
                    }
                }
            }

            /*var contents = new StringBuilder();
            
            ms.Position = 0;
            using var reader = new StreamReader(ms);
            while(reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    line = line.Trim();
                    line = line.Replace("::", "_");
                    line = line.Replace("x:", "_");
                    contents.Append(line);
                }
            }
            
            Contents = contents.ToString();*/
            
            ms.Position = 0;
            var reader = new StreamReader(ms);
            Contents = await reader.ReadToEndAsync();
        }
    }
}
