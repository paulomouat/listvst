namespace ListVst.OutputFormatting.HtmlFile;

public class PluginSection : Section<PluginDescriptor, ProjectDescriptor>
{
    public PluginSection(string id = "listing-by-plugin", string title = "Listing by plugin")
        : base(id, title)
    { }

    public override void Add(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.PluginDescriptor.FullName)
            .ThenBy(pair => pair.ProjectDescriptor.Path)
            .ToLookup(pair => pair.PluginDescriptor, pair => pair.ProjectDescriptor);

        base.Add(lookup);
    }

    protected override IEntryIndex BuildEntryIndex(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var index = new PluginEntryIndex(Id + "-index", "All entries", this);
        index.AddFromLookup(lookup);
        return index;
    }
    
    protected override IEntryList BuildEntryList(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var list = new PluginEntryList(Id + "-entries", this);
        list.AddFromLookup(lookup);
        return list;
    }
}