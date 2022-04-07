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

        var pathSection = CreatePathSection(details);
        var pluginSection = CreatePluginSection(details);

        var mainIndex = CreateMainIndex(new[] { pathSection, pluginSection });
        
        document.Body.Add(mainIndex);
        
        document.Add(pathSection);
        document.Add(pluginSection);
        
        await using var stream = File.OpenWrite(options.Path);
        document.Save(stream);
    }

    private static Section CreatePathSection(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.Path)
            .ThenBy(pd => pd.Name)
            .ToLookup(e => e.Path, e => e.Name);

        var section = Section.Create("listing-by-path","Listing by path", lookup);

        return section;
    }
    
    private static Section CreatePluginSection(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.Name)
            .ThenBy(pd => pd.Path)
            .ToLookup(e => e.Name, e => e.Path);

        var section = Section.Create("listing-by-plugin", "Listing by plugin", lookup);

        return section;
    }

    private static XElement CreateMainIndex(IEnumerable<Section> sections)
    {
        var container = new XElement("p");

        var title = new XElement("div", "Main index");
        title.SetAttributeValue("class", "main-index-title");
        container.Add(title);
        
        foreach (var section in sections)
        {
            var entry = new XElement("div");
            var anchor = new XElement("a", new XAttribute("href", "#" + section.Id), section.Title);
            entry.Add(anchor);

            container.Add(entry);
        }
        
        return container;
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