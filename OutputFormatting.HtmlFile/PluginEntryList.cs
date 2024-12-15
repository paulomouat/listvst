namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryList(string id, ISection parentSection) : EntryList(id, parentSection)
{
    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pr => pr.PluginDescriptor.FullName)
            .ThenBy(pr => pr.ProjectDescriptor.Path)
            .ToLookup(pr => pr.PluginDescriptor, pr => pr.ProjectDescriptor);
        
        AddFromLookup(lookup);
    }
    
    private void AddFromLookup(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var regrouped = lookup.ToLookup(k => k.Key.FullName,
            g => g);
        
        var entries = regrouped
            .Select(groups => (PluginDescriptor: groups.First().Key, ProjectsByPlugin: groups))
            .Select(e =>
                e.PluginDescriptor.ToEntry()
                    .WithTitle()
                    .WithHeadings(ParentSection.Id)
                    .WithItems(e.ProjectsByPlugin)
            );

        Add(entries);
    }
}