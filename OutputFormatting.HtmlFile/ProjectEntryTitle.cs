using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryTitle : EntryTitle
{
    private ProjectDescriptor ProjectDescriptor { get; }

    public ProjectEntryTitle(ProjectDescriptor projectDescriptor)
    {
        ProjectDescriptor = projectDescriptor;
    }
    
    public ProjectEntryTitle WithSpecialFolder()
    {
        var specialFolder = ProjectDescriptor.SpecialFolder;
        if (!string.IsNullOrWhiteSpace(specialFolder))
        {
            var specialFolderElement = new XElement("div", specialFolder);
            Add(specialFolderElement);
        }

        return this;
    }

    public ProjectEntryTitle WithName()
    {
        var projectNameElement = new XElement("div",
            new XAttribute("class", "key title"),
            ProjectDescriptor.Name);
        Add(projectNameElement);
        return this;
    }

    public ProjectEntryTitle WithPath()
    {
        var pathElement = new XElement("div", string.Join(" / ", ProjectDescriptor.Subsegments));
        Add(pathElement);
        return this;
    }
}