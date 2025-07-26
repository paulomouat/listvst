using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryTitle(ProjectDescriptor projectDescriptor) : EntryTitle
{
    private ProjectDescriptor ProjectDescriptor { get; } = projectDescriptor;

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
        AddProjectTypeToPath(ProjectDescriptor.ProjectType, pathElement);
        Add(pathElement);
        return this;
    }
    
    private static void AddProjectTypeToPath(ProjectType itemType, XElement pathElement)
    {
        var span = new XElement("span", new XAttribute("class", "projecttype"), itemType.Designation);
        pathElement.Add(span);
    }    
}