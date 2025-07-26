using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public static class ProjectDescriptorExtensions
{
    public static ProjectEntry ToEntry(this ProjectDescriptor projectDescriptor) => new(projectDescriptor);

    public static ProjectEntryTitle ToEntryTitle(this ProjectDescriptor projectDescriptor) =>
        new ProjectEntryTitle(projectDescriptor)
            .WithSpecialFolder()
            .WithName()
            .WithPath();
    
    public static IEnumerable<XElement> ToXElements(this IEnumerable<ProjectDescriptor> descriptors, ILookup<ProjectDescriptor, PluginDescriptor> pluginsByProject)
    {
        var results = new List<XElement>();
        
        var categories = descriptors
            .ToLookup(pd => pd.SpecialFolder)
            .ToLookup(g => g.Key, g => g.ToLookup(pd => pd.Name));

        foreach (var category in categories)
        {
            XElement? categoryElement = null;
            var categoryKey = category.Key;
            if (!string.IsNullOrWhiteSpace(categoryKey))
            {
                categoryElement = new XElement("div",
                    new XAttribute("class", "item-category"),
                    new XElement("div", new XAttribute("class", "item-category-title"), category.Key));
                results.Add(categoryElement);
            }

            foreach (var projectFoldersInCategory in category)
            {
                foreach (var projectsInFolder in projectFoldersInCategory)
                {
                    var projectsInFolderElement = new XElement("div", new XAttribute("class", "item-container"),
                        new XElement("div", new XAttribute("class", "item-container-title"), projectsInFolder.Key));

                    if (categoryElement == null)
                    {
                        results.Add(projectsInFolderElement);
                    }
                    else
                    {
                        categoryElement.Add(projectsInFolderElement);
                    }

                    foreach (var pd in projectsInFolder)
                    {
                        var plugins = pluginsByProject[pd];
                        var entry = pd.ToXElement(plugins);
                        projectsInFolderElement.Add(entry);
                    }
                }
            }
        }

        return results;
    }

    private static XElement ToXElement(this ProjectDescriptor projectDescriptor, IEnumerable<PluginDescriptor> pluginDescriptors)
    {
        var entry = new XElement("div", new XAttribute("class", "item"));
        var itemKey = projectDescriptor.Path;
        var itemName = string.Join(" / ", projectDescriptor.Subsegments);
        AddItemToEntry(itemKey, itemName, entry);
        AddProjectTypeToEntry(projectDescriptor.ProjectType, entry);
        var itemPluginTypes = pluginDescriptors.Select(i => i.PluginType).OrderBy(i => i).Distinct();
        AddPluginTypesToEntry(itemPluginTypes, entry);
        return entry;
    }
    
    private static void AddItemToEntry(string itemKey, string itemName, XElement entry)
    {
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(itemKey)), itemName);
        entry.Add(anchor);
    }

    private static void AddPluginTypesToEntry(IEnumerable<PluginType> itemTypes, XElement entry)
    {
        foreach (var itemType in itemTypes)
        {
            AddPluginTypeToEntry(itemType, entry);
        }
    }
    
    private static void AddPluginTypeToEntry(PluginType itemType, XElement entry)
    {
        var span = new XElement("span", new XAttribute("class", "plugintype"), itemType.Designation);
        entry.Add(span);
    }
    
    private static void AddProjectTypeToEntry(ProjectType itemType, XElement entry)
    {
        var span = new XElement("span", new XAttribute("class", "projecttype"), itemType.Designation);
        entry.Add(span);
    }    
}