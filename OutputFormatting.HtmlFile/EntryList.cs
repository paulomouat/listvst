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
    
    public virtual void Add(ILookup<TEntry, TItem> lookup)
    {
        foreach(var group in lookup)
        {
            var key = GetKey(group.Key);
            var entryId = new Id(key).Value;
            
            var entry = new XElement("div",
                new XAttribute("id", entryId),
                new XAttribute("class", "entry"));
            
            var entryTitle = new XElement("div",
                new XAttribute("class", "key title"),
                key);
            entry.Add(entryTitle);
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
            
            Add(group, entry);
            Add(entry);
        }
    }

    public virtual void Add(IEnumerable<TItem> item, XElement entry)
    { }

    protected virtual string GetKey(TEntry entry)
    {
        return string.Empty;
    }
}