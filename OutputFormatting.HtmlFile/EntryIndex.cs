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
    
    public virtual void Add(ILookup<TEntry, TItem> lookup)
    {
        var linkToTop = new XElement("a",
            new XAttribute("class", "link-to-top"),
            new XAttribute("href", "#document-title"),
            "top");
        Add(linkToTop);

        var statsElement = new XElement("div", "Number of entries: " + lookup.Count,
            new XAttribute("class", "stats"));
        Add(statsElement);
        
        foreach (var group in lookup)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            var itemName = GetKey(group.Key);
            Add(itemName, entry);
            Add(entry);
        }
    }

    public virtual void Add(TItem itemName, XElement entry)
    { }

    protected virtual void Add(string itemName, XElement entry)
    {
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(itemName)), itemName);
        entry.Add(anchor);
    }

    protected virtual string GetKey(TEntry entry)
    {
        return string.Empty;
    }
}