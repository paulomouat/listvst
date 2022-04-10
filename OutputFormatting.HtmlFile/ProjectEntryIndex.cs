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
}