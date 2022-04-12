using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryIndex : EntryIndex<ProjectDescriptor, PluginDescriptor>
{
    public ProjectEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }
    
    public override void Add(PluginDescriptor item, XElement entry)
    {
        var itemName = item.Manufacturer + " " + item.Name;
        base.Add(itemName, entry);
    }

    protected override string GetKey(ProjectDescriptor entry)
    {
        return entry.Path;
    }
}