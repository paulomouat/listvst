using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryList : EntryList<PluginDescriptor, ProjectDescriptor>
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
            var entry = BuildEntry(first);
            AddTitle(first, entry);
            AddHeadings(first, entry);
            AddItemsToEntry(groups, entry);
            Add(entry);
        }
    }
    
    public override void AddTitle(IGrouping<PluginDescriptor, ProjectDescriptor> group, XElement entry)
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

    public virtual void AddItemsToEntry(IEnumerable<IGrouping<PluginDescriptor, ProjectDescriptor>> groups, XElement entry)
    {
        var pairs = groups
            .SelectMany(g => g.Select(h => new { ProjectDescriptor = h, PluginDescriptor = g.Key }));
        var lookup = pairs.ToLookup(p => p.ProjectDescriptor, p => p.PluginDescriptor);
        var projectDescriptors = lookup.Select(g => g.Key);
        entry.Add(projectDescriptors.ToXElements(lookup));
    }

    protected override string GetKey(PluginDescriptor entry)
    {
        return entry.FullName;
    }
}