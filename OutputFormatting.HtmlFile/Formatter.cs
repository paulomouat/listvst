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
        
        var allByPath = details
            .ToLookup(e => e.Path, e => e.Vst)
            .OrderBy(v => v.Key);

        var byPathEntries = ToEntries(allByPath);
        foreach (var entry in byPathEntries)
        {
            document.Add(new XElement("p", entry));
        }
        
        var allByVst = details
            .ToLookup(e => e.Vst, e => e.Path)
            .OrderBy(v => v.Key);

        var byVstEntries = ToEntries(allByVst);
        foreach (var entry in byVstEntries)
        {
            document.Add(new XElement("p", entry));
        }

        await using var stream = File.OpenWrite(options.Path);
        document.Save(stream);
    }

    private static IEnumerable<XElement> ToEntries(IEnumerable<IGrouping<string, string>> lookup)
    {
        var elements = new List<XElement>();
        
        foreach(var group in lookup)
        {
            var key = new XElement("div", group.Key);
            foreach(var item in group)
            {
                var element = new XElement("div", item);
                key.Add(element);
            }
            
            elements.Add(key);
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