using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryIndex<TEntry, TItem> : XElement, IEntryIndex
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
    
    public virtual void AddFromLookup(ILookup<TEntry, TItem> lookup)
    {
        AddHeadings(lookup);
        AddItems(lookup);
    }

    protected virtual void AddHeadings(ILookup<TEntry, TItem> lookup)
    {
        var linkToTop = new XElement("a",
            new XAttribute("class", "link-to-top"),
            new XAttribute("href", "#document-title"),
            "top");
        Add(linkToTop);

        var statsElement = new XElement("div", "Number of entries: " + lookup.Count,
            new XAttribute("class", "stats"));
        Add(statsElement);
    }
    
    protected virtual void AddItems(ILookup<TEntry, TItem> lookup)
    { }
}