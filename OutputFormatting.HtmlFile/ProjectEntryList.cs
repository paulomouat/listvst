using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryList : EntryList<ProjectDescriptor, PluginDescriptor>
{
    public ProjectEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }
    
    public override void Add(PluginDescriptor pluginDescriptor, XElement entry)
    {
        var fullName = pluginDescriptor.FullName;
        var outputName = pluginDescriptor.Manufacturer + " " + pluginDescriptor.Name;
        var anchor = new XElement("a",
            new XAttribute("href", "#" + new Id(fullName)),
            outputName);
        var element = new XElement("div",
            new XAttribute("class", "item"),
            anchor);
        entry.Add(element);
    }

    protected override string GetKey(ProjectDescriptor entry)
    {
        return entry.Path;
    }
}