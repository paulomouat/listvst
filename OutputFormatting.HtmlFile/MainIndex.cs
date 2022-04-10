using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class MainIndex : XElement
{
    public string Id { get; }
    public string Title { get; }
    
    public MainIndex(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;

        SetAttributeValue("id", id);
        
        var titleElement = new XElement("div", title,
            new XAttribute("class", "main-index title"));
        Add(titleElement);
    }

    public virtual void Add(IEnumerable<ISection> sections)
    {
        foreach (var section in sections)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            var anchor = new XElement("a", new XAttribute("href", "#" + section.Id), section.Title);
            entry.Add(anchor);

            Add(entry);
        }
    }
}