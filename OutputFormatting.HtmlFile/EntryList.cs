using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public abstract class EntryList : XElement, IEntryList
{
    public string Id { get; }

    protected ISection ParentSection { get; }

    protected EntryList(string id, ISection parentSection)
        : base("div")
    {
        Id = id;
        ParentSection = parentSection;
        
        SetAttributeValue("id", id);
    }
}