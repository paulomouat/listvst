using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginSection : Section
{
    public PluginSection(string id = "listing-by-plugin", string title = "Listing by plugin")
        : base(id, title)
    { }

    public override void Add(IEnumerable<PluginDescriptor> details)
    {
        var lookup = details
            .OrderBy(pd => pd.Name)
            .ThenBy(pd => pd.ProjectDescriptor.Path)
            .ToLookup(e => e.Name, e => e);

        base.Add(lookup);
    }

    protected override XElement ToItemElement(PluginDescriptor pluginDescriptor)
    {
        var item = pluginDescriptor.ProjectDescriptor.Path;
        var anchor = new XElement("a", new XAttribute("href", "#" + new Id(item)), item);
        var element = new XElement("div", anchor);
        element.SetAttributeValue("class", "item");
        return element;
    }}