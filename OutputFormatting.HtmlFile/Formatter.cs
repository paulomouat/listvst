using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Formatter : IOutputFormatter
{
    public string Format => "html";
    
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

        var document = CreateOutputDocument();

        var pathSection = CreatePathSection(details);
        document.Add(pathSection);
        
        /*var allByPath = details
            .ToLookup(e => e.Path, e => e.Vst)
            .OrderBy(v => v.Key);

        var byPathEntries = ToEntries(allByPath);
        document.Add(byPathEntries);*/
        
        var allByVst = details
            .ToLookup(e => e.Vst, e => e.Path)
            .OrderBy(v => v.Key);

        var byVstEntries = ToEntries(allByVst);
        document.Add(byVstEntries);

        await using var stream = File.OpenWrite(options.Path);
        document.Save(stream);
    }

    private static XElement CreatePathSection(IEnumerable<(string Path, string Vst)> details)
    {
        var container = new XElement("div");
        
        var title = new XElement("div", "Listing by path");
        var listing = new XElement("div");
        
        var allByPath = details
            .ToLookup(e => e.Path, e => e.Vst)
            .OrderBy(v => v.Key);

        var entries = ToEntries(allByPath);
        listing.Add(entries);
        
        container.Add(title);
        container.Add(listing);

        return container;
    }
    
    private static IEnumerable<XElement> ToEntries(IEnumerable<IGrouping<string, string>> lookup)
    {
        var elements = new List<XElement>();
        
        foreach(var group in lookup)
        {
            var entry = new XElement("p");
            entry.SetAttributeValue("class", "entry");
            
            var title = new XElement("div", group.Key);
            title.SetAttributeValue("class", "title");
            
            entry.Add(title);
            
            foreach(var item in group)
            {
                var element = new XElement("div", item);
                element.SetAttributeValue("class", "item");
                title.Add(element);
            }
            
            elements.Add(entry);
        }

        return elements;
    }

    private static XElement CreateOutputDocument()
    {
        var root = new XElement("html");
        var body = new XElement("body");

        root.Add(body);

        return root;
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