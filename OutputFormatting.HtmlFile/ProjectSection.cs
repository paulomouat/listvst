namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectSection : Section
{
    public ProjectSection(string id = "listing-by-path", string title = "Listing by project path")
        : base(id, title)
    { }

    public override void Populate(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.ProjectDescriptor.Path)
            .ThenBy(pd => pd.Name)
            .ToLookup(e => e.ProjectDescriptor.Path, e => e.Name);

        base.Populate(lookup);
    }
}