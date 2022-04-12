namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectSection : Section<ProjectDescriptor, PluginDescriptor>
{
    public ProjectSection(string id = "listing-by-path", string title = "Listing by project path")
        : base(id, title)
    { }

    public override void Add(IEnumerable<PluginData> data)
    {
        var lookup = data
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.FullName)
            .ToLookup(pair => pair.ProjectDescriptor, pair => pair.PluginDescriptor);

        base.Add(lookup);
    }

    protected override IEntryIndex BuildEntryIndex(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var index = new ProjectEntryIndex(Id + "-index", "All entries", this);
        index.Add(lookup);
        return index;
    }
    
    protected override IEntryList BuildEntryList(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var list = new ProjectEntryList(Id + "-entries", this);
        list.Add(lookup);
        return list;
    }
}