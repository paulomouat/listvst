using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryTitle(PluginDescriptor pluginDescriptor) : EntryTitle
{
    private PluginDescriptor PluginDescriptor { get; } = pluginDescriptor;

    public PluginEntryTitle WithManufacturer()
    {
        var manufacturer = PluginDescriptor.Manufacturer;
        if (!string.IsNullOrWhiteSpace(manufacturer))
        {
            var manufacturerElement = new XElement("div", manufacturer);
            Add(manufacturerElement);
        }

        return this;
    }

    public PluginEntryTitle WithName()
    {
        var nameElement = new XElement("div",
            new XAttribute("class", "key title"),
            PluginDescriptor.Name);
        Add(nameElement);
        return this;
    }
}