using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Section : XElement
{
    public string Id { get; private set; }
    public string Title { get; private set; }
    
    public static Section Create(string id, string title, IEnumerable<IGrouping<string, string>> lookup)
    {
        var container = new Section(id, title);
        
        var titleElement = new XElement("div", new XAttribute("id", id), title);
        titleElement.SetAttributeValue("class", "section title");

        var index = EntryIndex.Create(id + "-index", "All entries", lookup.Select(g => g.Key));
        
        var listing = new XElement("div");
        var entries = ToEntries(lookup);
        listing.Add(entries);
        
        container.Add(titleElement);
        container.Add(index);
        container.Add(listing);

        return container;
    }
    
    private static IEnumerable<XElement> ToEntries(IEnumerable<IGrouping<string, string>> lookup)
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