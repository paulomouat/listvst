using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryList : EntryList
{
    public PluginEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }

    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.PluginDescriptor.FullName)
            .ThenBy(pair => pair.ProjectDescriptor.Path)
            .ToLookup(pair => pair.PluginDescriptor, pair => pair.ProjectDescriptor);
        AddFromLookup(lookup);
    }
    
    public virtual void AddFromLookup(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var regrouped = lookup.ToLookup(k => k.Key.FullName,
            g => g);
        
        foreach(var groups in regrouped)
        {
            var first = groups.First();
            var entry = new Entry(new Id(GetKey(first.Key)));
            AddTitle(first, entry);
            AddHeadings(first, entry);
            AddItemsToEntry(groups, entry);
            Add(entry);
        }
    }
    
    private static void AddTitle(IGrouping<PluginDescriptor, ProjectDescriptor> group, XElement entry)
    {
        var pd = group.Key;

        var titleElement = new XElement("div");
        
        if (!string.IsNullOrWhiteSpace(pd.Manufacturer))
        {
            var manufacturerElement = new XElement("div", pd.Manufacturer);
            titleElement.Add(manufacturerElement);
        }
        
        var nameElement = new XElement("div",
            new XAttribute("class", "key title"),
            pd.Name);
        titleElement.Add(nameElement);
        
        entry.Add(titleElement);
    }
    
    private void AddHeadings(IGrouping<PluginDescriptor, ProjectDescriptor> group, XElement entry)
    {
        var linkToTop = new LinkToTop();
        entry.Add(linkToTop);
        var linkToSection = new LinkToSection(ParentSection.Id);
        entry.Add(linkToSection);
    }

    private static void AddItemsToEntry(IEnumerable<IGrouping<PluginDescriptor, ProjectDescriptor>> groups, XElement entry)
    {
        var pairs = groups
            .SelectMany(g => g.Select(h => new { ProjectDescriptor = h, PluginDescriptor = g.Key }));
        var lookup = pairs.ToLookup(p => p.ProjectDescriptor, p => p.PluginDescriptor);
        var projectDescriptors = lookup.Select(g => g.Key);
        entry.Add(projectDescriptors.ToXElements(lookup));
    }

    private static string GetKey(PluginDescriptor entry)
    {
        return entry.FullName;
    }
}