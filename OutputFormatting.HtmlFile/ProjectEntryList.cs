using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryList : EntryList
{
    public ProjectEntryList(string id, Section parentSection)
        : base(id, parentSection)
    { }
    
    public override void Add(PluginDescriptor pluginDescriptor, XElement entry)
    {
        var item = pluginDescriptor.Name;
        var anchor = new XElement("a",
            new XAttribute("href", "#" + new Id(item)),
            item);
        var element = new XElement("div",
            new XAttribute("class", "item"),
            anchor);
        entry.Add(element);
    }
}