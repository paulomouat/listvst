namespace ListVst.OutputFormatting.HtmlFile;

public class PluginEntryIndex(string id, string title, ISection parentSection) : EntryIndex(id, title, parentSection)
{
    public virtual void AddPluginRecords(IEnumerable<PluginRecord> data)
    {
        var lookup = data
            .OrderBy(pair => pair.PluginDescriptor.FullName)
            .ThenBy(pair => pair.ProjectDescriptor.Path)
            .ToLookup(pair => pair.PluginDescriptor, pair => pair.ProjectDescriptor);
        AddHeadings(lookup);
        AddItems(lookup);        
    }
    
    private void AddHeadings(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var linkToTop = new LinkToTop();
        Add(linkToTop);
        var stats = new Stats(lookup.Count);
        Add(stats);
    }

    private void AddItems(ILookup<PluginDescriptor, ProjectDescriptor> lookup)
    {
        var descriptors = lookup
            .Select(g => g.Key)
            .ToList();

        var indexListing = new IndexListing();
        Add(indexListing);
        indexListing.Add(descriptors.ToXElements());
    }
}