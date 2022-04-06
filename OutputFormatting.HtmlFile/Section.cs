using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class Section : XElement
{
    public string Id { get; private set; }
    public string Title { get; private set; }

    public Section(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;
    }
    
    public static Section Create(string id, string title, IEnumerable<IGrouping<string, string>> lookup)
    {
        var container = new Section(id, title);
        
        var titleElement = new XElement("div", new XAttribute("id", id), title);
        titleElement.SetAttributeValue("class", "section-title");

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
            
            var entry = new XElement("p");
            entry.SetAttributeValue("id", id);
            entry.SetAttributeValue("class", "entry");
            
            var title = new XElement("div", key);
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
}