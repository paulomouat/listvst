using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public abstract class EntryList<TEntry, TItem> : XElement, IEntryList
{
    public string Id { get; }

    public ISection ParentSection { get; }
    
    public EntryList(string id, ISection parentSection)
        : base("div")
    {
        Id = id;
        ParentSection = parentSection;
        
        SetAttributeValue("id", id);
    }

    public virtual XElement BuildEntry(IGrouping<TEntry, TItem> group)
    {
        var key = GetKey(group.Key);
        var entryId = new Id(key).Value;
            
        var entry = new XElement("div",
            new XAttribute("id", entryId),
            new XAttribute("class", "entry"));
            
        return entry;
    }

    public abstract void AddTitle(IGrouping<TEntry, TItem> group, XElement entry);
    
    public virtual void AddHeadings(IGrouping<TEntry, TItem> group, XElement entry)
    {
        var linkToTop = new XElement("a",
            new XAttribute("class", "link-to-top"),
            new XAttribute("href", "#document-title"),
            "top");
        entry.Add(linkToTop);
        var linkToSection = new XElement("a",
            new XAttribute("class", "link-to-section"),
            new XAttribute("href", "#" + ParentSection.Id),
            "section index");
        entry.Add(linkToSection);
    }
    
    public virtual void AddItemsToEntry(IEnumerable<TItem> items, XElement entry)
    { }

    protected virtual string GetKey(TEntry entry)
    {
        return string.Empty;
    }
}