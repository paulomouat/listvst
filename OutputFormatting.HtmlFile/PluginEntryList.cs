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

        var entries = regrouped.Select(groups =>
        {
            var first = groups.First();
            var pd = first.Key;
            var entry = pd.ToEntry();
            var title = pd.ToPluginEntryTitle();
            entry.Add(title);
            AddHeadings(entry);
            AddItemsToEntry(groups, entry);
            return entry;
        });

        Add(entries);
    }
    
    private void AddHeadings(XElement entry)
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
}