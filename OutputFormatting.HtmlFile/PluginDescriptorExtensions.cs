using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public static class PluginDescriptorExtensions
{
    public static PluginEntry ToEntry(this PluginDescriptor pluginDescriptor) => new(pluginDescriptor);

    public static XElement ToEntryTitle(this PluginDescriptor pluginDescriptor) =>
        new PluginEntryTitle(pluginDescriptor)
            .WithManufacturer()
            .WithName();
    
    public static IEnumerable<XElement> ToXElements(this IEnumerable<PluginDescriptor> descriptors)
    {
        var results = new List<XElement>();

        var manufacturers = descriptors
            .ToLookup(pd => pd.FullName)    
            .ToLookup(g => g.Select(pd => pd.Manufacturer).First(),
                g => g.Distinct().ToList());

        foreach (var manufacturer in manufacturers)
        {
            var categoryElement = new XElement("div",
                new XAttribute("class", "item-container"),
                new XElement("div", new XAttribute("class", "item-container-title"), manufacturer.Key));
            results.Add(categoryElement);

            foreach (var pds in manufacturer)
            {
                var pd = pds.First();
                var entry = new XElement("div", new XAttribute("class", "item"));
                var itemKey = pd.FullName;
                var itemName = pd.Name;
                AddItemToEntry(itemKey, itemName, entry);
                var itemTypes = pds.Select(i => i.PluginType).OrderBy(i => i);
                AddTypesToEntry(itemTypes, entry);
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

    private static void AddTypesToEntry(IEnumerable<PluginType> itemTypes, XElement entry)
    {
        foreach (var itemType in itemTypes)
        {
            AddTypeToEntry(itemType, entry);
        }
    }
    
    private static void AddTypeToEntry(PluginType itemType, XElement entry)
    {
        var span = new XElement("span", new XAttribute("class", "plugintype"), itemType.Designation);
        entry.Add(span);
    }
}