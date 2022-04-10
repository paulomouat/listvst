namespace ListVst.OutputFormatting.HtmlFile;

public class PluginSection : Section
{
    public PluginSection(string id = "listing-by-plugin", string title = "Listing by plugin")
        : base(id, title)
    { }

    public override void Populate(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.Name)
            .ThenBy(pd => pd.ProjectDescriptor.Path)
            .ToLookup(e => e.Name, e => e.ProjectDescriptor.Path);

        base.Populate(lookup);
    }
}