using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryIndex : EntryIndex
{
    public ProjectEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }

    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.FullName)
            .ToLookup(pair => pair.ProjectDescriptor, pair => pair.PluginDescriptor);
        AddHeadings(lookup);
        AddItems(lookup);       
    }
    
    private void AddHeadings(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var linkToTop = new LinkToTop();
        Add(linkToTop);

        var statsElement = new XElement("div", "Number of entries: " + lookup.Count,
            new XAttribute("class", "stats"));
        Add(statsElement);
    }
    
    private void AddItems(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var descriptors = lookup
            .Select(g => g.Key)
            .ToList();

        var allItemsElement = new XElement("div", new XAttribute("class", "index-items"));
        Add(allItemsElement);
        allItemsElement.Add(descriptors.ToXElements(lookup));
    }
}