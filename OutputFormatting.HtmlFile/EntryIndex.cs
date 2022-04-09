using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryIndex : XElement
{
    public string Id { get; }
    public string Title { get; }
    
    public static EntryIndex Create(string id, string title, IEnumerable<string> values)
    {
        var container = new EntryIndex(id, title);
        container.SetAttributeValue("id", id);
        
        var titleElement = new XElement("div", title,
            new XAttribute("class", "index title"));
        container.Add(titleElement);

        var valueList = values.ToList();
        
        var statsElement = new XElement("div", "Number of entries: " + valueList.Count,
            new XAttribute("class", "stats"));
        container.Add(statsElement);
        
        foreach (var value in valueList)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            var anchor = new XElement("a", new XAttribute("href", "#" + new Id(value)), value);
            entry.Add(anchor);

            container.Add(entry);
        }
        
        return container;
    }

    private EntryIndex(string id, string title, string tag = "div")
        : base(tag)
    {
        Id = id;
        Title = title;
    }
}