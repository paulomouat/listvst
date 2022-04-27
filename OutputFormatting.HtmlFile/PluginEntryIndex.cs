using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryIndex : EntryIndex
{
    public PluginEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }

    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.PluginDescriptor.FullName)
            .ThenBy(pair => pair.ProjectDescriptor.Path)
            .ToLookup(pair => pair.PluginDescriptor, pair => pair.ProjectDescriptor);
        AddHeadings(lookup);
        AddItems(lookup);        
    }
    
    private void AddHeadings(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var linkToTop = new LinkToTop();
        Add(linkToTop);

        var statsElement = new XElement("div", "Number of entries: " + lookup.Count,
            new XAttribute("class", "stats"));
        Add(statsElement);
    }

    private void AddItems(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var descriptors = lookup
            .Select(g => g.Key)
            .ToList();

        var allItemsElement = new XElement("div", new XAttribute("class", "index-items"));
        Add(allItemsElement);
        allItemsElement.Add(descriptors.ToXElements());
    }
}