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