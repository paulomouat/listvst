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

        var allItemsElement = new XElement("div", new XAttribute("class", "index-items"));
        Add(allItemsElement);
        allItemsElement.Add(descriptors.ToXElements());
    }
}