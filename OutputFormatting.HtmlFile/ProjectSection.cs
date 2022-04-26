namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectSection : Section<ProjectDescriptor, PluginDescriptor>
{
    public ProjectSection(string id = "listing-by-path", string title = "Listing by project path")
        : base(id, title)
    { }

    public override void Add(IEnumerable<PluginRecord> data)
    {
        base.Add(data);
        
        var lookup = data
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.FullName)
            .ToLookup(pair => pair.ProjectDescriptor, pair => pair.PluginDescriptor);

        base.Add(lookup);
    }

    protected override IEntryIndex BuildEntryIndex(IEnumerable<PluginRecord> data)
    {
        var index = new ProjectEntryIndex(Id + "-index", "All entries", this);
        index.AddPluginRecords(data);
        return index;
    }
    
    protected override IEntryList BuildEntryList(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var list = new ProjectEntryList(Id + "-entries", this);
        list.AddFromLookup(lookup);
        return list;
    }
}