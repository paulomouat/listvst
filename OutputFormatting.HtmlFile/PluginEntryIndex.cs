using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryIndex : EntryIndex<PluginDescriptor, ProjectDescriptor>
{
    public PluginEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }
    
    protected override void AddItems(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var descriptors = lookup
            .Select(g => g.Key)
            .ToList();

        Add(descriptors.ToXElements());
    }
    
    public override void AddItemToEntry(ProjectDescriptor item, XElement entry)
    {
        var itemName = item.Path;
        base.AddItemToEntry(itemName, itemName, entry);
    }
    
    protected override string GetKey(PluginDescriptor entry)
    {
        return entry.FullName;
    }
}