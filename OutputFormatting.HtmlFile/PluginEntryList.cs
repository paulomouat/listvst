using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryList : EntryList<PluginDescriptor, ProjectDescriptor>
{
    public PluginEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }

    public override void AddTitle(IGrouping<PluginDescriptor, ProjectDescriptor> group, XElement entry)
    {
        var pd = group.Key;

        var titleElement = new XElement("div");
        
        if (!string.IsNullOrWhiteSpace(pd.Manufacturer))
        {
            var manufacturerElement = new XElement("div", pd.Manufacturer);
            titleElement.Add(manufacturerElement);
        }
        
        var nameElement = new XElement("div",
            new XAttribute("class", "key title"),
            pd.Name);
        titleElement.Add(nameElement);
        
        entry.Add(titleElement);
    }

    public override void AddItemsToEntry(IEnumerable<ProjectDescriptor> projectDescriptors, XElement entry)
    {
        entry.Add(projectDescriptors.ToXElements());
    }

    protected override string GetKey(PluginDescriptor entry)
    {
        return entry.FullName;
    }
}