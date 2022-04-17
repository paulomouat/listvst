using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryList : EntryList<ProjectDescriptor, PluginDescriptor>
{
    public ProjectEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }

    public override void AddTitle(IGrouping<ProjectDescriptor, PluginDescriptor> group, XElement entry)
    {
        var pd = group.Key;

        var titleElement = new XElement("div");
        
        if (!string.IsNullOrWhiteSpace(pd.SpecialFolder))
        {
            var specialFolderElement = new XElement("div", pd.SpecialFolder);
            titleElement.Add(specialFolderElement);
        }
        
        var projectNameElement = new XElement("div",
            new XAttribute("class", "key title"),
            pd.Name);
        titleElement.Add(projectNameElement);
        
        var pathElement = new XElement("div", string.Join(" / ", pd.Subsegments));
        titleElement.Add(pathElement);
        
        entry.Add(titleElement);
    }
    
    public override void AddItemsToEntry(IEnumerable<PluginDescriptor> pluginDescriptors, XElement entry)
    {
        entry.Add(pluginDescriptors.ToXElements());
    }

    protected override string GetKey(ProjectDescriptor entry)
    {
        return entry.Path;
    }
}