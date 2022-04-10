using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryIndex : XElement
{
    public string Id { get; }
    public string Title { get; }

    public EntryIndex(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;
        
        SetAttributeValue("id", id);
        
        var titleElement = new XElement("div", title,
            new XAttribute("class", "index title"));
        Add(titleElement);
    }
    
    public virtual void Add(IEnumerable<string> values)
    {
        var valueList = values.ToList();
        
        var linkToTop = new XElement("a",
            new XAttribute("class", "link-to-top"),
            new XAttribute("href", "#document-title"),
            "top");
        Add(linkToTop);

        var statsElement = new XElement("div", "Number of entries: " + valueList.Count,
            new XAttribute("class", "stats"));
        Add(statsElement);
        
        foreach (var value in valueList)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            var anchor = new XElement("a", new XAttribute("href", "#" + new Id(value)), value);
            entry.Add(anchor);

            Add(entry);
        }
    }
}