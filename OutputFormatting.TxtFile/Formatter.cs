namespace ListVst.OutputFormatting.TxtFile;

public class Formatter : IOutputFormatter
{
    public string Format => "txt";

    protected virtual async Task Write(IEnumerable<PluginRecord> data, IFileOutputFormatterOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Path))
        {
            throw new ArgumentException(nameof(options.Path));
        }
        
        var lines = new List<string>();
        
        var allByPath = data
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.Name)
            .ToLookup(pair => pair.ProjectDescriptor.Path, pair => pair.PluginDescriptor.Name);
        
        lines.AddRange(ToLines(allByPath));

        var allByVst = data
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

    Task IOutputFormatter.Write(IEnumerable<PluginRecord> data, IOutputFormatterOptions options)
    {
        if (options is not IFileOutputFormatterOptions formatterOptions)
        {
            throw new ArgumentException("This formatter requires an options type of IFileOutputFormatterOptions");
        }

        return Write(data, formatterOptions);
    }
}