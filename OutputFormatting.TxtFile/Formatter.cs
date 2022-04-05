namespace ListVst.OutputFormatting.TxtFile;

public class Formatter : IOutputFormatter
{
    public string Format => "txt";
    
    public async Task Write(IEnumerable<(string Path, string Vst)> details, IFileOutputFormatterOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.Path))
        {
            throw new ArgumentException(nameof(options.Path));
        }
        
        var lines = new List<string>();
        
        var allByPath = details
            .ToLookup(e => e.Path, e => e.Vst)
            .OrderBy(v => v.Key);
        
        lines.AddRange(ToLines(allByPath));

        var allByVst = details
            .ToLookup(e => e.Vst, e => e.Path)
            .OrderBy(v => v.Key);
        
        lines.AddRange(ToLines(allByVst));

        await File.WriteAllLinesAsync(options.Path, lines);
    }

    private static IEnumerable<string> ToLines(IEnumerable<IGrouping<string, string>> lookup)
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

    Task IOutputFormatter.Write(IEnumerable<(string Path, string Vst)> details, IOutputFormatterOptions options)
    {
        if (options is not IFileOutputFormatterOptions formatterOptions)
        {
            throw new ArgumentException("This formatter requires an options type of IFileOutputFormatterOptions");
        }

        return Write(details, formatterOptions);
    }
}