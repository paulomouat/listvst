using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Section : XElement
{
    public string Id { get; }
    public string Title { get; }
    
    public static Section Create(string id, string title, IEnumerable<IGrouping<string, string>> lookup)
    {
        var container = new Section(id, title);
        container.SetAttributeValue("id", id);
        
        var titleElement = new XElement("div", new XAttribute("class", "section title"), title);

        var index = EntryIndex.Create(id + "-index", "All entries", lookup.Select(g => g.Key));
        
        var listing = new XElement("div", new XAttribute("id", id + "-entries"));
        var entries = container.ToEntries(lookup);
        listing.Add(entries);
        
        container.Add(titleElement);
        container.Add(index);
        container.Add(listing);

        return container;
    }
    
    private IEnumerable<XElement> ToEntries(IEnumerable<IGrouping<string, string>> lookup)
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
                "index");
            entry.Add(linkToSection);
            
            foreach(var item in group)
            {
                var anchor = new XElement("a", new XAttribute("href", "#" + new Id(item).Value), item);
                var element = new XElement("div", anchor);
                element.SetAttributeValue("class", "item");
                entry.Add(element);
            }
            
            elements.Add(entry);
        }

        return elements;
    }

    private Section(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;
    }
}