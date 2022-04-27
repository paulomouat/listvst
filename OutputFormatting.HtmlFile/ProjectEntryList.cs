using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryList : EntryList
{
    public ProjectEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }

    
    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.FullName)
            .ToLookup(pair => pair.ProjectDescriptor, pair => pair.PluginDescriptor);
        AddFromLookup(lookup);
    }

    private void AddFromLookup(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var entries = lookup.Select(group =>
        {
            var pd = group.Key;
            var entry = pd.ToEntry();
            var title = pd.ToProjectEntryTitle();
            entry.Add(title);
            AddHeadings(group, entry);
            AddItemsToEntry(group, entry);
            return entry;
        });

        Add(entries);
    }
    
    private void AddHeadings(IGrouping<ProjectDescriptor, PluginDescriptor> group, XElement entry)
    {
        var linkToTop = new LinkToTop();
        entry.Add(linkToTop);
        var linkToSection = new LinkToSection(ParentSection.Id);
        entry.Add(linkToSection);
    }
    
    private static void AddItemsToEntry(IEnumerable<PluginDescriptor> pluginDescriptors, XElement entry)
    {
        entry.Add(pluginDescriptors.ToXElements());
    }
}