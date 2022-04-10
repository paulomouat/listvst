using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public abstract class Section : XElement
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

    public abstract void Add(IEnumerable<PluginDescriptor> details);

    protected virtual void Add(ILookup<string, PluginDescriptor> lookup)
    {
        var index = BuildEntryIndex(lookup);
        var list = BuildEntryList(lookup);
        Add(index);
        Add(list);
    }

    protected virtual EntryIndex BuildEntryIndex(ILookup<string, PluginDescriptor> lookup)
    {
        var index = new EntryIndex(Id + "-index", "All entries");
        var entryNames = lookup.Select(g => g.Key);
        index.Add(entryNames);
        return index;
    }

    protected virtual EntryList BuildEntryList(ILookup<string, PluginDescriptor> lookup)
    {
        var list = new EntryList(Id + "-entries");
        list.Add(lookup);
        return list;
    }
}