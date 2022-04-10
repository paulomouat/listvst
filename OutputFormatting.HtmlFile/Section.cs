using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public abstract class Section : XElement
{
    public string Id { get; }
    public string Title { get; }
    
    public Section(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;
    }

    public abstract void Populate(IEnumerable<PluginDescriptor> details);

    protected virtual void Populate(IEnumerable<IGrouping<string, string>> lookup)
    {
        SetAttributeValue("id", Id);
        
        var titleElement = new XElement("div", new XAttribute("class", "section title"), Title);

        var lookupList = lookup.ToList();
        var entryNames = lookupList.Select(g => g.Key);
        var index = EntryIndex.Create(Id + "-index", "All entries", entryNames);
        
        var listing = new XElement("div", new XAttribute("id", Id + "-entries"));
        var entries = ToEntries(lookupList);
        listing.Add(entries);
        
        Add(titleElement);
        Add(index);
        Add(listing);
    }
    
    protected virtual IEnumerable<XElement> ToEntries(IEnumerable<IGrouping<string, string>> lookup)
    {
        var elements = new List<XElement>();
        
        foreach(var group in lookup)
        {
            var key = group.Key;
            var id = new Id(key).Value;
            
            var entry = new XElement("div");
            entry.SetAttributeValue("id", id);
            entry.SetAttributeValue("class", "entry");
            
            var keyTitle = new XElement("div", key);
            keyTitle.SetAttributeValue("class", "key title");
            entry.Add(keyTitle);
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
            
            foreach(var item in group)
            {
                var anchor = new XElement("a", new XAttribute("href", "#" + new Id(item)), item);
                var element = new XElement("div", anchor);
                element.SetAttributeValue("class", "item");
                entry.Add(element);
            }
            
            elements.Add(entry);
        }

        return elements;
    }
}