using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryList : EntryList<PluginDescriptor, ProjectDescriptor>
{
    public PluginEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }

    public override void AddItemsToEntry(IEnumerable<ProjectDescriptor> projectDescriptors, XElement entry)
    {
        entry.Add(projectDescriptors.ToXElements());
    }

    protected override string GetKey(PluginDescriptor entry)
    {
        return entry.FullName;
    }
}