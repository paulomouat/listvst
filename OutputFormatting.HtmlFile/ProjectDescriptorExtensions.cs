using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public static class ProjectDescriptorExtensions
{
    public static IEnumerable<XElement> ToXElements(this IEnumerable<ProjectDescriptor> descriptors)
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
                    new XElement("div", category.Key));
                results.Add(categoryElement);
            }

            foreach (var projectFoldersInCategory in category)
            {
                foreach (var projectsInFolder in projectFoldersInCategory)
                {
                    var projectsInFolderElement = new XElement("div", new XAttribute("class", "item-container"),
                        new XElement("div", projectsInFolder.Key));

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
                        var entry = new XElement("div", new XAttribute("class", "item"));
                        var itemKey = pd.Path;
                        var itemName = string.Join(" / ", pd.Subsegments);
                        AddItemToEntry(itemKey, itemName, entry);
                        projectsInFolderElement.Add(entry);
                    }
                }
            }
        }

        return results;
    }
    
    private static void AddItemToEntry(string itemKey, string itemName, XElement entry)
    {
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(itemKey)), itemName);
        entry.Add(anchor);
    }
}