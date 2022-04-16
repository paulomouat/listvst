using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryList : EntryList<ProjectDescriptor, PluginDescriptor>
{
    public ProjectEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }
    
    public override void AddItemsToEntry(IEnumerable<PluginDescriptor> pluginDescriptors, XElement entry)
    {
        entry.Add(pluginDescriptors.ToXElements());
    }

    protected override string GetKey(ProjectDescriptor entry)
    {
        return entry.Path;
    }
}