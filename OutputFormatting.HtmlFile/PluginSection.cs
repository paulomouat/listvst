namespace ListVst.OutputFormatting.HtmlFile;

public class PluginSection : Section
{
    public PluginSection(string id = "listing-by-plugin", string title = "Listing by plugin")
        : base(id, title)
    { }

    public override void Add(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.Name)
            .ThenBy(pd => pd.ProjectDescriptor.Path)
            .ToLookup(e => e.Name, e => e);

        base.Add(lookup);
    }

    protected override EntryList BuildEntryList(ILookup<string, PluginDescriptor> lookup)
    {
        var list = new PluginEntryList(Id + "-entries");
        list.Add(lookup);
        return list;
    }
}