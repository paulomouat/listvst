using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class EntryList<TEntry, TItem> : XElement, IEntryList
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
    
    public virtual void AddFromLookup(ILookup<TEntry, TItem> lookup)
    {
        foreach(var group in lookup)
        {
            var entry = BuildEntry(group);
            AddTitle(group, entry);
            AddHeadings(group, entry);
            AddItemsToEntry(group, entry);
            Add(entry);
        }
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

    public virtual void AddTitle(IGrouping<TEntry, TItem> group, XElement entry)
    {
        var key = GetKey(group.Key);
        var entryTitle = new XElement("div",
            new XAttribute("class", "key title"),
            key);
        entry.Add(entryTitle);
    }
    
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