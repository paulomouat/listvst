using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryIndex : XElement, IEntryIndex
{
    public string Id { get; }
    public string Title { get; }

    public ISection ParentSection { get; }
    
    public EntryIndex(string id, string title, ISection parentSection)
        : base("div")
    {
        Id = id;
        Title = title;
        ParentSection = parentSection;
        
        SetAttributeValue("id", id);
        
        var titleElement = new XElement("div", title,
            new XAttribute("class", "index title"));
        Add(titleElement);
    }
}