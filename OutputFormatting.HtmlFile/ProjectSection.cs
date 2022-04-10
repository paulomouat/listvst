namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectSection : Section
{
    public ProjectSection(string id = "listing-by-path", string title = "Listing by project path")
        : base(id, title)
    { }

    public override void Add(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.ProjectDescriptor.Path)
            .ThenBy(pd => pd.Name)
            .ToLookup(e => e.ProjectDescriptor.Path, e => e);

        base.Add(lookup);
    }

    protected override EntryList BuildEntryList(ILookup<string, PluginDescriptor> lookup)
    {
        var list = new ProjectEntryList(Id + "-entries", this);
        list.Add(lookup);
        return list;
    }
}