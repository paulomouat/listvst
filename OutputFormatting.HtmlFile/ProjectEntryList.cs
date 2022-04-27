using System.Xml.Linq;

namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryList : EntryList
{
    public ProjectEntryList(string id, ISection parentSection)
        : base(id, parentSection)
    { }

    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.FullName)
            .ToLookup(pair => pair.ProjectDescriptor, pair => pair.PluginDescriptor);
        AddFromLookup(lookup);
    }
    
    private XElement BuildEntry(IGrouping<ProjectDescriptor, PluginDescriptor> group)
    {
        var key = GetKey(group.Key);
        var entryId = new Id(key).Value;
            
        var entry = new XElement("div",
            new XAttribute("id", entryId),
            new XAttribute("class", "entry"));
            
        return entry;
    }

    private void AddFromLookup(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        foreach(var group in lookup)
        {
            var entry = BuildEntry(group);
            AddTitle(group, entry);
            AddHeadings(group, entry);
            AddItemsToEntry(group, entry);
            Add(entry);
        }
    }
    
    private void AddTitle(IGrouping<ProjectDescriptor, PluginDescriptor> group, XElement entry)
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
    
    private void AddHeadings(IGrouping<ProjectDescriptor, PluginDescriptor> group, XElement entry)
    {
        var linkToTop = new LinkToTop();
        entry.Add(linkToTop);
        var linkToSection = new LinkToSection(ParentSection.Id);
        entry.Add(linkToSection);
    }
    
    private void AddItemsToEntry(IEnumerable<PluginDescriptor> pluginDescriptors, XElement entry)
    {
        entry.Add(pluginDescriptors.ToXElements());
    }

    private string GetKey(ProjectDescriptor entry)
    {
        return entry.Path;
    }
}