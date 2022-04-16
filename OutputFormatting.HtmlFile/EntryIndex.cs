using System.Text.RegularExpressions;
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

    public virtual void AddItemToEntry(TItem item, XElement entry)
    { }

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
    {
        foreach (var group in lookup)
        {
            var entry = new XElement("div", new XAttribute("class", "item"));
            var itemName = GetKey(group.Key);
            AddItemToEntry(itemName, itemName, entry);
            Add(entry);
        }
    }

    protected virtual void AddItemToEntry(string itemKey, string itemName, XElement entry)
    {
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(itemKey)), itemName);
        entry.Add(anchor);
    }

    protected virtual string GetKey(TEntry entry)
    {
        return string.Empty;
    }
}