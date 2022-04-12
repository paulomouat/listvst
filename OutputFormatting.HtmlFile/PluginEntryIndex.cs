using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryIndex : EntryIndex<PluginDescriptor, ProjectDescriptor>
{
    public PluginEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }
    
    public override void Add(ProjectDescriptor item, XElement entry)
    {
        var itemName = item.Path;
        base.Add(itemName, entry);
    }
    
    protected override string GetKey(PluginDescriptor entry)
    {
        return entry.FullName;
    }
}