using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public static class ProjectDescriptorExtensions
{
    public static Entry ToEntry(this ProjectDescriptor projectDescriptor)
    {
        var id = new Id(projectDescriptor.Path);
        var entry = new Entry(id);
        return entry;
    }
    
    public static XElement ToProjectEntryTitle(this ProjectDescriptor projectDescriptor)
    {
        var titleElement = new XElement("div");
        
        if (!string.IsNullOrWhiteSpace(projectDescriptor.SpecialFolder))
        {
            var specialFolderElement = new XElement("div", projectDescriptor.SpecialFolder);
            titleElement.Add(specialFolderElement);
        }
        
        var projectNameElement = new XElement("div",
            new XAttribute("class", "key title"),
            projectDescriptor.Name);
        titleElement.Add(projectNameElement);
        
        var pathElement = new XElement("div", string.Join(" / ", projectDescriptor.Subsegments));
        titleElement.Add(pathElement);

        return titleElement;
    }
    
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

    public static XElement ToXElement(this ProjectDescriptor projectDescriptor, IEnumerable<PluginDescriptor> pluginDescriptors)
    {
        var entry = new XElement("div", new XAttribute("class", "item"));
        var itemKey = projectDescriptor.Path;
        var itemName = string.Join(" / ", projectDescriptor.Subsegments);
        AddItemToEntry(itemKey, itemName, entry);
        var itemTypes = pluginDescriptors.Select(i => i.PluginType).OrderBy(i => i).Distinct();
        AddTypesToEntry(itemTypes, entry);
        return entry;
    }
    
    private static void AddItemToEntry(string itemKey, string itemName, XElement entry)
    {
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(itemKey)), itemName);
        entry.Add(anchor);
    }

    private static void AddTypesToEntry(IEnumerable<PluginType> itemTypes, XElement entry)
    {
        foreach (var itemType in itemTypes)
        {
            AddTypeToEntry(itemType, entry);
        }
    }
    
    private static void AddTypeToEntry(PluginType itemType, XElement entry)
    {
        var typeAbbreviation = "?";
        var typeClass = itemType.ToString().ToLower();
        switch (typeClass)
        {
            case "audiounit":
                typeAbbreviation = "AU";
                break;
            case "vst":
                typeAbbreviation = "VST";
                break;
            case "vst3":
                typeAbbreviation = "VST3";
                break;
        }
        var span = new XElement("span", new XAttribute("class", "plugintype"), typeAbbreviation);
        entry.Add(span);
    }}