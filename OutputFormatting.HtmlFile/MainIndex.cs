using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class MainIndex : XElement
{
    public string Id { get; }
    public string Title { get; }

    public static MainIndex Create(string id, string title, IEnumerable<Section> sections)
    {
        var container = new MainIndex(id, title);

        var titleElement = new XElement("div", container.Title);
        titleElement.SetAttributeValue("class", "main-index title");
        container.Add(titleElement);
        
        foreach (var section in sections)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            var anchor = new XElement("a", new XAttribute("href", "#" + section.Id), section.Title);
            entry.Add(anchor);

            container.Add(entry);
        }
        
        return container;
    }
    
    private MainIndex(string id, string title, string tag = "div")
        : base(tag)
    {
        Id = id;
        Title = title;
    }
}