namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryIndex : EntryIndex<ProjectDescriptor, PluginDescriptor>
{
    public ProjectEntryIndex(string id, string title, ISection parentSection)
        : base(id, title, parentSection)
    { }

    protected override string GetKey(ProjectDescriptor entry)
    {
        return entry.Path;
    }

    protected override void AddItems(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var descriptors = lookup
            .Select(g => g.Key)
            .ToList();

        Add(descriptors.ToXElements());
    }
}