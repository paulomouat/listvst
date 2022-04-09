using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Formatter : IOutputFormatter
{
    public string Format => "html";

    protected virtual async Task Write(IEnumerable<PluginDescriptor> details, IFileOutputFormatterOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.Path))
        {
            throw new ArgumentException(nameof(options.Path));
        }

        var document = Document.Create("List of VSTs");

        var detailsList = details.ToList();
        
        var pathSection = CreatePathSection(detailsList);
        var pluginSection = CreatePluginSection(detailsList);

        var mainIndex = MainIndex.Create("main-index","Main index",new[] { pathSection, pluginSection });
       
        document.Add(mainIndex);
        document.Add(pathSection);
        document.Add(pluginSection);

        document.Save(options.Path);
        await Task.CompletedTask;
    }

    private static Section CreatePathSection(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.ProjectDescriptor.Path)
            .ThenBy(pd => pd.Name)
            .ToLookup(e => e.ProjectDescriptor.Path, e => e.Name);

        var section = Section.Create("listing-by-path","Listing by path", lookup);

        return section;
    }
    
    private static Section CreatePluginSection(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.Name)
            .ThenBy(pd => pd.ProjectDescriptor.Path)
            .ToLookup(e => e.Name, e => e.ProjectDescriptor.Path);

        var section = Section.Create("listing-by-plugin", "Listing by plugin", lookup);

        return section;
    }

    Task IOutputFormatter.Write(IEnumerable<PluginDescriptor> details, IOutputFormatterOptions options)
    {
        if (options is not IFileOutputFormatterOptions formatterOptions)
        {
            throw new ArgumentException("This formatter requires an options type of IFileOutputFormatterOptions");
        }

        return Write(details, formatterOptions);
    }
}