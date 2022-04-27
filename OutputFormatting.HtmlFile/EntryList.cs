using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public abstract class EntryList : XElement, IEntryList
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
}