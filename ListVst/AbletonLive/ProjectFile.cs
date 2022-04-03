using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ListVst.AbletonLive
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
                await using (var gz = new GZipStream(fs, CompressionMode.Decompress))
                {
                    var buffer = new byte[1024];
                    int n;
                    while ((n = gz.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, n);
                    }
                }
            }

            ms.Position = 0;
            var reader = new StreamReader(ms);
            Contents = await reader.ReadToEndAsync();
        }
    }
}
