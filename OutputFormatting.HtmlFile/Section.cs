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

        var listing = new XElement("div");
        
        var entries = ToEntries(lookup);
        listing.Add(entries);
        
        container.Add(titleElement);
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
}