namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntry : Entry
{
    private PluginDescriptor PluginDescriptor { get; }

    public PluginEntry(PluginDescriptor pluginDescriptor)
        : base(new Id(pluginDescriptor.FullName))
    {
        PluginDescriptor = pluginDescriptor;
    }

    public PluginEntry WithTitle()
    {
        Add(PluginDescriptor.ToEntryTitle());
        return this;
    }
    
    public PluginEntry WithHeadings(string parentSectionId)
    {
        var linkToTop = new LinkToTop();
        Add(linkToTop);
        var linkToSection = new LinkToSection(parentSectionId);
        Add(linkToSection);
        return this;
    }

    public PluginEntry WithItems(IEnumerable<IGrouping<PluginDescriptor, ProjectDescriptor>> groups)
    {
        var pairs = groups
            .SelectMany(g => g.Select(h => new { ProjectDescriptor = h, PluginDescriptor = g.Key }));
        var lookup = pairs.ToLookup(p => p.ProjectDescriptor, p => p.PluginDescriptor);
        var projectDescriptors = lookup.Select(g => g.Key);
        Add(projectDescriptors.ToXElements(lookup));
        return this;
    }
}