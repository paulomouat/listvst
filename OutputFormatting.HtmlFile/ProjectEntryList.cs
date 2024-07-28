namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryList(string id, ISection parentSection) : EntryList(id, parentSection)
{
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
        var entries = lookup
            .Select(group => (ProjectDescriptor: group.Key, PluginDescriptors: group))
            .Select(e =>
                e.ProjectDescriptor.ToEntry()
                    .WithTitle()
                    .WithHeadings(ParentSection.Id)
                    .WithItems(e.PluginDescriptors)
            );

        Add(entries);
    }
}