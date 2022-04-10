using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public abstract class Section : XElement
{
    public string Id { get; }
    public string Title { get; }

    protected Section(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;

        SetAttributeValue("id", Id);
        var titleElement = new XElement("div", new XAttribute("class", "section title"), Title);
        Add(titleElement);
    }

    public abstract void Add(IEnumerable<PluginDescriptor> details);

    protected virtual void Add(ILookup<string, PluginDescriptor> lookup)
    {
        var index = new EntryIndex(Id + "-index", "All entries");
        var entryNames = lookup.Select(g => g.Key);
        index.Add(entryNames);
        
        var listing = new XElement("div", new XAttribute("id", Id + "-entries"));
        var entries = ToEntryElements(lookup);
        listing.Add(entries);
        
        Add(index);
        Add(listing);
    }
    
    protected virtual IEnumerable<XElement> ToEntryElements(ILookup<string, PluginDescriptor> lookup)
    {
        var elements = new List<XElement>();
        
        foreach(var group in lookup)
        {
            var key = group.Key;
            var entryId = new Id(key).Value;
            
            var entry = new XElement("div");
            entry.SetAttributeValue("id", entryId);
            entry.SetAttributeValue("class", "entry");
            
            var entryTitle = new XElement("div", key);
            entryTitle.SetAttributeValue("class", "key title");
            entry.Add(entryTitle);
            var linkToTop = new XElement("a",
                new XAttribute("class", "link-to-top"),
                new XAttribute("href", "#document-title"),
                "top");
            entry.Add(linkToTop);
            var linkToSection = new XElement("a",
                new XAttribute("class", "link-to-section"),
                new XAttribute("href", "#" + Id),
                "section index");
            entry.Add(linkToSection);
            
            foreach(var pluginDescriptor in group)
            {
                var itemElement = ToItemElement(pluginDescriptor);
                entry.Add(itemElement);
            }
            
            elements.Add(entry);
        }

        return elements;
    }

    protected virtual XElement ToItemElement(PluginDescriptor pluginDescriptor)
    {
        var item = pluginDescriptor.Name;
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(item)), item);
        var element = new XElement("div", anchor);
        element.SetAttributeValue("class", "item");
        return element;
    }
}