using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryIndex : XElement
{
    public string Id { get; }
    public string Title { get; }

    public Section ParentSection { get; }
    
    public EntryIndex(string id, string title, Section parentSection)
        : base("div")
    {
        Id = id;
        Title = title;
        ParentSection = parentSection;
        
        SetAttributeValue("id", id);
        
        var titleElement = new XElement("div", title,
            new XAttribute("class", "index title"));
        Add(titleElement);
    }
    
    public virtual void Add(ILookup<string, PluginDescriptor> lookup)
    {
        var linkToTop = new XElement("a",
            new XAttribute("class", "link-to-top"),
            new XAttribute("href", "#document-title"),
            "top");
        Add(linkToTop);

        var statsElement = new XElement("div", "Number of entries: " + lookup.Count,
            new XAttribute("class", "stats"));
        Add(statsElement);
        
        foreach (var group in lookup)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            Add(group.Key, entry);
            Add(entry);
        }
    }

    public virtual void Add(string itemName, XElement entry)
    {
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(itemName)), itemName);
        entry.Add(anchor);
    }
}