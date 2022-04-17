using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryIndex : EntryIndex<ProjectDescriptor, PluginDescriptor>
{
    public ProjectEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }

    protected override void AddItems(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var descriptors = lookup
            .Select(g => g.Key)
            .ToList();

        var allItemsElement = new XElement("div", new XAttribute("class", "all-items"));
        Add(allItemsElement);
        allItemsElement.Add(descriptors.ToXElements());
    }
}