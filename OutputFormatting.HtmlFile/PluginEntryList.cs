using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryList : EntryList<PluginDescriptor, ProjectDescriptor>
{
    public PluginEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }

    public override void Add(ProjectDescriptor projectDescriptor, XElement entry)
    {
        var item = projectDescriptor.Path;
        var anchor = new XElement("a",
            new XAttribute("href", "#" + new Id(item)),
            item);
        var element = new XElement("div",
            new XAttribute("class", "item"),
            anchor);
        entry.Add(element);
    }

    protected override string GetKey(PluginDescriptor entry)
    {
        return entry.Name;
    }
}