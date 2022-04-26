namespace ListVst.OutputFormatting.HtmlFile;

public class PluginSection : Section<PluginDescriptor, ProjectDescriptor>
{
    public PluginSection(string id = "listing-by-plugin", string title = "Listing by plugin")
        : base(id, title)
    { }

    public override void Add(IEnumerable<PluginRecord> data)
    {
        base.Add(data);

        var lookup = data
            .OrderBy(pair => pair.PluginDescriptor.FullName)
            .ThenBy(pair => pair.ProjectDescriptor.Path)
            .ToLookup(pair => pair.PluginDescriptor, pair => pair.ProjectDescriptor);

        base.Add(lookup);
    }

    protected override IEntryIndex BuildEntryIndex(IEnumerable<PluginRecord> data)
    {
        var index = new PluginEntryIndex(Id + "-index", "All entries", this);
        index.AddPluginRecords(data);
        return index;
    }
    
    protected override IEntryList BuildEntryList(IEnumerable<PluginRecord> data)
    {
        var list = new PluginEntryList(Id + "-entries", this);
        list.AddPluginRecords(data);
        return list;
    }
}