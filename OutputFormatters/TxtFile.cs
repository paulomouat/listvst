using System.Text;

namespace ListVst.OutputFormatters;

public class TxtFile : IOutputFormatter
{
    public string Format => "txt";

    private string Path { get; }
    
    public TxtFile(string path)
    {
        Path = path;
    }
    
    public async Task Write(IEnumerable<(string Path, string Vst)> details)
    {
        var lines = new List<string>();
        
        var allByPath = details
            .ToLookup(e => e.Path, e => e.Vst)
            .OrderBy(v => v.Key);
        
        lines.AddRange(ToLines(allByPath));

        var allByVst = details
            .ToLookup(e => e.Vst, e => e.Path)
            .OrderBy(v => v.Key);
        
        lines.AddRange(ToLines(allByVst));

        await File.WriteAllLinesAsync(Path, lines);
    }

    private IEnumerable<string> ToLines(IOrderedEnumerable<IGrouping<string, string>> lookup)
    {
        var lines = new List<string>();

        foreach(var group in lookup)
        {
            lines.Add(group.Key);
            foreach(var element in group)
            {
                lines.Add(element);
            }

            lines.Add(string.Empty);
        }

        return lines;
    }
}