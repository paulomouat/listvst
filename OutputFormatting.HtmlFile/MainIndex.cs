using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class MainIndex : XElement
{
    public string Id { get; }
    public string Title { get; }
    
    public MainIndex(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;

        SetAttributeValue("id", id);
        
        var titleElement = new XElement("div", title,
            new XAttribute("class", "main-index title"));
        Add(titleElement);
    }

    public virtual void Add(IEnumerable<ISection> sections)
    {
        foreach (var section in sections)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            var anchor = new XElement("a", new XAttribute("href", "#" + section.Id), section.Title);
            entry.Add(anchor);

            Add(entry);
        }
        
        var separator = new XElement("p", new XAttribute("class", "item"));
        Add(separator);
        
        var selectedFormats = new XElement("div",
            GetFormatCheckboxes(["au", "vst", "vst3"]),
            new XAttribute("id", "selected-formats"),
            new XAttribute("class", "item"));
        Add(selectedFormats);
        
        Add(separator);
        
        var selectedTypes = new XElement("div",
            GetTypeCheckboxes(["als", "song", "project"]),
            new XAttribute("id", "selected-types"),
            new XAttribute("class", "item"));
        Add(selectedTypes);
    }

    private static IEnumerable<XElement> GetFormatCheckboxes(IEnumerable<string> formats)
    {
        var checkboxes = formats.Select(format =>
            new XElement("input", format.ToUpperInvariant(),
                new XAttribute("id", $"format-{format.ToLowerInvariant()}"),
                new XAttribute("type", "checkbox"),
                new XAttribute("onclick", "updateSelection();")//,
                //new XAttribute("checked", "checked")
                ));
        return checkboxes;
    }

    private static IEnumerable<XElement> GetTypeCheckboxes(IEnumerable<string> types)
    {
        var typeMap = new Dictionary<string, string>
        {
            ["als"] = "Ableton Live",
            ["song"] = "Studio One Song",
            ["project"] = "Studio One Project",
        };

        var checkboxes = types.Select(type =>
            new XElement("input", typeMap[type],
                new XAttribute("id", $"type-{type.ToLowerInvariant()}"),
                new XAttribute("type", "checkbox"),
                new XAttribute("onclick", "updateSelection();")//,
                //new XAttribute("checked", "checked")
            ));
        return checkboxes;
    }
}