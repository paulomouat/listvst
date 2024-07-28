using System.IO.Compression;
using System.Text;

namespace ListVst.Processing.AbletonLive;

public class ProjectFile(string path) : IProjectFile
{
    public string Name { get; } = System.IO.Path.GetFileNameWithoutExtension(path);
    public string Path { get; } = path;
    public string? Contents { get; private set; }

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

        var contents = new StringBuilder();
            
        ms.Position = 0;
        using var reader = new StreamReader(ms);
        while(reader.Peek() >= 0)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
            {
                line = line.Trim();

                if (!line.StartsWith("<"))
                {
                    contents.Append(' ');
                }

                line = line.Replace("::", "_");
                line = line.Replace("x:", "_");
                contents.Append(line);
            }
        }
            
        Contents = contents.ToString();
    }
}