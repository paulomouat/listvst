namespace ListVst.OutputFormatting.HtmlFile;

public class ProjectEntryIndex(string id, string title, ISection parentSection) : EntryIndex(id, title, parentSection)
{
    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.ProjectDescriptor.Path)
            .ThenBy(pair => pair.PluginDescriptor.FullName)
            .ToLookup(pair => pair.ProjectDescriptor, pair => pair.PluginDescriptor);
        AddHeadings(lookup);
        AddItems(lookup);       
    }
    
    private void AddHeadings(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var linkToTop = new LinkToTop();
        Add(linkToTop);
        var stats = new Stats(lookup.Count);
        Add(stats);
    }
    
    private void AddItems(ILookup<ProjectDescriptor, PluginDescriptor> lookup)
    {
        var descriptors = lookup
            .Select(g => g.Key)
            .ToList();

        var indexListing = new IndexListing();
        Add(indexListing);
        indexListing.Add(descriptors.ToXElements(lookup));
    }
}