using System.IO.Compression;

namespace ListVst.StudioOne
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

            ms.Position = 0;
            var reader = new StreamReader(ms);
            Contents = await reader.ReadToEndAsync();
        }
    }
}
