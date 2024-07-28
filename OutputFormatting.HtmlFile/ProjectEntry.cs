namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntry(ProjectDescriptor projectDescriptor) : Entry(new Id(projectDescriptor.Path))
{
    private ProjectDescriptor ProjectDescriptor { get; } = projectDescriptor;

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