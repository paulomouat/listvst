using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryIndex : XElement
{
    public string Id { get; private set; }
    public string Title { get; private set; }

    public EntryIndex(string id, string title, string tag = "p")
        : base(tag)
    {
        Id = id;
        Title = title;
    }
    
    public static EntryIndex Create(string id, string title, IEnumerable<string> values)
    {
        var container = new EntryIndex(id, title);

        var titleElement = new XElement("div", title);
        titleElement.SetAttributeValue("class", "index-title");
        container.Add(titleElement);        
        
        foreach (var value in values)
        {
            var entry = new XElement("div");
            var anchor = new XElement("a", new XAttribute("href", "#" + new Id(value).Value), value);
            entry.Add(anchor);

            container.Add(entry);
        }
        
        return container;
    }
}