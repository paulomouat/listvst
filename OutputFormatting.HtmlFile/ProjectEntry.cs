namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntry : Entry
{
    private ProjectDescriptor ProjectDescriptor { get; }

    public ProjectEntry(ProjectDescriptor projectDescriptor)
        : base(new Id(projectDescriptor.Path))
    {
        ProjectDescriptor = projectDescriptor;
    }

    public ProjectEntry WithTitle()
    {
        Add(ProjectDescriptor.ToEntryTitle());
        return this;
    }
    
    public ProjectEntry WithHeadings(string parentSectionId)
    {
        var linkToTop = new LinkToTop();
        Add(linkToTop);
        var linkToSection = new LinkToSection(parentSectionId);
        Add(linkToSection);
        return this;
    }

    public ProjectEntry WithItems(IEnumerable<PluginDescriptor> pluginDescriptors)
    {
        Add(pluginDescriptors.ToXElements());
        return this;
    }
}