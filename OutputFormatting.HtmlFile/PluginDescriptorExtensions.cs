using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public static class PluginDescriptorExtensions
{
    public static IEnumerable<XElement> ToXElements(this IEnumerable<PluginDescriptor> descriptors)
    {
        var results = new List<XElement>();
        
        var categories = descriptors
            .ToLookup(pd => pd.Manufacturer);

        foreach (var category in categories)
        {
            var categoryElement = new XElement("div",
                new XAttribute("class", "item-category"),
                new XElement("div", category.Key));
            results.Add(categoryElement);

            foreach (var pd in category)
            {
                var entry = new XElement("div", new XAttribute("class", "item"));
                var itemKey = pd.FullName;
                var itemName = pd.Name;
                AddItemToEntry(itemKey, itemName, entry);
                categoryElement.Add(entry);
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