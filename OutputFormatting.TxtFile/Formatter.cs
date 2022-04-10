namespace ListVst.OutputFormatting.TxtFile;

public class Formatter : IOutputFormatter
{
    public string Format => "txt";

    protected virtual async Task Write(IEnumerable<PluginProjectPair> pairs, IFileOutputFormatterOptions options)
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
        
        var allByPath = pairs
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.Name)
            .ToLookup(pair => pair.ProjectDescriptor.Path, pair => pair.PluginDescriptor.Name);
        
        lines.AddRange(ToLines(allByPath));

        var allByVst = pairs
            .OrderBy(pair => pair.PluginDescriptor.Name)
            .ThenBy(pair => pair.ProjectDescriptor.Path)
            .ToLookup(pair => pair.PluginDescriptor.Name, e => e.ProjectDescriptor.Path);
        
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

    Task IOutputFormatter.Write(IEnumerable<PluginProjectPair> pairs, IOutputFormatterOptions options)
    {
        if (options is not IFileOutputFormatterOptions formatterOptions)
        {
            throw new ArgumentException("This formatter requires an options type of IFileOutputFormatterOptions");
        }

        return Write(pairs, formatterOptions);
    }
}