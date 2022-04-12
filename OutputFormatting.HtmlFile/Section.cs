using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public abstract class Section<TEntry, TItem> : XElement, ISection
{
    public string Id { get; }
    public string Title { get; }

    protected Section(string id, string title)
        : base("div")
    {
        Id = id;
        Title = title;

        SetAttributeValue("id", Id);
        var titleElement = new XElement("div", new XAttribute("class", "section title"), Title);
        Add(titleElement);
    }

    public abstract void Add(IEnumerable<PluginData> data);

    protected virtual void Add(ILookup<TEntry, TItem> lookup)
    {
        var index = BuildEntryIndex(lookup);
        var list = BuildEntryList(lookup);
        Add(index);
        Add(list);
    }

    protected abstract IEntryIndex BuildEntryIndex(ILookup<TEntry, TItem> lookup);
    protected abstract IEntryList BuildEntryList(ILookup<TEntry, TItem> lookup);
}